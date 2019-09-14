using System;
using System.Globalization;
using System.Windows.Data;

namespace FantaAsta.Converters
{
	class ThreeInitialsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is string str ? str.Substring(0, 3).ToUpper() : string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
