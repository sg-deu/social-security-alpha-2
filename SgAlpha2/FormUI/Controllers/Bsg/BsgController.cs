﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FormUI.Controllers.Helpers;
using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;

namespace FormUI.Controllers.Bsg
{
    public static class BsgButtons
    {
        public const string AddChild    = "AddChild";
        public const string RemoveChild = "RemoveChild";
    }

    public static class BsgActions
    {
        public static string    Overview()                              { return $"~/bsg/overview"; }
        public static string    Consent(string formId)                  { return $"~/bsg/consent/{formId}"; }
        public static string    UKVerify(string formId)                 { return $"~/bsg/ukverify/{formId}"; }
        public static string    ApplicantDetails(string formId)         { return $"~/bsg/applicantDetails/{formId}"; }
        public static string    Ajax_DobChanged()                       { return $"~/bsg/ajax_dobChanged"; }
        public static string    ExpectedChildren(string formId)         { return $"~/bsg/expectedChildren/{formId}"; }
        public static string    ExistingChildren(string formId)         { return $"~/bsg/existingChildren/{formId}"; }
        public static string    ApplicantBenefits(string formId)        { return $"~/bsg/applicantBenefits/{formId}"; }
        public static string    PartnerBenefits(string formId)          { return $"~/bsg/partnerBenefits/{formId}"; }
        public static string    GuardianBenefits(string formId)         { return $"~/bsg/guardianBenefits/{formId}"; }
        public static string    GuardianPartnerBenefits(string formId)  { return $"~/bsg/guardianPartnerBenefits/{formId}"; }
        public static string    PartnerDetails1(string formId)          { return $"~/bsg/partnerDetails1/{formId}"; }
        public static string    PartnerDetails2(string formId)          { return $"~/bsg/partnerDetails2/{formId}"; }
        public static string    GuardianDetails1(string formId)         { return $"~/bsg/guardianDetails1/{formId}"; }
        public static string    GuardianDetails2(string formId)         { return $"~/bsg/guardianDetails2/{formId}"; }
        public static string    GuardianPartnerDetails1(string formId)  { return $"~/bsg/guardianPartnerDetails1/{formId}"; }
        public static string    GuardianPartnerDetails2(string formId)  { return $"~/bsg/guardianPartnerDetails2/{formId}"; }
        public static string    HealthProfessional(string formId)       { return $"~/bsg/healthProfessional/{formId}"; }
        public static string    PaymentDetails(string formId)           { return $"~/bsg/paymentDetails/{formId}"; }
        public static string    Declaration(string formId)              { return $"~/bsg/declaration/{formId}"; }
        public static string    Ineligible(string formId)               { return $"~/bsg/ineligible/{formId}"; }
        public static string    Complete()                              { return $"~/bsg/complete"; }
    }

    public class BsgViews
    {
        public const string Benefits            = "Benefits";
        public const string RelationDetails1    = "RelationDetails1";
        public const string RelationDetails2    = "RelationDetails2";
    }

    public class BsgController : FormController
    {
        [HttpGet]
        public ActionResult Overview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Overview(object notUsed)
        {
            var cmd = new StartBestStartGrant();

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => View());
        }

        [HttpGet]
        public ActionResult Consent(string id)
        {
            return Consent_Render(id, null);
        }

        [HttpPost]
        public ActionResult Consent(string id, Consent consent)
        {
            var cmd = new AddConsent
            {
                FormId = id,
                Consent = consent,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => Consent_Render(id, consent));
        }

        private ActionResult Consent_Render(string formId, Consent details)
        {
            return NavigableView<ConsentModel>(formId, Sections.Consent, (m, f) =>
            {
                m.Consent = details ?? f.Consent;
            });
        }

        [HttpGet]
        public ActionResult UKVerify(string id)
        {
            return UKVerify_Render(id, null);
        }

        [HttpPost]
        public ActionResult UKVerify(string id, UKVerify ukverify)
        {
            var cmd = new AddUKVerify
            {
                FormId = id,
                UKVerify = ukverify,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => UKVerify_Render(id, ukverify));
        }

