# RTI Connext DDS Connector: C#/.NET

## Installation and Platform support
Check [here](https://github.com/rticommunity/rticonnextdds-connector#getting-started-with-net) and [here](https://github.com/rticommunity/rticonnextdds-connector#platform-support).
If you still have trouble write on the [RTI Community Forum](https://community.rti.com/forums/technical-questions)

### Available examples
In this directory you can find 1 set of examples

 * **simple**: shows how to write samples and how to read/take.
 * **mixed**: shows how to write and read samples from complex types like sequences and inner structures.
 * **objects**: shows how to write and read samples mapped from C# objects like *classes* and *structs*.

### API Overview:
#### Import the RTI Connector library
Import the `librtiddsconnector_dotnet.dll` library to start using *Connector*. The API is under the namespace `RTI.Connector`.

#### Instantiate a new Connector
To create a new *Connector* you have to pass the path to an XML file and a configuration name. For more information on
the XML format check the [XML App Creation guide](https://community.rti.com/static/documentation/connext-dds/5.2.3/doc/manuals/connext_dds/xml_application_creation/RTI_ConnextDDS_CoreLibraries_XML_AppCreation_GettingStarted.pdf) or
have a look to the [ShapeExample.xml](ShapeExample.xml) file included in this directory.

```csharp
Connector = connector = new Connector("MyParticipantLibrary::Zero", "ShapeExample.xml");
```

#### Delete a Connector
To destroy all the DDS entities that belong to a *Connector* previously created, you can call the `Dispose` method. Another option is to use the `using` statement. This is a safer approach since in case of exception it guarantees that the object will be disposed.

```csharp
using (Connector connector = new Connector(configName, configPath)) {
    // Do stuff with Connector

    // Before going out of the scope, connector.Dispose() is automatically called
}
```

#### Write a sample
To write a sample first we need to create a *Writer* by passing the *Connector* object and the writer name as it's defined in the XML configuration.

```csharp
Writer writer = new Writer(connector, "MyPublisher::MySquareWriter");
```

Then we can start using the *Instance* associated to this *Writer* and set its fields.

```csharp
Instance instance = writer.Instance;
// There are three overloads for 'Set' for int, string and bool types
instance.Set("x", 1);
instance.Set("color", "BLUE");
instance.Set("flag", true);
```

and finally, we can write the instance:

```csharp
writer.Write();
```

Alternative, we can fill the *Instance* fields from a C# object like a *class* or *struct*. To do so, we can use the `SetFrom` method from the `Instance` class:

```csharp
// Set some values to the instance
instance.Set("x", 2);
instance.Set("y", 3);

// Overwrite and set values from this object
MyClass obj = new MyClass {
    x = 1,
    color = "BLUE"
};
instance.SetFrom(obj);

// write
writer.Write();
```

or write directly the object with the `Write(obj)` method from the `Writer`:
```csharp
MyClass obj = new MyClass {
    x = 1,
    color = "BLUE"
};

writer.Write(obj);
```

#### Reading samples
To read samples we need to get the *Reader* defined in the configuration.

```csharp
Reader reader = new Reader(connector, "MySubscriber::MySquareReader");
```

Then we can retrieve the samples by calling the `Read` or `Take` methods. The former will keep the samples in the internal queue and the latter will remove them.

To access to the samples we can use the `Samples` property from the *Reader*. This is an `IEnumerable<Sample>` type so we can iterate over them.

```csharp
// Read samples
reader.Take();
Console.WriteLine("Received {0} samples", reader.Samples.Count);

// Iterate over the read samples
foreach (Sample sample in reader.Samples) {
    if (sample.IsValid) {
        // This sample contains user data.
        // You can get the fields with the GetInt, GetString and GetBool methods.
        int x = sample.GetInt("x");
        string color = sample.GetString("color");

        // Or by using the Get<T> method
        int y = sample.Get<int>("y");
    } else {
        // This is a metadata sample.
        Console.WriteLine("Received metadata");
    }
}
```

We can also convert the sample into a C# object by using the `GetAs<T>` and `GetAsObject` methods of the sample:
```csharp
MyClass sample = sample.GetAs<MyClass>();
int x = sample.x;
string color = sample.color;
```
