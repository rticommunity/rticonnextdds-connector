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

    sealed class Reader : IDisposable
    {
        readonly ReaderPtr handle;

        public Reader(Connector connector, string entityName)
        {
            Connector = connector;
            EntityName = entityName;
            handle = new ReaderPtr(connector, entityName);
            if (handle.IsInvalid)
                throw new COMException("Error creating reader");
        }

        public Connector Connector {
            get;
            private set;
        }

        public string EntityName {
            get;
            private set;
        }

        public int GetSamplesLength()
        {
            return (int)NativeMethods.RTIDDSConnector_getSamplesLength(
                Connector.Handle,
                EntityName);
        }

        public void Read()
        {
            if (Connector.Disposed)
                throw new ObjectDisposedException(nameof(Connector));

            NativeMethods.RTIDDSConnector_read(Connector.Handle, EntityName);
        }

        public void Take()
        {
            if (Connector.Disposed)
                throw new ObjectDisposedException(nameof(Connector));

            NativeMethods.RTIDDSConnector_take(Connector.Handle, EntityName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool freeManagedResources)
        {
            if (freeManagedResources && !handle.IsInvalid)
                handle.Dispose();
        }

        static class NativeMethods
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
            public static extern double RTIDDSConnector_getSamplesLength(
                Connector.ConnectorPtr connectorHandle,
                string entityName);
        }

        sealed class ReaderPtr : SafeHandle
        {
            public ReaderPtr(Connector connector, string entityName)
                : base(IntPtr.Zero, true)
            {
                handle = NativeMethods.RTIDDSConnector_getReader(
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
