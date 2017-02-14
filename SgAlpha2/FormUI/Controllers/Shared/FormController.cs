using System;
using System.Web.Mvc;
using FormUI.Domain.Util;

namespace FormUI.Controllers.Shared
{
    public class FormController : Controller
    {
        public static Func<Func<object>, object> Executor = domainFunc =>
        {
            using (var repository = Repository.New())
            {
                try
                {
                    DomainRegistry.Repository = repository;
                    return domainFunc();
                }
                finally
                {
                    DomainRegistry.Repository = null;
                }
            }
        };

        protected ActionResult Exec(Action domainAction, Func<ActionResult> success, Func<ActionResult> failure)
        {
            if (!ModelState.IsValid)
                return failure();

            var result = Executor(() => { domainAction(); return null; });
            return success();
        }
    }
}