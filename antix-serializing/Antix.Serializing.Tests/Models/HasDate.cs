using System;

namespace Antix.Serializing.Tests.Models
{
    public class HasDate
    {
        public DateTimeOffset Value { get; set; }
        public DateTimeOffset? NullableValue { get; set; }
    }
}