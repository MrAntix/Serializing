using System;
using Antix.Serializing.Abstraction.Builders;

namespace Antix.Serializing.Builders
{
    public static class SerializerBuilderExtensions
    {
        public static ISerializerBuilder Format<T>(
            this ISerializerBuilder builder,
            string format)
        {
            return builder.Format<T>(
                v => string.Format(string.Concat("{0:", format, "}"), v));
        }

        public static ISerializerBuilder Format<T>(
            this ISerializerBuilder builder,
            IFormatProvider format)
        {
            return builder.Format<T>(v => string.Format(format, "{0}", v));
        }

        public static ISerializerBuilder Format<T>(
            this ISerializerBuilder builder,
            Func<T, string> format)
        {
            return builder.Format(
                typeof (T),
                v => v == null ? null : format((T) v));
        }
    }
}