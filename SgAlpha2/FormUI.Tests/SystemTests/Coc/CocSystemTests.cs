using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using FormUI.Controllers.Coc;
using FormUI.Domain;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.Util;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;
using OpenQA.Selenium;
using BsgForm = FormUI.Domain.BestStartGrantForms.BestStartGrant;

namespace FormUI.Tests.SystemTests.Coc
{
    [TestFixture]
    public class CocSystemTests : SystemTest
    {
        private bool            _runningAlltests;
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
            BsgForm bsg = null;

            Db(r =>
            {
                bsg =
                    new BestStartGrantBuilder("existingForm")
                        .PreviousApplicationFor(userId)
                        .Insert(f =>
                        {
                            f.ApplicantDetails.FirstName = "st_fn";
                        });
            });

            App.GoTo(FormUI.Controllers.Home.HomeActions.Index());
            App.VerifyCanSeeText("Choose");

            App.Submit();

            FillInConsent();
            App.Submit();

            FillInIdentity(userId);
            App.Submit();

            FillInOptions();
            App.Submit();

            FillInApplicantDetails(bsg);
            App.Submit();

            var filename = FillInEvidence();
            App.ClickButton("");

            App.VerifyCanSeeText("Thank you");

            Db(r =>
            {
                var doc = r.Query<ChangeOfCircs>().ToList().Single();

                VerifyConsent(doc);
                VerifyIdentity(doc, userId);
                VerifyOptions(doc);
                VerifyApplicantDetails(doc);
                VerifyEvidence(doc, filename);
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

        private void FillInOptions()
        {
            var form = App.FormForModel<Options>();
            form.Check(m => m.ChangePersonalDetails, true);
            form.Check(m => m.Other, true);
            form.TypeTextArea(m => m.OtherDetails, "system test details");
        }

        private void VerifyOptions(ChangeOfCircs doc)
        {
            doc.Options.ChangePersonalDetails.Should().BeTrue();
            doc.Options.Other.Should().BeTrue();
            doc.Options.OtherDetails.Should().Be("system test details");
            _verifiedSections.Add(Sections.Options);
        }

        private void FillInApplicantDetails(BsgForm bsg)
        {
            var form = App.FormForModel<ApplicantDetails>();

            form.GetText("Verify existing Title populated", m => m.Title, t => t.Should().NotBeNullOrWhiteSpace());
            form.GetText("Verify existing FullName", m => m.FullName, t => t.Should().NotBeNullOrWhiteSpace());
            form.GetText("Verify existing Address.Line1", m => m.Address.Line1, t => t.Should().NotBeNullOrWhiteSpace());
            form.GetText("Verify existing MobilePhoneNumber", m => m.MobilePhoneNumber, t => t.Should().NotBeNullOrWhiteSpace());
            form.GetText("Verify existing HomePhoneNumer", m => m.HomePhoneNumer, t => t.Should().NotBeNullOrWhiteSpace());
            form.GetText("Verify existing EmailAddress", m => m.EmailAddress, t => t.Should().NotBeNullOrWhiteSpace());

            form.TypeText(m => m.Title, "coc title");
            form.TypeText(m => m.FullName, "coc full name");
            form.TypeText(m => m.Address.Line1, "coc a.line1");
            form.TypeText(m => m.Address.Line2, "coc a.line2");
            form.TypeText(m => m.Address.Line3, "coc a.line3");
            form.TypeText(m => m.Address.Postcode, "CO1 1CP");
            form.TypeText(m => m.MobilePhoneNumber, "1234567");
            form.TypeText(m => m.HomePhoneNumer, "2345678");
            form.TypeText(m => m.EmailAddress, "coc.test@coc.com");
        }

        private void VerifyApplicantDetails(ChangeOfCircs doc)
        {
            doc.ApplicantDetails.Title.Should().Be("coc title");
            doc.ApplicantDetails.FullName.Should().Be("coc full name");
            doc.ApplicantDetails.Address.Line1.Should().Be("coc a.line1");
            doc.ApplicantDetails.Address.Line2.Should().Be("coc a.line2");
            doc.ApplicantDetails.Address.Line3.Should().Be("coc a.line3");
            doc.ApplicantDetails.Address.Postcode.Should().Be("CO1 1CP");
            doc.ApplicantDetails.MobilePhoneNumber.Should().Be("1234567");
            doc.ApplicantDetails.HomePhoneNumer.Should().Be("2345678");
            doc.ApplicantDetails.EmailAddress.Should().Be("coc.test@coc.com");
            _verifiedSections.Add(Sections.ApplicantDetails);
        }

        private string FillInEvidence()
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "some test content");
            App.FindElement($"select file {tempFile}", By.Name("file"), e => e.SendKeys(tempFile));
            App.ClickButton(CocButtons.UploadFile);

            var form = App.FormForModel<Evidence>();
            form.Check(m => m.SendingByPost, true);

            File.Delete(tempFile);
            return Path.GetFileName(tempFile);
        }

        private void VerifyEvidence(ChangeOfCircs doc, string filename)
        {
            doc.Evidence.Files.Count.Should().Be(1);
            doc.Evidence.Files[0].Name.Should().Be(filename);

            var cloudStore = DomainRegistry.CloudStore as LocalCloudStore;
            var storedContent = cloudStore.Retrieve("coc-" + doc.Id, doc.Evidence.Files[0].CloudName);
            Encoding.ASCII.GetString(storedContent).Should().Be("some test content");

            doc.Evidence.SendingByPost.Should().BeTrue();
            _verifiedSections.Add(Sections.Evidence);
        }
    }
}
