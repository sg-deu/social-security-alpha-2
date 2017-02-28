using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    public class RepositoryTests : DomainTest
    {
        [Test]
        public void InsertAndLoad()
        {
            string id;

            using (var repository = LocalRepository.New())
            {
                var doc = new BestStartGrantBuilder("form123").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "some data" }).Value();
                id = doc.Id;
                repository.Insert(doc);
            }

            using (var repository = LocalRepository.New(deleteAllDocuments: false))
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.ApplicantDetails.FirstName.Should().Be("some data");
            }
        }

        [Test]
        public void Update()
        {
            string id;

            using (var repository = LocalRepository.New())
            {
                var doc = new BestStartGrantBuilder("form123").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "some data" }).Value();
                id = doc.Id;
                repository.Insert(doc);
            }

            using (var repository = LocalRepository.New(deleteAllDocuments: false))
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.ApplicantDetails.FirstName = "updated value";
                repository.Update(doc);
            }

            using (var repository = LocalRepository.New(deleteAllDocuments: false))
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.ApplicantDetails.FirstName.Should().Be("updated value");
            }
        }

        [Test]
        public void Delete()
        {
            string id;

            using (var repository = LocalRepository.New())
            {
                var doc = new BestStartGrantBuilder("form123").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "some data" }).Value();
                id = doc.Id;
                repository.Insert(doc);
            }

            using (var repository = LocalRepository.New(deleteAllDocuments: false))
            {
                var doc = repository.Load<BestStartGrant>(id);
                repository.Delete(doc);
            }

            using (var repository = LocalRepository.New(deleteAllDocuments: false))
            {
                var count =
                    repository.Query<BestStartGrant>()
                        .ToList()
                        .Count;

                count.Should().Be(0);
            }
        }

        [Test]
        public void Insert_Fails_IsValidationContextHasErrors()
        {
            using (var repository = LocalRepository.New())
            {
                var doc = new BestStartGrantBuilder("form123").Value();

                DomainRegistry.ValidationContext = new ValidationContext(false);
                Assert.Throws<DomainException>(() => repository.Insert(doc));
            }
        }

        [Test]
        public void Update_Fails_IsValidationContextHasErrors()
        {
            using (var repository = LocalRepository.New())
            {
                var doc = new BestStartGrantBuilder("form123").Value();
                repository.Insert(doc);

                DomainRegistry.ValidationContext = new ValidationContext(false);
                Assert.Throws<DomainException>(() => repository.Update(doc));
            }
        }

        [Test]
        public void Delete_Fails_IsValidationContextHasErrors()
        {
            using (var repository = LocalRepository.New())
            {
                var doc = new BestStartGrantBuilder("form123").Value();
                repository.Insert(doc);

                DomainRegistry.ValidationContext = new ValidationContext(false);
                Assert.Throws<DomainException>(() => repository.Delete(doc));
            }
        }

        [Test]
        public void Query()
        {
            using (var repository = LocalRepository.New())
            {
                repository.Insert(new BestStartGrantBuilder("form1").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "Bsg1" }).Value());
                repository.Insert(new BestStartGrantBuilder("form2").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "Bsg2" }).Value());
                repository.Insert(new BestStartGrantBuilder("form3").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "Bsg3" }).Value());

                var count =
                    repository.Query<BestStartGrant>()
                        .ToList()
                        .Count;

                count.Should().Be(3);
            }
        }

        [Test]
        public void QueryWithFilter()
        {
            using (var repository = LocalRepository.New())
            {
                var doc1 = repository.Insert(new BestStartGrantBuilder("form1").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "Bsg1" }).Value());
                var doc2 = repository.Insert(new BestStartGrantBuilder("form2").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "Bsg2" }).Value());
                var doc3 = repository.Insert(new BestStartGrantBuilder("form3").With(f => f.ApplicantDetails, new ApplicantDetails { FirstName = "Bsg3" }).Value());

                var doc =
                    repository.Query<BestStartGrant>()
                        .Where(bsg => bsg.ApplicantDetails.FirstName == "Bsg2")
                        .ToList()
                        .Single();

                doc.ApplicantDetails.FirstName.Should().Be("Bsg2");
                doc.Id.Should().Be(doc2.Id);
            }
        }
    }
}
