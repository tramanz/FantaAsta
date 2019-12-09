using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using FantaAsta.Models;

namespace FantaAsta.Utilities
{
	public static class Constants
	{
		public static string RAW_DATA_FILE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "Quotazioni_Fantacalcio.csv");
		public static string DATA_DIRECTORY_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FantaAstaManager");
		public static string DATA_FILE_PATH = Path.Combine(DATA_DIRECTORY_PATH, "FantaAstaData.xml");

		public static Squadra ATALANTA = new Squadra("Atalanta", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E71B8")));
		public static Squadra BOLOGNA = new Squadra("Bologna", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A21C26")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A2F48")));
		public static Squadra BRESCIA = new Squadra("Brescia", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#004E8E")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")));
		public static Squadra CAGLIARI = new Squadra("Cagliari", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AD002A")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#002350")));
		public static Squadra FIORENTINA = new Squadra("Fiorentina", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#482E92")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")));
		public static Squadra GENOA = new Squadra("Genoa", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00265D")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1393D")));
		public static Squadra INTER = new Squadra("Inter", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0068A8")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#221F20")));
		public static Squadra JUVENTUS = new Squadra("Juventus", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")));
		public static Squadra LAZIO = new Squadra("Lazio", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6397D0")));
		public static Squadra LECCE = new Squadra("Lecce", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF200")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED1B23")));
		public static Squadra MILAN = new Squadra("Milan", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB090B")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")));
		public static Squadra NAPOLI = new Squadra("Napoli", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#12A0D7")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")));
		public static Squadra PARMA = new Squadra("Parma", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEC532")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#035183")));
		public static Squadra ROMA = new Squadra("Roma", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E1F2F")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0BC42")));
		public static Squadra SAMPDORIA = new Squadra("Sampdoria", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0152BC")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")));
		public static Squadra SASSUOLO = new Squadra("Sassuolo", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#21633B")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")));
		public static Squadra SPAL = new Squadra("Spal", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1199CC")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")));
		public static Squadra TORINO = new Squadra("Torino", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B1C20")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")));
		public static Squadra UDINESE = new Squadra("Udinese", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")));
		public static Squadra VERONA = new Squadra("Verona", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#004A9A")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEE500")));

		public static List<Squadra> SERIE_A = new List<Squadra>
		{
			ATALANTA, BOLOGNA, BRESCIA, CAGLIARI, FIORENTINA,
			GENOA, INTER, JUVENTUS, LAZIO, LECCE,
			MILAN, NAPOLI, PARMA, ROMA, SAMPDORIA,
			SASSUOLO, SPAL, TORINO, UDINESE, VERONA
		};
	}
}
