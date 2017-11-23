// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    using System;

    /// <summary>
    /// Connector sample reader.
    /// </summary>
    public class Reader : IDisposable
    {
        readonly Interface.Reader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        /// <param name="connector">Parent connector.</param>
        /// <param name="entityName">Entity name.</param>
        public Reader(Connector connector, string entityName)
        {
            if (connector == null)
                throw new ArgumentNullException(nameof(connector));
            if (string.IsNullOrEmpty(entityName))
                throw new ArgumentNullException(nameof(entityName));

            Name = entityName;
            reader = new Interface.Reader(
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

        /// <summary>
        /// Gets a value indicating whether this <see cref="Reader"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed {
            get;
            private set;
        }

        internal Interface.Reader InternalReader {
            get { return reader; }
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
            if (Disposed)
                throw new ObjectDisposedException(nameof(Reader));

            reader.Read();
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
            if (Disposed)
                throw new ObjectDisposedException(nameof(Reader));

            reader.Take();
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Reader"/> object.
        /// </summary>
        /// <remarks>
        /// Calling this method doesn't delete the DataWriter.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool freeManagedResources)
        {
            if (Disposed)
                return;

            Disposed = true;
            if (freeManagedResources)
                reader.Dispose();
        }
    }
}
