using System;
using System.Linq;
using System.Xml.Linq;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_nulls
    {
        [Fact]
        public void include_nulls()
        {
            var sut = new SerializerBuilder()
                .Create(new SerializerSettings
                            {
                                IncludeNulls = true
                            });

            var result = sut.Serialize(new Simple
                                           {
                                               Name = null
                                           });

            Console.Write(result);

            Assert.Equal("<Simple><Name nill=\"true\"/></Simple>", result);

            var xml = XDocument.Parse(result);
            var att = xml.Root.Elements().Single().Attributes().Single();
            Assert.Equal("nill", att.Name);
            Assert.Equal("true", att.Value);
        }

        [Fact]
        public void do_not_include_nulls()
        {
            var sut = new SerializerBuilder()
                .Create(new SerializerSettings
                            {
                                IncludeNulls = false
                            });

            var result = sut.Serialize(new Simple
                                           {
                                               Name = null
                                           });

            Console.Write(result);

            Assert.Equal("<Simple></Simple>", result);

            Assert.DoesNotThrow(() => XDocument.Parse(result));
        }
    }
}