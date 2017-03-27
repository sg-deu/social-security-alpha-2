using System;
using System.Diagnostics;
using FormUI.Domain.Util;
using Newtonsoft.Json;

namespace FormUI.Domain.Forms
{
    public abstract class Form : IDocument
    {
        protected Form() { }

        protected Form(string id)
        {
            Id = id;
            Started = DomainRegistry.NowUtc();
        }

        protected static IRepository Repository
        {
            [DebuggerStepThrough]
            get { return DomainRegistry.Repository; }
        }

        protected static ICloudStore CloudStore
        {
            [DebuggerStepThrough]
            get { return DomainRegistry.CloudStore; }
        }

        [JsonProperty(PropertyName = "id")]
        public string       Id          { get; protected set; }

        public string       UserId      { get; protected set; }
        public DateTime     Started     { get; protected set; }
        public DateTime?    Completed   { get; protected set; }

        protected void OnUserIdentified(string userId)
        {
            UserId = userId;
        }

        protected void OnBeforeUpdate(bool isFinalUpdate)
        {
            if (Completed.HasValue)
                throw new DomainException("This application has already been submitted and cannot be modified");

            if (isFinalUpdate)
                Completed = DomainRegistry.NowUtc();
        }

    }
}