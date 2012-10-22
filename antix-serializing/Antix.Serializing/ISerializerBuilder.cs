using System;

namespace Antix.Serializing
{
    public interface ISerializerBuilder
    {
        POXSerializer Create();

        ISerializerBuilder Format(
            Func<object, Type, string, bool> canFormat,
            Func<object, string> format);

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