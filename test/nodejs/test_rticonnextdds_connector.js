var expect=require('chai').expect
var rti= require(__dirname + '/../../rticonnextdds-connector')
var sinon = require('sinon')

describe('Connector Tests',function() {

  it('Connector should throw an error for invalid xml path', function(){
    var participant_profile = "MyParticipantLibrary::Zero"
    var invalid_xml_path = "invalid/path/to/xml"
    expect(function(){
      new rti.Connector(participant_profile,invalid_xml_path)
    }).to.throw(Error)
  })

  it('Connector should throw an error for invalid participant profile', function(){
    var invalid_participant_profile = "InvalidParticipantProfile"  
    var xml_path = __dirname+ "/../xml/ShapeExample.xml"
    expect(function(){
      new rti.Connector(invalid_participant_profile,xml_path)
    }).to.throw(Error)
  })

  it('Connector should throw an error for invalid xml profile', function(){
    var participant_profile = "MyParticipantLibrary::Zero"
    var invalid_xml = __dirname + "/../xml/InvalidXml.xml"
    expect(function(){
      new rti.Connector(participant_profile,invalid_xml)
    }).to.throw(Error)
  })
 
  it('Connector should get instantiated for valid' +
         'xml and participant profile', function(){
    var participant_profile = "MyParticipantLibrary::Zero"
    var xml_profile = __dirname +  "/../xml/ShapeExample.xml"
    var connector =  new rti.Connector(participant_profile,xml_profile)
    expect(connector).to.exist
    expect(connector).to.be.instanceOf(rti.Connector) 
  })

  it('Multiple Connector objects can be instantiated', function(){
    var participant_profile = "MyParticipantLibrary::Zero"
    var xml_profile = __dirname + "/../xml/ShapeExample.xml"
    var connectors = []
    for(var i=0; i< 5; i++)
      connectors.push(new rti.Connector(participant_profile,xml_profile))
    connectors.forEach(function(connector){
      expect(connector).to.exist 
      expect(connector).to.be.instanceOf(rti.Connector)
    }) 
  })

  describe('Connector callback test', function () {
    var connector

    //Initialization before all tests are executed
    before(function(){
      var participant_profile = "MyParticipantLibrary::Zero"
      var xml_profile = __dirname +  "/../xml/ShapeExample.xml"
      connector =  new rti.Connector(participant_profile,xml_profile)
    })

    //Cleanup after all tests have executed
    after(function() {
      this.timeout(0)
      connector.delete()
    })

    it('on_data_available callback gets called when data is available',
      function (done){
      //spies are used for testing callbacks
      var spy = sinon.spy() 
      setTimeout(function(){
        expect(spy.calledOnce).to.be.true
        done() //Pattern for async testing:  next test won't execute until done gets called.
      },1000) //Expectation Test will execute after 1000 milisec
      connector.once("on_data_available",spy)
      output = connector.getOutput("MyPublisher::MySquareWriter")
      testMsg='{"x":1,"y":1,"z":true,"color":"BLUE","shapesize":5}'
      output.write(JSON.parse(testMsg))
    }) 
    
  })
})
