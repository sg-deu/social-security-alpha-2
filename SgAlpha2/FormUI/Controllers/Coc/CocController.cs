using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FormUI.Controllers.Shared;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Queries;
using FormUI.Domain.ChangeOfCircsForm.Responses;

namespace FormUI.Controllers.Coc
{
    public static class CocButtons
    {
        public const string UploadFile = "UploadFile";
        public const string RemoveFile = "RemoveFile";
    }

    public static class CocActions
    {
        public static string Overview()                     { return $"~/coc/overview"; }
        public static string Consent(string id)             { return $"~/coc/consent/{id}"; }
        public static string Identity(string id)            { return $"~/coc/identity/{id}"; }
        public static string Options(string id)             { return $"~/coc/options/{id}"; }
        public static string ApplicantDetails(string id)    { return $"~/coc/applicantDetails/{id}"; }
        public static string ExpectedChildren(string id)    { return $"~/coc/expectedChildren/{id}"; }
        public static string HealthProfessional(string id)  { return $"~/coc/healthProfessional/{id}"; }
        public static string PaymentDetails(string id)      { return $"~/coc/paymentDetails/{id}"; }
        public static string Evidence(string id)            { return $"~/coc/evidence/{id}"; }
        public static string Declaration(string id)         { return $"~/coc/declaration/{id}"; }
        public static string Complete()                     { return $"~/coc/complete"; }
    }

    public class CocText
    {
        public static string TitlePrefix() { return "Change of Circumstances - "; }
    }

