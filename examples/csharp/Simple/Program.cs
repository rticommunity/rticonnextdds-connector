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
            bool publishMode = false;

            using (var connector = new Connector(configName, configPath)) {
                if (publishMode)
                    Publish(connector);
                else
                    Subscribe(connector);
            }
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

        static void Subscribe(Connector connector)
        {
            string readerName = "MySubscriber::MySquareReader";
            Reader reader = new Reader(connector, readerName);

            for (int count = 0; count < 10; count++) {
                Console.WriteLine("Waiting 1 second...");
                Thread.Sleep(1000);

                reader.Take();
                Console.WriteLine("Received {0} samples", reader.Samples.Count);
                foreach (Sample sample in reader.Samples) {
                    if (sample.IsValid) {
                        Console.WriteLine(
                            "Received [x={0}, y={1}, size={2}, color={3}]",
                             sample.GetInt("x"),
                             sample.GetInt("y"),
                             sample.Get<int>("shapesize"),
                             sample.GetString("color"));
                    } else {
                        Console.WriteLine("Received metadata");
                    }
                }
            }
        }
    }
}
