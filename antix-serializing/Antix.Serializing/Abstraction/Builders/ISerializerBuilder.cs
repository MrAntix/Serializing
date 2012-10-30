using System;
using System.Reflection;
using Antix.Serializing.Builders;

namespace Antix.Serializing.Abstraction.Builders
{
    public interface ISerializerBuilder
    {
        POXSerializer Build();

        ISerializerBuilder Format(
            MemberInfo memberInfo,
            Func<object, string> format);

        ISerializerBuilder Type<T>(
            Action<SerializerTypeConfiguration<T>> assign);

        ISerializerBuilder ExcludeNulls();
        ISerializerBuilder IncludeNulls();

        ISerializerBuilder UseThreadCulture();
        ISerializerBuilder UseCulture(string cultureName);
        ISerializerBuilder IgnoreCulture();

        ISerializerBuilder EnumAsNumber();
        ISerializerBuilder EnumAsName();

        ISerializerBuilder DateTimeFormatString(string value);
        ISerializerBuilder TimeSpanFormatString(string value);
        ISerializerBuilder EnumFormatString(string value);
        ISerializerBuilder NumberFormatString(string value);
    }
}