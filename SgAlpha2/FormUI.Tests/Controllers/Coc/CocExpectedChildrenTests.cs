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
    public class CocExpectedChildrenTests : CocSectionTest
    {
        [Test]
        public void ExpectedChildren_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.ExpectedChildren(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.ExpectedChildren });
                response.Doc.Form<ExpectedChildren>(1).GetText(m => m.ExpectedBabyCount).Should().Be(detail.ExpectedChildren.ExpectedBabyCount.ToString());
            });
        }

        [Test]
        public void ExpectedChildren_GET_DefaultsToExpectedBaby_AndHidesQuestion()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                detail.ExpectedChildren = null;
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.ExpectedChildren(detail.Id));

                response.Doc.Form<ExpectedChildren>(1).GetText(m => m.IsBabyExpected).Should().Be(true.ToString());
                response.Doc.Find("#IsBabyExpected_FormGroup").ShouldBeHidden();
            });
        }

        [Test]
        public void ExpectedChildren_POST_PopulatesExpectedChildren()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.ExpectedChildren("form123")).Form<ExpectedChildren>(1)
                    .SetText(m => m.ExpectedBabyCount, "2")
                    .Submit(client);

                ExecutorStub.Executed<AddExpectedChildren>(0).ShouldBeEquivalentTo(new AddExpectedChildren
                {
                    FormId = "form123",
                    ExpectedChildren = new ExpectedChildren { IsBabyExpected = true, ExpectedBabyCount = 2 },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void ExpectedChildren_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddExpectedChildren, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.ExpectedChildren("form123")).Form<ExpectedChildren>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
