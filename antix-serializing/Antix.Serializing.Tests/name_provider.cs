using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antix.Serializing.Providers;
using Antix.Serializing.Tests.Models;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class name_provider
    {
        [Fact]
        public void gets_type_name()
        {
            var names = new Dictionary<MemberInfo, string>();

            var sut = new NameProvider(names);
            var type = typeof (Simple);

            Assert.Equal(type.Name, sut.Get(type));
        }

        [Fact]
        public void gets_property_name()
        {
            var names = new Dictionary<MemberInfo, string>();

            var sut = new NameProvider(names);
            var propertyInfo = typeof (Simple).GetProperties().First();

            Assert.Equal(propertyInfo.Name, sut.Get(propertyInfo));
        }

        [Fact]
        public void overrides_type_name()
        {
            const string expectedName = "Egg";
            var names = new Dictionary<MemberInfo, string>
                            {
                                {typeof (Simple), expectedName}
                            };

            var sut = new NameProvider(names);

            Assert.Equal(expectedName, sut.Get(typeof (Simple)));
        }

        [Fact]
        public void overrides_property_name()
        {
            const string expectedName = "Egg";
            var names = new Dictionary<MemberInfo, string>
                            {
                                {typeof (Simple).GetProperties().First(), expectedName}
                            };

            var sut = new NameProvider(names);

            Assert.Equal(expectedName, sut.Get(typeof (Simple).GetProperties().First()));
        }
    }
}