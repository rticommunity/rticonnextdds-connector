"""Samples's writer."""
##############################################################################
# Copyright (c) 2005-2015 Real-Time Innovations, Inc. All rights reserved.
# Permission to modify and use for internal purposes granted.
# This software is provided "as is", without warranty, express or implied.
##############################################################################

from sys import path as sysPath
from os import path as osPath
from time import sleep
import rticonnextdds_connector as rti

filepath = osPath.dirname(osPath.realpath(__file__))
sysPath.append(filepath + "/../../../")

connector = rti.Connector("MyParticipantLibrary::Zero",
                          filepath + "/../Mixed.xml")
outputDDS = connector.getOutput("MyPublisher::MySquareWriter")

for i in range(1, 500):
    """We clear the instance associated to this output
    otherwise the sample will have the values set in the
    previous iteration"""
    outputDDS.clear_members()

    # Here an example on how to set the members of a sequence of complex types
    outputDDS.instance.setNumber("innerStruct[1].x", i)
    outputDDS.instance.setNumber("innerStruct[2].x", i+1)

    # Here an example on how to set a string
    outputDDS.instance.setString("color", "BLUE")

    # Here an example on how to set a number
    outputDDS.instance.setNumber("x", i)

    """Here we are going to set the elements of a sequence.
    - the sequence was declared with maxSize 30
    - we will always set two elements and..
    - ... the third element only half of the time

    If you open rtiddsspy you will see that the length is
    automatically set to the right value."""
    outputDDS.instance.setNumber("aOctetSeq[1]", 42)
    outputDDS.instance.setNumber("aOctetSeq[2]", 43)

    if i % 2 == 0:
        outputDDS.instance.setNumber("aOctetSeq[3]", 44)
    # Now we write the sample
    outputDDS.write()
    sleep(2)
