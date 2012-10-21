using System;
using System.Globalization;
using Antix.Serializing.Tests.Models;
using Antix.Serializing.Tests.XUnitExtensions;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class serialize_dates
    {
        [Fact]
        public void standard()
        {
            var sut = new SerializerBuilder()
                .Create();

            var result = sut.Serialize(new HasDate());

            Assert.Equal("<HasDate><Date>0001-01-01T00:00:00</Date></HasDate>", result);
        }

        [Fact]
        public void overriding()
        {
            var sut = new SerializerBuilder()
                .Format<DateTimeOffset>("d MMM yyyy")
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               Date = new DateTime(2000, 12, 13)
                                           });

            Assert.Equal("<HasDate><Date>13 Dec 2000</Date></HasDate>", result);
        }

        [Fact]
        public void overriding_with_formatter()
        {
            var sut = new SerializerBuilder()
                .Format<DateTimeOffset>(CultureInfo.GetCultureInfo("en-GB"))
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               Date = new DateTime(2000, 12, 13)
                                           });

            Assert.Equal("<HasDate><Date>13/12/2000 00:00:00 +00:00</Date></HasDate>", result);
        }

        [Fact]
        public void overriding_with_check_and_formatter()
        {
            var sut = new SerializerBuilder()
                .Format((v, t, n) => true, v => "Hello")
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               Date = new DateTime(2000, 12, 13)
                                           });

            Assert.Equal("<HasDate>Hello</HasDate>", result);
        }

        [Fact]
        public void use_default_invariant_culture()
        {
            var sut = new SerializerBuilder()
                .Settings(new SerializerSettings
                              {
                                  DateTimeFormatString = null
                              })
                .Create();

            var result = sut.Serialize(new HasDate());

            Assert.Equal("<HasDate><Date>01/01/0001 00:00:00 +00:00</Date></HasDate>", result);
        }

        [Fact]
        public void use_set_culture()
        {
            var sut = new SerializerBuilder()
                .Settings(new SerializerSettings
                              {
                                  FormatProvider = CultureInfo.GetCultureInfo("fr-FR"),
                                  DateTimeFormatString = "D"
                              })
                .Create();

            var result = sut.Serialize(new HasDate());

            Assert.Equal("<HasDate><Date>lundi 1 janvier 0001</Date></HasDate>", result);
        }

        [Fact]
        [UseCulture("fr-FR")]
        public void change_thread_culture()
        {
            var sut = new SerializerBuilder()
                .Create();

            var result = sut.Serialize(new HasDate());

            Assert.Equal("<HasDate><Date>0001-01-01T00:00:00</Date></HasDate>", result);
        }
    }
}