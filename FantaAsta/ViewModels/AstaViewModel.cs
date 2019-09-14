using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
    public class AstaViewModel : BindableBase
    {
		private readonly MainModel m_mainModel;

		private Giocatore m_giocatoreCorrente;
		public Giocatore GiocatoreCorrente
		{
			get { return m_giocatoreCorrente; }
			set { SetProperty(ref m_giocatoreCorrente, value); }
		}

		public List<string> Ruoli => new List<string> { "P", "D", "C", "A" };

		private string m_ruoloSelezionato;
		public string RuoloSelezionato
		{
			get { return m_ruoloSelezionato; }
			set { SetProperty(ref m_ruoloSelezionato, value); EstraiGiocatoreCommand?.RaiseCanExecuteChanged(); }
		}

		public List<string> Squadre => m_mainModel.Squadre.Select(s => s.Nome).ToList();

		private string m_squadraSelezionata;
		public string SquadraSelezionata
		{
			get { return m_squadraSelezionata; }
			set { SetProperty(ref m_squadraSelezionata, value); AssegnaGiocatoreCommand?.RaiseCanExecuteChanged(); }
		}

		private string m_prezzo;
		public string Prezzo
		{
			get { return m_prezzo; }
			set { SetProperty(ref m_prezzo, value); AssegnaGiocatoreCommand?.RaiseCanExecuteChanged(); }
		}

		public AstaViewModel(MainModel mainModel)
		{
			m_mainModel = mainModel;

			EstraiGiocatoreCommand = new DelegateCommand(EstraiGiocatore, AbilitaEstraiGiocatore);
			AssegnaGiocatoreCommand = new DelegateCommand(AssegnaGiocatore, AbilitaAssegnaGiocatore);
		}

		public DelegateCommand EstraiGiocatoreCommand { get; private set; }
		private void EstraiGiocatore()
		{
			GiocatoreCorrente = m_mainModel.EstraiGiocatore((Ruoli)Enum.Parse(typeof(Ruoli), RuoloSelezionato));
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
			else if (m_mainModel.Squadre.Select(s => s.Giocatori).Where(g => g.Contains(GiocatoreCorrente)).Count() > 0)
			{
				MessageBox.Show("Il giocatore selezionato è già assegnato ad una squadra", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (prezzo < GiocatoreCorrente.Quotazione)
			{
				MessageBox.Show("Il prezzo di acquisto non può essere inferiore alla quotazione del giocatore", "ATTENZIONE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var squadra = m_mainModel.Squadre.Where(s => s.Nome.Equals(SquadraSelezionata)).Single();

				bool result = m_mainModel.AggiungiGiocatore(squadra, GiocatoreCorrente, Convert.ToDouble(Prezzo));

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
	}
}
