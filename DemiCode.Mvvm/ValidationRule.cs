using System;
using System.Linq.Expressions;

namespace DemiCode.Mvvm
{
    public class ValidationRule
    {
        public string PropertyName { get; private set; }
        public Func<bool> Condition { get; private set; }
        public string ErrorMessage { get; private set; }

        public ValidationRule(string propertyName, Func<bool> condition, string errorMessage)
        {
            PropertyName = propertyName;
            Condition = condition;
            ErrorMessage = errorMessage;
        }

        public static ValidationRule Create<TProperty>(Expression<Func<TProperty>> property, Func<bool> condition, string errorMessage)
        {
            var propertyMember = ExpressionHelper.GetPropertyFromExpression(property);
            return new ValidationRule(propertyMember.Name, condition, errorMessage);
        }

        public static ValidationRule CreateOptionalDouble(Expression<Func<double?>> property, Func<double?> currentValue, string errorMessage)
        {
            return Create(property, () => !currentValue().HasValue || !Double.IsNaN(currentValue().Value), errorMessage);
        }

        /// <summary>
        /// Create rule that validates a string as not null or empty.
        /// </summary>
        public static ValidationRule CreateRequiredString(Expression<Func<string>> property, Func<string> currentValue, string errorMessage)
        {
            return Create(property, () => !String.IsNullOrEmpty(currentValue()), errorMessage);
        }
    }
}
