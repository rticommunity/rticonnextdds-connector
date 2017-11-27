#!/bin/bash
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

StyleCop.Baboon "${SCRIPT_DIR}"/Settings.StyleCop "${SCRIPT_DIR}"/../Connector "${SCRIPT_DIR}"/../Connector/bin "${SCRIPT_DIR}"/../Connector/obj
if [ $? -ne 0 ] ; then exit 1; fi

StyleCop.Baboon "${SCRIPT_DIR}"/Settings.StyleCop "${SCRIPT_DIR}"/../Connector.UnitTests "${SCRIPT_DIR}"/../Connector.UnitTets/bin "${SCRIPT_DIR}"/../Connector.UnitTest/obj "${SCRIPT_DIR}"/../Connector.UnitTests/TestTypes.cs
if [ $? -ne 0 ] ; then exit 1; fi

gendarme --ignore "${SCRIPT_DIR}"/gendarme.ignore --html "${SCRIPT_DIR}"/gendarme_report.html "${SCRIPT_DIR}"/../Connector/bin/Debug/net35/librtiddsconnector_dotnet.dll
if [ $? -ne 0 ] ; then exit 2; fi
