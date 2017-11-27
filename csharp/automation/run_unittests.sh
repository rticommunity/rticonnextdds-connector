#!/bin/bash
ROOT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/..
export LD_LIBRARY_PATH=${ROOT_DIR}/../lib/x64Linux2.6gcc4.4.5

# First for mono / .net framework
nuget install NUnit.Runners -OutputDirectory "${ROOT_DIR}"/testrunner
mono "${ROOT_DIR}"/testrunner/NUnit.ConsoleRunner.*/tools/nunit3-console.exe "${ROOT_DIR}"/Connector.UnitTests/bin/Debug/net45/librtiddsconnector_dotnet.UnitTests.dll --process:Single
if [ $? -ne 0 ] ; then exit 1; fi

# Then with dotnet
dotnet test --no-build -f netcoreapp2.0 "${ROOT_DIR}"/Connector.UnitTests/Connector.UnitTests.csproj
if [ $? -ne 0 ] ; then exit 2; fi
