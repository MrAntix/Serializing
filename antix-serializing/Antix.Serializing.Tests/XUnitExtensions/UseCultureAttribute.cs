using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Xunit;

namespace Antix.Serializing.Tests.XUnitExtensions
{
    /// <summary>
    ///   <see cref="https://github.com/AutoFixture/AutoFixture/blob/master/Src/AutoFixtureUnitTest/UseCultureAttribute.cs" />
    /// </summary>
    public class UseCultureAttribute : BeforeAfterTestAttribute
    {
        [ThreadStatic] static CultureInfo _originalCulture;
        [ThreadStatic] static CultureInfo _originalUiCulture;

        readonly CultureInfo _culture;
        readonly CultureInfo _uiCulture;

        public UseCultureAttribute(string culture)
            : this(culture, culture)
        {
        }

        public UseCultureAttribute(string culture, string uiCulture)
        {
            _culture = new CultureInfo(culture);
            _uiCulture = new CultureInfo(uiCulture);
        }


        public override void Before(MethodInfo methodUnderTest)
        {
            _originalCulture = Thread.CurrentThread.CurrentCulture;
            _originalUiCulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = _culture;
            Thread.CurrentThread.CurrentUICulture = _uiCulture;
        }

        public override void After(MethodInfo methodUnderTest)
        {
            Thread.CurrentThread.CurrentCulture = _originalCulture;
            Thread.CurrentThread.CurrentUICulture = _originalUiCulture;
        }
    }
}