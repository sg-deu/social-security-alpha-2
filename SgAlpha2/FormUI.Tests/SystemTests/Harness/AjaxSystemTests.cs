using System;
using FluentAssertions;
using FormUI.Controllers.Harness;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;
using OpenQA.Selenium;

namespace FormUI.Tests.SystemTests.Harness
{
    [TestFixture]
    public class AjaxSystemTests : SystemTest
    {
        [Test]
        public void ShowHide()
        {
            App.GoTo(HarnessActions.AjaxForm());
            App.VerifyCanSeeText("Ajax form");

            var form = App.FormForModel<AjaxFormModel>();
            var string1Selector = form.CssSelectFormGroup(f => f.String1);
            var string2Selector = form.CssSelectFormGroup(f => f.String2);

            App.FindElement("verify String1 not visible", By.CssSelector(string1Selector), e => e.Displayed.Should().BeFalse());
            App.FindElement("verify String2 not visible", By.CssSelector(string2Selector), e => e.Displayed.Should().BeFalse());

            form.TypeDate(f => f.Date, DateTime.Now.Date);
            form.BlurDate(f => f.Date);

            App.FindElement("verify String1 is now visible", By.CssSelector(string1Selector), e => e.Displayed.Should().BeTrue());
            App.FindElement("verify String2 not visible", By.CssSelector(string2Selector), e => e.Displayed.Should().BeFalse());

            form.TypeDate(f => f.Date, DateTime.Now.Date - TimeSpan.FromDays(1));
            form.BlurDate(f => f.Date);

            App.FindElement("verify String1 is now visible", By.CssSelector(string1Selector), e => e.Displayed.Should().BeTrue());
            App.FindElement("verify String2 is now visible", By.CssSelector(string2Selector), e => e.Displayed.Should().BeTrue());

            form.TypeText(f => f.String1, "string 1 test");
            form.TypeText(f => f.String2, "string 2 test");

            App.Submit();

            form.GetText("verify String1 is 'string 1 test'", f => f.String1, v => v.Should().Be("string 1 test"));
        }
    }

}
