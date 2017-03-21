using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FormUI.Controllers.Coc;
using FormUI.Domain;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.Util;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Coc
{
    [TestFixture]
    public class CocSystemTests : SystemTest
    {
        private bool _runningAlltests;
        private IList<Sections> _verifiedSections = new List<Sections>();

        [TestFixtureTearDown]
        protected void TestFixtureTearDown()
        {
            base.TearDown();

            if (TestRegistry.TestHasFailed)
                return;

            if (!_runningAlltests)
                return;

            // verify each section has been tested
            foreach (Sections section in Enum.GetValues(typeof(Sections)))
                if (!FeatureToggles.SkipWorkInProgressSection(section))
                    _verifiedSections.Should().Contain(section, "section {0} should be filled in and verified", section);
        }

        [Test]
        public void RunningAllTests()
        {
            // this is to verify in the TestFixtureTearDown that we've verified all sections of the form
            _runningAlltests = true;
        }

        [Test]
        public void CompleteApplication()
        {
            var userId = "existing.user@site.com";

            Db(r =>
            {
                new BestStartGrantBuilder("existingForm")
                    .PreviousApplicationFor(userId)
                    .Insert(f => f.ApplicantDetails.FirstName = "system test first name");
            });

            App.GoTo(FormUI.Controllers.Home.HomeActions.Index());
            App.VerifyCanSeeText("Choose");

            App.Submit();

            FillInConsent();
            App.Submit();

            FillInIdentity(userId);
            App.Submit();

            App.VerifyCanSeeText("Thank you");

            Db(r =>
            {
                var doc = r.Query<ChangeOfCircs>().ToList().Single();

                VerifyConsent(doc);
                VerifyIdentity(doc, userId);
            });
        }

        private void FillInConsent()
        {
            var form = App.FormForModel<Consent>();
            form.Check(m => m.AgreedToConsent, true);
        }

        private void VerifyConsent(ChangeOfCircs doc)
        {
            doc.Should().NotBeNull();
            _verifiedSections.Add(Sections.Consent);
        }

        private void FillInIdentity(string userId)
        {
            var form = App.FormForModel<IdentityModel>();
            form.TypeText(m => m.Email, userId);
        }

        private void VerifyIdentity(ChangeOfCircs doc, string userId)
        {
            doc.UserId.Should().Be(userId);
            _verifiedSections.Add(Sections.Identity);
        }
    }
}
