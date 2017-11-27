// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace Mixed
{
    using System;
    using System.Threading;
    using RTI.Connector;

    static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || (args[0] != "pub" && args[0] != "sub")) {
                Console.WriteLine("USAGE: Mixed.exe pub|sub [count=0]");
                return;
            }

            string configPath = "Mixed.xml";
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
                // Clear all the instance members
                instance.Clear();

                // Sets a number
                instance.Set("x", i);

                // Sets a string
                instance.Set("color", "BLUE");

                // Sets elements of a sequence.
                // The sequence size is automatically updated (2 or 3).
                instance.Set("aOctetSeq[1]", 42);
                instance.Set("aOctetSeq[2]", 43);
                if (i % 2 == 0)
                    instance.Set("aOctetSeq[3]", 44);

                // Sets elements of complex types
                instance.Set("innerStruct[1].x", i);
                instance.Set("innerStruct[2].x", i + 1);

                // Finally write the sample and wait some time
                Console.WriteLine("Writing sample {0}", i);
                writer.Write();
                Thread.Sleep(2000);
            }
        }

        static void Subscribe(Connector connector, int count)
        {
            string readerName = "MySubscriber::MySquareReader";
            Reader reader = new Reader(connector, readerName);

            for (int i = 0; i < count || count == 0; i++) {
                // Poll for samples every 2 seconds
                Console.WriteLine("Waiting 2 seconds...");
                Thread.Sleep(2000);

                reader.Take();
                Console.WriteLine("Received {0} samples", reader.Samples.Count);
                foreach (Sample sample in reader.Samples) {
                    if (sample.IsValid) {
                        // Gets an integer using generic types
                        int x = sample.Get<int>("x");

                        // Gets a string
                        string color = sample.GetString("color");

                        // Gets the size of a sequence
                        int seqLength = sample.GetInt("aOctetSeq#");
                        Console.WriteLine("I received a sequence with {0} elements",
                                          seqLength);
                    } else {
                        Console.WriteLine("Received metadata");
                    }
                }
            }
        }
    }
}
