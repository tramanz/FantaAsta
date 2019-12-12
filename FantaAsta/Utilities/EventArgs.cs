using FantaAsta.Models;

namespace FantaAsta.EventArgs
{
	public class GiocatoreAggiuntoEventArgs : System.EventArgs
	{
		public Giocatore Giocatore { get; }

		public FantaSquadra FantaSquadra { get; }

		public double PrezzoAcquisto { get; }

		public GiocatoreAggiuntoEventArgs(Giocatore giocatore, FantaSquadra fantaSquadra, double prezzo)
		{
			Giocatore = giocatore;
			FantaSquadra = fantaSquadra;
			PrezzoAcquisto = prezzo;
		}
	}

	public class GiocatoreRimossoEventArgs : System.EventArgs
	{
		public Giocatore Giocatore { get; }

		public FantaSquadra FantaSquadra { get; }

		public double PrezzoVendita { get; }

		public GiocatoreRimossoEventArgs(Giocatore giocatore, FantaSquadra fantaSquadra, double prezzo)
		{
			Giocatore = giocatore;
			FantaSquadra = fantaSquadra;
			PrezzoVendita = prezzo;
		}
	}
}