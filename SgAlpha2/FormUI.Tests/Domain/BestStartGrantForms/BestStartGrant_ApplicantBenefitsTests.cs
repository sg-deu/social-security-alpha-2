﻿using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_ApplicantBenefitsTests : DomainTest
    {
        [Test]
        public void AddApplicantBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantBenefitsShouldBeValid(form, m => { });

            ApplicantBenefitsShouldBeInvalid(form, m => m.SetEmpty());
            ApplicantBenefitsShouldBeInvalid(form, m => m.SetEmpty(b => { b.Unknown = true; b.HasIncomeSupport = true; }));
            ApplicantBenefitsShouldBeInvalid(form, m => m.SetEmpty(b => { b.HasIncomeSupport = true; b.None = true; }));
            ApplicantBenefitsShouldBeInvalid(form, m => m.SetEmpty(b => { b.None = true; b.Unknown = true; }));
        }

        protected void ApplicantBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddApplicantBenefits(BenefitsBuilder.NewWithBenefit(mutator)));
        }

        protected void ApplicantBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddApplicantBenefits(BenefitsBuilder.NewWithBenefit(mutator)));
        }
    }
}
