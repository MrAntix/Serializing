using System;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Builders;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class deserialize_dates
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void standard()
        {
            const string xml = "<HasDate><Value>2001-12-21T11:22:33</Value></HasDate>";
            Console.Write(xml);

            var sut = GetBuilder().Build();

            var actual = sut.Deserialize<HasDate>(xml);

            Assert.NotNull(actual);
            Assert.Equal(
                new DateTimeOffset(2001, 12, 21, 11, 22, 33, TimeSpan.FromSeconds(0)),
                actual.Value);
        }

        [Fact]
        public void cannot_parse()
        {
            const string xml = "<HasDate><Value>XXX</Value></HasDate>";
            Console.Write(xml);

            var sut = GetBuilder().Build();

            var ex = Assert.Throws<DeserializationException>(
                () => sut.Deserialize<HasDate>(xml)
                );

            Assert.Equal(typeof(DateTimeOffset), ex.Type);
            Assert.Equal("XXX", ex.Value);
        }

        [Fact]
        public void cannot_parse_nullable()
        {
            const string xml = "<HasDate><NullableValue>XXX</NullableValue></HasDate>";
            Console.Write(xml);

            var sut = GetBuilder().Build();

            var ex = Assert.Throws<DeserializationException>(
                () => sut.Deserialize<HasDate>(xml)
                );

            Assert.Equal(typeof(DateTimeOffset), ex.Type);
            Assert.Equal("XXX", ex.Value);
        }
    }
}