var sleep = require('sleep');
var rti = require('./node');

var connector = new rti.Connector("MyParticipantLibrary::Zero",__dirname +"/examples/nodejs/ShapeExample.xml");
var input = connector.getInput("MyPublisher::MySquareReader");
var output = connector.getOutput("MyPublisher::MySquareWriter");

var i =0;
for (;;) {
  console.log("Waiting for samples...");
  input.read();
  var length = input.samples.getLength();
  console.log(length);
  for (j=1; j <= length; j++) {
    if (input.infos.isValid(i)) {
      console.log(JSON.stringify(input.samples.getJSON(j)));
    }
  }
  i = i + 1;
  output.instance.setNumber("x",i);
  output.instance.setNumber("y",i*2);
  output.instance.setNumber("shapesize",30);
  output.instance.setString("color", "REEDHARRY");
  console.log("Writing...", i);
  output.write();
  // sleep.sleep(2);
}
