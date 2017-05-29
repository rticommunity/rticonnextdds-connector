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

    sealed class Connector : IDisposable
    {
        public Connector(string configName, string configFile)
        {
            Handle = new ConnectorPtr(configName, configFile);
        }

        ~Connector()
        {
            Dispose(false);
        }

        public bool Disposed {
            get;
            private set;
        }

        public ConnectorPtr Handle {
            get;
            private set;
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
            if (freeManagedResources && !Handle.IsInvalid)
                Handle.Dispose();
        }

        sealed internal class ConnectorPtr : SafeHandle
        {
            public ConnectorPtr(string configName, string configFile)
                : base(IntPtr.Zero, true)
            {
                handle = SafeNativeMethods.RTIDDSConnector_new(
                    configName,
                    configFile,
                    IntPtr.Zero);
            }

            public override bool IsInvalid => handle == IntPtr.Zero;

            protected override bool ReleaseHandle()
            {
                SafeNativeMethods.RTIDDSConnector_delete(handle);
                return true;
            }
        }

        [SuppressUnmanagedCodeSecurity]
        static class SafeNativeMethods
        {
            [DllImport("rti_dds_connector")]
            public static extern IntPtr RTIDDSConnector_new(
                string configName,
                string configFile,
                IntPtr config);

            [DllImport("rti_dds_connector")]
            public static extern void RTIDDSConnector_delete(IntPtr handle);
        }
    }
}
