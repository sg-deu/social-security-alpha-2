using System;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.Util
{
    public class LocalRepository : Repository
    {
        private static bool _isSetup = false;

        private static void SetUp()
        {
            if (_isSetup)
                return;

            var localDbUri = new Uri(VstsSettings.GetSetting("localDbUri", "https://localhost:8081"));
            var localDbKey = VstsSettings.GetSetting("localDbKey", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            Repository.Init(localDbUri, localDbKey);
            _isSetup = true;
        }

        public static LocalRepository New(bool deleteAllDocuments = true)
        {
            SetUp();
            return new LocalRepository(deleteAllDocuments);
        }

        private LocalRepository(bool deleteAllDocuments) : base(NewClient())
        {
            if (deleteAllDocuments)
                DeleteAllDocuments();
        }

        public LocalRepository Register()
        {
            DomainRegistry.Repository = this;
            return this;
        }

        public LocalRepository DeleteAllDocuments()
        {
            foreach (var collectionType in Links.Keys)
            {
                var collectionLink = Links[collectionType];
                var query = _client.CreateDocumentQuery(collectionLink);

                foreach (var doc in query)
                    TaskUtil.Await(() => _client.DeleteDocumentAsync(doc.SelfLink));
            }

            return this;
        }
    }
}
