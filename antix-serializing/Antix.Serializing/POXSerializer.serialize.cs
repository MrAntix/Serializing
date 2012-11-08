using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Antix.Serializing
{
    public partial class POXSerializer
    {
        public void Serialize(
            TextWriter writer, object value, string name)
        {
            if (value == null) return;

            var type = value.GetType();

            WriteTag(writer, value, type, name);
        }

        void WriteTag(
            TextWriter writer, object value,
            MemberInfo memberInfo, string name)
        {
            if (name == null)
                name = _nameProvider.Get(memberInfo);

            value = _formatterProvider.Get(memberInfo, value);

            if (value == null)
            {
                if (Settings.IncludeNulls)
                    writer.Write("<{0} nill=\"true\"/>", name);
            }
            else
            {
                writer.Write("<{0}>", name);
                WriteTagContent(writer, value, memberInfo);
                writer.Write("</{0}>", name);
            }
        }

        void WriteTagContent(TextWriter writer, object value, MemberInfo memberInfo)
        {
            var type = value.GetType(); // value is never null see WriteTag

            var notNullableType = type.GetNonNullableType();
            if (notNullableType == typeof(DateTime)
                || notNullableType == typeof(DateTimeOffset))
            {
                WriteValue(writer, value, Settings.DateTimeFormatString);
                return;
            }

            if (notNullableType == typeof(TimeSpan))
            {
                WriteValue(writer, value, Settings.TimeSpanFormatString);
                return;
            }

            if (notNullableType.IsEnum)
            {
                WriteValue(writer, value, Settings.EnumFormatString);
                return;
            }

            var typeCode = Type.GetTypeCode(notNullableType);

            if (!type.IsArray && typeCode.IsNumeric())
            {
                WriteValue(writer, value, Settings.NumberFormatString);
                return;
            }

            switch (typeCode)
            {
                case TypeCode.Object:
                    if (type.IsEnumerable())
                    {
                        WriteObjects(
                            writer, value as IEnumerable,
                            _itemNameProvider.TryGet(memberInfo));
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

        void WriteObjects(TextWriter writer, IEnumerable values, string name)
        {
            foreach (var value in values)
            {
                Serialize(writer, value, name);
            }
        }

        void WriteObject(TextWriter writer, object value, IReflect type)
        {
            foreach (var property in type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyValue = property.GetValue(value, new object[] { });

                WriteTag(writer, propertyValue, property, null);
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