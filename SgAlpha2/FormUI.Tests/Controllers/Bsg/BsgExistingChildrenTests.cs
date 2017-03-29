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
        public void ExistingChildren_AsksChildDetails_OnlyWhenSelectedYesToChildren()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123", 2);
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ExistingChildren(detail.Id));
                var form = response.Form<ExistingChildren>(1);

                form.RadioShows(m => m.AnyExistingChildren, true, "children-details");
                response.Doc.FindAll("button[name=RemoveChild]").Count.Should().Be(2, "should have 2 remove buttons");
            });
        }

        [Test]
        public void ExistingChildren_GET_WhenExactlyOneChild_NoRemoveButtonIsShown()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123", 1);
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ExistingChildren(detail.Id));
                var form = response.Form<ExistingChildren>(1);

                form.RadioShows(m => m.AnyExistingChildren, true, "children-details");
                response.Doc.FindAll("button[name=RemoveChild]").Count.Should().Be(0, "no remove child button when there's only 1 child (select 'no' instead)");
            });
        }

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
        public void ExistingChildren_GET_WhenNoExistingChildren_PrePopulatesOneChild()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                detail.ExistingChildren.Children = new List<ExistingChild>();
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ExistingChildren(detail.Id));

                response.Doc.FindAll(".existing-child").Count.Should().Be(1, "should have at least one child pre-populated (albeit hidden)");
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
                    .SelectYes(m => m.AnyExistingChildren)
                    .SetText(m => m.Children[0].FirstName, "child 0 first name")
                    .SetText(m => m.Children[0].Surname, "child 0 surname")
                    .SetDate(m => m.Children[0].DateOfBirth, "03", "04", "2005")
                    .SetText(m => m.Children[0].Relationship, Relationship.KinshipCarer.ToString())
                    .SelectYes(m => m.Children[0].ChildBenefit)
                    .SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK)).Form<ExistingChildren>(1) // add a second child
                    .SetText(m => m.Children[1].FirstName, "child 1 first name")
                    .SetText(m => m.Children[1].Surname, "child 1 surname")
                    .SetDate(m => m.Children[1].DateOfBirth, "02", "03", "2004")
                    .SetText(m => m.Children[1].Relationship, Relationship.Parent.ToString())
                    // leave child benefit as null/empty
                    .SubmitName("", client);

                ExecutorStub.Executed<AddExistingChildren>(0).ShouldBeEquivalentTo(new AddExistingChildren
                {
                    FormId = "form123",
                    ExistingChildren = new ExistingChildren
                    {
                        AnyExistingChildren = true,
                        Children = new List<ExistingChild>()
                        {
                            new ExistingChild
                            {
                                FirstName = "child 0 first name",
                                Surname = "child 0 surname",
                                DateOfBirth = new DateTime(2005, 04, 03),
                                Relationship = Relationship.KinshipCarer,
                                ChildBenefit = true,
                            },
                            new ExistingChild
                            {
                                FirstName = "child 1 first name",
                                Surname = "child 1 surname",
                                DateOfBirth = new DateTime(2004, 03, 02),
                                Relationship = Relationship.Parent,
                                ChildBenefit = null,
                            },
                        },
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void ExistingChildren_POST_AutomaticallyRemovesChildren_WhenIndicatingNoChildrenInHousehold()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExistingChildren("form123")).Form<ExistingChildren>(1)
                    .SelectYes(m => m.AnyExistingChildren)
                    .SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK)).Form<ExistingChildren>(1) // add a child
                    .SelectNo(m => m.AnyExistingChildren)
                    .SubmitName("", client);

                ExecutorStub.Executed<AddExistingChildren>(0).ShouldBeEquivalentTo(new AddExistingChildren
                {
                    FormId = "form123",
                    ExistingChildren = new ExistingChildren
                    {
                        AnyExistingChildren = false,
                        Children = new List<ExistingChild>(),
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

                response.Doc.FindAll(".existing-child").Count.Should().Be(1);

                response = response
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
