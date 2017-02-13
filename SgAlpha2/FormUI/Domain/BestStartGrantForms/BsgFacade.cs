using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Domain.BestStartGrantForms
{
    public static class BsgFacade
    {
        static BsgFacade()
        {
            Init();
        }

        public static void Init()
        {
            Start = ExecuteStart;
        }

        public static Action<AboutYou> Start;

        private static void ExecuteStart(AboutYou aboutYou)
        {
            BestStartGrant.Start(aboutYou);
        }
    }
}