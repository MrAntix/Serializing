using System;
using System.IO;
using System.Text;

namespace Antix.Serializing
{
    public interface ISerializer
    {
        Encoding Encoding { get; }
        IFormatProvider FormatProvider { get; }

        bool IncludeNulls { get; }

        void Serialize(TextWriter writer, object value);
    }
}