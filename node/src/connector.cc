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

  //create object handler for rti_connector struct.
  ObjectHandle<rti_connector>* conn = new ObjectHandle<rti_connector>("rti_connector");

  v8::String::Utf8Value cName(args[0]);
  std::string configName(*cName, cName.length()); //replace with memory allocated extraction

  v8::String::Utf8Value fName(args[1]);
  std::string fileName(*fName, fName.length()); //replace with memory allocated extraction

  conn->pointer = RTIDDSConnector_new(configName.c_str(), fileName.c_str());

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

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  //Please don't let it be:
  assert(conn->pointer != NULL);

  RTIDDSConnector_take(conn->pointer, readerName.c_str());

  NanReturnUndefined();
}

NAN_METHOD(reader_read) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);
  assert(args[0]->IsObject());
  assert(args[1]->IsString());

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  // That wouldn't be good:
  assert(conn->pointer != NULL);

  RTIDDSConnector_read(conn->pointer, readerName.c_str());

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

  v8::String::Utf8Value wName(args[1]);
  std::string writerName(*wName, wName.length()); //replace with memory allocated extraction

  RTIDDSConnector_write(conn->pointer, writerName.c_str());

  NanReturnUndefined();
}

NAN_METHOD(number_into_samples) {
  NanScope();

  // argument sanity check replace by throwing exceptions
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value wName(args[1]);
  std::string writerName(*wName, wName.length()); //replace with memory allocated extraction

  v8::String::Utf8Value fName(args[2]);
  std::string fieldName(*fName, fName.length()); //replace with memory allocated extraction

  double number = args[3]->NumberValue();

  RTIDDSConnector_setNumberIntoSamples(
    conn->pointer,
    writerName.c_str(),
    fieldName.c_str(),
    number
  );

  NanReturnUndefined();
}

NAN_METHOD(string_into_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  //I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value wName(args[1]);
  std::string writerName(*wName, wName.length()); //replace with memory allocated extraction

  v8::String::Utf8Value fName(args[2]);
  std::string fieldName(*fName, fName.length()); //replace with memory allocated extraction

  v8::String::Utf8Value val(args[3]);
  std::string value(*val, val.length()); //replace with memory allocated extraction

  RTIDDSConnector_setStringIntoSamples(
    conn->pointer,
    writerName.c_str(),
    fieldName.c_str(),
    value.c_str()
  );

  NanReturnUndefined();
}

NAN_METHOD(boolean_into_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value wName(args[1]);
  std::string writerName(*wName, wName.length()); //replace with memory allocated extraction

  v8::String::Utf8Value fName(args[2]);
  std::string fieldName(*fName, fName.length()); //replace with memory allocated extraction

  bool value = args[3]->BooleanValue();

  RTIDDSConnector_setBooleanIntoSamples(
    conn->pointer,
    writerName.c_str(),
    fieldName.c_str(),
    (int)value
  );

  NanReturnUndefined();
}

NAN_METHOD(samples_length) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  double length = RTIDDSConnector_getSamplesLength(conn->pointer, readerName.c_str());

  NanReturnValue(NanNew<Number>(length));
}

NAN_METHOD(number_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  int index = (int)args[2]->NumberValue();

  v8::String::Utf8Value fName(args[3]);
  std::string fieldName(*fName, fName.length()); //replace with memory allocated extraction

  double number = RTIDDSConnector_getNumberFromSamples(
    conn->pointer,
    readerName.c_str(),
    index,
    fieldName.c_str()
  );

  NanReturnValue(NanNew<Number>(number));
}

NAN_METHOD(boolean_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  int index = (int)args[2]->NumberValue();

  v8::String::Utf8Value fName(args[3]);
  std::string fieldName(*fName, fName.length()); //replace with memory allocated extraction

  bool boolean = (bool)RTIDDSConnector_getBooleanFromSamples(
    conn->pointer,
    readerName.c_str(),
    index,
    fieldName.c_str()
  );

  NanReturnValue(NanNew<Boolean>(boolean));
}

NAN_METHOD(string_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  int index = (int)args[2]->NumberValue();

  v8::String::Utf8Value fName(args[3]);
  std::string fieldName(*fName, fName.length()); //replace with memory allocated extraction

  char* string = RTIDDSConnector_getStringFromSamples(
    conn->pointer,
    readerName.c_str(),
    index,
    fieldName.c_str()
  );

  NanReturnValue(NanNew<String>(string));
}

NAN_METHOD(json_from_samples) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 3);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  int index = (int)args[2]->NumberValue();

  char* string = RTIDDSConnector_getJSONSample(
    conn->pointer,
    readerName.c_str(),
    index
  );

  NanReturnValue(NanNew<String>(string));
}

NAN_METHOD(infos_length) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 2);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  double length = RTIDDSConnector_getInfosLength(
    conn->pointer,
    readerName.c_str()
  );

  NanReturnValue(NanNew<Number>(length));
}

NAN_METHOD(boolean_from_infos) {
  NanScope();

  // argument sanity check replace by returning undefined
  assert(args.Length() == 4);

  ObjectHandle<rti_connector>* conn = ObjectHandle<rti_connector>::Unwrap(args[0]);

  // I can't even:
  assert(conn->pointer != NULL);

  v8::String::Utf8Value rName(args[1]);
  std::string readerName(*rName, rName.length()); //replace with memory allocated extraction

  int index = (int)args[2]->NumberValue();

  v8::String::Utf8Value val(args[1]);
  std::string value(*val, val.length()); //replace with memory allocated extraction

  bool boolean = (bool)RTIDDSConnector_getBooleanFromInfos(
    conn->pointer,
    readerName.c_str(),
    index,
    value.c_str()
  );

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
