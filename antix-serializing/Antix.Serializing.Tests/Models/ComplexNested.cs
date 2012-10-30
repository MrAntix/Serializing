using System;
using System.Collections.Generic;

namespace Antix.Serializing.Tests.Models
{
    public class ComplexNested
    {
        public string StringValue { get; set; }
        public DateTime DateValue { get; set; }
        public int IntegerValue { get; set; }
        public int[] IntegerArrayValue { get; set; }

        public Complex ComplexValue { get; set; }
        public List<Complex> ComplexCollectionValue { get; set; }
    }
}