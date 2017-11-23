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

    sealed class Sample
    {
        readonly Reader reader;
        readonly int index;

        public Sample(Reader reader, int index)
        {
            this.reader = reader;
            this.index = index;
        }

        public int GetNumberFromSample(string field)
        {
            return (int)NativeMethods.RTIDDSConnector_getNumberFromSamples(
                reader.Connector.Handle,
                reader.EntityName,
                index,
                field);
        }

        public bool GetBoolFromSample(string field)
        {
            return NativeMethods.RTIDDSConnector_getBooleanFromSamples(
                reader.Connector.Handle,
                reader.EntityName,
                index,
                field) != 0;
        }

        public string GetStringFromSample(string field)
        {
            return NativeMethods.RTIDDSConnector_getStringFromSamples(
                reader.Connector.Handle,
                reader.EntityName,
                index,
                field);
        }

        public string GetJsonFromSample()
        {
            return NativeMethods.RTIDDSConnector_getJSONSample(
                reader.Connector.Handle,
                reader.EntityName,
                index);
        }

        public bool GetBoolFromInfo(string field)
        {
            return NativeMethods.RTIDDSConnector_getBooleanFromInfos(
                reader.Connector.Handle,
                reader.EntityName,
                index,
                field) != 0;
        }

        static class NativeMethods
        {
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern int RTIDDSConnector_getBooleanFromInfos(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);
            
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern double RTIDDSConnector_getNumberFromSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);
            
            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern int RTIDDSConnector_getBooleanFromSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern string RTIDDSConnector_getStringFromSamples(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index,
                string name);

            [DllImport("rtiddsconnector", CharSet = CharSet.Ansi)]
            public static extern string RTIDDSConnector_getJSONSample(
                Connector.ConnectorPtr connectorHandle,
                string entityName,
                int index);
        }
    }
}
