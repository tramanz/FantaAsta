using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using FantaAsta.Models;
using FantaAsta.Enums;

namespace FantaAsta.ViewModels
{
	public class AstaViewModel : BindableBase
	{
		#region Private fields

		private readonly Lega m_lega;

		private string m_ruoloSelezionato;

		private Giocatore m_giocatoreCorrente;

		private string m_squadraSelezionata;

		private string m_prezzo;

		#endregion

		#region Binding

		public List<string> Ruoli => new List<string> { "P", "D", "C", "A" };

		public List<string> Squadre => m_lega?.FantaSquadre.Select(s => s.Nome).ToList();

		public string RuoloSelezionato
		{
			get { return m_ruoloSelezionato; }
			set { SetProperty(ref m_ruoloSelezionato, value); EstraiGiocatoreCommand?.RaiseCanExecuteChanged(); }
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

		public string Prezzo
		{
			get { return m_prezzo; }
			set { SetProperty(ref m_prezzo, value); AssegnaGiocatoreCommand?.RaiseCanExecuteChanged(); }
		}

		#endregion

		public AstaViewModel(Lega lega)
		{
			m_lega = lega;

			EstraiGiocatoreCommand = new DelegateCommand(EstraiGiocatore, AbilitaEstraiGiocatore);
			AssegnaGiocatoreCommand = new DelegateCommand(AssegnaGiocatore, AbilitaAssegnaGiocatore);
		}

		#region Comandi

		public DelegateCommand EstraiGiocatoreCommand { get; private set; }
		private void EstraiGiocatore()
		{
			GiocatoreCorrente = m_lega.EstraiGiocatore((Ruoli)Enum.Parse(typeof(Ruoli), RuoloSelezionato));
		}
		private bool AbilitaEstraiGiocatore()
		{
			return !string.IsNullOrEmpty(RuoloSelezionato);
		}

		public DelegateCommand AssegnaGiocatoreCommand { get; private set; }
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
	}
}
