using System;
using System.IO;

namespace FantaAsta.Constants
{
	public static class CommonConstants
	{
		public static string DATA_DIRECTORY_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FantaLegaManager");
		public static string DATA_FILE_PATH = Path.Combine(DATA_DIRECTORY_PATH, "FantaLegaData.xml");

		public const double BUDGET_ESTIVO = 500;
		public const double BUDGET_INVERNALE = 100;

		#region DIALOGHI

		public const string MESSAGE_DIALOG = "Message";
		public const string MODIFICA_DIALOG = "Modifica";
		public const string PREZZO_DIALOG = "Prezzo";
		public const string AGGIUNGI_DIALOG = "Aggiungi";
		public const string ASSEGNA_DIALOG = "Assegna";

		#endregion
	}
}