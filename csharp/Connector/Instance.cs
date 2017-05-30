// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    /// <summary>
    /// Writer instance.
    /// </summary>
    public class Instance
    {
        readonly Writer writer;
        readonly Interface.Instance instance;

        internal Instance(Writer writer)
        {
            this.writer = writer;
            instance = new Interface.Instance(writer.InternalWriter);
        }

        #if NET40
        /// <summary>
        /// Sets a value to the specified field.
        /// </summary>
        /// <param name="field">Field name.</param>
        public dynamic this[string field] {
            set {
                if (value is int)
                    instance.SetNumber(field, value);
                else if (value is string)
                    instance.SetString(field, value);
                else if (value is bool)
                    instance.SetBool(field, value);
            }
        }
        #endif

        /// <summary>
        /// Clear all the members of this instance.
        /// </summary>
        public void Clear()
        {
            instance.Clear();
        }

        /// <summary>
        /// Set the specified number value to a field.
        /// </summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value for the field.</param>
        public void Set(string field, int value)
        {
            instance.SetNumber(field, value);
        }

        /// <summary>
        /// Set the specified boolean value to a field.
        /// </summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value for the field.</param>
        public void Set(string field, bool value)
        {
            instance.SetBool(field, value);
        }

        /// <summary>
        /// Set the specified string value to a field.
        /// </summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Value for the field.</param>
        public void Set(string field, string value)
        {
            instance.SetString(field, value);
        }
    }
}
