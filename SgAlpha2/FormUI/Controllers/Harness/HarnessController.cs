using System.Web.Mvc;
using Newtonsoft.Json;

namespace FormUI.Controllers.Harness
{
    public static class HarnessActions
    {
        public static string Index()        { return "~/harness/"; }
        public static string InputText()    { return "~/harness/inputText/"; }
        public static string InputDate()    { return "~/harness/inputDate/"; }
        public static string Form()         { return "~/harness/form/"; }
    }

    public class HarnessController : Controller
    {
        public ActionResult Index()     { return View(); }
        public ActionResult InputText() { return View(); }
        public ActionResult InputDate() { return View(); }

        [HttpGet]
        public ActionResult Form() { return View(); }

        [HttpPost]
        public ActionResult Form(HarnessModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            return Content(json);
        }
    }
}