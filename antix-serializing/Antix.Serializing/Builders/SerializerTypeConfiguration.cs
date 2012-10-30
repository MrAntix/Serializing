using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Antix.Serializing.Abstraction.Builders;

namespace Antix.Serializing.Builders
{
    public class SerializerTypeConfiguration<T> :
        SerializerConfiguration<SerializerTypeConfiguration<T>, T>,
        ISerializerTypeConfiguration
    {
        readonly List<ISerializerConfiguration> _propertyConfigurations;

        public SerializerTypeConfiguration() :
            base(typeof (T))
        {
            _propertyConfigurations = new List<ISerializerConfiguration>();
        }

        public SerializerTypeConfiguration<T> Property<TValue>(
            Expression<Func<T, TValue>> propertyExpression,
            Action<SerializerPropertyConfiguration<TValue>> assign)
        {
            if (propertyExpression == null) throw new ArgumentNullException("propertyExpression");
            if (assign == null) throw new ArgumentNullException("assign");

            var memberInfo = GetMember(propertyExpression);
            var propertyConfiguration
                = new SerializerPropertyConfiguration<TValue>(memberInfo);

            _propertyConfigurations.Add(propertyConfiguration);

            assign(propertyConfiguration);

            return this;
        }

        IEnumerable<ISerializerConfiguration> ISerializerTypeConfiguration.Properties
        {
            get { return _propertyConfigurations; }
        }

        static MemberInfo GetMember<TValue>(Expression<Func<T, TValue>> propertyExpression)
        {
            var lambda = propertyExpression as LambdaExpression;
            var method = lambda.Body as MemberExpression;
            if (method == null) throw new SerializerException("Expected property expression");

            return method.Member;
        }
    }
}