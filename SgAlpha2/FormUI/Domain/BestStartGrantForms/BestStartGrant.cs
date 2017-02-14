using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Forms;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms
{
    public class BestStartGrant : Form
    {
        public BestStartGrant() : base(Guid.NewGuid().ToString())
        {
        }

        public AboutYou AboutYou { get; set; }

        public static bool Start(AboutYou aboutYou)
        {
            Validate(aboutYou);

            var form = new BestStartGrant
            {
                AboutYou = aboutYou,
            };

            Repository.Insert(form);

            return true;
        }

        private static void Validate(AboutYou aboutYou)
        {
            var ctx = new ValidationContext<AboutYou>(aboutYou);

            ctx.Required(m => m.FirstName, "Please supply a First name");

            ctx.ThrowIfError();
        }
    }
}