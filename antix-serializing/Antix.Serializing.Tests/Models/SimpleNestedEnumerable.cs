using System.Collections.Generic;

namespace Antix.Serializing.Tests.Models
{
    public class SimpleNestedEnumerable
    {
        public string Name { get; set; }
        public IEnumerable<Simple> Simples { get; set; }
    }
}