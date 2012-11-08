using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Antix.Serializing.Abstraction;
using Antix.Serializing.Providers;

namespace Antix.Serializing
{


    public partial class POXSerializer :
        ISerializer
    {
        readonly NameProvider _nameProvider;
        readonly NameProvider _itemNameProvider;
        readonly FormatterProvider _formatterProvider;
        readonly SerializerSettings _settings;

        internal POXSerializer(
            NameProvider nameProvider,
            NameProvider itemNameProvider,
            FormatterProvider formatterProvider,
            SerializerSettings settings)
        {
            _nameProvider = nameProvider;
            _itemNameProvider = itemNameProvider;
            _formatterProvider = formatterProvider;
            _settings = settings;
        }

        public SerializerSettings Settings
        {
            get { return _settings; }
        }
    }
}