    public class CocController : FormController
    {
        [HttpPost]
        public ActionResult Overview(object notUsed)
        {
            var cmd = new StartChangeOfCircs();

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
        public ActionResult Identity(string id)
        {
            //Retrieve the formid (session identifier) that we saved prior to leaving BSG and entering GOV.UKVerify site
            if (Request.Cookies["formId"] != null && id == null)
            {
                System.Web.HttpCookie aCookie = Request.Cookies["formId"];
                id = aCookie.Value;
            }

            return Identity_Render(id, null);
        }

        [HttpPost]
        public ActionResult Identity(string id, IdentityModel model)
        {
            //Retrieve the formid (session identifier) that we saved prior to leaving BSG and entering GOV.UKVerify site
            if (Request.Cookies["formId"] != null && id == null)
            {
                System.Web.HttpCookie aCookie = Request.Cookies["formId"];
                id = aCookie.Value;
            }

            var cmd = new AddIdentity
            {
                FormId = id,
                Identity = model.Email,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => Identity_Render(id, model.Email));
        }

        private ActionResult Identity_Render(string formId, string email)
        {
            //Save the formid (session identifier) prior to leaving BSG and entering GOV.UKVerify site
            System.Web.HttpCookie aCookie = new System.Web.HttpCookie("formId");
            aCookie.Value = formId;
            aCookie.Expires = DateTime.UtcNow.AddHours(1);
            Response.Cookies.Add(aCookie);

            return NavigableView<IdentityModel>(formId, Sections.Identity, (m, f) =>
            {
                var actionUrl = System.Configuration.ConfigurationManager.AppSettings["gov.ukverifylogin"];

                m.VerifyPath = actionUrl;
                m.Email = email ?? f.Identity;
            });
        }

        [HttpGet]
        public ActionResult Options(string id)
        {
            return Options_Render(id, null);
        }

        [HttpPost]
        public ActionResult Options(string id, Options options)
        {
            var cmd = new AddOptions
            {
                FormId = id,
                Options = options,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => Options_Render(id, options));
        }

        private ActionResult Options_Render(string formId, Options details)
        {
            return NavigableView<OptionsModel>(formId, Sections.Options, (m, f) =>
            {
                m.Options = details ?? f.Options;
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

        [HttpGet]
        public ActionResult ExpectedChildren(string id)
        {
            return ExpectedChildren_Render(id, null);
        }

        private ActionResult ExpectedChildren_Render(string formId, ExpectedChildren details)
        {
            return NavigableView<ExpectedChildrenModel>(formId, Bsg.BsgViews.ExpectedChildren, Sections.ExpectedChildren, (m, f) =>
            {
                m.TitlePrefix = CocText.TitlePrefix();
                m.Title = "Expected baby";
                m.ExpectedChildren = details ?? f.ExpectedChildren;
            });
        }

        [HttpGet]
        public ActionResult HealthProfessional(string id)
        {
            return HealthProfessional_Render(id, null);
        }

        private ActionResult HealthProfessional_Render(string formId, HealthProfessional details)
        {
            return NavigableView<HealthProfessionalModel>(formId, Bsg.BsgViews.HealthProfessional, Sections.HealthProfessional, (m, f) =>
            {
                m.TitlePrefix = CocText.TitlePrefix();
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
        public ActionResult Evidence(string id)
        {
            return Evidence_Render(id, null);
        }

        [HttpPost]
        public ActionResult Evidence(string id, Evidence evidence)
        {
            if (WasClicked(CocButtons.UploadFile))
            {
                if (Request.Files.Count == 0)
                {
                    ModelState.AddModelError("", "Could not upload file");
                    return Evidence_Render(id, evidence);
                }

                var file = Request.Files[0];

                if (string.IsNullOrWhiteSpace(file.FileName))
                {
                    ModelState.AddModelError("", "Please select a file to upload");
                    return Evidence_Render(id, evidence);
                }

                const int maxSize = 1024 * 1024 * 2;

                if (file.ContentLength > maxSize)
                {
                    ModelState.AddModelError("", "Please select a file that is smaller than 2MB");
                    return Evidence_Render(id, evidence);
                }

                // TODO: Add remaining validation - ie: filetype and max 20 files

                using (var br = new BinaryReader(file.InputStream))
                {
                    var addFile = new AddEvidenceFile
                    {
                        FormId = id,
                        Filename = file.FileName,
                        Content = br.ReadBytes((int)file.InputStream.Length),
                    };

                    return Exec(addFile,
                        success: () => Redirect(CocActions.Evidence(id)),
                        failure: () => Evidence_Render(id, evidence));
                }
            }
            else if (WasClicked(CocButtons.RemoveFile))
            {
                var cloudName = Request.Form[CocButtons.RemoveFile];

                var delFile = new RemoveEvidenceFile
                {
                    FormId = id,
                    CloudName = cloudName,
                };

                return Exec(delFile,
                    success: () => Redirect(CocActions.Evidence(id)),
                    failure: () => Evidence_Render(id, evidence));
            }

            var cmd = new AddEvidence
            {
                FormId = id,
                Evidence = evidence,
            };

            return Exec(cmd,
                success: next => RedirectNext(next),
                failure: () => Evidence_Render(id, evidence));
        }

        private ActionResult Evidence_Render(string formId, Evidence details)
        {
            return NavigableView<EvidenceModel>(formId, Sections.Evidence, (m, f) =>
            {
                m.Evidence = details ?? f.Evidence;

                m.UploadedFiles = f.Evidence != null
                    ? f.Evidence.Files.Select(ef => ef.Name).ToList()
                    : new List<string>();
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
                m.RequiresGuardianDeclaration = f.ApplicantDetails.Age < 16;
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
                return Redirect(CocActions.Complete());

            var action = SectionActionStrategy.For(next.Section.Value).Action(next.Id);
            return Redirect(action);
        }

        private CocDetail FindForm(string formId, Sections section)
        {
            var query = new FindCocSection
            {
                FormId = formId,
                Section = section,
            };

            var form = Exec(query);

            return form;
        }

        private ActionResult NavigableView<TModel>(string formId, Sections section, Action<TModel, CocDetail> mutator)
            where TModel : NavigableModel, new()
        {
            return NavigableView(formId, null, section, mutator);
        }

        private ActionResult NavigableView<TModel>(string formId, string view, Sections section, Action<TModel, CocDetail> mutator)
            where TModel : NavigableModel, new()
        {
            var form = !string.IsNullOrWhiteSpace(formId)
                ? FindForm(formId, section)
                : new CocDetail();

            var model = new TModel();

            var previousSection = form.PreviousSection;

            if (previousSection.HasValue)
                model.PreviousAction = SectionActionStrategy.For(previousSection.Value).Action(formId);

            model.IsFinalPage = form.IsFinalSection;

            if (mutator != null)
                mutator(model, form);

            return View(view, model);
        }
    }
}