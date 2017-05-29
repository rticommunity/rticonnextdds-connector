// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
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

        public void SetBool(string field, bool val)
        {
            SafeNativeMethods.RTIDDSConnector_setBooleanIntoSamples(
                writer.Connector.Handle,
                writer.EntityName,
                field,
                val ? 1 : 0);
        }

        public void SetString(string field, string val)
        {
            SafeNativeMethods.RTIDDSConnector_setStringIntoSamples(
                writer.Connector.Handle,
                writer.EntityName,
                field,
                val);
        }

        public void Clear()
        {
            SafeNativeMethods.RTIDDSConnector_clear(
                writer.Connector.Handle,
                writer.EntityName);
        }

        [SuppressUnmanagedCodeSecurity]
        static class SafeNativeMethods
        {
            [DllImport("rti_dds_connector")]
            public static extern void RTIDDSConnector_setNumberIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                double val);

            [DllImport("rti_dds_connector")]
            public static extern void RTIDDSConnector_setBooleanIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                int val);

            [DllImport("rti_dds_connector")]
            public static extern void RTIDDSConnector_setStringIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                string val);

            [DllImport("rti_dds_connector")]
            public static extern void RTIDDSConnector_clear(
                Connector.ConnectorPtr connectorHandle,
                string entityName);
        }
    }
}
