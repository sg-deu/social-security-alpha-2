using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FormUI.Controllers.Helpers
{
    public static class ReflectHelper
    {
        public static string GetExpressionText(this LambdaExpression propertyLambda)
        {
            if (propertyLambda.Body.NodeType == ExpressionType.Convert)
                propertyLambda = LambdaExpression.Lambda((propertyLambda.Body as UnaryExpression).Operand);

            return ExpressionHelper.GetExpressionText(propertyLambda);
        }

        public static IList<string> GetEnumStringValues(this Type enumType)
        {
            return Enum.GetNames(enumType).ToList();
        }

        public static IDictionary<TEnum, string> GetEnumDescriptions<TEnum>() where TEnum : struct
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var descriptions = enumValues.ToDictionary(e => e, e => GetEnumDescription(e));
            return descriptions;
        }

        public static string GetEnumDescription<TEnum>(Nullable<TEnum> value) where TEnum : struct
        {
            return (value.HasValue) ? GetEnumDescription(value.Value) : "";
        }

        public static string GetEnumDescription<TEnum>(TEnum value) where TEnum : struct
        {
            var type = typeof(TEnum);
            var name = value.ToString();

            var field = type.GetField(name);

            if (field == null)
                throw new System.Exception(string.Format("Unable to find enum type {0}", value));

            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }
    }
}