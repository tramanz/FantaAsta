using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class ModificaViewModel : BindableBase, IDialogAware
	{
		private readonly MainModel m_mainModel;
		private Squadra m_squadra;

		#region IDialogAware

		public string Title { get; set; }

		public event Action<IDialogResult> RequestClose;

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed()
		{
		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
			string nome = parameters.GetValue<string>("squadra");

			m_squadra = m_mainModel.Squadre.Where(s => s.Nome.Equals(nome)).Single();

			foreach (var giocatore in m_squadra.Giocatori)
			{
				Rosa.Add(giocatore.Nome);
			}
		}

		#endregion

		public ObservableCollection<string> Rosa{ get; private set; }
		private string m_giocatoreSelezionato;
		public string GiocatoreSelezionato
		{
			get { return m_giocatoreSelezionato; }
			set { SetProperty(ref m_giocatoreSelezionato, value); RimuoviCommand?.RaiseCanExecuteChanged(); }
		}

		public ObservableCollection<string> Svincolati { get; private set; }
		private string m_svincolatoSelezionato;
		public string SvincolatoSelezionato
		{
			get { return m_svincolatoSelezionato; }
			set { SetProperty(ref m_svincolatoSelezionato, value); AggiungiCommand?.RaiseCanExecuteChanged(); }
		}

		private string m_prezzo;
		public string Prezzo
		{
			get { return m_prezzo; }
			set { SetProperty(ref m_prezzo, value); AggiungiCommand?.RaiseCanExecuteChanged(); }
		}

		public ModificaViewModel(MainModel mainModel)
		{
			m_mainModel = mainModel;

			Svincolati = new ObservableCollection<string>(m_mainModel.ListaPortieri.Concat(m_mainModel.ListaDifensori).Concat(m_mainModel.ListaCentrocampisti).Concat(m_mainModel.ListaAttaccanti).OrderBy(g => g.Nome).Select(s => s.Nome));
			Rosa = new ObservableCollection<string>();

			AggiungiCommand = new DelegateCommand(Aggiungi, AbilitaAggiungi);
			RimuoviCommand = new DelegateCommand(Rimuovi, AbilitaRimuovi);
			ChiudiCommand = new DelegateCommand(Chiudi);
		}

		public DelegateCommand AggiungiCommand { get; private set; }
		private void Aggiungi()
		{
			var nome = SvincolatoSelezionato;
			var svincolato = m_mainModel.ListaPortieri.Concat(m_mainModel.ListaDifensori).Concat(m_mainModel.ListaCentrocampisti).Concat(m_mainModel.ListaAttaccanti).Where(g => g.Nome.Equals(nome)).Single();

			if (!double.TryParse(Prezzo, NumberStyles.AllowDecimalPoint, null, out double prezzo))
			{
				MessageBox.Show("Inserire un numero", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (m_mainModel.Squadre.Select(s => s.Giocatori).Where(g => g.Contains(svincolato)).Count() > 0)
			{
				MessageBox.Show("Il giocatore selezionato è già assegnato ad una squadra", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (prezzo < svincolato.Quotazione)
			{
				MessageBox.Show("Il prezzo di acquisto non può essere inferiore alla quotazione del giocatore", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				bool result = m_mainModel.AggiungiGiocatore(m_squadra, svincolato, Convert.ToDouble(Prezzo));

				string msg = result ? "Giocatore aggiunto" : "Il giocatore non può essere aggiunto";
				string capt = result ? "OPERAZIONE COMPLETATA" : "OPERAZIONE FALLITA";
				MessageBoxImage img = result ? MessageBoxImage.Information : MessageBoxImage.Error;

				Svincolati.Remove(nome);

				Rosa.Add(nome);

				MessageBox.Show(msg, capt, MessageBoxButton.OK, img);
			}
		}
		private bool AbilitaAggiungi()
		{
			return SvincolatoSelezionato != null && !string.IsNullOrEmpty(Prezzo);
		}

		public DelegateCommand RimuoviCommand { get; private set; }
		private void Rimuovi()
		{
			var giocatore = m_squadra.Giocatori.Where(g => g.Nome.Equals(GiocatoreSelezionato)).Single();

			m_mainModel.RimuoviGiocatore(m_squadra, giocatore);

			Rosa.Remove(GiocatoreSelezionato);

			Svincolati.Add(GiocatoreSelezionato);
			Svincolati.OrderBy(s => s);

			MessageBox.Show("Giocatore rimosso", "OPERAZIONE COMPLETATA", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		private bool AbilitaRimuovi()
		{
			return GiocatoreSelezionato != null;
		}

		public DelegateCommand ChiudiCommand { get; private set; }
		private void Chiudi()
		{
			RequestClose?.Invoke(new DialogResult(ButtonResult.OK));	
		}
	}
}
