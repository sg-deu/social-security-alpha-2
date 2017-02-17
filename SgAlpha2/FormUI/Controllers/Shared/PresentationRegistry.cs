using System;
using FormUI.Domain.Util.Facade;

namespace FormUI.Controllers.Shared
{
    public class PresentationRegistry
    {
        public static Func<CqExecutor> NewExecutor;
    }
}