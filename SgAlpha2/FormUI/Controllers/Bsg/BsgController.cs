using System.Web.Mvc;
using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public static class BsgButtons
    {
        public const string AddChild    = "AddChild";
        public const string RemoveChild = "RemoveChild";
    }

    public static class BsgActions
    {
        public static string    Overview()                          { return "~/bsg/overview"; }
        public static string    AboutYou()                          { return "~/bsg/aboutYou"; }
        public static string    ExpectedChildren(string formId)     { return $"~/bsg/expectedChildren/{formId}"; }
        public static string    ExistingChildren(string formId)     { return $"~/bsg/existingChildren/{formId}"; }
        public static string    Complete()                          { return "~/bsg/complete"; }
    }

    public class BsgController : FormController
    {
        [HttpGet]
        public ActionResult Overview()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AboutYou()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AboutYou(AboutYou aboutYou)
        {
            var cmd = new StartBestStartGrant
            {
                AboutYou = aboutYou,
            };

            return Exec(cmd,
                success: formId => Redirect(BsgActions.ExpectedChildren(formId)),
                failure: () => AboutYou());
        }

        [HttpGet]
        public ActionResult ExpectedChildren(string id)
        {
            return View();
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
                success: () => Redirect(BsgActions.Complete()),
                failure: () => ExpectedChildren(id));
        }

        [HttpGet]
        public ActionResult ExistingChildren(string id)
        {
            var model = new ExistingChildren();
            return View(model);
        }

        [HttpPost]
        public ActionResult ExistingChildren(string id, ExistingChildren existingChildren)
        {
            if (WasClicked(BsgButtons.AddChild))
            {
                existingChildren.Children.Add(new ExistingChild());
                return View(existingChildren);
            }

            if (WasClicked(BsgButtons.RemoveChild))
            {
                var childIndex = int.Parse(Request.Form[BsgButtons.RemoveChild]);
                existingChildren.Children.RemoveAt(childIndex);
                RemoveModelStateArray<ExistingChildren>(m => m.Children, childIndex);
                return View(existingChildren);
            }

            var cmd = new AddExistingChildren
            {
                FormId = id,
                ExistingChildren = existingChildren,
            };

            return Exec(cmd,
                success: () => Redirect(BsgActions.Complete()),
                failure: () => View(existingChildren));
        }

        [HttpGet]
        public ActionResult Complete()
        {
            return View();
        }
    }
}