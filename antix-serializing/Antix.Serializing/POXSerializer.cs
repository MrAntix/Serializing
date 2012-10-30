using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Antix.Serializing.Abstraction;
using Antix.Serializing.Providers;

namespace Antix.Serializing
{
    public class POXSerializer :
        ISerializer
    {
        readonly NameProvider _nameProvider;
        readonly ValueProvider _valueProvider;

        internal POXSerializer(
            NameProvider nameProvider,
            ValueProvider valueProvider,
            ISerializerSettings settings)
        {
            _nameProvider = nameProvider;
            _valueProvider = valueProvider;

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

            var type = value.GetType();

            WriteTag(writer, value, type);
        }

        void WriteTag(TextWriter writer, object value, MemberInfo memberInfo)
        {
            var name = _nameProvider.Get(memberInfo);
            value = _valueProvider.Get(memberInfo, value);

            if (value == null)
            {
                if (_includeNulls)
                    writer.Write("<{0} nill=\"true\"/>", name);
            }
            else
            {
                writer.Write("<{0}>", name);
                WriteTagContent(writer, value, value.GetType());
                writer.Write("</{0}>", name);
            }
        }

        void WriteTagContent(TextWriter writer, object value, Type type)
        {
            var notNullableType = type.GetNonNullableType();
            if (notNullableType == typeof(DateTime)
                || notNullableType == typeof(DateTimeOffset))
            {
                WriteValue(writer, value, _dateTimeFormatString);
                return;
            }

            if (notNullableType == typeof(TimeSpan))
            {
                WriteValue(writer, value, _timeSpanFormatString);
                return;
            }

            if (notNullableType.IsEnum)
            {
                WriteValue(writer, value, _enumFormatString);
                return;
            }

            var typeCode = Type.GetTypeCode(notNullableType);

            if (!type.IsArray && typeCode.IsNumeric())
            {
                WriteValue(writer, value, _numberFormatString);
                return;
            }

            switch (typeCode)
            {
                case TypeCode.Object:
                    if (type.IsEnumerable())
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

        void WriteObjects(TextWriter writer, IEnumerable values)
        {
            foreach (var value in values)
            {
                Serialize(writer, value);
            }
        }

        void WriteObject(TextWriter writer, object value, IReflect type)
        {
            foreach (var property in type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyValue = property.GetValue(value, new object[] {});

                WriteTag(writer, propertyValue, property);
            }
        }

        static void WriteValue(TextWriter writer, object value, string format)
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
    }
}