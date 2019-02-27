using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DemiCode.Mvvm
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetPropertyFromExpression<TParent, TProperty>(Expression<Func<TParent, TProperty>> property)
        {
            var member = ((MemberExpression)property.Body).Member as PropertyInfo;
            if (member == null)
                throw new InvalidOperationException("Expression must be a property");
            return member;
        }

        public static PropertyInfo GetPropertyFromExpression<TProperty>(Expression<Func<TProperty>> property)
        {
            PropertyInfo member;
            var memberExpression = property.Body as MemberExpression;
            if (memberExpression != null)
            {
                member = memberExpression.Member as PropertyInfo;
            }
            else
            {
                var unaryExpression = (UnaryExpression)property.Body;
                member = ((MemberExpression)unaryExpression.Operand).Member as PropertyInfo;
            }
            if (member == null)
                throw new InvalidOperationException("Expression must be a property");
            return member;
        }

    }
}
