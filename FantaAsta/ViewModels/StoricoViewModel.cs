using System.Collections.ObjectModel;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class StoricoViewModel
	{
		#region Private fields

		private readonly Lega m_lega;

		#endregion

		#region Properties

		public ObservableCollection<string> Azioni { get; }

		#endregion

		public StoricoViewModel(Lega lega)
		{
			m_lega = lega;

			Azioni = new ObservableCollection<string>();

			m_lega.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_lega.GiocatoreRimosso += OnGiocatoreRimosso;
			m_lega.RoseResettate += OnRoseResettate;
		}

		#region Private methods

		#region Event handlers

		private void OnGiocatoreAggiunto(object sender, EventArgs.GiocatoreAggiuntoEventArgs e)
		{
			Azioni.Add($"{e.Giocatore.Nome} è stato acquistato da {e.FantaSquadra.Nome.ToUpper()} per {e.PrezzoAcquisto} fantamilioni.");
		}

		private void OnGiocatoreRimosso(object sender, EventArgs.GiocatoreRimossoEventArgs e)
		{
			Azioni.Add($"{e.Giocatore.Nome} è stato venduto da {e.FantaSquadra.Nome.ToUpper()} per {e.PrezzoVendita} fantamilioni.");
		}

		private void OnRoseResettate(object sender, System.EventArgs e)
		{
			Azioni.Add("Le rose sono state resettate.");
		}

		#endregion

		#endregion
	}
}
