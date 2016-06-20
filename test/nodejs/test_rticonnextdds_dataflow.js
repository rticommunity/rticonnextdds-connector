var expect=require('chai').expect
var rti= require(__dirname + '/../../rticonnextdds-connector')
var sleep=require('sleep')

// Test Parameterization- describe block will execute once for each param
var params=["read","take"] 

params.forEach(function(retrieval_method) {

  describe('DataflowTests for '+retrieval_method, function(){
    var connection, input, output, testMsg
    //Initialization before all tests execute
    before(function(){
      testMsg={"x":1,"y":1,"z":true,"color":"BLUE","shapesize":5}
      var participant_profile = "MyParticipantLibrary::Zero"
      var xml_profile = __dirname + "/../xml/ShapeExample.xml"
      connector = new rti.Connector(participant_profile,xml_profile)
      input = connector.getInput("MySubscriber::MySquareReader")
      output = connector.getOutput("MyPublisher::MySquareWriter")
    })  
   
    //Clean-up after all tests execute
    after(function() {
      this.timeout(0)
      connector.delete()
    })

    //Initialization done before each test executes
    beforeEach(function(){
      //take pre-existing samples from middleware chache
      input.take()
      output.instance.setFromJSON(JSON.parse(JSON.stringify(testMsg)))
      output.write()
      //loop to allow sometime for discovery of Input and Output objects
      for (var i=0;i<20;i++){
        sleep.usleep(500)
        input[retrieval_method]()
        if (input.samples.getLength() > 0)
          break
      }
    })

    it('samples length should be 1',function(){
      var len=input.samples.getLength()
      expect(len).to.equal(1)
    })

    it('infos length should be 1',function(){
      var len=input.infos.getLength()
      expect(len).to.equal(1)
    })

    it('data received should be valid',function(){
      var validity=input.infos.isValid(1)
      expect(validity).to.equal(1)
    })
 
    it('received JSON representation of data should be the same as '+
      'the JSON object sent',function(){
      var received_JSON=input.samples.getJSON(1)
      expect(received_JSON).to.deep.equal(JSON.parse(JSON.stringify(testMsg)))
    })

    it('received fields of data should be the same as '+
      'that of the JSON object sent',function(){
      var x = input.samples.getNumber(1,"x")
      var y = input.samples.getNumber(1,"y")
      var z = input.samples.getBoolean(1,"z")
      var color = input.samples.getString(1,"color")
      var shapesize = input.samples.getNumber(1,"shapesize")

      expect(x).to.equal(testMsg['x'])
      expect(y).to.equal(testMsg['y'])
      //NOTE: getBoolean returns an Integer representation of Boolean
      expect(z).to.equal(+ testMsg['z'])
      expect(shapesize).to.equal(testMsg['shapesize'])
      expect(color).to.equal(testMsg['color'])
    })
    
    //Unimplemented tests
    it('Behavior of getBoolean on String or Number fields should be considered')
    it('Behavior of getString on Number or Boolean fields should be considered')
    it('Behavior of getNumber on String or Boolean fields should be considered')

  })
})
