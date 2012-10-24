using System;
using System.Runtime.Serialization;
using Antix.Serializing.Properties;

namespace Antix.Serializing
{
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