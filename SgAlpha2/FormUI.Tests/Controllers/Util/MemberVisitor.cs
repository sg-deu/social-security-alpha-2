using System;
using System.Collections;
using System.Reflection;

namespace FormUI.Tests.Controllers.Util
{
    public class MemberVisitor
    {
        public delegate void Visitor(string name, MemberInfo memberInfo, Type type, object value);

        public static void Visit(object source, Visitor visitor)
        {
            VisitMembers(visitor, source, "");
        }

        private static void VisitMembers(Visitor visitor, object source, string prefix)
        {
            if (source == null)
                return;

            var type = source.GetType();

            if (IsList(type))
            {
                VisitList(visitor, source, prefix);
                return;
            }

            var props = type.GetProperties();

            foreach (var prop in props)
                VisitMember(visitor, prop, prop.PropertyType, prop.Name, () => prop.GetValue(source, null), prefix);

            var fields = type.GetFields();

            foreach (var field in fields)
                VisitMember(visitor, field, field.FieldType, field.Name, () => field.GetValue(source), prefix);
        }

        private static void VisitMember(Visitor visitor, MemberInfo memberInfo, Type type, string name, Func<object> value, string prefix)
        {
            if (IsCompound(type))
                VisitMembers(visitor, value(), PrefixFor(prefix, name));
            else
                visitor(PrefixFor(prefix, name), memberInfo, type, value());
        }

        private static void VisitList(Visitor visitor, object source, string prefix)
        {
            var enumerable = (IEnumerable)source;
            var index = 0;

            foreach (var item in enumerable)
                VisitMembers(visitor, item, prefix + $"[{index++}]");
        }

        private static bool IsList(Type type)
        {
            if (type == typeof(string))
                return false;

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static bool IsCompound(Type type)
        {
            if (type.IsValueType || type == typeof(string))
                return false;

            if (type.FullName.Contains("FormUI.Domain."))
                return true;

            throw new Exception("Unhandled type: " + type);
        }

        private static string PrefixFor(string existingPrefix, string name)
        {
            return string.IsNullOrWhiteSpace(existingPrefix)
                ? name
                : existingPrefix + "." + name;
        }
    }
}
