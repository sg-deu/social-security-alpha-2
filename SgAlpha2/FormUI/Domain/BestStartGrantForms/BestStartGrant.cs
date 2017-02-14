﻿using System;
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
            ctx.Required(m => m.SurnameOrFamilyName, "Please supply a Surname or family name");
            ctx.Required(m => m.DateOfBirth, "Please supply a Date of Birth");
            ctx.Required(m => m.NationalInsuranceNumber, "National Insurance number");
            ctx.Required(m => m.CurrentAddress.Street1, "Please supply an Address Street");
            ctx.Required(m => m.CurrentAddress.TownOrCity, "Please supply a Town or City");
            ctx.Required(m => m.CurrentAddress.Postcode, "Please supply a Postcode");
            ctx.Required(m => m.CurrentAddress.DateMovedIn, "Please supply the Date You or your Partner moved into this address");
            ctx.Required(m => m.CurrentAddressStatus, "Please indicate if this address is Permanent or Temporary");
            ctx.Required(m => m.ContactPreference, "Please supply a contact preference");

            if (aboutYou.ContactPreference.HasValue)
                switch(aboutYou.ContactPreference.Value)
                {
                    case ContactPreference.Email:
                        ctx.Required(m => m.EmailAddress, "Please supply an Email address");
                        break;

                    case ContactPreference.Phone:
                        ctx.Required(m => m.PhoneNumer, "Please supply a Phone number");
                        break;

                    case ContactPreference.Text:
                        ctx.Required(m => m.MobilePhoneNumber, "Please supply a Mobile phone number");
                        break;

                    default:
                        throw new Exception("Unhandled contact preference: " + aboutYou.ContactPreference);
                }

            ctx.ThrowIfError();
        }
    }
}