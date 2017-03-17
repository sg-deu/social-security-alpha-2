using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Home;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Tests.Controllers.Util;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Coc
{
    [TestFixture]
    public class CocTests : WebTest
    {
        [Test]
        public void Overview_POST_StartsForm()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<StartChangeOfCircs>(), new NextSection
                {
                    Id = "form123",
                    Section = Sections.Consent,
                });

                var response = client.Get(HomeActions.Index()).Form<object>(1)
                    .Submit(client);

                ExecutorStub.Executed<StartChangeOfCircs>().Length.Should().Be(1);

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }
    }
}
