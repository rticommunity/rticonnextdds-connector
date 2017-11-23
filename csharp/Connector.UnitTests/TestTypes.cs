// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace RTI.Connector.UnitTests
{
    // We are using fields without upper case to match XML defined members

    class MyClassType
    {
        public string color { get; set; }
        public int x { get; set; }
        public bool hidden { get; set; }
    }

    class MyInvalidClassType
    {
        public int color { get; set; }
        public double x { get; set; }
    }

    class MyFakeFieldsTypes
    {
        public string color { get; set; }
        public int x { get; set; }
        public bool hidden { get; set; }
        public int Fake { get; set; }
    }

    struct MyStructType
    {
        public string color { get; set; }
        public int x { get; set; }
        public bool hidden { get; set; }
    }

    class ComplexType : MyClassType
    {
        public float angle { get; set; } 
        public int[] list { get; set; }
        public Inner inner { get; set; }

        public class Inner
        {
            public int z { get; set; }
        }
    }
}
