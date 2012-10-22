using System;
using System.Collections.Generic;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_dictionaries
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void standard()
        {
            var sut = GetBuilder().Create();

            var result = sut.Serialize(
                new HasDictionary
                    {
                        Value = new Dictionary<string, Simple>
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
        }


        [Fact]
        public void deserialize_with_data_contract_serializer()
        {
            var sut = GetBuilder().Create();

            var result = sut.Serialize(
                new HasDictionary
                    {
                        Value = new Dictionary<string, Simple>
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
        }
    }
}