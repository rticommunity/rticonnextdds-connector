// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    using System;

    public class Sample
    {
        readonly Reader reader;
        readonly int index;
        readonly Interface.Sample internalSample;

        internal Sample(Reader reader, int index)
        {
            this.reader = reader;
            this.index = index;
            internalSample = new Interface.Sample(reader.InternalReader, index);
        }

        public bool IsValid => internalSample.GetBoolFromInfo("valid_data");

        public int GetInt(string field)
        {
            return internalSample.GetNumberFromSample(field);
        }

        public bool GetBool(string field)
        {
            return internalSample.GetBoolFromSample(field);
        }

        public string GetString(string field)
        {
            return internalSample.GetStringFromSample(field);
        }

        #if NET40
        public dynamic Get<T>(string field)
        {
            if (typeof(T) == typeof(int))
                return GetInt(field);
            else if (typeof(T) == typeof(bool))
                return GetBool(field);
            else if (typeof(T) == typeof(string))
                return GetString(field);
            else
                throw new FormatException();
        }
        #endif
    }
}
