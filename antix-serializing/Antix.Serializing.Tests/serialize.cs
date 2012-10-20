using System;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize
    {
        [Fact]
        public void simple()
        {
            var sut = new SerializerBuilder().Create();

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
            var sut = new SerializerBuilder().Create();

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
            var sut = new SerializerBuilder().Create();

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