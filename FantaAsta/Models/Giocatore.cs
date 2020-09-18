using System;
using System.Runtime.Serialization;
using FantaAsta.Enums;

namespace FantaAsta.Models
{
	[DataContract(Name = "Giocatore", Namespace = "")]
	public class Giocatore : ICloneable
	{
		#region Properties

		[DataMember(Name = "ID")] 
		public int ID { get; set; }

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

		public bool InLista { get; set; } = true;

		public bool Scartato { get; set; } = false;

		#endregion

		public Giocatore(int id, Ruoli ruolo, string nome, Squadra squadra, double quotazione)
		{
			ID = id;
			Ruolo = ruolo;
			Nome = nome;
			Squadra = squadra;
			Quotazione = quotazione;
		}

		#region Public methods

		public override bool Equals(object obj)
		{
			if (obj is Giocatore other)
			{
				bool res = true;

				res &= ID == other.ID;
				res &= Ruolo == other.Ruolo;
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

		public object Clone()
		{
			return new Giocatore(ID, Ruolo, Nome, Squadra, Quotazione) { Prezzo = Prezzo };
		}

		#endregion
	}
}