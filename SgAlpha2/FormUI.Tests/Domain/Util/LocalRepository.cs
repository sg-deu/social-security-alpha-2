using System;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.Util
{
    public class LocalRepository : Repository
    {
        public static void SetUp()
        {
            var localDbUri = new Uri(VstsSettings.GetSetting("localDbUri", "https://localhost:8081"));
            var localDbKey = VstsSettings.GetSetting("localDbKey", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            Repository.Init(localDbUri, localDbKey);
        }

        public static void TearDown()
        {
        }

        public LocalRepository() : this(true) { }

        public LocalRepository(bool deleteAllDocuments) : base(NewClient())
        {
            if (deleteAllDocuments)
                DeleteAllDocuments();
        }

        public LocalRepository DeleteAllDocuments()
        {
            foreach (var collectionType in Links.Keys)
            {
                var collectionLink = Links[collectionType];
                var query = _client.CreateDocumentQuery(collectionLink);

                foreach (var doc in query)
                    _client.DeleteDocumentAsync(doc.SelfLink).Wait();
            }

            return this;
        }
    }
}
