using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Antix.Serializing
{
    public class SerializerBuilder:
        ISerializerBuilder
    {
        readonly IDictionary<Func<object, Type, string, bool>, Func<object, string>> _formatters;

        public SerializerBuilder()
        {
            _formatters
                = new Dictionary<Func<object, Type, string, bool>, Func<object, string>>
                      {
                          {
                              (v, t, n) => t == typeof (DateTimeOffset)
                                           || t == typeof (DateTime),
                              v => string.Format("{0:s}", v)
                          }
                      };
        }

        public SerializerBuilder Format(
            Func<object, Type, string, bool> canFormat,
            Func<object, string> format)
        {
            _formatters.Add(canFormat, format);

            return this;
        }

        public POXSerializer Create(
            ISerializerSettings settings)
        {
            return new POXSerializer(
                new ReadOnlyDictionary<Func<object, Type, string, bool>, Func<object, string>>(_formatters),
                settings);
        }
    }
}