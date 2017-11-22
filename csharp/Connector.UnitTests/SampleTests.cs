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
    using System.Linq;
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

            writer.Instance.Set("x", 3);
            writer.Instance.Set("y", 4);
            writer.Instance.Set("color", "BLUE");
            writer.Instance.Set("hidden", true);
            writer.Write();

            reader.WaitForSamples(1000);
            reader.Take();
            samples = reader.Samples;
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
            Assert.AreEqual(1, samples.Count);
        }

        [Test]
        public void SampleIteratorContainsOneSample()
        {
            int count = 0;
            foreach (var sample in samples)
                count++;
            Assert.AreEqual(1, count);
        }

        // This is just for coverage, all IEnumerable<T> implementations
        // have also the non-generic version, we are testing it here:
        // Helper extension method
        static IEnumerable NonGenericSampleEnumerable(IEnumerable samples)
        {
            foreach (object sample in samples)
                yield return sample;
        }

        [Test]
        public void ListContainsJustOneSampleWithNormalEnumerator()
        {
            var list = NonGenericSampleEnumerable(samples);
            Assert.AreEqual(1, list.Cast<Sample>().Count());
        }

        [Test]
        public void RecievedSampleIsValid()
        {
            Assert.IsTrue(samples.Single().IsValid);
        }

        [Test]
        public void ReceivedSampleHasValidIntFields()
        {
            Sample sample = samples.Single();
            Assert.AreEqual(3, sample.GetInt("x"));
            Assert.AreEqual(4, sample.GetInt("y"));
        }

        [Test]
        public void ReceivedSampleHasValidStringField()
        {
            Sample sample = samples.Single();
            Assert.AreEqual("BLUE", sample.GetString("color"));
        }

        [Test]
        public void ReceivedSampleHasValidBoolField()
        {
            Sample sample = samples.Single();
            Assert.AreEqual(true, sample.GetBool("hidden"));
        }

        [Test]
        public void GetGenericReturnsValidValues()
        {
            Sample sample = samples.Single();
            Assert.AreEqual(3, sample.Get<int>("x"));
            Assert.AreEqual(4, sample.Get<int>("y"));
            Assert.AreEqual("BLUE", sample.Get<string>("color"));
            Assert.AreEqual(true, sample.Get<bool>("hidden"));
        }

        [Test]
        public void GetGenericWithInvalidFormatThrowException()
        {
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
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetInt("fakeInt"));
            Assert.DoesNotThrow(() => sample.GetString("fakeString"));
            Assert.DoesNotThrow(() => sample.GetBool("fakeBool"));
        }

        [Test]
        public void GetWrongVariableTypeForIntsDoesNotThrowException()
        {
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetInt("color"));
            Assert.DoesNotThrow(() => sample.GetInt("hidden"));
            Assert.DoesNotThrow(() => sample.GetInt("angle"));
            Assert.DoesNotThrow(() => sample.GetInt("fillKind"));
        }

        [Test]
        public void GetWrongVariableTypeForStringsDoesNotThrowException()
        {
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetString("x"));
            Assert.DoesNotThrow(() => sample.GetString("hidden"));
            Assert.DoesNotThrow(() => sample.GetString("angle"));
            Assert.DoesNotThrow(() => sample.GetString("fillKind"));
        }

        [Test]
        public void GetWrongVariableTypeForBoolsDoesNotThrowException()
        {
            Sample sample = samples.Single();
            Assert.DoesNotThrow(() => sample.GetBool("x"));
            Assert.DoesNotThrow(() => sample.GetBool("color"));
            Assert.DoesNotThrow(() => sample.GetBool("angle"));
            Assert.DoesNotThrow(() => sample.GetBool("fillKind"));
        }
    }
}
