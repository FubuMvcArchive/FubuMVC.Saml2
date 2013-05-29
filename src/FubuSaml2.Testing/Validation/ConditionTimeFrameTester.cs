using FubuCore.Dates;
using FubuSaml2.Validation;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;
using FubuCore;

namespace FubuSaml2.Testing.Validation
{
    [TestFixture]
    public class ConditionTimeFrameTester
    {
        private SystemTime systemTime   ;
        private ConditionTimeFrame theCondition;
        // INVALID IS
        // if (now < notBefore || now >= notOnOrAfter)

        [SetUp]
        public void SetUp()
        {
            systemTime = SystemTime.AtLocalTime("0800".ToTime());
            theCondition = new ConditionTimeFrame(systemTime);
        }

        [Test]
        public void valid()
        {
            var response = new SamlResponse
            {
                Conditions = new ConditionGroup
                {
                    NotBefore = systemTime.UtcNow().AddMinutes(-5),
                    NotOnOrAfter = systemTime.UtcNow().AddMinutes(5)
                }
            };

            theCondition.Validate(response);
            response.Errors.Any().ShouldBeFalse();
        }

        [Test]
        public void invalid_because_it_is_before_the_not_before()
        {
            var response = new SamlResponse
            {
                Conditions = new ConditionGroup
                {
                    NotBefore = systemTime.UtcNow().AddMinutes(1),
                    NotOnOrAfter = systemTime.UtcNow().AddMinutes(5)
                }
            };

            theCondition.Validate(response);
            response.Errors.Single().ShouldEqual(new SamlError(SamlValidationKeys.TimeFrameDoesNotMatch));
        }

        [Test]
        public void invalid_because_it_is_equal_to_the_after_time()
        {
            var response = new SamlResponse
            {
                Conditions = new ConditionGroup
                {
                    NotBefore = systemTime.UtcNow().AddMinutes(-1),
                    NotOnOrAfter = systemTime.UtcNow()
                }
            };

            new ConditionTimeFrame(systemTime).Validate(response);
            response.Errors.Single().ShouldEqual(new SamlError(SamlValidationKeys.TimeFrameDoesNotMatch));
        }

        [Test]
        public void invalid_because_it_is_after_to_the_after_time()
        {
            var response = new SamlResponse
            {
                Conditions = new ConditionGroup
                {
                    NotBefore = systemTime.UtcNow().AddMinutes(-5),
                    NotOnOrAfter = systemTime.UtcNow().AddMinutes(-1)
                }
            };

            new ConditionTimeFrame(systemTime).Validate(response);
            response.Errors.Single().ShouldEqual(new SamlError(SamlValidationKeys.TimeFrameDoesNotMatch));
        }
    }
}