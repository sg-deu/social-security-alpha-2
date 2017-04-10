using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_EvidenceTests : DomainTest
    {
        [Test]
        public void AddEvidenceFile()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            var fileBytes = Encoding.ASCII.GetBytes("some content");
            form.AddEvidenceFile("FileName1.txt", fileBytes);

            var storedForm = Repository.Load<BestStartGrant>(form.Id);

            storedForm.Evidence.Files.Count.Should().Be(1);

            var file = storedForm.Evidence.Files[0];
            file.Name.Should().Be("FileName1.txt");

            CloudStore.List("bsg-" + form.Id).Should().Contain(file.CloudName);
        }

        [Test]
        public void AddEvidenceFile_HandlesTwoFilesWithSameName()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            var fileBytes = Encoding.ASCII.GetBytes("filename.txt content");
            form.AddEvidenceFile("filename", fileBytes);
            form.AddEvidenceFile("filename", fileBytes);

            var storedForm = Repository.Load<BestStartGrant>(form.Id);

            storedForm.Evidence.Files.Count.Should().Be(2);

            storedForm.Evidence.Files[0].Name.Should().Be("filename");
            storedForm.Evidence.Files[1].Name.Should().Be("filename");

            storedForm.Evidence.Files[0].CloudName.Should().NotBe(storedForm.Evidence.Files[1].CloudName, "cloud names should be unique");

            CloudStore.List("bsg-form").Should().Contain(storedForm.Evidence.Files[0].CloudName);
            CloudStore.List("bsg-form").Should().Contain(storedForm.Evidence.Files[1].CloudName);
        }

        [Test]
        public void Evidence_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            EvidenceShouldBeValid(form, m => { });
            EvidenceShouldBeValid(form, m => { m.SendingByPost = true; m.Files = new List<EvidenceFile>(); });
            EvidenceShouldBeValid(form, m => { m.SendingByPost = false; m.Files = new List<EvidenceFile> { new EvidenceFile() }; });

            EvidenceShouldBeInvalid(form, m => { m.SendingByPost = false; m.Files = new List<EvidenceFile>(); });
        }

        protected void EvidenceShouldBeValid(BestStartGrant form, Action<Evidence> mutator)
        {
            Builder.Modify(form).With(f => f.Evidence, null);
            ShouldBeValid(() => form.AddEvidence(EvidenceBuilder.NewValid(mutator)));
        }

        protected void EvidenceShouldBeInvalid(BestStartGrant form, Action<Evidence> mutator)
        {
            Builder.Modify(form).With(f => f.Evidence, null);
            ShouldBeInvalid(() => form.AddEvidence(EvidenceBuilder.NewValid(mutator)));
        }
    }
}
