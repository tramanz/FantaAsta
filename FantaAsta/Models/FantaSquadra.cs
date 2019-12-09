using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FantaAsta.Models
{
	[DataContract(Name = "FantaSquadra", Namespace = "")]
	public class FantaSquadra
	{ 
		#region Properties

		[DataMember(Name = "Nome")]
		public string Nome { get; set;  }

		[DataMember(Name = "Giocatori")]
		public List<Giocatore> Giocatori { get; set; }

		[DataMember(Name = "Budget")]
		public double Budget { get; set; }

		#endregion

		public FantaSquadra(string nome)
		{
			Nome = nome;
			Giocatori = new List<Giocatore>();
			Budget = 500;
		}

		#region Public methods

		public void AggiungiGiocatore(Giocatore giocatore)
		{
			if (!Giocatori.Contains(giocatore))
			{
				Giocatori.Add(giocatore);
				Budget -= giocatore.Prezzo;
			}
		}

		public void RimuoviGiocatore(Giocatore giocatore, double prezzo)
		{
			if (Giocatori.Contains(giocatore))
			{
				Giocatori.Remove(giocatore);
				Budget += prezzo;
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is FantaSquadra squadra))
				return false;
			return squadra.Nome.Equals(Nome, StringComparison.Ordinal);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
