var expect=require('chai').expect
var rti= require(__dirname + '/../../rticonnextdds-connector')

describe('Output Tests',function() {
  var connector=null 
  // Initialization before all tests execute
  before(function(){
    var participant_profile = "MyParticipantLibrary::Zero"
    var xml_profile = __dirname + "/../xml/ShapeExample.xml"
    connector = new rti.Connector(participant_profile,xml_profile)
  })

  // Cleanup after all tests have executed
  after(function() {
    this.timeout(0)
    connector.delete()
    
  })

  it('Output object should not get instantiated for invalid DataWriter',function(){
    var invalid_DW = "invalidDW"
    expect(function(){
      connector.getWriter(invalid_DW)
    }).to.throw(Error)
  }) 

  it('Output object should get instantiated for valid ' +
      'Publication::DataWriter name',function (){
    var valid_DW = "MyPublisher::MySquareWriter" 
    var output= connector.getOutput(valid_DW)
    expect(output).to.exist 
    expect(output.name).to.equal(valid_DW)
    expect(output.connector).to.equal(connector)
  })

  describe('Tests on Output\'s Instance',function() {
    var output = null
    //Initialization before all tests execute in this describe block
    before(function(){
      output= connector.getOutput("MyPublisher::MySquareWriter")  
    })

    it('output\'s instance should exist',function(){
      expect(output.instance).to.exist
    })

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setNumber on non-existent field should throw error and '+
      'subscriber should not get a message with default values',function(){
      expect(function (){
        output.instance.setNumber("invalid_field",1)
      }).to.throw(Error)
    })

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setString on non-existent field should throw error and '+
      'subscriber should not get a message with default values',function(){
      expect(function (){
        output.instance.setString("invalid_field","value")
      }).to.throw(Error)
    })

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setBoolean on non-existent field should throw error and '+
      'subscriber should not get a message with default values',function(){
      expect(function (){
        output.instance.setBoolean("invalid_field",true)
      }).to.throw(Error)
    })

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setFromJSON should throw error for a JSON object ' +
      'with non-existent fields and subscriber should not get ' +
      'a message with default values',function(){
       expect(function (){
          var invalid_data = '{"invalid_field":1}'
          output.instance.setFromJSON(JSON.parse(
            '{"invalid_field":1}'))
      }).to.throw(Error)
    })

    it('setString with boolean value should throw Error', function(){
      expect(function(){
        var string_field="color"
        output.instance.setString(string_field,true)
      }).to.throw(Error)
    })

    it('setString with number value should throw Error', function(){
      expect(function(){
        var string_field="color"
        output.instance.setString(string_field,11)
      }).to.throw(Error)
    })

    it('setString with dictionary value should throw Error', function(){
      expect(function(){
        var string_field="color"
        output.instance.setString(string_field,{"key":"value"})
      }).to.throw(Error)
    })

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setNumber with string value should throw Error and'+
      'subscriber should not get a message with erroneous field data', function(){
      expect(function(){
        var number_field="x"
        output.instance.setNumber(number_field,"value")
      }).to.throw(Error)
    })

    // un-implemented test
    it('Note: implicit type-conversion for setNumber with boolean value')

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setNumber with dictionary value should throw Error and '+
      'subscriber should not get a message with erroneous field data', function(){
      expect(function(){
        var number_field="x"
        output.instance.setNumber(number_field,{"key":"value"})
      }).to.throw(Error)
    })
 
    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setBoolean with string value should throw Error and '+
      'subscriber should not get a  message with erroneous field data', function(){
      expect(function(){
        var boolean_field="z"
        output.instance.setBoolean(boolean_field,"value")
      }).to.throw(Error)
    })

    // unimplemented test
    it('Note: implicit type-conversion for setBoolean with number value')

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setBoolean with dictionary value should throw Error and '+
      'subscriber should not get a  message with erroneous field data', function(){
      expect(function(){
        var boolean_field="z"
        output.instance.setBoolean(boolean_field,{"key":"value"})
      }).to.throw(Error)
    })

    //skipped test: Condition being tested has not been accounted for yet
    it.skip('setFromJSON for JSON object with incompatible value types ' + 
      'should throw Error and subscriber should not get a message with ' + 
      'erroneous field data', function(){
        expect(function(){
          var str= '{"x":"5","y":true,"color":true,"shapesize":"5","z":"value"}'
          output.instance.setFromJSON(JSON.parse(str))
        }).to.throw(Error)
    })
  })

})
