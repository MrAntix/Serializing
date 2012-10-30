using System.Globalization;
using System.Text;

namespace Antix.Serializing
{
    public class SerializerSettings
    {
        readonly Encoding _encoding;
        readonly CultureInfo _culture;

        readonly bool _includeNulls;

        readonly string _dateTimeFormatString;
        readonly string _timeSpanFormatString;
        readonly string _enumFormatString;
        readonly string _numberFormatString;

        readonly string _anonymousTypeName;

        public SerializerSettings(
            Encoding encoding, CultureInfo culture,
            bool includeNulls,
            string dateTimeFormatString, string timeSpanFormatString,
            string enumFormatString, string numberFormatString,
            string anonymousTypeName)
        {
            _encoding = encoding;
            _culture = culture;
            _includeNulls = includeNulls;
            _dateTimeFormatString = dateTimeFormatString;
            _timeSpanFormatString = timeSpanFormatString;
            _enumFormatString = enumFormatString;
            _numberFormatString = numberFormatString;
            _anonymousTypeName = anonymousTypeName;
        }

        public Encoding Encoding
        {
            get { return _encoding; }
        }

        public CultureInfo Culture
        {
            get { return _culture; }
        }

        public bool IncludeNulls
        {
            get { return _includeNulls; }
        }

        public string DateTimeFormatString
        {
            get { return _dateTimeFormatString; }
        }

        public string TimeSpanFormatString
        {
            get { return _timeSpanFormatString; }
        }

        public string EnumFormatString
        {
            get { return _enumFormatString; }
        }

        public string NumberFormatString
        {
            get { return _numberFormatString; }
        }

        public string AnonymousTypeName
        {
            get { return _anonymousTypeName; }
        }
    }
}