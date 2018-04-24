rticonnextdds-connector: Lua/C Integration
========

### Installation and platform support
To compile this example, you will need to clone this repository. You can do that by executing:

```bash
git clone --depth=1 https://github.com/rticommunity/rticonnextdds-connector.git
```

We provide example makefiles in the [make](make/) directory and Windows solution in the [win](win) directory.
To compile the example, you need a copy of [RTI Connext DDS](https://www.rti.com/downloads).
If you are on a Unix-like system, you can compile your example using make:

```bash
cd example\lua_c_integration
make -f make/Makefile.x64Darwin12clang4.1
```

After you compile, set the library path and run the executable.

On Linux:

```bash
export LD_LIBRARY_PATH=../../lib/x64Darwin12clang4.1/
./objs/x64Darwin12clang4.1/main
```

On OS X (Mac):

```bash
export DYLD_LIBRARY_PATH=../../lib/x64Darwin12clang4.1/
./objs/x64Darwin12clang4.1/main
```


### Available example
In this directory, you can find an example on how to use the Lua RTI Connext DDS
Connector from your C application.
See the file [main.c]() and [Alert.lua](), and look at the overview below.

### Lua API
The Lua API used in the example, [Alert.lua](Alert.lua), is available in the [Prototyper Getting Started Guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf).

### Overview:
#### Include the connector library
If you want to use the `lua rticonnextdds-connector` from your C application, include the following header file:

```c
#include "lua_binding/lua_binding_ddsConnector.h"
```

#### Instantiate a new connector
To create a new connector, you have to pass an XML file and a configuration name. For more information on
the XML format, see the [XML Application Creation Getting Started Guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_XML_AppCreation_GettingStarted.pdf) or
have a look to the [Simple.xml](Simple.xml) file included in this directory.  

```c
struct RTIDDSConnector *connector = NULL;
connector = RTIDDSConnector_new(
        "MyParticipantLibrary::Zero",
        "./Simple.xml", NULL);
```

#### Assert the Lua script that will be executed
Once the connector has been created, you have to assert what Lua code will be executed. To do so, use the `RTIDDSConnector_assertCode` API.
This API gets a pointer to the connector, an optional string (which can be NULL) containing the Lua script and an optional string (which can be NULL) with a path to a file containing a Lua script. If both strings are NULL, the connector will not execute any Lua code.

The last parameter is the interval, in seconds, at which point the Lua script is checked for changes. A negative value disables reloads.

```c
RTIDDSConnector_assertCode(connector,NULL,"./Alert.lua",4);
```

#### Pass parameters from C to Lua
The connector offers an API to allow C programs to set values in a Lua table called `CONTEXT`. All you have to do is call the `RTIDDSConnector_set[Number|String|Boolean]IntoContext` API:

```c
RTIDDSConnector_setNumberIntoContext(connector,"temp", temp);
```

#### Execute the Lua script
When ready, your C loop can execute the Lua script asserted before just by calling `RTIDDSConnector_execute`:

```c
RTIDDSConnector_execute(connector);
```

`RTIDDSConnector_execute` will return once the script execution is finished.


#### Delete a connector
To destroy all the DDS entities that belong to a connector previously created, call the ```RTIDDSConnector_delete``` function.

```c
RTIDDSConnector_delete(connector);
```

#### Threading considerations
The Connector is not thread safe. If you wish to call the Connector APIs from different threads, you will have to protect those calls yourself.
