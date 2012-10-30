using System;
using System.IO;
using System.Text;

namespace Antix.Serializing.Abstraction
{
    public interface ISerializer
    {
        Encoding Encoding { get; }
        IFormatProvider FormatProvider { get; }

        bool IncludeNulls { get; }

        void Serialize(TextWriter writer, object value);
    }
}