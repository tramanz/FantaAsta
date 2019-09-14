using System;
using System.Globalization;
using System.Windows.Data;
using FantaAsta.Models;

namespace FantaAsta.Converters
{
	public class GiocatoreToNomeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is Giocatore giocatore ? giocatore.Nome : string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
