using System;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using Prism.Commands;
using Prism.Services.Dialogs;
using FantaAsta.Models;
using FantaAsta.Enums;

namespace FantaAsta.ViewModels
{
	public class AstaViewModel : BaseViewModel, IDisposable
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private readonly Timer m_timer;

		private Giocatore m_giocatoreCorrente;

		private string m_ruoloSelezionato;

		private bool m_modalitaAstaInvernaleAttiva;

		private int m_repetitions;

		#endregion

		#region Properties

		public List<string> Ruoli => new List<string> { "P", "D", "C", "A" };

		public Giocatore GiocatoreCorrente
		{
			get { return m_giocatoreCorrente; }
			set { SetProperty(ref m_giocatoreCorrente, value); }
		}

		public string RuoloSelezionato
		{
			get { return m_ruoloSelezionato; }
			set { SetProperty(ref m_ruoloSelezionato, value); EstraiGiocatoreCommand?.RaiseCanExecuteChanged(); }
		}

		public bool ModalitaAstaInvernaleAttiva
		{
			get { return m_modalitaAstaInvernaleAttiva; }
			set { SetProperty(ref m_modalitaAstaInvernaleAttiva, value); }
		}

		#region Commands

		public DelegateCommand EstraiGiocatoreCommand { get; }
		public DelegateCommand AssegnaGiocatoreCommand { get; }
		public DelegateCommand CambiaModalitaAstaCommand { get; }

		#endregion

		#endregion

		public AstaViewModel(IDialogService dialogService, Lega lega) : base(lega)
		{
			m_dialogService = dialogService;

			m_lega.GiocatoreAggiunto += OnGiocatoreAggiunto;
			m_lega.GiocatoreRimosso += OnGiocatoreRimosso;
			m_lega.ModalitàAstaCambiata += OnModalitaAstaCambiata;

			m_timer = new Timer { AutoReset = true, Enabled = false, Interval = 50 };
			m_timer.Elapsed += OnTick;

			EstraiGiocatoreCommand = new DelegateCommand(EstraiGiocatore, AbilitaEstraiGiocatore);
			AssegnaGiocatoreCommand = new DelegateCommand(AssegnaGiocatore, AbilitaAssegnaGiocatore);
			CambiaModalitaAstaCommand = new DelegateCommand(CambiaModalitaAsta);
		}

		#region Private methods

		#region Event handlers

		private void OnTick(object sender, ElapsedEventArgs e)
		{
			GiocatoreCorrente = m_lega.EstraiGiocatore((Ruoli)Enum.Parse(typeof(Ruoli), RuoloSelezionato));

			m_repetitions++;

			if (m_repetitions == 6)
			{
				m_timer.Stop();
				m_timer.Interval = 50;
				m_timer.Enabled = false;

				m_repetitions = 0;

				EstraiGiocatoreCommand?.RaiseCanExecuteChanged();
				AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
			}
			else
			{
				m_timer.Interval += 50;
			}
		}

		private void OnModalitaAstaCambiata(object sender, System.EventArgs e)
		{
			ModalitaAstaInvernaleAttiva = m_lega.ModalitaAstaInvernaleAttiva;
		}

		private void OnGiocatoreAggiunto(object sender, EventArgs.GiocatoreAggiuntoEventArgs e)
		{
			AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
		}

		private void OnGiocatoreRimosso(object sender, EventArgs.GiocatoreRimossoEventArgs e)
		{
			AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
		}

		#endregion

		#region Commands

		private void EstraiGiocatore()
		{
			m_lega.ControllaAcquistoGiocatore(GiocatoreCorrente);

			m_timer.Enabled = true;
			m_timer.Start();

			EstraiGiocatoreCommand?.RaiseCanExecuteChanged();
			AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
		}
		private bool AbilitaEstraiGiocatore()
		{
			return !string.IsNullOrEmpty(RuoloSelezionato) && !m_timer.Enabled;
		}

		private void AssegnaGiocatore()
		{
			m_dialogService.ShowDialog("Assegna", new DialogParameters
				{
					{ "Type", DialogType.Popup },
					{ "Giocatore", GiocatoreCorrente }
				}, null);

			AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
		}
		private bool AbilitaAssegnaGiocatore()
		{
			return GiocatoreCorrente != null && !m_timer.Enabled && m_lega.FantaSquadre.SingleOrDefault(s => s.Giocatori.Contains(GiocatoreCorrente)) == null;
		}

		private void CambiaModalitaAsta()
		{
			m_lega.CambiaModalitaAsta();
		}

		#endregion

		#endregion

		#region IDisposable

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// eliminare lo stato gestito (oggetti gestiti).

					m_timer.Dispose();
				}

				// liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
				// impostare campi di grandi dimensioni su Null.

				disposedValue = true;
			}
		}

		// eseguire l'override di un finalizzatore solo se Dispose(bool disposing) include il codice per liberare risorse non gestite.
		// ~AstaViewModel()
		// {
		//   // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
		//   Dispose(false);
		// }

		public void Dispose()
		{
			// Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
			Dispose(true);
			// rimuovere il commento dalla riga seguente se è stato eseguito l'override del finalizzatore.
			// GC.SuppressFinalize(this);
		}

		#endregion
	}
}