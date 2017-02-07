using System.Web.Mvc;

namespace FormUI.Controllers.Harness
{
    public static class HarnessActions
    {
        public static string Index()        { return "~/"; }
        public static string InputText()    { return "~/harness/inputText/"; }
        public static string InputDate()    { return "~/harness/inputDate/"; }
    }

    public class HarnessController : Controller
    {
        public ActionResult Index()     { return View(); }
        public ActionResult InputText() { return View(); }
        public ActionResult InputDate() { return View(); }
    }
}