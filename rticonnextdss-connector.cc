#include <node.h>
#include "connector.h"

using namespace v8;

void InitAll(Handle<Object> exports) {
  Connector::Init(exports);
}

NODE_MODULE(rticonnext-dds, InitAll)
