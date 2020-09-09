using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FantaAsta.Models;

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
			return value is string initials && parameter is List<Squadra> squadre ? squadre.Single(s => s.Nome.StartsWith(initials, StringComparison.OrdinalIgnoreCase)).Nome : string.Empty;
		}
	}
}
