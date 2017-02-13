﻿using System;
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

        public static Func<AboutYou, bool> Start;

        private static bool ExecuteStart(AboutYou aboutYou)
        {
            return BestStartGrant.Start(aboutYou);
        }
    }
}