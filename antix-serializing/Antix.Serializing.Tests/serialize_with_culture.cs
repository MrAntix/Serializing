using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Tests.Models;
using Antix.Serializing.Tests.XUnitExtensions;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_with_culture
    {
        [Fact]
        [UseCulture("fr-FR")]
        public void by_default_current_thread_culture_has_no_effect()
        {
            var sut = new SerializerBuilder()
                .Create();

            var result = sut.Serialize(new HasNumber
                                           {
                                               Value = 10.5M
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("10.5", xml.XPathSelectElement("/HasNumber/Value").Value);
        }

        [Fact]
        [UseCulture("fr-FR")]
        public void use_thread_culture()
        {
            var sut = new SerializerBuilder()
                .UseThreadCulture()
                .Create();

            var result = sut.Serialize(new HasNumber
                                           {
                                               Value = 10.5M
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("10,5", xml.XPathSelectElement("/HasNumber/Value").Value);
        }

        [Fact]
        public void use_specified_culture()
        {
            var sut = new SerializerBuilder()
                .UseCulture("fr-FR")
                .Create();

            var result = sut.Serialize(new HasNumber
                                           {
                                               Value = 10.5M
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.Equal("10,5", xml.XPathSelectElement("/HasNumber/Value").Value);
        }
    }
}