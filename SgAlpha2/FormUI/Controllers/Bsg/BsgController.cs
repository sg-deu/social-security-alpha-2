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
        public static string    ApplicantBenefits1(string formId)   { return $"~/bsg/applicantBenefits1/{formId}"; }
        public static string    ApplicantBenefits2(string formId)   { return $"~/bsg/applicantBenefits2/{formId}"; }
        public static string    HealthProfessional(string formId)   { return $"~/bsg/healthProfessional/{formId}"; }
        public static string    PaymentDetails(string formId)       { return $"~/bsg/paymentDetails/{formId}"; }
        public static string    Declaration(string formId)          { return $"~/bsg/declaration/{formId}"; }
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
                success: () => Redirect(BsgActions.ExistingChildren(id)),
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
                success: () => Redirect(BsgActions.ApplicantBenefits1(id)),
                failure: () => View(existingChildren));
        }

        [HttpGet]
        public ActionResult ApplicantBenefits1(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApplicantBenefits1(string id, ApplicantBenefits applicantBenefits)
        {
            var cmd = new AddApplicantBenefits
            {
                FormId = id,
                Part = Part.Part1,
                ApplicantBenefits = applicantBenefits,
            };

            return Exec(cmd,
                success: () => Redirect(BsgActions.ApplicantBenefits2(id)),
                failure: () => ApplicantBenefits1(id));
        }

        [HttpGet]
        public ActionResult ApplicantBenefits2(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApplicantBenefits2(string id, ApplicantBenefits applicantBenefits)
        {
            var cmd = new AddApplicantBenefits
            {
                FormId = id,
                Part = Part.Part2,
                ApplicantBenefits = applicantBenefits,
            };

            return Exec(cmd,
                success: () => Redirect(BsgActions.HealthProfessional(id)),
                failure: () => ApplicantBenefits2(id));
        }

        [HttpGet]
        public ActionResult HealthProfessional(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult HealthProfessional(string id, HealthProfessional healthProfessional)
        {
            var cmd = new AddHealthProfessional
            {
                FormId = id,
                HealthProfessional = healthProfessional,
            };

            return Exec(cmd,
                success: () => Redirect(BsgActions.PaymentDetails(id)),
                failure: () => HealthProfessional(id));
        }

        [HttpGet]
        public ActionResult PaymentDetails(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult PaymentDetails(string id, PaymentDetails paymentDetails)
        {
            var cmd = new AddPaymentDetails
            {
                FormId = id,
                PaymentDetails = paymentDetails,
            };

            return Exec(cmd,
                success: () => Redirect(BsgActions.Declaration(id)),
                failure: () => PaymentDetails(id));
        }

        [HttpGet]
        public ActionResult Declaration(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Declaration(string id, Declaration declaration)
        {
            var cmd = new Complete
            {
                FormId = id,
                Declaration = declaration,
            };

            return Exec(cmd,
                success: () => Redirect(BsgActions.Complete()),
                failure: () => Declaration(id));
        }

        [HttpGet]
        public ActionResult Complete()
        {
            return View();
        }
    }
}