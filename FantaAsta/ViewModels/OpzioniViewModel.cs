using System.Globalization;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using FantaAsta.Enums;
using FantaAsta.Events;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class OpzioniViewModel : BaseDialogViewModel
	{
		#region Constants

		private const int BUDGET_STEP = 10;

		#endregion

		#region Private fields

		private readonly IDialogService m_dialogService;

		private Opzioni m_opzioniSalvate;
		private Opzioni m_opzioniCopiate;

		#endregion

		#region Properties

		public string BudgetIniziale
		{
			get { return m_opzioniCopiate.BudgetIniziale.ToString(); }
			set
			{
				if (double.TryParse(value, NumberStyles.Integer, null, out double number))
				{
					m_opzioniCopiate.BudgetIniziale = number;

					RaisePropertyChanged(nameof(BudgetIniziale));
				}
			}
		}

		public string BudgetAggiuntivo
		{
			get { return m_opzioniCopiate.BudgetAggiuntivo.ToString(); }
			set
			{
				if (double.TryParse(value, NumberStyles.Integer, null, out double number))
				{
					m_opzioniCopiate.BudgetAggiuntivo = number;

					RaisePropertyChanged(nameof(BudgetAggiuntivo));
				}
			}
		}

		#region Commands

		public DelegateCommand DiminuisciBudgetInizialeCommand { get; }
		public DelegateCommand AumentaBudgetInizialeCommand { get; }

		public DelegateCommand DiminuisciBudgetAggiuntivoCommand { get; }
		public DelegateCommand AumentaBudgetAggiuntivoCommand { get; }

		#endregion

		#endregion

		public OpzioniViewModel(IEventAggregator eventAggregator, IDialogService dialogService, Lega lega) : base(eventAggregator, lega)
		{
			m_dialogService = dialogService;

			m_opzioniSalvate = m_lega.Opzioni;
			m_opzioniSalvate.Copia(ref m_opzioniCopiate);

			DiminuisciBudgetInizialeCommand = new DelegateCommand(DiminuisciBudgetIniziale);
			AumentaBudgetInizialeCommand = new DelegateCommand(AumentaBudgetIniziale);

			DiminuisciBudgetAggiuntivoCommand = new DelegateCommand(DiminuisciBudgetAggiuntivo);
			AumentaBudgetAggiuntivoCommand = new DelegateCommand(AumentaBudgetAggiuntivo);
		}

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{ }

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			Title = $"Opzioni";
		}

		protected override void InizializzaBottoni(IDialogParameters parameters)
		{
			Buttons.Add(new DialogButton("Conferma", new DelegateCommand(Conferma)));
			Buttons.Add(new DialogButton("Annulla", new DelegateCommand(Chiudi)));
		}

		#endregion

		#region Private methods

		private void DiminuisciBudgetIniziale()
		{
			BudgetIniziale = (double.Parse(BudgetIniziale) - BUDGET_STEP).ToString();
		}

		private void AumentaBudgetIniziale()
		{
			BudgetIniziale = (double.Parse(BudgetIniziale) + BUDGET_STEP).ToString();
		}

		private void DiminuisciBudgetAggiuntivo()
		{
			BudgetAggiuntivo = (double.Parse(BudgetAggiuntivo) - BUDGET_STEP).ToString();
		}

		private void AumentaBudgetAggiuntivo()
		{
			BudgetAggiuntivo = (double.Parse(BudgetAggiuntivo) + BUDGET_STEP).ToString();
		}

		private void Salva()
		{
			m_opzioniCopiate.Copia(ref m_opzioniSalvate);
		}

		private bool Valida()
		{
			bool res = true;

			res &= double.TryParse(BudgetIniziale, NumberStyles.Integer, null, out double _);
			res &= double.TryParse(BudgetAggiuntivo, NumberStyles.Integer, null, out double _);

			return res;
		}

		private void Conferma()
		{
			if (Valida())
			{
				if (!m_opzioniCopiate.Equals(m_opzioniSalvate))
				{
					if (m_opzioniCopiate.BudgetIniziale != m_opzioniSalvate.BudgetIniziale)
					{
						ButtonResult result = m_dialogService.ShowMessage("Le rose si resetteranno. Sei sicuro di voler salvare le modifiche?", MessageType.Warning);
						if (result == ButtonResult.Yes)
						{
							Salva();
							m_eventAggregator.GetEvent<OpzioniModificateEvent>().Publish();
							Chiudi();
						}
					}
					else
					{
						Salva();
						Chiudi();
					}
				}
				else
				{
					Chiudi();
				}
			}
			else
			{
				_ = m_dialogService.ShowMessage("Valori non corretti", MessageType.Error);
			}
		}

		private void Chiudi()
		{
			RaiseRequestClose(new DialogResult(ButtonResult.OK));
		}

		#endregion
	}
}