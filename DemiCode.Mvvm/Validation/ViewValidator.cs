using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DemiCode.Mvvm.Wpf;

namespace DemiCode.Mvvm.Validation
{
	public static class ViewValidator
	{
		public static readonly DependencyProperty ValidationRulesProperty =
			DependencyProperty.RegisterAttached(
				"ValidationRules",
				typeof (ValidationRuleCollection),
				typeof (ViewValidator),
				new UIPropertyMetadata(null));

		public static ValidationRuleCollection GetValidationRules(DependencyObject element)
		{
			return (ValidationRuleCollection)element.GetValue(ValidationRulesProperty);
		}

		public static void SetValidationRules(DependencyObject element, ValidationRuleCollection value)
		{
			element.SetValue(ValidationRulesProperty, value);
		}


		/// <summary>
		/// Return true if all <see cref="ValidationRule"/> elements in <paramref name="element"/> validates successfully
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool IsValid(DependencyObject element)
		{
		    var cachedResults = new Dictionary<BindingExpression, bool>();
		    return IsValidImpl(element, cachedResults);
		}

	    public static void AddValidators(DependencyObject parent)
		{
			var rules = parent.GetValue(ValidationRulesProperty) as ValidationRuleCollection;
			if (rules != null && rules.Count > 0)
			{
				foreach (var propertyBinding in parent.GetPropertyBindings())
				{
					foreach (var rule in rules)
					{
						propertyBinding.Binding.ValidationRules.Add(rule);
					}
				}
			}

			foreach (var child in parent.GetChildren())
			{
				AddValidators(child);
			}
		}


        private static bool IsValidImpl(DependencyObject me, IDictionary<BindingExpression, bool> cachedResults)
        {
            // Validate all the bindings on "me"
            foreach (var expression in me.GetPropertyBindings()
                .SelectMany(propertyBinding => propertyBinding.GetInvalidExpressions()))
            {
                if (cachedResults.ContainsKey(expression))
                    return cachedResults[expression];

                expression.UpdateSource();
                var isValid = !expression.HasError;
                cachedResults.Add(expression, isValid);

                if (!isValid)
                    return false;
            }

            // Validate all the bindings on the children
            return me.GetChildren().All(c =>
            {
                // If child is a known IViewValidation instance, we'll shortcut our crawl here
                var viewValidation = c as IViewValidation;
                if (viewValidation != null)
                    return viewValidation.IsValid;
                
                return IsValidImpl(c, cachedResults);
            });
        }

    }
}