using System;
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
        public static string    Overview()                          { return $"~/bsg/overview"; }
        public static string    Start()                             { return $"~/bsg/start"; }
        public static string    Consent(string formId)              { return $"~/bsg/consent/{formId}"; }
        public static string    ApplicantDetails(string formId)     { return $"~/bsg/applicantDetails/{formId}"; }
        public static string    Ajax_DobChanged()                   { return $"~/bsg/ajax_dobChanged"; }
        public static string    ExpectedChildren(string formId)     { return $"~/bsg/expectedChildren/{formId}"; }
        public static string    ExistingChildren(string formId)     { return $"~/bsg/existingChildren/{formId}"; }
        public static string    ApplicantBenefits1(string formId)   { return $"~/bsg/applicantBenefits1/{formId}"; }
        public static string    ApplicantBenefits2(string formId)   { return $"~/bsg/applicantBenefits2/{formId}"; }
        public static string    GuardianDetails1(string formId)     { return $"~/bsg/guardianDetails1/{formId}"; }
        public static string    GuardianDetails2(string formId)     { return $"~/bsg/guardianDetails2/{formId}"; }
        public static string    HealthProfessional(string formId)   { return $"~/bsg/healthProfessional/{formId}"; }
        public static string    PaymentDetails(string formId)       { return $"~/bsg/paymentDetails/{formId}"; }
        public static string    Declaration(string formId)          { return $"~/bsg/declaration/{formId}"; }
        public static string    Complete()                          { return $"~/bsg/complete"; }
    }

    public class BsgViews
    {
        public const string Address = "Address";
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
        public ActionResult ApplicantDetails(string id)
        {
            return ApplicantDetails_Render(id, null);
        }

        [HttpPost]
        public ActionResult ApplicantDetails(string id, ApplicantDetails applicantDetails)
        {
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
            });
        }

        [HttpGet]
        public ActionResult ApplicantBenefits1(string id)
        {
            return ApplicantBenefits1_Render(id, null);
        }

        [HttpPost]
        public ActionResult ApplicantBenefits1(string id, ApplicantBenefits applicantBenefits)
        {
            var cmd = new AddApplicantBenefits
            {
                FormId = id,
                Part = Part.Part1,
                ApplicantBenefits = applicantBenefits,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => ApplicantBenefits1_Render(id, applicantBenefits));
        }

        private ActionResult ApplicantBenefits1_Render(string formId, ApplicantBenefits details)
        {
            return NavigableView<ApplicantBenefitsModel>(formId, Sections.ApplicantBenefits1, (m, f) =>
            {
                m.ApplicantBenefits = details ?? f.ApplicantBenefits;
            });
        }

        [HttpGet]
        public ActionResult ApplicantBenefits2(string id)
        {
            return ApplicantBenefits2_Render(id, null);
        }

        [HttpPost]
        public ActionResult ApplicantBenefits2(string id, ApplicantBenefits applicantBenefits)
        {
            var cmd = new AddApplicantBenefits
            {
                FormId = id,
                Part = Part.Part2,
                ApplicantBenefits = applicantBenefits,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => ApplicantBenefits2_Render(id, applicantBenefits));
        }

        private ActionResult ApplicantBenefits2_Render(string formId, ApplicantBenefits details)
        {
            return NavigableView<ApplicantBenefitsModel>(formId, Sections.ApplicantBenefits2, (m, f) =>
            {
                m.ApplicantBenefits = details ?? f.ApplicantBenefits;
            });
        }

        [HttpGet]
        public ActionResult GuardianDetails1(string id)
        {
            return GuardianDetails1_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianDetails1(string id, GuardianDetails guardianDetails)
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

        private ActionResult GuardianDetails1_Render(string formId, GuardianDetails details)
        {
            return NavigableView<GuardianDetailsModel>(formId, Sections.GuardianDetails1, (m, f) =>
            {
                m.GuardianDetails = details ?? f.GuardianDetails;
            });
        }

        [HttpGet]
        public ActionResult GuardianDetails2(string id)
        {
            return GuardianDetails2_Render(id, null);
        }

        [HttpPost]
        public ActionResult GuardianDetails2(string id, GuardianDetails guardianDetails)
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

        private ActionResult GuardianDetails2_Render(string formId, GuardianDetails details)
        {
            return NavigableView<GuardianDetailsModel>(formId, Sections.GuardianDetails2, (m, f) =>
            {
                m.GuardianDetails = details ?? f.GuardianDetails;
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
        public ActionResult Complete()
        {
            return View();
        }

        private RedirectResult RedirectNext(NextSection next)
        {
            if (!next.Section.HasValue)
                return Redirect(BsgActions.Complete());

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
            var form = !string.IsNullOrWhiteSpace(formId)
                ? FindForm(formId, section)
                : new BsgDetail();

            var model = new TModel();

            var previousSection = form.PreviousSection;

            if (previousSection.HasValue)
                model.PreviousAction = SectionActionStrategy.For(previousSection.Value).Action(formId);

            model.IsFinalPage = form.IsFinalSection;

            if (mutator != null)
                mutator(model, form);

            return View(model);
        }
    }
}