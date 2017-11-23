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
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using NUnit.Framework;

    [TestFixture, SingleThreaded]
    public class ConnectorTests
    {
        [Test]
        public void ConstructorWithNullOrEmptyConfigNameThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Connector(null, TestResources.ConfigPath));
            Assert.Throws<ArgumentNullException>(
                () => new Connector(string.Empty, TestResources.ConfigPath));
        }

        [Test]
        public void ConstructorWithNullOrEmptyConfigFileThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Connector(TestResources.PublisherConfig, null));
            Assert.Throws<ArgumentNullException>(
                () => new Connector(TestResources.PublisherConfig, string.Empty));
        }

        [Test]
        public void ConstructorWithMissingConfigFileThrowsException()
        {
            Assert.Throws<COMException>(
                () => new Connector(TestResources.PublisherConfig, "FakeConfig.xml"));
        }

        [Test]
        public void ConstructorWithInvalidConfigNameThrowsException()
        {
            Assert.Throws<COMException>(
                () => new Connector("InvalidLib::PartPub", TestResources.ConfigPath));
            Assert.Throws<COMException>(
                () => new Connector("PartLib::InvalidPart", TestResources.ConfigPath));
        }

        [Test]
        public void ConstructorWithValidConfigIsSuccessful()
        {
            Connector connector = null;
            Assert.DoesNotThrow(
                () => connector = new Connector(
                    TestResources.PublisherConfig,
                    TestResources.ConfigPath));
            Assert.IsNotNull(connector.InternalConnector);
            connector.Dispose();
        }

        [Test]
        public void CreatingAndDeletingConnectorIsSuccessful()
        {
            int tries = 5;
            for (int i = 0; i < tries; i++)
                ConstructorWithValidConfigIsSuccessful();
        }

        [Test]
        public void ConstructorSetsProperties()
        {
            using (var connector = TestResources.CreatePublisherConnector()) {
                Assert.AreEqual(TestResources.PublisherConfig, connector.ConfigName);
                Assert.AreEqual(TestResources.ConfigPath, connector.ConfigFile);
            }
        }

        [Test]
        public void DisposeChangesProperty()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            Assert.IsFalse(connector.Disposed);
            connector.Dispose();
            Assert.IsTrue(connector.Disposed);
        }

        [Test]
        public void DisposingTwiceDoesNotThrowException()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            connector.Dispose();
            Assert.DoesNotThrow(connector.Dispose);
            Assert.IsTrue(connector.Disposed);
        }

        [Test]
        public void WaitForSamplesWithNegativeTimeOutThrowsException()
        {
            using (var connector = TestResources.CreatePublisherConnector()) {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => connector.WaitForSamples(-10));
            }
        }

        // These tests may block indefinitely so we need the timeout but it's
        // not available in the NUnit .NET Core version:
        // https://github.com/nunit/nunit/issues/1638
#if NET45
        [Test, Timeout(1000)]
        public void WaitForSamplesWithZeroTimeOutDoesNotBlock()
        {
            using (var connector = TestResources.CreatePublisherConnector()) {
                Stopwatch watch = Stopwatch.StartNew();
                Assert.IsFalse(connector.WaitForSamples(0));
                watch.Stop();
                Assert.Less(watch.ElapsedMilliseconds, 10);
            }
        }

        [Test, Timeout(1000)]
        public void WaitForSamplesCanTimeOut()
        {
            using (var connector = TestResources.CreatePublisherConnector()) {
                Stopwatch watch = Stopwatch.StartNew();
                Assert.IsFalse(connector.WaitForSamples(100));
                watch.Stop();
                Assert.Less(watch.ElapsedMilliseconds, 110);
                Assert.Greater(watch.ElapsedMilliseconds, 90);
            }
        }
#endif

        [Test]
        public void WaitForSamplesAfterDisposeThrowsException()
        {
            Connector connector = TestResources.CreatePublisherConnector();
            connector.Dispose();
            Assert.Throws<ObjectDisposedException>(() => connector.WaitForSamples(0));
        }
    }
}
