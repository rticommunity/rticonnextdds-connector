﻿// (c) Copyright, Real-Time Innovations, 2017.
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

    sealed class Connector : IDisposable
    {
        const int RetCodeTimeOut = 10;

        public Connector(string configName, string configFile)
        {
            Handle = new ConnectorPtr(configName, configFile);
            if (Handle.IsInvalid)
                throw new COMException("Error creating connector");
        }

        ~Connector()
        {
            Dispose(false);
        }

        public ConnectorPtr Handle {
            get;
            private set;
        }

        public bool Disposed {
            get;
            private set;
        }

        public bool WaitForSamples(int timeoutMillis)
        {
            int retcode = NativeMethods.RTIDDSConnector_wait(Handle, timeoutMillis);
            return retcode != RetCodeTimeOut;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool freeManagedResources)
        {
            Disposed = true;
            if (freeManagedResources && !Handle.IsInvalid)
                Handle.Dispose();
        }

        internal sealed class ConnectorPtr : SafeHandle
        {
            public ConnectorPtr(string configName, string configFile)
                : base(IntPtr.Zero, true)
            {
                handle = NativeMethods.RTIDDSConnector_new(
                    configName,
                    configFile,
                    IntPtr.Zero);
            }

            public override bool IsInvalid => handle == IntPtr.Zero;

            protected override bool ReleaseHandle()
            {
                NativeMethods.RTIDDSConnector_delete(handle);
                handle = IntPtr.Zero;
                return true;
            }
        }

        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern IntPtr RTIDDSConnector_new(
                string configName,
                string configFile,
                IntPtr config);

            [DllImport("rtiddsconnector")]
            public static extern void RTIDDSConnector_delete(IntPtr handle);

            [DllImport("rtiddsconnector")]
            public static extern int RTIDDSConnector_wait(
                ConnectorPtr connectorHandle,
                int timeout);
        }
    }
}
