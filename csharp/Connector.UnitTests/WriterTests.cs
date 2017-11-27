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
    public class WriterTests
    {
        Connector connector;

        [SetUp]
        public void SetUp()
        {
            connector = TestResources.CreatePublisherConnector();
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
                () => new Writer(null, TestResources.WriterName));
        }

        [Test]
        public void ConstructorWithNullOrEmptyEntityNameThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Writer(connector, null));
            Assert.Throws<ArgumentNullException>(
                () => new Writer(connector, string.Empty));
        }

        [Test]
        public void ConstructorWithMissingEntityNameThrowsException()
        {
            Assert.Throws<COMException>(
                () => new Writer(connector, "FakePublisher::MySquareWriter"));
            Assert.Throws<COMException>(
                () => new Writer(connector, "MyPublisher::FakeWriter"));
        }

        [Test]
        public void ConstructorWithValidConfigIsSuccessful()
        {
            Writer writer = null;
            Assert.DoesNotThrow(
                () => writer = new Writer(connector, TestResources.WriterName));
            Assert.IsNotNull(writer.InternalWriter);
        }

        [Test]
        public void ConstructorSetsProperties()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            Assert.AreEqual(TestResources.WriterName, writer.Name);
            Assert.IsNotNull(writer.Instance);
        }

        [Test]
        public void WriteAfterDisposeThrowsException()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(writer.Write);
        }

        [Test]
        public void WriteWithDisposedConnectorThrowsException()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(writer.Write);
        }

        [Test]
        public void WriteDoesNotThrowException()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            Assert.DoesNotThrow(writer.Write);
        }

        [Test]
        public void WriteObjAfterDisposeThrowsException()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            writer.Dispose();
            MyClassType obj = new MyClassType { color = "test", x = 3 };
            Assert.Throws<ObjectDisposedException>(() => writer.Write(obj));
        }

        [Test]
        public void WriteObjWithDisposedConnectorThrowsException()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            connector.Dispose();
            MyClassType obj = new MyClassType { color = "test", x = 3 };
            Assert.Throws<ObjectDisposedException>(() => writer.Write(obj));
        }

        [Test]
        public void WriteObjDoesNotThrowException()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            MyClassType obj = new MyClassType { color = "test", x = 3 };
            Assert.DoesNotThrow(() => writer.Write(obj));
        }

        [Test]
        public void DisposeChangesProperty()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            Assert.IsFalse(writer.Disposed);
            writer.Dispose();
            Assert.IsTrue(writer.Disposed);
        }

        [Test]
        public void DisposeDoesNotDisposeConnector()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            writer.Dispose();
            Assert.IsTrue(writer.Disposed);
            Assert.IsFalse(connector.Disposed);
        }

        [Test]
        public void DisposingTwiceDoesNotThrowException()
        {
            Writer writer = new Writer(connector, TestResources.WriterName);
            writer.Dispose();
            Assert.DoesNotThrow(writer.Dispose);
            Assert.IsTrue(writer.Disposed);
        }
    }
}
