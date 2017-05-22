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

    public class Connector : IDisposable
    {
        readonly Interface.Connector internalConnector;

        public Connector(string configName, string configFile)
        {
            ConfigName = configName;
            ConfigFile = configFile;

            internalConnector = new Interface.Connector(configName, configFile);
        }

        ~Connector()
        {
            Dispose(false);
        }

        public string ConfigName {
            get;
            private set;
        }

        public string ConfigFile {
            get;
            private set;
        }

        public bool Disposed {
            get;
            private set;
        }

        internal Interface.Connector InternalConnector {
            get { return internalConnector; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool freeManagedResources)
        {
            if (Disposed)
                return;

            Disposed = true;
            if (freeManagedResources)
                internalConnector.Dispose();
        }
    }
}
