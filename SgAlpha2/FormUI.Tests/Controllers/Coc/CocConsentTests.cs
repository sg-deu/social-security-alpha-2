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
    public class CocConsentTests : CocSectionTest
    {
        [Test]
        public void Consent_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.Consent(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.Consent });
                response.Doc.Form<Consent>(1).GetConfirm(m => m.AgreedToConsent).Should().Be(detail.Consent.AgreedToConsent);
            });
        }

        [Test]
        public void Consent_POST_PopulatesConsent()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.Consent("form123")).Form<Consent>(1)
                    .SelectConfirm(m => m.AgreedToConsent, true)
                    .Submit(client);

                ExecutorStub.Executed<AddConsent>(0).ShouldBeEquivalentTo(new AddConsent
                {
                    FormId = "form123",
                    Consent = new Consent { AgreedToConsent = true },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void Consent_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddConsent, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.Consent("form123")).Form<Consent>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
