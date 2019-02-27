using System;
using System.Globalization;
using System.Windows.Controls;

namespace DemiCode.Mvvm.Validation
{
    public class RequiredIntegerValidationRule : System.Windows.Controls.ValidationRule
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
            if (!String.IsNullOrEmpty(stringValue))
            {
                int val;
                if (Int32.TryParse(stringValue, NumberStyles.Integer, cultureInfo, out val))
                    return ValidationResult.ValidResult;
            }
		    return new ValidationResult(false, "Value is not a valid number");
		}
	}
}