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
    public class BsgHealthProfessionalTests : BsgSectionTest
    {
        [Test]
        public void HealthProfessional_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.HealthProfessional(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.HealthProfessional });
                response.Doc.Form<HealthProfessional>(1).GetText(m => m.Pin).Should().Be(detail.HealthProfessional.Pin);
            });
        }

        [Test]
        public void HealthProfessional_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.HealthProfessional("form123")).Form<HealthProfessional>(1)
                    .SetText(m => m.Pin, "ABC12345")
                    .Submit(client);

                ExecutorStub.Executed<AddHealthProfessional>(0).ShouldBeEquivalentTo(new AddHealthProfessional
                {
                    FormId = "form123",
                    HealthProfessional = new HealthProfessional
                    {
                        Pin = "ABC12345",
                    },
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

                var response = client.Get(BsgActions.HealthProfessional("form123")).Form<HealthProfessional>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
