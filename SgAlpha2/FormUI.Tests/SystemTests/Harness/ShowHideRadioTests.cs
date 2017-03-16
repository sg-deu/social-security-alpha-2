using FluentAssertions;
using FormUI.Controllers.Harness;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;
using OpenQA.Selenium;

namespace FormUI.Tests.SystemTests.Harness
{
    [TestFixture]
    public class ShowHideRadioTests : SystemTest
    {
        [Test]
        public void ShowHideRadio()
        {
            App.GoTo(HarnessActions.ShowHideRadio());
            App.VerifyCanSeeText("Show/Hide for radio");

            App.FindElement("Verify part1-yes is hidden", By.Id("part1-yes"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify part1-no is hidden", By.Id("part1-no"), e => e.Displayed.Should().BeFalse());

            App.FindElement("Verify part2-yes is visible", By.Id("part2-yes"), e => e.Displayed.Should().BeTrue());
            App.FindElement("Verify part2-no is hidden", By.Id("part2-no"), e => e.Displayed.Should().BeFalse());

            App.FindElement("Verify part3-yes is hidden", By.Id("part3-yes"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify part3-no is hidden", By.Id("part3-no"), e => e.Displayed.Should().BeTrue());

            var form = App.FormForModel<HarnessModel>();
            form.SelectRadio(m => m.Radio4, true);

            App.FindElement("Verify part1-yes is hidden", By.Id("part1-yes"), e => e.Displayed.Should().BeTrue());
            App.FindElement("Verify part1-no is hidden", By.Id("part1-no"), e => e.Displayed.Should().BeFalse());

            form.SelectRadio(m => m.Radio4, false);

            App.FindElement("Verify part1-yes is hidden", By.Id("part1-yes"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify part1-no is hidden", By.Id("part1-no"), e => e.Displayed.Should().BeTrue());

            App.FindElement("Verify part2-yes is visible", By.Id("part2-yes"), e => e.Displayed.Should().BeTrue());
            App.FindElement("Verify part2-no is hidden", By.Id("part2-no"), e => e.Displayed.Should().BeFalse());

            App.FindElement("Verify part3-yes is hidden", By.Id("part3-yes"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify part3-no is hidden", By.Id("part3-no"), e => e.Displayed.Should().BeTrue());

            App.Submit();

            App.FindElement("Verify part2-yes is visible", By.Id("part2-yes"), e => e.Displayed.Should().BeTrue());
            App.FindElement("Verify part2-no is hidden", By.Id("part2-no"), e => e.Displayed.Should().BeFalse());

            App.FindElement("Verify part3-yes is hidden", By.Id("part3-yes"), e => e.Displayed.Should().BeFalse());
            App.FindElement("Verify part3-no is hidden", By.Id("part3-no"), e => e.Displayed.Should().BeTrue());
        }
    }

}
