using System.Diagnostics;
using System.Reflection;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public class AboutViewModel : BaseDialogViewModel
	{
		#region Properties

		public string Versione { get { return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion; } }
		
		public string Copyright { get { return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright; } }
		
		#endregion

		public AboutViewModel(IEventAggregator eventAggregator, Asta asta) : base(eventAggregator, asta)
		{ }

		#region Protected methods

		protected override void InizializzaIcona(IDialogParameters parameters)
		{ }

		protected override void InizializzaTitolo(IDialogParameters parameters)
		{
			Title = $"Informazioni su FantaAsta Manager";
		}

		protected override void InizializzaBottoni(IDialogParameters parameters)
		{
			Buttons.Add(new DialogButton("Ok", new DelegateCommand(() => RaiseRequestClose(new DialogResult(ButtonResult.OK)))));
		}

		#endregion
	}
}