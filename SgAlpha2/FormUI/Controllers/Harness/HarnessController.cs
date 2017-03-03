using System;
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
        public static string CheckBoxes()   { return "~/harness/checkBoxes/"; }
        public static string Form()         { return "~/harness/form/"; }
        public static string AjaxForm()     { return "~/harness/ajaxForm"; }
    }

    public class HarnessController : Controller
    {
        public ActionResult Index()         { return View(); }
        public ActionResult Layout()        { return View(); }
        public ActionResult InputText()     { return View(); }
        public ActionResult InputDate()     { return View(); }
        public ActionResult Radio()         { return View(); }
        public ActionResult CheckBoxes()    { return View(); }


        [HttpGet]
        public ActionResult Form()
        {
            var model = new HarnessModel
            {
                Text1 = "initial value",
                DateTime1 = new DateTime(2003, 02, 01),
                Int1 = 5,
                Radio1 = RValues1.Value2,
                CheckBox2 = true,
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

        [HttpGet]
        public ActionResult AjaxForm()
        {
            return View();
        }
    }
}