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
            VerifyNotEmptyMessage(message);
            Custom(property, s => string.IsNullOrWhiteSpace(s) ? message : null);
        }

        public void Required(Expression<Func<T, DateTime?>> property, string message)
        {
            VerifyNotEmptyMessage(message);
            Custom(property, d => !d.HasValue ? message : null);
        }

        public void Required<TStruct>(Expression<Func<T, Nullable<TStruct>>> property, string message)
            where TStruct : struct
        {
            VerifyNotEmptyMessage(message);
            Custom(property, v => !v.HasValue ? message : null);
        }

        public void InPast(Expression<Func<T, DateTime?>> property, string message)
        {
            VerifyNotEmptyMessage(message);
            Custom(property, d => d.HasValue && d.Value.Date >= DomainRegistry.NowUtc().Date ? message : null);
        }

        public void InFuture(Expression<Func<T, DateTime?>> property, string message)
        {
            VerifyNotEmptyMessage(message);
            Custom(property, d => d.HasValue && d.Value.Date < DomainRegistry.NowUtc().Date ? message : null);
        }

        public void Custom<TProp>(Expression<Func<T, TProp>> property, Func<TProp, string> validator)
        {
            TProp value = property.Compile()(_model);
            var message = validator(value);

            if (message != null)
                Current.AddError(property, message);
        }

        private void VerifyNotEmptyMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("message cannot be empty or whitespace", "message");
        }

        public void ThrowIfError()
        {
            Current.ThrowIfError();
        }
    }
}