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

    sealed class Reader
    {
        readonly ReaderPtr handle;

        public Reader(Connector connector, string entityName)
        {
            Connector = connector;
            EntityName = entityName;
            handle = new ReaderPtr(connector, entityName);
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
            NativeMethods.RTIDDSConnector_read(Connector.Handle, EntityName);
        }

        public void Take()
        {
            NativeMethods.RTIDDSConnector_take(Connector.Handle, EntityName);
        }

        public void WaitForSamples(int timeoutMillis)
        {
            NativeMethods.RTIDDSConnector_wait(Connector.Handle, timeoutMillis);
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
