using System;
using System.IO;

namespace FantaAsta.Constants
{
	public static class CommonConstants
	{
		#region PERCORSI

		public static string DATA_DIRECTORY_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FantaLegaManager");
		public static string DATA_FILE_PATH = Path.Combine(DATA_DIRECTORY_PATH, "FantaLegaData.xml");
		public static string SETTINGS_FILE_PATH = Path.Combine(DATA_DIRECTORY_PATH, "FantaLegaSettings.xml");

		#endregion

		#region DEFAULT OPZIONI

		public const double BUDGET_INIZIALE_DEFAULT = 500;
		public const double BUDGET_AGGIUNTIVO_DEFAULT = 100;

		#endregion

		#region REGIONS

		public const string MENU_REGION = "MenuRegion";
		public const string MAIN_REGION = "MainRegion";
		public const string CONTENT_REGION = "ContentRegion";

		#endregion

		#region DIALOGHI

		public const string MESSAGE_DIALOG = "Message";
		public const string MODIFICA_DIALOG = "Modifica";
		public const string PREZZO_DIALOG = "Prezzo";
		public const string AGGIUNGI_DIALOG = "Aggiungi";
		public const string ASSEGNA_DIALOG = "Assegna";
		public const string OPZIONI_DIALOG = "Opzioni";

		#endregion
	}
}