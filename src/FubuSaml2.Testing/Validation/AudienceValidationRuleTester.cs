using System;
using FubuSaml2.Validation;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;
using FubuSaml2.Xml;

namespace FubuSaml2.Testing.Validation
{
    [TestFixture]
    public class AudienceValidationRuleTester
    {
        private SamlResponse response;
        private AudienceValidationRule theRule;

        [SetUp]
        public void SetUp()
        {
            response = new SamlResponse();

            theRule = new AudienceValidationRule("foo:bar", "bar:foo");
        }

        [Test]
        public void no_conditions_so_it_passes()
        {
            theRule.Validate(response);
            response.Errors.Any().ShouldBeFalse();
        }

        [Test]
        public void one_matching_audience_so_no_errors()
        {
            response.AddAudienceRestriction(theRule.Audiences.ElementAt(0));
            response.AddAudienceRestriction("something:random");

            theRule.Validate(response);
            response.Errors.Any().ShouldBeFalse();
        }

        [Test]
        public void has_audiences_that_do_not_match()
        {
            response.AddAudienceRestriction("something:random");

            theRule.Validate(response);
            response.Errors.Single().ShouldEqual(new SamlError(SamlValidationKeys.AudiencesDoNotMatch));
        }
    }
}