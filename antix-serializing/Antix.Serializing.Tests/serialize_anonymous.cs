using System;
using System.Xml.Linq;
using System.Xml.XPath;
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
                .Create();

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
                .Create();

            var result = sut.Serialize(new
                                           {
                                               AProperty = "Hello"
                                           }, "Object");
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/Object/AProperty").Value);
        }

        [Fact]
        public void does_not_throw_as_sub_object()
        {
            var sut = GetBuilder()
                .Create();

            var result = sut.Serialize(new
                                           {
                                               AProperty = new {ASubObjectProperty = "Hello"}
                                           }, "Object");
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/Object/AProperty/ASubObjectProperty").Value);
        }
    }
}