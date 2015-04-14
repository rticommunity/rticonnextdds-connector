var rti = require('./node');

var connector = new rti.Connector("MyParticipantLibrary::Zero",__dirname +"/examples/nodejs/ShapeExample.xml");
var reader = connector.getInput("MyPublisher::MySquareReader");

console.log(reader);
connector.delete();
console.log(connector);
