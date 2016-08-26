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

connector = rti.Connector("MyParticipantLibrary::Zero", filepath + "/../Mixed.xml");
output    = connector.getOutput("MyPublisher::MySquareWriter")

for i in range(1,500):
	# We clear the instance associated to this output
	# otherwise the sample will have the values set in the
	# previous iteration
	output.clear_members();
	# Here an example on how to set a number
	output.instance.setNumber("x", i);
	# Here an example on how to set a string
	output.instance.setString("color", "BLUE");
	# Here we are going to set the elements of a sequence.
	# - the sequence was declared with maxSize 30
	# - we will always set two elements and..
	# - ... the third element only half of the time
	# If you open rtiddsspy you will see that the length is
	# automatically set to the right value.
	output.instance.setNumber("aOctetSeq[1]", 42);
	output.instance.setNumber("aOctetSeq[2]", 43);
	if i%2==0:
		output.instance.setNumber("aOctetSeq[3]", 44);
	# Now we write the sample
	output.write();
	time.sleep(2)
