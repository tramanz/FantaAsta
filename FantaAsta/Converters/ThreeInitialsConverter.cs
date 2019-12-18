using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FantaAsta.Models;
using FantaAsta.Utilities;

namespace FantaAsta.Converters
{
	[ValueConversion(typeof(Squadra), typeof(string))]
	public class ThreeInitialsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is Squadra squadra ? squadra.Nome.Substring(0, 3).ToUpper() : string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is string str ? Constants.SERIE_A.Where(s => s.Nome.StartsWith(str, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() : null;
		}
	}
}
