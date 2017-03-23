using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_EvidenceTests : DomainTest
    {
        [Test]
        public void AddEvidenceFile()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            var fileBytes = Encoding.ASCII.GetBytes("some content");
            form.AddEvidenceFile("FileName1.txt", fileBytes);

            var storedForm = Repository.Load<ChangeOfCircs>(form.Id);

            storedForm.Evidence.Files.Count.Should().Be(1);

            var file = storedForm.Evidence.Files[0];
            file.Name.Should().Be("FileName1.txt");

            CloudStore.List(form.Id).Should().Contain(file.CloudName);
        }

        [Test]
        public void AddEvidenceFile_HandlesTwoFilesWithSameName()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            var fileBytes = Encoding.ASCII.GetBytes("filename.txt content");
            form.AddEvidenceFile("filename", fileBytes);
            form.AddEvidenceFile("filename", fileBytes);

            var storedForm = Repository.Load<ChangeOfCircs>(form.Id);

            storedForm.Evidence.Files.Count.Should().Be(2);

            storedForm.Evidence.Files[0].Name.Should().Be("filename");
            storedForm.Evidence.Files[1].Name.Should().Be("filename");

            storedForm.Evidence.Files[0].CloudName.Should().NotBe(storedForm.Evidence.Files[1].CloudName, "cloud names should be unique");

            CloudStore.List("form").Should().Contain(storedForm.Evidence.Files[0].CloudName);
            CloudStore.List("form").Should().Contain(storedForm.Evidence.Files[1].CloudName);
        }

        [Test]
        public void Evidence_Validation()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            EvidenceShouldBeValid(form, m => { });
            EvidenceShouldBeValid(form, m => { m.SendingByPost = true; m.Files = new List<EvidenceFile>(); });
            EvidenceShouldBeValid(form, m => { m.SendingByPost = false; m.Files = new List<EvidenceFile> { new EvidenceFile() }; });

            EvidenceShouldBeInvalid(form, m => { m.SendingByPost = false; m.Files = new List<EvidenceFile>(); });
        }

        protected void EvidenceShouldBeValid(ChangeOfCircs form, Action<Evidence> mutator)
        {
            Builder.Modify(form).With(f => f.Evidence, null);
            ShouldBeValid(() => form.AddEvidence(EvidenceBuilder.NewValid(mutator)));
        }

        protected void EvidenceShouldBeInvalid(ChangeOfCircs form, Action<Evidence> mutator)
        {
            Builder.Modify(form).With(f => f.Evidence, null);
            ShouldBeInvalid(() => form.AddEvidence(EvidenceBuilder.NewValid(mutator)));
        }
    }
}
