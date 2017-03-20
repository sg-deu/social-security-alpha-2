using System;
using System.Linq;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Forms;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm
{
    public class ChangeOfCircs : Form
    {
        protected ChangeOfCircs() : base(Guid.NewGuid().ToString())
        {
        }

        public Consent Consent { get; protected set; }

        public CocDetail FindSection(Sections section)
        {
            var detail = new CocDetail
            {
                Consent = Consent,
            };

            Navigation.Populate(detail, section, this);

            return detail;
        }

        public static NextSection Start()
        {
            var form = new ChangeOfCircs();
            Repository.Insert(form);

            return new NextSection
            {
                Id = form.Id,
                Section = Navigation.Order.First(),
            };
        }

        public NextSection AddConsent(Consent consent)
        {
            Validate(consent);

            Consent = consent;
            return OnSectionCompleted(Sections.Consent);
        }

        private NextSection OnSectionCompleted(Sections section)
        {
            var next = Navigation.Next(this, section);
            Repository.Update(this);
            return next;
        }

        private static void Validate(Consent consent)
        {
            var ctx = new ValidationContext<Consent>(consent);

            ctx.Required(m => m.AgreedToConsent, "Please indicate that you agree");

            ctx.ThrowIfError();
        }
    }
}