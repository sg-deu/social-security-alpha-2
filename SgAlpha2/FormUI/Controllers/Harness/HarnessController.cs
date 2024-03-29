﻿using System;
using System.Web.Mvc;
using FormUI.Controllers.Helpers;
using FormUI.Controllers.Shared;
using Newtonsoft.Json;

namespace FormUI.Controllers.Harness
{
    public static class HarnessActions
    {
        public static string Index()            { return "~/harness/"; }
        public static string Layout()           { return "~/harness/layout"; }
        public static string InputText()        { return "~/harness/inputText/"; }
        public static string TextArea()         { return "~/harness/textArea/"; }
        public static string InputDate()        { return "~/harness/inputDate/"; }
        public static string Radio()            { return "~/harness/radio/"; }
        public static string CheckBoxes()       { return "~/harness/checkBoxes/"; }
        public static string Form()             { return "~/harness/form/"; }
        public static string AjaxForm()         { return "~/harness/ajaxForm"; }
        public static string AjaxPostback()     { return "~/harness/ajaxPostback"; }
        public static string ShowHideCheckbox() { return "~/harness/showHideCheckbox/"; }
        public static string ShowHideRadio()    { return "~/harness/showHideRadio/"; }
    }

    public class HarnessController : FormController
    {
        public ActionResult Index()                         { return View(); }
        public ActionResult Layout()                        { return View(); }
        public ActionResult InputText()                     { return View(new HarnessModel { Text4 = "disabled value" }); }
        public ActionResult TextArea(HarnessModel model)    { return View(); }
        public ActionResult InputDate()                     { return View(); }
        public ActionResult Radio()                         { return View(); }
        public ActionResult CheckBoxes()                    { return View(); }

        public static string UnitTestActionFor(Domain.BestStartGrantForms.Sections section)
        {
            try { return Bsg.SectionActionStrategy.For(section).Action("unitTest"); }
            catch { return "javascript:alert('SectionActionStrategy not implemented')"; }
        }

        public static string UnitTestActionFor(Domain.ChangeOfCircsForm.Sections section)
        {
            try { return Coc.SectionActionStrategy.For(section).Action("unitTest"); }
            catch { return "javascript:alert('SectionActionStrategy not implemented')"; }
        }

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

        public ActionResult AjaxForm(AjaxFormModel model)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AjaxPostback(AjaxFormModel model)
        {
            var showString1 = false;
            var showString2 = false;

            if (model.Date.HasValue)
            {
                var modelDate = model.Date.Value.Date;
                var today = DateTime.Now.Date;
                var yesterday = today - TimeSpan.FromDays(1);

                showString1 = modelDate == today || modelDate == yesterday;
                showString2 = modelDate == yesterday;
            }

            return AjaxActions(new []
            {
                AjaxAction.ShowHideFormGroup<AjaxFormModel>(m => m.String1, showString1),
                AjaxAction.ShowHideFormGroup<AjaxFormModel>(m => m.String2, showString2),
            });
        }

        public ActionResult ShowHideCheckbox(HarnessModel model)
        {
            return View(Request.HttpMethod == "GET" ? new HarnessModel { CheckBox2 = true } : model);
        }

        public ActionResult ShowHideRadio(HarnessModel model)
        {
            return View(Request.HttpMethod == "GET" ? new HarnessModel { Radio5 = true, Radio6 = false } : model);
        }
    }
}