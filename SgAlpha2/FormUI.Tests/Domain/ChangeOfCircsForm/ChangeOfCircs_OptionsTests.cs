using System;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_OptionsTests : DomainTest
    {
        [Test]
        public void Options_Validation()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            OptionsShouldBeValid(form, m => { });
            OptionsShouldBeValid(form, m => { m.Other = false; m.OtherDetails = null; });

            OptionsShouldBeInvalid(form, m => { m.Other = true; m.OtherDetails = null; });

            // until the following sections are implemented, they are invalid selections
            OptionsShouldBeInvalid(form, m => m.ChangePartnerDetails = true);
            OptionsShouldBeInvalid(form, m => m.ChangeChildrenDetails = true);
            OptionsShouldBeInvalid(form, m => m.ChangePaymentDetails = true);
        }

        protected void OptionsShouldBeValid(ChangeOfCircs form, Action<Options> mutator)
        {
            ShouldBeValid(() => form.AddOptions(OptionsBuilder.NewValid(mutator)));
        }

        protected void OptionsShouldBeInvalid(ChangeOfCircs form, Action<Options> mutator)
        {
            ShouldBeInvalid(() => form.AddOptions(OptionsBuilder.NewValid(mutator)));
        }
    }
}
