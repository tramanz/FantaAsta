using System;
using System.Collections.Generic;
using System.IO;
using FantaAsta.Models;

namespace FantaAsta.Utilities
{
	public static class Constants
	{
		public static string DATA_DIRECTORY_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FantaLegaManager");
		public static string DATA_FILE_PATH = Path.Combine(DATA_DIRECTORY_PATH, "FantaLegaData.xml");

		public const double BUDGET_ESTIVO = 500;
		public const double BUDGET_INVERNALE = 100;

		#region Squadre

		public static Squadra AREZZO = new Squadra("Arezzo", "#7B101A", "#FFFFFF");
		public static Squadra ASCOLI = new Squadra("Ascoli", "#000000", "#FFFFFF");
		public static Squadra ATALANTA = new Squadra("Atalanta", "#000000", "#1E71B8");
		public static Squadra AVELLINO = new Squadra("Avellino", "#FFFFFF", "#009774");
		public static Squadra BARI = new Squadra("Vicenza", "#FE0000", "#FFFFFF");
		public static Squadra BENEVENTO = new Squadra("Benevento", "FBF428", "#DE0728");
		public static Squadra BOLOGNA = new Squadra("Bologna", "#A21C26", "#1A2F48");
		public static Squadra BRESCIA = new Squadra("Brescia", "#004E8E", "#FFFFFF");
		public static Squadra CAGLIARI = new Squadra("Cagliari", "#AD002A", "#002350");
		public static Squadra CARPI = new Squadra("Carpi", "#FFFFFF", "#FE0000");
		public static Squadra CATANIA = new Squadra("Catania", "#0096D5", "#EF3123");
		public static Squadra CESENA = new Squadra("Cesena", "#FFFFFF", "#000000");
		public static Squadra CITTADELLA = new Squadra("Cittadella", "#B30033", "#FFFFFF");
		public static Squadra CHIEVO = new Squadra("Chievo", "#FFFF01", "#161384");
		public static Squadra COSENZA = new Squadra("Cosenza", "#130059", "#F4001C");
		public static Squadra CREMONESE = new Squadra("Cremonese", "#BD0020", "#CCCCCC");
		public static Squadra CROTONE = new Squadra("Crotone", "#130059", "#F4001C");
		public static Squadra EMPOLI = new Squadra("Empoli", "#1A5CAA", "#FFFFFF");
		public static Squadra ENTELLA = new Squadra("Entella", "#FFFFFF", "#79D3F6");
		public static Squadra FIORENTINA = new Squadra("Fiorentina", "#482E92", "#FFFFFF");
		public static Squadra FROSINONE = new Squadra("Frosinone", "#2B63B0", "#FBFF2A");
		public static Squadra GENOA = new Squadra("Genoa", "#00265D", "#E1393D");
		public static Squadra INTER = new Squadra("Inter", "#0068A8", "#221F20");
		public static Squadra JUVENTUS = new Squadra("Juventus", "#FFFFFF", "#000000");
		public static Squadra JUVESTABIA = new Squadra("Juve Stabia", "#FAFF29", "#300185");
		public static Squadra LAZIO = new Squadra("Lazio", "#FFFFFF", "#6397D0");
		public static Squadra LECCE = new Squadra("Lecce", "#FFF200", "#ED1B23");
		public static Squadra LIVORNO = new Squadra("Livorno", "#8E3144", "#000000");
		public static Squadra MILAN = new Squadra("Milan", "#FB090B", "#000000");
		public static Squadra MODENA = new Squadra("Modena", "#FBF428", "#2A4268");
		public static Squadra MONZA = new Squadra("Monza", "#FE0000", "#FFFFFF");
		public static Squadra NAPOLI = new Squadra("Napoli", "#12A0D7", "#FFFFFF");
		public static Squadra PADOVA = new Squadra("Padova", "#FFFFFF", "#E21301");
		public static Squadra PALERMO = new Squadra("Palermo", "#F9A6D2", "#000000");
		public static Squadra PARMA = new Squadra("Parma", "#FEC532", "#035183");
		public static Squadra PERUGIA = new Squadra("Perugia", "#EA0001", "#FFFFFF");
		public static Squadra PESCARA = new Squadra("Pescara", "#FFFFFF", "#0096D5");
		public static Squadra PIACENZA = new Squadra("Piacenza", "#FE0000", "FFFFFF");
		public static Squadra PISA = new Squadra("Pisa", "#000000", "#5479D4");
		public static Squadra PORDENONE = new Squadra("Pordenone", "#FFFFFF", "#008735");
		public static Squadra REGGINA = new Squadra("Reggina", "#8E0000", "#FFFFFF");
		public static Squadra ROMA = new Squadra("Roma", "#8E1F2F", "#F0BC42");
		public static Squadra SALERNITANA = new Squadra("Salernitana", "#711C31", "#FFFFFF");
		public static Squadra SAMPDORIA = new Squadra("Sampdoria", "#0152BC", "#FFFFFF");
		public static Squadra SASSUOLO = new Squadra("Sassuolo", "#21633B", "#000000");
		public static Squadra SIENA = new Squadra("Siena", "#000000", "#FFFFFF");
		public static Squadra SPAL = new Squadra("Spal", "#1199CC", "#FFFFFF");
		public static Squadra SPEZIA = new Squadra("Spezia", "#FFFFFF", "#000000");
		public static Squadra TERNANA = new Squadra("Ternana", "#DE0728", "#296E15");
		public static Squadra TORINO = new Squadra("Torino", "#7B1C20", "#FFFFFF");
		public static Squadra TRAPANI = new Squadra("Trapani", "#7B1C22", "#FFFFFF");
		public static Squadra TRIESTINA = new Squadra("Triestina", "#FE0000", "#FFFFFF");
		public static Squadra UDINESE = new Squadra("Udinese", "#FFFFFF", "#000000");
		public static Squadra VERONA = new Squadra("Verona", "#004A9A", "#FEE500");
		public static Squadra VICENZA = new Squadra("Vicenza", "#FFFFFF", "#FE0000");
		public static Squadra VENEZIA = new Squadra("Venezia", "#000000", "#FF972E");

		public static List<Squadra> SERIE_A = new List<Squadra>
		{
			ATALANTA,
			BOLOGNA,
			BRESCIA,
			CAGLIARI,
			FIORENTINA,
			GENOA,
			INTER,
			JUVENTUS,
			LAZIO,
			LECCE,
			MILAN,
			NAPOLI,
			PARMA,
			ROMA,
			SAMPDORIA,
			SASSUOLO,
			SPAL,
			TORINO,
			UDINESE,
			VERONA
		};

		#endregion
	}
}
