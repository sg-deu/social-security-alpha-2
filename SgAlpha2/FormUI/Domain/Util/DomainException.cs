using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FormUI.Domain.Util
{
    public class DomainException : Exception
    {
        public IDictionary<LambdaExpression, string> PropertyErrors;
    }
}