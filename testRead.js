var sleep = require('sleep');
var rti = require('./node');

var connector = new rti.Connector("MyParticipantLibrary::Zero",__dirname +"/examples/nodejs/ShapeExample.xml");
var input = connector.getInput("MySubscriber::MySquareReader");
var output = connector.getOutput("MyPublisher::MySquareWriter");

connector.on('on_data_available', function() {
  console.log("|Reading... ");
  input.take();
  var length = input.samples.getLength();
  for (j=1; j <= length; j++) {
    if (input.infos.isValid(j)) {
      console.log(input.samples.getJSON(j));
    }
  }
});
