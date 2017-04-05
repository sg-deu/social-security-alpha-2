using System;
using System.Collections;
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

                var expectedAnswers = FindAllExpectedAnswers(detail);

                var namesToIgnore = new List<string>
                {
                    "Id",
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

        private IList<string> FindAllExpectedAnswers(object source)
        {
            var expectedAnswers = new List<string>();
            AddNames(expectedAnswers, source, "");
            return expectedAnswers;
        }

        private void AddNames(IList<string> names, object source, string prefix)
        {
            if (source == null)
                return;

            var type = source.GetType();

            if (IsList(type))
            {
                AddList(names, source, prefix);
                return;
            }
        
            var props = type.GetProperties();

            foreach (var prop in props)
                AddName(names, prop.PropertyType, prop.Name, () => prop.GetValue(source, null), prefix);

            var fields = type.GetFields();

            foreach (var field in fields)
                AddName(names, field.FieldType, field.Name, () => field.GetValue(source), prefix);
        }

        private void AddName(IList<string> names, Type type, string name, Func<object> value, string prefix)
        {
            if (IsCompound(type))
                AddNames(names, value(), PrefixFor(prefix, name));
            else
                names.Add(PrefixFor(prefix, name));
        }

        private void AddList(IList<string> names, object source, string prefix)
        {
            var enumerable = (IEnumerable)source;
            var index = 0;

            foreach (var item in enumerable)
                AddNames(names, item, prefix + $"[{index++}]");
        }

        private bool IsList(Type type)
        {
            if (type == typeof(string))
                return false;

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private bool IsCompound(Type type)
        {
            if (type.IsValueType || type == typeof(string))
                return false;

            if (type.FullName.Contains("FormUI.Domain."))
                return true;

            throw new Exception("Unhandled type: " + type);
        }

        private string PrefixFor(string existingPrefix, string name)
        {
            return string.IsNullOrWhiteSpace(existingPrefix)
                ? name
                : existingPrefix + "." + name;
        }
    }
}
