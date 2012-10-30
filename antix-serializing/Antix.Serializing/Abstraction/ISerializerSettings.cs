using System;
using System.Text;

namespace Antix.Serializing.Abstraction
{
    public interface ISerializerSettings
    {
        Encoding Encoding { get; set; }
        IFormatProvider FormatProvider { get; set; }

        bool IncludeNulls { get; set; }
        string DateTimeFormatString { get; set; }
        string NumberFormatString { get; set; }
        string TimeSpanFormatString { get; set; }
        string EnumFormatString { get; set; }
    }
}