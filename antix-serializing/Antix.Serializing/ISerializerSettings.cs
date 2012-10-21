using System;
using System.Text;

namespace Antix.Serializing
{
    public interface ISerializerSettings
    {
        Encoding Encoding { get; set; }
        IFormatProvider FormatProvider { get; set; }

        bool IncludeNulls { get; set; }
        string DateTimeFormatString { get; set; }
        string NumberFormatString { get; set; }
        string TimeSpanFormatString { get; set; }
    }
}