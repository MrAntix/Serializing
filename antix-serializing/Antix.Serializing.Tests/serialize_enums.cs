using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Builders;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_enums
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void standard()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new HasEnum());

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("One", xml.XPathSelectElement("/HasEnum/Value").Value);
        }

        [Fact]
        public void flags()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new HasEnum
                                           {
                                               FlagsValue = HasEnum.FlaggedTypes.One | HasEnum.FlaggedTypes.Two
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("One, Two", xml.XPathSelectElement("/HasEnum/FlagsValue").Value);
        }

        [Fact]
        public void nullable_null()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new HasEnum
                                           {
                                               NullableValue = null
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.False(xml.XPathSelectElements("/HasEnum/NullableValue").Any());
        }

        [Fact]
        public void nullable_not_null()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new HasEnum
                                           {
                                               NullableValue = HasEnum.Types.Two
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Two", xml.XPathSelectElement("/HasEnum/NullableValue").Value);
        }

        [Fact]
        public void override_formatting()
        {
            var sut = GetBuilder()
                .EnumAsNumber()
                .Build();

            var result = sut.Serialize(new HasEnum());

            Console.Write(result);

            var xml = XDocument.Parse(result);
            var value = xml.XPathSelectElement("/HasEnum/Value").Value;

            Assert.Equal("0", value);
        }

        [Fact]
        public void override_formatting_by_type()
        {
            var sut = GetBuilder()
                .Format<HasEnum.Types>(v => "Bob")
                .Build();

            var result = sut.Serialize(new HasEnum
                                           {
                                               NullableValue = HasEnum.Types.Two
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Bob", xml.XPathSelectElement("/HasEnum/Value").Value);
            Assert.Equal("Bob", xml.XPathSelectElement("/HasEnum/NullableValue").Value);
        }

        [Fact]
        public void override_formatting_custom()
        {
            var sut = GetBuilder()
                .Type<HasEnum>(
                    t => t.Property(o => o.NullableValue,
                                    p => p.Formatter(v => "Bob")))
                .Build();

            var result = sut.Serialize(new HasEnum());

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Bob", xml.XPathSelectElement("/HasEnum/NullableValue").Value);
        }
    }
}