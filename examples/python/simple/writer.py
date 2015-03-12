##############################################################################
#    (c) 2005-2015 Copyright, Real-Time Innovations, All rights reserved.    #
#                                                                            #
# RTI grants Licensee a license to use, modify, compile, and create          #
# derivative works of the Software.  Licensee has the right to distribute    #
# object form only for use with RTI products. The Software is provided       #
# "as is", with no warranty of any type, including any warranty for fitness  #
# for any purpose. RTI is under no obligation to maintain or support the     #
# Software.  RTI shall not be liable for any incidental or consequential     #
# damages arising out of the use or inability to use the software.           #
#                                                                            #
##############################################################################
import sys
import os
filepath = os.path.dirname(os.path.realpath(__file__))
sys.path.append(filepath + "/../../../");
import time
import rticonnextdds_connector as rti

connector = rti.Connector("MyParticipantLibrary::Zero", filepath + "/../ShapeExample.xml");
output    = connector.getOutput("MyPublisher::MySquareWriter")

for i in range(1,500): 
	output.instance.setNumber("x", i);
	output.instance.setNumber("y", i*2);
	output.instance.setNumber("shapesize", 30);
	output.instance.setString("color", "BLUE");
	output.write();
	time.sleep(2)

