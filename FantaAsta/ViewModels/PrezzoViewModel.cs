using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class PrezzoViewModel : BindableBase, IDialogAware
	{
		#region Private fields

		private readonly Lega m_lega;

		private FantaSquadra m_squadra;

		private Giocatore m_giocatore;

		private double m_prezzo;

		private string m_movimento;

		#endregion

		#region Properties

		public string Title => "Inserisci prezzo";

		public string Movimento
		{
			get { return m_movimento; }
			set { SetProperty(ref m_movimento, value); }
		}

		public double Prezzo
		{
			get { return m_prezzo; }
			set { SetProperty(ref m_prezzo, value); ConfermaCommand?.RaiseCanExecuteChanged(); }
		}

		#region Commands

		public DelegateCommand ConfermaCommand { get; }
		public DelegateCommand AnnullaCommand { get; }

		#endregion

		#endregion

		#region Events

		public event Action<IDialogResult> RequestClose;

		public event EventHandler SelectNameTextBox;

		#endregion

		public PrezzoViewModel(Lega lega)
		{
			m_lega = lega;

			ConfermaCommand = new DelegateCommand(Conferma, AbilitaConferma);
			AnnullaCommand = new DelegateCommand(Chiudi);
		}

		#region Public methods

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed()
		{ }

		public void OnDialogOpened(IDialogParameters parameters)
		{
			m_giocatore = parameters.GetValue<Giocatore>("Giocatore");
			m_squadra = parameters.GetValue<FantaSquadra>("FantaSquadra");
			Movimento = $"Inserisci il prezzo di {parameters.GetValue<string>("Movimento")}";
		}

		#endregion

		#region Private methods

		#region Commands

		private void Conferma()
		{
			if (double.IsNaN(Prezzo))
			{
				MessageBox.Show("Inserire un prezzo", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);

				SelectNameTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else
			{ 
				if (Movimento.Remove(0, 23).Equals("acquisto", StringComparison.OrdinalIgnoreCase))
				{
					AcquistaGiocatore();
				}
				else if (Movimento.Remove(0, 23).Equals("vendita", StringComparison.OrdinalIgnoreCase))
				{
					VendiGiocatore();
				}

				Chiudi();
			}
		}

		private bool AbilitaConferma()
		{
			return !double.IsNaN(Prezzo);
		}

		private void Chiudi()
		{
			RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
		}

		private void AcquistaGiocatore()
		{
			if (Prezzo < m_giocatore.Quotazione)
			{
				MessageBox.Show("Il prezzo di acquisto non può essere inferiore alla quotazione del giocatore.", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);

				SelectNameTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else
			{
				bool result = m_lega.AggiungiGiocatore(m_squadra, m_giocatore, Prezzo);

				string msg = result ? "Giocatore aggiunto" : "Il giocatore non può essere aggiunto";
				string capt = result ? "OPERAZIONE COMPLETATA" : "OPERAZIONE FALLITA";
				MessageBoxImage img = result ? MessageBoxImage.Information : MessageBoxImage.Error;

				MessageBox.Show(msg, capt, MessageBoxButton.OK, img);
			}
		}

		private void VendiGiocatore()
		{
			if (Prezzo <= 0)
			{
				MessageBox.Show("Il prezzo di vendita non può essere minore o uguale a 0.", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);

				SelectNameTextBox?.Invoke(this, System.EventArgs.Empty);
			}
			else
			{
				m_lega.RimuoviGiocatore(m_squadra, m_giocatore, Prezzo);

				MessageBox.Show("Giocatore rimosso", "OPERAZIONE COMPLETATA", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		#endregion

		#endregion
	}

}
