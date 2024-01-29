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
import re
import shutil
import sys
import zipfile
from pathlib import Path, PurePosixPath
from typing import Any, Dict

import boto3
import yaml
from botocore.exceptions import ClientError


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


def get_connext_dds_dir(zip_file: zipfile.ZipFile):
    """Get the name of the Connext DDS directory from inside a ZIP file.

    Args:
        zip_file (zipfile.ZipFile): Opened ZIP file from which the Connext directory name will be obtained.
    Returns:
        The name of the Connext directory inside the ZIP file.
    """
    pattern = r"rti_connext_dds-\d+\.\d+\.\d+"

    for file_info in zip_file.infolist():
        # Extract the directory name from the file path
        directory_name = file_info.filename.split("/")[0]

        # Add the directory name to the set (to ensure uniqueness)
        if re.match(pattern, directory_name):
            return directory_name

    return None


def retrieve_connext_libraries(
    data: Dict[str, Any],
    storage_url: str,
    storage_path: str,
    output_path: Path,
):
    """Download libraries for a specific version from the storage.

    Args:
        data (Dict[str, Any]): Information read from the YAML file.
        storage_url (str): URL to the storage.
        storage_path (str): Path where the Connext Packages are stored.
        version (str): Version number of the latest bundles.
        output_path (Path): Where to download the libs.
    """
    s3 = boto3.client("s3")
    successful = True
    version = data["version"]

    for arch, arch_data in data["architectures"].items():
        logging.info(f" - Downloading minimal package for {arch_data['name']} ...")
        filename = f"rti_connext_dds-{version}-min-{arch_data['name']}.zip"
        remote_file_path = PurePosixPath(
            storage_path,
            version,
            filename,
        )
        download_dest = Path(f"/tmp/connext_downloads/{filename}")
        download_dest.parent.mkdir(parents=True, exist_ok=True)

        try:
            with open(download_dest, "wb") as f:
                s3.download_fileobj(storage_url, str(remote_file_path), f)
        except ClientError:
            logging.error(
                f"[Error] Could not find bundles for architecture {arch_data['name']}"
            )
            successful = False
            continue

        logging.info("  - Extracting necessary libs...")

        with zipfile.ZipFile(download_dest, "r") as zip_file:
            connext_dds_dir = get_connext_dds_dir(zip_file)

            if not connext_dds_dir:
                logging.error(
                    f"[ERROR] Could not find Connext directory inside {filename}"
                )

            for file in arch_data["libs"]:
                logging.info(f"   - Extracting {file} ...")

                if "dds" in file:
                    inner_path = f"{connext_dds_dir}/lib/{arch_data['name']}/{file}"
                else:
                    inner_path = (
                        f"{connext_dds_dir}/resource/app/lib/{arch_data['name']}/{file}"
                    )

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

    return successful


def main():
    args = get_argument_parser().parse_args()
    logging.basicConfig(format="%(message)s", level=logging.INFO)

    with open("config.yaml", "r") as file:
        data = yaml.safe_load(file)

    create_folder_structure(data, args.output_path)

    logging.info("Attempting to retrieve Connext native libraries ...")

    successful = retrieve_connext_libraries(
        data, args.storage_url, args.storage_path, args.output_path
    )

    if not successful:
        logging.error("There were some errors downloading the libs!")
        sys.exit(1)

    logging.info("All libraries were retrieved successfully!")


if __name__ == "__main__":
    main()
