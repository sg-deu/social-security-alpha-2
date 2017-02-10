using System;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.Util
{
    public class TestRepository
    {
        public static void SetUp()
        {
            var localDbUri = new Uri("https://localhost:8081");
            var localDbKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

            Repository.Init(localDbUri, localDbKey);
        }

        public static void TearDown()
        {
        }
    }
}
