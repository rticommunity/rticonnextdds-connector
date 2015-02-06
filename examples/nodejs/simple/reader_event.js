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
var input = connector.getInput("MySubscriber::MySquareReader");

connector.on('on_data_available',
   function() {
     input.take();
     for (i=1; i <= input.samples.getLength(); i++) {
         if (input.infos.isValid(i)) {
             console.log(JSON.stringify(input.samples.getJSON(i)));
         }    
     }

});

console.log("Waiting for data");
