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

    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || (args[0] != "pub" && args[0] != "sub")) {
                Console.WriteLine("USAGE: Simple.exe pub|sub [count=0]");
                return;
            }

            string configPath = "ShapeExample.xml";
            string configName = "MyParticipantLibrary::Zero";
            bool publishMode = args[0] == "pub";
            int count = args.Length > 1 ? Int32.Parse(args[1]) : 0;

            Console.WriteLine("Initializating RTI Connector");
            using (var connector = new Connector(configName, configPath)) {
                if (publishMode)
                    Publish(connector, count);
                else
                    Subscribe(connector, count);

                Console.WriteLine("Finalizing RTI Connector");
            }
        }

        static void Publish(Connector connector, int count)
        {
            string writerName = "MyPublisher::MySquareWriter";
            Writer writer = new Writer(connector, writerName);

            Instance instance = writer.Instance;
            for (int i = 0; i < count || count == 0; i++) {
                Console.WriteLine("Writing sample {0}", i);

                // Optionally, clear the instance field from previous iterations
                instance.Clear();
                instance.Set("x", i);
                instance.Set("y", i * 2);
                instance.Set("shapesize", 30);
                instance.Set("color", "BLUE");

                writer.Write();
                Thread.Sleep(100);
            }
        }

        static void Subscribe(Connector connector, int count)
        {
            string readerName = "MySubscriber::MySquareReader";
            Reader reader = new Reader(connector, readerName);

            for (int i = 0; i < count || count == 0; i++) {
                // Poll for samples every second
                Console.WriteLine("Waiting 1 second...");
                Thread.Sleep(1000);

                // Take samples. Accesible from Reader.Samples
                reader.Take();
                Console.WriteLine("Received {0} samples", reader.Samples.Count);
                foreach (Sample sample in reader.Samples) {
                    if (sample.IsValid) {
                        Console.WriteLine(
                            "Received [x={0}, y={1}, size={2}, color={3}]",
                             sample.GetInt("x"),
                             sample.Get<int>("y"),
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
