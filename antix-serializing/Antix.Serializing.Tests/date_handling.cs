using System;
using System.Globalization;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class date_handling
    {
        [Fact]
        public void standard()
        {
            var sut = POXSerializer
                .Create();

            var result = sut.Serialize(new HasDate());

            Assert.Equal("<HasDate><Date>0001-01-01T00:00:00</Date></HasDate>", result);
        }

        [Fact]
        public void overriding()
        {
            var sut = POXSerializer
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
            var sut = POXSerializer
                .Format<DateTimeOffset>(CultureInfo.GetCultureInfo("en-GB"))
                .Create();

            var result = sut.Serialize(new HasDate
                                           {
                                               Date = new DateTime(2000, 12, 13)
                                           });

            Assert.Equal("<HasDate><Date>13/12/2000 00:00:00 +00:00</Date></HasDate>", result);
        }
    }
}