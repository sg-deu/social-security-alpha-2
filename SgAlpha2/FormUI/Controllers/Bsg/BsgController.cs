﻿using System.Web.Mvc;
using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public static class BsgActions
    {
        public static string    Overview()                          { return "~/bsg/overview"; }
        public static string    AboutYou()                          { return "~/bsg/aboutYou"; }
        public static string    ExpectedChildren(string formId)     { return $"~/bsg/expectedChildren/{formId}"; }
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
        public ActionResult Complete()
        {
            return View();
        }
    }
}