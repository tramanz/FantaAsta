using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FantaAsta.Utilities;

namespace FantaAsta.Converters
{
	public class ThreeInitialsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is string str ? str.Substring(0, 3).ToUpper() : string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string nome = string.Empty;

			if (value is string str)
			{
				nome = Constants.SERIE_A.Select(s => s.Nome).ToList().Find(n => n.StartsWith(str, StringComparison.OrdinalIgnoreCase));
			}

			return !string.IsNullOrEmpty(nome) ? nome : string.Empty;
		}
	}
}
