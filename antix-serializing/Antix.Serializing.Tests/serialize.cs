using System;
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

            var result = sut.Serialize(new SimpleNestedEnumerable
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
    }
}