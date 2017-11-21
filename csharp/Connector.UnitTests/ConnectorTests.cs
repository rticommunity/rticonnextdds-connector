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
            using (var connector = CreateConnector()) {
                Assert.AreEqual(TestResources.PublisherConfig, connector.ConfigName);
                Assert.AreEqual(TestResources.ConfigPath, connector.ConfigFile);
            }
        }

        [Test]
        public void DisposeChangesProperty()
        {
            Connector connector = CreateConnector();
            Assert.IsFalse(connector.Disposed);
            connector.Dispose();
            Assert.IsTrue(connector.Disposed);
        }

        [Test]
        public void DisposingTwiceDoesNotThrowException()
        {
            Connector connector = CreateConnector();
            connector.Dispose();
            Assert.DoesNotThrow(connector.Dispose);
            Assert.IsTrue(connector.Disposed);
        }

        Connector CreateConnector()
        {
            return new Connector(
                TestResources.PublisherConfig,
                TestResources.ConfigPath);
        }
    }
}
