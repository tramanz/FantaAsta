using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FantaAsta.Utilities;

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

		public double ValoreMedio { get; set; }

		#endregion

		public FantaSquadra(string nome)
		{
			Nome = nome;
			Giocatori = new List<Giocatore>();
			Budget = Constants.BUDGET_ESTIVO;
		}

		#region Public methods

		public void AggiungiGiocatore(Giocatore giocatore)
		{
			if (!Giocatori.Contains(giocatore))
			{
				Giocatori.Add(giocatore);
				Budget -= giocatore.Prezzo;
				AggiornaValore();
			}
		}

		public void RimuoviGiocatore(Giocatore giocatore, double prezzo)
		{
			if (Giocatori.Contains(giocatore))
			{
				Giocatori.Remove(giocatore);
				Budget += prezzo;
				AggiornaValore();
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

		#region Private methods

		[OnDeserialized()]
		private void AggiornaValoreInDeserializzazione(StreamingContext streamingContext)
		{
			AggiornaValore();
		}

		private void AggiornaValore()
		{
			double sum = 0;
			foreach (Giocatore giocatore in Giocatori)
			{
				sum += giocatore.Quotazione;
			}

			ValoreMedio = sum / Giocatori.Count;
		}

		#endregion
	}
}
