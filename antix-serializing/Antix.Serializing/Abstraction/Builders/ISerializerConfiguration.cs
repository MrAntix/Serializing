using System;
using System.Reflection;

namespace Antix.Serializing.Abstraction.Builders
{
    internal interface ISerializerConfiguration
    {
        MemberInfo MemberInfo { get; }
        string Name { get; }
        string ItemName { get; }
        Func<object, string> Formatter { get; set; }
    }
}