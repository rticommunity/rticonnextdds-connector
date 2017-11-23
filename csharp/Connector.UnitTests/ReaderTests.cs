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
    using System.Runtime.InteropServices;
    using NUnit.Framework;

    [TestFixture]
    public class ReaderTests
    {
        Connector connector;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreateSubscriberConnector();
        }

        [TearDown]
        public void TearDown()
        {
            connector?.Dispose();
        }

        [Test]
        public void ConstructorWithNullConnectorThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Reader(null, TestResources.ReaderName));
        }

        [Test]
        public void ConstructorWithNullOrEmptyEntityNameThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Reader(connector, null));
            Assert.Throws<ArgumentNullException>(
                () => new Reader(connector, string.Empty));
        }

        [Test]
        public void ConstructorWithMissingEntityNameThrowsException()
        {
            Assert.Throws<COMException>(
                () => new Reader(connector, "FakeSubscriber::MySquareReader"));
            Assert.Throws<COMException>(
                () => new Reader(connector, "MySubscriber::FakeReader"));
        }

        [Test]
        public void ConstructorWithValidConfigIsSuccessful()
        {
            Reader reader = null;
            Assert.DoesNotThrow(
                () => reader = new Reader(connector, TestResources.ReaderName));
            Assert.IsNotNull(reader.InternalReader);
        }

        [Test]
        public void ConstructorSetsProperties()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            Assert.AreEqual(TestResources.ReaderName, reader.Name);
            Assert.IsNotNull(reader.Samples);
        }

        [Test]
        public void DisposeChangesProperty()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            Assert.IsFalse(reader.Disposed);
            reader.Dispose();
            Assert.IsTrue(reader.Disposed);
        }

        [Test]
        public void DisposeDoesNotDisposeConnector()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            reader.Dispose();
            Assert.IsTrue(reader.Disposed);
            Assert.IsFalse(connector.Disposed);
        }

        [Test]
        public void DisposingTwiceDoesNotThrowException()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            reader.Dispose();
            Assert.DoesNotThrow(reader.Dispose);
            Assert.IsTrue(reader.Disposed);
        }

        [Test]
        public void ReadDoesNotThrowException()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            Assert.DoesNotThrow(reader.Read);
        }

        [Test]
        public void ReadAfterDisposeThrowsException()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            reader.Dispose();
            Assert.Throws<ObjectDisposedException>(reader.Read);
        }

        [Test]
        public void ReadAfterDisposingConnectorThrowsException()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(reader.Read);
        }

        [Test]
        public void TakeDoesNotThrowException()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            Assert.DoesNotThrow(reader.Take);
        }

        [Test]
        public void TakeAfterDisposeThrowsException()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            reader.Dispose();
            Assert.Throws<ObjectDisposedException>(reader.Take);
        }

        [Test]
        public void TakeAfterDisposingConnectorThrowsException()
        {
            Reader reader = new Reader(connector, TestResources.ReaderName);
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(reader.Take);
        }
    }
}
