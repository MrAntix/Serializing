using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Antix.Serializing
{
    public class SerializerBuilder
    {
        readonly IDictionary<Func<PropertyInfo, bool>, Func<object, string>> _formatters;

        public SerializerBuilder()
        {
            _formatters
                = new Dictionary<Func<PropertyInfo, bool>, Func<object, string>>
                      {
                          {
                              p => p.PropertyType == typeof (DateTimeOffset)
                                   || p.PropertyType == typeof (DateTime),
                              v => string.Format("{0:s}", v)
                          }
                      };
        }

        public SerializerBuilder Format<T>(string format)
        {
            return Format<T>(v =>
                             string.Format(string.Concat("{0:", format, "}"), v));
        }

        public SerializerBuilder Format<T>(IFormatProvider format)
        {
            return Format<T>(v => string.Format(format, "{0}", v));
        }

        SerializerBuilder Format<T>(Func<T, string> format)
        {
            _formatters.Add(
                pi => Type.GetTypeCode(pi.PropertyType) == Type.GetTypeCode(typeof (T)),
                v => format((T) v));

            return this;
        }

        public POXSerializer Create()
        {
            return Create(new SerializerSettings());
        }

        public POXSerializer Create(
            IPOXSerializerSettings settings)
        {
            return new POXSerializer(
                new ReadOnlyDictionary<Func<PropertyInfo, bool>, Func<object, string>>(_formatters),
                settings);
        }
    }
}