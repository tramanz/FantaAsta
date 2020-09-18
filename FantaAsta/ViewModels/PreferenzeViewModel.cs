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
	public class PreferenzeViewModel : BaseDialogViewModel
	{
		#region Constants

		private const int BUDGET_STEP = 10;

		#endregion

		#region Private fields

		private readonly IDialogService m_dialogService;

		private Preferenze m_copiaPreferenze;

		#endregion

		#region Properties

		public string BudgetIniziale
		{
			get { return m_copiaPreferenze.BudgetIniziale.ToString(); }
			set
			{
				if (double.TryParse(value, NumberStyles.Integer, null, out double number))
				{
					m_copiaPreferenze.BudgetIniziale = number;

					RaisePropertyChanged(nameof(BudgetIniziale));
				}
			}
		}

		public string BudgetAggiuntivo
		{
			get { return m_copiaPreferenze.BudgetAggiuntivo.ToString(); }
			set
			{
				if (double.TryParse(value, NumberStyles.Integer, null, out double number))
				{
					m_copiaPreferenze.BudgetAggiuntivo = number;

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

		public PreferenzeViewModel(IEventAggregator eventAggregator, IDialogService dialogService, Lega lega) : base(eventAggregator, lega)
		{
			m_dialogService = dialogService;

			m_copiaPreferenze = m_lega.Preferenze.Clone() as Preferenze;
			
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
			Title = "Preferenze";
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
			m_lega.SalvaPreferenze(m_copiaPreferenze);
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
				if (m_copiaPreferenze.PreferenzeImpostate)
				{
					if (!m_copiaPreferenze.Equals(m_lega.Preferenze))
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
					Salva();
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