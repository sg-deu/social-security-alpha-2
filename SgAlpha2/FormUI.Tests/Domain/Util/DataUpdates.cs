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
        public void AssignUserIdToBsgs()
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);

            using (var repo = LocalRepository.New(deleteAllDocuments: false))
                foreach (var doc in repo.Query<BestStartGrant>().ToList())
                {
                    var changed = false;

                    if (doc.UserId != doc.ApplicantDetails?.EmailAddress)
                    {
                        Builder.Modify(doc).With(d => d.UserId, doc.ApplicantDetails?.EmailAddress);
                        changed = true;
                    }

                    if (doc.Started == DateTime.MinValue)
                    {
                        Builder.Modify(doc).With(f => f.Started, DateTime.UtcNow);
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
