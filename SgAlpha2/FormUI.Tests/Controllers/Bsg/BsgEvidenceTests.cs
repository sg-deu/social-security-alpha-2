using System.Net;
using System.Text;
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
    public class BsgEvidenceTests : BsgSectionTest
    {
        [Test]
        public void Evidence_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Evidence(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.Evidence });
                response.Doc.Form<Evidence>(1).GetConfirm(m => m.SendingByPost).Should().Be(detail.Evidence.SendingByPost);
                response.Doc.Document.Body.TextContent.Should().Contain("No files uploaded");
            });
        }

        [Test]
        public void Evidence_GET_ListsExistingFiles()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                detail.Evidence.Files.Add(new EvidenceFile { Name = "file 1" });
                detail.Evidence.Files.Add(new EvidenceFile { Name = "file 2" });
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Evidence(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.Evidence });
                response.Doc.Form<Evidence>(1).GetConfirm(m => m.SendingByPost).Should().Be(detail.Evidence.SendingByPost);
                response.Doc.Document.Body.TextContent.Should().NotContain("No files uploaded");
                response.Doc.FindAll("#uploadedFiles li").Count.Should().Be(2);
            });
        }

        [Test]
        public void Evidence_POST_UploadFile()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Evidence("form123"));

                response = response.Form<Evidence>(1)
                    .AddFile("UploadedFile.pdf", Encoding.ASCII.GetBytes("uploaded content"))
                    .SubmitName(BsgButtons.UploadFile, client);

                ExecutorStub.Executed<AddEvidenceFile>(0).ShouldBeEquivalentTo(new AddEvidenceFile
                {
                    FormId = "form123",
                    Filename = "UploadedFile.pdf",
                    Content = Encoding.ASCII.GetBytes("uploaded content"),
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Evidence("form123"));
            });
        }

        [Test]
        public void Evidence_POST_RemoveFile()
        {
            WebAppTest(client =>
            {
                // prep this test by adding a file to remove    
                var cloudName = System.Guid.NewGuid().ToString();
                var detail = NewBsgDetail("form123");

                detail.Evidence.Files.Add(new EvidenceFile { Name = "UploadedFile.pdf", CloudName = cloudName });
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                // now remove it
                var response = client.Get(BsgActions.Evidence(detail.Id))
                    .Form<EvidenceFile>(1)
                    .SubmitName(BsgButtons.RemoveFile, client);

                ExecutorStub.Executed<RemoveEvidenceFile>(0).ShouldBeEquivalentTo(new RemoveEvidenceFile
                {
                    FormId = detail.Id,
                    CloudName = cloudName,
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Evidence(detail.Id));
            });
        }

        [Test]
        public void Evidence_POST_PopulatesEvidence()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Evidence("form123")).Form<Evidence>(1)
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

                var response = client.Get(BsgActions.Evidence("form123")).Form<Evidence>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
