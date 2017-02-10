using System;
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
        public void TestEnvironment()
        {
            var setting = Environment.GetEnvironmentVariable("Alpha2Db");
            Console.WriteLine(setting ?? "<null>");
        }

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

            using (var repository = new LocalRepository())
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

            using (var repository = new LocalRepository())
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.Value = "updated value";
                repository.Update(doc);
            }

            using (var repository = new LocalRepository())
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.Value.Should().Be("updated value");
            }
        }

        [Test]
        public void Query()
        {
            using (var repository = new LocalRepository(true))
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
    }
}
