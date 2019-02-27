using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DemiCode.Mvvm.Wpf.Converters
{
    /// <summary>
    /// Converter that negates a boolean-to-visibility binding.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        private readonly BooleanToVisibilityConverter _converter = new BooleanToVisibilityConverter();

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a Visibility property");

            return _converter.Convert(!Convert.ToBoolean(value), targetType, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean property");

            var convertedValue = _converter.ConvertBack(value, targetType, parameter, culture);
            return convertedValue != null && !Convert.ToBoolean(convertedValue);
        }
    }
}