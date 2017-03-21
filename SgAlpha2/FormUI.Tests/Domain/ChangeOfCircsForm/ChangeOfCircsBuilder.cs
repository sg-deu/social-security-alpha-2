using System;
using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    public class ChangeOfCircsBuilder : Builder<ChangeOfCircs>
    {
        public ChangeOfCircsBuilder(string formId)
        {
            With(f => f.Id, formId);
        }

        public ChangeOfCircsBuilder WithCompletedSections()
        {
            With(f => f.Consent, ConsentBuilder.NewValid());
            With(f => f.UserId, "existing.user@known.com");

            With(f => f.Started, DomainRegistry.NowUtc() - TimeSpan.FromHours(24));
            With(f => f.Completed, DomainRegistry.NowUtc());
            VerifyConsistent(_instance);
            return this;
        }

        public static void CopySectionsFrom(ChangeOfCircs form, CocDetail detail)
        {
            detail.Consent = form.Consent;
            detail.Identity = form.UserId;
        }

        public static void VerifyConsistent(ChangeOfCircs doc)
        {
            doc.Started.Should().NotBe(DateTime.MinValue);
        }
    }
}
