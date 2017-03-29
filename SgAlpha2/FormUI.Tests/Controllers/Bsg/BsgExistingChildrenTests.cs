using System;
using System.Collections.Generic;
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
    public class BsgExistingChildrenTests : BsgSectionTest
    {
        [Test]
        public void ExistingChildren_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ExistingChildren(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.ExistingChildren });
                response.Doc.Form<ExistingChildren>(1).GetText(m => m.Children[1].Surname).Should().Be(detail.ExistingChildren.Children[1].Surname);
            });
        }

        [Test]
        public void ExistingChildren_AsksForReason_OnlyWhenNoChildBenefit()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ExistingChildren(detail.Id));
                var form = response.Form<ExistingChildren>(1);

                form.RadioShows(m => m.Children[0].ChildBenefit, false, "child0-reason");
                form.RadioShows(m => m.Children[1].ChildBenefit, false, "child1-reason");
            });
        }

        [Test]
        public void ExistingChildren_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExistingChildren("form123")).Form<ExistingChildren>(1)
                    .SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK)).Form<ExistingChildren>(1) // add a child
                    .SetText(m => m.Children[0].FirstName, "child 0 first name")
                    .SetText(m => m.Children[0].Surname, "child 0 surname")
                    .SetDate(m => m.Children[0].DateOfBirth, "03", "04", "2005")
                    .SetText(m => m.Children[0].Relationship, Relationship.KinshipCarer.ToString())
                    .SelectYes(m => m.Children[0].ChildBenefit)
                    .SelectYes(m => m.Children[0].FormalKinshipCare)
                    .SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK)).Form<ExistingChildren>(1) // add a second child
                    .SetText(m => m.Children[1].FirstName, "child 1 first name")
                    .SetText(m => m.Children[1].Surname, "child 1 surname")
                    .SetDate(m => m.Children[1].DateOfBirth, "02", "03", "2004")
                    .SetText(m => m.Children[1].Relationship, Relationship.Parent.ToString())
                    // leave child benefit as null/empty
                    .SelectNo(m => m.Children[1].FormalKinshipCare)
                    .SubmitName("", client);

                ExecutorStub.Executed<AddExistingChildren>(0).ShouldBeEquivalentTo(new AddExistingChildren
                {
                    FormId = "form123",
                    ExistingChildren = new ExistingChildren
                    {
                        Children = new List<ExistingChild>()
                        {
                            new ExistingChild
                            {
                                FirstName = "child 0 first name",
                                Surname = "child 0 surname",
                                DateOfBirth = new DateTime(2005, 04, 03),
                                Relationship = Relationship.KinshipCarer,
                                ChildBenefit = true,
                                FormalKinshipCare = true,
                            },
                            new ExistingChild
                            {
                                FirstName = "child 1 first name",
                                Surname = "child 1 surname",
                                DateOfBirth = new DateTime(2004, 03, 02),
                                Relationship = Relationship.Parent,
                                ChildBenefit = null,
                                FormalKinshipCare = false,
                            },
                        },
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void ExistingChildren_POST_CanAddRemoveChildren()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExistingChildren("form123"));

                response.Doc.FindAll(".existing-child").Count.Should().Be(0);

                response = response
                    .Form<ExistingChildren>(1).SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK))
                    .Form<ExistingChildren>(1).SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK))
                    .Form<ExistingChildren>(1).SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.FindAll(".existing-child").Count.Should().Be(3);

                response = response.Form<ExistingChildren>(1)
                    .SetText(m => m.Children[0].FirstName, "child 0")
                    .SetText(m => m.Children[1].FirstName, "child 1")
                    .SetText(m => m.Children[2].FirstName, "child 2")
                    .SubmitNameValue(BsgButtons.RemoveChild, "1", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.FindAll(".existing-child").Count.Should().Be(2);

                var form = response.Form<ExistingChildren>(1);
                form.GetText(m => m.Children[0].FirstName).Should().Be("child 0");
                form.GetText(m => m.Children[1].FirstName).Should().Be("child 2");
            });
        }

        [Test]
        public void ExistingChildren_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddExistingChildren, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.ExistingChildren("form123")).Form<ExistingChildren>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
