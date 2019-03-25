rticonnextdds-connector
=======

### RTI Connector for Connext DDS 6.0.0 - Getting Started
*RTI Connector* for Connext DDS is a quick and easy way to access the power and
functionality of [RTI Connext DDS](http://www.rti.com/products/index.html).
It is based on [XML Application Creation](https://community.rti.com/static/documentation/connext-dds/6.0.0/doc/manuals/connext_dds/xml_application_creation/RTI_ConnextDDS_CoreLibraries_XML_AppCreation_GettingStarted.pdf) and Dynamic Data.

*Connector* was created to quickly and easily develop demos
and proofs of concept. It can be useful for anybody who needs
a quick way to script tests and interact with Connext DDS using different scripting languages.

*Connector* can be used to quickly create tests for your distributed system and, thanks
to the binding with scripting languages and the use of XML, to easily integrate
with many other available technologies.

The *Connector* library is provided in binary form for selected architectures. Scripting language bindings and examples are provided in source format.

### Language Support

 * **[Node.js/JavaScript](https://github.com/rticommunity/rticonnextdds-connector-js)**
 * **[Python](https://github.com/rticommunity/rticonnextdds-connector-py)**
 * **[C#](https://github.com/rticommunity/rticonnextdds-connector-cs)**
 * **[Go](https://github.com/rticommunity/rticonnextdds-connector-go)**
 * **[Lua (through RTI Prototyper)](https://community.rti.com/downloads/experimental/rti-prototyper-with-lua)**
 * **[C](https://github.com/rticommunity/rticonnextdds-connector/tree/master/examples/lua_c_integration)**

### Platform support
*Connector* is supported on [select architectures](https://github.com/rticommunity/rticonnextdds-connector/tree/master/lib). If you need another architecture, please contact your RTI account manager or sales@rti.com.

**Windows Note**: RTI tested the Node.js/JavaScript Connector on Windows® 10 64-bit. Those tests showed that npm works best with Visual Studio Express 2013.
Feel free to ask questions on the [RTI Community forum](https://community.rti.com/forums/technical-questions) for more details on Windows and *Connector*.

To check the version of the libraries, run the following command. For example:

``` bash
strings librtiddsconnector.dylib | grep BUILD
```

### Threading model
The *Connector* Native API does not yet implement any mechanism for thread safety. Originally, the *Connector* native code was built to work with *Prototyper* and Lua. That was a single-threaded loop. RTI then introduced support for JavaScript and Python. For now, you are responsible for protecting calls to *Connector*. (In future, thread safety may be added in the native layer.)
In Node.js/JavaScript, threading should not be a problem due to the 'callback' style of the language itself.
In Python, you will have to protect the calls to *Connector* if you are using different threads. For an example, see [Protecting calls to the Connector library](https://github.com/rticommunity/rticonnextdds-connector/tree/master/examples/python#protecting-calls-to-the-connector-library) in the Python README.



### Support
*Connector* is an experimental RTI product. If you have questions, use the [RTI Community forum](https://community.rti.com/forums/technical-questions).

### License
With the sole exception of the contents of the "examples" subdirectory, all use of this product is subject to the RTI Software License Agreement included at the top level of this repository. Files within the "examples" subdirectory are licensed as marked within the file.

This software is an experimental ("pre-production") product. The Software is provided "as is," with no warranty of any type, including any warranty for fitness for any purpose. RTI is under no obligation to maintain or support the software. RTI shall not be liable for any incidental or consequential damages arising out of the use or inability to use the software.
