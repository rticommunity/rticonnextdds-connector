#include <nan.h>
#include "connector.h"
#include "reader.h"

using namespace v8;
using namespace node;
using namespace rti;

static Persistent<FunctionTemplate> reader_constructor;

Reader::Reader(std::string readerName)
 : readerName_(readerName) {

 }

Reader::~Reader() {
}

void Reader::Init() {
  //Prepare reader_constructor template.
  Local<FunctionTemplate> tpl = NanNew<FunctionTemplate>(Reader::New);
  NanAssignPersistent(reader_constructor, tpl);
  tpl->SetClassName(NanNew("Reader"));
  tpl->InstanceTemplate()->SetInternalFieldCount(1);

  NODE_SET_PROTOTYPE_METHOD(tpl, "read", Reader::Read);
  NODE_SET_PROTOTYPE_METHOD(tpl, "take", Reader::Take);
}

NAN_METHOD(Reader::New) {
  NanScope();

  if (args.IsConstructCall()) {
    Connector* conn = node::ObjectWrap::Unwrap<Connector>(args[0]->ToObject());

    v8::String::Utf8Value rName(args[1]->ToString());
    std::string readerName(*rName, rName.length());

    Reader* reader = new Reader(readerName);
    reader->connector = conn->GetPointer();

    reader->Wrap(args.This());
    args.GetReturnValue().Set(args.This());
  } else {
    //turn into construct call.
  }
}

v8::Local<v8::Object> Reader::NewInstance(Local<Object> connector, Local<Value> readerName) {
  NanEscapableScope();

  Local<Object> instance;
  Local<FunctionTemplate> constructorHandle = NanNew<FunctionTemplate>(reader_constructor);

  const int argc = 2;
  Handle<Value> argv[argc] = { connector, readerName };
  instance = constructorHandle->GetFunction()->NewInstance(argc, argv);

  return NanEscapeScope(instance);
}

NAN_METHOD(Reader::Read) {
  NanScope();

  Reader* reader = node::ObjectWrap::Unwrap<Reader>(args.This());
  RTIDDSConnector_read(reader->connector, reader->readerName_.c_str());

  NanReturnUndefined();
}

NAN_METHOD(Reader::Take) {
  NanScope();

  Reader* reader = node::ObjectWrap::Unwrap<Reader>(args.This());
  RTIDDSConnector_take(reader->connector, reader->readerName_.c_str());

  NanReturnUndefined();
}
