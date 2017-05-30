// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    /// <summary>
    /// Connector sample reader.
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        /// <param name="connector">Parent connector.</param>
        /// <param name="entityName">Entity name.</param>
        public Reader(Connector connector, string entityName)
        {
            Name = entityName;
            InternalReader = new Interface.Reader(
                connector.InternalConnector,
                entityName);
            Samples = new SampleCollection(this);
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get;
            private set;
        }

        /// <summary>
        /// Gets the samples read or taken from this reader.
        /// </summary>
        /// <value>The samples read or taken.</value>
        public SampleCollection Samples {
            get;
            private set;
        }

        internal Interface.Reader InternalReader {
            get;
            private set;
        }

        /// <summary>
        /// Reads samples with this reader and do not remove them from the
        /// internal queue.
        /// </summary>
        /// <remarks>
        /// The samples are accessible from the
        /// <see cref="Samples"/> property. 
        /// </remarks>
        public void Read()
        {
            InternalReader.Read();
        }

        /// <summary>
        /// Reads samples with this reader and remove them from the
        /// internal queue.
        /// </summary>
        /// <remarks>
        /// The samples are accesible from the
        /// <see cref="Samples"/> property. 
        /// </remarks>
        public void Take()
        {
            InternalReader.Take();
        }

        /// <summary>
        /// Waits until a sample is received or the specified time passed.
        /// </summary>
        /// <param name="timeoutMillis">Timeout in milliseconds.</param>
        public void WaitForSamples(int timeoutMillis)
        {
            InternalReader.WaitForSamples(timeoutMillis);
        }
    }
}
