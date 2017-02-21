using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace FormUI.Domain.Util
{
    public class ValidationContext
    {
        private bool                                    _isValid;
        private IDictionary<LambdaExpression, string>   _errors     = new Dictionary<LambdaExpression, string>();

        public ValidationContext(bool isValid)
        {
            _isValid = isValid;
        }

        public void AddError(LambdaExpression property, string message)
        {
            _errors.Add(property, message);
        }

        public void ThrowIfError()
        {
            if (_errors.Count == 0 && _isValid)
                return;

            throw new DomainException(_errors);
        }
    }

    public class ValidationContext<T>
    {
        private T _model;

        public ValidationContext(T model)
        {
            _model = model;
        }

        private ValidationContext Current { [DebuggerStepThrough] get { return DomainRegistry.ValidationContext; } }

        public void Required(Expression<Func<T, string>> property, string message)
        {
            var value = property.Compile()(_model);

            if (string.IsNullOrWhiteSpace(value))
                Current.AddError(property, message);
        }

        public void Required(Expression<Func<T, DateTime?>> property, string message)
        {
            var value = property.Compile()(_model);

            if (!value.HasValue)
                Current.AddError(property, message);
        }

        public void Required<TStruct>(Expression<Func<T, Nullable<TStruct>>> property, string message)
            where TStruct : struct
        {
            Nullable<TStruct> value = property.Compile()(_model);

            if (!value.HasValue)
                Current.AddError(property, message);
        }

        public void Custom<TProp>(Expression<Func<T, TProp>> property, Func<TProp, string> validator)
        {
            TProp value = property.Compile()(_model);
            var message = validator(value);

            if (message != null)
                Current.AddError(property, message);
        }

        public void ThrowIfError()
        {
            Current.ThrowIfError();
        }
    }
}