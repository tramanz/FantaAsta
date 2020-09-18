using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FantaAsta.Utilities;

namespace FantaAsta.Models
{
	[DataContract(Name = "FantaSquadra", Namespace = "")]
	public class FantaSquadra : ICloneable
	{
		#region Properties

		[DataMember(Name = "Nome")]
		public string Nome { get; set; }

		[DataMember(Name = "Giocatori")]
		public List<Giocatore> Giocatori { get; set; }

		[DataMember(Name = "Budget")]
		public double Budget { get; set; }

		public double ValoreMedio { get; set; }

		#endregion

		public FantaSquadra(string nome, double budget)
		{
			Nome = nome;
			Giocatori = new List<Giocatore>();
			Budget = budget;
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
				_ = Giocatori.Remove(giocatore);
				Budget += prezzo;
				AggiornaValore();
			}
		}

		public object Clone()
		{
			return new FantaSquadra(Nome, Budget) { Giocatori = Giocatori.Clone() };
		}

		public override bool Equals(object obj)
		{
			if (obj is FantaSquadra other)
			{
				bool res = true;

				res &= Nome.Equals(other.Nome, StringComparison.OrdinalIgnoreCase);
				res &= Budget == other.Budget;
				res &= Giocatori.Count == other.Giocatori.Count;

				if (res)
				{
					foreach (Giocatore giocatore in Giocatori)
					{
						res &= other.Giocatori.Contains(giocatore) && giocatore.Prezzo == other.Giocatori.Find(g => g.Equals(giocatore)).Prezzo;
					} 
				}

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
