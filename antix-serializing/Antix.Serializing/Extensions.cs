using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Antix.Serializing.Abstraction;
using Antix.Serializing.IO;

namespace Antix.Serializing
{
    public static class Extensions
    {
        public static string Serialize(
            this ISerializer serializer,
            object value)
        {
            if (value == null) return string.Empty;

            var stream = new MemoryStream();
            using (var writer = new FormattingWriter(
                stream,
                serializer.Encoding,
                serializer.FormatProvider ?? Thread.CurrentThread.CurrentCulture))
            {
                serializer.Serialize(writer, value);

                writer.Flush();
                stream.Seek(0, 0);

                return serializer.Encoding.GetString(stream.ToArray());
            }
        }

        public static Type GetNonNullableType(this Type type)
        {
            return type.IsGenericType
                       ? type.GetGenericTypeDefinition() == typeof (Nullable<>)
                             ? Nullable.GetUnderlyingType(type)
                             : type
                       : type;
        }

        public static bool IsNumeric(this TypeCode typeCode)
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

        public static bool IsEnumerable(this Type type)
        {
            return
                typeof(IEnumerable).IsAssignableFrom(type)
                || type.GetInterfaces()
                       .Any(i => i.IsGenericType
                                 && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static bool IsAnonymous(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static string Head(this string value, string cut)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var index = value.IndexOf(cut, StringComparison.Ordinal);
            return index == -1 ? value : value.Substring(0, index);
        }
    }
}