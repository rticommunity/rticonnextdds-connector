#include <node.h>
#include "connector.h"

using namespace v8;

extern "C" {

  void Init(Handle<Object> exports) {
    rti::init_connector(exports);
  }

  NODE_MODULE(rtiddsconnector, Init);
}
