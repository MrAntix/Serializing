using System;
using System.Collections.Generic;
using System.Reflection;

namespace Antix.Serializing.Providers
{
    public class NameProvider : TypeKeyedCache<string>
    {
        readonly string _anonymousTypeName;
        public NameProvider(
            IEnumerable<Tuple<MemberInfo, string>> names, 
            string anonymousTypeName) :
            base(names)
        {
            _anonymousTypeName = anonymousTypeName;
        }

        public NameProvider(
            IDictionary<MemberInfo, string> names,
            string anonymousTypeName) :
            base(names)
        {
            _anonymousTypeName = anonymousTypeName;
        }

        public string Get(MemberInfo memberInfo)
        {
            string name;
            if (!TryGet(memberInfo, out name))
            {
                var type = memberInfo as Type;
                if (type!=null && type.IsAnonymous())
                    return _anonymousTypeName;

                name = memberInfo.Name.Head("`");
            }

            return name;
        }

        public string TryGet(MemberInfo memberInfo)
        {
            string name;
            TryGet(memberInfo, out name);

            return name;
        }
    }
}