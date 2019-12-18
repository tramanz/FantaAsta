using System;
using System.Globalization;
using System.Windows.Data;

namespace FantaAsta.Converters
{
	[ValueConversion(typeof(double), typeof(string))]
	public class StringDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is double dou ? dou.ToString() : string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is string str && double.TryParse(str, NumberStyles.AllowDecimalPoint, null, out double number) ? number : double.NaN;
		}
	}
}
