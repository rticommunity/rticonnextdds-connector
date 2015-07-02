#pragma once

#include <string>

// typedef struct rti_connector rti_connector;
using namespace v8;

extern "C" {
  struct rti_connector;

  //RTIDDSConnector construction and destruction;
  rti_connector* RTIDDSConnector_new(const char* configName, const char* fileName);
  void RTIDDSConnector_delete(rti_connector* connector);

  //RTIDDSConnector read methods;
  void RTIDDSConnector_read(rti_connector* connector, const char* readerName);
  void RTIDDSConnector_take(rti_connector* connector, const char* readerName);

  //RTIDDSConnector write method;
  void RTIDDSConnector_write(rti_connector* connector, const char* writerName);

  //RTIDDSConnector writer methods;
  void RTIDDSConnector_setNumberIntoSamples(
    rti_connector* connector,
    const char* writerName,
    const char* field,
    double number
  );
  void RTIDDSConnector_setStringIntoSamples(
    rti_connector* connector,
    const char* writerName,
    const char* field,
    const char* string
  );
  void RTIDDSConnector_setBooleanIntoSamples(
    rti_connector* connector,
    const char* writerName,
    const char* field,
    int boolean
  );

  //RTIDDSConnector sample methods;
  double RTIDDSConnector_getSamplesLength(rti_connector* connector, const char* readerName);
  double RTIDDSConnector_getNumberFromSamples(
    rti_connector* connector,
    const char* readerName,
    int index,
    const char* fieldName
  );
  int RTIDDSConnector_getBooleanFromSamples(
    rti_connector* connector,
    const char* readerName,
    int index,
    const char* fieldName
  );
  char* RTIDDSConnector_getStringFromSamples(
    rti_connector* connector,
    const char* readerName,
    int index,
    const char* fieldName
  );
  char* RTIDDSConnector_getJSONSample(
    rti_connector* connector,
    const char* readerName,
    int index
  );

  //RTIDDSConnector info methods;
  double RTIDDSConnector_getInfosLength(
    rti_connector* connector,
    const char* readerName
  );
  int RTIDDSConnector_getBooleanFromInfos(
    rti_connector* connector,
    const char* readerName,
    int index,
    const char* value
  );
}
