using System;
using System.IO;

namespace Antix.Serializing.Abstraction
{
    public interface ISerializer
    {
        /// <summary>
        /// <para>Serializer Settings</para>
        /// </summary>
        SerializerSettings Settings { get; }

        void Serialize(TextWriter writer, object value, string name);

        object Deserialize(Stream input, Type type);
    }
}