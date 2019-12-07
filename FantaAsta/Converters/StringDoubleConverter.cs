using System;
using System.Globalization;
using System.Windows.Data;

namespace FantaAsta.Converters
{
	public class StringDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is double number ? number.ToString() : string.Empty;		
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return double.NaN;
			}
			else if (value is string stringNumber && double.TryParse(stringNumber, NumberStyles.AllowDecimalPoint, null, out double number))
			{
				return number;
			}
			else
			{
				return 0;
			}
		}
	}
}
