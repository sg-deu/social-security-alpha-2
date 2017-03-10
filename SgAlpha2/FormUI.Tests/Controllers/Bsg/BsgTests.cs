using System;
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
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgTests : WebTest
    {
        [Test]
        public void Overview_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Overview());

                response.Doc.Document.Body.TextContent.Should().Contain("Overview");
            });
        }

        [Test]
        public void Overview_POST_StartsForm()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<StartBestStartGrant>(), new NextSection
                {
                    Id = "form123",
                    Section = Sections.Consent,
                });

                var response = client.Get(BsgActions.Overview()).Form<object>(1)
                    .Submit(client);

                ExecutorStub.Executed<StartBestStartGrant>().Length.Should().Be(1);

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void FirstSectionDoesNotNeedId()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupQuery<FindBsgSection, BsgDetail>((cmd, r) => { throw new DomainException("cannot call FindBsgSection with a null id"); });

                var firstSection = Navigation.Order.First();
                var firstAction = SectionActionStrategy.For(firstSection).Action(null);

                client.Get(firstAction);
            });
        }

        [Test]
        public void Consent_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.Consent(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.Consent });
                response.Doc.Form<Consent>(1).GetConfirm(m => m.AgreedToConsent).Should().Be(detail.Consent.AgreedToConsent);
            });
        }

        [Test]
        public void Consent_POST_PopulatesConsent()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Consent("form123")).Form<Consent>(1)
                    .SelectConfirm(m => m.AgreedToConsent, true)
                    .Submit(client);

                ExecutorStub.Executed<AddConsent>(0).ShouldBeEquivalentTo(new AddConsent
                {
                    FormId = "form123",
                    Consent = new Consent { AgreedToConsent = true },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void Consent_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddConsent, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.Consent("form123")).Form<Consent>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void ApplicantDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ApplicantDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.ApplicantDetails });
                response.Doc.Form<ApplicantDetails>(1).GetText(m => m.FirstName).Should().Be(detail.ApplicantDetails.FirstName);
            });
        }

        [Test]
        public void ApplicantDetails_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1)
                    .SetText(m => m.FirstName, "first name")
                    .Submit(client);

                ExecutorStub.Executed<AddApplicantDetails>(0).ShouldBeEquivalentTo(new AddApplicantDetails
                {
                    FormId = "form123",
                    ApplicantDetails = new ApplicantDetails { FirstName = "first name" },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void ApplicantDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddApplicantDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void ApplicantDetails_AjaxShowsHidesQuestions()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantDetails("form123"));

                ExecutorStub.SetupQuery(It.IsAny<FindApplicantDetailsConfig>(), new ApplicantDetailsConfig
                {
                    ShouldAskCareQuestion = true,
                    ShouldAskEducationQuestion = false,
                });

                var ajaxActions = response.Form<ApplicantDetails>(1)
                    .OnChange(f => f.DateOfBirth, client);

                ajaxActions.Should().NotBeNull();
                ajaxActions.Length.Should().Be(2);

                ajaxActions.ForFormGroup<ApplicantDetails>(f => f.PreviouslyLookedAfter).ShouldShowHide(response.Doc, true);
                ajaxActions.ForFormGroup<ApplicantDetails>(f => f.FullTimeEducation).ShouldShowHide(response.Doc, false);
            });
        }

        [Test]
        public void ExpectedChildren_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ExpectedChildren(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.ExpectedChildren });
                response.Doc.Form<ExpectedChildren>(1).GetText(m => m.ExpectedBabyCount).Should().Be(detail.ExpectedChildren.ExpectedBabyCount.ToString());
            });
        }

        [Test]
        public void ExpectedChildren_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExpectedChildren("form123")).Form<ExpectedChildren>(1)
                    .SetDate(m => m.ExpectancyDate, "01", "02", "2003")
                    .SetText(m => m.ExpectedBabyCount, "2")
                    .Submit(client);

                ExecutorStub.Executed<AddExpectedChildren>(0).ShouldBeEquivalentTo(new AddExpectedChildren
                {
                    FormId = "form123",
                    ExpectedChildren = new ExpectedChildren
                    {
                        ExpectancyDate = new DateTime(2003, 02, 01),
                        ExpectedBabyCount = 2,
                    },
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

                var response = client.Get(BsgActions.ExpectedChildren("form123")).Form<ExpectedChildren>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
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
        public void ExistingChildren_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExistingChildren("form123")).Form<ExistingChildren>(1)
                    .SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK)).Form<ExistingChildren>(1) // add a child
                    .SetText(m => m.Children[0].FirstName, "child 0 first name")
                    .SetText(m => m.Children[0].Surname, "child 0 surname")
                    .SetDate(m => m.Children[0].DateOfBirth, "03", "04", "2005")
                    .SetText(m => m.Children[0].RelationshipToChild, "child 0 relationship")
                    .SelectYes(m => m.Children[0].ChildBenefit)
                    .SelectYes(m => m.Children[0].FormalKinshipCare)
                    .SubmitName(BsgButtons.AddChild, client, r => r.SetExpectedResponse(HttpStatusCode.OK)).Form<ExistingChildren>(1) // add a second child
                    .SetText(m => m.Children[1].FirstName, "child 1 first name")
                    .SetText(m => m.Children[1].Surname, "child 1 surname")
                    .SetDate(m => m.Children[1].DateOfBirth, "02", "03", "2004")
                    .SetText(m => m.Children[1].RelationshipToChild, "child 1 relationship")
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
                                RelationshipToChild = "child 0 relationship",
                                ChildBenefit = true,
                                FormalKinshipCare = true,
                            },
                            new ExistingChild
                            {
                                FirstName = "child 1 first name",
                                Surname = "child 1 surname",
                                DateOfBirth = new DateTime(2004, 03, 02),
                                RelationshipToChild = "child 1 relationship",
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

        [Test]
        public void ApplicantBenefits_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ApplicantBenefits(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.ApplicantBenefits });
                response.Doc.Form<Benefits>(1).GetText(m => m.HasExistingBenefit).Should().Be(detail.ApplicantBenefits.HasExistingBenefit.ToString());
            });
        }

        [Test]
        public void ApplicantBenefits_POST_CanAddApplicantBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantBenefits("form123")).Form<Benefits>(1)
                    .SetText(m => m.HasExistingBenefit, YesNoDk.No.ToString())
                    .Submit(client);

                ExecutorStub.Executed<AddApplicantBenefits>(0).ShouldBeEquivalentTo(new AddApplicantBenefits
                {
                    FormId = "form123",
                    ApplicantBenefits = new Benefits
                    {
                        HasExistingBenefit = YesNoDk.No,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void ApplicantBenefits_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddApplicantBenefits, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.ApplicantBenefits("form123")).Form<Benefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void PartnerBenefits_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.PartnerBenefits(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.PartnerBenefits });
                response.Doc.Form<Benefits>(1).GetText(m => m.HasExistingBenefit).Should().Be(detail.PartnerBenefits.HasExistingBenefit.ToString());
            });
        }

        [Test]
        public void PartnerBenefits_POST_CanAddApplicantBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.PartnerBenefits("form123")).Form<Benefits>(1)
                    .SetText(m => m.HasExistingBenefit, YesNoDk.No.ToString())
                    .Submit(client);

                ExecutorStub.Executed<AddPartnerBenefits>(0).ShouldBeEquivalentTo(new AddPartnerBenefits
                {
                    FormId = "form123",
                    PartnerBenefits = new Benefits
                    {
                        HasExistingBenefit = YesNoDk.No,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void PartnerBenefits_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddPartnerBenefits, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.PartnerBenefits("form123")).Form<Benefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void GuardianBenefits_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianBenefits(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianBenefits });
                response.Doc.Form<Benefits>(1).GetText(m => m.HasExistingBenefit).Should().Be(detail.GuardianBenefits.HasExistingBenefit.ToString());
            });
        }

        [Test]
        public void GuardianBenefits_POST_CanAddGuardianBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianBenefits("form123")).Form<Benefits>(1)
                    .SetText(m => m.HasExistingBenefit, YesNoDk.No.ToString())
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianBenefits>(0).ShouldBeEquivalentTo(new AddGuardianBenefits
                {
                    FormId = "form123",
                    GuardianBenefits = new Benefits
                    {
                        HasExistingBenefit = YesNoDk.No,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianBenefits_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianBenefits, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianBenefits("form123")).Form<Benefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void GuardianPartnerBenefits_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianPartnerBenefits(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianPartnerBenefits });
                response.Doc.Form<Benefits>(1).GetText(m => m.HasExistingBenefit).Should().Be(detail.GuardianPartnerBenefits.HasExistingBenefit.ToString());
            });
        }

        [Test]
        public void GuardianPartnerBenefits_POST_CanAddGuardianPartnerBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianPartnerBenefits("form123")).Form<Benefits>(1)
                    .SetText(m => m.HasExistingBenefit, YesNoDk.No.ToString())
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianPartnerBenefits>(0).ShouldBeEquivalentTo(new AddGuardianPartnerBenefits
                {
                    FormId = "form123",
                    GuardianPartnerBenefits = new Benefits
                    {
                        HasExistingBenefit = YesNoDk.No,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianPartnerBenefits_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianPartnerBenefits, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianPartnerBenefits("form123")).Form<Benefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void GuardianDetails1_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianDetails1(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianDetails1 });
                response.Doc.Form<GuardianDetails>(1).GetText(m => m.Title).Should().Be(detail.GuardianDetails.Title);
            });
        }

        [Test]
        public void GuardianDetails1_POST_CanAddGuardianDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianDetails1("form123")).Form<GuardianDetails>(1)
                    .SetText(m => m.Title, "test title")
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianDetails>(0).ShouldBeEquivalentTo(new AddGuardianDetails
                {
                    FormId = "form123",
                    Part = Part.Part1,
                    GuardianDetails = new GuardianDetails
                    {
                        Title = "test title",
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianDetails1_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianDetails1("form123")).Form<GuardianDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void GuardianDetails2_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianDetails2(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianDetails2 });
                response.Doc.Form<GuardianDetails>(1).GetText(m => m.Address.Line1).Should().Be(detail.GuardianDetails.Address.Line1);
            });
        }

        [Test]
        public void GuardianDetails2_POST_CanAddGuardianDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianDetails2("form123")).Form<GuardianDetails>(1)
                    .SetText(m => m.Address.Line1, "line 1")
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianDetails>(0).ShouldBeEquivalentTo(new AddGuardianDetails
                {
                    FormId = "form123",
                    Part = Part.Part2,
                    GuardianDetails = new GuardianDetails
                    {
                        Address = new Address { Line1 = "line 1" },
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianDetails2_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianDetails2("form123")).Form<GuardianDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void HealthProfessional_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.HealthProfessional(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.HealthProfessional });
                response.Doc.Form<HealthProfessional>(1).GetText(m => m.Pin).Should().Be(detail.HealthProfessional.Pin);
            });
        }

        [Test]
        public void HealthProfessional_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.HealthProfessional("form123")).Form<HealthProfessional>(1)
                    .SetText(m => m.Pin, "ABC12345")
                    .Submit(client);

                ExecutorStub.Executed<AddHealthProfessional>(0).ShouldBeEquivalentTo(new AddHealthProfessional
                {
                    FormId = "form123",
                    HealthProfessional = new HealthProfessional
                    {
                        Pin = "ABC12345",
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void HealthProfessional_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddHealthProfessional, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.HealthProfessional("form123")).Form<HealthProfessional>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void PaymentDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.PaymentDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.PaymentDetails });
                response.Doc.Form<PaymentDetails>(1).GetText(m => m.NameOfAccountHolder).Should().Be(detail.PaymentDetails.NameOfAccountHolder);
            });
        }

        [Test]
        public void PaymentDetails_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.PaymentDetails("form123")).Form<PaymentDetails>(1)
                    .SelectNo(m => m.LackingBankAccount)
                    .SetText(m => m.NameOfAccountHolder, "test name")
                    .SetText(m => m.NameOfBank, "test bank")
                    .SetText(m => m.SortCode, "01-02-03")
                    .SetText(m => m.AccountNumber, "01234567")
                    .SetText(m => m.RollNumber, "roll/number")
                    .Submit(client);

                ExecutorStub.Executed<AddPaymentDetails>(0).ShouldBeEquivalentTo(new AddPaymentDetails
                {
                    FormId = "form123",
                    PaymentDetails = new PaymentDetails
                    {
                        LackingBankAccount = false,
                        NameOfAccountHolder = "test name",
                        NameOfBank = "test bank",
                        SortCode = "01-02-03",
                        AccountNumber = "01234567",
                        RollNumber = "roll/number",
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void PaymentDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddPaymentDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.PaymentDetails("form123")).Form<AddPaymentDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

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
                ExecutorStub.SetupCommand(It.IsAny<AddDeclaration>(), new NextSection { Section = null });

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
        public void Complete_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Complete());

                response.Text.ToLower().Should().Contain("received");
            });
        }

        private static BsgDetail NewBsgDetail(string formId)
        {
            var detail = new BsgDetail
            {
                Id = formId,

                Consent                 = ConsentBuilder.NewValid(),
                ApplicantDetails        = ApplicantDetailsBuilder.NewValid(),
                ExpectedChildren        = ExpectedChildrenBuilder.NewValid(),
                ExistingChildren        = ExistingChildrenBuilder.NewValid(),
                ApplicantBenefits       = BenefitsBuilder.NewValid(),
                PartnerBenefits         = BenefitsBuilder.NewValid(),
                GuardianBenefits        = BenefitsBuilder.NewValid(),
                GuardianPartnerBenefits = BenefitsBuilder.NewValid(),
                GuardianDetails         = GuardianDetailsBuilder.NewValid(Part.Part2),
                HealthProfessional      = HealthProfessionalBuilder.NewValid(),
                PaymentDetails          = PaymentDetailsBuilder.NewValid(),
                Declaration             = DeclarationBuilder.NewValid(),
            };

            return detail;
        }
    }
}
