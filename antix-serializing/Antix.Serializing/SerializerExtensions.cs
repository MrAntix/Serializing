using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using Antix.Serializing.IO;
using Antix.Serializing.Properties;

namespace Antix.Serializing
{
    public static class SerializerExtensions
    {
        public static string Serialize(
            this ISerializer serializer,
            object value)
        {
            return Serialize(serializer, value, null);
        }

        public static string Serialize(
            this ISerializer serializer,
            object value, string name)
        {
            if (value == null) return string.Empty;

            var stream = new MemoryStream();
            using (var writer = new FormattingWriter(
                stream,
                serializer.Encoding,
                serializer.FormatProvider ?? Thread.CurrentThread.CurrentCulture))
            {
                serializer.Serialize(writer, value, name);

                writer.Flush();
                stream.Seek(0, 0);

                return serializer.Encoding.GetString(stream.ToArray());
            }
        }
    }

    [Serializable]
    public class SerializerException : Exception
    {
        public SerializerException(string message)
            : base(message)
        {
        }

        protected SerializerException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class AnonymousTypeSerializerException : SerializerException
    {
        public AnonymousTypeSerializerException()
            : base(Resources.AnonymousTypeSerializerException_message)
        {
        }

        protected AnonymousTypeSerializerException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}