using System;
using System.Diagnostics;
using FormUI.Domain.Util;
using Newtonsoft.Json;

namespace FormUI.Domain.Forms
{
    public abstract class Form : IDocument
    {
        public Form(string id)
        {
            Id = id;
            Started = DomainRegistry.NowUtc();
        }

        protected static IRepository Repository
        {
            [DebuggerStepThrough]
            get { return DomainRegistry.Repository; }
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
    }
}