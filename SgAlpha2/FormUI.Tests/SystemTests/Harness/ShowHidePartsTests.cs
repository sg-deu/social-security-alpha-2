using FluentAssertions;
using FormUI.Controllers.Harness;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;
using OpenQA.Selenium;

namespace FormUI.Tests.SystemTests.Harness
{
    [TestFixture]
    public class ShowHidePartsTests : SystemTest
    {
        [Test]
        public void ShowHideParts()
        {
            App.GoTo(HarnessActions.ShowHideParts());
            App.VerifyCanSeeText("Show/Hide parts");

            App.FindElement("Verify hidden part is hidden", By.Id("hidden-part"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify shown part is shown", By.Id("shown-part"), e => e.Displayed.Should().BeTrue());

            App.FindElement("Verify hidden part 2 is hidden", By.Id("hidden-part2"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify shown part 2 is shown", By.Id("shown-part2"), e => e.Displayed.Should().BeTrue());

            var form = App.FormForModel<HarnessModel>();
            form.Check(m => m.CheckBox1, true);

            App.FindElement("Verify hidden part is now shown", By.Id("hidden-part"), e => e.Displayed.Should().BeTrue());
            App.FindElement("Verify shown part is now hidden", By.Id("shown-part"), e => e.Displayed.Should().BeFalse());

            App.FindElement("Verify hidden part 2 is still hidden", By.Id("hidden-part2"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify shown part 2 is still shown", By.Id("shown-part2"), e => e.Displayed.Should().BeTrue());

            App.Submit();

            App.FindElement("Verify hidden part is now shown", By.Id("hidden-part"), e => e.Displayed.Should().BeTrue());
            App.FindElement("Verify shown part is now hidden", By.Id("shown-part"), e => e.Displayed.Should().BeFalse());

            App.FindElement("Verify hidden part 2 is still hidden", By.Id("hidden-part2"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify shown part 2 is still shown", By.Id("shown-part2"), e => e.Displayed.Should().BeTrue());
        }
    }
}
