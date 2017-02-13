using System;
using System.Web.Mvc;
using FormUI.Controllers.Shared;
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
            return Exec(() => BsgFacade.Start(aboutYou),
                success: () => Redirect(BsgActions.Complete()),
                failure: () => { throw new NotImplementedException("failure not tested yet"); });
        }

        [HttpGet]
        public ActionResult Complete()
        {
            return View();
        }
    }
}