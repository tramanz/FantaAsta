using FantaAsta.Models;

namespace FantaAsta.EventArgs
{
	public class GiocatoreEventArgs : System.EventArgs
	{
		public Giocatore Giocatore { get; }

		public FantaSquadra FantaSquadra { get; }

		public GiocatoreEventArgs(Giocatore giocatore, FantaSquadra fantaSquadra)
		{
			Giocatore = giocatore;
			FantaSquadra = fantaSquadra;
		}
	}
}