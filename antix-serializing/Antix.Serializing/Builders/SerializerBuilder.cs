using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Antix.Serializing.Abstraction;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Providers;

namespace Antix.Serializing.Builders
{
    public class SerializerBuilder :
        ISerializerBuilder
    {
        readonly SerializerBuilderSettings _settings;
        readonly IDictionary<Type, ISerializerTypeConfiguration> _typeConfiguration;

        public SerializerBuilder()
        {
            _settings = new SerializerBuilderSettings();
            _typeConfiguration = new Dictionary<Type, ISerializerTypeConfiguration>();
        }

        public ISerializerBuilder Type<T>(
            Action<SerializerTypeConfiguration<T>> assign)
        {
            if (assign == null) throw new ArgumentNullException("assign");

            var typeConfiguration = GetOrAddTypeConfiguration<T>();

            assign(typeConfiguration);

            return this;
        }

        public ISerializerBuilder Format(
            MemberInfo memberInfo,
            Func<object, string> format)
        {
            var type = (Type) memberInfo;

            var typeConfiguration = GetOrAddTypeConfiguration(
                type,
                () => (ISerializerTypeConfiguration) Activator.CreateInstance(
                    typeof (SerializerTypeConfiguration<>).MakeGenericType(type))
                );
            typeConfiguration.Formatter = format;

            return this;
        }

        SerializerTypeConfiguration<T> GetOrAddTypeConfiguration<T>()
        {
            return (SerializerTypeConfiguration<T>)
                   GetOrAddTypeConfiguration(
                       typeof (T),
                       () => new SerializerTypeConfiguration<T>());
        }

        ISerializerTypeConfiguration GetOrAddTypeConfiguration(
            Type type,
            Func<ISerializerTypeConfiguration> create)
        {
            if (_typeConfiguration.ContainsKey(type))
                return _typeConfiguration[type];

            var typeConfiguration = create();
            _typeConfiguration.Add(type, typeConfiguration);

            return typeConfiguration;
        }

        public ISerializer Build()
        {
            var names = BuildNames();
            var nameProvider = new NameProvider(names, _settings.AnonymousTypeName);

            var itemNames = BuildItemNames();
            var itemNameProvider = new NameProvider(itemNames, _settings.AnonymousTypeName);

            var formatters = BuildFormatters();
            var valueProvider = new FormatterProvider(formatters);

            var settings = BuildSettings();

            return new POXSerializer(
                nameProvider,
                itemNameProvider,
                valueProvider,
                settings);
        }

        SerializerSettings BuildSettings()
        {
            return new SerializerSettings(
                _settings.Encoding,
                _settings.Culture,
                _settings.IncludeNulls,
                _settings.DateTimeFormatString,
                _settings.TimeSpanFormatString,
                _settings.EnumFormatString,
                _settings.NumberFormatString,
                _settings.AnonymousTypeName);
        }

        IEnumerable<Tuple<MemberInfo, Func<object, string>>> BuildFormatters()
        {
            foreach (var typeConfiguration in _typeConfiguration.Values)
            {
                if (typeConfiguration.Formatter != null)
                    yield return Tuple.Create(typeConfiguration.MemberInfo, typeConfiguration.Formatter);

                foreach (var propertyConfiguration in typeConfiguration.Properties
                                                                       .Where(c => c.Formatter != null))
                    yield return Tuple.Create(propertyConfiguration.MemberInfo, propertyConfiguration.Formatter);
            }
        }

        IEnumerable<Tuple<MemberInfo, string>> BuildNames()
        {
            foreach (var typeConfiguration in _typeConfiguration.Values)
            {
                if (typeConfiguration.Name != null)
                    yield return Tuple.Create(typeConfiguration.MemberInfo, typeConfiguration.Name);

                foreach (var propertyConfiguration in typeConfiguration.Properties
                                                                       .Where(c => c.Name != null))
                    yield return Tuple.Create(propertyConfiguration.MemberInfo, propertyConfiguration.Name);
            }
        }

        IEnumerable<Tuple<MemberInfo, string>> BuildItemNames()
        {
            foreach (var typeConfiguration in _typeConfiguration.Values)
            {
                if (typeConfiguration.ItemName != null)
                    yield return Tuple.Create(typeConfiguration.MemberInfo, typeConfiguration.ItemName);

                foreach (var propertyConfiguration in typeConfiguration.Properties
                                                                       .Where(c => c.ItemName != null))
                    yield return Tuple.Create(propertyConfiguration.MemberInfo, propertyConfiguration.ItemName);
            }
        }

        public ISerializerBuilder EnumAsNumber()
        {
            _settings.EnumFormatString = "d";

            return this;
        }

        public ISerializerBuilder EnumAsName()
        {
            _settings.EnumFormatString = "g";

            return this;
        }

        public ISerializerBuilder DateTimeFormatString(string value)
        {
            _settings.DateTimeFormatString = value;

            return this;
        }

        public ISerializerBuilder TimeSpanFormatString(string value)
        {
            _settings.TimeSpanFormatString = value;

            return this;
        }

        public ISerializerBuilder EnumFormatString(string value)
        {
            _settings.EnumFormatString = value;

            return this;
        }

        public ISerializerBuilder NumberFormatString(string value)
        {
            _settings.NumberFormatString = value;

            return this;
        }

        public ISerializerBuilder IncludeNulls()
        {
            _settings.IncludeNulls = true;
            return this;
        }

        public ISerializerBuilder ExcludeNulls()
        {
            _settings.IncludeNulls = false;
            return this;
        }

        public ISerializerBuilder UseThreadCulture()
        {
            _settings.Culture = null;
            return this;
        }

        public ISerializerBuilder UseCulture(CultureInfo culture)
        {
            _settings.Culture = culture;
            _settings.DateTimeFormatString = "g";
            return this;
        }

        public ISerializerBuilder IgnoreCulture()
        {
            _settings.Culture = CultureInfo.InvariantCulture;
            _settings.DateTimeFormatString = "s";
            return this;
        }

        public ISerializerBuilder UseEncoding(Encoding encoding)
        {
            _settings.Encoding = encoding;

            return this;
        }
    }
}