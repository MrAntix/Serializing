﻿using System;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Builders;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class deserialize
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void simple()
        {
            const string xml = "<Simple><Name>Name</Name></Simple>";
            Console.Write(xml);

            var sut = GetBuilder().Build();

            var actual = sut.Deserialize<Simple>(xml);

            Assert.NotNull(actual);
            Assert.Equal("Name", actual.Name);
        }

        [Fact]
        public void nested()
        {
            const string xml = "<SimpleNested><Name>Name</Name><Simple><Name>Nested Name</Name></Simple></SimpleNested>";
            Console.Write(xml);

            var sut = GetBuilder().Build();

            var actual = sut.Deserialize<SimpleNested>(xml);

            Assert.NotNull(actual);
            Assert.Equal("Name", actual.Name);

            Assert.NotNull(actual.Simple);
            Assert.Equal("Nested Name", actual.Simple.Name);
        }
    }
}