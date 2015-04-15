#include "connector.h"

using namespace v8;
using namespace node;
using namespace rti;

NAN_METHOD(rti::object_new) {
  NanScope();
  NanReturnValue(args.This());
}

NAN_METHOD(connector_new) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);

  // char *configName, *fileName;

  //create object handler for rti_connector struct.
  ObjectHandle<rti_connector>* conn = new ObjectHandle<rti_connector>("rti_connector");

  v8::String::Utf8Value configName(args[0]->ToString());
  v8::String::Utf8Value fileName(args[1]->ToString());

  conn->pointer = RTIDDSConnector_new(*configName, *fileName);

  //This is just for my sanity:
  assert(conn->pointer != NULL);

  NanReturnValue(conn->object);
}

NAN_METHOD(connector_delete) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 1);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  //Oh god no:
  assert(conn->pointer != NULL);

  RTIDDSConnector_delete(conn->pointer);

  conn->pointer = NULL;

  NanReturnUndefined();
}

NAN_METHOD(reader_take) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);
  assert(args[0]->IsObject());
  assert(args[1]->IsString());

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  v8::String::Utf8Value readerName(args[1]->ToString());

  //Please don't let it be:
  assert(conn->pointer != NULL);

  RTIDDSConnector_take(conn->pointer, *readerName);

  NanReturnUndefined();
}

NAN_METHOD(reader_read) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);
  assert(args[0]->IsObject());
  assert(args[1]->IsString());

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  v8::String::Utf8Value readerName(args[1]->ToString());

  // That wouldn't be good:
  assert(conn->pointer != NULL);

  RTIDDSConnector_read(conn->pointer, *readerName);

  NanReturnUndefined();
}


NAN_METHOD(writer_write) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);
  assert(args[0]->IsObject());
  assert(args[1]->IsString());

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value writerName(args[1]->ToString());

  RTIDDSConnector_write(conn->pointer, *writerName);

  NanReturnUndefined();
}

NAN_METHOD(number_into_samples) {
  NanScope();

  // argument sanity check replace by throwing exceptions
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value writerName(args[1]->ToString());
  v8::String::Utf8Value fieldName(args[2]->ToString());

  double number = args[3]->NumberValue();

  RTIDDSConnector_setNumberIntoSamples(conn->pointer,*writerName,*fieldName,number);

  NanReturnUndefined();
}

NAN_METHOD(string_into_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  //I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value writerName(args[1]->ToString());
  v8::String::Utf8Value fieldName(args[2]->ToString());
  v8::String::Utf8Value value(args[3]->ToString());

  RTIDDSConnector_setStringIntoSamples(conn->pointer,*writerName,*fieldName,*value);

  NanReturnUndefined();
}

NAN_METHOD(boolean_into_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value writerName(args[1]->ToString());
  v8::String::Utf8Value fieldName(args[2]->ToString());
  int value = (int)args[3]->BooleanValue();

  RTIDDSConnector_setBooleanIntoSamples(conn->pointer,*writerName,*fieldName,value);

  NanReturnUndefined();
}

NAN_METHOD(samples_length) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value readerName(args[1]->ToString());

  double length = RTIDDSConnector_getSamplesLength(conn->pointer, *readerName);

  NanReturnValue(NanNew<Number>(length));
}

NAN_METHOD(number_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value readerName(args[1]->ToString());
  int index = (int)args[2]->NumberValue();
  v8::String::Utf8Value fieldName(args[3]->ToString());

  double number = RTIDDSConnector_getNumberFromSamples(conn->pointer,*readerName,index,*fieldName);

  NanReturnValue(NanNew<Number>(number));
}

NAN_METHOD(boolean_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value readerName(args[1]->ToString());
  int index = (int)args[2]->NumberValue();
  v8::String::Utf8Value fieldName(args[3]->ToString());

  bool boolean = (bool)RTIDDSConnector_getBooleanFromSamples(conn->pointer,*readerName,index,*fieldName);

  NanReturnValue(NanNew<Boolean>(boolean));
}

NAN_METHOD(string_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value readerName(args[1]->ToString());
  int index = (int)args[2]->NumberValue();
  v8::String::Utf8Value fieldName(args[3]->ToString());

  char* string = RTIDDSConnector_getStringFromSamples(conn->pointer,*readerName,index,*fieldName);

  NanReturnValue(NanNew<String>(string));
}

NAN_METHOD(json_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 3);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value readerName(args[1]->ToString());
  int index = (int)args[2]->NumberValue();

  char* string = RTIDDSConnector_getJSONSample(conn->pointer,*readerName,index);

  NanReturnValue(NanNew<String>(string));
}

NAN_METHOD(infos_length) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value readerName(args[1]->ToString());

  double length = RTIDDSConnector_getInfosLength(conn->pointer,*readerName);

  NanReturnValue(NanNew<Number>(length));
}

NAN_METHOD(boolean_from_infos) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value readerName(args[1]->ToString());
  int index = (int)args[2]->NumberValue();
  v8::String::Utf8Value value(args[1]->ToString());

  bool boolean = (bool)RTIDDSConnector_getBooleanFromInfos(conn->pointer,*readerName,index,*value);

  NanReturnValue(NanNew<Boolean>(boolean));
}

void rti::init_connector(Handle<Object> target) {
  //connector
  NODE_SET_METHOD(target, "connector_new", connector_new);
  NODE_SET_METHOD(target, "connector_delete", connector_delete);

  //reader
  NODE_SET_METHOD(target, "reader_take", reader_take);
  NODE_SET_METHOD(target, "reader_read", reader_read);

  //writer
  NODE_SET_METHOD(target, "writer_write", writer_write);

  //samples
  NODE_SET_METHOD(target, "set_number_into_samples", number_into_samples);
  NODE_SET_METHOD(target, "set_string_into_samples", string_into_samples);
  NODE_SET_METHOD(target, "set_boolean_into_samples", boolean_into_samples);
  NODE_SET_METHOD(target, "get_samples_length", samples_length);
  NODE_SET_METHOD(target, "get_number_from_samples", number_from_samples);
  NODE_SET_METHOD(target, "get_boolean_from_samples", boolean_from_samples);
  NODE_SET_METHOD(target, "get_string_from_samples", string_from_samples);
  NODE_SET_METHOD(target, "get_json_sample", json_from_samples);

  //infos
  NODE_SET_METHOD(target, "get_infos_length", infos_length);
  NODE_SET_METHOD(target, "get_boolean_from_infos", boolean_from_infos);
}
