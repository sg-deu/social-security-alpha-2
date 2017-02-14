using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FormUI.Domain.Util
{
    public class ValidationContext<T>
    {
        private T                                       _model;
        private IDictionary<LambdaExpression, string>   _errors = new Dictionary<LambdaExpression, string>();

        public ValidationContext(T model)
        {
            _model = model;
        }

        public void Required(Expression<Func<T, string>> stringProperty, string message)
        {
            var value = stringProperty.Compile()(_model);

            if (string.IsNullOrWhiteSpace(value))
                _errors.Add(stringProperty, message);
        }

        public void ThrowIfError()
        {
            if (_errors.Count == 0)
                return;

            throw new DomainException { PropertyErrors = _errors };
        }
    }
}