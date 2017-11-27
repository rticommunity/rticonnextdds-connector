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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using NUnit.Framework;

    // Since we are sending and waiting for samples in the same domain and topic
    // we need to run these test sequentially.
    [TestFixture, SingleThreaded]
    public class SampleTests
    {
        Connector connector;
        Writer writer;
        Reader reader;
        SampleCollection samples;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreatePubSubConnector();
            writer = new Writer(connector, TestResources.WriterName);
            reader = new Reader(connector, TestResources.ReaderName);
            samples = reader.Samples;

            // Wait for discovery
            Thread.Sleep(100);
        }

        [TearDown]
        public void TearDown()
        {
            reader.Dispose();
            writer.Dispose();
            connector.Dispose();
        }

        [Test]
        public void GetNumberOfSamplesReturnsValidValue()
        {
            SendAndTakeOrReadStandardSample(true);
            Assert.AreEqual(1, samples.Count);
        }

        [Test]
        public void SampleIteratorContainsOneSample()
        {
            SendAndTakeOrReadStandardSample(true);
            int count = 0;
            foreach (var sample in samples)
                count++;
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ListContainsJustOneSampleWithNormalEnumerator()
        {
            SendAndTakeOrReadStandardSample(true);
            var list = NonGenericSampleEnumerable(samples);
            Assert.AreEqual(1, list.Cast<Sample>().Count());
        }

        [Test]
        public void RecievedSampleIsValid()
        {
            SendAndTakeOrReadStandardSample(true);
            Assert.IsTrue(samples.Single().IsValid);
        }

        [Test]
        public void ReceivedSampleHasValidIntFields()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.AreEqual(3, sample.GetInt("x"));
            Assert.AreEqual(4, sample.GetInt("y"));
        }

        [Test]
        public void ReceivedSampleHasValidStringField()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.AreEqual("BLUE", sample.GetString("color"));
        }

        [Test]
        public void ReceivedSampleHasValidBoolField()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.AreEqual(true, sample.GetBool("hidden"));
        }

        [Test]
        public void GetGenericReturnsValidValues()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.AreEqual(3, sample.Get<int>("x"));
            Assert.AreEqual(4, sample.Get<int>("y"));
            Assert.AreEqual("BLUE", sample.Get<string>("color"));
            Assert.AreEqual(true, sample.Get<bool>("hidden"));
        }

        [Test]
        public void GetGenericWithInvalidFormatThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.Throws<FormatException>(() => sample.Get<double>("x"));
            Assert.Throws<FormatException>(() => sample.Get<decimal>("x"));
            Assert.Throws<FormatException>(() => sample.Get<float>("x"));
            Assert.Throws<FormatException>(() => sample.Get<byte>("x"));
            Assert.Throws<FormatException>(() => sample.Get<sbyte>("x"));
            Assert.Throws<FormatException>(() => sample.Get<ushort>("x"));
            Assert.Throws<FormatException>(() => sample.Get<short>("x"));
            Assert.Throws<FormatException>(() => sample.Get<uint>("x"));
            Assert.Throws<FormatException>(() => sample.Get<long>("x"));
            Assert.Throws<FormatException>(() => sample.Get<ulong>("x"));
        }

        [Test]
        public void GetNonExistingFieldsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetInt("fakeInt"));
            Assert.DoesNotThrow(() => sample.GetString("fakeString"));
            Assert.DoesNotThrow(() => sample.GetBool("fakeBool"));
        }

        [Test]
        public void GetWrongVariableTypeForIntsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetInt("color"));
            Assert.DoesNotThrow(() => sample.GetInt("hidden"));
            Assert.DoesNotThrow(() => sample.GetInt("angle"));
            Assert.DoesNotThrow(() => sample.GetInt("fillKind"));
        }

        [Test]
        public void GetWrongVariableTypeForStringsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetString("x"));
            Assert.DoesNotThrow(() => sample.GetString("hidden"));
            Assert.DoesNotThrow(() => sample.GetString("angle"));
            Assert.DoesNotThrow(() => sample.GetString("fillKind"));
        }

        [Test]
        public void GetWrongVariableTypeForBoolsDoesNotThrowException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetBool("x"));
            Assert.DoesNotThrow(() => sample.GetBool("color"));
            Assert.DoesNotThrow(() => sample.GetBool("angle"));
            Assert.DoesNotThrow(() => sample.GetBool("fillKind"));
        }

        [Test]
        public void TakeRemovesSamples()
        {
            SendAndTakeOrReadStandardSample(true);
            Assert.AreEqual(1, samples.Count);
            reader.Take();
            Assert.AreEqual(0, samples.Count);
        }

        [Test]
        public void ReadDoesNotRemoveSamples()
        {
            SendAndTakeOrReadStandardSample(false);
            Assert.AreEqual(1, samples.Count);
            reader.Read();
            Assert.AreEqual(1, samples.Count);
            Assert.AreEqual(4, samples.First().GetInt("y"));
        }

        [Test]
        public void TakeAfterReadRemovesSample()
        {
            SendAndTakeOrReadStandardSample(false);
            Assert.AreEqual(1, samples.Count);
            reader.Take();
            Assert.AreEqual(1, samples.Count);
            reader.Take();
            Assert.AreEqual(0, samples.Count);
        }

        [Test]
        public void GetValidObjectSample()
        {
            MyClassType obj = new MyClassType {
                color = "test",
                x = 3,
                hidden = true };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            MyClassType received = sample.GetAs<MyClassType>();

            Assert.AreEqual("test", received.color);
            Assert.AreEqual(3, received.x);
            Assert.AreEqual(true, received.hidden);
        }

        [Test]
        public void GetValidStructSample()
        {
            MyStructType obj = new MyStructType {
                color = "test",
                x = 3,
                hidden = true
            };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            MyStructType received = sample.GetAs<MyStructType>();

            Assert.AreEqual("test", received.color);
            Assert.AreEqual(3, received.x);
            Assert.AreEqual(true, received.hidden);
        }

        [Test]
        public void SendClassAndReceiveStructSample()
        {
            MyStructType obj = new MyStructType {
                color = "test",
                x = 3,
                hidden = true
            };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            MyClassType received = sample.GetAs<MyClassType>();

            Assert.AreEqual("test", received.color);
            Assert.AreEqual(3, received.x);
            Assert.AreEqual(true, received.hidden);
        }

        [Test]
        public void SendAnonymousAndReceiveObjectSample()
        {
            var obj = new {
                color = "test",
                x = 3,
                hidden = true
            };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            dynamic received = sample.GetAsObject();

            Assert.AreEqual("test", received.color.Value);
            Assert.AreEqual(3, received.x.Value);
            Assert.AreEqual(true, received.hidden.Value);
        }

        [Test]
        public void SendDictionaryAndReceiveSample()
        {
            var obj = new Dictionary<string, object> {
                { "color", "test" },
                { "x", 3 },
                { "hidden", true }
            };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            var received = sample.GetAs<Dictionary<string, object>>();

            Assert.AreEqual("test", received["color"]);
            Assert.AreEqual(3, received["x"]);
            Assert.AreEqual(true, received["hidden"]);
        }

        [Test]
        public void GetCompleClassSample()
        {
            ComplexType obj = new ComplexType {
                color = "test",
                x = 3,
                angle = 3.14f,
                hidden = true,
                list = new[] { 0, 1, 2, 3, 4 },
                inner = new ComplexType.Inner { z = -1 }
            };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            ComplexType received = sample.GetAs<ComplexType>();

            Assert.AreEqual("test", received.color);
            Assert.AreEqual(3, received.x);
            Assert.AreEqual(true, received.hidden);
            Assert.AreEqual(3.14f, received.angle);
            Assert.AreEqual(5, received.list.Length);
            Assert.AreEqual(3, received.list[3]);
            Assert.AreEqual(-1, received.inner.z);
        }

        [Test]
        public void GetClassWithInvalidFieldsThrowsException()
        {
            MyInvalidClassType obj = new MyInvalidClassType {
                color = 3,
                x = 3.3,
            };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            Assert.Throws<Newtonsoft.Json.JsonSerializationException>(
                () => sample.GetAs<MyInvalidClassType>());
        }

        [Test]
        public void GetClassWithMissingFieldsIsEmpty()
        {
            MyFakeFieldsTypes obj = new MyFakeFieldsTypes {
                color = "blue",
                x = 3,
                Fake = 4,
            };

            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            MyFakeFieldsTypes received = sample.GetAs<MyFakeFieldsTypes>();

            Assert.AreEqual("blue", received.color);
            Assert.AreEqual(3, received.x);
            Assert.AreEqual(0, received.Fake);
        }

        [Test]
        public void GetNumberSamplesAfterDisposingConnectorThrowsException()
        {
            SendAndTakeOrReadStandardSample(true);
            connector.Dispose();
            int count = 0;
            Assert.Throws<ObjectDisposedException>(() => count = samples.Count);
            Assert.Throws<ObjectDisposedException>(() => count = samples.Count());
            Assert.Throws<ObjectDisposedException>(() => samples.Single());
        }

        [Test]
        public void GetFieldsAfterDisposingConnectorThrowsException()
        {
            SendAndTakeOrReadStandardSample(true);
            Sample sample = samples.Single();
            bool validSample = false;
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => sample.Get<int>("x"));
            Assert.Throws<ObjectDisposedException>(() => sample.Get<string>("color"));
            Assert.Throws<ObjectDisposedException>(() => sample.Get<bool>("hidden"));
            Assert.Throws<ObjectDisposedException>(() => validSample = sample.IsValid);
        }

        [Test]
        public void GetJsonSampleAfterDisposingConnectorThrowsException()
        {
            MyClassType obj = new MyClassType {
                color = "test",
                x = 3,
                hidden = true
            };
            SendAndTakeOrReadObjectSample(obj, true);
            Sample sample = samples.Single();
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => sample.GetAs<MyStructType>());
        }

        [Test]
        public void SetInstanceFieldDoesNotCleanJsonObj()
        {
            writer.Instance.Set("shapesize", 10);
            MyClassType obj = new MyClassType {
                color = "test",
                x = 3,
                hidden = true
            };
            writer.Instance.SetFrom(obj);
            writer.Instance.Set("x", 5);
            writer.Instance.Set("y", 4);
            writer.Write();

            Assert.IsTrue(connector.WaitForSamples(1000));
            reader.Take();
            Sample sample = samples.Single();

            Assert.AreEqual("test", sample.Get<string>("color"));
            Assert.AreEqual(5, sample.Get<int>("x"));
            Assert.AreEqual(true, sample.Get<bool>("hidden"));
            Assert.AreEqual(4, sample.Get<int>("y"));
            Assert.AreEqual(10, sample.Get<int>("shapesize"));
        }

        [Test]
        public void SetJsonObjDoesNotResetInstance()
        {
            writer.Instance.Set("shapesize", 10);
            writer.Instance.Set("x", 5);
            writer.Instance.Set("y", 4);
            MyClassType obj = new MyClassType {
                color = "test",
                x = 3,
                hidden = true
            };
            writer.Instance.SetFrom(obj);
            writer.Write();

            Assert.IsTrue(connector.WaitForSamples(1000));
            reader.Take();
            Sample sample = samples.Single();

            Assert.AreEqual("test", sample.Get<string>("color"));
            Assert.AreEqual(3, sample.Get<int>("x"));
            Assert.AreEqual(true, sample.Get<bool>("hidden"));
            Assert.AreEqual(4, sample.Get<int>("y"));
            Assert.AreEqual(10, sample.Get<int>("shapesize"));
        }

        [Test]
        public void SendObjectFromWrite()
        {
            MyClassType obj = new MyClassType {
                color = "test",
                x = 3,
                hidden = true
            };

            writer.Write(obj);
            Assert.IsTrue(connector.WaitForSamples(1000));
            reader.Take();
            Sample sample = samples.Single();

            Assert.AreEqual("test", sample.Get<string>("color"));
            Assert.AreEqual(3, sample.Get<int>("x"));
            Assert.AreEqual(true, sample.Get<bool>("hidden"));
        }

        [Test]
        public void SendObjectFromWriteCleansDefaultInstance()
        {
            writer.Instance.Set("shapesize", 10);
            writer.Instance.Set("x", 5);
            writer.Instance.Set("y", 4);
            MyClassType obj = new MyClassType {
                color = "test",
                x = 3,
                hidden = true
            };

            writer.Write(obj);
            Assert.IsTrue(connector.WaitForSamples(1000));
            reader.Take();
            Sample sample = samples.Single();

            Assert.AreEqual("test", sample.Get<string>("color"));
            Assert.AreEqual(3, sample.Get<int>("x"));
            Assert.AreEqual(true, sample.Get<bool>("hidden"));
            Assert.AreEqual(0, sample.Get<int>("y"));
            Assert.AreEqual(0, sample.Get<int>("shapesize"));
        }

        // This is just for coverage, all IEnumerable<T> implementations
        // have also the non-generic version, we are testing it here:
        // Helper extension method
        static IEnumerable NonGenericSampleEnumerable(IEnumerable samples)
        {
            foreach (object sample in samples)
                yield return sample;
        }

        void SendAndTakeOrReadStandardSample(bool take)
        {
            writer.Instance.Set("x", 3);
            writer.Instance.Set("y", 4);
            writer.Instance.Set("color", "BLUE");
            writer.Instance.Set("hidden", true);
            writer.Write();

            Assert.IsTrue(connector.WaitForSamples(1000));
            if (take)
                reader.Take();
            else
                reader.Read();
        }

        void SendAndTakeOrReadObjectSample(object obj, bool take)
        {
            writer.Instance.SetFrom(obj);
            writer.Write();

            Assert.IsTrue(connector.WaitForSamples(1000));
            if (take)
                reader.Take();
            else
                reader.Read();
        }
    }
}
