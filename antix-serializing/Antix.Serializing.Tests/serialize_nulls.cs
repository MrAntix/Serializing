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
    public class serialize_nulls
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void include_nulls()
        {
            var sut = GetBuilder()
                .IncludeNulls()
                .Build();

            var result = sut.Serialize(new Simple
                                           {
                                               Name = null
                                           });

            Console.Write(result);

            Assert.Equal("<Simple><Name nill=\"true\"/></Simple>", result);

            var xml = XDocument.Parse(result);
            var att = xml.XPathSelectElement("/Simple/Name").Attributes().Single();
            Assert.Equal("nill", att.Name);
            Assert.Equal("true", att.Value);
        }

        [Fact]
        public void do_not_include_nulls()
        {
            var sut = GetBuilder()
                .ExcludeNulls()
                .Build();

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