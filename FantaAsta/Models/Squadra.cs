using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace FantaAsta.Models
{
	public class Squadra : BindableBase
	{
		public string Nome { get; }
		public ObservableCollection<Giocatore> Giocatori { get; }

		private double m_budget;
		public double Budget
		{
			get { return m_budget; }
			set { SetProperty(ref m_budget, value); }
		}

		public Squadra(string nome)
		{
			Nome = nome;
			Giocatori = new ObservableCollection<Giocatore>();
			Budget = 500;
		}
	}
}
