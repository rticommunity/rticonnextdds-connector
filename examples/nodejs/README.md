rticonnextdds-connector: nodejs/javascript examples
========

### Installation and Platform support
Check [here](https://github.com/gianpiero/rticonnextdds-connector#getting-started-with-nodejs) and [here](https://github.com/gianpiero/rticonnextdds-connector#platform-support).
If you still have trouble write on the [forum](https://community.rti.com/forums/technical-questions)

### API Overview:
#### require the connector library
If you want to use the `rticonnextdds-connector` you have to require it:

```js
var rti = require('rticonnextdds-connector');
```

#### instantiate a new connector
To create a new connector you have to pass an xml file and a configuation name. For more information on
the XML format check the [XML App Creation guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_XML_AppCreation_GettingStarted.pdf) or
have a look to the [ShapeExample.xml](ShapeExample.xml) file included in this directory.  

```js
var connector = new rti.Connector("MyParticipantLibrary::Zero","./ShapeExample.xml");
```

#### write a sample
To write a sample first we have to get a reference to the output port:

```js
var output = connector.getOutput("MyPublisher::MySquareWriter");
```

then we have to set the instance's fields:

```js
output.instance.setNumber("x",i);
output.instance.setNumber("y",i*2);
output.instance.setNumber("shapesize",30);
output.instance.setString("color", "BLUE");
```

and then we can write:

```js
output.write();
```

#### setting the instance's fields:
The content of an instance can be set in two ways:

 * **Field by field**:

```js
output.instance.setNumber("y",i*2);
```

The APIs to do that are only 3: `setNumber(fieldName, number);` `setBoolean(fieldName, boolean);` and `setString(fieldName, string);`.

Nested fields can be accessed with the dot notation: `"x.y.z"`, and array with square brakets: `"x.y[1].z"`. For more info on how to access
fields, check Section 6.4 'Data Access API' of the
[RTI Prototyper Getting Started Guide](https://community.rti.com/rti-doc/510/ndds.5.1.0/doc/pdf/RTI_CoreLibrariesAndUtilities_Prototyper_GettingStarted.pdf)


 * **Passing a JSON object**:

```js
output.setFromJSON(jsonObj)
```

**WARNING**: This second way is not well supported/tested. For example array and sequences are not supported yet.

#### reading/taking data
To read/take samples first we have to get a reference to the input port:

```js
var input = connector.getInput("MySubscriber::MySquareReader");
```

then we can call the `read()` or `take()` API:

```js
output.read();
```

 or

 ```js
 output.take();
 ```

The read/take operation can return multiple samples. So we have to iterate on an array:

```js
for (i=1; i <= input.samples.getLength(); i++) {
  if (input.infos.isValid(i)) {
    console.log(JSON.stringify(input.samples.getJSON(i)));
  }
}
```

#### accessing samples fields after a read/take
A `read()` or `take()` operation can return multiple samples. They are stored in an array.

We can access them in two ways:

 * **Field by field**:

 ```js
 for (i=1; i <= input.samples.getLength(); i++) {
   if (input.infos.isValid(i)) {
     console.log(input.samples.getNumber(i, "x"));
   }
 }
 ```

 The APIs to do that are only 3: `getNumber(indexm fieldName);` `getBoolean(index, fieldName);` and `getString(index, fieldName);`.

 * **as a JSON object**:

 ```js
 for (i=1; i <= input.samples.getLength(); i++) {
   if (input.infos.isValid(i)) {
     console.log(JSON.stringify(input.samples.getJSON(i)));
   }
 }
 ```
