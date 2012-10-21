using System;

namespace Antix.Serializing.Tests.Models
{
    public class HasTimeSpan
    {
        public TimeSpan Value { get; set; }
        public TimeSpan? NullableValue { get; set; }
    }
}