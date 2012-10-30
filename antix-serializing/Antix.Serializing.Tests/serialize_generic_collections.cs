using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Builders;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_generic_collections
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void deserialize_dictionary()
        {
            var sut = GetBuilder().Build();

            var result = sut.Serialize(
                new HasGenericCollections
                    {
                        ValueDictionary = new Dictionary<string, Simple>
                                              {
                                                  {
                                                      "One", new Simple
                                                                 {
                                                                     Name = "Name"
                                                                 }
                                                  }
                                              }
                    }
                );

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("One", xml.XPathSelectElement("/HasGenericCollections/ValueDictionary/KeyValuePair/Key").Value);
            Assert.Equal("Name",
                         xml.XPathSelectElement("/HasGenericCollections/ValueDictionary/KeyValuePair/Value").Value);
        }

        [Fact]
        public void deserialize_generic_list()
        {
            var sut = GetBuilder().Build();

            var result = sut.Serialize(
                new HasGenericCollections
                    {
                        ValueList = new List<Simple>
                            (
                            new[]
                                {
                                    new Simple
                                        {
                                            Name = "Name"
                                        }
                                }
                            )
                    }
                );

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("Name", xml.XPathSelectElement("/HasGenericCollections/ValueList/Simple/Name").Value);
        }
    }
}