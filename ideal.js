var rti = require('./node/rticonnextdds-connector');

var connector = new rti.Connector("MyParticipantLibrary::Zero",__dirname +"/examples/nodejs/ShapeExample.xml");

var subscriber = connector.subscribe("MyPublisher::MySquareReader");

subscriber.on('data', function() {
  subscriber.read(function(err, samples, length) {
    for(var i=0; i<length; i++) {
      console.log(samples[i]);
    }
  });
});

connector.unsubscribe(subscriber);

var publisher = connector.publish("MyPublisher::MySquareWriter");
var data = {
  "id": 1,
  "url": "http://google.com",
  "active": true
};
publisher.write(data);
