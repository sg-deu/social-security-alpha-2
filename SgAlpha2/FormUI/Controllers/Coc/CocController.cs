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
    }

    public static class CocActions
    {
        public static string Overview()                     { return $"~/coc/overview"; }
        public static string Consent(string id)             { return $"~/coc/consent/{id}"; }
        public static string Identity(string id)            { return $"~/coc/identity/{id}"; }
        public static string Options(string id)             { return $"~/coc/options/{id}"; }
        public static string ApplicantDetails(string id)    { return $"~/coc/applicantDetails/{id}"; }
        public static string Evidence(string id)            { return $"~/coc/evidence/{id}"; }
        public static string Complete()                     { return $"~/coc/complete"; }
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
            return Identity_Render(id, null);
        }

        [HttpPost]
        public ActionResult Identity(string id, IdentityModel model)
        {
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
            return NavigableView<IdentityModel>(formId, Sections.Identity, (m, f) =>
            {
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