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
        public void Form()
        {
            WebAppTest(client =>
            {
                var form = client.Get(HarnessActions.Form()).Form<HarnessModel>(1);

                var response = form
                    .SetText(m => m.Text1, "Value 1")
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                var responseJson = response.Text;
                responseJson.Should().NotBeNullOrWhiteSpace("model should be returned as JSON");

                var boundModel = JsonConvert.DeserializeObject<HarnessModel>(responseJson);

                boundModel.Text1.Should().Be("Value 1");
            });
        }
    }
}
