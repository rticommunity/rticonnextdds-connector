var expect=require('chai').expect
var rti= require(__dirname+ '/../../rticonnextdds-connector')

describe('Input Tests',function() {
  var connector=null 
  //Initialization before all tests are executed 
  before(function(){
    var participant_profile = "MyParticipantLibrary::Zero"
    var xml_profile = __dirname +  "/../xml/ShapeExample.xml"
    connector = new rti.Connector(participant_profile,xml_profile)
  })
  
  //cleanup after all tests have executed
  after(function() {
    this.timeout(0)
    connector.delete()
  })

  it('Input object should not get instantiated for invalid DataReader',function(){
    var invalid_DR = "invalidDR"
    expect(function(){
      connector.getInput(invalid_DR)
    }).to.throw(Error)
  }) 

  it('Input object should get instantiated for valid ' +
      'Subscription::DataReader name',function (){
    var valid_DR = "MySubscriber::MySquareReader" 
    var input = connector.getInput(valid_DR)
    expect(input).to.exist 
    expect(input.name).to.equal(valid_DR)
    expect(input.connector).to.equal(connector)
  })
})
