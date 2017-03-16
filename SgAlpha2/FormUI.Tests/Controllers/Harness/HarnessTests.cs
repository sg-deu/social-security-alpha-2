using System;
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
        public void ShowHideParts_GET()
        {
            VerifyView(HarnessActions.ShowHideParts());
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
        public void ShowHide()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HarnessActions.ShowHideParts());
                var form = response.Form<HarnessModel>(1);

                form.WhenCheckedShows(m => m.CheckBox1, "hidden-part");
                form.WhenUncheckedShows(m => m.CheckBox1, "shown-part");
            });
        }
    }
}
