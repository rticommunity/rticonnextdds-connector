var bindings = require('../build/Release/rtiddsconnector.node'),
    util = require('util'),
    eventEmitter = require('events').EventEmitter;

/*
 * Samples Object
 *
 *
 */
var Samples = function(pointer, name) {
  this._connector = pointer;
  this._name = name;
};

Samples.prototype.getLength = function() {
  return bindings.get_samples_length(this._connector,this._name);
};

Samples.prototype.getNumber = function(index, fieldName) {
  return bindings.get_number_from_samples(this._connector,this._name,index,fieldName);
};

Samples.prototype.getBoolean = function(index, fieldName) {
  return bindings.get_boolean_from_samples(this._connector,this._name,index,fieldName);
};

Samples.prototype.getString = function(index, fieldName) {
  return bindings.get_string_from_samples(this._connector,this._name,index,fieldName);
};

Samples.prototype.getJSON = function(index) {
  return JSON.parse(bindings.get_json_sample(this._connector,this._name,index));
};

/*
 * Infos Object
 *
 *
 */
var Infos = function(pointer, name) {
  this._connector = pointer;
  this._name = name;
};

Infos.prototype.getLength = function() {
  return bindings.get_infos_length(this._connector,this._name);
};

Infos.prototype.isValid = function(index) {
  return bindings.get_boolean_from_infos(this._connector,this._name,index,'valid_data');
};

/*
 * Input Object
 *
 *
 */
var Input = function(pointer,name) {
  this._connector = pointer;
  this._name = name;
  this.samples = new Samples(this._connector,this._name);
  this.infos = new Infos(this._connector,this._name);
};

Input.prototype.read = function() {
  bindings.reader_read(this._connector,this._name);
};

Input.prototype.take = function() {
  bindings.reader_take(this._connector,this._name);
};


/*
 * Instance Object
 *
 *
 */
var Instance = function(pointer, name) {
  this._connector = pointer;
  this._name = name;
};

Instance.prototype.setNumber = function(fieldName, value) {
  bindings.set_number_into_samples(this._connector,this._name,fieldName,value);
};

Instance.prototype.setBoolean = function(fieldName, value) {
  bindings.set_boolean_into_samples(this._connector,this._name,fieldName,value);
};

Instance.prototype.setString = function(fieldName, value) {
  bindings.set_string_into_samples(this._connector,this._name,fieldName,value);
};

Instance.prototype.setFromJSONI = function(prefix,jsonObj) {
  for (var key in jsonObj) {
    var value = jsonObj[key];
    if (typeof(value) == "string") {
      console.log('set ' + prefix+key + ' to ' + value);
      this.setString(prefix+key,value);
    } else if (typeof(value) == "number") {
      console.log('set ' + prefix+key + ' to ' + value);
      this.setNumber(prefix+key,value);
    } else if (typeof(value) == "boolean") {
      console.log('set ' + prefix+key + ' to ' + value);
      this.setBoolean(prefix+key,value);
    } else if (typeof(value) == "object") {
      oldprefix = prefix;
      if (prefix.length > 0) {
        prefix = prefix + '.';
        lengthAdded = lengthAdded + 1;
      }
      prefix = prefix + key + '.';
      setFromJSONI(prefix, value);
      prefix = oldprefix;
    } else {
       console.log('Nothing to do for key: ' + key + ' of value: ' + value);
    }
    //console.log(key + ' = ' + value);
  }
};

Instance.prototype.setFromJSON = function(jsonObj) {
  this.setFromJSONI("",jsonObj);
};

/*
 * Output Object
 *
 *
 */
var Output = function(pointer, name) {
  this._connector = pointer;
  this._name = name;
  this.instance = new Instance(this._connector, this._name);
};

Output.prototype.write = function() {
  return bindings.writer_write(this._connector,this._name);
};

/*
 * Connector Object
 *
 *
 */
var Connector = function(configName,fileName) {
  eventEmitter.call(this);
  this._pointer = bindings.connector_new(configName,fileName);
  // this._dataAvailable = false;

  var onDataAvailable = function(){
    // var connector = this;
    // var rc = rtin.RTIDDSConnector_wait.async(
    //     conn._native, -1,
    //     function(err, res) {
    //       if (err) throw err;
    //       connector.emit("data");
    //       onDataAvailable();
    //     }
    // );
  };
};

//inherit EventEmitter
util.inherits(Connector, eventEmitter);

Connector.prototype.delete = function() {
  return bindings.connector_delete(this._pointer);
};

Connector.prototype.getInput = function(inputName) {
  return new Input(this._pointer,inputName);
};

Connector.prototype.getOutput = function(outputName) {
  return new Output(this._pointer,outputName);
};

module.exports.Connector = Connector;
