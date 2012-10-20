using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Antix.Serializing
{
    public class POXSerializer:
        ISerializer
    {
        readonly IReadOnlyDictionary<Func<object, Type, string, bool>, Func<object, string>> _formatters;

        internal POXSerializer(
            IReadOnlyDictionary<Func<object, Type, string, bool>, Func<object, string>> formatters,
            ISerializerSettings settings)
        {
            _formatters = formatters;
            _encoding = settings.Encoding;
            _includeNulls = settings.IncludeNulls;
        }

        readonly Encoding _encoding;
        readonly bool _includeNulls;

        public Encoding Encoding
        {
            get { return _encoding; }
        }

        public bool IncludeNulls
        {
            get { return _includeNulls; }
        }

        public void Serialize(TextWriter writer, object value)
        {
            if (value == null) return;

            var type = value.GetType();

            WriteValue(writer, value, type, type.Name);
        }

        void Write(TextWriter writer, object value, Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    if (IsEnumerable(type))
                    {
                        WriteObjects(writer, value as IEnumerable);
                    }
                    else
                    {
                        WriteObject(writer, value, type);
                    }
                    break;
                default:
                    writer.Write(value);
                    break;
            }
        }

        void WriteObject(TextWriter writer, object value, IReflect type)
        {
            foreach (var property in type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyValue = property.GetValue(value, new object[] {});
                var propertyType = propertyValue == null
                                       ? property.PropertyType
                                       : propertyValue.GetType();

                WriteValue(writer, propertyValue, propertyType, property.Name);
            }
        }

        void WriteValue(TextWriter writer, object value, Type type, string name)
        {
            var formatter = (from f in _formatters
                             where f.Key(value, type, name)
                             select f.Value).LastOrDefault();
            if (formatter != null)
            {
                value = formatter(value);
            }

            if (value == null)
            {
                if(_includeNulls)
                    writer.Write("<{0} nill=\"true\"/>", name);
            }
            else
            {
                writer.Write("<{0}>", name);
                Write(writer, value, value.GetType());
                writer.Write("</{0}>", name);
            }
        }

        void WriteObjects(TextWriter writer, IEnumerable values)
        {
            foreach (var value in values)
            {
                Serialize(writer, value);
            }
        }

        static bool IsEnumerable(Type type)
        {
            return
                typeof (IEnumerable).IsAssignableFrom(type)
                || type.GetInterfaces()
                       .Any(i => i.IsGenericType
                                 && i.GetGenericTypeDefinition() == typeof (IEnumerable<>));
        }
    }
}