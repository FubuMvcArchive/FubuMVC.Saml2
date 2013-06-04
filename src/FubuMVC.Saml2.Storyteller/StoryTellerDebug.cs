using NUnit.Framework;
using StoryTeller.Execution;

namespace StoryTellerTestHarness
{
    [TestFixture, Explicit]
    public class Template
    {
        private ProjectTestRunner runner;

        [TestFixtureSetUp]
        public void SetupRunner()
        {
            runner = new ProjectTestRunner(@"C:\code\FubuMVC.Saml2\src\FubuMVC.Saml2.Storyteller\storyteller.xml");
        }

        [Test]
        public void Happy_path_authentication()
        {
            runner.RunAndAssertTest("SamlResponse/Happy path authentication");
        }

        [TestFixtureTearDown]
        public void TeardownRunner()
        {
            runner.Dispose();
        }
    }
}