/*****************************************************************************
 *    (c) 2005-2015 Copyright, Real-Time Innovations, All rights reserved.   *
 *                                                                           *
 *         Permission to modify and use for internal purposes granted.       *
 * This software is provided "as is", without warranty, express or implied.  *
 *                                                                           *
 *****************************************************************************/

var sleep = require('sleep');
var rti   = require('rticonnextdds-connector');

var connector = new rti.Connector("MyParticipantLibrary::Zero","./ShapeExample.xml");
var input = connector.getInput("MySubscriber::MySquareReader");

connector.on('on_data_available',
   function() {
     input.take();
     for (i=1; i <= input.samples.getLength(); i++) {
         if (input.infos.isValid(i)) {
             console.log(input.samples.toJSON(i));
         }    
     }

});

console.log("Waiting for data");
