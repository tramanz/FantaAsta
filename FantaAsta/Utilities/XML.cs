using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace FantaAsta.Utilities
{
	public static class XML
	{
		#region Private fields

		private static XmlWriterSettings m_xmlWriterSettings = new XmlWriterSettings() { Indent = true };

		private static XmlDictionaryReaderQuotas m_xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas();

		#endregion

		#region Public methods

		public static void Serialize(string filePath, object data)
		{
			try
			{
				DataContractSerializer dcs = new DataContractSerializer(data.GetType());

				using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
				using (XmlWriter xdw = XmlWriter.Create(fs, m_xmlWriterSettings))
				{
					dcs.WriteObject(xdw, data);
				}
			}
			catch
			{
				// TODO: gestire fallimento serializzazione
			}
		}

		public static object Deserialize(string filePath, Type dataType)
		{
			try
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Open))
				{
					DataContractSerializer ser = new DataContractSerializer(dataType);

					XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, m_xmlDictionaryReaderQuotas);

					return ser.ReadObject(reader);
				}
			}
			catch
			{
				// TODO: gestire fallimento deserializzazione
				return null;
			}
		}

		#endregion
	}
}
