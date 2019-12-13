using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using FantaAsta.Models;
using FantaAsta.Enums;
using FantaAsta.EventArgs;

namespace FantaAsta.ViewModels
{
	public class AstaViewModel : BindableBase, IDisposable
	{
		#region Private fields

		private readonly Timer m_timer;

		private readonly Lega m_lega;

		private Giocatore m_giocatoreCorrente;

		private string m_squadraSelezionata;
		private string m_ruoloSelezionato;
		private string m_prezzo;

		private int m_repetitions;

		#endregion

		#region Proprietà

		public List<string> Ruoli => new List<string> { "P", "D", "C", "A" };

		public ObservableCollection<string> Squadre { get; }

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

		#region Commands

		public DelegateCommand EstraiGiocatoreCommand { get; }
		public DelegateCommand AssegnaGiocatoreCommand { get; }

		#endregion

		#endregion

		public AstaViewModel(Lega lega)
		{
			m_lega = lega;
			m_lega.FantaSquadraAggiunta += OnFantaSquadraAggiunta;
			m_lega.FantaSquadraRimossa += OnFantaSquadraRimossa;

			m_timer = new Timer { AutoReset = true, Enabled = false, Interval = 50 };
			m_timer.Elapsed += OnTick;

			Squadre = new ObservableCollection<string>(m_lega?.FantaSquadre.Select(s => s.Nome));

			EstraiGiocatoreCommand = new DelegateCommand(EstraiGiocatore, AbilitaEstraiGiocatore);
			AssegnaGiocatoreCommand = new DelegateCommand(AssegnaGiocatore, AbilitaAssegnaGiocatore);
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
				m_timer.Interval = 75;
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
			Squadre.OrderBy(s => s);
		}

		private void OnFantaSquadraRimossa(object sender, FantaSquadraEventArgs e)
		{
			Squadre.Remove(e.FantaSquadra.Nome);
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
				MessageBox.Show("Inserire un numero", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (m_lega.FantaSquadre.Select(s => s.Giocatori).Where(g => g.Contains(GiocatoreCorrente)).Count() > 0)
			{
				MessageBox.Show("Il giocatore selezionato è già assegnato ad una squadra", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (prezzo < GiocatoreCorrente.Quotazione)
			{
				MessageBox.Show("Il prezzo di acquisto non può essere inferiore alla quotazione del giocatore", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var squadra = m_lega.FantaSquadre.Where(s => s.Nome.Equals(SquadraSelezionata)).Single();

				bool result = m_lega.AggiungiGiocatore(squadra, GiocatoreCorrente, Convert.ToDouble(Prezzo));

				string msg = result ? "Giocatore aggiunto" : "Il giocatore non può essere aggiunto";
				string capt = result ? "OPERAZIONE COMPLETATA" : "OPERAZIONE FALLITA";
				MessageBoxImage img = result ? MessageBoxImage.Information : MessageBoxImage.Error;

				MessageBox.Show(msg, capt, MessageBoxButton.OK, img);
			}
		}
		private bool AbilitaAssegnaGiocatore()
		{
			return GiocatoreCorrente != null && !string.IsNullOrEmpty(SquadraSelezionata) && !string.IsNullOrEmpty(Prezzo);
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
