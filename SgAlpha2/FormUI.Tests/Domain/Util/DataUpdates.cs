using System;
using System.Linq;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    [Explicit("Intended to be triggered manually by a developer")]
    public class DataUpdates
    {
        [Test]
        public void UpdateDataToMakeConsistent()
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);

            using (var repo = LocalRepository.New(deleteAllDocuments: false))
                foreach (var doc in repo.Query<BestStartGrant>().ToList())
                {
                    var changed = false;

                    if (doc.UserId != doc.ApplicantDetails?.EmailAddress)
                    {
                        Builder.Modify(doc).With(d => d.UserId, doc.ApplicantDetails?.EmailAddress);
                        Console.WriteLine("Changing {0} UserId={1}", doc.Id, doc.ApplicantDetails?.EmailAddress);
                        changed = true;
                    }

                    if (doc.Started == DateTime.MinValue)
                    {
                        // this is not currently reached, as the default constructor will set Started
                        Builder.Modify(doc).With(f => f.Started, DateTime.UtcNow);
                        Console.WriteLine("Changing {0} Started={1}", doc.Id, DateTime.UtcNow);
                        changed = true;
                    }

                    if (doc.Declaration?.AgreedToLegalStatement == true && !doc.Completed.HasValue)
                    {
                        Builder.Modify(doc).With(f => f.Completed, DateTime.UtcNow);
                        Console.WriteLine("Changing {0} Completed={1}", doc.Id, DateTime.UtcNow);
                        changed = true;
                    }

                    if (changed)
                        repo.Update(doc);

                    var checkDoc = repo.Load<BestStartGrant>(doc.Id);
                    BestStartGrantBuilder.VerifyConsistent(checkDoc);
                }
        }
    }
}
