// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Collection of samples read with a <see cref="Reader"/>.
    /// </summary>
    public class SampleCollection : IEnumerable<Sample>
    {
        readonly Reader reader;

        internal SampleCollection(Reader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Gets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        public int Count => reader.InternalReader.GetSamplesLength();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Sample> GetEnumerator()
        {
            return new SampleEnumerator(reader, Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Sample collection enumerator for a SampleCollection.
    /// </summary>
    public class SampleEnumerator : IEnumerator<Sample>
    {
        readonly Reader reader;
        readonly int count;
        int index;

        internal SampleEnumerator(Reader reader, int count)
        {
            this.reader = reader;
            this.count = count;
            index = 0;
        }

        /// <summary>
        /// Gets the current iterating sample.
        /// </summary>
        /// <value>The current sample.</value>
        public Sample Current => new Sample(reader, index);

        object IEnumerator.Current => Current;

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if the enumerator was successfully advanced to the
        /// next element; <c>false</c> if the enumerator has passed the end of
        /// the collection.
        /// </returns>
        public bool MoveNext()
        {
            if (index >= count)
                return false;

            index++;
            return true;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the
        /// first element in the collection.
        /// </summary>
        public void Reset()
        {
            index = 0;
        }

        /// <summary>
        /// Releases all resource used by the
        /// <see cref="SampleEnumerator"/> object.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
