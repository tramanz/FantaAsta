using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FantaAsta.Utilities;

namespace FantaAsta.Resources.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class ThreeInitialsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is string nome ? nome.Substring(0, 3).ToUpper() : string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is string initials ? Constants.SERIE_A.Select(s => s.Nome).FirstOrDefault(s => s.StartsWith(initials, StringComparison.OrdinalIgnoreCase)) : string.Empty;
		}
	}
}
