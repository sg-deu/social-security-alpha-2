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

        public void Required(Expression<Func<T, string>> property, string message)
        {
            var value = property.Compile()(_model);

            if (string.IsNullOrWhiteSpace(value))
                _errors.Add(property, message);
        }

        public void Required(Expression<Func<T, DateTime?>> property, string message)
        {
            var value = property.Compile()(_model);

            if (!value.HasValue)
                _errors.Add(property, message);
        }

        public void Required<TStruct>(Expression<Func<T, Nullable<TStruct>>> property, string message)
            where TStruct : struct
        {
            Nullable<TStruct> value = property.Compile()(_model);

            if (!value.HasValue)
                _errors.Add(property, message);
        }

        public void Custom<TProp>(Expression<Func<T, TProp>> property, Func<TProp, string> validator)
        {
            TProp value = property.Compile()(_model);
            var message = validator(value);

            if (message != null)
                _errors.Add(property, message);
        }

        public void ThrowIfError()
        {
            if (_errors.Count == 0)
                return;

            throw new DomainException(_errors);
        }
    }
}