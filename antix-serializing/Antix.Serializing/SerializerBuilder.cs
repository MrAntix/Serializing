using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Antix.Serializing
{
    public class SerializerBuilder :
        ISerializerBuilder
    {
        readonly IDictionary<Func<object, Type, string, bool>, Func<object, string>> _formatters;
        ISerializerSettings _settings;

        public SerializerBuilder()
        {
            _formatters
                = new Dictionary<Func<object, Type, string, bool>, Func<object, string>>();
            _settings = new SerializerSettings();
        }

        public ISerializerBuilder Format(
            Func<object, Type, string, bool> canFormat,
            Func<object, string> format)
        {
            _formatters.Add(canFormat, format);

            return this;
        }

        public POXSerializer Create()
        {
            return new POXSerializer(
                new ReadOnlyDictionary<Func<object, Type, string, bool>, Func<object, string>>(_formatters),
                _settings);
        }

        public ISerializerBuilder Settings(ISerializerSettings settings)
        {
            _settings = settings;

            return this;
        }

        public ISerializerBuilder EnumAsNumber()
        {
            _settings.EnumFormatString = "d";

            return this;
        }

        public ISerializerBuilder EnumAsName()
        {
            _settings.EnumFormatString = "g";

            return this;
        }

        public ISerializerBuilder DateTimeFormatString(string value)
        {
            _settings.DateTimeFormatString = value;

            return this;
        }

        public ISerializerBuilder TimeSpanFormatString(string value)
        {
            _settings.TimeSpanFormatString = value;

            return this;
        }

        public ISerializerBuilder EnumFormatString(string value)
        {
            _settings.EnumFormatString = value;

            return this;
        }

        public ISerializerBuilder NumberFormatString(string value)
        {
            _settings.NumberFormatString = value;

            return this;
        }

        public ISerializerBuilder IncludeNulls()
        {
            _settings.IncludeNulls = true;
            return this;
        }

        public ISerializerBuilder ExcludeNulls()
        {
            _settings.IncludeNulls = false;
            return this;
        }

        public ISerializerBuilder UseThreadCulture()
        {
            _settings.FormatProvider = null;
            return this;
        }

        public ISerializerBuilder UseCulture(string cultureName)
        {
            _settings.FormatProvider = CultureInfo.GetCultureInfo(cultureName);
            _settings.DateTimeFormatString = "g";
            return this;
        }

        public ISerializerBuilder IgnoreCulture()
        {
            _settings.FormatProvider = CultureInfo.InvariantCulture;
            _settings.DateTimeFormatString = "s";
            return this;
        }
    }
}