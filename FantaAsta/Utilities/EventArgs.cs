using FantaAsta.Models;

namespace FantaAsta.EventArgs
{
	public class FantaSquadraEventArgs : System.EventArgs
	{
		#region Properties

		public FantaSquadra FantaSquadra { get; }

		#endregion

		public FantaSquadraEventArgs(FantaSquadra fantaSquadra)
		{
			FantaSquadra = fantaSquadra;
		}
	}

	public class GiocatoreAggiuntoEventArgs : FantaSquadraEventArgs
	{
		#region Properties

		public Giocatore Giocatore { get; }

		public double PrezzoAcquisto { get; }

		#endregion

		public GiocatoreAggiuntoEventArgs(Giocatore giocatore, FantaSquadra fantaSquadra, double prezzo) : base (fantaSquadra)
		{
			Giocatore = giocatore;
			PrezzoAcquisto = prezzo;
		}
	}

	public class GiocatoreRimossoEventArgs : FantaSquadraEventArgs
	{
		#region Properties
		
		public Giocatore Giocatore { get; }

		public double PrezzoVendita { get; }

		#endregion

		public GiocatoreRimossoEventArgs(Giocatore giocatore, FantaSquadra fantaSquadra, double prezzo) : base(fantaSquadra)
		{
			Giocatore = giocatore;
			PrezzoVendita = prezzo;
		}
	}
}