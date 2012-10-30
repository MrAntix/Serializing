using System;
using System.Globalization;
using System.Text;

namespace Antix.Serializing.Abstraction.Builders
{
    public static class Extensions
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

        public static ISerializerBuilder UseCulture(
            this ISerializerBuilder builder,
            string cultureName)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            return builder.UseCulture(culture);
        }

        public static ISerializerBuilder UseEncoding(
            this ISerializerBuilder builder,
            string encodingName)
        {
            var culture = Encoding.GetEncoding(encodingName);
            return builder.UseEncoding(culture);
        }
    }
}