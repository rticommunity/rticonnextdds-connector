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

    sealed class Writer
    {
        readonly WriterPtr handle;

        public Writer(Connector connector, string entityName)
        {
            Connector = connector;
            EntityName = entityName;
            handle = new WriterPtr(connector, entityName);
        }

        public Connector Connector {
            get;
            private set;
        }

        public string EntityName {
            get;
            private set;
        }

        public void Write()
        {
            SafeNativeMethods.RTIDDSConnector_write(Connector.Handle, EntityName);
        }

        sealed class WriterPtr : SafeHandle
        {
            public WriterPtr(Connector connector, string entityName)
                : base(IntPtr.Zero, true)
            {
                handle = SafeNativeMethods.RTIDDSConnector_getWriter(
                    connector.Handle,
                    entityName);
            }

            public override bool IsInvalid => handle == IntPtr.Zero;

            protected override bool ReleaseHandle()
            {
                return true;
            }
        }

        static class SafeNativeMethods
        {
            [DllImport("rtiddsconnector")]
            public static extern IntPtr RTIDDSConnector_getWriter(
                Connector.ConnectorPtr connectorHandle,
                string entityName);

            [DllImport("rtiddsconnector")]
            public static extern void RTIDDSConnector_write(
                Connector.ConnectorPtr connectorHandle,
                string entityName);
        }
    }
}
