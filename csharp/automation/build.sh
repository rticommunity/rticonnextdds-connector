#!/bin/bash
REPO_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/..

command -v dotnet >/dev/null 2>&1 && dotnet restore || nuget restore
msbuild /v:minimal "${SCRIPT_DIR}"/Connector.sln
