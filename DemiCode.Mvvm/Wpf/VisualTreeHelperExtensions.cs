using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DemiCode.Mvvm.Wpf
{
	/// <summary>
	/// Extension methods for enumerating elements and bindings in a <see cref="DependencyObject"/>.
	/// </summary>
	public static class VisualTreeHelperExtensions
	{
		public struct PropertyBinding
		{
			public DependencyObject Parent;
			public Binding Binding;
			public DependencyProperty Property;
		}

		/// <summary>
		/// Get property bindings for the given <see cref="DependencyObject"/>.
		/// </summary>
		public static IEnumerable<PropertyBinding> GetPropertyBindings(this DependencyObject parent)
		{
			var localValues = parent.GetLocalValueEnumerator();
			while (localValues.MoveNext())
			{
				var entry = localValues.Current;
				if (!BindingOperations.IsDataBound(parent, entry.Property)) continue;

				var binding = BindingOperations.GetBinding(parent, entry.Property);
				if (binding == null) continue;
				yield return new PropertyBinding { Parent = parent, Binding = binding, Property = entry.Property };
			}
		}

		/// <summary>
		/// Return a collection of children elements.
		/// </summary>
		public static IEnumerable<DependencyObject> GetChildren(this DependencyObject parent)
		{
			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (var i = 0; i < childrenCount; i++)
			{
				yield return VisualTreeHelper.GetChild(parent, i);
			}
		}

		/// <summary>
		/// Return a collection of expressions that fails validation rules.
		/// </summary>
		public static IEnumerable<BindingExpression> GetInvalidExpressions(this PropertyBinding propertyBinding)
		{
			var parent = propertyBinding.Parent;
			var property = propertyBinding.Property;
			var propertyValue = parent.GetValue(property);

			return propertyBinding.Binding.ValidationRules
                .Select(rule => rule.Validate(propertyValue, null))
                .Where(result => !result.IsValid).Select(result => BindingOperations.GetBindingExpression(parent, property))
                .Where(expression => expression != null);
		}
	}
}