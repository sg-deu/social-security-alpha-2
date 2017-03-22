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
    public class CocOptionsTests : CocSectionTest
    {
        [Test]
        public void Options_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.Options(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.Options });
                response.Doc.Form<Options>(1).GetConfirm(m => m.ChangePersonalDetails).Should().Be(detail.Options.ChangePersonalDetails);
            });
        }

        [Test]
        public void Options_POST_PopulatesOptions()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.Options("form123")).Form<Options>(1)
                    .SelectConfirm(m => m.ChangePersonalDetails, true)
                    .Submit(client);

                ExecutorStub.Executed<AddOptions>(0).ShouldBeEquivalentTo(new AddOptions
                {
                    FormId = "form123",
                    Options = new Options { ChangePersonalDetails = true },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void Options_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddOptions, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.Options("form123")).Form<Options>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
