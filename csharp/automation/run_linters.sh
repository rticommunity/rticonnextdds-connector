#!/bin/bash
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

msbuild /v:minimal "${SCRIPT_DIR}"/../Connector.sln
if [ $? -ne 0 ] ; then exit 3; fi

StyleCop.Baboon "${SCRIPT_DIR}"/Settings.StyleCop "${SCRIPT_DIR}"/../Connector "${SCRIPT_DIR}"/../Connector/bin "${SCRIPT_DIR}"/../Connector/obj
if [ $? -ne 0 ] ; then exit 1; fi

gendarme --ignore "${SCRIPT_DIR}"/gendarme.ignore --html "${SCRIPT_DIR}"/gendarme_report.html "${SCRIPT_DIR}"/../Connector/bin/Debug/librtiddsconnector_dotnet.dll
if [ $? -ne 0 ] ; then exit 2; fi
