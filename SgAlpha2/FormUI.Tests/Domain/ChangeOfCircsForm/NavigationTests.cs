using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class NavigationTests : DomainTest
    {
        [Test]
        public void RequiresApplicantDetails()
        {
            var form = new ChangeOfCircsBuilder("form")
                .With(f => f.Options, OptionsBuilder.NewValid())
                .Value();

            form.Options.ChangePersonalDetails = false;

            Navigation.RequiresApplicantDetails(form).Should().BeFalse();

            form.Options.ChangePersonalDetails = true;

            Navigation.RequiresApplicantDetails(form).Should().BeTrue();
        }

        [Test]
        public void RequiresPaymentDetails()
        {
            var form = new ChangeOfCircsBuilder("form")
                .With(f => f.Options, OptionsBuilder.NewValid())
                .Value();

            form.Options.ChangePaymentDetails = false;

            Navigation.RequiresPaymentDetails(form).Should().BeFalse();

            form.Options.ChangePaymentDetails = true;

            Navigation.RequiresPaymentDetails(form).Should().BeTrue();
        }

        [Test]
        public void RequiresExpectedChildren()
        {
            var form = new ChangeOfCircsBuilder("form")
                .With(f => f.Options, OptionsBuilder.NewValid())
                .Value();

            form.Options.AddExpectedBaby = false;

            Navigation.RequiresExpectedChildren(form).Should().BeFalse();

            form.Options.AddExpectedBaby = true;

            Navigation.RequiresExpectedChildren(form).Should().BeTrue();
        }

        [Test]
        public void RequiresHealthProfessional()
        {
            var form = new ChangeOfCircsBuilder("form")
                .With(f => f.Options, OptionsBuilder.NewValid())
                .Value();

            form.Options.AddExpectedBaby = false;

            Navigation.RequiresHealthProfessional(form).Should().BeFalse();

            form.Options.AddExpectedBaby = true;

            Navigation.RequiresHealthProfessional(form).Should().BeTrue();
        }
    }
}