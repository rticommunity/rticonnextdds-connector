// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector.UnitTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class InstanceTests
    {
        Connector connector;
        Writer writer;
        Instance instance;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreatePublisherConnector();
            writer = new Writer(connector, TestResources.WriterName);
            instance = writer.Instance;
        }

        [TearDown]
        public void TearDown()
        {
            writer.Dispose();
            connector.Dispose();
        }

        [Test]
        public void SetNonExistingFieldsDoNotThrowException()
        {
            // You will see warning messages on the console instead
            Assert.DoesNotThrow(() => instance.Set("fakeInt", 3));
            Assert.DoesNotThrow(() => instance.Set("fakeString", "helloworld"));
            Assert.DoesNotThrow(() => instance.Set("fakeBool", false));
        }

        [Test]
        public void ClearDoesNotThrowExceptionWithoutSettingFields()
        {
            Assert.DoesNotThrow(instance.Clear);
        }

        [Test]
        public void ClearDoesNotThrowExceptionAfterSettingFields()
        {
            instance.Set("x", 3);
            Assert.DoesNotThrow(instance.Clear);
        }

        [Test]
        public void ClearDoesNotThrowExceptionAfterInvalidAssignment()
        {
            instance.Set("fakeInt", 4);
            Assert.DoesNotThrow(instance.Clear);

            Assert.DoesNotThrow(() => instance.Set("x", "test"));
            Assert.DoesNotThrow(instance.Clear);

            Assert.DoesNotThrow(() => instance.Set("color", 3));
            Assert.DoesNotThrow(instance.Clear);

            Assert.DoesNotThrow(() => instance.Set("hidden", "test"));
            Assert.DoesNotThrow(instance.Clear);
        }

        [Test]
        public void SetWrongVariableTypeForIntegersDoesNotThrowException()
        {
            Assert.DoesNotThrow(() => instance.Set("x", "test"));
            Assert.DoesNotThrow(() => instance.Set("shapesize", "3"));
            Assert.DoesNotThrow(() => instance.Set("angle", "3.14"));
            Assert.DoesNotThrow(() => instance.Set("x", true));
        }

        [Test]
        public void SetWrongVariableTypeForStringsDoesNotThrowException()
        {
            // You will see warning messages on the console instead
            Assert.DoesNotThrow(() => instance.Set("color", 3));
            Assert.DoesNotThrow(() => instance.Set("color", true));
        }

        [Test]
        public void SetWrongVariableTypeForBoolDoesNotThrowException()
        {
            Assert.DoesNotThrow(() => instance.Set("hidden", "test"));
            Assert.DoesNotThrow(() => instance.Set("hidden", "true"));
            Assert.DoesNotThrow(() => instance.Set("hidden", 0));
        }

        [Test]
        public void SetCorrectTypesDoNotThrowException()
        {
            Assert.DoesNotThrow(() => instance.Set("x", 3));
            Assert.DoesNotThrow(() => instance.Set("color", "BLUE"));
            Assert.DoesNotThrow(() => instance.Set("hidden", false));
        }

        [Test]
        public void SetFieldsAfterDisposingConnectorThrowsException()
        {
            MyClassType sample = new MyClassType {
                color = "test",
                x = 3,
                hidden = true
            };

            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => instance.Set("x", 3));
            Assert.Throws<ObjectDisposedException>(() => instance.Set("color", "BLUE"));
            Assert.Throws<ObjectDisposedException>(() => instance.Set("hidden", false));
            Assert.Throws<ObjectDisposedException>(() => instance.SetFrom(sample));
        }

        [Test]
        public void ClearFieldsAfterDisposingConnectorThrowsException()
        {
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(instance.Clear);
        }

        [Test]
        public void SetClassObjectWithValidTypesDoesNotThrowException()
        {
            MyClassType sample = new MyClassType {
                color = "test",
                x = 3,
                hidden = true
            };
            Assert.DoesNotThrow(() => instance.SetFrom(sample));
            Assert.DoesNotThrow(writer.Write);
        }

        [Test]
        public void SetStructObjectWithValidTypesDoesNotThrowException()
        {
            MyStructType sample = new MyStructType {
                color = "test",
                x = 3,
                hidden = true
            };
            Assert.DoesNotThrow(() => instance.SetFrom(sample));
            Assert.DoesNotThrow(writer.Write);
        }

        [Test]
        public void SetObjectWithAnonymousTypesDoesNotThrowException()
        {
            var sample = new {
                color = "test",
                x = 3,
                hidden = true
            };
            Assert.DoesNotThrow(() => instance.SetFrom(sample));
            Assert.DoesNotThrow(writer.Write);
        }

        [Test]
        public void SetDictionaryWithValidTypesDoesNotThrowException()
        {
            var sample = new Dictionary<string, object> {
                { "color", "test" },
                { "x", 3 },
                { "hidden", true }
            };

            Assert.DoesNotThrow(() => instance.SetFrom(sample));
            Assert.DoesNotThrow(writer.Write);
        }

        [Test]
        public void SetClassObjectWithInvalidTypesDoesNotThrowException()
        {
            MyInvalidClassType sample = new MyInvalidClassType {
                color = 4,
                x = 3.3,
            };
            Assert.DoesNotThrow(() => instance.SetFrom(sample));
            Assert.DoesNotThrow(writer.Write);
        }

        [Test]
        public void SetClassObjectWithMissingTypesDoesNotThrowException()
        {
            MyFakeFieldsTypes sample = new MyFakeFieldsTypes {
                color = "test",
                Fake = 3,
            };
            Assert.DoesNotThrow(() => instance.SetFrom(sample));
            Assert.DoesNotThrow(writer.Write);
        }
    }
}
