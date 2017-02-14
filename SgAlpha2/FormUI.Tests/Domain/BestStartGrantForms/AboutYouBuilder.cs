using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class AboutYouBuilder
    {
        public static AboutYou NewValidAboutYou(Action<AboutYou> mutator = null)
        {
            var aboutYou = new AboutYou
            {
                FirstName = "test first name",
            };

            if (mutator != null)
                mutator(aboutYou);

            return aboutYou;
        }
    }
}
