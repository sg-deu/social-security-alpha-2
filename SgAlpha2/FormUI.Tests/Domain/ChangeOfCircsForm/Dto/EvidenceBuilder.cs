using System;
using System.Text;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
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

        public static Evidence AddFiles(this Evidence evidence, ChangeOfCircs form, int count)
        {
            var cloudStore = DomainRegistry.CloudStore;

            for (int i = 0; i < count; i++)
            {
                var fileName = $"evidence{i + 1}.txt";
                var cloudName = $"cloud{i}.txt";
                cloudStore.Store(form.Id, cloudName, fileName, Encoding.ASCII.GetBytes($"file content {i}"));
                evidence.Files.Add(new EvidenceFile { Name = fileName, CloudName = cloudName });
            }

            return evidence;
        }
    }
}
