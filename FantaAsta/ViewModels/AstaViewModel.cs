using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Timers;
using Prism.Commands;
using Prism.Services.Dialogs;
using FantaAsta.Models;
using FantaAsta.Enums;
using FantaAsta.EventArgs;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class AstaViewModel : BaseViewModel, IDisposable
	{
		#region Private fields

		private readonly IDialogService m_dialogService;

		private readonly Timer m_timer;

		private ObservableCollection<string> m_squadre;

		private Giocatore m_giocatoreCorrente;

		private string m_squadraSelezionata;
		private string m_ruoloSelezionato;
		private string m_prezzo;

		private bool m_modalitaAstaInvernaleAttiva;

		private int m_repetitions;

		#endregion

		#region Properties

		public List<string> Ruoli => new List<string> { "P", "D", "C", "A" };

		public ObservableCollection<string> Squadre
		{
			get { return m_squadre; }
			set { SetProperty(ref m_squadre, value); }
		}

		public Giocatore GiocatoreCorrente
		{
			get { return m_giocatoreCorrente; }
			set { SetProperty(ref m_giocatoreCorrente, value); }
		}

		public string SquadraSelezionata
		{
			get { return m_squadraSelezionata; }
			set { SetProperty(ref m_squadraSelezionata, value); AssegnaGiocatoreCommand?.RaiseCanExecuteChanged(); }
		}

		public string RuoloSelezionato
		{
			get { return m_ruoloSelezionato; }
			set { SetProperty(ref m_ruoloSelezionato, value); EstraiGiocatoreCommand?.RaiseCanExecuteChanged(); }
		}

		public string Prezzo
		{
			get { return m_prezzo; }
			set { SetProperty(ref m_prezzo, value); AssegnaGiocatoreCommand?.RaiseCanExecuteChanged(); }
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

			m_lega.FantaSquadraAggiunta += OnFantaSquadraAggiunta;
			m_lega.FantaSquadraRimossa += OnFantaSquadraRimossa;
			m_lega.ModalitàAstaCambiata += OnModalitaAstaCambiata;

			m_timer = new Timer { AutoReset = true, Enabled = false, Interval = 50 };
			m_timer.Elapsed += OnTick;

			Squadre = new ObservableCollection<string>(m_lega?.FantaSquadre.Select(s => s.Nome).OrderBy(s => s));

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
			}
			else
			{
				m_timer.Interval += 50;
			}
		}

		private void OnFantaSquadraAggiunta(object sender, FantaSquadraEventArgs e)
		{
			Squadre.Add(e.FantaSquadra.Nome);
			Squadre = new ObservableCollection<string>(Squadre.OrderBy(s => s));
		}

		private void OnFantaSquadraRimossa(object sender, FantaSquadraEventArgs e)
		{
			Squadre.Remove(e.FantaSquadra.Nome);
		}

		private void OnModalitaAstaCambiata(object sender, System.EventArgs e)
		{
			ModalitaAstaInvernaleAttiva = m_lega.ModalitaAstaInvernaleAttiva;
		}

		#endregion

		#region Commands

		private void EstraiGiocatore()
		{
			m_timer.Enabled = true;
			m_timer.Start();

			EstraiGiocatoreCommand?.RaiseCanExecuteChanged();
		}
		private bool AbilitaEstraiGiocatore()
		{
			return !string.IsNullOrEmpty(RuoloSelezionato) && !m_timer.Enabled;
		}

		private void AssegnaGiocatore()
		{
			if (!double.TryParse(Prezzo, NumberStyles.AllowDecimalPoint, null, out double prezzo))
			{
				m_dialogService.ShowMessage("Inserire un numero", MessageType.Error);
			}
			else if (m_lega.FantaSquadre.Select(s => s.Giocatori).Where(g => g.Contains(GiocatoreCorrente)).Count() > 0)
			{
				m_dialogService.ShowMessage("Il giocatore selezionato è già assegnato ad una squadra", MessageType.Error);
			}
			else if (prezzo < GiocatoreCorrente.Quotazione)
			{
				m_dialogService.ShowMessage("Il prezzo di acquisto non può essere inferiore alla quotazione del giocatore", MessageType.Error);
			}
			else
			{
				var squadra = m_lega.FantaSquadre.Where(s => s.Nome.Equals(SquadraSelezionata)).Single();

				bool result = m_lega.AggiungiGiocatore(squadra, GiocatoreCorrente, Convert.ToDouble(Prezzo));

				string msg = result ? "Giocatore aggiunto" : "Il giocatore non può essere aggiunto";
				MessageType type = result ? MessageType.Notification : MessageType.Error;

				m_dialogService.ShowMessage(msg, type);
			}
		}
		private bool AbilitaAssegnaGiocatore()
		{
			return GiocatoreCorrente != null && !string.IsNullOrEmpty(SquadraSelezionata) && !string.IsNullOrEmpty(Prezzo);
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