        private ActionResult UKVerify_Render(string formId, UKVerify details)
        {
            //Save the formid (session identifier) prior to leaving BSG and entering GOV.UKVerify site
            System.Web.HttpCookie aCookie = new System.Web.HttpCookie("formId");
            aCookie.Value = formId;
            aCookie.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(aCookie);

            return NavigableView<UKVerifyModel>(formId, Sections.UKVerify, (m, f) =>
            {
                m.UKVerify = details ?? f.UKVerify;
            });
        }

        [HttpGet]
        public ActionResult ApplicantDetails(string id)
        {
            //Retrieve the formid (session identifier) that we saved prior to leaving BSG and entering GOV.UKVerify site
            if (Request.Cookies["formId"] != null && id == null)
            {
                System.Web.HttpCookie aCookie = Request.Cookies["formId"];
                id = aCookie.Value;
            }

            return ApplicantDetails_Render(id, null);
        }

        [HttpPost]
        public ActionResult ApplicantDetails(string id, ApplicantDetails applicantDetails)
        {
            //Retrieve the formid (session identifier) that we saved prior to leaving BSG and entering GOV.UKVerify site
            if (Request.Cookies["formId"] != null && id == null)
            {
                System.Web.HttpCookie aCookie = Request.Cookies["formId"];
                id = aCookie.Value;
            }

            var cmd = new AddApplicantDetails
            {
                FormId = id,
                ApplicantDetails = applicantDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => ApplicantDetails_Render(id, applicantDetails));
        }

        private ActionResult ApplicantDetails_Render(string formId, ApplicantDetails details)
        {
            return NavigableView<ApplicantDetailsModel>(formId, Sections.ApplicantDetails, (m, f) =>
            {
                m.ApplicantDetails = details ?? f.ApplicantDetails;
            });
        }

        [HttpPost]
        public ActionResult Ajax_DobChanged(ApplicantDetails applicantDetails)
        {
            var config = Exec(new FindApplicantDetailsConfig { ApplicantDetails = applicantDetails });

            return AjaxActions(new[]
            {
                AjaxAction.ShowHideFormGroup<ApplicantDetails>(m => m.PreviouslyLookedAfter, config.ShouldAskCareQuestion),
                AjaxAction.ShowHideFormGroup<ApplicantDetails>(m => m.FullTimeEducation, config.ShouldAskEducationQuestion),
            });
        }

        [HttpGet]
        public ActionResult ExpectedChildren(string id)
        {
            return ExpectedChildren_Render(id, null);
        }

        [HttpPost]
        public ActionResult ExpectedChildren(string id, ExpectedChildren expectedChildren)
        {
            var cmd = new AddExpectedChildren
            {
                FormId = id,
                ExpectedChildren = expectedChildren,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => ExpectedChildren_Render(id, expectedChildren));
        }

        private ActionResult ExpectedChildren_Render(string formId, ExpectedChildren details)
        {
            return NavigableView<ExpectedChildrenModel>(formId, Sections.ExpectedChildren, (m, f) =>
            {
                m.ExpectedChildren = details ?? f.ExpectedChildren;
            });
        }

        [HttpGet]
        public ActionResult ExistingChildren(string id)
        {
            return ExistingChildren_Render(id, null);
        }

        [HttpPost]
        public ActionResult ExistingChildren(string id, ExistingChildren existingChildren)
        {
            if (WasClicked(BsgButtons.AddChild))
            {
                existingChildren.Children.Add(new ExistingChild());
                return ExistingChildren_Render(id, existingChildren);
            }

            if (WasClicked(BsgButtons.RemoveChild))
            {
                var childIndex = int.Parse(Request.Form[BsgButtons.RemoveChild]);
                existingChildren.Children.RemoveAt(childIndex);
                RemoveModelStateArray<ExistingChildren>(m => m.Children, childIndex);
                return ExistingChildren_Render(id, existingChildren);
            }

            if (existingChildren.AnyExistingChildren == false)
                existingChildren.Children?.Clear();

            var cmd = new AddExistingChildren
            {
                FormId = id,
                ExistingChildren = existingChildren,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => ExistingChildren_Render(id, existingChildren));
        }

        private ActionResult ExistingChildren_Render(string formId, ExistingChildren details)
        {
            return NavigableView<ExistingChildrenModel>(formId, Sections.ExistingChildren, (m, f) =>
            {
                m.ExistingChildren = details ?? f.ExistingChildren ?? new ExistingChildren();

                if (m.ExistingChildren.Children?.Count < 1)
                    m.ExistingChildren.Children = new List<ExistingChild> { new ExistingChild() };
            });
        }

