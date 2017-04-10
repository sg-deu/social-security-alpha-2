using System.Collections.Generic;
using System.Linq;
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
    public class BsgDeclarationTests : BsgSectionTest
    {
        [Test]
        public void Declaration_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Declaration(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.Declaration });
                response.Doc.Form<Declaration>(1).GetConfirm(m => m.AgreedToLegalStatement).Should().Be(detail.Declaration.AgreedToLegalStatement);
            });
        }

        [Test]
        public void Declaration_POST_CompletesForm()
        {
            WebAppTest(client =>
            {
                // the declaration form is now dependent on a completed ApplicantDetails, so...
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);
                ExecutorStub.SetupCommand(It.IsAny<AddDeclaration>(), new NextSection { Type = NextType.Complete });

                var response = client.Get(BsgActions.Declaration(detail.Id)).Form<Declaration>(1)
                    .SelectConfirm(m => m.AgreedToLegalStatement, true)
                    .Submit(client);

                ExecutorStub.Executed<AddDeclaration>(0).ShouldBeEquivalentTo(new AddDeclaration
                {
                    FormId = "form123",
                    Declaration = new Declaration
                    {
                        AgreedToLegalStatement = true,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Complete());
            });
        }

        [Test]
        public void Declaration_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                // the declaration form is now dependent on a completed ApplicantDetails, so...
                // even if the form is not queried such as this test, the browser cannot render the page without ApplicantDetails
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);
                ExecutorStub.SetupCommand<AddDeclaration, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.Declaration(detail.Id)).Form<Declaration>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }


        [Test]
        public void DeclarationU16_GET()
        {
            WebAppTest(client =>
            {
                // the declaration form is now dependent on a completed ApplicantDetails, so...
                var detail = NewBsgDetail("form123"); // then ensure under 16...
                detail.ApplicantDetails.DateOfBirth = System.DateTime.Now.AddYears(-15);
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Declaration(detail.Id));
                response.Text.ToLower().Should().Contain("you are under the age of 16");
            });
        }

        [Test]
        public void Declaration_GET()
        {
            WebAppTest(client =>
            {
                // the declaration form is now dependent on a completed ApplicantDetails, so...
                var detail = NewBsgDetail("form123"); // with default age ...
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Declaration(detail.Id));
                response.Text.ToLower().Should().Contain("the information you've given");
            });
        }

        [Test]
        [Explicit("RGB - WIP")]
        public void Declaration_GET_DisplaysAllSectionContent()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                detail.PartnerDetails.InheritAddress = true;
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var expectedAnswers = new List<string>();
                MemberVisitor.Visit(detail, (name, member, type, value) => expectedAnswers.Add(name));

                var namesToIgnore = new List<string>
                {
                    "Id",
                    "Consent.",
                    "Declaration.",
                    "PreviousSection",
                    "IsFinalSection",
                };

                // the partner details inherit the address, and the guardian and guardian's partner have full addresses
                namesToIgnore.Add("PartnerDetails.Address.");
                namesToIgnore.Add("GuardianDetails.InheritAddress");
                namesToIgnore.Add("GuardianPartnerDetails.InheritAddress");

                expectedAnswers = expectedAnswers
                    .Where(ea => !namesToIgnore.Any(i => ea.StartsWith(i)))
                    .ToList();

                var response = client.Get(BsgActions.Declaration(detail.Id));
                var missingAnswers = new List<string>();

                foreach (var expectedAnswer in expectedAnswers)
                {
                    var selector = $"[data-answer-for='{expectedAnswer}']";
                    var output = response.Doc.FindAll(selector);

                    output.Count.Should().BeLessOrEqualTo(1, "did not expected multiple entries for {0}", expectedAnswer);

                    if (output.Count < 1)
                        missingAnswers.Add(expectedAnswer);
                }

                Assert.Fail("Could not find answers to the following questions:\n{0}", string.Join("\n", missingAnswers));
            });
        }

        [Test]
        public void Declaration_GET_HandlesMinimalSections()
        {
            WebAppTest(client =>
            {
                var detail = NewMinimalBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Declaration(detail.Id));

                response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Declaration_GET_HandlesMissingSections()
        {
            WebAppTest(client =>
            {
                var detail = new BsgDetail { Id = "form123" };
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Declaration(detail.Id));

                response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            });
        }
    }
}
