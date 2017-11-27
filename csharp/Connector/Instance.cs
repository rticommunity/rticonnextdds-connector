// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    using Newtonsoft.Json;

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

        /// <summary>
        /// Set instance fields from the object.
        /// </summary>
        /// <param name="obj">Object to serialize as json for the instance.</param>
        public void SetFrom(object obj)
        {
            instance.SetJson(JsonConvert.SerializeObject(obj));
        }
    }
}
