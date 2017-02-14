using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrantTests : DomainTest
    {
        [Test]
        public void Start_Validation()
        {
            ShouldBeValid(m => { });
            ShouldBeValid(m => m.Title = null);
            ShouldBeValid(m => m.OtherNames = null);

            ShouldBeInvalid(m => m.FirstName = null);
            ShouldBeInvalid(m => m.SurnameOrFamilyName = null);
            ShouldBeInvalid(m => m.DateOfBirth = null);
            ShouldBeInvalid(m => m.NationalInsuranceNumberText = null);
            ShouldBeInvalid(m => m.CurrentAddress.Street1 = null);
            ShouldBeInvalid(m => m.CurrentAddress.Street2 = null);
            ShouldBeInvalid(m => m.CurrentAddress.TownOrCity = null);
            ShouldBeInvalid(m => m.CurrentAddress.Postcode = null);
            ShouldBeInvalid(m => m.CurrentAddress.DateMovedIn = null);
            ShouldBeInvalid(m => m.CurrentAddressStatus = null);
            ShouldBeInvalid(m => m.ContactPreference = null);
        }

        #region test helpers

        protected void ShouldBeValid(Action<AboutYou> mutator)
        {
            var aboutYou = AboutYouBuilder.NewValidAboutYou(mutator);
            Assert.DoesNotThrow(() => BestStartGrant.Start(aboutYou), mutator.ToString());
        }

        protected void ShouldBeInvalid(Action<AboutYou> mutator)
        {
            var aboutYou = AboutYouBuilder.NewValidAboutYou(mutator);
            Assert.Throws<DomainException>(() => BestStartGrant.Start(aboutYou), mutator.ToString());
        }

        #endregion
    }
}
