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

		public string Budget
		{
			get { return m_opzioniCopiate.BudgetIniziale.ToString(); }
			set
			{
				if (double.TryParse(value, NumberStyles.Integer, null, out double number))
				{
					m_opzioniCopiate.BudgetIniziale = number;

					RaisePropertyChanged(nameof(Budget));
				}
			}
		}

		#region Commands

		public DelegateCommand DiminuisciBudgetCommand { get; }
		public DelegateCommand AumentaBudgetCommand { get; }

		#endregion

		#endregion

		public OpzioniViewModel(IEventAggregator eventAggregator, IDialogService dialogService, Lega lega) : base(eventAggregator, lega)
		{
			m_dialogService = dialogService;

			m_opzioniSalvate = m_lega.Opzioni;
			m_opzioniSalvate.Copia(ref m_opzioniCopiate);

			DiminuisciBudgetCommand = new DelegateCommand(DiminuisciBudget);
			AumentaBudgetCommand = new DelegateCommand(AumentaBudget);
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

		private void DiminuisciBudget()
		{
			Budget = (double.Parse(Budget) - BUDGET_STEP).ToString();
		}

		private void AumentaBudget()
		{
			Budget = (double.Parse(Budget) + BUDGET_STEP).ToString();
		}

		private void Salva()
		{
			m_opzioniCopiate.Copia(ref m_opzioniSalvate);
		}

		private bool Valida()
		{
			bool res = true;

			res &= double.TryParse(Budget, NumberStyles.Integer, null, out double _);

			return res;
		}

		private void Conferma()
		{
			if (Valida())
			{
				if (!m_opzioniCopiate.Equals(m_opzioniSalvate))
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