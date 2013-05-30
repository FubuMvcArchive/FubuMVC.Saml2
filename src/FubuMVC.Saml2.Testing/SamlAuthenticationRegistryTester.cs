using FubuCore.Binding;
using FubuMVC.Authentication;
using FubuSaml2;
using FubuSaml2.Encryption;
using FubuSaml2.Validation;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;

namespace FubuMVC.Saml2.Testing
{
    [TestFixture]
    public class when_trying_to_apply_with_no_saml : InteractionContext<SamlAuthenticationStrategy>
    {
        private InMemoryRequestData theRequestData;
        private AuthResult theResult;

        protected override void beforeEach()
        {
            theRequestData = new InMemoryRequestData();
            Services.Inject<IRequestData>(theRequestData);

            Services.PartialMockTheClassUnderTest();
            ClassUnderTest.Expect(x => x.ProcessSamlResponseXml("anything"))
                          .IgnoreArguments().Repeat.Never();



            theResult = ClassUnderTest.TryToApply();
        }

        [Test]
        public void does_not_try_to_process_any_saml()
        {
            ClassUnderTest.VerifyAllExpectations();
        }
        
        [Test]
        public void the_result_is_just_false()
        {
            theResult.Continuation.ShouldBeNull();
            theResult.Success.ShouldBeFalse();
        }
    }

    [TestFixture]
    public class when_try_to_apply_with_saml_response : InteractionContext<SamlAuthenticationStrategy>
    {
        private AuthResult theDirectoryResult;
        private InMemoryRequestData theRequestData;
        private string theResponseXml;
        private AuthResult theResult;

        protected override void beforeEach()
        {
            theDirectoryResult = new AuthResult();
            MockFor<ISamlDirector>().Stub(x => x.Result()).Return(theDirectoryResult);


            theResponseXml = "<Response />";

            theRequestData = new InMemoryRequestData();
            theRequestData["SamlResponse"] = theResponseXml;
            theRequestData.Value("SamlResponse").ShouldNotBeNull();

            Services.Inject<IRequestData>(theRequestData);

            Services.PartialMockTheClassUnderTest();
            ClassUnderTest.Expect(x => x.ProcessSamlResponseXml(theResponseXml));

            theResult = ClassUnderTest.TryToApply();
        }

        [Test]
        public void should_have_processed_the_saml_xml()
        {
            ClassUnderTest.VerifyAllExpectations();
        }

        [Test]
        public void should_return_the_result_from_the_director()
        {
            theResult.ShouldBeTheSameAs(theDirectoryResult);
        }
    }

    [TestFixture]
    public class when_processing_the_saml_xml : InteractionContext<SamlAuthenticationStrategy>
    {
        private ISamlValidationRule[] theRules;
        private SamlResponse theResponse;
        private string theXml;
        private ISamlResponseHandler[] theHandlers;

        protected override void beforeEach()
        {
            theRules = Services.CreateMockArrayFor<ISamlValidationRule>(5);

            theResponse = new SamlResponse();
            theXml = "<Response />";

            MockFor<ISamlResponseReader>().Stub(x => x.Read(theXml)).Return(theResponse);

            theHandlers = Services.CreateMockArrayFor<ISamlResponseHandler>(3);
            theHandlers[2].Stub(x => x.CanHandle(theResponse)).Return(true);

            ClassUnderTest.ProcessSamlResponseXml(theXml);
        }

        [Test]
        public void should_run_all_the_validation_rules()
        {
            theRules.Each(rule => {
                rule.AssertWasCalled(x => x.Validate(theResponse));
            });
        }

        [Test]
        public void uses_the_first_saml_response_handler_that_matches()
        {
            var theDirector = MockFor<ISamlDirector>();

            // The [2] handler matches via its CanHandle method, the others don't
            theHandlers[2].AssertWasCalled(x => x.Handle(theDirector, theResponse));

            theHandlers[0].AssertWasNotCalled(x => x.Handle(theDirector, theResponse));
            theHandlers[1].AssertWasNotCalled(x => x.Handle(theDirector, theResponse));
        }
    }
}