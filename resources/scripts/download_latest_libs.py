# (c) 2023 Copyright, Real-Time Innovations, Inc.  All rights reserved. RTI
# grants Licensee a license to use, modify, compile, and create derivative
# works of the Software.  Licensee has the right to distribute object form only
# for use with RTI products.  The Software is provided "as is", with no
# warranty of any type, including any warranty for fitness for any purpose. RTI
# is under no obligation to maintain or support the Software.  RTI shall not be
# liable for any incidental or consequential damages arising out of the use or
# inability to use the software.

import argparse
import io
import logging
import shutil
import sys
import urllib.request
import zipfile
from pathlib import Path
from typing import Any, Dict
from urllib.error import HTTPError

import requests
import yaml


def get_argument_parser():
    """Create argument parser.

    Returns:
        The argument parser.
    """
    parser = argparse.ArgumentParser(
        description=__doc__,
        allow_abbrev=False,
        formatter_class=argparse.RawDescriptionHelpFormatter,
    )
    parser.add_argument(
        "-o",
        "--output-path",
        type=Path,
        required=True,
        metavar="path",
        dest="output_path",
        help="Output zip path.",
    )
    parser.add_argument(
        "--storage-url",
        required=True,
        dest="storage_url",
        help="URL to storage.",
    )
    parser.add_argument(
        "--storage-path",
        required=True,
        dest="storage_path",
        help="Path from the storage from where libs will be downloaded.",
    )

    return parser


def create_folder_structure(data: Dict[str, Any], output_path: Path):
    """Create the folder structure to store the libraries that are going to be downloaded.

    Args:
        data (Dict[str, Any]): Information read from the YAML file.
        output_path (Path): Where to create the folder structure to store the libs.
    """
    lib_dir = output_path.joinpath("lib")
    lib_dir.mkdir(exist_ok=True, parents=True)

    for arch in data["architectures"].keys():
        arch_dir = lib_dir.joinpath(arch)
        arch_dir.mkdir(exist_ok=True, parents=True)


def get_latest_bundles_version(latest_folder: str):
    """Gets the latest bundles' version from the folder name.

    Args:
        latest_folder (str): Name of the latest folder.

    Returns:
        The version number of the latest bundles.
    """
    version = latest_folder.split("_")[1]

    return version[0 : version.rfind(".")]


def get_latest_subfolder(storage_url: str, storage_path: str):
    """Gets the latest subdirectory from the given folder.

    Args:
        storage_url (str): URL to the storage.
        storage_path (str): Path from which the latest subdirectory will be extracted.

    Returns:
        The latest subdirectory from the given `storage_path`.
    """
    url = f"{storage_url}/api/storage/{storage_path}"
    response = requests.get(url)
    folder = None

    if response.status_code == 200:
        data = response.json()
        files = data["children"]

        # Filter out files and get only folders
        folders = [f["uri"] for f in files if f["folder"]]

        folder = folders[-1].replace("/", "")

    return folder


def retrieve_latest_libraries(
    data: Dict[str, Any], storage_url: str, libs_dir: str, version: str, output_path: Path
):
    """Download latest libraries from storage.

    Args:
        data (Dict[str, Any]): Information read from the YAML file.
        storage_url (str): URL to the storage.
        libs_dir (str): Directory where latest libraries are stored.
        version (str): Version number of the latest bundles.
        output_path (Path): Where to download the libs.
    """
    logging.info("Attempting to retrieve Connext native libraries ...")

    for arch, arch_data in data["architectures"].items():
        for lib in arch_data["libs"]:
            outer_path = (
                f"{storage_url}/{libs_dir}/staging/connextdds-staging-{arch_data['name']}.tgz"
            )

            if "dds" in lib:
                inner_path = f"licensed/rti_connext_dds-{version}/lib/{arch_data['name']}/{lib}"
            # If it's not one of our libs, try to find it in the resource folder (like "vcruntime140.dll")
            else:
                inner_path = (
                    f"licensed/rti_connext_dds-{version}/resource/app/lib/{arch_data['name']}/{lib}"
                )

            logging.info(f" - Retrieving {lib} ({arch_data['name']}) ...")

            try:
                urllib.request.urlretrieve(
                    f"{outer_path}!/{inner_path}", output_path.joinpath("lib", arch, lib)
                )
            except HTTPError:
                logging.error(
                    f"[Error] Could not find bundles for architecture {arch_data['name']}"
                )
                raise

    logging.info("All libraries were retrieved successfully!")


def retrieve_libraries_for_specific_version(
    data: Dict[str, Any], storage_url: str, storage_path: str, version: str, output_path: Path
):
    """Download libraries for a specific version from the storage.

    Args:
        data (Dict[str, Any]): Information read from the YAML file.
        storage_url (str): URL to the storage.
        storage_path (str): Path where the Connext Packages are stored.
        version (str): Version number of the latest bundles.
        output_path (Path): Where to download the libs.
    """
    logging.info("Attempting to retrieve Connext native libraries ...")
    url = f"{storage_url}/{storage_path}/{version}"

    for arch, arch_data in data["architectures"].items():
        logging.info(f" - Downloading minimal package for {arch_data['name']} ...")

        response = urllib.request.urlopen(
            f"{url}/rti_connext_dds-{version}-min-{arch_data['name']}.zip"
        )
        compressed_file = io.BytesIO(response.read())

        logging.info(f"  - Extracting necessary libs...")

        with zipfile.ZipFile(compressed_file, "r") as zip_file:
            for file in arch_data["libs"]:
                logging.info(f"   - Extracting {file} ...")

                if "dds" in file:
                    inner_path = f"rti_connext_dds-{version}/lib/{arch_data['name']}/{file}"
                else:
                    inner_path = f"rti_connext_dds-{version}/resource/app/lib/{arch_data['name']}/{file}"

                try:
                    source = zip_file.open(inner_path)
                except KeyError:
                    logging.error(
                        f" [Error] Could not extract {file} from the compressed file, please "
                        "verify that file exists within it."
                    )
                    raise
                target = open(output_path.joinpath("lib", arch, file), "wb")
                with source, target:
                    shutil.copyfileobj(source, target)


def main():
    args = get_argument_parser().parse_args()
    logging.basicConfig(format="%(message)s", level=logging.INFO)

    with open("config.yaml", "r") as file:
        data = yaml.safe_load(file)

    create_folder_structure(data, args.output_path)

    if data["version"] == "latest":
        latest_folder = get_latest_subfolder(args.storage_url, args.storage_path)

        if not latest_folder:
            sys.exit(
                "Latest directory not found, make sure the URL to the storage and the Path are valid."
            )

        version = get_latest_bundles_version(latest_folder)
        libs_dir = f"{args.storage_path}/{latest_folder}"

        retrieve_latest_libraries(data, args.storage_url, libs_dir, version, args.output_path)
    else:
        version = data["version"]

        retrieve_libraries_for_specific_version(
            data, args.storage_url, args.storage_path, version, args.output_path
        )


if __name__ == "__main__":
    main()
