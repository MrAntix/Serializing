using System;
using System.Reflection;
using Antix.Serializing.Abstraction.Builders;

namespace Antix.Serializing.Builders
{
    public abstract class SerializerConfiguration<TBuilder, TValue> :
        ISerializerConfiguration
        where TBuilder : class
    {
        protected SerializerConfiguration(MemberInfo memberInfo)
        {
            MemberInfo = memberInfo;
        }

        protected readonly MemberInfo MemberInfo;
        string _name;
        string _itemName;
        Func<object, string> _formatter;

        MemberInfo ISerializerConfiguration.MemberInfo
        {
            get { return MemberInfo; }
        }

        string ISerializerConfiguration.Name
        {
            get { return _name; }
        }

        string ISerializerConfiguration.ItemName
        {
            get { return _itemName; }
        }

        Func<object, string> ISerializerConfiguration.Formatter
        {
            get { return _formatter; }
            set { _formatter = value; }
        }

        public TBuilder Name(
            string value)
        {
            _name = value;

            return this as TBuilder;
        }

        public TBuilder ItemName(
            string value)
        {
            _itemName = value;

            return this as TBuilder;
        }

        public TBuilder Formatter(
            Func<TValue, string> value)
        {
            _formatter = o => value((TValue) o);

            return this as TBuilder;
        }
    }
}