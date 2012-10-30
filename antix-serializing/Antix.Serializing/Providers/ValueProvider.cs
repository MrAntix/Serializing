using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Antix.Serializing.Providers
{
    public class ValueProvider
    {
        readonly IReadOnlyDictionary<MemberInfo, Func<object, string>> _formatters;

        public ValueProvider(IEnumerable<Tuple<MemberInfo, Func<object, string>>> names) :
            this(names.ToDictionary(t => t.Item1, t => t.Item2))
        {
        }

        public ValueProvider(IDictionary<MemberInfo, Func<object, string>> names)
        {
            _formatters = new ReadOnlyDictionary<MemberInfo, Func<object, string>>(names);
        }

        public object Get(MemberInfo memberInfo, object value)
        {
            if (_formatters.ContainsKey(memberInfo))
                return _formatters[memberInfo](value);

            var type = memberInfo as Type;
            if (type != null)
            {
                var notNullableType = type.GetNonNullableType();
                if (type != notNullableType
                    && _formatters.ContainsKey(notNullableType))
                    return _formatters[notNullableType](value);
            }

            var propertyInfo = memberInfo as PropertyInfo;
            return propertyInfo != null 
                ? Get(propertyInfo.PropertyType,value) 
                : value;
        }
    }
}