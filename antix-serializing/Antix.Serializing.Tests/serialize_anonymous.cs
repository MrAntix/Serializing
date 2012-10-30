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
        public void throws_when_name_not_supplied()
        {
            var sut = GetBuilder()
                .Build();

            Assert.Throws<AnonymousTypeSerializerException>(
                () => sut.Serialize(new
                                        {
                                            AProperty = "Hello"
                                        })
                );
        }

        [Fact]
        public void does_not_throw_when_name_supplied()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new
                                           {
                                               AProperty = "Hello"
                                           });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/Object/AProperty").Value);
        }

        [Fact]
        public void does_not_throw_as_sub_object()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new HasObject
                                           {
                                               Value = new {ASubObjectProperty = "Hello"}
                                           });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/HasObject/Value/ASubObjectProperty").Value);
        }

        [Fact]
        public void does_not_throw_as_sub_object_array()
        {
            var sut = GetBuilder()
                .Type<HasObject>(
                    c => c.Name("Object")
                          .Property(o => o.ValueArray,
                                    pc => pc.Name("Barry")
                                            .Formatter(v => "Paul")))
                .Type<Object>(c => { })
                .Build();

            var result = sut.Serialize(new HasObject
                                           {
                                               ValueArray = new object[] {new {ASubObjectProperty = "Hello"}}
                                           });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/HasObject/Value/ASubObjectProperty").Value);
        }

        [Fact]
        public void does_not_throw_as_sub_object_of_anonymous()
        {
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new
                                           {
                                               AProperty = new {ASubObjectProperty = "Hello"}
                                           });
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/Object/AProperty/ASubObjectProperty").Value);
        }
    }
}