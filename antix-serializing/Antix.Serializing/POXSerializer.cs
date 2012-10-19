using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Antix.Serializing
{
    public class POXSerializer
    {
        readonly IReadOnlyDictionary<Func<PropertyInfo, bool>, Func<object, string>> _formatters;

        internal POXSerializer(
            IReadOnlyDictionary<Func<PropertyInfo, bool>, Func<object, string>> formatters,
            IPOXSerializerSettings settings)
        {
            _formatters = formatters;
            _encoding = settings.Encoding;
        }

        readonly Encoding _encoding;

        public Encoding Encoding
        {
            get { return _encoding; }
        }

        public string Serialize(object value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, _encoding))
            {
                Serialize(writer, value);

                writer.Flush();
                stream.Seek(0, 0);

                return _encoding.GetString(stream.ToArray());
            }
        }

        public void Serialize(TextWriter stream, object value)
        {
            if (value == null) return;

            Serialize(stream, value, value.GetType().Name);
        }

        public void Serialize(TextWriter stream, object value, string name)
        {
            stream.Write("<{0}>", name);

            var type = value.GetType();
            Write(stream, type, value);

            stream.Write("</{0}>", name);
        }

        void Write(TextWriter stream, Type type, object value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    if (IsEnumerable(type))
                    {
                        WriteObjects(stream, value as IEnumerable);
                    }
                    else
                    {
                        WriteObject(stream, value, type);
                    }
                    break;
                default:
                    stream.Write(value);
                    break;
            }
        }

        void WriteObject(TextWriter stream, object value, Type type)
        {
            foreach (var property in type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyValue = property.GetValue(value, new object[] {});
                var formatter = (from f in _formatters
                                 where f.Key(property)
                                 select f.Value).LastOrDefault();
                if (formatter != null)
                {
                    propertyValue = formatter(propertyValue);
                }

                Serialize(
                    stream,
                    propertyValue,
                    property.Name);
            }
        }

        void WriteObjects(TextWriter stream, IEnumerable values)
        {
            foreach (var value in values)
            {
                Serialize(stream, value);
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

        public static SerializerBuilder Format<T>(string format)
        {
            return new SerializerBuilder()
                .Format<T>(format);
        }

        public static SerializerBuilder Format<T>(IFormatProvider format)
        {
            return new SerializerBuilder()
                .Format<T>(format);
        }

        public static POXSerializer Create()
        {
            return Create(new SerializerSettings());
        }

        public static POXSerializer Create(IPOXSerializerSettings settings)
        {
            return new SerializerBuilder()
                .Create(settings);
        }
    }
}