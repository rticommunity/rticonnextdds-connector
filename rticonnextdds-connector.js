var os = require('os');
var ffi = require("ffi");
var util = require('util');
EventEmitter = require('events').EventEmitter

var LIB_FULL_PATH = "";
switch (os.platform()) {
  case 'darwin':
     LIB_FULL_PATH = __dirname + '/lib/x64Darwin12clang4.1/librti_dds_connector.dylib';
     break;
}

var rtin = ffi.Library(LIB_FULL_PATH, {
"RTIDDSConnector_new": [ "pointer", ["string", "string"]],
"RTIDDSConnector_getSamplesLength": [ "double", ["pointer", "string"]],
"RTIDDSConnector_getInfosLength": [ "double", ["pointer", "string"]],
"RTIDDSConnector_setNumberIntoSamples": [ "void", ["pointer", "string", "string", "double"]],
"RTIDDSConnector_getNumberFromSamples": [ "double", ["pointer", "string", "int", "string"]],
"RTIDDSConnector_getNumberFromInfos": [ "double", ["pointer", "string", "int", "string"]],
"RTIDDSConnector_setBooleanIntoSamples": [ "void", ["pointer", "string", "string", "int"]],
"RTIDDSConnector_getBooleanFromSamples": [ "int", ["pointer", "string", "int", "string"]],
"RTIDDSConnector_getBooleanFromInfos": [ "int", ["pointer", "string", "int", "string"]],
"RTIDDSConnector_setStringIntoSamples": [ "void", ["pointer", "string", "string", "string"]],
"RTIDDSConnector_getStringFromSamples": [ "string", ["pointer", "string", "int", "string"]],
"RTIDDSConnector_getStringFromInfos": [ "string", ["pointer", "string", "int", "string"]],
"RTIDDSConnector_write": [ "void", ["pointer", "string"]],
"RTIDDSConnector_read": [ "void", ["pointer", "string"]],
"RTIDDSConnector_take": [ "void", ["pointer", "string"]],
"RTIDDSConnector_wait": [ "int", ["pointer", "int"]],
"RTIDDSConnector_getJSONSample": [ "string", ["pointer", "string", "int"]],
});


function Samples(input) {

  this.getLength = function() {
    return rtin.RTIDDSConnector_getSamplesLength(input.connector.native,input.name);
  }

  this.getNumber = function(index, fieldName) {
    return rtin.RTIDDSConnector_getNumberFromSamples(input.connector.native,input.name,index,fieldName);
  }

  this.getBoolean = function(index, fieldName) {
    return rtin.RTIDDSConnector_getBooleanFromSamples(input.connector.native,input.name,index,fieldName);
  }

  this.getString = function(index, fieldName) {
    return rtin.RTIDDSConnector_getStringFromSamples(input.connector.native,input.name,index,fieldName);
  }

  this.toJSON = function(index) {
    return rtin.RTIDDSConnector_getJSONSample(input.connector.native, input.name, index);
  }

}

function Infos(input) {

  this.getLength = function() { 
    return rtin.RTIDDSConnector_getInfosLength(input.connector.native,input.name);
  }

  this.isValid = function(index) {
    return rtin.RTIDDSConnector_getBooleanFromInfos(input.connector.native,input.name,index,'valid_data');
  }

}

function Input(connector,name) {
  this.connector = connector;
  this.name = name;
  this.samples = new Samples(this)
  this.infos = new Infos(this);

  this.read = function() {
    rtin.RTIDDSConnector_read(this.connector.native,name);
  }

  this.take = function() {
    rtin.RTIDDSConnector_take(this.connector.native,name);
  }
}


function Instance(output) {

  this.setNumber = function(fieldName, value) {
    rtin.RTIDDSConnector_setNumberIntoSamples(output.connector.native,output.name,fieldName,value);
  }

  this.setBoolean = function(fieldName, value) {
    rtin.RTIDDSConnector_setBooleanIntoSamples(output.connector.native,output.name,fieldName,value);
  }

  this.setString = function(fieldName, value) {
    rtin.RTIDDSConnector_setStringIntoSamples(output.connector.native,output.name,fieldName,value);
  }
}


function Output(connector,name) {
  this.connector = connector;
  this.name = name;
  this.instance = new Instance(this)

  this.write = function() {
    return rtin.RTIDDSConnector_write(this.connector.native,name);
  }
}

function Connector(configName,fileName) {
  this.native = rtin.RTIDDSConnector_new(configName,fileName);
  var on_data_available_run = false;

  this.getInput = function(inputName) {
    return new Input(this,inputName);
  }

  this.getOutput = function(outputName) {
    return new Output(this,outputName);
  }

  var onDataAvailable = function(connector) {
      var rc = rtin.RTIDDSConnector_wait.async(
          connector.native, -1,
          function(err, res) {
            if (err) throw err;
            connector.emit("on_data_available");
            if (on_data_available_run == true) {
              onDataAvailable(connector);
            }
          }
      );
  }

  var newListerCallBack = function(eventName, fnListener) {
    if (eventName=='on_data_available') {
      if (on_data_available_run == false) {
        on_data_available_run = true;
        onDataAvailable(this);
      }
    }
  } 
 
  this.on('newListener', newListerCallBack);

  var removeListenerCallBack = function(eventName, fnListener) {
    if (EventEmitter.listenerCount(this, eventName) == 0) {
      on_data_available_run = false;
    }
  }

  this.on('removeListener', removeListenerCallBack);

}

util.inherits(Connector,EventEmitter);

module.exports.Connector = Connector;
