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

    public class SampleCollection : IReadOnlyCollection<Sample>
    {
        readonly Reader reader;

        internal SampleCollection(Reader reader)
        {
            this.reader = reader;
        }

        public int Count => reader.InternalReader.GetSamplesLength();

        public IEnumerator<Sample> GetEnumerator()
        {
            return new SampleEnumerator(reader, Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

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

        public Sample Current => new Sample(reader, index);

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (index >= count)
                return false;

            index++;
            return true;
        }

        public void Reset()
        {
            index = 0;
        }

        public void Dispose()
        {
        }
    }
}
