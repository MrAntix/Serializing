using System;
using System.Collections.Generic;
using System.Reflection;

namespace Antix.Serializing.Providers
{
    public class FormatterProvider : TypeKeyedCache<Func<object, string>>
    {
        public FormatterProvider(IEnumerable<Tuple<MemberInfo, Func<object, string>>> formatters) :
            base(formatters)
        {
        }

        public FormatterProvider(IDictionary<MemberInfo, Func<object, string>> formatters) :
            base(formatters)
        {
        }

        public object Get(MemberInfo memberInfo, object value)
        {
            Func<object, string> formatter;
            return TryGet(memberInfo, out formatter)
                       ? formatter(value)
                       : value;
        }
    }
}