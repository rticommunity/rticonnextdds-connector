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
    using NUnit.Framework;

    [TestFixture]
    public class ConnectorTests
    {
        [Test]
        public void ConstructorWithNullOrEmptyConfigNameThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Connector(null, "test.xml"));
            Assert.Throws<ArgumentNullException>(
                () => new Connector(string.Empty, "test.xml"));
        }

        [Test]
        public void ConstructorWithNullOrEmptyConfigFileThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Connector("config", null));
            Assert.Throws<ArgumentNullException>(
                () => new Connector("config", string.Empty));
        }

        [Test]
        public void ConstructorWithMissingConfigFileThrowsException()
        {
            Assert.Throws<NullReferenceException>(
                () => new Connector("config", "FakeConfig.xml"));
        }
    }
}
