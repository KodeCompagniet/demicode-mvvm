using System;
using System.Windows.Data;

namespace DemiCode.Mvvm.Wpf.Converters
{
    /// <summary>
    /// A converter that compares the source value to the parameter value and returns true if the values are equal, false if not.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class ComparisonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool) && targetType != typeof(bool?))
                throw new ArgumentException("The target type must always be bool or nullable bool", "targetType");

            if (value == null)
                return parameter == null;

            if (parameter == null)
                return false;

            if (value.GetType() != parameter.GetType())
                throw new ArgumentException("The type of 'value' must be same as type of 'parameter'", "value");

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value.GetType() != typeof(bool) && value.GetType() != typeof(bool?)))
                throw new ArgumentException("'value' must always be bool, either true or false", "value");

            var flag = (value != null) && value.Equals(true);

            if (parameter == null)
            {
                if (targetType.IsValueType)
                    throw new ArgumentException(
                        "A null parameter value cannot be assigned to target type " + targetType, "targetType");

                return flag ? null : Binding.DoNothing;
            }

            if (targetType != parameter.GetType())
                throw new ArgumentException("Target type must be the same as type of 'parameter'", "targetType");

            return flag ? parameter : Binding.DoNothing;
        }
    }
}