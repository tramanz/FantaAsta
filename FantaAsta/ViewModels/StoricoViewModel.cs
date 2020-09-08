using System.Collections.ObjectModel;
using Prism.Events;
using FantaAsta.Events;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class StoricoViewModel : BaseViewModel
	{
		#region Properties

		public ObservableCollection<string> Azioni { get; }

		#endregion

		public StoricoViewModel(IEventAggregator eventAggregator, Lega lega) : base(eventAggregator, lega)
		{
			Azioni = new ObservableCollection<string>();

			m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Subscribe(OnGiocatoreAggiunto);
			m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Subscribe(OnGiocatoreRimosso);
			m_eventAggregator.GetEvent<FantaSquadraAggiuntaEvent>().Subscribe(OnFantaSquadraAggiunta);
			m_eventAggregator.GetEvent<FantaSquadraRimossaEvent>().Subscribe(OnFantaSquadraRimossa);
			m_eventAggregator.GetEvent<RoseResettateEvent>().Subscribe(OnRoseResettate);
		}

		#region Private methods

		private void AggiungiAzione(string azione)
		{
			Azioni.Add(string.Empty);
			for (int i = Azioni.Count - 1; i > 0; i--)
			{
				Azioni[i] = Azioni[i - 1];
			}
			Azioni[0] = azione;
		}

		#region Event handlers

		private void OnGiocatoreAggiunto(GiocatoreAggiuntoEventArgs args)
		{
			AggiungiAzione($"{args.Giocatore.Nome} è stato acquistato da {args.FantaSquadra.Nome.ToUpper()} per {args.PrezzoAcquisto} fantamilioni.");
		}

		private void OnGiocatoreRimosso(GiocatoreRimossoEventArgs args)
		{
			AggiungiAzione($"{args.Giocatore.Nome} è stato venduto da {args.FantaSquadra.Nome.ToUpper()} per {args.PrezzoVendita} fantamilioni.");
		}

		private void OnFantaSquadraAggiunta(FantaSquadraEventArgs args)
		{
			AggiungiAzione($"La fantasquadra {args.FantaSquadra.Nome.ToUpper()} è stata aggiunta alla lega.");
		}

		private void OnFantaSquadraRimossa(FantaSquadraEventArgs args)
		{
			AggiungiAzione($"La fantasquadra {args.FantaSquadra.Nome.ToUpper()} è stata rimossa dalla lega.");
		}

		private void OnRoseResettate()
		{
			AggiungiAzione("Le rose sono state resettate.");
		}

		#endregion

		#endregion
	}
}