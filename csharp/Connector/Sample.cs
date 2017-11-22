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
    using System.Globalization;
    
    /// <summary>
    /// Sample read with a <see cref="Reader"/>.
    /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Sample"/> contains
        /// data or just metadata information.
        /// </summary>
        /// <value><c>true</c> if contains data; otherwise, <c>false</c>.</value>
        public bool IsValid => internalSample.GetBoolFromInfo("valid_data");

        /// <summary>
        /// Gets the value from an integer field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public int GetInt(string field)
        {
            return internalSample.GetNumberFromSample(field);
        }

        /// <summary>
        /// Gets the value from an boolean field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public bool GetBool(string field)
        {
            return internalSample.GetBoolFromSample(field);
        }

        /// <summary>
        /// Gets the value from an string field.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        public string GetString(string field)
        {
            return internalSample.GetStringFromSample(field);
        }

        /// <summary>
        /// Get the specified field value.
        /// </summary>
        /// <returns>The field value.</returns>
        /// <param name="field">Field name.</param>
        /// <typeparam name="T">
        /// The field type. It can be 'int', 'bool' or 'string'.
        /// </typeparam>
        public T Get<T>(string field)
        {
            object val;
            if (typeof(T) == typeof(int))
                val = GetInt(field);
            else if (typeof(T) == typeof(bool))
                val = GetBool(field);
            else if (typeof(T) == typeof(string))
                val = GetString(field);
            else
                throw new FormatException("Unsupported field type");

            return (T)Convert.ChangeType(
                val,
                typeof(T),
                CultureInfo.InvariantCulture);
        }
    }
}
