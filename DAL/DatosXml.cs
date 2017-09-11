using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Data;
using System.IO;
using MarUtils.Soporte;

namespace DAL
{
	public class DatosXml
	{
		static XmlDocument doc;
		static string PATH_DENSIDADES = "";

		public static void Init()
		{
			doc = new XmlDocument();
			PATH_DENSIDADES = Datos.SettingsGet("Resolucion");
		}

		public static decimal DensidadGetForIdArticulo(uint idArticulo)
		{
			decimal res = 1;
			try
			{
				if (File.Exists(PATH_DENSIDADES))
				{
					doc.Load(PATH_DENSIDADES);
					foreach (XmlElement e in doc.DocumentElement.ChildNodes)
					{
						uint id = uint.Parse(e["ARTICULO"].InnerText);
						if (id == idArticulo)
						{
 							res = decimal.Parse(e["VALOR"].InnerText);
							if (res > 1)
								res = decimal.Parse(e["VALOR"].InnerText.Replace(',', '^').Replace('.', ',').Replace('^', '.'));
							return res;
						}
					}
					// Si llego hasta aca es porque no estaba
					File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ERROR 2.Llamar al Jefe.txt", "ERROR 2.Llamar al Jefe.txt");
				}
				else
				{
					//aca es que lo borraron
					File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ERROR 1.Llamar al Jefe.txt", "ERROR 1.Llamar al Jefe.txt");
				}
			}
			catch(Exception ex)
			{
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ERROR 3.Llamar al Jefe.txt", ex.Message);
			}
			return res;
		}

		public static ABMResultado DensidadInsertSingle(uint idArticulo, decimal valor)
		{
			try
			{
				if (File.Exists(PATH_DENSIDADES))
				{
					doc.Load(PATH_DENSIDADES);
					XmlNode dens = doc.DocumentElement.AppendChild(doc.CreateElement("DENSIDAD"));
					dens.AppendChild(doc.CreateElement("ARTICULO")).InnerText = idArticulo.ToString();
					dens.AppendChild(doc.CreateElement("VALOR")).InnerText = valor.ToString();
					doc.Save(PATH_DENSIDADES);
					return new ABMResultado(1);
				}
				else
				{
					return new ABMResultado(enErrores.DatosFaltantes, "No existe el archivo de densidades!");
				}
			}
			catch (Exception ex)
			{
				return new ABMResultado(enErrores.Otro, ex.Message);
			}
		}

		public static ABMResultado DensidadDeleteForIdArticulo(uint idArticulo)
		{
			XmlElement res = null;
			try
			{
				if (File.Exists(PATH_DENSIDADES))
				{
					doc.Load(PATH_DENSIDADES);
					foreach (XmlElement e in doc.DocumentElement.ChildNodes)
					{
						uint id = uint.Parse(e["ARTICULO"].InnerText);
						if (id == idArticulo)
						{
							res = e;
							break;
						}
					}
					if (res != null)
					{
						doc.DocumentElement.RemoveChild(res);
						doc.Save(PATH_DENSIDADES);
						return new ABMResultado(1);
					}
					return new ABMResultado(enErrores.Otro, "No existe la densidad para dicho articulo");
				}
				else
				{
					//aca es que lo borraron
					File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ERROR 1.Llamar al Jefe.txt", "ERROR 1.Llamar al Jefe.txt");
					return new ABMResultado(enErrores.DatosFaltantes, "No existe el archivo de densidades!");
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ERROR 3.Llamar al Jefe.txt", ex.Message);
				return new ABMResultado(enErrores.Otro, ex.Message);
			}
		}
	}
}
