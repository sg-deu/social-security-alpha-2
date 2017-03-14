using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgConsentTests : BsgSectionTest
    {
        [Test]
        public void Consent_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Consent(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.Consent });
                response.Doc.Form<Consent>(1).GetConfirm(m => m.AgreedToConsent).Should().Be(detail.Consent.AgreedToConsent);
            });
        }

        [Test]
        public void Consent_POST_PopulatesConsent()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Consent("form123")).Form<Consent>(1)
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

                var response = client.Get(BsgActions.Consent("form123")).Form<Consent>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
