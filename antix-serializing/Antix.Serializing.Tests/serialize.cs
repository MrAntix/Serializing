using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Builders;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void simple()
        {
            var sut = GetBuilder().Build();

            var result = sut.Serialize(new Simple
                                           {
                                               Name = "Name"
                                           });

            Console.Write(result);

            Assert.Equal("<Simple><Name>Name</Name></Simple>", result);
        }

        [Fact]
        public void nested()
        {
            var sut = GetBuilder().Build();

            var result = sut.Serialize(new SimpleNested
                                           {
                                               Name = "Name",
                                               Simple = new Simple
                                                            {
                                                                Name = "Nested Name"
                                                            }
                                           });

            Console.Write(result);

            Assert.Equal("<SimpleNested><Name>Name</Name><Simple><Name>Nested Name</Name></Simple></SimpleNested>",
                         result);
        }

        [Fact]
        public void nested_enumerables()
        {
            var sut = GetBuilder().Build();

            var result = sut.Serialize(
                new SimpleNestedEnumerable
                    {
                        Name = "Name",
                        Simples = new[]
                                      {
                                          new Simple
                                              {
                                                  Name = "Nested Name"
                                              }
                                      }
                    });

            Console.Write(result);

            Assert.Equal(
                "<SimpleNestedEnumerable><Name>Name</Name><Simples><Simple><Name>Nested Name</Name></Simple></Simples></SimpleNestedEnumerable>",
                result);
        }

        [Fact]
        public void nested_enumerables_can_specify_item_name()
        {
            var sut = GetBuilder()
                .Type<SimpleNestedEnumerable>(
                    t => t.Property(o => o.Simples,
                                    p => p.Name("Items")
                                          .ItemName("Item")))
                .Build();

            var result = sut.Serialize(
                new SimpleNestedEnumerable
                    {
                        Name = "Name",
                        Simples = new[]
                                      {
                                          new Simple
                                              {
                                                  Name = "Nested Name"
                                              }
                                      }
                    });

            Console.Write(result);

            Assert.Equal(
                "<SimpleNestedEnumerable><Name>Name</Name><Items><Item><Name>Nested Name</Name></Item></Items></SimpleNestedEnumerable>",
                result);
        }

        [Fact]
        public void can_supply_root_name()
        {
            const string expectedName = "Bob";
            var sut = GetBuilder()
                .Build();

            var result = sut.Serialize(new Simple
                                           {
                                               Name = expectedName
                                           }, "Object");
            Console.WriteLine(result);

            var xml = XDocument.Parse(result);
            Assert.Equal(expectedName, xml.XPathSelectElement("/Object/Name").Value);
        }
    }
}