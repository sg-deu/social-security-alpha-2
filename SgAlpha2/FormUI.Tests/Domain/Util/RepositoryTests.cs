using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void SaveAndLoad()
        {
            string id;

            using (var repository = new LocalRepository())
            {
                var doc = new BestStartGrant() { Value = "some data" };
                id = doc.Id;
                repository.Insert(doc);
            }

            using (var repository = new LocalRepository(deleteAllDocuments: false))
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.Value.Should().Be("some data");
            }
        }

        [Test]
        public void Update()
        {
            string id;

            using (var repository = new LocalRepository())
            {
                var doc = new BestStartGrant() { Value = "some data" };
                id = doc.Id;
                repository.Insert(doc);
            }

            using (var repository = new LocalRepository(deleteAllDocuments: false))
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.Value = "updated value";
                repository.Update(doc);
            }

            using (var repository = new LocalRepository(deleteAllDocuments: false))
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.Value.Should().Be("updated value");
            }
        }

        [Test]
        public void Query()
        {
            using (var repository = new LocalRepository())
            {
                repository.Insert(new BestStartGrant { Value = "Bsg1" });
                repository.Insert(new BestStartGrant { Value = "Bsg2" });
                repository.Insert(new BestStartGrant { Value = "Bsg3" });

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
            using (var repository = new LocalRepository())
            {
                var doc1 = repository.Insert(new BestStartGrant { Value = "Bsg1" });
                var doc2 = repository.Insert(new BestStartGrant { Value = "Bsg2" });
                var doc3 = repository.Insert(new BestStartGrant { Value = "Bsg3" });

                var doc =
                    repository.Query<BestStartGrant>()
                        .Where(bsg => bsg.Value == "Bsg2")
                        .ToList()
                        .Single();

                doc.Value.Should().Be("Bsg2");
                doc.Id.Should().Be(doc2.Id);
            }
        }
    }
}
