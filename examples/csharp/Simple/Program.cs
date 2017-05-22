// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace Simple
{
    using System;
    using System.Threading;
    using RTI.Connector;

    class MainClass
    {
        public static void Main()
        {
            string configPath = "ShapeExample.xml";
            string configName = "MyParticipantLibrary::Zero";

            using (var connector = new Connector(configName, configPath))
                Publish(connector);
        }

        static void Publish(Connector connector)
        {
            string writerName = "MyPublisher::MySquareWriter";
            Writer writer = new Writer(connector, writerName);

            Instance instance = writer.Instance;
            for (int i = 0; i < 5; i++) {
                Console.WriteLine("Writing sample {0}", i);

                instance.Clear();
                instance["x"] = i;
                instance["y"] = i * 2;
                instance.Set("shapesize", 30);
                instance.Set("color", "BLUE");

                writer.Write();
                Thread.Sleep(2000);
            }
        }
    }
}
