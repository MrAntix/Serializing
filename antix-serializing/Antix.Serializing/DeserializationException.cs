using System;
using System.Runtime.Serialization;
using Antix.Serializing.Properties;

namespace Antix.Serializing
{
    [Serializable]
    public class DeserializationException : SerializerException
    {
        readonly object _value;
        readonly Type _type;

        public DeserializationException(object value, Type type)
            : base(string.Format(
                Resources.DeserializationException_type, value, type))
        {
            _value = value;
            _type = type;
        }

        protected DeserializationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public object Value
        {
            get { return _value; }
        }

        public Type Type
        {
            get { return _type; }
        }
    }
}