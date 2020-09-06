using System.Runtime.Serialization;
using System.Windows.Media;

namespace FantaAsta.Models
{
	[DataContract(Name = "Squadra", Namespace = "")]
	public class Squadra
	{
		#region Public fields

		[DataMember(Name = "Nome")]
		public string Nome { get; set; }

		[DataMember(Name = "Colore1RGB")]
		public string Colore1RGB { get; set; }
		public SolidColorBrush Colore1 { get; private set; }

		[DataMember(Name = "Colore2RGB")]
		public string Colore2RGB { get; set; }
		public SolidColorBrush Colore2 { get; private set; }

		#endregion

		public Squadra(string nome, string hexColor1, string hexColor2)
		{
			Nome = nome;
			Colore1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor1));
			Colore2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor2));
		}

		#region Private methods

		[OnSerializing()]
		private void OnSerializing(StreamingContext context)
		{
			Colore1RGB = Colore1.Color.ToString();
			Colore2RGB = Colore2.Color.ToString();
		}

		[OnDeserialized()]
		private void OnDeserialized(StreamingContext context)
		{
			Colore1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colore1RGB));
			Colore2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colore2RGB));
		}

		#endregion
	}
}
