using System;
using System.Timers;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using FantaAsta.Constants;
using FantaAsta.Enums;
using FantaAsta.Events;
using FantaAsta.Models;

namespace FantaAsta.ViewModels
{
	public class AstaViewModel : BaseViewModel, IDisposable
	{
		#region Constants

		private const int MAX_REPETITIONS = 6;

		#endregion

		#region Private fields

		private readonly IDialogService m_dialogService;

		private readonly Timer m_timer;

		private int m_repetitions;

		private Giocatore m_giocatoreCorrente;

		private Ruoli m_ruoloSelezionato;
		private bool m_isPortieriSelected;
		private bool m_isDifensoriSelected;
		private bool m_isCentrocampistiSelected;
		private bool m_isAttaccantiSelected;

		private bool m_isAstaEstivaSelected;
		private bool m_isAstaInvernaleSelected;

		#endregion

		#region Properties

		public Giocatore GiocatoreCorrente
		{
			get { return m_giocatoreCorrente; }
			set { _ = SetProperty(ref m_giocatoreCorrente, value); }
		}

		public bool IsPortieriSelected
		{
			get { return m_isPortieriSelected; }
			set 
			{
				_ = SetProperty(ref m_isPortieriSelected, value);
				if (value)
				{
					m_ruoloSelezionato = Ruoli.P;
				}
			}
		}

		public bool IsDifensoriSelected
		{
			get { return m_isDifensoriSelected; }
			set
			{
				_ = SetProperty(ref m_isDifensoriSelected, value);
				if (value)
				{
					m_ruoloSelezionato = Ruoli.D;
				}
			}
		}

		public bool IsCentrocampistiSelected
		{
			get { return m_isCentrocampistiSelected; }
			set
			{
				_ = SetProperty(ref m_isCentrocampistiSelected, value);
				if (value)
				{
					m_ruoloSelezionato = Ruoli.C;
				}
			}
		}

		public bool IsAttaccantiSelected
		{
			get { return m_isAttaccantiSelected; }
			set
			{
				_ = SetProperty(ref m_isAttaccantiSelected, value);
				if (value)
				{
					m_ruoloSelezionato = Ruoli.A;
				}
			}
		}

		public bool IsAstaEstivaSelected
		{
			get { return !m_lega.ModalitaAstaInvernaleAttiva; }
			set 
			{
				_ = SetProperty(ref m_isAstaEstivaSelected, value);
				if (value)
				{
					m_lega.CambiaModalitaAsta();
				}
			}
		}

		public bool IsAstaInvernaleSelected
		{ 
			get { return m_lega.ModalitaAstaInvernaleAttiva; }
			set
			{
				_ = SetProperty(ref m_isAstaInvernaleSelected, value);
				if (value)
				{
					m_lega.CambiaModalitaAsta();
				}
			}
		}

		public bool BottoniAttivi { get { return !m_timer.Enabled; } }

		#region Commands

		public DelegateCommand EstraiGiocatoreCommand { get; }
		public DelegateCommand AssegnaGiocatoreCommand { get; }

		#endregion

		#endregion

		public AstaViewModel(IEventAggregator eventAggregator, IDialogService dialogService, Lega lega) : base(eventAggregator, lega)
		{
			m_dialogService = dialogService;

			m_eventAggregator.GetEvent<GiocatoreAggiuntoEvent>().Subscribe(OnGiocatoreAggiunto);
			m_eventAggregator.GetEvent<GiocatoreRimossoEvent>().Subscribe(OnGiocatoreRimosso);

			m_timer = new Timer { AutoReset = true, Enabled = false, Interval = 50 };
			m_timer.Elapsed += OnTick;

			IsPortieriSelected = true;

			EstraiGiocatoreCommand = new DelegateCommand(EstraiGiocatore, AbilitaEstraiGiocatore);
			AssegnaGiocatoreCommand = new DelegateCommand(AssegnaGiocatore, AbilitaAssegnaGiocatore);
		}
		
		#region Private methods

		#region Event handlers

		private void OnTick(object sender, ElapsedEventArgs e)
		{
			GiocatoreCorrente = m_lega.EstraiGiocatore(m_ruoloSelezionato);

			if (++m_repetitions == MAX_REPETITIONS)
			{
				m_timer.Stop();
				m_timer.Interval = 50;
				m_timer.Enabled = false;

				m_repetitions = 0;

				EstraiGiocatoreCommand?.RaiseCanExecuteChanged();
				AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
			
				RaisePropertyChanged(nameof(BottoniAttivi));
			}
			else
			{
				m_timer.Interval += 50;
			}
		}

		private void OnGiocatoreAggiunto(GiocatoreAggiuntoEventArgs args)
		{
			AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
		}

		private void OnGiocatoreRimosso(GiocatoreRimossoEventArgs args)
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

			RaisePropertyChanged(nameof(BottoniAttivi));
		}
		private bool AbilitaEstraiGiocatore()
		{
			return BottoniAttivi;
		}

		private void AssegnaGiocatore()
		{
			m_dialogService.ShowDialog(CommonConstants.ASSEGNA_DIALOG, new DialogParameters
				{
					{ "Type", DialogType.Popup },
					{ "Giocatore", GiocatoreCorrente }
				}, null);

			AssegnaGiocatoreCommand?.RaiseCanExecuteChanged();
		}
		private bool AbilitaAssegnaGiocatore()
		{
			return BottoniAttivi && GiocatoreCorrente != null;
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