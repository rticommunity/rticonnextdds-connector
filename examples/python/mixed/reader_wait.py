##############################################################################
# Copyright (c) 2005-2015 Real-Time Innovations, Inc. All rights reserved.
# Permission to modify and use for internal purposes granted.
# This software is provided "as is", without warranty, express or implied.
##############################################################################

import sys
import os
filepath = os.path.dirname(os.path.realpath(__file__))
sys.path.append(filepath + "/../../../");
import time
import rticonnextdds_connector as rti


connector = rti.Connector("MyParticipantLibrary::Zero",filepath + "/../Mixed.xml");
input = connector.getInput("MySubscriber::MySquareReader");

for i in range(1,500):
	timeout = -1
	print("Calling wait with a timeout of " + repr(timeout));
	ret = connector.wait(timeout);
	print("The wait returned " + repr(ret));
	input.take();
	numOfSamples = input.samples.getLength();
	for j in range (1, numOfSamples+1):
		if input.infos.isValid(j):
			# There are two alternative way to access the sample...
			# 1) get it as a dictionary and then access the field in the
			#    standard python way:
			sample = input.samples.getDictionary(j);
			x = sample['x'];
			color = sample['color'];
			# or
			# 2) access each single field with the connector API:
			x = input.samples.getNumber(j, "x");
			color = input.samples.getString(j, "color");
			# This is how you get the size of a seqence:
			seqLength = input.samples.getNumber(j, "aOctetSeq#");
			print("I received a seqence with " + repr(seqLength) + "elements" );
			# Print the sample
			print(sample);
	time.sleep(2);
