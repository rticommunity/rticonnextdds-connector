// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
// namespace RTI.Connector
namespace RTI.Connector.UnitTests
{
    using System.IO;
    using System.Reflection;

    public static class TestResources
    {
        public static string ConfigPath => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "TestConfig.xml");

        public static string PublisherConfig => "PartLib::PartPub";

        public static string SubscriberConfig => "PartLib::PartSub";

        public static string PubSubConfig => "PartLib::PartPubSub";

        public static string WriterName => "MyPublisher::MySquareWriter";

        public static string ReaderName => "MySubscriber::MySquareReader";

        public static Connector CreatePublisherConnector()
        {
            return new Connector(PublisherConfig, ConfigPath);
        }

        public static Connector CreateSubscriberConnector()
        {
            return new Connector(SubscriberConfig, ConfigPath);
        }

        public static Connector CreatePubSubConnector()
        {
            return new Connector(PubSubConfig, ConfigPath);
        }
    }
}
