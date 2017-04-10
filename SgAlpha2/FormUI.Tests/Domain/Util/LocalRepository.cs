using System;
using System.Linq;
using System.Net.Sockets;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Forms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    public class LocalRepository : Repository
    {
        private const string DefaultDbUri = "https://localhost:8081";
        private const string DefaultDbKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private static bool _isSetup = false;

        private static void SetUp()
        {
            if (_isSetup)
                return;

            var dbUri = new Uri(VstsSettings.GetSetting("dbUri", DefaultDbUri));
            var dbKey = VstsSettings.GetSetting("dbKey", DefaultDbKey);

            Repository.Init(dbUri, dbKey);
            _isSetup = true;
        }

        public static void VerifyRunning()
        {
            var dbUri = VstsSettings.GetSetting("dbUri", DefaultDbUri);

            if (dbUri != DefaultDbUri)
                return; // this is just a check to help local devs

            try
            {
                using (var tcpClient = new TcpClient())
                    tcpClient.Connect("localhost", 8081);
            }
            catch
            {
                Assert.Fail("Could not verify DocumentDB connection - please verify DocumentDB is running");
            }
        }

        public static LocalRepository New(bool deleteAllDocuments = true)
        {
            SetUp();
            return new LocalRepository(deleteAllDocuments);
        }

        public static void AddTestDocuments()
        {
            if (!_isSetup)
                return; // DB wasn't touched, so nothing to do here

            var dbUri = VstsSettings.GetSetting("dbUri", DefaultDbUri);

            if (dbUri != DefaultDbUri)
                return; // this data is only to help local devs - don't put on live site

            using (var repository = New(deleteAllDocuments: false).Register())
            {
                DomainRegistry.ValidationContext = new ValidationContext(true);
                DomainRegistry.NowUtc = () => DateTime.UtcNow;
                DomainRegistry.CloudStore = LocalCloudStore.New(clearContainer: false);

                AddTestBsg(repository, "unitTest");

                var coc = AddTestCoc(repository, "unitTest");

                AddTestBsg(repository, "unitTestExisting", bsg =>
                {
                    bsg.ApplicantDetails.EmailAddress = coc.UserId;

                    Builder.Modify(bsg)
                        .With(f => f.UserId, coc.UserId)
                        .With(f => f.Declaration, BestStartGrantForms.Dto.DeclarationBuilder.NewValid())
                        .With(f => f.Completed, DateTime.UtcNow);
                });
            }
        }

        private static void AddTestBsg(LocalRepository repository, string formId, Action<BestStartGrant> mutator = null)
        {
            var existingForm = repository.Query<BestStartGrant>()
                .Where(f => f.Id == formId)
                .ToList()
                .FirstOrDefault();

            if (existingForm != null)
                return;

            new BestStartGrantBuilder(formId)
                .WithCompletedSections(markAsCompleted: false)
                .With(f => f.Declaration, null)
                .Insert(f =>
                {
                    f.Evidence.AddFiles(f, 2);
                    mutator?.Invoke(f);
                });
        }

        private static ChangeOfCircs AddTestCoc(LocalRepository repository, string formId)
        {
            var existingForm = repository.Query<ChangeOfCircs>()
                .Where(f => f.Id == formId)
                .ToList()
                .FirstOrDefault();

            if (existingForm != null)
                return existingForm;

            return new ChangeOfCircsBuilder(formId)
                .WithCompletedSections(markAsCompleted: false)
                .Insert(f =>
                {
                    f.ExistingPaymentDetails.WithoutAccount();
                    f.Evidence.AddFiles(f, 2);
                });
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
