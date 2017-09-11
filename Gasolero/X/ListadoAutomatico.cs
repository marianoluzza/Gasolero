using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;

namespace X.Listados
{
	/// <summary>
	/// Arma listados automaticamente
	/// </summary>
	public static class ListadoAutomatico
	{
		//Raiz
		//	-> TablaMain = 'tabla' (nombre de la tabla)
		//	-> Grupo
		//		-> Dato = 'dato' (dato para agrupar, pertenece a tblMain)
		//		-> Tabla = 'tabla' (tabla de grupos)
		//		-> Display = 'colNbre' (q columna mostrar como nbre grp)
		//		-> Clave = 'colKey' (q columna indica el grp, pertenece a tblGrp) 
		//		-> Lista (si Tabla es vacio entonces estos son los grupos)
		//			-> Item* = 'nbre_i' (nombres de grps)
		//	-> DatosTotalizadores (uno item x cada esq)
		//		-> Item*
		//			-> Dato = 'dato_i' (q dato?)
		//			-> Valor = 'valor_i' (q mostrar, @ para sumar)
		//	-> GrupoTotalizador
		//		-> Nombre = 'nbre' (nbre y key del grp totalizador)
		//		-> UsarColoresGrp = true o false
		//		-> ForeColor = Color
		//		-> BackColor = Color
		//		-> UsarColoresGrpTt = true o false
		//		-> ForeColorTt = Color
		//		-> BackColorTt = Color
		//		-> Datos (la cantidad de items deber ser %0 la cant de esq)
		//			->Item*
		//				-> Dato = 'dato_i'
		//				-> Valor = 'valor_i'
		//				-> Indice = N (en q renglon va)
		//	-> Esquemas
		//		-> Item*
		//				-> Dato = 'dato_i'
		//				-> Alias = 'alias_i'
		//				-> Posicion = N 
		//				-> Alineacion = {I,D,C}

		/// <summary>
		/// Armado un bindind para los datos dados
		/// </summary>
		/// <param name="raiz">nodo raiz del xml de configuracion</param>
		/// <param name="lista">la lista que se va a mostrar</param>
		/// <param name="ds">conjunto de tablas del listado</param>
		public static ListadoBinding GetBinding(XmlNode raiz, ListView lista, DataSet ds)
		{
			List<ListadoEsquema> esquemas = new List<ListadoEsquema>();
			foreach (XmlNode esqXml in raiz["Esquemas"])
			{
				ListadoEsquema esq = new ListadoEsquema();
				esq.Alias = esqXml["Alias"].InnerText;
				esq.Dato = esqXml["Dato"].InnerText;
				esq.Posicion = int.Parse(esqXml["Posicion"].InnerText);
				esq.Alineacion = esqXml["Alineacion"].InnerText;
				esquemas.Add(esq);
			}
			ListadoBinding res = new ListadoBinding(lista, esquemas);
			foreach (DataTable dt in ds.Tables)
				res.ActualizarTabla(dt);
			res.TablaMain = raiz["TablaMain"].InnerText;
			XmlNode grpXml = raiz["Grupo"];
			res.DatoGrupo = grpXml["Dato"].InnerText;
			string tblGrp = grpXml["Tabla"].InnerText;
			if (tblGrp != "")
			{
				foreach (DataRow r in ds.Tables[tblGrp].Rows)
				{
					ListadoGrupo grp = new ListadoGrupo();
					grp.Clave = r[grpXml["Clave"].InnerText].ToString();
					grp.Nombre = r[grpXml["Display"].InnerText].ToString();
					res.Grupos.Add(grp);
				}
			}
			else
			{
				foreach (XmlNode grpItXml in grpXml["Lista"])
				{
					res.Grupos.Add(new ListadoGrupo(grpItXml.InnerText));
				}
			}
			foreach (XmlNode datoTtXml in raiz["DatosTotalizadores"])
			{
				res.DatosTotalizadores[datoTtXml["Dato"].InnerText] = datoTtXml["Valor"].InnerText;
			}
			XmlNode grpTtXml = raiz["GrupoTotalizador"];
			res.GrupoTotalizador.Clave = grpTtXml["Nombre"].InnerText;
			res.GrupoTotalizador.Nombre = grpTtXml["Nombre"].InnerText;
			//res.GrupoTotalizador.Clave = grpTtXml["Nombre"].InnerText;
			int cant = grpTtXml["Datos"].ChildNodes.Count / esquemas.Count;
			for (int i = 0; i < cant; i++)
				res.GrupoTotalizadorDatos.AddFila(i);
			foreach(XmlNode datoTtTtXml in grpTtXml["Datos"])
			{
				string dato = datoTtTtXml["Dato"].InnerText;
				int indice = int.Parse(datoTtTtXml["Indice"].InnerText);
				res.GrupoTotalizadorDatos[dato, indice] = datoTtTtXml["Valor"].InnerText;
			}
			return res;
		}

