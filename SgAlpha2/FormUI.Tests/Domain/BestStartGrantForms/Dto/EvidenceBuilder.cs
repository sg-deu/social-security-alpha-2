using System;
using System.Text;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public static class EvidenceBuilder
    {
        public static Evidence NewValid(Action<Evidence> mutator = null)
        {
            var value = new Evidence
            {
                SendingByPost = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static Evidence AddFiles(this Evidence evidence, BestStartGrant form, int count)
        {
            var cloudStore = DomainRegistry.CloudStore as LocalCloudStore;
            var existingFiles = cloudStore.List("bsg-" + form.Id);

            for (int i = 0; i < count; i++)
            {
                var fileName = $"evidence{i + 1}.txt";
                var cloudName = $"cloud{i}.txt";

                if (!existingFiles.Contains(cloudName))
                    cloudStore.Store("bsg-" + form.Id, cloudName, fileName, Encoding.ASCII.GetBytes($"file content {i}"));

                evidence.Files.Add(new EvidenceFile { Name = fileName, CloudName = cloudName });
            }

            return evidence;
        }
    }
}
