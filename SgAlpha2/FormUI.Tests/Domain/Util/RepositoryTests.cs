using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.Util;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
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

            using (var repository = Repository.New())
            {
                var doc = new BestStartGrant() { Value = "some data" };
                id = doc.Id;
                repository.Insert(doc);
            }

            using (var repository = Repository.New())
            {
                var doc = repository.Load<BestStartGrant>(id);
                doc.Value.Should().Be("some data");
            }
        }

        [Test]
        public void Update()
        {

        }

        [Test]
        public void Query()
        {
        }

        [Test]
        [Explicit("Just for local testing for now")]
        public void SaveDocument()
        {
            var dbUri = new Uri("https://localhost:8081");
            var dbKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

            using (var client = new DocumentClient(dbUri, dbKey))
            {
                var db =
                    new Database
                    {
                        Id = "FormsDb",
                    };

                client.CreateDatabaseIfNotExistsAsync(db).Wait();

                var collectionUri = UriFactory.CreateDocumentCollectionUri(db.Id, "Form");

                var collection =
                    new DocumentCollection
                    {
                        Id = "Form",
                    };

                var dbLlink = UriFactory.CreateDatabaseUri(db.Id);
                client.CreateDocumentCollectionIfNotExistsAsync(dbLlink, collection).Wait();

                var cLink = UriFactory.CreateDocumentCollectionUri(db.Id, collection.Id);

                var formQuery = client.CreateDocumentQuery<BestStartGrant>(cLink);
                var existingDocs = formQuery;

                foreach (var existingDoc in existingDocs)
                {
                    Console.WriteLine(existingDoc.Id);
                    client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(db.Id, collection.Id,  existingDoc.Id)).Wait();
                }

                for (var i = 0; i < 5; i++)
                {
                    var doc = new BestStartGrant();
                    var savedDoc = client.CreateDocumentAsync(cLink, doc).Result;
                }
            }
        }
    }
}
