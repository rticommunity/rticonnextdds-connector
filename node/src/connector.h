#pragma once

#include <string>
#include <node.h>
#include <node_object_wrap.h>
#include <nan.h>
// #include <librti_dds_connector.h>
#include "binding.h"

using namespace v8;

namespace rti {

  /**
   * A function to use as a JS function that does nothing and returns this
   */
   static inline NAN_METHOD(object_new);

   void init_connector(v8::Handle<v8::Object> target);

  /**
   * This utility class allows to keep track of a C pointer that we attached
   * to a JS object. It differs from node's ObjectWrap in the fact that it
   * does not need a constructor and both attributes are public.
   * Node's ObjectWrap is useful to wrap C++ classes whereas this class is useful
   * to wrap C structs. THIS CLASS DOES NOT MANAGE C MEMORY ALLOCATION
   */
  template <typename T>
    class ObjectHandle : public node::ObjectWrap {
      public:
        /**
         * @constructor
         * Create a new ObjectHandle object with the given name
         * the name can be used later to identify the wrapped objects
         */
        ObjectHandle(const char* name);

        /**
         * Utility function to retrieve an ObjectHandle from a JS object
         * @param obj, the JS Object in which the ObjectHandle was wrapped
         */
        static ObjectHandle<T>* Unwrap(v8::Handle<v8::Value> obj);

        /**
         * A pointer to the C struct (most often) that we want to wrap
         * We do not allocate this
         */
        T* pointer;

        /**
         *  The JS Object that we set our pointer in
         *  We do create this one
         */
        v8::Persistent<v8::Object> object;

        /**
         * Get the name of the ObjectHandle that we gave it during instanciation
         */
        char* GetName() {
          return *(v8::String::Utf8Value(name_));
        }

      protected:
      private:
        v8::Persistent<v8::String> name_;

        /**
         * Empty function to set as constructor for an FunctionTemplate
         * @deprecated
         */
        v8::Handle<v8::Value> New(const FunctionCallbackInfo<Value>& args) {
          v8::HandleScope scope;
          // do nothing;
          return args.This();
        }
    };

  template <typename T>
    ObjectHandle<T>::ObjectHandle(const char* name) : pointer(NULL) {
      v8::Local<v8::FunctionTemplate> tpl = NanNew<v8::FunctionTemplate>(object_new);
      v8::Handle<v8::String> tempName = NanNew<String>(name == NULL ? "CObject" : name);
      NanAssignPersistent(name_, tempName);


      tpl->SetClassName(tempName);
      tpl->InstanceTemplate()->SetInternalFieldCount(1);

      v8::Local<v8::ObjectTemplate> otpl = tpl->InstanceTemplate();
      v8::Local<v8::Object> tempObject = otpl->NewInstance();

      NanSetInternalFieldPointer(tempObject, 0, this);

      NanAssignPersistent(object, tempObject);
    }

  template <typename T>
    ObjectHandle<T>* ObjectHandle<T>::Unwrap(v8::Handle<v8::Value> obj) {
      assert(obj->IsObject());
      v8::Handle<v8::Object> handle = obj->ToObject();
      ObjectHandle<T>* ptr = node::ObjectWrap::Unwrap<ObjectHandle<T> >(handle);
      return ptr;
    }

} // end namespace RTI
