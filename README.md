rticonnextdds-connector
=======
 
### RTI Connector for Connext DDS
RTI Connector for Connext DDS is a quick and easy way to access the power and
functionality of [RTI Connext DDS](http://www.rti.com/products/index.html).
It is based on [XML Application Creation](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_XML_AppCreation_GettingStarted.pdf) and Dynamic Data.

*RTI Connector* was created by the RTI Research Group to quickly and easily develop demos
and proof of concept. We think that it can be useful for anybody who needs
a quick way to script tests and interact with DDS using different scripting languages.

It can be used to quickly create tests for your distributed system and, thanks
to the binding with scripting languages and the use of XML, to easily integrate
with tons of other available technologies.

The RTI Connector library is provided in binary form for selected architectures. Scripting language bindings and examples are provided in source format.

### Language Support

 * **Node.js/JavaScript**: we use [libffi](https://github.com/node-ffi/node-ffi) to call our library, but we try to hide
that from you using a nice JavaScript wrapper. We tested with node v8.7.0; it should work also with lower versions.
 * **Python**: here we use [ctypes](https://docs.python.org/2/library/ctypes.html) to call our native functions; of course, everything is hidden in a nice Python wrapper. We tested with both Python 2.7.14 and Python 3.6.3
 * **Lua**: (Lua version 5.1) we have been supporting Lua in our [RTI Prototyper](https://community.rti.com/downloads/experimental/rti-prototyper-with-lua) for a while now.
Check more information on our [blog](http://blogs.rti.com/tag/lua/) or in the [Getting Started Guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf). Also, stay tuned: a version that can be used directly with the standard Lua interpreter is coming ...
 * **C**: for the native code lovers, we have header files so you can call the RTI Connector API directly in your C application; that's how the RTI Prototyper is implemented. The Lua version used is 5.1.

### Platform support
We are building our library for a few architectures only. Check them out [here](https://github.com/rticommunity/rticonnextdds-connector/tree/master/lib). If you need another architecture.

**Windows Note**: For Node.js, we tested on Win10 64 bit. We notice that npm works best with VS Express 2013.
Feel free to ask questions on the [RTI Community forum](https://community.rti.com/forums/technical-questions) for more details.

If you want to check the version of the libraries, run the following command:

``` bash
strings librtiddsconnector.dylib | grep BUILD
```

### Threading model
The RTI Connext DDS Connector Native API does not yet implement any mechanism for thread safety. Originally the Connector native code was built to work with RTI DDS Prototyper and Lua. That was a single threaded loop. We then introduced support for JavaScript and Python. For now, the responsibility of protecting calls to the Connector is left to you. This may change in the future.
In Node.js/JavaScript this should not be a problem due to the 'callback' style of the language itself.
In Python you will have to protect the calls to the Connector if you are using different threads. For an example, see the Python section's [Protecting calls to the connector library](https://github.com/rticommunity/rticonnextdds-connector/tree/master/examples/python#protecting-calls-to-the-connector-library).

### What is this git repository
This git repository is our way to make Connector available to you!
As of today we included Node.js, Python, and Lua (through RTI Prototyper) for selected
architectures.

Also, for Node.js users, we will use this repository for the npm registry.

### Support
This is an experimental RTI product. As such, we do offer support through the [RTI Community forum](https://community.rti.com/forums/technical-questions) where fellow users and RTI engineers can help you.
We'd love your feedback.

### Documentation
We do not have much documentation yet. But we promise you: if you look at the
examples, you'll see that Connector is very easy to use.

For an overview of the API in JavaScript, check this [page](examples/nodejs/README.md). For the Python version, visit this [page](examples/python/README.md).

For how to access the data sample fields, see Section 6.4 'Data Access API' of the
[RTI Prototyper Getting Started Guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf)  

### Getting started with Node.js
Be sure you have all the tools to work with Node.js. Then invoke:

``` bash
$ npm install rticonnextdds-connector
```

When that is done, cd into your node_modules directory and have a look at the [examples/nodejs/README.md](examples/nodejs/README.md).
### Getting started with Python
Be sure you have Python. Then clone this repository:

``` bash
$ git clone https://github.com/rticommunity/rticonnextdds-connector.git
```

You can also use pip:

``` bash
$ pip install rticonnextdds_connector
```

Or, you can download the [zip file](https://github.com/rticommunity/rticonnextdds-connector/archive/master.zip)
and unzip it.

When that is done, cd into your new directory and have a look at the [examples/python/README.md](examples/python/README.md).

### License
With the sole exception of the contents of the "examples" subdirectory, all use of this product is subject to the RTI Software License Agreement included at the top level of this repository. Files within the "examples" subdirectory are licensed as marked within the file.

This software is an experimental ("pre-production") product. The Software is provided "as is." with no warranty of any type, including any warranty for fitness for any purpose. RTI is under no obligation to maintain or support the software. RTI shall not be liable for any incidental or consequential damages arising out of the use or inability to use the software.
