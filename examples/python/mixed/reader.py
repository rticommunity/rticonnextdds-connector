##############################################################################
# Copyright (c) 2005-2015 Real-Time Innovations, Inc. All rights reserved.
# Permission to modify and use for internal purposes granted.
# This software is provided "as is", without warranty, express or implied.
##############################################################################
"""Reader without using the wait call."""

from __future__ import print_function
from sys import path as sysPath
from os import path as osPath
from time import sleep
import rticonnextdds_connector as rti

filepath = osPath.dirname(osPath.realpath(__file__))
sysPath.append(filepath + "/../../../")

connector = rti.Connector("MyParticipantLibrary::Zero",
                          filepath + "/../Mixed.xml")
inputDDS = connector.getInput("MySubscriber::MySquareReader")

for i in range(1, 500):
    inputDDS.take()
    numOfSamples = inputDDS.samples.getLength()
    for j in range(1, numOfSamples+1):
        if inputDDS.infos.isValid(j):
            """There are two alternative way to access the sample...
            1) Get it as a dictionary and then access the field in the
            standard python way:"""
            sample = inputDDS.samples.getDictionary(j)
            x = sample['x']
            color = sample['color']

            # 2) Access each single field with the connector API:
            x = inputDDS.samples.getNumber(j, "x")
            color = inputDDS.samples.getString(j, "color")

            # This is how you get the size of a seqence:
            seqLength = inputDDS.samples.getNumber(j, "aOctetSeq#")
            print("I received a seqence with " + repr(seqLength) + "elements")

            # Print the sample
            print(sample)
    sleep(2)
