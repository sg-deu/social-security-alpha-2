using System.Web.Mvc;
using Newtonsoft.Json;

namespace FormUI.Controllers.Harness
{
    public static class HarnessActions
    {
        public static string Index()        { return "~/harness/"; }
        public static string Layout()       { return "~/harness/layout"; }
        public static string InputText()    { return "~/harness/inputText/"; }
        public static string InputDate()    { return "~/harness/inputDate/"; }
        public static string Radio()        { return "~/harness/radio/"; }
        public static string Form()         { return "~/harness/form/"; }
    }

    public class HarnessController : Controller
    {
        public ActionResult Index()     { return View(); }
        public ActionResult Layout()    { return View(); }
        public ActionResult InputText() { return View(); }
        public ActionResult InputDate() { return View(); }
        public ActionResult Radio()     { return View(); }

        [HttpGet]
        public ActionResult Form()
        {
            var model = new HarnessModel
            {
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Form(HarnessModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var json = JsonConvert.SerializeObject(model);
            return Content(json);
        }
    }
}