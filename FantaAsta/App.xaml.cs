using System;
using System.IO;
using System.Text;
using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using FantaAsta.Models;
using FantaAsta.ViewModels;
using FantaAsta.Views;

namespace FantaAsta
{
	/// <summary>
	/// Logica di interazione per App.xaml
	/// </summary>
	public partial class App : PrismApplication
	{
		private MainModel m_mainModel;

		protected override Window CreateShell()
		{
			return new Shell();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			m_mainModel = new MainModel();

			containerRegistry.RegisterInstance(typeof(MainModel), m_mainModel);

			containerRegistry.RegisterDialog<ModificaView, ModificaViewModel>("Modifica");
		}

		private void OnExit(object sender, ExitEventArgs e)
		{
			string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FantaRose2019-2020.csv");
			StringBuilder output = new StringBuilder();

			string s = string.Empty;
			foreach (var squadra in m_mainModel.Squadre)
			{
				s += $"{squadra.Nome};;;;";
			}
			output.AppendLine(s);

			for (int i = 0; i < 25; i++)
			{
				s = string.Empty;
				foreach (var squadra in m_mainModel.Squadre)
				{
					if (i < squadra.Giocatori.Count)
					{
						s += $"{squadra.Giocatori[i].Ruolo};{squadra.Giocatori[i].Nome};{squadra.Giocatori[i].Squadra};{squadra.Giocatori[i].Prezzo};";
					}
					else
					{
						s += "/;/;/;/;";
					}
				}
				output.AppendLine(s);
			}

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}

			File.WriteAllText(filePath, output.ToString());
		}
	}
}
