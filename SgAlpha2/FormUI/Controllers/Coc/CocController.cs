using System.Web.Mvc;
using FormUI.Controllers.Shared;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Responses;

namespace FormUI.Controllers.Coc
{
    public static class CocActions
    {
        public static string Overview()         { return $"~/coc/overview"; }
        public static string Consent(string id) { return $"~/coc/consent/{id}"; }
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
            return View();
        }

        private RedirectResult RedirectNext(NextSection next)
        {
            if (!next.Section.HasValue)
                throw new System.Exception("not handled yet");

            var action = SectionActionStrategy.For(next.Section.Value).Action(next.Id);
            return Redirect(action);
        }
    }
}