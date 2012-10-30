using System.Reflection;

namespace Antix.Serializing.Builders
{
    public class SerializerPropertyConfiguration<TValue> :
        SerializerConfiguration<SerializerPropertyConfiguration<TValue>, TValue>
    {
        public SerializerPropertyConfiguration(MemberInfo member) :
            base(member)
        {
        }
    }
}