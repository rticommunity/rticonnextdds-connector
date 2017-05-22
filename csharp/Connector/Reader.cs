// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    public class Reader
    {
        public Reader(Connector connector, string entityName)
        {
            Name = entityName;
            InternalReader = new Interface.Reader(
                connector.InternalConnector,
                entityName);
        }

        public string Name {
            get;
            private set;
        }

        internal Interface.Reader InternalReader {
            get;
            private set;
        }

        public void Read()
        {
            InternalReader.Read();
        }

        public void Take()
        {
            InternalReader.Take();
        }

        public void WaitForSamples(int timeoutMillis)
        {
            InternalReader.WaitForSamples(timeoutMillis);
        }
    }
}
