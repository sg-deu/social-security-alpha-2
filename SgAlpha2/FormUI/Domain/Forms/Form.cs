﻿using FormUI.Domain.Util;
using Newtonsoft.Json;

namespace FormUI.Domain.Forms
{
    public abstract class Form : IDocument
    {
        public Form(string id)
        {
            Id = id;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }
    }
}