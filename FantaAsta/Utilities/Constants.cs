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

		public static List<Squadra> SQUADRE = new List<Squadra>
		{
			new Squadra("Arezzo", "#7B101A", "#FFFFFF"),
			new Squadra("Ascoli", "#000000", "#FFFFFF"),
			new Squadra("Atalanta", "#000000", "#1E71B8"),
			new Squadra("Avellino", "#FFFFFF", "#009774"),
			new Squadra("Vicenza", "#FE0000", "#FFFFFF"),
			new Squadra("Benevento", "#FBF428", "#DE0728"),
			new Squadra("Bologna", "#A21C26", "#1A2F48"),
			new Squadra("Brescia", "#004E8E", "#FFFFFF"),
			new Squadra("Cagliari", "#AD002A", "#002350"),
			new Squadra("Carpi", "#FFFFFF", "#FE0000"),
			new Squadra("Catania", "#0096D5", "#EF3123"),
			new Squadra("Cesena", "#FFFFFF", "#000000"),
			new Squadra("Cittadella", "#B30033", "#FFFFFF"),
			new Squadra("Chievo", "#FFFF01", "#161384"),
			new Squadra("Cosenza", "#130059", "#F4001C"),
			new Squadra("Cremonese", "#BD0020", "#CCCCCC"),
			new Squadra("Crotone", "#130059", "#F4001C"),
			new Squadra("Empoli", "#1A5CAA", "#FFFFFF"),
			new Squadra("Entella", "#FFFFFF", "#79D3F6"),
			new Squadra("Fiorentina", "#482E92", "#FFFFFF"),
			new Squadra("Frosinone", "#2B63B0", "#FBFF2A"),
			new Squadra("Genoa", "#00265D", "#E1393D"),
			new Squadra("Inter", "#0068A8", "#221F20"),
			new Squadra("Juventus", "#FFFFFF", "#000000"),
			new Squadra("Juve Stabia", "#FAFF29", "#300185"),
			new Squadra("Lazio", "#FFFFFF", "#6397D0"),
			new Squadra("Lecce", "#FFF200", "#ED1B23"),
			new Squadra("Livorno", "#8E3144", "#000000"),
			new Squadra("Milan", "#FB090B", "#000000"),
			new Squadra("Modena", "#FBF428", "#2A4268"),
			new Squadra("Monza", "#FE0000", "#FFFFFF"),
			new Squadra("Napoli", "#12A0D7", "#FFFFFF"),
			new Squadra("Padova", "#FFFFFF", "#E21301"),
			new Squadra("Palermo", "#F9A6D2", "#000000"),
			new Squadra("Parma", "#FEC532", "#035183"),
			new Squadra("Perugia", "#EA0001", "#FFFFFF"),
			new Squadra("Pescara", "#FFFFFF", "#0096D5"),
			new Squadra("Piacenza", "#FE0000", "#FFFFFF"),
			new Squadra("Pisa", "#000000", "#5479D4"),
			new Squadra("Pordenone", "#FFFFFF", "#008735"),
			new Squadra("Reggina", "#8E0000", "#FFFFFF"),
			new Squadra("Roma", "#8E1F2F", "#F0BC42"),
			new Squadra("Salernitana", "#711C31", "#FFFFFF"),
			new Squadra("Sampdoria", "#0152BC", "#FFFFFF"),
			new Squadra("Sassuolo", "#21633B", "#000000"),
			new Squadra("Siena", "#000000", "#FFFFFF"),
			new Squadra("Spal", "#1199CC", "#FFFFFF"),
			new Squadra("Spezia", "#FFFFFF", "#000000"),
			new Squadra("Ternana", "#DE0728", "#296E15"),
			new Squadra("Torino", "#7B1C20", "#FFFFFF"),
			new Squadra("Trapani", "#7B1C22", "#FFFFFF"),
			new Squadra("Triestina", "#FE0000", "#FFFFFF"),
			new Squadra("Udinese", "#FFFFFF", "#000000"),
			new Squadra("Verona", "#004A9A", "#FEE500"),
			new Squadra("Vicenza", "#FFFFFF", "#FE0000"),
			new Squadra("Venezia", "#000000", "#FF972E")
		};
	}
}