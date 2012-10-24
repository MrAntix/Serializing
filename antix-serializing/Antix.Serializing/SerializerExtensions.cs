﻿using System.IO;
using System.Threading;
using Antix.Serializing.IO;

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
}