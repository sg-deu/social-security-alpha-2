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
    public class BsgApplicantDetailsTests : BsgSectionTest
    {
        [Test]
        public void ApplicantDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ApplicantDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.ApplicantDetails });
                response.Doc.Form<ApplicantDetails>(1).GetText(m => m.FirstName).Should().Be(detail.ApplicantDetails.FirstName);
            });
        }

        [Test]
        public void ApplicantDetails_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1)
                    .SetText(m => m.FirstName, "first name")
                    .Submit(client);

                ExecutorStub.Executed<AddApplicantDetails>(0).ShouldBeEquivalentTo(new AddApplicantDetails
                {
                    FormId = "form123",
                    ApplicantDetails = new ApplicantDetails { FirstName = "first name" },
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

                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void ApplicantDetails_AjaxShowsHidesQuestions()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantDetails("form123"));

                ExecutorStub.SetupQuery(It.IsAny<FindApplicantDetailsConfig>(), new ApplicantDetailsConfig
                {
                    ShouldAskCareQuestion = true,
                    ShouldAskEducationQuestion = false,
                });

                var ajaxActions = response.Form<ApplicantDetails>(1)
                    .OnChange(f => f.DateOfBirth, client);

                ajaxActions.Should().NotBeNull();
                ajaxActions.Length.Should().Be(2);

                ajaxActions.ForFormGroup<ApplicantDetails>(f => f.PreviouslyLookedAfter).ShouldShowHide(response.Doc, true);
                ajaxActions.ForFormGroup<ApplicantDetails>(f => f.FullTimeEducation).ShouldShowHide(response.Doc, false);
            });
        }
    }
}
