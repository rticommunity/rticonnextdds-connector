##############################################################################
# Copyright (c) 2005-2015 Real-Time Innovations, Inc. All rights reserved.
# Permission to modify and use for internal purposes granted.
# This software is provided "as is", without warranty, express or implied.
##############################################################################
"""Samples's writer."""

from sys import path as sysPath
from os import path as osPath
from time import sleep
try:
    import rticonnextdds_connector as rti
except ImportError:
    filepath = osPath.dirname(osPath.realpath(__file__))
    sysPath.append(filepath + "/../../../")
    import rticonnextdds_connector as rti

connector = rti.Connector("MyParticipantLibrary::Zero",
                          filepath + "/../ShapeExample.xml")
outputDDS = connector.getOutput("MyPublisher::MySquareWriter")

for i in range(1, 500):
    outputDDS.instance.setNumber("x", i)
    outputDDS.instance.setNumber("y", i*2)
    outputDDS.instance.setNumber("shapesize", 30)
    outputDDS.instance.setString("color", "BLUE")
    outputDDS.write()
    sleep(2)
