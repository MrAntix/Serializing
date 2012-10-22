using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_timespans
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void standard()
        {
            var sut = GetBuilder()
                .Create();

            var result = sut.Serialize(new HasTimeSpan());

            var xml = XDocument.Parse(result);
            Assert.Equal("00:00:00", xml.XPathSelectElement("/HasTimeSpan/Value").Value);
        }

        [Fact]
        public void nullable_null()
        {
            var sut = GetBuilder()
                .Create();

            var result = sut.Serialize(new HasTimeSpan
                                           {
                                               NullableValue = null
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.False(xml.XPathSelectElements("/HasTimeSpan/NullableValue").Any());
        }

        [Fact]
        public void overriding_with_formatter()
        {
            var sut = GetBuilder()
                .Format<TimeSpan>(CultureInfo.GetCultureInfo("en-GB"))
                .Create();

            var result = sut.Serialize(new HasTimeSpan
                                           {
                                               Value = TimeSpan.FromHours(1)
                                           });

            var xml = XDocument.Parse(result);
            Assert.Equal("01:00:00", xml.XPathSelectElement("/HasTimeSpan/Value").Value);
        }
    }
}