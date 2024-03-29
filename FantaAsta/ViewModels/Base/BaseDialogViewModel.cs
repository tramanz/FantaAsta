﻿using System.Threading;
using Prism.Events;
using FantaAsta.Models;
using FantaAsta.Utilities.Dialogs;

namespace FantaAsta.ViewModels
{
	public abstract class BaseDialogViewModel : DialogAwareViewModel
	{
		#region Protected fields

		protected readonly IEventAggregator m_eventAggregator;

		protected readonly SynchronizationContext m_syncContext;

		protected readonly Asta m_asta;

		#endregion

		protected BaseDialogViewModel(IEventAggregator eventAggregator, Asta asta)
		{
			m_eventAggregator = eventAggregator;

			m_syncContext = SynchronizationContext.Current;
			
			m_asta = asta;
		}
	}
}