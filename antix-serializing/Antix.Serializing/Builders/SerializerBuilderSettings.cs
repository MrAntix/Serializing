using System.Globalization;
using System.Text;
using Antix.Serializing.Properties;

namespace Antix.Serializing.Builders
{
    public class SerializerBuilderSettings
    {
        public SerializerBuilderSettings()
        {
            Encoding = string.IsNullOrWhiteSpace(Settings.Default.Encoding)
                           ? Encoding.Default
                           : Encoding.GetEncoding(Settings.Default.Encoding);
            Culture = string.IsNullOrWhiteSpace(Settings.Default.Culture)
                          ? CultureInfo.InvariantCulture
                          : CultureInfo.GetCultureInfo(Settings.Default.Culture);

            IncludeNulls = Settings.Default.IncludeNulls;
            DateTimeFormatString = Settings.Default.DateTimeFormatString;
            TimeSpanFormatString = Settings.Default.TimeSpanFormatString;
            NumberFormatString = Settings.Default.NumberFormatString;
            EnumFormatString = Settings.Default.EnumFormatString;

            AnonymousTypeName = Settings.Default.AnonymousTypeName;
        }

        public Encoding Encoding { get; set; }

        public CultureInfo Culture { get; set; }

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

        public string AnonymousTypeName { get; set; }
    }
}