﻿using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Forms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgGuardianPartnerDetailsTests : BsgSectionTest
    {
        [Test]
        public void GuardianPartnerDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianPartnerDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianPartnerDetails });
                response.Doc.Form<RelationDetails>(1).GetText(m => m.Title).Should().Be(detail.GuardianPartnerDetails.Title);
            });
        }

        [Test]
        public void GuardianPartnerDetails_POST_CanAddGuardianPartnerDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianPartnerDetails("form123")).Form<RelationDetails>(1)
                    .SetText(m => m.Title, "test title")
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianPartnerDetails>(0).ShouldBeEquivalentTo(new AddGuardianPartnerDetails
                {
                    FormId = "form123",
                    GuardianPartnerDetails = new RelationDetails
                    {
                        Title = "test title",
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianPartnerDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianPartnerDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianPartnerDetails("form123")).Form<RelationDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void GuardianPartnerDetails_GET_PopulatesExistingAddressDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianPartnerDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianPartnerDetails });
                response.Doc.Form<RelationDetails>(1).GetText(m => m.Address.Line1).Should().Be(detail.GuardianPartnerDetails.Address.Line1);
            });
        }

        [Test]
        public void GuardianPartnerDetails_POST_CanAddGuardianPartnerAddressDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianPartnerDetails("form123")).Form<RelationDetails>(1)
                    .SetText(m => m.Address.Line1, "line 1")
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianPartnerDetails>(0).ShouldBeEquivalentTo(new AddGuardianPartnerDetails
                {
                    FormId = "form123",
                    GuardianPartnerDetails = new RelationDetails
                    {
                        Address = new Address { Line1 = "line 1" },
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }
    }
}
