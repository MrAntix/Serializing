using System.Collections.Generic;

namespace Antix.Serializing.Abstraction.Builders
{
    internal interface ISerializerTypeConfiguration :
        ISerializerConfiguration
    {
        IEnumerable<ISerializerConfiguration> Properties { get; }
    }
}