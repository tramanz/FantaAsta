using System;
using System.IO;

namespace FantaAsta.Utilities
{
	public static class Constants
	{
		public static string DATA_DIRECTORY_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FantaLegaManager");
		public static string DATA_FILE_PATH = Path.Combine(DATA_DIRECTORY_PATH, "FantaLegaData.xml");

		public const double BUDGET_ESTIVO = 500;
		public const double BUDGET_INVERNALE = 100;
	}
}