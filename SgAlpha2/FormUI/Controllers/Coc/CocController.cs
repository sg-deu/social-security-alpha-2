using System;
using System.Web.Mvc;
using FormUI.Controllers.Shared;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Queries;
using FormUI.Domain.ChangeOfCircsForm.Responses;

namespace FormUI.Controllers.Coc
{
    public static class CocActions
    {
        public static string Overview()             { return $"~/coc/overview"; }
        public static string Consent(string id)     { return $"~/coc/consent/{id}"; }
        public static string Identity(string id)    { return $"~/coc/identity/{id}"; }
        public static string Complete()             { return $"~/coc/complete"; }
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