using System;
using System.Windows.Media;

namespace FantaAsta.Models
{
	public class Squadra
	{
		#region Public fields

		public string Nome { get; }

		public SolidColorBrush Colore1 { get; }
		public SolidColorBrush Colore2 { get; }

		#endregion

		public Squadra(string nome, string hexColor1, string hexColor2)
		{
			Nome = nome;
			Colore1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor1));
			Colore2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor2));
		}

		#region Public methods

		public override bool Equals(object obj)
		{
			if (obj is Squadra other)
			{
				bool res = true;

				res &= Nome.Equals(other.Nome, StringComparison.OrdinalIgnoreCase);

				return res;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
