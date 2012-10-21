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
                = new Dictionary<Func<object, Type, string, bool>, Func<object, string>>
                      {
                          //{
                          //    (v, t, n) => t == typeof (DateTimeOffset)
                          //                 || t == typeof (DateTime),
                          //    v => string.Format("{0:s}", v)
                          //}
                      };
            _settings = new SerializerSettings();
        }

        public SerializerBuilder Format(
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
            return this;
        }

        public ISerializerBuilder IgnoreCulture()
        {
            _settings.FormatProvider = CultureInfo.InvariantCulture;
            return this;
        }
    }
}