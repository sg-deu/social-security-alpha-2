﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FormUI.Domain.Util
{
    public class DomainException : Exception
    {
        protected DomainException()
        {
            Messages = new List<string>();
            PropertyErrors = new Dictionary<LambdaExpression, string>();
        }

        public DomainException(string message) : this(new string[] { message })
        { }

        public DomainException(IEnumerable<string> messages) : this()
        {
            Messages = messages;
        }

        public DomainException(IDictionary<LambdaExpression, string> propertyErrors) : this()
        {
            PropertyErrors = propertyErrors;
        }

        public IEnumerable<string>                      Messages        { get; protected set; }
        public IDictionary<LambdaExpression, string>    PropertyErrors  { get; protected set; }

        public override string ToString()
        {
            return string.Join("; ", new string[]
                {
                    string.Join("; ", Messages),
                    string.Join("; ", PropertyErrors.Select(pe => $"{pe.Key.ToString()}:{pe.Value}")),
                }.Where(e => !string.IsNullOrWhiteSpace(e)));
        }

        public override string Message { get { return ToString(); } }
    }
}