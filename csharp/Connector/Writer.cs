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
    /// Connector sample writer.
    /// </summary>
    public class Writer : IDisposable
    {
        readonly Interface.Writer writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Writer"/> class.
        /// </summary>
        /// <param name="connector">Parent connector.</param>
        /// <param name="entityName">Entity name.</param>
        public Writer(Connector connector, string entityName)
        {
            if (connector == null)
                throw new ArgumentNullException(nameof(connector));
            if (string.IsNullOrEmpty(entityName))
                throw new ArgumentNullException(nameof(entityName));

            Name = entityName;
            writer = new Interface.Writer(connector.InternalConnector, entityName);
            Instance = new Instance(this);
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <value>The entity name.</value>
        public string Name {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unique instance associated with this writer.
        /// </summary>
        /// <value>The writer instance.</value>
        public Instance Instance {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Writer"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed {
            get;
            private set;
        }

        internal Interface.Writer InternalWriter {
            get { return writer; }
        }

        /// <summary>
        /// Write the writer instance.
        /// </summary>
        public void Write()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(Writer));

            writer.Write();
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Writer"/> object.
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
                writer.Dispose();
        }
    }
}
