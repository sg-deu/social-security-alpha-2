﻿using System;
using System.Collections.Generic;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Forms;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms
{
    public class BestStartGrant : Form
    {
        protected BestStartGrant() : base(Guid.NewGuid().ToString())
        {
        }

        public AboutYou             AboutYou            { get; protected set; }
        public ExpectedChildren     ExpectedChildren    { get; protected set; }
        public ExistingChildren     ExistingChildren    { get; protected set; }
        public HealthProfessional   HealthProfessional  { get; protected set; }

        public static string Start(AboutYou aboutYou)
        {
            Validate(aboutYou);

            var form = new BestStartGrant
            {
                AboutYou = aboutYou,
            };

            Repository.Insert(form);

            return form.Id;
        }

        public void AddExpectedChildren(ExpectedChildren expectedChildren)
        {
            Validate(expectedChildren);

            ExpectedChildren = expectedChildren;
            Repository.Update(this);
        }

        public void AddExistingChildren(ExistingChildren existingChildren)
        {
            Validate(existingChildren);

            ExistingChildren = existingChildren;
            Repository.Update(this);
        }

        public void AddHealthProfessional(HealthProfessional healthProfessional)
        {
            Validate(healthProfessional);

            HealthProfessional = healthProfessional;
            Repository.Update(this);
        }

        private static void Validate(AboutYou aboutYou)
        {
            var ctx = new ValidationContext<AboutYou>(aboutYou);

            ctx.Required(m => m.FirstName, "Please supply a First name");
            ctx.Required(m => m.SurnameOrFamilyName, "Please supply a Surname or family name");
            ctx.Required(m => m.DateOfBirth, "Please supply a Date of Birth");
            ctx.InPast(m => m.DateOfBirth, "Please supply a Date of Birth in the past");
            ctx.Custom(m => m.NationalInsuranceNumber, ni => ValidateNationalInsuranceNumber(aboutYou));
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

        private static string ValidateNationalInsuranceNumber(AboutYou aboutYou)
        {
            var ni = aboutYou.NationalInsuranceNumber;

            if (string.IsNullOrWhiteSpace(ni))
                return "Please supply a National Insurance number";

            ni = ni.Replace(" ", "").ToUpper();

            // if ni is "AB/123456/C"
            if (ni.Length > 2 && ni[2] == '/')
                ni = ni.Substring(0, 2) + ni.Substring(3, ni.Length - 3);

            // if ni is "AB123456/C"
            if (ni.Length > 8 && ni[8] == '/')
                ni = ni.Substring(0, 8) + ni.Substring(9, ni.Length - 9);

            const string invalidMessage = "Please supply a valid National Insurance number in the format 'AB 12 34 56 C'";

            if (ni.Length != 9)
                return invalidMessage;

            // ni should now be "AB123456C", so positions 0, 1, and 8 are letters, and the rest are numbers
            var letterPositions = new List<int> { 0, 1, 8 };
            for (var characterIndex = 0; characterIndex < ni.Length; characterIndex++)
            {
                if (letterPositions.Contains(characterIndex) && !char.IsLetter(ni[characterIndex]))
                    return invalidMessage;

                if (!letterPositions.Contains(characterIndex) && !char.IsNumber(ni[characterIndex]))
                    return invalidMessage;
            }

            // now we know it's valid, put spaces (back) in to format correctly
            ni = string.Format("{0} {1} {2} {3} {4}",
                ni.Substring(0, 2),     // {0} AB
                ni.Substring(2, 2),     // {1} 12
                ni.Substring(4, 2),     // {2} 34
                ni.Substring(6, 2),     // {3} 56
                ni.Substring(8, 1));    // {4} C

            aboutYou.NationalInsuranceNumber = ni;

            return null;
        }

        private static void Validate(ExpectedChildren expectedChildren)
        {
            var ctx = new ValidationContext<ExpectedChildren>(expectedChildren);

            ctx.InFuture(m => m.ExpectancyDate, "Please supply an expectancy date in the future");

            ctx.Custom(m => m.ExpectedBabyCount, babyCount =>
                babyCount.HasValue && (babyCount.Value < 1 || babyCount.Value > 10)
                    ? "Please supply a number of babies between 1 and 10"
                    : null);

            ctx.ThrowIfError();
        }

        private static void Validate(ExistingChildren existingChildren)
        {
            var ctx = new ValidationContext<ExistingChildren>(existingChildren);

            for (var i = 0; i < existingChildren.Children.Count; i ++)
            {
                ctx.Required(c => c.Children[i].FirstName, "Please supply a First name");
                ctx.Required(c => c.Children[i].Surname, "Please supply a Surname or family name");
                ctx.Required(c => c.Children[i].DateOfBirth, "Please supply a Date of Birth");
                ctx.InPast(c => c.Children[i].DateOfBirth, "Please supply a Date of Birth in the past");
                ctx.Required(c => c.Children[i].RelationshipToChild, "Please supply the relationship to the child");
                ctx.Required(c => c.Children[i].FormalKinshipCare, "Please indicate is their is formal kinship care");
            }

            ctx.ThrowIfError();
        }

        private static void Validate(HealthProfessional healthProfessional)
        {
            var ctx = new ValidationContext<HealthProfessional>(healthProfessional);

            ctx.Required(c => c.Pin, "Please supply a GMC No. or NMC pin");

            ctx.ThrowIfError();
        }
    }
}