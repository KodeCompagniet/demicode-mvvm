using System;
using System.Linq.Expressions;

namespace DemiCode.Mvvm.Helpers
{
    internal static class NotifyPropertyChangedHelper
    {
        public static string GetPropertyNameFromExpression<T>(Expression<Func<T>> property)
        {
            return ((MemberExpression)property.Body).Member.Name;
        }

    }
}