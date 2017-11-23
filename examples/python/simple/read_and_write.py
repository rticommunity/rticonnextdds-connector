##############################################################################
# Copyright (c) 2005-2015 Real-Time Innovations, Inc. All rights reserved.
# Permission to modify and use for internal purposes granted.
# This software is provided "as is", without warranty, express or implied.
##############################################################################
"""Read a sample and write a new one."""

from sys import path as sysPath
from os import path as osPath
from time import sleep
filepath = osPath.dirname(osPath.realpath(__file__))
sysPath.append(filepath + "/../../../")
import rticonnextdds_connector as rti

connector = rti.Connector("MyParticipantLibrary::Zero",
                          filepath + "/../ShapeExample.xml")
inputDDS = connector.getInput("MySubscriber::MySquareReader")
outputDDS = connector.getOutput("MyPublisher::MySquareWriter")

for i in range(1, 500):
    inputDDS.take()
    numOfSamples = inputDDS.samples.getLength()
    for j in range(1, numOfSamples + 1):
        if inputDDS.infos.isValid(j):
            # This gives you a dictionary
            sample = inputDDS.samples.getDictionary(j)
            tmp = sample['x']
            sample['x'] = sample['y']
            sample['y'] = tmp
            sample['color'] = "RED"
            outputDDS.instance.setDictionary(sample)
            outputDDS.write()
    sleep(2)
