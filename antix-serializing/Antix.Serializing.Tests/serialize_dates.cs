﻿using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Antix.Serializing.Tests.Models;
using Antix.Serializing.Tests.XUnitExtensions;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_dates
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

            var result = sut.Serialize(new HasDate());

            var xml = XDocument.Parse(result);
            Assert.Equal("0001-01-01T00:00:00", xml.XPathSelectElement("/HasDate/Value").Value);
        }

        [Fact]
        public void nullable_null()
        {
            var sut = GetBuilder()
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               NullableValue = null
                                           });

            Console.Write(result);

            var xml = XDocument.Parse(result);
            Assert.False(xml.XPathSelectElements("/HasDate/NullableValue").Any());
        }

        [Fact]
        public void overriding()
        {
            var sut = GetBuilder()
                .Format<DateTimeOffset>("d MMM yyyy")
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               Value = new DateTime(2000, 12, 13)
                                           });

            var xml = XDocument.Parse(result);
            Assert.Equal("13 Dec 2000", xml.XPathSelectElement("/HasDate/Value").Value);
        }

        [Fact]
        public void overriding_with_formatter()
        {
            var sut = GetBuilder()
                .Format<DateTimeOffset>(CultureInfo.GetCultureInfo("en-GB"))
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               Value = new DateTime(2000, 12, 13)
                                           });

            var xml = XDocument.Parse(result);
            Assert.Equal("13/12/2000 00:00:00 +00:00", xml.XPathSelectElement("/HasDate/Value").Value);
        }

        [Fact]
        public void overriding_with_check_and_formatter()
        {
            var sut = GetBuilder()
                .Format((v, t, n) => true, v => "Hello")
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               Value = new DateTime(2000, 12, 13)
                                           });

            var xml = XDocument.Parse(result);
            Assert.Equal("Hello", xml.XPathSelectElement("/HasDate").Value);
        }

        [Fact]
        public void use_default_invariant_culture()
        {
            var sut = GetBuilder()
                .DateTimeFormatString(null)
                .Create();

            var result = sut.Serialize(new HasDate());

            var xml = XDocument.Parse(result);
            Assert.Equal("01/01/0001 00:00:00 +00:00", xml.XPathSelectElement("/HasDate").Value);
        }

        [Fact]
        public void use_set_culture()
        {
            var sut = GetBuilder()
                .UseCulture("fr-FR")
                .DateTimeFormatString("D")
                .Create();

            var result = sut.Serialize(new HasDate());

            var xml = XDocument.Parse(result);
            Assert.Equal("lundi 1 janvier 0001", xml.XPathSelectElement("/HasDate/Value").Value);
        }

        [Fact]
        public void use_set_culture_then_ignore()
        {
            var sut = GetBuilder()
                .UseCulture("fr-FR")
                .IgnoreCulture()
                .Create();

            var result = sut.Serialize(new HasDate());

            var xml = XDocument.Parse(result);
            Assert.Equal("0001-01-01T00:00:00", xml.XPathSelectElement("/HasDate/Value").Value);
        }

        [Fact]
        [UseCulture("fr-FR")]
        public void change_thread_culture()
        {
            var sut = GetBuilder()
                .Create();

            var result = sut.Serialize(new HasDate());

            var xml = XDocument.Parse(result);
            Assert.Equal("0001-01-01T00:00:00", xml.XPathSelectElement("/HasDate/Value").Value);
        }
    }
}