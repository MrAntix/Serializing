using System.IO;

namespace Antix.Serializing
{
    public static class SerializerExtensions{

    public static string Serialize(
            this ISerializer serializer,
            object value)
        {
            if (value == null) return string.Empty;

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, serializer.Encoding))
            {
                serializer.Serialize(writer, value);

                writer.Flush();
                stream.Seek(0, 0);

                return serializer.Encoding.GetString(stream.ToArray());
            }
        }
    }
}