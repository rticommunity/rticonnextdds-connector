rticonnextdds-connector
=======

### RTI Connext DDS Connector
RTI Connext DDS Connector, or shortly `RTI Connector', is a quick and easy
way to access the power and the functionalities of [RTI Connext DDS](http://www.rti.com/products/index.html).
It is based on [XML App Creation](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_XML_AppCreation_GettingStarted.pdf) and Dynamic Data.

It was created by the RTI Research Group to quickly and easily develop demos
and proof of concept. We think that it can be useful for anybody that needs
a quick way to script tests and interact with DDS.

It can be used to quickly create tests for your distributed system and, thanks
to the binding with scripting languages and the use of XML, to easily integrate
with tons of other available technologies.

### Language Support

 * **nodejs/javascript**: we use [libffi](https://github.com/node-ffi/node-ffi) to call our library, but we try to hide
that from you using a nice javascript wrapper.
 * **python**: here we use [ctypes](https://docs.python.org/2/library/ctypes.html) to call our native functions; of course
everything is hidden in a nice python wrapper.
 * **lua**: we have been supporting lua in our [RTI Prototyper](https://community.rti.com/downloads/experimental/rti-prototyper-with-lua) for a while now.
Check more information on our [blog](http://blogs.rti.com/tag/lua/) or on the [Getting Started Guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf). Also, stay tuned: a version that can be used
directily with the standard lua interprerter is coming...
 * **C**: for the native code lovers, we have include files so you can call the
connector API directly in your C app; that's how the RTI Prototyper is
implemented. Just not ready to release yet...

### Platform support
We are building our library for few architectures only. Check them out here. If you need another architecture
feel free to ask on the [community forum](https://community.rti.com/forums/technical-questions)


### What is this git repository
It is our way to make the connector technology available to you!
As of today we included nodejs, python and lua (through RTI Prototyper) for few
architectures.

Also, we will use this repo for the npm registry.

### Support
This is *not* an official RTI product, so is not officially supported. You can
try to get help writing on the [community forum](https://community.rti.com/forums/technical-questions).

### Documentation
We do not have much documentation yet. But we promise you: if you look at the
examples you'll see that is very easy to use our Connector.

We got documentation on out to access the data sample fields in Section 6.4 'Data Access API' of the
[RTI Prototyper Getting Started Guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf)  

### Getting started with nodejs
Be sure you have all the tools to work with nodejs. Then invoke:

``` bash
$ npm install dds
```

When that is done, cd into your node_modules directory and have a look to the [examples/nodejs/README.md](examples/nodejs/README.md)
### Getting started with python
Be sure you have python (<3). Then clone this repo:

``` bash
$ git clone https://github.com/rticommunity/rticonnextdds-connector.git
```

You can also download the [zip file](https://github.com/rticommunity/rticonnextdds-connector/archive/master.zip) 
and then unzip it.

When that is done, cd into your new directory and have a look to the [examples/python/README.md](examples/python/README.md)

### License:
With the sole exception of the contents of the "examples" subdirectory, all use
of this product is subject to the RTI Software License Agreement included at
the top level of this repository.  
Files within the "examples" subdirectory are licensed as marked within the file.

This software is an experimental (aka "pre-production") product.  
The Software is provided "as is", with no warranty of any type, including any
warranty for fitness for any purpose. RTI is under no obligation to maintain or
support the Software.
RTI shall not be liable for any incidental or consequential damages arising out
of the use or inability to use the software.
