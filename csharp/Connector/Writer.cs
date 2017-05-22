// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    public class Writer
    {
        readonly Connector connector;
        readonly Interface.Writer writer;

        public Writer(Connector connector, string entityName)
        {
            this.connector = connector;
            Name = entityName;
            writer = new Interface.Writer(connector.InternalConnector, entityName);
            Instance = new Instance(this);
        }

        public string Name {
            get;
            private set;
        }

        public Instance Instance {
            get;
            private set;
        }

        internal Interface.Writer InternalWriter {
            get { return writer; }
        }

        public void Write()
        {
            writer.Write();
        }
    }
}