		/// <summary>
		/// Devuelve un xml base para crear listados automáticos
		/// </summary>
		public static XmlDocument GetXmlBase()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement raiz = doc.AppendChild(doc.CreateElement("RAIZ")) as XmlElement, aux;
			//
			raiz.AppendChild(doc.CreateComment("nombre de la tabla"));
			raiz.AppendChild(doc.CreateElement("TablaMain"));
			aux = raiz.AppendChild(doc.CreateElement("Grupo")) as XmlElement;
			//
			aux.AppendChild(doc.CreateComment("dato para agrupar, pertenece a tblMain"));
			aux.AppendChild(doc.CreateElement("Dato"));
			aux.AppendChild(doc.CreateComment("tabla de grupos"));
			aux.AppendChild(doc.CreateElement("Tabla"));
			aux.AppendChild(doc.CreateComment("q columna mostrar como nbre grp"));
			aux.AppendChild(doc.CreateElement("Display"));
			aux.AppendChild(doc.CreateComment("q columna indica el grp, pertenece a tblGrp"));
			aux.AppendChild(doc.CreateElement("Clave"));
			aux.AppendChild(doc.CreateComment("si Tabla es vacio entonces estos son los grupos"));
			aux = aux.AppendChild(doc.CreateElement("Lista")) as XmlElement;
			aux.AppendChild(doc.CreateComment("nombres de grps - Item*"));
			aux.AppendChild(doc.CreateElement("Item"));
			//
			aux = raiz.AppendChild(doc.CreateElement("DatosTotalizadores")) as XmlElement;
			aux.AppendChild(doc.CreateComment("un item x cada esquema - Item*"));
			aux = aux.AppendChild(doc.CreateElement("Item")) as XmlElement;
			aux.AppendChild(doc.CreateComment("q dato?"));
			aux.AppendChild(doc.CreateElement("Dato"));
			aux.AppendChild(doc.CreateComment("q mostrar, @ para sumar"));
			aux.AppendChild(doc.CreateElement("Valor"));
			//
			aux = raiz.AppendChild(doc.CreateElement("GrupoTotalizador")) as XmlElement;
			aux.AppendChild(doc.CreateComment("nbre y key del grp totalizador"));
			aux.AppendChild(doc.CreateElement("Nombre"));
			aux.AppendChild(doc.CreateComment("true o false"));
			aux.AppendChild(doc.CreateElement("UsarColoresGrp"));
			aux.AppendChild(doc.CreateComment("Color"));
			aux.AppendChild(doc.CreateElement("ForeColor"));
			aux.AppendChild(doc.CreateComment("Color"));
			aux.AppendChild(doc.CreateElement("BackColor"));
			aux.AppendChild(doc.CreateComment("true o false"));
			aux.AppendChild(doc.CreateElement("UsarColoresGrpTt"));
			aux.AppendChild(doc.CreateComment("Color"));
			aux.AppendChild(doc.CreateElement("ForeColorTt"));
			aux.AppendChild(doc.CreateComment("Color"));
			aux.AppendChild(doc.CreateElement("BackColorTt"));
			aux = aux.AppendChild(doc.CreateElement("Datos")) as XmlElement;
			aux.AppendChild(doc.CreateComment("la cantidad de items deber ser %0 la cant de esquemas"));
			aux = aux.AppendChild(doc.CreateElement("Item")) as XmlElement;
			aux.AppendChild(doc.CreateComment("q dato?"));
			aux.AppendChild(doc.CreateElement("Dato"));
			aux.AppendChild(doc.CreateComment("q mostrar, @ para sumar"));
			aux.AppendChild(doc.CreateElement("Valor"));
			aux.AppendChild(doc.CreateComment("N (en q renglon va)"));
			aux.AppendChild(doc.CreateElement("Indice"));
			//
			aux = raiz.AppendChild(doc.CreateElement("Esquemas")) as XmlElement;
			aux.AppendChild(doc.CreateComment("Item*"));
			aux = aux.AppendChild(doc.CreateElement("Item")) as XmlElement;
			aux.AppendChild(doc.CreateComment("dato_i"));
			aux.AppendChild(doc.CreateElement("Dato"));
			aux.AppendChild(doc.CreateComment("alias_i"));
			aux.AppendChild(doc.CreateElement("Alias"));
			aux.AppendChild(doc.CreateComment("N"));
			aux.AppendChild(doc.CreateElement("Posicion"));
			aux.AppendChild(doc.CreateComment("{I,D,C}"));
			aux.AppendChild(doc.CreateElement("Alineacion"));
			//
			return doc;
		}
	}
}
