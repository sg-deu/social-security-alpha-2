using System;
using System.Collections.Generic;
using System.Linq;
using FormUI.Domain.BestStartGrantForms;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace FormUI.Domain.Util
{
    public class Repository : IRepository, IDisposable
    {
        private const string DbName = "Forms";

        private static Uri      _dbUri;
        private static string   _dbKey;

        public static IDictionary<Type, Uri> Links;

        public static void Init(Uri dbUri, string dbKey)
        {
            _dbUri = dbUri;
            _dbKey = dbKey;

            using (var client = NewClient())
            {
                var db = new Database { Id = DbName };
                client.CreateDatabaseIfNotExistsAsync(db).Wait();

                Links = new Dictionary<Type, Uri>();
                CreateCollection<BestStartGrant>(client);
            }
        }

        private static void CreateCollection<T>(DocumentClient client)
        {
            var collectionType = typeof(T);
            var dbLlink = UriFactory.CreateDatabaseUri(DbName);
            var collection = new DocumentCollection { Id = collectionType.Name };

            client.CreateDocumentCollectionIfNotExistsAsync(dbLlink, collection).Wait();

            var collectionLink = UriFactory.CreateDocumentCollectionUri(DbName, collection.Id);
            Links.Add(collectionType, collectionLink);
        }

        public static DocumentClient NewClient()
        {
            var client = new DocumentClient(_dbUri, _dbKey);
            return client;
        }

        public static Repository New()
        {
            return new Repository(NewClient());
        }

        private DocumentClient _client;

        public Repository(DocumentClient client)
        {
            _client = client;
        }

        public void Insert<T>(T doc)
        {
            var collectionLink = Links[typeof(T)];
            _client.CreateDocumentAsync(collectionLink, doc).Wait();
        }

        public T Load<T>(string id)
        {
            var documentUri = UriFactory.CreateDocumentUri(DbName, typeof(T).Name, id);
            var response = _client.ReadDocumentAsync(documentUri).Result;
            var doc = (T)(dynamic)response.Resource;
            return doc;
        }

        public void Update<T>(T doc)
            where T : IDocument
        {
            var documentUri = UriFactory.CreateDocumentUri(DbName, typeof(T).Name, doc.Id);
            _client.ReplaceDocumentAsync(documentUri, doc).Wait();
        }

        public IQueryable<T> Query<T>()
        {
            throw new NotImplementedException("query");
        }

        public void Dispose()
        {
            using (_client) { }
        }
    }
}