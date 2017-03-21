using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Coc;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Queries;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Coc
{
    [TestFixture]
    public class CocIdentityTests : CocSectionTest
    {
        [Test]
        public void Identity_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.Identity(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.Identity });
                response.Doc.Form<IdentityModel>(1).GetText(m => m.Email).Should().Be(detail.Identity);
            });
        }

        [Test]
        public void Identity_POST_PopulatesIdentity()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.Identity("form123")).Form<IdentityModel>(1)
                    .SetText(m => m.Email, "test.mail@test.com")
                    .Submit(client);

                ExecutorStub.Executed<AddIdentity>(0).ShouldBeEquivalentTo(new AddIdentity
                {
                    FormId = "form123",
                    Identity = "test.mail@test.com",
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void Identity_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddIdentity, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.Identity("form123")).Form<IdentityModel>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
