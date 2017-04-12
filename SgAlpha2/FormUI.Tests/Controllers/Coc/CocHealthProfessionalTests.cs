using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Coc;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Queries;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Coc
{
    [TestFixture]
    public class CocHealthProfessionalTests : CocSectionTest
    {
        [Test]
        public void HealthProfessional_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.HealthProfessional(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.HealthProfessional });
                response.Doc.Form<HealthProfessional>(1).GetText(m => m.Pin).Should().Be(detail.HealthProfessional.Pin);
            });
        }

        [Test]
        public void HealthProfessional_POST_PopulatesHealthProfessional()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.HealthProfessional("form123")).Form<HealthProfessional>(1)
                    .SetText(m => m.Pin, "test pin")
                    .Submit(client);

                ExecutorStub.Executed<AddHealthProfessional>(0).ShouldBeEquivalentTo(new AddHealthProfessional
                {
                    FormId = "form123",
                    HealthProfessional = new HealthProfessional { Pin = "test pin" },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void HealthProfessional_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddHealthProfessional, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.HealthProfessional("form123")).Form<HealthProfessional>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
