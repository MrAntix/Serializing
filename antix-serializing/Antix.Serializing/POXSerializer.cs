using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Antix.Serializing
{
    public class POXSerializer :
        ISerializer
    {
        readonly IReadOnlyDictionary<
            Func<object, Type, string, bool>,
            Func<object, string>> _formatters;

        internal POXSerializer(
            IReadOnlyDictionary<Func<object, Type, string, bool>, Func<object, string>> formatters,
            ISerializerSettings settings)
        {
            _formatters = formatters;
            _formatProvider = settings.FormatProvider;
            _encoding = settings.Encoding;
            _includeNulls = settings.IncludeNulls;
            _dateTimeFormatString = settings.DateTimeFormatString;
            _timeSpanFormatString = settings.TimeSpanFormatString;
            _enumFormatString = settings.EnumFormatString;
            _numberFormatString = settings.NumberFormatString;
        }

        readonly Encoding _encoding;
        readonly IFormatProvider _formatProvider;

        readonly bool _includeNulls;
        readonly string _dateTimeFormatString;
        readonly string _numberFormatString;
        readonly string _timeSpanFormatString;
        readonly string _enumFormatString;

        public Encoding Encoding
        {
            get { return _encoding; }
        }

        public IFormatProvider FormatProvider
        {
            get { return _formatProvider; }
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

        public void Serialize(TextWriter writer, object value)
        {
            if (value == null) return;

            var type = GetNonNullableType(value.GetType());

            Serialize(writer, value, type, Head(type.Name, "`"));
        }

        void Serialize(TextWriter writer, object value, Type type)
        {
            if (type == typeof (DateTime)
                || type == typeof (DateTimeOffset))
            {
                Write(writer, value, _dateTimeFormatString);
                return;
            }

            if (type == typeof (TimeSpan))
            {
                Write(writer, value, _timeSpanFormatString);
                return;
            }

            if (type.IsEnum)
            {
                Write(writer, value, _enumFormatString);
                return;
            }

            var typeCode = Type.GetTypeCode(type);

            if (!type.IsArray && IsNumericTypeCode(typeCode))
            {
                Write(writer, value, _numberFormatString);
                return;
            }

            switch (typeCode)
            {
                case TypeCode.Object:
                    if (IsEnumerable(type))
                    {
                        Serialize(writer, value as IEnumerable);
                    }
                    else
                    {
                        Serialize(writer, value, (IReflect) type);
                    }
                    break;
                default:
                    writer.Write(value);
                    break;
            }
        }

        void Serialize(TextWriter writer, object value, Type type, string name)
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
                if (_includeNulls)
                    writer.Write("<{0} nill=\"true\"/>", name);
            }
            else
            {
                writer.Write("<{0}>", name);
                Serialize(writer, value, GetNonNullableType(value.GetType()));
                writer.Write("</{0}>", name);
            }
        }

        void Serialize(TextWriter writer, IEnumerable values)
        {
            foreach (var value in values)
            {
                Serialize(writer, value);
            }
        }

        void Serialize(TextWriter writer, object value, IReflect type)
        {
            foreach (var property in type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyValue = property.GetValue(value, new object[] {});
                var propertyType =
                    propertyValue == null
                        ? property.PropertyType
                        : propertyValue.GetType();

                Serialize(writer, propertyValue, propertyType, property.Name);
            }
        }

        static void Write(TextWriter writer, object value, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                writer.Write(value);
            }
            else
            {
                writer.Write(
                    string.Concat("{0:", format, "}"),
                    value
                    );
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

        static Type GetNonNullableType(Type type)
        {
            return type.IsGenericType
                       ? type.GetGenericTypeDefinition() == typeof (Nullable<>)
                             ? Nullable.GetUnderlyingType(type)
                             : type
                       : type;
        }

        static bool IsNumericTypeCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        static string Head(string value, string cut)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var index = value.IndexOf(cut, StringComparison.Ordinal);
            return index == -1 ? value : value.Substring(0, index);
        }
    }
}