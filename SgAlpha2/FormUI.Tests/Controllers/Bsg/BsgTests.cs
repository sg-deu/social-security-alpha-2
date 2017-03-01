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
        public void Consent_POST_StartsForm()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<StartBestStartGrant>(), "form123");

                var response = client.Get(BsgActions.Consent()).Form<Consent>(1)
                    .SelectConfirm(m => m.AgreedToConsent, true)
                    .Submit(client);

                ExecutorStub.Executed<StartBestStartGrant>(0).ShouldBeEquivalentTo(new StartBestStartGrant
                {
                    Consent = new Consent { AgreedToConsent = true },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.ApplicantDetails("form123"));
            });
        }

        [Test]
        public void Consent_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<StartBestStartGrant, string>((cmd, r) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.Consent()).Form<Consent>(1)
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

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.ExpectedChildren("form123"));
            });
        }

        [Test]
        public void ApplicantDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1)
                    .SetDate(m => m.DateOfBirth, "in", "va", "lid")
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
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

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.ExistingChildren("form123"));
            });
        }

        [Test]
        public void ExpectedChildren_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExpectedChildren("form123")).Form<ExpectedChildren>(1)
                    .SetDate(m => m.ExpectancyDate, "in", "va", "lid")
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
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

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.ApplicantBenefits1("form123"));
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
                ExecutorStub.SetupVoidCommand(It.IsAny<AddExistingChildren>(), cmd => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.ExistingChildren("form123")).Form<ExistingChildren>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void ApplicantBenefits1_POST_CanAddApplicantBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantBenefits1("form123")).Form<ApplicantBenefits>(1)
                    .SelectNo(m => m.HasExistingBenefit)
                    .Submit(client);

                ExecutorStub.Executed<AddApplicantBenefits>(0).ShouldBeEquivalentTo(new AddApplicantBenefits
                {
                    FormId = "form123",
                    Part = Part.Part1,
                    ApplicantBenefits = new ApplicantBenefits
                    {
                        HasExistingBenefit = false,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.ApplicantBenefits2("form123"));
            });
        }

        [Test]
        public void ApplicantBenefits1_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupVoidCommand(It.IsAny<AddApplicantBenefits>(), cmd => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.ApplicantBenefits1("form123")).Form<ApplicantBenefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void ApplicantBenefits2_POST_CanAddApplicantBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantBenefits2("form123")).Form<ApplicantBenefits>(1)
                    .SelectNo(m => m.ReceivingBenefitForUnder20)
                    .SelectYes(m => m.YouOrPartnerInvolvedInTradeDispute)
                    .Submit(client);

                ExecutorStub.Executed<AddApplicantBenefits>(0).ShouldBeEquivalentTo(new AddApplicantBenefits
                {
                    FormId = "form123",
                    Part = Part.Part2,
                    ApplicantBenefits = new ApplicantBenefits
                    {
                        ReceivingBenefitForUnder20 = false,
                        YouOrPartnerInvolvedInTradeDispute = true,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.HealthProfessional("form123"));
            });
        }

        [Test]
        public void ApplicantBenefits2_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupVoidCommand(It.IsAny<AddApplicantBenefits>(), cmd => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.ApplicantBenefits1("form123")).Form<ApplicantBenefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
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

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.PaymentDetails("form123"));
            });
        }

        [Test]
        public void HealthProfessional_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupVoidCommand(It.IsAny<AddHealthProfessional>(), cmd => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.HealthProfessional("form123")).Form<HealthProfessional>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
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

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Declaration("form123"));
            });
        }

        [Test]
        public void PaymentDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupVoidCommand(It.IsAny<AddPaymentDetails>(), cmd => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.PaymentDetails("form123")).Form<AddPaymentDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void Declaration_POST_CompletesForm()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Declaration("form123")).Form<Declaration>(1)
                    .SelectConfirm(m => m.AgreedToLegalStatement, true)
                    .Submit(client);

                ExecutorStub.Executed<Complete>(0).ShouldBeEquivalentTo(new Complete
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
                ExecutorStub.SetupVoidCommand(It.IsAny<Complete>(), cmd => { throw new DomainException("simulated logic error"); });

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

                Consent             = ConsentBuilder.NewValid(),
                ApplicantDetails    = ApplicantDetailsBuilder.NewValid(),
                ExpectedChildren    = ExpectedChildrenBuilder.NewValid(),
                ExistingChildren    = ExistingChildrenBuilder.NewValid(),
                ApplicantBenefits   = ApplicantBenefitsBuilder.NewValid(Part.Part2),
                HealthProfessional  = HealthProfessionalBuilder.NewValid(),
                PaymentDetails      = PaymentDetailsBuilder.NewValid(),
                Declaration         = DeclarationBuilder.NewValid(),
            };

            return detail;
        }
    }
}
