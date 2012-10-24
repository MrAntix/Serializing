using System;
using System.Runtime.Serialization;

namespace Antix.Serializing
{
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
}