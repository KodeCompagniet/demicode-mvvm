using System;
using System.Globalization;
using System.Windows.Controls;

namespace DemiCode.Mvvm.Validation
{
	public class OptionalNumericValidationRule : System.Windows.Controls.ValidationRule
	{
		/// <summary>
		/// When overridden in a derived class, performs validation checks on a value.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Windows.Controls.ValidationResult"/> object.
		/// </returns>
		/// <param name="value">The value from the binding target to check.</param><param name="cultureInfo">The culture to use in this rule.</param>
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			var stringValue = value as string;
			if (String.IsNullOrEmpty(stringValue))
				return ValidationResult.ValidResult;

			double val;
			if (Double.TryParse(stringValue, NumberStyles.Any, cultureInfo, out val))
				return ValidationResult.ValidResult;

			return new ValidationResult(false, "Value is not a valid number");
		}
	}
}