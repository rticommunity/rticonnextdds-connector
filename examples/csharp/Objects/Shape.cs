// (c) Copyright, Real-Time Innovations, 2017.
// All rights reserved.
//
// No duplications, whole or partial, manual or electronic, may be made
// without express written permission.  Any such copies, or
// revisions thereof, must display this notice unaltered.
// This code contains trade secrets of Real-Time Innovations, Inc.
namespace Objects
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Shape
    {
        public static int MaxSequenceLength { get; } = 30;

        public static int MaxInnerStructLength { get; } = 3;

        public string Color {
            get;
            set;
        }

        public int X {
            get;
            set;
        }

        public float Angle {
            get;
            set;
        }

        [JsonProperty("aLongSeq")]
        public IList<byte> Sequence {
            get;
            set;
        }

        public ShapeFillKind FillKind {
            get;
            set;
        }

        public InnerStruct[] InnerStruct {
            get;
            set;
        }
    }
}
