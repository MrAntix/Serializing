using System;

namespace Antix.Serializing.Tests.Models
{
    public class HasEnum
    {
        public Types Value { get; set; }
        public Types? NullableValue { get; set; }
        public FlaggedTypes FlagsValue { get; set; }

        public enum Types
        {
            One,
            Two
        }

        [Flags]
        public enum FlaggedTypes
        {
            One = 1,
            Two
        }
    }
}