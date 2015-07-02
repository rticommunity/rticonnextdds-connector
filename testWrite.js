var sleep = require('sleep');
var rti = require('./node');

var connector = new rti.Connector("MyParticipantLibrary::Zero",__dirname +"/examples/nodejs/ShapeExample.xml");
var input = connector.getInput("MySubscriber::MySquareReader");
var output = connector.getOutput("MyPublisher::MySquareWriter");

var i = 0;
while ( i < 3 ) {
  if(i === 0) sleep.sleep(1);
  output.instance.setNumber("x",i);
  output.instance.setNumber("y",i*2);
  output.instance.setNumber("shapesize",30);
  output.instance.setString("color", "REEDHARRY");
  console.log("|Writing...", i);
  output.write();
  i = i + 1;
}
