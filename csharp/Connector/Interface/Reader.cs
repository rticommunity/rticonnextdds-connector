// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    sealed class Reader : IDisposable
    {
        readonly ReaderPtr handle;

        public Reader(Connector connector, string entityName)
        {
            Connector = connector;
            EntityName = entityName;
            handle = new ReaderPtr(connector, entityName);
        }

        ~Reader()
        {
            Dispose(false);
        }

        public Connector Connector {
            get;
            private set;
        }

        public string EntityName {
            get;
            private set;
        }

        public bool Disposed {
            get;
            private set;
        }

        public int GetSamplesLength()
        {
            return (int)SafeNativeMethods.RTIDDSConnector_getSamplesLength(
                Connector.Handle,
                EntityName);
        }

        public void Read()
        {
            SafeNativeMethods.RTIDDSConnector_read(Connector.Handle, EntityName);
        }

        public void Take()
        {
            SafeNativeMethods.RTIDDSConnector_take(Connector.Handle, EntityName);
        }

        public void WaitForSamples(int timeoutMillis)
        {
            SafeNativeMethods.RTIDDSConnector_wait(Connector.Handle, timeoutMillis);
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
            if (freeManagedResources && !handle.IsInvalid)
                handle.Dispose();
        }

        static class SafeNativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern IntPtr RTIDDSConnector_getReader(
                Connector.ConnectorPtr connectorHandle,
                string entityName);
            
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_read(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_take(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern int RTIDDSConnector_wait(
                Connector.ConnectorPtr connectorHandle,
                int timeout);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern double RTIDDSConnector_getSamplesLength(
                Connector.ConnectorPtr connectorHandle,
                string entityName);
        }

        sealed class ReaderPtr : SafeHandle
        {
            public ReaderPtr(Connector connector, string entityName)
                : base(IntPtr.Zero, true)
            {
                handle = SafeNativeMethods.RTIDDSConnector_getReader(
                    connector.Handle,
                    entityName);
            }

            public override bool IsInvalid => handle == IntPtr.Zero;

            protected override bool ReleaseHandle()
            {
                return true;
            }
        }
    }
}
