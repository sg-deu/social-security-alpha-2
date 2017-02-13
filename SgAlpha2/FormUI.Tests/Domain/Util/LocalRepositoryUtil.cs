﻿using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    [Explicit("Intended to be triggered manually by a developer")]
    public class LocalRepositoryUtil
    {
        [Test]
        public void DeleteAllDatabases()
        {
            using (var client = LocalRepository.NewClient())
            {
                var databases = client.CreateDatabaseQuery();

                foreach (var database in databases)
                    client.DeleteDatabaseAsync(database.SelfLink).Wait();
            }
        }
    }
}