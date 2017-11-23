// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using RTI.Connector;

    static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || (args[0] != "pub" && args[0] != "sub")) {
                Console.WriteLine("USAGE: Objects.exe pub|sub [count=0]");
                return;
            }

            string configPath = "Configuration.xml";
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

            for (int i = 0; i < count || count == 0; i++) {
                // Create the class to send
                Shape shape = new Shape {
                    X = i,
                    Color = "BLUE",
                    Angle = 3.14f,
                    FillKind = ShapeFillKind.HorizontalHatch
                };

                // Sets elements of a sequence.
                // The sequence size is automatically updated (2 or 3).
                shape.Sequence = new List<byte>();;
                shape.Sequence.Add(42);
                shape.Sequence.Add(43);
                if (i % 2 == 0)
                    shape.Sequence.Add(44);

                // Sets elements of complex types
                shape.InnerStruct = new InnerStruct[3];
                shape.InnerStruct[1].X = i;
                shape.InnerStruct[2].X = i + 1;

                // Finally write the sample and wait some time
                Console.WriteLine("Writing sample {0}", i);
                writer.Write(shape);
                Thread.Sleep(2000);
            }
        }

        static void Subscribe(Connector connector, int count)
        {
            string readerName = "MySubscriber::MySquareReader";
            Reader reader = new Reader(connector, readerName);

            for (int i = 0; i < count || count == 0; i++) {
                // Wait for upto 2 seconds for samples.
                if (!connector.WaitForSamples(3000))
                    continue;

                reader.Take();
                var sampleList = reader.Samples
                    .Where(s => s.IsValid)
                    .Select(s => s.GetAs<Shape>());
                foreach (Shape sample in sampleList) {
                    Console.Write($"[x:{sample.X}");
                    Console.Write($",color:{sample.Color}");
                    Console.Write($",angle:{sample.Angle}");
                    Console.Write($",fillKind:{sample.FillKind}");
                    for (int j = 0; j < sample.Sequence.Count; j++)
                        Console.Write($",seq[{j}]:{sample.Sequence[j]}");
                    for (int j = 0; j < sample.InnerStruct.Length; j++)
                        Console.Write($",inner[{j}]:{sample.InnerStruct[j].X}");
                    Console.WriteLine("]");
                }
            }
        }
    }
}
