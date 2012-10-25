using System;
using System.Globalization;
using System.Text;

namespace Antix.Serializing
{
    public class SerializerSettings :
        ISerializerSettings
    {
        public SerializerSettings()
        {
            Encoding = Encoding.Default;
            FormatProvider = CultureInfo.InvariantCulture;

            IncludeNulls = false;
            DateTimeFormatString = "s";
            TimeSpanFormatString = "c";
            NumberFormatString = "g";
            EnumFormatString = "g";
        }

        public Encoding Encoding { get; set; }

        public IFormatProvider FormatProvider { get; set; }

        public bool IncludeNulls { get; set; }

        /// <summary>
        ///     <see cref="http://msdn.microsoft.com/en-us/library/az4se3k1.aspx" />
        /// </summary>
        public string DateTimeFormatString { get; set; }

        /// <summary>
        ///     <see cref="http://msdn.microsoft.com/en-us/library/ee372286.aspx" />
        /// </summary>
        public string TimeSpanFormatString { get; set; }

        /// <summary>
        ///     <see cref="http://msdn.microsoft.com/en-us/library/c3s1ez6e.aspx" />
        /// </summary>
        public string EnumFormatString { get; set; }

        /// <summary>
        ///     <see cref="http://msdn.microsoft.com/en-us/library/dwhawy9k.aspx" />
        /// </summary>
        public string NumberFormatString { get; set; }
    }
}