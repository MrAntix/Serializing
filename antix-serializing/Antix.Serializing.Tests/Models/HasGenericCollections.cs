using System.Collections.Generic;

namespace Antix.Serializing.Tests.Models
{
    public class HasGenericCollections
    {
        public IDictionary<string, Simple> ValueDictionary { get; set; }
        public IList<Simple> ValueList { get; set; }
    }
}