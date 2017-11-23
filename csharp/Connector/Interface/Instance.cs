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

    sealed class Instance
    {
        readonly Writer writer;

        public Instance(Writer writer)
        {
            this.writer = writer;
        }

        public void SetNumber(string field, int val)
        {
            if (writer.Connector.Disposed)
                throw new ObjectDisposedException(nameof(Connector));

            NativeMethods.RTIDDSConnector_setNumberIntoSamples(
                writer.Connector.Handle,
                writer.EntityName,
                field,
                val);
        }

        public void SetBool(string field, bool val)
        {
            if (writer.Connector.Disposed)
                throw new ObjectDisposedException(nameof(Connector));

            NativeMethods.RTIDDSConnector_setBooleanIntoSamples(
                writer.Connector.Handle,
                writer.EntityName,
                field,
                val ? 1 : 0);
        }

        public void SetString(string field, string val)
        {
            if (writer.Connector.Disposed)
                throw new ObjectDisposedException(nameof(Connector));

            NativeMethods.RTIDDSConnector_setStringIntoSamples(
                writer.Connector.Handle,
                writer.EntityName,
                field,
                val);
        }

        public void SetJson(string json)
        {
            if (writer.Connector.Disposed)
                throw new ObjectDisposedException(nameof(Connector));

            NativeMethods.RTIDDSConnector_setJSONInstance(
                writer.Connector.Handle,
                writer.EntityName,
                json);
        }

        public void Clear()
        {
            if (writer.Connector.Disposed)
                throw new ObjectDisposedException(nameof(Connector));

            NativeMethods.RTIDDSConnector_clear(
                writer.Connector.Handle,
                writer.EntityName);
        }

        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setNumberIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                double val);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setBooleanIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                int val);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setStringIntoSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string name,
                string val);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_setJSONInstance(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                string json);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern void RTIDDSConnector_clear(
                Connector.ConnectorPtr connectorHandle,
                string entityName);
        }
    }
}
