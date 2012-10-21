using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_numbers
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void standard()
        {
            var sut = GetBuilder()
                .Create();

            var result = sut.Serialize(new HasNumber());

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("0", xml.XPathSelectElement("/HasNumber/Value").Value);
        }

        [Fact]
        public void nullable_null()
        {
            var sut = GetBuilder()
                .Create();

            var result = sut.Serialize(new HasNumber
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
                .Create();

            var result = sut.Serialize(new HasNumber
                                           {
                                               NullableValue = 1
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("1", xml.XPathSelectElement("/HasNumber/NullableValue").Value);
        }

        [Fact]
        public void override_formatting()
        {
            var sut = GetBuilder()
                .EnumAsNumber()
                .Create();

            var result = sut.Serialize(new HasNumber());

            Console.Write(result);

            var xml = XDocument.Parse(result);
            var value = xml.XPathSelectElement("/HasNumber/Value").Value;

            Assert.Equal("0", value);
        }

        [Fact]
        public void override_formatting_by_type()
        {
            var sut = GetBuilder()
                .Format<decimal>(v => "Bob")
                .Create();

            var result = sut.Serialize(new HasNumber
                                           {
                                               NullableValue = 1
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Bob", xml.XPathSelectElement("/HasNumber/Value").Value);
            Assert.Equal("Bob", xml.XPathSelectElement("/HasNumber/NullableValue").Value);
        }

        [Fact]
        public void override_formatting_custom()
        {
            var sut = GetBuilder()
                .Format((v, t, n) => n == "NullableValue", v => "Bob")
                .Create();

            var result = sut.Serialize(new HasNumber());

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Bob", xml.XPathSelectElement("/HasNumber/NullableValue").Value);
        }
    }
}