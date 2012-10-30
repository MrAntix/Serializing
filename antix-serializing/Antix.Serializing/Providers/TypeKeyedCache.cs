using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Antix.Serializing.Providers
{
    public abstract class TypeKeyedCache<TContent>
    {
        readonly IReadOnlyDictionary<MemberInfo, TContent> _content;

        protected TypeKeyedCache(IEnumerable<Tuple<MemberInfo, TContent>> content) :
            this(content.ToDictionary(t => t.Item1, t => t.Item2))
        {
        }

        protected TypeKeyedCache(IDictionary<MemberInfo, TContent> content)
        {
            _content = new ReadOnlyDictionary<MemberInfo, TContent>(content);
        }

        protected bool TryGet(MemberInfo memberInfo, out TContent content)
        {
            if (_content.ContainsKey(memberInfo))
            {
                content = _content[memberInfo];
                return true;
            }

            var type = memberInfo as Type;
            if (type != null)
            {
                var notNullableType = type.GetNonNullableType();
                if (type != notNullableType
                    && _content.ContainsKey(notNullableType))
                {
                    content = _content[notNullableType];
                    return true;
                }
            }

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return TryGet(propertyInfo.PropertyType, out content);
            }

            content = default(TContent);
            return false;
        }
    }
}