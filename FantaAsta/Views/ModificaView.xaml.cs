using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using FantaAsta.Models;

namespace FantaAsta.Views
{
	/// <summary>
	/// Interaction logic for ModificaView.xaml
	/// </summary>
	public partial class ModificaView : UserControl, IDisposable
	{
		#region Private fields

		private readonly Timer m_timer;

		private string m_parola = string.Empty;

		#endregion

		public ModificaView()
		{
			InitializeComponent();

			m_timer = new Timer
			{ 
				AutoReset = false,
				Enabled = false,
				Interval = 1000 
			};
			m_timer.Elapsed += OnTick;
		}

		#region Private methods

		private void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (m_timer.Enabled)
			{
				m_timer.Stop();
			}
			m_timer.Enabled = true;
			m_timer.Start();

			if (((e.Key >= Key.A && e.Key <= Key.Z) || e.Key == Key.Space || e.Key == Key.Oem4) && sender is ListView lista)
			{
				m_parola += e.Key == Key.Space ? " " : e.Key == Key.Oem4 ? "'" : e.Key.ToString();

				Giocatore giocatore = ((ObservableCollection<Giocatore>)lista.ItemsSource).FirstOrDefault(g => g.Nome.StartsWith(m_parola, StringComparison.OrdinalIgnoreCase));
				if (giocatore != null)
				{
					lista.SelectedItem = giocatore;
					lista.ScrollIntoView(giocatore);
				}
			}
		}

		private void OnTick(object sender, ElapsedEventArgs e)
		{
			m_parola = string.Empty;

			m_timer.Stop();
			m_timer.Enabled = false;
		}

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

					m_timer?.Dispose();
				}

				// liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
				// impostare campi di grandi dimensioni su Null.

				disposedValue = true;
			}
		}

		// eseguire l'override di un finalizzatore solo se Dispose(bool disposing) include il codice per liberare risorse non gestite.
		// ~ModificaView()
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
