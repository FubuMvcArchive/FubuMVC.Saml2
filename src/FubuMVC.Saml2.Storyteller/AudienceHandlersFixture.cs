using System;
using StoryTeller.Engine;
using StructureMap;
using System.Linq;

namespace FubuMVC.Saml2.Storyteller
{
    public class AudienceHandlersFixture : Fixture
    {
        public AudienceHandlersFixture()
        {
            Title = "Saml Audiences";
        }

        public override void SetUp(ITestContext context)
        {
            FakeSamlResponseHandler.ClearAudiences();
        }

        [FormatAs("We recognize to audience {audience}")]
        public void Audience(string audience)
        {
            FakeSamlResponseHandler.Add(audience);
        }
    }
}