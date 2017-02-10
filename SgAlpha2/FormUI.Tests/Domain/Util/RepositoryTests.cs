using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    public class Form
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    [TestFixture]
    public class RepositoryTests
    {
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

                var existingDocs = client.ReadDocumentFeedAsync(cLink).Result;
                foreach (var existingDoc in existingDocs)
                {
                    client.DeleteDocumentAsync(existingDoc.SelfLink).Wait();
                }

                for (var i = 0; i < 5; i++)
                {
                    var doc =
                        new Form()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Value = "My document, created at: " + DateTime.UtcNow,
                        };

                    var savedDoc = client.CreateDocumentAsync(cLink, doc).Result;
                }
            }
        }
    }
}
