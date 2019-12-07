using System;
using System.Collections.Generic;

namespace FantaAsta.Models
{
	public class FantaSquadra
	{ 
		#region Properties

		public string Nome { get; }
		public List<Giocatore> Giocatori { get; }
		public double Budget { get; private set; }

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

		public void RimuoviGiocatore(Giocatore giocatore)
		{
			if (Giocatori.Contains(giocatore))
			{
				Giocatori.Remove(giocatore);
				Budget += giocatore.Prezzo;
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
