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
            ShouldBeValid(m => { }, "AboutYouBuilder.NewValidAboutYou should pass validation");
            ShouldBeValid(m => m.Title = null, "Title should be optional");
            ShouldBeValid(m => m.OtherNames = null, "OtherNames should be optional");

            ShouldBeInvalid(m => m.FirstName = null, "FirstName should be required");
        }

        protected void ShouldBeValid(Action<AboutYou> mutator, string message)
        {
            var aboutYou = AboutYouBuilder.NewValidAboutYou(mutator);
            Assert.DoesNotThrow(() => BestStartGrant.Start(aboutYou), message);
        }

        protected void ShouldBeInvalid(Action<AboutYou> mutator, string message)
        {
            var aboutYou = AboutYouBuilder.NewValidAboutYou(mutator);
            Assert.Throws<DomainException>(() => BestStartGrant.Start(aboutYou), message);
        }
    }
}
