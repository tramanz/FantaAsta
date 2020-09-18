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

		[DataMember(Name = "Budget")]
		public double Budget { get; set; }

		[DataMember(Name = "Rosa")]
		public List<Giocatore> Rosa { get; set; }

		public double ValoreMedio { get; set; }

		#endregion

		public FantaSquadra(string nome, double budget)
		{
			Nome = nome;
			Rosa = new List<Giocatore>();
			Budget = budget;
		}

		#region Public methods

		public override bool Equals(object obj)
		{
			if (obj is FantaSquadra other)
			{
				bool res = true;

				res &= Nome.Equals(other.Nome, StringComparison.OrdinalIgnoreCase);
				res &= Budget == other.Budget;
				res &= Rosa.Count == other.Rosa.Count;

				if (res)
				{
					foreach (Giocatore giocatore in Rosa)
					{
						res &= other.Rosa.Contains(giocatore) && giocatore.Prezzo == other.Rosa.Find(g => g.Equals(giocatore)).Prezzo;
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

		public object Clone()
		{
			return new FantaSquadra(Nome, Budget) { Rosa = Rosa.Clone() };
		}

		public void AggiungiGiocatore(Giocatore giocatore)
		{
			if (!Rosa.Contains(giocatore))
			{
				Rosa.Add(giocatore);
				Budget -= giocatore.Prezzo;
				AggiornaValore();
			}
		}

		public void RimuoviGiocatore(Giocatore giocatore, double prezzo)
		{
			if (Rosa.Contains(giocatore))
			{
				_ = Rosa.Remove(giocatore);
				Budget += prezzo;
				AggiornaValore();
			}
		}

		#endregion

		#region Private methods

		[OnDeserialized()]
		private void OnDeserialized(StreamingContext streamingContext)
		{
			AggiornaValore();
		}

		private void AggiornaValore()
		{
			double sum = 0;
			foreach (Giocatore giocatore in Rosa)
			{
				sum += giocatore.Quotazione;
			}

			ValoreMedio = sum / Rosa.Count;
		}

		#endregion
	}
}
