using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Tests.Models;
using Antix.Serializing.Tests.XUnitExtensions;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_numbers
    {
        [Fact]
        public void standard()
        {
            var sut = new SerializerBuilder()
                .Create();

            var result = sut.Serialize(new HasNumber
                                           {
                                               Decimal = 10.5M
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            var value = xml.XPathSelectElement("/HasNumber/Decimal").Value;

            Assert.Equal("10.5", value);
        }

        [Fact]
        [UseCulture("fr-FR")]
        public void standard_thread_culture()
        {
            var sut = new SerializerBuilder()
                .Create();

            var result = sut.Serialize(new HasNumber
                                           {
                                               Decimal = 10.5M
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            var value = xml.XPathSelectElement("/HasNumber/Decimal").Value;

            Assert.Equal("10.5", value);
        }
    }
}