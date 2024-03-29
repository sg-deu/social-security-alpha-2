﻿using System;
using System.Net;
using FluentAssertions;
using FormUI.Controllers.Harness;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Harness
{
    [TestFixture]
    public class HarnessTests : WebTest
    {
        [Test]
        public void Index_GET()
        {
            VerifyView(HarnessActions.Index());
        }

        [Test]
        public void Layout_GET()
        {
            VerifyView(HarnessActions.Layout());
        }

        [Test]
        public void InputText_GET()
        {
            VerifyView(HarnessActions.InputText());
        }

        [Test]
        public void TextArea_GET()
        {
            VerifyView(HarnessActions.TextArea());
        }

        [Test]
        public void InputDate_GET()
        {
            VerifyView(HarnessActions.InputDate());
        }

        [Test]
        public void Radio_GET()
        {
            VerifyView(HarnessActions.Radio());
        }

        [Test]
        public void CheckBoxes_GET()
        {
            VerifyView(HarnessActions.CheckBoxes());
        }

        [Test]
        public void Form()
        {
            WebAppTest(client =>
            {
                var form = client.Get(HarnessActions.Form()).Form<HarnessModel>(1);

                var response = form
                    .SetText(m => m.Text1, "Value 1")
                    .SetDate(m => m.DateTime1, "03", "02", "2001")
                    .SetText(m => m.Int1, "01")
                    .SetText(m => m.Radio1, RValues1.Value2.ToString())
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                var responseJson = response.Text;
                responseJson.Should().NotBeNullOrWhiteSpace("model should be returned as JSON");

                var boundModel = JsonConvert.DeserializeObject<HarnessModel>(responseJson);

                boundModel.Text1.Should().Be("Value 1");
                boundModel.DateTime1.Should().Be(new DateTime(2001, 02, 03));
                boundModel.Int1.Should().Be(1);
                boundModel.Radio1.Should().Be(RValues1.Value2);
            });
        }

        [Test]
        public void AjaxDateTime()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HarnessActions.AjaxForm());
                var form = response.Form<AjaxFormModel>(1);

                var ajaxActions = form
                    .SetDate(f => f.Date, "", "", "")
                    .OnChange(f => f.Date, client);

                ajaxActions.Length.Should().Be(2);

                ajaxActions.ForFormGroup<AjaxFormModel>(f => f.String1).ShouldShowHide(response.Doc, false);
                ajaxActions.ForFormGroup<AjaxFormModel>(f => f.String2).ShouldShowHide(response.Doc, false);
            });
        }

        [Test]
        public void ShowHideCheckbox()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HarnessActions.ShowHideCheckbox());
                var form = response.Form<HarnessModel>(1);

                form.WhenCheckedShows(m => m.CheckBox1, "hidden-part");
                form.WhenUncheckedShows(m => m.CheckBox1, "shown-part");

                form.WhenCheckedShows(m => m.CheckBox2, "hidden-part2");
                form.WhenUncheckedShows(m => m.CheckBox2, "shown-part2");
            });
        }

        [Test]
        public void ShowHideRadio()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HarnessActions.ShowHideRadio());
                var form = response.Form<HarnessModel>(1);

                form.RadioShows(m => m.Radio4, true, "part1-yes");
                form.RadioShows(m => m.Radio4, false, "part1-no");

                form.RadioShows(m => m.Radio5, true, "part2-yes");
                form.RadioShows(m => m.Radio5, false, "part2-no");

                form.RadioShows(m => m.Radio6, true, "part3-yes");
                form.RadioShows(m => m.Radio6, false, "part3-no");
            });
        }
    }
}
