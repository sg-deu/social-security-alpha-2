using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgUKVerifyTests : BsgSectionTest
    {
        //[Test]
        //public void UKVerify_GET_PopulatesExistingDetails()
        //{
        //    WebAppTest(client =>
        //    {
        //        var detail = NewBsgDetail("form123");
        //        ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

        //        var response = client.Get(BsgActions.UKVerify(detail.Id));

        //        ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.UKVerify });
        //        response.Doc.Form<UKVerify>(1).GetConfirm(m => m.AgreedToConsent).Should().Be(detail.UKVerify.AgreedToConsent);
        //    });
        //}

        //[Test]
        //public void UKVerify_POST_PopulatesConsent()
        //{
        //    WebAppTest(client =>
        //    {
        //        var response = client.Get(BsgActions.Consent("form123")).Form<UKVerify>(1)
        //            .SelectConfirm(m => m.AgreedToConsent, true)
        //            .Submit(client);

        //        ExecutorStub.Executed<AddUKVerify>(0).ShouldBeEquivalentTo(new AddUKVerify
        //        {
        //            FormId = "form123",
        //            UKVerify = new UKVerify { AgreedToConsent = true },
        //        });

        //        response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
        //    });
        //}

        [Test]
        public void UKVerify_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddUKVerify, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.UKVerify("form123")).Form<UKVerify>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
