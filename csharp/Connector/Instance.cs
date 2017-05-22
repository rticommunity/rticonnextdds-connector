// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    public class Instance
    {
        readonly Writer writer;
        readonly Interface.Instance instance;

        internal Instance(Writer writer)
        {
            this.writer = writer;
            instance = new Interface.Instance(writer.InternalWriter);
        }

        public int this[string field] {
            set {
                instance.SetNumber(field, value);
            }
        }
    }
}
