#!/bin/bash
ROOT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/..
export LD_LIBRARY_PATH=${ROOT_DIR}/../lib/x64Linux2.6gcc4.4.5

NUNIT=`ls ${ROOT_DIR}/testrunner/NUnit.ConsoleRunner.*/tools/nunit3-console.exe`
echo '^RTI.Connector\.[^U][\.A-Za-z0-9`]+$' > $NUNIT.covcfg

# From https://github.com/inorton/XR.Baboon/
echo 'Running covem'
covem $NUNIT "${ROOT_DIR}"/Connector.UnitTests/bin/Debug/net45/librtiddsconnector_dotnet.UnitTests.dll --process:Single >/dev/null 2>&1

echo 'Getting results'
MATCHED=`sqlite3 $NUNIT.covcfg.covdb "select count(hits) from lines;"`
COVERED=`sqlite3 $NUNIT.covcfg.covdb "select count(hits) from lines where hits > 0;"`
echo "Result: $COVERED/$MATCHED ($((COVERED * 100 / MATCHED))%)"
cov-html $NUNIT.covcfg.covdb Connector
rm -rf coverage-report
mv html coverage-report

cov-gtk $NUNIT.covcfg.covdb >/dev/null 2>&1
