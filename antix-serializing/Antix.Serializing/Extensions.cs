using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Antix.Serializing.Abstraction;
using Antix.Serializing.IO;

namespace Antix.Serializing
{
    public static class Extensions
    {
        public static T Deserialize<T>(
            this ISerializer serializer,
            string value)
        {
            return (T)Deserialize(serializer, value, typeof(T));
        }

        public static object Deserialize(
            this ISerializer serializer,
            string value,
            Type type)
        {
            using (var stream = new MemoryStream(
                serializer.Settings.Encoding.GetBytes(value)))
            {
                return serializer.Deserialize(stream, type);
            }
        }

        public static string Serialize(
            this ISerializer serializer,
            object value)
        {
            return Serialize(serializer, value, null);
        }

        public static string Serialize(
            this ISerializer serializer,
            object value,
            string name)
        {
            if (value == null) return string.Empty;

            var stream = new MemoryStream();
            using (var writer = new FormattingWriter(
                stream,
                serializer.Settings.Encoding,
                serializer.Settings.Culture ?? Thread.CurrentThread.CurrentCulture))
            {
                serializer.Serialize(writer, value, name);

                writer.Flush();
                stream.Seek(0, 0);

                return serializer.Settings.Encoding.GetString(stream.ToArray());
            }
        }

        public static void Serialize(
            this ISerializer serializer, TextWriter writer, object value)
        {
            serializer.Serialize(writer, value, null);
        }

        public static Type GetNonNullableType(this Type type)
        {
            return type.IsGenericType
                       ? type.GetGenericTypeDefinition() == typeof (Nullable<>)
                             ? Nullable.GetUnderlyingType(type)
                             : type
                       : type;
        }

        public static Type GetPropertyTypeOrType(this MemberInfo memberInfo)
        {
            var propertyInfo = memberInfo as PropertyInfo;

            return propertyInfo != null ? propertyInfo.PropertyType: memberInfo as Type;
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
                typeof (IEnumerable).IsAssignableFrom(type)
                || type.GetInterfaces()
                       .Any(i => i.IsGenericType
                                 && i.GetGenericTypeDefinition() == typeof (IEnumerable<>));
        }

        public static bool IsAnonymous(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return Attribute.IsDefined(type, typeof (CompilerGeneratedAttribute), false)
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