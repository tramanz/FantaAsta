using System.Collections.Generic;
using Prism.Mvvm;
using FantaAsta.EventArgs;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class StoricoViewModel : BindableBase
	{
		#region Private fields

		private readonly Lega m_lega;

		#endregion

		#region Properties

		public List<string> Azioni { get; }

		#endregion

		public StoricoViewModel(Lega lega)
		{
			m_lega = lega;

			Azioni = new List<string>();

			m_lega.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_lega.GiocatoreRimosso += OnGiocatoreRimosso;
			m_lega.FantaSquadraAggiunta += OnFantaSquadraAggiunta;
			m_lega.FantaSquadraRimossa += OnFantaSquadraRimossa;
			m_lega.RoseResettate += OnRoseResettate;
		}

		#region Private methods

		private void AggiungiAzione(string azione)
		{
			Azioni.Add(azione);
			Azioni.Reverse();
			RaisePropertyChanged(nameof(Azioni));
		}

		#region Event handlers

		private void OnGiocatoreAggiunto(object sender, GiocatoreAggiuntoEventArgs e)
		{
			AggiungiAzione($"{e.Giocatore.Nome} è stato acquistato da {e.FantaSquadra.Nome.ToUpper()} per {e.PrezzoAcquisto} fantamilioni.");
		}

		private void OnGiocatoreRimosso(object sender, GiocatoreRimossoEventArgs e)
		{
			AggiungiAzione($"{e.Giocatore.Nome} è stato venduto da {e.FantaSquadra.Nome.ToUpper()} per {e.PrezzoVendita} fantamilioni.");
		}

		private void OnFantaSquadraAggiunta(object sender, FantaSquadraEventArgs e)
		{
			AggiungiAzione($"La fantasquadra {e.FantaSquadra.Nome.ToUpper()} è stata aggiunta alla lega.");
		}

		private void OnFantaSquadraRimossa(object sender, FantaSquadraEventArgs e)
		{
			AggiungiAzione($"La fantasquadra {e.FantaSquadra.Nome.ToUpper()} è stata rimossa dalla lega.");
		}

		private void OnRoseResettate(object sender, System.EventArgs e)
		{
			AggiungiAzione("Le rose sono state resettate.");
		}

		#endregion

		#endregion
	}
}
