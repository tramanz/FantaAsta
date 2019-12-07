using System;
using System.Collections.Generic;
using System.Windows.Media;
using FantaAsta.Enums;

namespace FantaAsta.Models
{
	public class Giocatore
	{
		#region Private fields

		private double m_prezzo;

		#endregion

		#region Properties

		public int ID { get; }
		public Ruoli Ruolo { get; }
		public string Nome { get; }
		public Squadra Squadra { get; }
		public double Quotazione { get; }
		public double Prezzo { get; set; }

		#endregion
		public Giocatore(int id, Ruoli ruolo, string nome, string squadra, double quotazione)
		{
			ID = id;
			Ruolo = ruolo;
			Nome = nome;
			Squadra = SerieA.Squadre.Find(s => s.Nome.Equals(squadra, StringComparison.OrdinalIgnoreCase));
			Quotazione = quotazione;
		}
	}

	public class Squadra
	{
		public string Nome { get; }

		public Brush Colore1 { get; }

		public Brush Colore2 { get; }

		public Squadra(string nome, Brush colore1, Brush colore2)
		{
			Nome = nome;
			Colore1 = colore1;
			Colore2 = colore2;
		}
	}

	public static class SerieA
	{
		public static Squadra ATALANTA = new Squadra("Atalanta", new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.Blue));
		public static Squadra BOLOGNA = new Squadra("Bologna", new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Blue));
		public static Squadra BRESCIA = new Squadra("Brescia", new SolidColorBrush(Colors.Cyan), new SolidColorBrush(Colors.White));
		public static Squadra CAGLIARI = new Squadra("Cagliari", new SolidColorBrush(Colors.DarkRed), new SolidColorBrush(Colors.Blue));
		public static Squadra FIORENTINA = new Squadra("Fiorentina", new SolidColorBrush(Colors.Purple), new SolidColorBrush(Colors.White));
		public static Squadra GENOA = new Squadra("Genoa", new SolidColorBrush(Colors.DarkBlue), new SolidColorBrush(Colors.Red));
		public static Squadra INTER = new Squadra("Inter", new SolidColorBrush(Colors.Blue), new SolidColorBrush(Colors.Black));
		public static Squadra JUVENTUS = new Squadra("Juventus", new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black));
		public static Squadra LAZIO = new Squadra("Lazio", new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Cyan));
		public static Squadra LECCE = new Squadra("Lecce", new SolidColorBrush(Colors.Yellow), new SolidColorBrush(Colors.Red));
		public static Squadra MILAN = new Squadra("Milan", new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Black));
		public static Squadra NAPOLI = new Squadra("Napoli", new SolidColorBrush(Colors.DarkCyan), new SolidColorBrush(Colors.White));
		public static Squadra PARMA = new Squadra("Parma", new SolidColorBrush(Colors.Yellow), new SolidColorBrush(Colors.Blue));
		public static Squadra ROMA = new Squadra("Roma", new SolidColorBrush(Colors.Brown), new SolidColorBrush(Colors.Yellow));
		public static Squadra SAMPDORIA = new Squadra("Sampdoria", new SolidColorBrush(Colors.DarkBlue), new SolidColorBrush(Colors.White));
		public static Squadra SASSUOLO = new Squadra("Sassuolo", new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.Black));
		public static Squadra SPAL = new Squadra("Spal", new SolidColorBrush(Colors.Cyan), new SolidColorBrush(Colors.White));
		public static Squadra TORINO = new Squadra("Torino", new SolidColorBrush(Colors.Brown), new SolidColorBrush(Colors.White));
		public static Squadra UDINESE = new Squadra("Udinese", new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.White));
		public static Squadra VERONA = new Squadra("Verona", new SolidColorBrush(Colors.SlateBlue), new SolidColorBrush(Colors.Yellow));

		public static List<Squadra> Squadre = new List<Squadra>
		{
			ATALANTA, BOLOGNA, BRESCIA, CAGLIARI, FIORENTINA,
			GENOA, INTER, JUVENTUS, LAZIO, LECCE,
			MILAN, NAPOLI, PARMA, ROMA, SAMPDORIA,
			SASSUOLO, SPAL, TORINO, UDINESE, VERONA
		};
	}
}
