﻿using System.Web.Mvc;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public static class BsgActions
    {
        public static string    Overview()  { return "~/bsg/overview"; }
        public static string    AboutYou()  { return "~/bsg/aboutYou"; }
        public static string    Complete()  { return "~/bsg/complete"; }
    }

    public class BsgController : Controller
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
            BsgFacade.Start(aboutYou);
            return Redirect(BsgActions.Complete());
        }

        [HttpGet]
        public ActionResult Complete()
        {
            return View();
        }
    }
}