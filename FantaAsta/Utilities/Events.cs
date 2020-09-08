using Prism.Events;
using FantaAsta.Models;

namespace FantaAsta.Events
{
	#region Events

	public class GiocatoreAggiuntoEvent : PubSubEvent<GiocatoreAggiuntoEventArgs>
	{ }

	public class GiocatoreRimossoEvent : PubSubEvent<GiocatoreRimossoEventArgs>
	{ }

	public class FantaSquadraAggiuntaEvent : PubSubEvent<FantaSquadraEventArgs>
	{ }

	public class FantaSquadraRimossaEvent : PubSubEvent<FantaSquadraEventArgs>
	{ }

	public class RoseResettateEvent : PubSubEvent
	{ }

	public class ModalitàAstaCambiataEvent : PubSubEvent
	{ }

	public class ApriFileDialogEvent : PubSubEvent
	{ }

	public class ListaImportataEvent : PubSubEvent
	{ }

	#endregion

	#region EventArgs

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

		public GiocatoreAggiuntoEventArgs(Giocatore giocatore, FantaSquadra fantaSquadra, double prezzo) : base(fantaSquadra)
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

	#endregion
}