        [HttpGet]
        public ActionResult ApplicantBenefits(string id)
        {
            return ApplicantBenefits_Render(id, null);
        }

        [HttpPost]
        public ActionResult ApplicantBenefits(string id, Benefits applicantBenefits)
        {
            var cmd = new AddApplicantBenefits
            {
                FormId = id,
                ApplicantBenefits = applicantBenefits,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => ApplicantBenefits_Render(id, applicantBenefits));
        }

        private ActionResult ApplicantBenefits_Render(string formId, Benefits details)
        {
            return NavigableView<BenefitsModel>(formId, BsgViews.Benefits, Sections.ApplicantBenefits, (m, f) =>
            {
                m.Title     = "Your benefits";
                m.Question  = "Tick all benefits you are currently receiving";
                m.Benefits  = details ?? f.ApplicantBenefits;
            });
        }

        [HttpGet]
        public ActionResult PartnerBenefits(string id)
        {
            return PartnerBenefits_Render(id, null);
        }

        [HttpPost]
        public ActionResult PartnerBenefits(string id, Benefits partnerBenefits)
        {
            var cmd = new AddPartnerBenefits
            {
                FormId = id,
                PartnerBenefits = partnerBenefits,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => PartnerBenefits_Render(id, partnerBenefits));
        }

        private ActionResult PartnerBenefits_Render(string formId, Benefits details)
        {
            return NavigableView<BenefitsModel>(formId, BsgViews.Benefits, Sections.PartnerBenefits, (m, f) =>
            {
                m.Title     = "Your Partner's benefits";
                m.Question  = "Tick all the benefits your partner is currently receiving";
                m.Benefits  = details ?? f.PartnerBenefits;
            });
        }

