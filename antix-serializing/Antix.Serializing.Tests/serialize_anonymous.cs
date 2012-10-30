using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Builders;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_anonymous
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void uses_default_name_for_anonymous()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new
                                           {
                                               AProperty = "Hello"
                                           });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/Anon/AProperty").Value);
        }

        [Fact]
        public void uses_default_name_for_anonymous_as_sub_object()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(
                new HasObject
                    {
                        Value = new {ASubObjectProperty = "Hello"}
                    });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/HasObject/Value/ASubObjectProperty").Value);
        }

        [Fact]
        public void uses_default_name_for_anonymous_as_sub_object_array()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(
                new HasObject
                {
                    ValueArray = new object[] { new { ASubObjectProperty = "Hello" } }
                });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/HasObject/ValueArray/Anon/ASubObjectProperty").Value);
        }

        [Fact]
        public void uses_property_name_as_sub_object_of_anonymous()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(
                new
                    {
                        AProperty = new {ASubObjectProperty = "Hello"}
                    });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/Anon/AProperty/ASubObjectProperty").Value);
        }
    }
}