using System.Collections.Generic;

namespace Antix.Serializing.Tests.Models
{
    public class HasDictionary
    {
        public IDictionary<string, Simple> Value { get; set; }
    }
}