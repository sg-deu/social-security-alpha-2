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
    public class CocApplicantDetailsTests : CocSectionTest
    {
        [Test]
        public void ApplicantDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.ApplicantDetails(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.ApplicantDetails });
                response.Doc.Form<ApplicantDetails>(1).GetText(m => m.FullName).Should().Be(detail.ApplicantDetails.FullName);
            });
        }

        [Test]
        public void ApplicantDetails_POST_PopulatesApplicantDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1)
                    .SetText(m => m.FullName, "test full name")
                    .Submit(client);

                ExecutorStub.Executed<AddApplicantDetails>(0).ShouldBeEquivalentTo(new AddApplicantDetails
                {
                    FormId = "form123",
                    ApplicantDetails = new ApplicantDetails { FullName = "test full name" },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void ApplicantDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddApplicantDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