        [HttpGet]
        public ActionResult GuardianBenefits(string id)
        {
            return GuardianBenefits_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianBenefits(string id, Benefits guardianBenefits)
        {
            var cmd = new AddGuardianBenefits
            {
                FormId = id,
                GuardianBenefits = guardianBenefits,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => GuardianBenefits_Render(id, guardianBenefits));
        }

        private ActionResult GuardianBenefits_Render(string formId, Benefits details)
        {
            return NavigableView<BenefitsModel>(formId, BsgViews.Benefits, Sections.GuardianBenefits, (m, f) =>
            {
                m.Title     = "Your Parent/Legal Guardian's benefits";
                m.Question  = "Tick all benefits your parent/legal guardian is currently receiving";
                m.Benefits  = details ?? f.GuardianBenefits;
            });
        }

        [HttpGet]
        public ActionResult GuardianPartnerBenefits(string id)
        {
            return GuardianPartnerBenefits_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianPartnerBenefits(string id, Benefits guardianPartnerBenefits)
        {
            var cmd = new AddGuardianPartnerBenefits
            {
                FormId = id,
                GuardianPartnerBenefits = guardianPartnerBenefits,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => GuardianPartnerBenefits_Render(id, guardianPartnerBenefits));
        }

        private ActionResult GuardianPartnerBenefits_Render(string formId, Benefits details)
        {
            return NavigableView<BenefitsModel>(formId, BsgViews.Benefits, Sections.GuardianPartnerBenefits, (m, f) =>
            {
                m.Title     = "Your Parent/Legal Guardian's Partner's benefits";
                m.Question  = "Tick all benefits your Parent/Legal Guardian's Partner is currently receiving";
                m.Benefits  = details ?? f.GuardianPartnerBenefits;
            });
        }

        [HttpGet]
        public ActionResult PartnerDetails1(string id)
        {
            return PartnerDetails1_Render(id, null);
        }

        [HttpPost]
        public ActionResult PartnerDetails1(string id, RelationDetails partnerDetails)
        {
            var cmd = new AddPartnerDetails
            {
                FormId = id,
                Part = Part.Part1,
                PartnerDetails = partnerDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => PartnerDetails1_Render(id, partnerDetails));
        }

        private ActionResult PartnerDetails1_Render(string formId, RelationDetails details)
        {
            return NavigableView<RelationDetailsModel>(formId, BsgViews.RelationDetails1, Sections.PartnerDetails1, (m, f) =>
            {
                m.Title = "Your Partner part 1";
                m.Heading = "Your Partner";
                m.HideRelationship = true;
                m.RelationDetails = details ?? f.PartnerDetails;
            });
        }

        [HttpGet]
        public ActionResult PartnerDetails2(string id)
        {
            return PartnerDetails2_Render(id, null);
        }

        [HttpPost]
        public ActionResult PartnerDetails2(string id, RelationDetails partnerDetails)
        {
            var cmd = new AddPartnerDetails
            {
                FormId = id,
                Part = Part.Part2,
                PartnerDetails = partnerDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => PartnerDetails2_Render(id, partnerDetails));
        }

        private ActionResult PartnerDetails2_Render(string formId, RelationDetails details)
        {
            return NavigableView<RelationDetailsModel>(formId, BsgViews.RelationDetails2, Sections.PartnerDetails2, (m, f) =>
            {
                m.Title = "Your Partner part 2";
                m.Heading = "Your Partner's address";
                m.RelationDetails = details ?? f.PartnerDetails;
                m.InheritedAddress = f.ApplicantDetails?.CurrentAddress;
            });
        }

        [HttpGet]
        public ActionResult GuardianDetails1(string id)
        {
            return GuardianDetails1_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianDetails1(string id, RelationDetails guardianDetails)
        {
            var cmd = new AddGuardianDetails
            {
                FormId = id,
                Part = Part.Part1,
                GuardianDetails = guardianDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => GuardianDetails1_Render(id, guardianDetails));
        }

        private ActionResult GuardianDetails1_Render(string formId, RelationDetails details)
        {
            return NavigableView<RelationDetailsModel>(formId, BsgViews.RelationDetails1, Sections.GuardianDetails1, (m, f) =>
            {
                m.Title = "Your Parent/Legal Guardian part 1";
                m.Heading = "Your Parent/Legal Guardian";
                m.RelationDetails = details ?? f.GuardianDetails;
            });
        }

        [HttpGet]
        public ActionResult GuardianDetails2(string id)
        {
            return GuardianDetails2_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianDetails2(string id, RelationDetails guardianDetails)
        {
            var cmd = new AddGuardianDetails
            {
                FormId = id,
                Part = Part.Part2,
                GuardianDetails = guardianDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => GuardianDetails2_Render(id, guardianDetails));
        }

        private ActionResult GuardianDetails2_Render(string formId, RelationDetails details)
        {
            return NavigableView<RelationDetailsModel>(formId, BsgViews.RelationDetails2, Sections.GuardianDetails2, (m, f) =>
            {
                m.Title = "Your Parent/Legal Guardian part 2";
                m.Heading = "Your Parent/Legal Guardian's address";
                m.RelationDetails = details ?? f.GuardianDetails;
            });
        }

        [HttpGet]
        public ActionResult GuardianPartnerDetails1(string id)
        {
            return GuardianPartnerDetails1_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianPartnerDetails1(string id, RelationDetails guardianPartnerDetails)
        {
            var cmd = new AddGuardianPartnerDetails
            {
                FormId = id,
                Part = Part.Part1,
                GuardianPartnerDetails = guardianPartnerDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => GuardianPartnerDetails1_Render(id, guardianPartnerDetails));
        }

        private ActionResult GuardianPartnerDetails1_Render(string formId, RelationDetails details)
        {
            return NavigableView<RelationDetailsModel>(formId, BsgViews.RelationDetails1, Sections.GuardianPartnerDetails1, (m, f) =>
            {
                m.Title = "Your Parent/Legal Guardian's Partner part 1";
                m.Heading = "Your Parent/Legal Guardian's Partner";
                m.RelationDetails = details ?? f.GuardianPartnerDetails;
            });
        }

        [HttpGet]
        public ActionResult GuardianPartnerDetails2(string id)
        {
            return GuardianPartnerDetails2_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianPartnerDetails2(string id, RelationDetails guardianPartnerDetails)
        {
            var cmd = new AddGuardianPartnerDetails
            {
                FormId = id,
                Part = Part.Part2,
                GuardianPartnerDetails = guardianPartnerDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => GuardianDetails2_Render(id, guardianPartnerDetails));
        }

        private ActionResult GuardianPartnerDetails2_Render(string formId, RelationDetails details)
        {
            return NavigableView<RelationDetailsModel>(formId, BsgViews.RelationDetails2, Sections.GuardianPartnerDetails2, (m, f) =>
            {
                m.Title = "Your Parent/Legal Guardian's Partner part 2";
                m.Heading = "Your Parent/Legal Guardian's Partner's address";
                m.RelationDetails = details ?? f.GuardianPartnerDetails;
            });
        }

        [HttpGet]
        public ActionResult HealthProfessional(string id)
        {
            return HealthProfessional_Render(id, null);
        }

        [HttpPost]
        public ActionResult HealthProfessional(string id, HealthProfessional healthProfessional)
        {
            var cmd = new AddHealthProfessional
            {
                FormId = id,
                HealthProfessional = healthProfessional,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => HealthProfessional_Render(id, healthProfessional));
        }

        private ActionResult HealthProfessional_Render(string formId, HealthProfessional details)
        {
            return NavigableView<HealthProfessionalModel>(formId, Sections.HealthProfessional, (m, f) =>
            {
                m.HealthProfessional = details ?? f.HealthProfessional;
            });
        }

        [HttpGet]
        public ActionResult PaymentDetails(string id)
        {
            return PaymentDetails_Render(id, null);
        }

        [HttpPost]
        public ActionResult PaymentDetails(string id, PaymentDetails paymentDetails)
        {
            var cmd = new AddPaymentDetails
            {
                FormId = id,
                PaymentDetails = paymentDetails,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => PaymentDetails_Render(id, paymentDetails));
        }

        private ActionResult PaymentDetails_Render(string formId, PaymentDetails details)
        {
            return NavigableView<PaymentDetailsModel>(formId, Sections.PaymentDetails, (m, f) =>
            {
                m.PaymentDetails = details ?? f.PaymentDetails;
            });
        }

        [HttpGet]
        public ActionResult Declaration(string id)
        {
            return Declaration_Render(id, null);
        }

        [HttpPost]
        public ActionResult Declaration(string id, Declaration declaration)
        {
            var cmd = new AddDeclaration
            {
                FormId = id,
                Declaration = declaration,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => Declaration_Render(id, declaration));
        }

        private ActionResult Declaration_Render(string formId, Declaration details)
        {
            return NavigableView<DeclarationModel>(formId, Sections.Declaration, (m, f) =>
            {
                m.Declaration = details ?? f.Declaration;
            });
        }

        [HttpGet]
        public ActionResult Ineligible(string id)
        {
            var model = new IneligibleModel { Id = id };
            return View(model);
        }

        [HttpGet]
        public ActionResult Complete()
        {
            return View();
        }

        private RedirectResult RedirectNext(NextSection next)
        {
            if (next.Type == NextType.Complete)
                return Redirect(BsgActions.Complete());

            if (next.Type == NextType.Ineligible)
                return Redirect(BsgActions.Ineligible(next.Id));

            var action = SectionActionStrategy.For(next.Section.Value).Action(next.Id);
            return Redirect(action);
        }

        private BsgDetail FindForm(string formId, Sections section)
        {
            var query = new FindBsgSection
            {
                FormId = formId,
                Section = section,
            };

            var form = Exec(query);

            return form;
        }

        private ActionResult NavigableView<TModel>(string formId, Sections section, Action<TModel, BsgDetail> mutator)
            where TModel : NavigableModel, new()
        {
            return NavigableView(formId, null, section, mutator);
        }

        private ActionResult NavigableView<TModel>(string formId, string view, Sections section, Action<TModel, BsgDetail> mutator)
            where TModel : NavigableModel, new()
        {
            var form = !string.IsNullOrWhiteSpace(formId)
                ? FindForm(formId, section)
                : new BsgDetail();

            var model = new TModel();

            var previousSection = form.PreviousSection;

            if (previousSection.HasValue)
                model.PreviousAction = SectionActionStrategy.For(previousSection.Value).Action(formId);

            model.IsFinalPage = form.IsFinalSection;
            mutator(model, form);

            return View(view, model);
        }
    }
}