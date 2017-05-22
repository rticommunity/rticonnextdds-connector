// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
using System;
namespace RTI.Connector.Interface
{
    using System.Runtime.InteropServices;
    using System.Security;

    sealed class Instance
    {
        readonly Writer writer;

        public Instance(Writer writer)
        {
            this.writer = writer;
        }

        public void SetNumber(string field, int val)
        {
            SafeNativeMethods.RTIDDSConnector_setNumberIntoSamples(
                writer.Connector.Handle,
                writer.EntityName,
                field,
                val);
        }

        [SuppressUnmanagedCodeSecurity]
        static class SafeNativeMethods
        {
            [DllImport("librti_dds_connector")]
            public static extern void RTIDDSConnector_setNumberIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                double val);
        }
    }
}
