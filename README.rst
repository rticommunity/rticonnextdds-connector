rticonnextdds-connector
=======================

RTI Connector for Connext DDS
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

RTI Connector for Connext DDS is a quick and easy way to access the
power and functionality of `RTI Connext
DDS <http://www.rti.com/products/index.html>`__. It is based on `XML App
Creation <https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_XML_AppCreation_GettingStarted.pdf>`__
and Dynamic Data.

RTI Connector was created by the RTI Research Group to quickly and
easily develop demos and proof of concept. We think that it can be
useful for anybody that needs a quick way to script tests and interact
with DDS using different scripting languages.

It can be used to quickly create tests for your distributed system and,
thanks to the binding with scripting languages and the use of XML, to
easily integrate with tons of other available technologies.

The RTI Connector library is provided in binary form for selected
architectures. Scripting language bindings and examples are provided in
source format.

Language Support
~~~~~~~~~~~~~~~~

-  **nodejs/javascript**: we use
   `libffi <https://github.com/node-ffi/node-ffi>`__ to call our
   library, but we try to hide that from you using a nice JavaScript
   wrapper.
-  **python**: here we use
   `ctypes <https://docs.python.org/2/library/ctypes.html>`__ to call
   our native functions; of course everything is hidden in a nice Python
   wrapper.
-  **lua**: we have been supporting Lua in our `RTI
   Prototyper <https://community.rti.com/downloads/experimental/rti-prototyper-with-lua>`__
   for a while now. Check more information on our
   `blog <http://blogs.rti.com/tag/lua/>`__ or on the `Getting Started
   Guide <https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf>`__.
   Also, stay tuned: a version that can be used directly with the
   standard Lua interprerter is coming...
-  **C**: for the native code lovers, we have header files so you can
   call the RTI Connector API directly in your C app; that's how the RTI
   Prototyper is implemented. Just not ready to release yet...

Platform support
~~~~~~~~~~~~~~~~

We are building our library for few architectures only. Check them out
`here <https://github.com/rticommunity/rticonnextdds-connector/tree/master/lib>`__.
If you need another architecture.

**Windows Note**: For nodejs, we tested on Win10 64 bit. We notice that
npm works best with VS Express 2013 Feel free to ask on the `RTI
Community
Forum <https://community.rti.com/forums/technical-questions>`__ for more
details.

If you want to check the version of the libraries you can run the
following command:

.. code:: bash

    strings librtiddsconnector.dylib | grep BUILD

Threading model
~~~~~~~~~~~~~~~

The RTI Connext DDS Connector Native API do not yet implement any
mechanism for thread safety. Originally the Connector native code was
built to work with RTI DDS Prototyper and Lua. That was a single
threaded loop. We then introduced support for javascript and python. For
now the responsibility of protecting calls to the Connector are left to
the user. This may change in the future. In node/javascript this should
not be a problem due to the 'callback' style of the language itself. In
python you will have to protect the calls to the connector if you are
using different threads. For an example, check the python section
`Protecting calls to the connector
library <https://github.com/rticommunity/rticonnextdds-connector/tree/master/examples/python#protecting-calls-to-the-connector-library>`__.

What is this git repository
~~~~~~~~~~~~~~~~~~~~~~~~~~~

It is our way to make the connector technology available to you! As of
today we included Node.js, Python and Lua (through RTI Prototyper) for
few architectures.

Also, for Node.js users, we will use this repo for the npm registry.

Support
~~~~~~~

This is an experimental RTI product. As such we do offer support through
the `RTI Community
Forum <https://community.rti.com/forums/technical-questions>`__ where
fellow users and RTI engineers can help you. We'd love your feedback.

Documentation
~~~~~~~~~~~~~

We do not have much documentation yet. But we promise you: if you look
at the examples you'll see that is very easy to use our connector.

For an overview of the API in JavaScript check this
`page <examples/nodejs/README.md>`__. For the Python version visit this
`one <examples/python/README.md>`__.

We have documentation on how to access the data sample fields in Section
6.4 'Data Access API' of the `RTI Prototyper Getting Started
Guide <https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf>`__

Getting started with nodejs
~~~~~~~~~~~~~~~~~~~~~~~~~~~

Be sure you have all the tools to work with nodejs. Then invoke:

.. code:: bash

    $ npm install rticonnextdds-connector

When that is done, cd into your node\_modules directory and have a look
to the `examples/nodejs/README.md <examples/nodejs/README.md>`__ ###
Getting started with python Be sure you have python (<3). Then clone
this repo:

.. code:: bash

    $ git clone https://github.com/rticommunity/rticonnextdds-connector.git

You can also download the `zip
file <https://github.com/rticommunity/rticonnextdds-connector/archive/master.zip>`__
and then unzip it.

When that is done, cd into your new directory and have a look to the
`examples/python/README.md <examples/python/README.md>`__

License
~~~~~~~

With the sole exception of the contents of the "examples" subdirectory,
all use of this product is subject to the RTI Software License Agreement
included at the top level of this repository. Files within the
"examples" subdirectory are licensed as marked within the file.

This software is an experimental (aka "pre-production") product. The
Software is provided "as is", with no warranty of any type, including
any warranty for fitness for any purpose. RTI is under no obligation to
maintain or support the Software. RTI shall not be liable for any
incidental or consequential damages arising out of the use or inability
to use the software.
