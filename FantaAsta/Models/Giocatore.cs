using System;
using System.Runtime.Serialization;
using FantaAsta.Enums;
using FantaAsta.Utilities;

namespace FantaAsta.Models
{
	[DataContract(Name = "Giocatore", Namespace = "")]
	public class Giocatore
	{
		#region Properties

		public int ID { get; }

		[DataMember(Name = "Ruolo")]
		public Ruoli Ruolo { get; set; }

		[DataMember(Name = "Nome")]
		public string Nome { get; set; }

		[DataMember(Name = "Squadra")]
		public Squadra Squadra { get; set;  }

		[DataMember(Name = "Quotazione")]
		public double Quotazione { get; set; }

		[DataMember(Name = "Prezzo")]
		public double Prezzo { get; set; }

		#endregion
		public Giocatore(int id, Ruoli ruolo, string nome, string squadra, double quotazione)
		{
			ID = id;
			Ruolo = ruolo;
			Nome = nome;
			Squadra = Constants.SERIE_A.Find(s => s.Nome.Equals(squadra, StringComparison.OrdinalIgnoreCase));
			Quotazione = quotazione;
		}
	}
}
