/*****************************************************************************
*    (c) 2005-2015 Copyright, Real-Time Innovations, All rights reserved.    *
*                                                                            *
* RTI grants Licensee a license to use, modify, compile, and create          *
* derivative works of the Software.  Licensee has the right to distribute    *
* object form only for use with RTI products. The Software is provided       *
* "as is", with no warranty of any type, including any warranty for fitness  *
* for any purpose. RTI is under no obligation to maintain or support the     *
* Software.  RTI shall not be liable for any incidental or consequential     *
* damages arising out of the use or inability to use the software.           *
*                                                                            *
******************************************************************************/

var sleep = require('sleep');
var rti   = require('rticonnextdds-connector');

var connector = new rti.Connector("MyParticipantLibrary::Zero",__dirname + "/../ShapeExample.xml");
var output = connector.getOutput("MyPublisher::MySquareWriter");

var i =0;
for (;;) {
    i = i + 1;
    output.instance.setNumber("x",i);
    output.instance.setNumber("y",i*2);
    output.instance.setNumber("shapesize",30);
    output.instance.setString("color", "BLUE");
    console.log("Writing...");
    output.write();
    sleep.usleep(1000);
}
