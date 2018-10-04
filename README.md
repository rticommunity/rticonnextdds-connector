rticonnextdds-connector
=======

### RTI Connector for Connext DDS
*RTI Connector* for Connext DDS is a quick and easy way to access the power and
functionality of [RTI Connext DDS](http://www.rti.com/products/index.html).
It is based on [XML Application Creation](https://community.rti.com/static/documentation/connext-dds/5.3.1/doc/manuals/connext_dds/xml_application_creation/RTI_ConnextDDS_CoreLibraries_XML_AppCreation_GettingStarted.pdf) and Dynamic Data.

*Connector* was created by the RTI Research Group to quickly and easily develop demos
and proof of concept. We think that it can be useful for anybody who needs
a quick way to script tests and interact with DDS using different scripting languages.

It can be used to quickly create tests for your distributed system and, thanks
to the binding with scripting languages and the use of XML, to easily integrate
with tons of other available technologies.

The *Connector* library is provided in binary form for selected architectures. Scripting language bindings and examples are provided in source format.

### Language Support

 * **[Node.js/JavaScript](https://github.com/rticommunity/rticonnextdds-connector-js)**
 * **[Python](https://github.com/rticommunity/rticonnextdds-connector-py)**
 * **[Go](https://github.com/rticommunity/rticonnextdds-connector-go)**
 * **[Lua (through RTI Prototyper)](https://community.rti.com/downloads/experimental/rti-prototyper-with-lua)**
 * **[C](https://github.com/rticommunity/rticonnextdds-connector/tree/master/examples/lua_c_integration)**

### Platform support
We are building our library for a few architectures only. Check them out [here](https://github.com/rticommunity/rticonnextdds-connector/tree/master/lib). If you need another architecture, please contact your RTI account manager or sales@rti.com.

**Windows Note**: We tested the Node.js/JavaScript Connector on Win10 64 bit. We notice that npm works best with VS Express 2013.
Feel free to ask questions on the [RTI Community forum](https://community.rti.com/forums/technical-questions) for more details on Windows and Connector.

If you want to check the version of the libraries, run the following command:

``` bash
strings librtiddsconnector.dylib | grep BUILD
```

### Threading model
The *Connector* Native API does not yet implement any mechanism for thread safety. Originally the *Connector* native code was built to work with *Prototyper* and Lua. That was a single threaded loop. We then introduced support for JavaScript and Python. For now, the responsibility of protecting calls to the *Connector* is left to you. (In future we may add thread safety in the native layer.)
In Node.js/JavaScript, threading should not be a problem due to the 'callback' style of the language itself.
In Python, you will have to protect the calls to the Connector if you are using different threads. For an example, see [Protecting calls to the Connector library](https://github.com/rticommunity/rticonnextdds-connector/tree/master/examples/python#protecting-calls-to-the-connector-library) in the Python README.



### Support
This is an experimental RTI product. As such, we offer support through the [RTI Community forum](https://community.rti.com/forums/technical-questions).

### License
With the sole exception of the contents of the "examples" subdirectory, all use of this product is subject to the RTI Software License Agreement included at the top level of this repository. Files within the "examples" subdirectory are licensed as marked within the file.

This software is an experimental ("pre-production") product. The Software is provided "as is," with no warranty of any type, including any warranty for fitness for any purpose. RTI is under no obligation to maintain or support the software. RTI shall not be liable for any incidental or consequential damages arising out of the use or inability to use the software.
