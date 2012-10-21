using System;

namespace Antix.Serializing
{
    public interface ISerializerBuilder
    {
        POXSerializer Create();

        SerializerBuilder Format(
            Func<object, Type, string, bool> canFormat,
            Func<object, string> format);
    }
}