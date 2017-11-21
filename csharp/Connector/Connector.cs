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
    /// RTI Connext DDS Connector.
    /// </summary>
    public class Connector : IDisposable
    {
        readonly Interface.Connector internalConnector;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connector"/> class.
        /// </summary>
        /// <param name="configName">XML configuration name.</param>
        /// <param name="configFile">XML configuration file path.</param>
        public Connector(string configName, string configFile)
        {
            if (string.IsNullOrEmpty(configName))
                throw new ArgumentNullException(nameof(configName));
            if (string.IsNullOrEmpty(configFile))
                throw new ArgumentNullException(nameof(configFile));

            ConfigName = configName;
            ConfigFile = configFile;

            internalConnector = new Interface.Connector(configName, configFile);
        }

        ~Connector()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the name of the configuration.
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigName {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to the configuration file.
        /// </summary>
        /// <value>The configuration file path.</value>
        public string ConfigFile {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Connector"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed {
            get;
            private set;
        }

        internal Interface.Connector InternalConnector {
            get { return internalConnector; }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Connector"/> object.
        /// </summary>
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
                internalConnector.Dispose();
        }
    }
}
