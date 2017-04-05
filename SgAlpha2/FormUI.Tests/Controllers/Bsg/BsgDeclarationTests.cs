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
                ExecutorStub.SetupCommand(It.IsAny<AddDeclaration>(), new NextSection { Type = NextType.Complete });

                var response = client.Get(BsgActions.Declaration("form123")).Form<Declaration>(1)
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
                ExecutorStub.SetupCommand<AddDeclaration, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.Declaration("form123")).Form<Declaration>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        [Explicit("RGB - WIP")]
        public void Declaration_GET_DisplaysAllSectionContent()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
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

                expectedAnswers = expectedAnswers
                    .Where(ea => !namesToIgnore.Any(i => ea.StartsWith(i)))
                    .ToList();

                var response = client.Get(BsgActions.Declaration(detail.Id));

                foreach (var expectedAnswer in expectedAnswers)
                {
                    var selector = $"[data-answer-for='{expectedAnswer}']";
                    var output = response.Doc.FindAll(selector);
                    output.Count.Should().Be(1, $"should be able to find answer {selector} in output");
                }
            });
        }

    }
}
