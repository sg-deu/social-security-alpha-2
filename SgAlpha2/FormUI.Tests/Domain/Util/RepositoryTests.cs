using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    public abstract class Form
    {
        public Form(string id)
        {
            Id = id;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }

    }

    public class BestStartGrant : Form
    {
        public BestStartGrant() : base(Guid.NewGuid().ToString())
        {
        }

        public string Value { get; set; }
    }

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
                    var doc =
                        new BestStartGrant
                        {
                            Value = "My document, created at: " + DateTime.UtcNow,
                        };

                    var savedDoc = client.CreateDocumentAsync(cLink, doc).Result;
                }
            }
        }
    }
}
