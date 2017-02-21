using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FormUI.Domain.BestStartGrantForms;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            var defaultSettings = new JsonSerializerSettings
            {
                ContractResolver = new RepositoryContractResolver(),
            };

            JsonConvert.DefaultSettings = () => defaultSettings;

            _dbUri = dbUri;
            _dbKey = dbKey;

            using (var client = NewClient())
            {
                var db = new Database { Id = DbName };
                TaskUtil.Await(() => client.CreateDatabaseIfNotExistsAsync(db));

                Links = new Dictionary<Type, Uri>();
                CreateCollection<BestStartGrant>(client);
            }
        }

        private static void CreateCollection<T>(DocumentClient client)
        {
            var collectionType = typeof(T);
            var dbLlink = UriFactory.CreateDatabaseUri(DbName);
            var collection = new DocumentCollection { Id = collectionType.Name };

            TaskUtil.Await(() => client.CreateDocumentCollectionIfNotExistsAsync(dbLlink, collection));

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

        protected DocumentClient _client;

        public Repository(DocumentClient client)
        {
            _client = client;
        }

        public T Insert<T>(T doc)
        {
            DomainRegistry.ValidationContext.ThrowIfError();
            var collectionLink = Links[typeof(T)];
            TaskUtil.Await(() => _client.CreateDocumentAsync(collectionLink, doc));
            return doc;
        }

        public T Load<T>(string id)
        {
            var documentUri = UriFactory.CreateDocumentUri(DbName, typeof(T).Name, id);
            var response = TaskUtil.Result(() => _client.ReadDocumentAsync(documentUri));
            var doc = (T)(dynamic)response.Resource;
            return doc;
        }

        public T Update<T>(T doc)
            where T : IDocument
        {
            DomainRegistry.ValidationContext.ThrowIfError();
            var documentUri = UriFactory.CreateDocumentUri(DbName, typeof(T).Name, doc.Id);
            TaskUtil.Await(() => _client.ReplaceDocumentAsync(documentUri, doc));
            return doc;
        }

        public void Delete<T>(T doc)
            where T : IDocument
        {
            DomainRegistry.ValidationContext.ThrowIfError();
            var documentUri = UriFactory.CreateDocumentUri(DbName, typeof(T).Name, doc.Id);
            TaskUtil.Await(() => _client.DeleteDocumentAsync(documentUri));
        }

        public IOrderedQueryable<T> Query<T>()
        {
            var collectionLink = Links[typeof(T)];
            var queryable = _client.CreateDocumentQuery<T>(collectionLink);
            return queryable;
        }

        public virtual void Dispose()
        {
            using (_client) { }
        }

        private class RepositoryContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var jsonProperty = base.CreateProperty(member, memberSerialization);

                if (jsonProperty.Writable)
                    return jsonProperty;

                jsonProperty.Writable = true;
                return jsonProperty;
            }
        }
    }
}