using FluentAssertions;
using FormUI.Domain;
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
            if (FeatureToggles.WorkingOnOptions(Sections.Options))
                Assert.Pass("awaiting removal of feature toggle");

            var form = new ChangeOfCircsBuilder("form")
                .With(f => f.Options, OptionsBuilder.NewValid())
                .Value();

            form.Options.ChangePersonalDetails = false;

            Navigation.RequiresApplicantDetails(form).Should().BeFalse();

            form.Options.ChangePersonalDetails = true;

            Navigation.RequiresApplicantDetails(form).Should().BeTrue();
        }
    }
}