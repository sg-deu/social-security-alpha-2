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
    public class CocEvidenceTests : CocSectionTest
    {
        [Test]
        public void Evidence_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.Evidence(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.Evidence });
                response.Doc.Form<Evidence>(1).GetConfirm(m => m.SendingByPost).Should().Be(detail.Evidence.SendingByPost);
                response.Doc.Document.Body.TextContent.Should().Contain("No files uploaded");
            });
        }
        [Test]
        public void Evidence_GET_ListsExistingFiles()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                detail.Evidence.Files.Add(new EvidenceFile { Name = "file 1" });
                detail.Evidence.Files.Add(new EvidenceFile { Name = "file 2" });
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.Evidence(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.Evidence });
                response.Doc.Form<Evidence>(1).GetConfirm(m => m.SendingByPost).Should().Be(detail.Evidence.SendingByPost);
                response.Doc.Document.Body.TextContent.Should().NotContain("No files uploaded");
                response.Doc.FindAll("#uploadedFiles li").Count.Should().Be(2);
            });
        }

        [Test]
        public void Evidence_POST_PopulatesEvidence()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.Evidence("form123")).Form<Evidence>(1)
                    .SelectConfirm(m => m.SendingByPost, true)
                    .SubmitName("", client);

                ExecutorStub.Executed<AddEvidence>(0).ShouldBeEquivalentTo(new AddEvidence
                {
                    FormId = "form123",
                    Evidence = new Evidence { SendingByPost = true },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void Evidence_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddEvidence, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.Evidence("form123")).Form<Evidence>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
