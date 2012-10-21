using System;

namespace Antix.Serializing
{
    public static class SerializerBuilderExtensions
    {
        public static ISerializerBuilder Format<T>(
            this ISerializerBuilder builder,
            string format)
        {
            return builder.Format<T>(v =>
                                     string.Format(string.Concat("{0:", format, "}"), v));
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
                (v, t, n) => t == typeof (T),
                v => format((T) v));
        }
    }
}