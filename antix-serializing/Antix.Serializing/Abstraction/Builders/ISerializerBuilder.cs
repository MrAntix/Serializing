using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Antix.Serializing.Builders;

namespace Antix.Serializing.Abstraction.Builders
{
    public interface ISerializerBuilder
    {
        ISerializer Build();

        ISerializerBuilder Format(
            MemberInfo memberInfo,
            Func<object, string> format);

        ISerializerBuilder Type<T>(
            Action<SerializerTypeConfiguration<T>> assign);

        ISerializerBuilder ExcludeNulls();
        ISerializerBuilder IncludeNulls();

        ISerializerBuilder UseThreadCulture();
        ISerializerBuilder UseCulture(CultureInfo culture);
        ISerializerBuilder IgnoreCulture();

        ISerializerBuilder UseEncoding(Encoding encoding);

        ISerializerBuilder EnumAsNumber();
        ISerializerBuilder EnumAsName();

        ISerializerBuilder DateTimeFormatString(string value);
        ISerializerBuilder TimeSpanFormatString(string value);
        ISerializerBuilder EnumFormatString(string value);
        ISerializerBuilder NumberFormatString(string value);
    }
}