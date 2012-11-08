using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Antix.Serializing
{
    public partial class POXSerializer
    {
        public object Deserialize(Stream input, Type type)
        {
            using (var reader = XmlReader.Create(input))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            return Read(reader, type);
                    }
                }
            }

            return null;
        }

        object Read(XmlReader reader, Type type)
        {
            var obj = Activator.CreateInstance(type);

            if (reader.HasAttributes)
            {
                reader.MoveToFirstAttribute();
                do
                {
                    var propertyInfo = type.GetProperty(reader.Name);
                    propertyInfo
                        .SetValue(
                            obj,
                            Convert.ChangeType(reader.Value, propertyInfo.PropertyType)
                        );
                } while (reader.MoveToNextAttribute());
            }

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var propertyInfo = type.GetProperty(reader.Name);
                        propertyInfo
                            .SetValue(
                                obj,
                                ReadValue(reader, propertyInfo.PropertyType)
                            );
                        break;
                }
            }

            return obj;
        }

        object ReadValue(XmlReader reader, Type type)
        {
            var notNullableType = type.GetNonNullableType();
            if (notNullableType == typeof (DateTime)
                || notNullableType == typeof (DateTimeOffset))
            {
                reader.Read();
                DateTimeOffset value;
                if (DateTimeOffset.TryParseExact(
                    reader.Value,
                    Settings.DateTimeFormatString, Settings.Culture, DateTimeStyles.None,
                    out value))
                {
                    return value;
                }

                if (notNullableType != type) return type.GetDefault();

                throw new DeserializationException(reader.Value, type);
            }

            var typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.Object)
            {
                return Read(reader, type);
            }

            reader.Read();
            return Convert.ChangeType(reader.Value, type);
        }
    }
}