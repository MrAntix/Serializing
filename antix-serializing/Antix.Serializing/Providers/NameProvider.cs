using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Antix.Serializing.Providers
{
    public class NameProvider
    {
        readonly IReadOnlyDictionary<MemberInfo, string> _names;

        public NameProvider(IEnumerable<Tuple<MemberInfo, string>> names) :
            this(names.ToDictionary(t => t.Item1, t => t.Item2))
        {
        }

        public NameProvider(IDictionary<MemberInfo, string> names)
        {
            _names = new ReadOnlyDictionary<MemberInfo, string>(names);
        }

        public string Get(MemberInfo type)
        {
            return _names.ContainsKey(type)
                       ? _names[type]
                       : type.Name.Head("`");
        }
    }
}