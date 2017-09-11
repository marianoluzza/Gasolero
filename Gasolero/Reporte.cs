using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Yogesh.ExcelXml;
using Negocio;

namespace Gasolero
{
	/// <summary>
	/// Reportes XLS
	/// </summary>
	public class Reporte
	{
		string _nbre = "";

		public string Nombre
		{
			get { return _nbre; }
			set { _nbre = value.Replace("\\", "-").Replace("/", "-"); }
		}

		ExcelXmlWorkbook _libro;

		public void HacerReporteVentasViejo(DateTime dde, DateTime hta)
		{
			DataTable dtFacts = FactEncabezado.GetListForFechas(dde, hta);
			DateTime ultFecha = dde.Date;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Ventas " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			//Assembly ass = Assembly.GetExecutingAssembly();
			//Stream fs = ass.GetManifestResourceStream("Gasolero.Recursos.Ventas.xls");
			//StreamReader sr = new StreamReader(fs);
			//File.WriteAllText(nbreReporte + ".xls", sr.ReadToEnd());
			//sr.Close();
			//Yogesh.ExcelXml.ExcelXmlWorkbook libro = Yogesh.ExcelXml.ExcelXmlWorkbook.Import(nbreReporte + ".xls");
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			//Yogesh.ExcelXml.Worksheet reporte = libro["Reporte"];
			Worksheet reporte = _libro.Add("Reporte");
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy";
			reporte.Columns(0).Style = estilo;
			//
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "0";
			reporte.Columns(2).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			reporte.Columns(5).Style = estilo;
			reporte.Columns(6).Style = estilo;
			reporte.Columns(7).Style = estilo;
			reporte.Columns(8).Style = estilo;
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[1].Value = "Letra";
			rEnca[2].Value = "Numero";
			rEnca[3].Value = "Cliente";
			rEnca[4].Value = "CUIT";
			rEnca[5].Value = "Neto";
			rEnca[6].Value = "No Gravado";
			rEnca[7].Value = "IVA";
			rEnca[8].Value = "Total";
			for (int i = 0; i <= 8; i++)
			{
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			}
			List<FactEncabezado> factsB = new List<FactEncabezado>();
			foreach (DataRow rFact in dtFacts.Rows)
			{
				FactEncabezado fact = new FactEncabezado(rFact);
				fact.GetFilas();
				Cliente cli = Cliente.GetSingleE(fact.IdCliente);
				if (fact.Fecha.Date != ultFecha.Date)
				{
					decimal neto = 0, iva = 0, total = 0, ii = 0;
					foreach (FactEncabezado f in factsB)
					{
						foreach (FactItem it in f.Cuerpo)
						{
							neto += it.Precio * it.Cantidad;
							iva += it.IVA * it.Cantidad;
							total += it.Monto;
							ii += total - (neto + iva);
						}
					}
					if (factsB.Count > 0)
					{
						Yogesh.ExcelXml.Row r = reporte.AddRow();
						r[0].Value = factsB[0].Fecha.ToShortDateString();
						r[1].Value = "B";
						r[2].Value = factsB[0].Numero + "/" + factsB[factsB.Count - 1].Numero;
						r[3].Value = cli.Nombre;
						r[4].Value = cli.CUIT;
						r[5].Value = decimal.Round(neto, 2);
						r[6].Value = decimal.Round(ii, 2);
						r[7].Value = decimal.Round(iva, 2);
						r[8].Value = decimal.Round(total, 2);
					}
					factsB.Clear();
					ultFecha = fact.Fecha;
				}
				if (fact.Letra == "A")
				{
					//Yogesh.ExcelXml.Row r = reporte[iRow++];
					Yogesh.ExcelXml.Row r = reporte.AddRow();
					r[0].Value = fact.Fecha.ToShortDateString();
					r[1].Value = fact.Letra;
					r[2].Value = fact.Numero;
					r[3].Value = cli.Nombre;
					r[4].Value = cli.CUIT;
					decimal neto = 0, iva = 0, total = 0, ii = 0;
					foreach (FactItem it in fact.Cuerpo)
					{
						neto += it.Precio * it.Cantidad;
						iva += it.IVA * it.Cantidad;
						total += it.Monto;
						ii += total - (neto + iva);
					}
					r[5].Value = decimal.Round(neto, 2);
					r[6].Value = decimal.Round(ii, 2);
					r[7].Value = decimal.Round(iva, 2);
					r[8].Value = decimal.Round(total, 2);
				}
				else
				{
					factsB.Add(fact);
				}
			}
			if (factsB.Count > 0)
			{
				decimal neto = 0, iva = 0, total = 0, ii = 0;
				foreach (FactEncabezado f in factsB)
				{
					foreach (FactItem it in f.Cuerpo)
					{
						neto += it.Precio * it.Cantidad;
						iva += it.IVA * it.Cantidad;
						total += it.Monto;
						ii += total - (neto + iva);
					}
				}
				if (factsB.Count > 0)
				{
					Cliente cli = Cliente.GetSingleE(factsB[0].IdCliente);
					Yogesh.ExcelXml.Row r = reporte.AddRow();
					r[0].Value = factsB[0].Fecha.ToShortDateString();
					r[1].Value = "B";
					r[2].Value = factsB[0].Numero + "/" + factsB[factsB.Count - 1].Numero;
					r[3].Value = cli.Nombre;
					r[4].Value = cli.CUIT;
					r[5].Value = decimal.Round(neto, 2);
					r[6].Value = decimal.Round(ii, 2);
					r[7].Value = decimal.Round(iva, 2);
					r[8].Value = decimal.Round(total, 2);
				}
				factsB.Clear();
			}
		}

		public void HacerReporteVentas(DateTime dde, DateTime hta)
		{
			HacerReporteVentas(dde, hta, "Reporte");
		}

		public void HacerReporteVentas(DateTime dde, DateTime hta, string nbreHoja)
		{
			DataTable dtFacts = FactEncabezado.GetListForFechas(dde, hta);
			DateTime ultFecha = dde.Date;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Ventas " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add(nbreHoja);
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy";
			reporte.Columns(0).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(1).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "0";
			estilo.Alignment.Horizontal = HorizontalAlignment.Right;
			reporte.Columns(2).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			reporte.Columns(5).Style = estilo;
			reporte.Columns(6).Style = estilo;
			reporte.Columns(7).Style = estilo;
			reporte.Columns(8).Style = estilo;
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[1].Value = "Letra";
			rEnca[2].Value = "Numero";
			rEnca[3].Value = "Cliente";
			rEnca[4].Value = "CUIT";
			rEnca[5].Value = "Neto";
			rEnca[6].Value = "No Gravado";
			rEnca[7].Value = "IVA";
			rEnca[8].Value = "Total";
			int[] anchos = new int[9] { 73, 42, 82, 219, 94, 93, 93, 93, 93 };
			for (int i = 0; i <= 8; i++)
			{
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = anchos[i] / 1.333333;
			}
			List<FactEncabezado> factsB = new List<FactEncabezado>();
			foreach (DataRow rFact in dtFacts.Rows)
			{
				FactEncabezado fact = new FactEncabezado(rFact);
				fact.GetFilas();
				if (fact.Fecha.Date != ultFecha.Date)
				{
					PrintB(reporte, factsB.ToArray());
					factsB.Clear();
					ultFecha = fact.Fecha;
				}
				if (fact.Letra == "A")
				{
					PrintA(reporte, fact);
				}
				else
				{
					factsB.Add(fact);
				}
			}
			if (factsB.Count > 0)
			{
				PrintB(reporte, factsB.ToArray());
				factsB.Clear();
			}
			PrintTotales(reporte);
		}

		public void HacerReporteVentasCliente(DateTime dde, DateTime hta, uint idCliente, string nbreHoja)
		{
			DataTable dtFacts = idCliente > 0 ? FactEncabezado.GetListForFechasIdCliente(dde, hta, idCliente) : FactEncabezado.GetListForFechas(dde, hta);
			DateTime ultFecha = dde.Date;
			//resolvi por crear un xls nuevo, y no basado en un existente
			if (idCliente > 0)
			{
				Cliente c = Cliente.GetSingleE(idCliente);
				Nombre = "Ventas a " + c.Nombre + " del " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			}
			else
				Nombre = "Ventas " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add(nbreHoja);
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy HH:MM";
			reporte.Columns(0).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "0.000";
			estilo.Alignment.Horizontal = HorizontalAlignment.Right;
			reporte.Columns(1).Style = estilo;
			//
			XmlStyle estiloCash = new Yogesh.ExcelXml.XmlStyle();
			estiloCash.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estiloCash.CustomFormatString = "0.000";//$ no funciona
			estiloCash.Alignment.Horizontal = HorizontalAlignment.Right;
			//estiloCash.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;//salen sólo 2 decimales
			reporte.Columns(2).Style = estiloCash;
			reporte.Columns(3).Style = estilo;
			reporte.Columns(4).Style = estiloCash;
			reporte.Columns(5).Style = estilo;
			reporte.Columns(6).Style = estiloCash;
			reporte.Columns(7).Style = estilo;
			reporte.Columns(8).Style = estiloCash;
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[1].Value = "Nafta Super Lts";
			rEnca[2].Value = "Nafta Super $";
			rEnca[3].Value = "GasOil Lts";
			rEnca[4].Value = "GasOil $";
			rEnca[5].Value = "GNC M3";
			rEnca[6].Value = "GNC $";
			rEnca[7].Value = "EuroDiesel Lts";
			rEnca[8].Value = "EuroDiesel $";
			int[] anchos = new int[9] { 165, 108, 108, 100, 100, 93, 93, 100, 100 };
			for (int i = 0; i <= 8; i++)
			{
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = anchos[i] / 1.333333;
			}
			foreach (DataRow rFact in dtFacts.Rows)
			{
				FactEncabezado fact = new FactEncabezado(rFact);
				fact.GetFilas();
				if (fact.Cuerpo.Count == 0)
					continue;
				Row r = reporte.AddRow();
				r[0].Value = fact.Fecha.ToString();
				switch (fact.Cuerpo[0].IdArticulo)
				{
					case 2://super
						r[1].Value = fact.Cuerpo[0].Cantidad;
						r[2].Value = fact.Cuerpo[0].Monto;
						break;
					case 3://gasoil
						r[3].Value = fact.Cuerpo[0].Cantidad;
						r[4].Value = fact.Cuerpo[0].Monto;
						break;
					case 4://gnc
						r[5].Value = fact.Cuerpo[0].Cantidad;
						r[6].Value = fact.Cuerpo[0].Monto;
						break;
					case 6://eurod
						r[7].Value = fact.Cuerpo[0].Cantidad;
						r[8].Value = fact.Cuerpo[0].Monto;
						break;
				}
			}
			//PrintTotales(reporte);
			Row rfinal = reporte.AddRow();
			rfinal[0].Value = "TOTALES";
			for (int i = 1; i <= 8; i++)
			{
				rfinal[i].Value = FormulaHelper.Formula("SUM", new Range(reporte[i, 1], reporte[i, dtFacts.Rows.Count]));
			}
			rfinal = reporte.AddRow();
			rfinal[0].Value = "TOTAL $";
			Formula fx = new Formula().Add("SUM").StartGroup();
			for (int i = 2; i <= 8; i += 2)
			{
				//fx.Add(reporte[i, dtFacts.Rows.Count + 1]);
				fx.Add(new Range(reporte[i, dtFacts.Rows.Count + 1], reporte[i, dtFacts.Rows.Count + 1]));
				if (i < 8)
					fx.Operator(',');
			}
			fx.EndGroup();
			rfinal[1].Value = fx;
			rfinal[1].Style = estiloCash;
		}

		public void HacerReporteCombustibles(DateTime dde, DateTime hta)
		{
			HacerReporteCombustibles(dde, hta, "Reporte");
		}

		public void HacerReporteCombustibles(DateTime dde, DateTime hta, string nbreHoja)
		{
			Row r;
			int columnas = 1;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Combustibles " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add(nbreHoja);
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy";
			reporte.Columns(0).Style = estilo;
			//
			DataTable dtArticulos = Articulo.GetAll();
			List<Articulo> articulos = new List<Articulo>();
			SortedList<uint, SortedList<DateTime, decimal>> cantidad_fecha_art = new SortedList<uint, SortedList<DateTime, decimal>>();
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				Articulo art = new Articulo(dtArticulos.Rows[i]);
				articulos.Add(art);
				cantidad_fecha_art.Add(art.IdArticulo, new SortedList<DateTime, decimal>(30));
				estilo = new Yogesh.ExcelXml.XmlStyle();
				estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
				estilo.CustomFormatString = "0.000";
				estilo.Alignment.Horizontal = HorizontalAlignment.Right;
				reporte.Columns(i).Style = estilo;
				columnas++;
			}
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[0].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[0].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(0).Width = 73 / 1.333333;
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				Articulo art = articulos[i - 1];
				rEnca[i].Value = art.Descripcion;
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = 84 / 1.333333;
				//
				DataTable dtFacts = FactEncabezado.GetTotalxArticuloForFechas(dde.Date, hta.Date.AddDays(1), art.IdArticulo);
				foreach (DataRow dr in dtFacts.Rows)
				{
					DateTime fecha = DateTime.Parse(dr["Dia"].ToString());
					cantidad_fecha_art[art.IdArticulo].Add(fecha, decimal.Parse(dr["Cantidad"].ToString()));
				}
			}
			for (dde = dde.Date; dde <= hta; dde = dde.AddDays(1))
			{
				r = reporte.AddRow();
				r[0].Value = dde.ToShortDateString();
				int i = 1;
				foreach (var art in articulos)
				{
					if (cantidad_fecha_art[art.IdArticulo].ContainsKey(dde))
						r[i].Value = cantidad_fecha_art[art.IdArticulo][dde];
					else
						r[i].Value = 0;
					i++;
				}
			}
			int rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "TOTALES";
			for (int i = 1; i < columnas; i++)
			{
				r[i].Value = FormulaHelper.Formula("SUM", new Range(reporte[i, 1], reporte[i, rFinal - 1]));
			}
			for (int i = 0; i < columnas; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				//r[i].DisplayFormat = DisplayFormatType.Currency;
			}
		}

		public void HacerReporteCombustiblesViejo(DateTime dde, DateTime hta, string nbreHoja)
		{
			Row r;
			int columnas = 1;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Combustibles " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add(nbreHoja);
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy";
			reporte.Columns(0).Style = estilo;
			//
			DataTable dtArticulos = Articulo.GetAll();
			List<Articulo> articulos = new List<Articulo>();
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				articulos.Add(new Articulo(dtArticulos.Rows[i]));
				estilo = new Yogesh.ExcelXml.XmlStyle();
				estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
				estilo.CustomFormatString = "0.000";
				estilo.Alignment.Horizontal = HorizontalAlignment.Right;
				reporte.Columns(i).Style = estilo;
				columnas++;
			}
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[0].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[0].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(0).Width = 73 / 1.333333;
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				Articulo art = new Articulo(dtArticulos.Rows[i]);
				rEnca[i].Value = art.Descripcion;
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = 84 / 1.333333;
			}
			for (dde = dde.Date; dde <= hta; dde = dde.AddDays(1))
			{
				r = reporte.AddRow();
				r[0].Value = dde.ToShortDateString();
				int i = 1;
				foreach (var art in articulos)
				{
					DataTable dtFacts = FactEncabezado.GetTotalxArticuloForFechas(dde, dde.AddDays(1), art.IdArticulo);
					if (dtFacts.Rows.Count > 0)
						r[i].Value = dtFacts.Rows[0]["Cantidad"];
					else
						r[i].Value = 0;
					i++;
				}
			}
			int rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "TOTALES";
			for (int i = 1; i < columnas; i++)
			{
				r[i].Value = FormulaHelper.Formula("SUM", new Range(reporte[i, 1], reporte[i, rFinal - 1]));
			}
			for (int i = 0; i < columnas; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				//r[i].DisplayFormat = DisplayFormatType.Currency;
			}
		}

		public void HacerLibroIVA(DateTime dde, DateTime hta, string nbreHoja)
		{
			DataTable dtFacts = FactEncabezado.GetListForFechas(dde, hta);
			DateTime ultFecha = dde.Date;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "IVA " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add(nbreHoja);
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy";
			reporte.Columns(0).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(1).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "0";
			estilo.Alignment.Horizontal = HorizontalAlignment.Right;
			reporte.Columns(2).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			reporte.Columns(5).Style = estilo;
			reporte.Columns(6).Style = estilo;
			reporte.Columns(7).Style = estilo;
			reporte.Columns(8).Style = estilo;
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[1].Value = "Letra";
			rEnca[2].Value = "Numero";
			rEnca[3].Value = "Cliente";
			rEnca[4].Value = "CUIT";
			rEnca[5].Value = "Neto";
			rEnca[6].Value = "No Gravado";
			rEnca[7].Value = "IVA";
			rEnca[8].Value = "Total";
			int[] anchos = new int[9] { 73, 42, 82, 219, 94, 93, 93, 93, 93 };
			for (int i = 0; i <= 8; i++)
			{
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = anchos[i] / 1.333333;
			}
			SortedList<FactEncabezado, DataTable> factsB = new SortedList<FactEncabezado, DataTable>();
			foreach (DataRow rFact in dtFacts.Rows)
			{
				FactEncabezado fact = new FactEncabezado(rFact);
				DataTable dtFiscales = FactItemFiscal.GetListForFactEncabezado(fact.IdFactEncabezado);
				if (fact.Fecha.Date != ultFecha.Date)
				{
					PrintB(reporte, factsB);
					factsB.Clear();
					ultFecha = fact.Fecha;
				}
				if (fact.Letra == "A")
				{
					PrintA(reporte, fact, dtFiscales);
				}
				else
				{
					if (!factsB.ContainsKey(fact))
						factsB.Add(fact, dtFiscales);
				}
			}
			if (factsB.Count > 0)
			{
				PrintB(reporte, factsB);
				factsB.Clear();
			}
			PrintTotales(reporte);
		}

		public void HacerReporteTurnos(DateTime dde, DateTime hta)
		{
			DataTable dtTurnos = Turno.GetForFechas(dde, hta);
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Turnos " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add("Reporte");
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy";
			reporte.Columns(0).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			reporte.Columns(7).Style = estilo;//Precio unit al final
			reporte.Columns(8).Style = estilo;
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[1].Value = "Turno";
			rEnca[2].Value = "Playero";
			rEnca[3].Value = " ";
			rEnca[4].Value = " ";
			rEnca[5].Value = " ";
			rEnca[6].Value = "Combustible";
			rEnca[7].Value = " ";
			rEnca[8].Value = "Total";
			for (int i = 0; i <= 8; i++)
			{
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			}
			//
			SortedList<uint, decimal> totalesXItem = new SortedList<uint, decimal>(4);
			SortedList<uint, decimal> totalTotalXItem = new SortedList<uint, decimal>(4);
			totalTotalXItem.Add(2, 0);
			totalTotalXItem.Add(3, 0);
			totalTotalXItem.Add(4, 0);
			totalTotalXItem.Add(6, 0);
			foreach (DataRow r in Articulo.GetSingle(7).Rows)
			{
				Articulo artAux = new Articulo(r);
				totalTotalXItem.Add(artAux.IdArticulo, 0);
			}
			Turno turnoPri = dtTurnos.Rows.Count > 0 ? new Turno(dtTurnos.Rows[0]) : null;
			SortedList<uint, decimal> artVistos = new SortedList<uint, decimal>();//id, precio
			int offsetResumenFinal = Reglas.Sede.ToLower().Contains("vm") ? -2 : 0;
			for (int i = 0; i < dtTurnos.Rows.Count; i++)
			{
				Turno t = new Turno(dtTurnos.Rows[i]);
				for (int surtidor = 1; surtidor <= Reglas.CantidadPuestos; surtidor++)
				{
					foreach (Cierre c in Cierre.GetListaXSurtidorTurno((uint)surtidor, t.Numero))
					{
						decimal vta = c.GetDiferencia() * c.Costo;
						if (totalesXItem.ContainsKey(Reglas.Puestos[surtidor - 1].IdArticulo))
							totalesXItem[Reglas.Puestos[surtidor - 1].IdArticulo] += vta;
						else
							totalesXItem.Add(Reglas.Puestos[surtidor - 1].IdArticulo, vta);
						if (!artVistos.ContainsKey(c.IdArticulo))
							artVistos.Add(c.IdArticulo, c.Costo);
						else
							artVistos[c.IdArticulo] = c.Costo;
					}
				}
				foreach (KeyValuePair<uint, decimal> kvp in totalesXItem)
				{
					Row r = reporte.AddRow();
					r[0].Value = t.FechaFinal.ToShortDateString();
					r[1].Value = Turno.GetTurnoNbre(t.Numero)[0].ToString();
					r[2].Value = Vendedor.GetSingleE(t.IdVendedor).Nombre;
					switch (kvp.Key)
					{
						case 2:
							r[6].Value = "N";
							break;
						case 3:
							r[6].Value = "O2";
							break;
						case 4:
							r[6].Value = "V";
							break;
						case 6:
							r[6].Value = "O3";
							break;
						case 7:
							r[6].Value = "GOM";
							break;
					}
					r[8].Value = decimal.Round(kvp.Value, 2);
					totalTotalXItem[kvp.Key] += kvp.Value;
				}
				totalesXItem.Clear();
			}
			if (turnoPri != null)
			{
				reporte.AddRow();
				Row rFinal = reporte.AddRow();
				rFinal[0].Value = "TOTALES";
				//ENCABEZADOS
				//
				rFinal = reporte.AddRow();
				rFinal[0].Value = turnoPri.FechaFinal.ToShortDateString();
				rFinal[1].Value = Turno.GetTurnoNbre(turnoPri.Numero)[0].ToString();
				rFinal[2].Value = Vendedor.GetSingleE(turnoPri.IdVendedor).Nombre;
				rFinal[6 - offsetResumenFinal].Value = "N";
				if (artVistos.ContainsKey(2))
					rFinal[7].Value = artVistos[2];
				rFinal[8].Value = decimal.Round(totalTotalXItem[2], 2);
				//
				turnoPri = new Turno(dtTurnos.Rows[dtTurnos.Rows.Count - 1]);
				rFinal = reporte.AddRow();
				rFinal[0].Value = turnoPri.FechaFinal.ToShortDateString();
				rFinal[1].Value = Turno.GetTurnoNbre(turnoPri.Numero)[0].ToString();
				rFinal[2].Value = Vendedor.GetSingleE(turnoPri.IdVendedor).Nombre;
				rFinal[6 - offsetResumenFinal].Value = "O2";
				if (artVistos.ContainsKey(3))
					rFinal[7].Value = artVistos[3];
				rFinal[8].Value = decimal.Round(totalTotalXItem[3], 2);
				//
				rFinal = reporte.AddRow();
				rFinal[6].Value = "V";
				if (artVistos.ContainsKey(4))
					rFinal[7].Value = artVistos[4];
				rFinal[8].Value = decimal.Round(totalTotalXItem[4], 2);
				rFinal = reporte.AddRow();
				rFinal[6 - offsetResumenFinal].Value = "O3";
				if (artVistos.ContainsKey(6))
					rFinal[7].Value = artVistos[6];
				rFinal[8].Value = decimal.Round(totalTotalXItem[6], 2);
				if (artVistos.ContainsKey(7))
				{
					rFinal = reporte.AddRow();
					rFinal[6 - offsetResumenFinal].Value = "GOM";
					rFinal[7].Value = artVistos[7];
					rFinal[8].Value = decimal.Round(totalTotalXItem[7], 2);
				}
			}
		}

		public void HacerReporteVales(DateTime dde, DateTime hta)
		{
			DataTable dtTurnos = Turno.GetForFechas(dde, hta);
			Row r;
			int columnas = 1;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Vales " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add("Reporte");
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy hh:mm";
			reporte.Columns(0).Style = estilo;
			//
			DataTable dtArticulos = Articulo.GetAll();
			List<Articulo> articulos = new List<Articulo>();
			//SortedList<uint, SortedList<DateTime, decimal>> cantidad_fecha_art = new SortedList<uint, SortedList<DateTime, decimal>>();
			SortedList<uint, SortedList<DateTime, ValeEntrega>> cantidad_fecha_art = new SortedList<uint, SortedList<DateTime, ValeEntrega>>();
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				Articulo art = new Articulo(dtArticulos.Rows[i]);
				articulos.Add(art);
				//cantidad_fecha_art.Add(art.IdArticulo, new SortedList<DateTime, decimal>(30));
				cantidad_fecha_art.Add(art.IdArticulo, new SortedList<DateTime, ValeEntrega>(30));
				estilo = new Yogesh.ExcelXml.XmlStyle();
				estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
				estilo.CustomFormatString = "0.000";
				estilo.Alignment.Horizontal = HorizontalAlignment.Right;
				reporte.Columns(i).Style = estilo;
				columnas++;
			}
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[0].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[0].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(0).Width = 73 / 1.333333;
			SortedList<uint, decimal> montos_x_art = new SortedList<uint, decimal>();
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				Articulo art = articulos[i - 1];
				montos_x_art.Add(art.IdArticulo, 0);
				rEnca[i].Value = art.Descripcion;
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = 84 / 1.333333;
				//
				DataTable dtVales = ValeEntrega.GetForFechasArticulo(dde, hta, art.IdArticulo);
				foreach (DataRow dr in dtVales.Rows)
				{
					//xDateTime fecha = DateTime.Parse(dr["Fecha"].ToString());
					ValeEntrega vale = new ValeEntrega(dr);
					//cantidad_fecha_art[art.IdArticulo].Add(vale.Fecha, vale.Cantidad);
					cantidad_fecha_art[art.IdArticulo].Add(vale.Fecha, vale);
				}
			}
			SortedList<DateTime, string> fechasTurnos_y_Parciales = new SortedList<DateTime, string>();
			foreach (DataRow dr in dtTurnos.Rows)
			{
				Turno turno = new Turno(dr);
				DataTable dtParciales = Parcial.GetForTurno(turno.Numero);
				foreach (DataRow drP in dtParciales.Rows)
				{
					Parcial parcial = new Parcial(drP);
					fechasTurnos_y_Parciales.Add(parcial.Fecha, "");
				}
				if (!fechasTurnos_y_Parciales.ContainsKey(turno.FechaFinal))
					fechasTurnos_y_Parciales.Add(turno.FechaFinal, "");
			}
			foreach (var kvp in fechasTurnos_y_Parciales)
			{
				DateTime fecha = kvp.Key;
				r = reporte.AddRow();
				r[0].Value = fecha;// turno.FechaFinal;
				int i = 1;
				foreach (var art in articulos)
				{
					//if (cantidad_fecha_art[art.IdArticulo].ContainsKey(turno.FechaFinal))
					if (cantidad_fecha_art[art.IdArticulo].ContainsKey(fecha))
					{
						//ValeEntrega ve = cantidad_fecha_art[art.IdArticulo][turno.FechaFinal];
						ValeEntrega ve = cantidad_fecha_art[art.IdArticulo][fecha];
						r[i].Value = ve.Cantidad;
						montos_x_art[ve.IdArticulo] += ve.Cantidad * ve.Precio;
					}
					else
						r[i].Value = 0;
					i++;
				}
			}
			//TOTAL x CANTIDAD
			int rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "TOTALES Ct";
			for (int i = 1; i < columnas; i++)
			{
				r[i].Value = FormulaHelper.Formula("SUM", new Range(reporte[i, 1], reporte[i, rFinal - 1]));
			}
			for (int i = 0; i < columnas; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				if (i > 0)
				{
					r[i].DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
					r[i].CustomFormatString = "0.000";
				}
			}
			//TT x $
			rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "TOTALES $";
			int iAux = 1;
			decimal totaTotal = 0;
			foreach (KeyValuePair<uint, decimal> kvp in montos_x_art)
			{
				totaTotal += kvp.Value;
				r[iAux].Value = kvp.Value;
				iAux++;
			}
			for (int i = 0; i < columnas; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				if (i > 0)
				{
					r[i].DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
					r[i].CustomFormatString = "0.000";
				}
			}
			//TT TT
			rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "GRAN TOTAL $";
			r[0].Border.Sides = BorderSides.Top | BorderSides.Bottom;
			r[0].Style.Font.Bold = true;
			r[1].Value = totaTotal;
			r[1].Border.Sides = BorderSides.Top | BorderSides.Bottom;
			r[1].Style.Font.Bold = true;
			r[1].DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			r[1].CustomFormatString = "0.000";
		}

		public void HacerReporteCC(DateTime dde, DateTime hta)
		{	
			Row r;
			//int columnas = 1;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Movimientos de CC " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add("Reporte");
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy hh:mm";
			reporte.Columns(0).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(1).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "0";
			estilo.Alignment.Horizontal = HorizontalAlignment.Right;
			reporte.Columns(2).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			reporte.Columns(5).Style = estilo;
			reporte.Columns(6).Style = estilo;
			reporte.Columns(7).Style = estilo;
			reporte.Columns(8).Style = estilo;
			reporte.Columns(9).Style = estilo;
			reporte.Columns(10).Style = estilo;
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[1].Value = "Turno";
			rEnca[2].Value = "Playero";
			rEnca[3].Value = "Cliente";
			rEnca[4].Value = "";
			rEnca[5].Value = " ";
			rEnca[6].Value = "   ";
			rEnca[7].Value = "Descripción";
			rEnca[8].Value = "INGRESO";
			rEnca[9].Value = "EGRESO";
			rEnca[10].Value = "Forma Pago";
			int[] anchos = new int[11] { 73, 42, 82, 219, 94, 93, 93, 93, 93, 93, 100 };
			for (int i = 0; i <= 10; i++)
			{
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = anchos[i] / 1.333333;
			}
			//
			SortedList<DateTime, Turno> turnos = new SortedList<DateTime, Turno>();
			foreach (DataRow dr in Turno.GetForFechas(dde, hta).Rows)
			{
				Turno t = new Turno(dr);
				turnos.Add(t.FechaFinal, t);
			}
			DataTable dtCC = CCMovimiento.GetListForFechas(dde, hta);
			uint ultTnoNum = 1;
			SortedList<uint, Vendedor> playeros = new SortedList<uint, Vendedor>();
			SortedList<uint, Cliente> clientes = new SortedList<uint, Cliente>();
			SortedList<uint, PagoForma> fpagos = new SortedList<uint, PagoForma>();
			SortedList<PagoForma, decimal> montosXfp = new SortedList<PagoForma, decimal>();
			foreach (DataRow dr in dtCC.Rows)
			{
				CCMovimiento ccmov = new CCMovimiento(dr);
				if (!playeros.ContainsKey(ccmov.IdVendedor))
					playeros.Add(ccmov.IdVendedor, Vendedor.GetSingleE(ccmov.IdVendedor));
				if (!clientes.ContainsKey(ccmov.IdCliente))
					clientes.Add(ccmov.IdCliente, Cliente.GetSingleE(ccmov.IdCliente));
				if (!fpagos.ContainsKey(ccmov.IdPagoForma))
				{
					var x = PagoForma.GetSingleE(ccmov.IdPagoForma);
					fpagos.Add(ccmov.IdPagoForma, x);
					montosXfp.Add(x, 0);
				}
				r = reporte.AddRow();
				r[0].Value = ccmov.Fecha;
				Turno t = null;
				foreach (var kvp in turnos)
				{
					if (ccmov.Fecha < kvp.Key)
					{
						t = kvp.Value;
						ultTnoNum = t.Numero;
						break;
					}
				}
				r[1].Value = Turno.GetTurnoNbre(t != null ? t.Numero : ultTnoNum + 1);
				r[2].Value = playeros[ccmov.IdVendedor].Nombre;
				r[3].Value = clientes[ccmov.IdCliente].Nombre;
				//rEnca[4].Value = "";
				//rEnca[5].Value = " ";
				//rEnca[6].Value = "   ";
				r[7].Value = ccmov.Descripcion;
				if (ccmov.Monto >= 0)
				{
					r[8].Value = ccmov.Monto;
					montosXfp[fpagos[ccmov.IdPagoForma]] += ccmov.Monto;
				}
				else
					r[9].Value = ccmov.Monto;
				r[10].Value = fpagos[ccmov.IdPagoForma].Descripcion;
			}
			//
			//TOTAL x CANTIDAD
			int rFinal = reporte.RowCount;
			r = reporte.AddRow();
			for (int i = 0; i < 3; i++)
				r[i].Value = reporte[i, 1].Value;//copiar 1° fila
			r[7].Value = "Total de Ventas por CC";
			r[9].Value = FormulaHelper.Formula("SUM", new Range(reporte[9, 1], reporte[9, rFinal - 1]));
			r = reporte.AddRow();//
			for (int i = 0; i < 3; i++)
				r[i].Value = reporte[i, rFinal - 1].Value;//copiar 2° fila
			foreach (var item in montosXfp)
			{
				if (item.Value == 0)
					continue;
				r[7].Value = "Ingreso " + item.Key.Descripcion + " en CC";
				r[8].Value = item.Value;
				r = reporte.AddRow();
			}
			
			
		}

		public void HacerReporteOperaciones(DateTime dde, DateTime hta)
		{
			DataTable dtOps = Operacion.GetForFechas(dde, hta);
			Row r;
			int columnas = 1;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Vales " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add("Reporte");
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy hh:mm";
			reporte.Columns(0).Style = estilo;
			//
			DataTable dtArticulos = Articulo.GetAll();
			List<Articulo> articulos = new List<Articulo>();
			//SortedList<uint, SortedList<DateTime, decimal>> cantidad_fecha_art = new SortedList<uint, SortedList<DateTime, decimal>>();
			SortedList<uint, SortedList<DateTime, ValeEntrega>> cantidad_fecha_art = new SortedList<uint, SortedList<DateTime, ValeEntrega>>();
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				Articulo art = new Articulo(dtArticulos.Rows[i]);
				articulos.Add(art);
				//cantidad_fecha_art.Add(art.IdArticulo, new SortedList<DateTime, decimal>(30));
				cantidad_fecha_art.Add(art.IdArticulo, new SortedList<DateTime, ValeEntrega>(30));
				estilo = new Yogesh.ExcelXml.XmlStyle();
				estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
				estilo.CustomFormatString = "0.000";
				estilo.Alignment.Horizontal = HorizontalAlignment.Right;
				reporte.Columns(i).Style = estilo;
				columnas++;
			}
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[0].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[0].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(0).Width = 73 / 1.333333;
			SortedList<uint, decimal> montos_x_art = new SortedList<uint, decimal>();
			for (int i = 1; i < dtArticulos.Rows.Count; i++)
			{
				Articulo art = articulos[i - 1];
				montos_x_art.Add(art.IdArticulo, 0);
				rEnca[i].Value = art.Descripcion;
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = 84 / 1.333333;
				//
				DataTable dtVales = ValeEntrega.GetForFechasArticulo(dde, hta, art.IdArticulo);
				foreach (DataRow dr in dtVales.Rows)
				{
					//xDateTime fecha = DateTime.Parse(dr["Fecha"].ToString());
					ValeEntrega vale = new ValeEntrega(dr);
					//cantidad_fecha_art[art.IdArticulo].Add(vale.Fecha, vale.Cantidad);
					cantidad_fecha_art[art.IdArticulo].Add(vale.Fecha, vale);
				}
			}
			SortedList<DateTime, string> fechasTurnos_y_Parciales = new SortedList<DateTime, string>();
			foreach (DataRow dr in dtOps.Rows)
			{
				Turno turno = new Turno(dr);
				DataTable dtParciales = Parcial.GetForTurno(turno.Numero);
				foreach (DataRow drP in dtParciales.Rows)
				{
					Parcial parcial = new Parcial(drP);
					fechasTurnos_y_Parciales.Add(parcial.Fecha, "");
				}
				if (!fechasTurnos_y_Parciales.ContainsKey(turno.FechaFinal))
					fechasTurnos_y_Parciales.Add(turno.FechaFinal, "");
			}
			foreach (var kvp in fechasTurnos_y_Parciales)
			{
				DateTime fecha = kvp.Key;
				r = reporte.AddRow();
				r[0].Value = fecha;// turno.FechaFinal;
				int i = 1;
				foreach (var art in articulos)
				{
					//if (cantidad_fecha_art[art.IdArticulo].ContainsKey(turno.FechaFinal))
					if (cantidad_fecha_art[art.IdArticulo].ContainsKey(fecha))
					{
						//ValeEntrega ve = cantidad_fecha_art[art.IdArticulo][turno.FechaFinal];
						ValeEntrega ve = cantidad_fecha_art[art.IdArticulo][fecha];
						r[i].Value = ve.Cantidad;
						montos_x_art[ve.IdArticulo] += ve.Cantidad * ve.Precio;
					}
					else
						r[i].Value = 0;
					i++;
				}
			}
			//TOTAL x CANTIDAD
			int rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "TOTALES Ct";
			for (int i = 1; i < columnas; i++)
			{
				r[i].Value = FormulaHelper.Formula("SUM", new Range(reporte[i, 1], reporte[i, rFinal - 1]));
			}
			for (int i = 0; i < columnas; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				if (i > 0)
				{
					r[i].DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
					r[i].CustomFormatString = "0.000";
				}
			}
			//TT x $
			rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "TOTALES $";
			int iAux = 1;
			decimal totaTotal = 0;
			foreach (KeyValuePair<uint, decimal> kvp in montos_x_art)
			{
				totaTotal += kvp.Value;
				r[iAux].Value = kvp.Value;
				iAux++;
			}
			for (int i = 0; i < columnas; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				if (i > 0)
				{
					r[i].DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
					r[i].CustomFormatString = "0.000";
				}
			}
			//TT TT
			rFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "GRAN TOTAL $";
			r[0].Border.Sides = BorderSides.Top | BorderSides.Bottom;
			r[0].Style.Font.Bold = true;
			r[1].Value = totaTotal;
			r[1].Border.Sides = BorderSides.Top | BorderSides.Bottom;
			r[1].Style.Font.Bold = true;
			r[1].DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			r[1].CustomFormatString = "0.000";
		}

		public void HacerReporteGobierno(DateTime dde, DateTime hta)
		{
			Row r;
			//resolvi por crear un xls nuevo, y no basado en un existente
			Nombre = "Cobranza Gobierno " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add("Reporte");
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy hh:mm";
			reporte.Columns(0).Style = estilo;
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Text;
			estilo.Alignment.Horizontal = HorizontalAlignment.Left;
			reporte.Columns(1).Style = estilo;
			for (int i = 2; i <= 5; i++)
			{
				estilo = new Yogesh.ExcelXml.XmlStyle();
				estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
				estilo.CustomFormatString = "0.000";
				estilo.Alignment.Horizontal = HorizontalAlignment.Right;
				reporte.Columns(i).Style = estilo;
			}
			//
			DataTable dtArticulos = Articulo.GetAll();
			List<Articulo> articulos = new List<Articulo>();
			SortedList<int, uint> cod_idarticulos = new SortedList<int, uint>();
			SortedList<uint, decimal> cantidad_vendida_vale = new SortedList<uint, decimal>();
			SortedList<uint, decimal> cantidad_vendida_gob = new SortedList<uint, decimal>();
			SortedList<uint, Articulo> id_art = new SortedList<uint, Articulo>();
			for (int i = 1; i < dtArticulos.Rows.Count; i++)//ver articulos
			{
				Articulo art = new Articulo(dtArticulos.Rows[i]);
				articulos.Add(art);
				id_art.Add(art.IdArticulo, art);
				cod_idarticulos.Add(art.CodProducto, art.IdArticulo);
				cantidad_vendida_vale.Add(art.IdArticulo, 0);
				cantidad_vendida_gob.Add(art.IdArticulo, 0);
			}
			DataTable dtOps = Operacion.GetForFechas(dde.Date, hta.Date.AddDays(1));//operaciones en el periodo
			foreach (DataRow dr in dtOps.Rows)//operaciones
			{
				Operacion op = new Operacion(dr);
				cantidad_vendida_vale[cod_idarticulos[op.CodProducto]] += op.Cantidad;
			}
			uint idGob = Negocio.Reglas.ClienteGobierno;
			foreach (Articulo a in articulos)//ventas
			{
				foreach (DataRow drFact in FactEncabezado.GetListForFechasIdClienteIdArticulo(dde.Date, hta.Date.AddDays(1), idGob, a.IdArticulo).Rows)
				{
					FactEncabezado fact = new FactEncabezado(drFact);
					fact.GetFilas();
					foreach (FactItem fi in fact.Cuerpo)
					{
						if (fi.IdArticulo == a.IdArticulo)
							cantidad_vendida_gob[a.IdArticulo] += fi.Cantidad;
					}
				}
			}
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[0].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[0].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(0).Width = 73 / 1.333333;
			rEnca[1].Value = "Combustible";
			rEnca[1].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[1].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(1).Width = 85 / 1.333333;
			rEnca[2].Value = "Precio";
			rEnca[2].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[2].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(2).Width = 73 / 1.333333;
			rEnca[3].Value = "Lts/M3";
			rEnca[3].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[3].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(3).Width = 73 / 1.333333;
			rEnca[4].Value = "Importe";
			rEnca[4].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[4].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(4).Width = 73 / 1.333333;
			rEnca[5].Value = "Total";
			rEnca[5].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rEnca[5].Alignment.Horizontal = HorizontalAlignment.Center;
			reporte.Columns(5).Width = 73 / 1.333333;
			//
			Range rango = null;
			Cell fInicioRango = null;
			Cell fFinalRango = null;
			int inicio = reporte.RowCount;
			int final = inicio;
			Row rFinal = null;
			foreach (var kvp in cantidad_vendida_vale)
			{
				DateTime fecha = dde;
				r = reporte.AddRow();
				rFinal = r;
				final++;
				fFinalRango = r[0];
				if (fInicioRango == null)
				{
					fInicioRango = r[0];
					fInicioRango.Style.Alignment.Vertical = VerticalAlignment.Center;
					fInicioRango.Value = dde.Date.ToShortDateString()+" al "+hta.Date.ToShortDateString();
				}
				r[1].Value = id_art[kvp.Key].Descripcion;
				r[2].Value = id_art[kvp.Key].Precio;
				r[3].Value = kvp.Value;
				r[4].Value = id_art[kvp.Key].Precio * kvp.Value;
			}
			rango = new Range(fInicioRango, fFinalRango);
			rango.Alignment.Vertical = VerticalAlignment.Distributed;
			bool b = rango.Merge();
			rFinal[5].Value = FormulaHelper.Formula("SUM", new Range(reporte[4, inicio], reporte[4, final - 1]));
			fInicioRango = null;
			fFinalRango = null;
			inicio = reporte.RowCount;
			final = inicio;
			foreach (var kvp in cantidad_vendida_gob)
			{
				DateTime fecha = dde;
				r = reporte.AddRow();
				final++;
				rFinal = r;
				fFinalRango = r[0];
				if (fInicioRango == null)
				{
					fInicioRango = r[0];
					fInicioRango.Style.Alignment.Vertical = VerticalAlignment.Center;
					fInicioRango.Value = "TK sin sistema";
				}
				r[1].Value = id_art[kvp.Key].Descripcion;
				r[2].Value = id_art[kvp.Key].Precio;
				r[3].Value = kvp.Value;
				r[4].Value = id_art[kvp.Key].Precio * kvp.Value;
			}
			rango = new Range(fInicioRango, fFinalRango);
			rango.Alignment.Vertical = VerticalAlignment.Distributed;
			b = rango.Merge();
			rFinal[5].Value = FormulaHelper.Formula("SUM", new Range(reporte[4, inicio], reporte[4, final - 1]));
			new Range(fInicioRango, fFinalRango).Merge();
			//TOTAL x CANTIDAD
			int iRowFinal = reporte.RowCount;
			r = reporte.AddRow();
			r[0].Value = "TOTALES";
			r[0].Style.Alignment.Horizontal = HorizontalAlignment.Center;
			new Range(r[0], r[4]).Merge();
			r[5].Value = FormulaHelper.Formula("SUM", new Range(reporte[4, 1], reporte[4, iRowFinal - 1]));
			for (int i = 0; i <= 5; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				if (i == 5)
				{
					r[i].DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
					r[i].CustomFormatString = "0.000";
				}
			}
		}

		public string HacerReporteInformesZ(DateTime dde, DateTime hta, string nbreHoja)
		{
			string error1 = "";
			List<int> zFaltantes = new List<int>();
			DataTable dtZCierres = ZCierre.GetListaXFechas(dde, hta);
			DateTime ultFecha = dde.Date;
			Nombre = "Informes Z " + dde.ToShortDateString() + " - " + hta.ToShortDateString();
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add(String.IsNullOrEmpty(nbreHoja) ? "Reporte" : nbreHoja);
			//
			//Estilos
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "dd/mm/yyyy";
			reporte.Columns(0).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "0";
			estilo.Alignment.Horizontal = HorizontalAlignment.Right;
			reporte.Columns(1).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			for (int i = 2; i <= 16; i++)
				reporte.Columns(i).Style = estilo;
			//
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estilo.CustomFormatString = "0";
			estilo.Alignment.Horizontal = HorizontalAlignment.Right;
			reporte.Columns(7).Style = estilo;
			reporte.Columns(8).Style = estilo;
			reporte.Columns(9).Style = estilo;
			reporte.Columns(10).Style = estilo;
			reporte.Columns(11).Style = estilo;
			reporte.Columns(12).Style = estilo;
			//
			reporte.Columns(17).Style = estilo;
			reporte.Columns(18).Style = estilo;
			reporte.Columns(19).Style = estilo;
			//
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			rEnca[0].Value = "Fecha";
			rEnca[1].Value = "Numero";
			rEnca[2].Value = "Venta Diaria";
			rEnca[3].Value = "IVA Diario";
			rEnca[4].Value = "No Gravado";
			rEnca[5].Value = "Venta Neta";//NEW
			rEnca[6].Value = "Percepciones";
			rEnca[7].Value = "Ultimo A Emitido";
			rEnca[8].Value = "Ultimo B/C Emitido";
			rEnca[9].Value = "Comp. Fiscales Emitidos";
			rEnca[10].Value = "Comp. Fiscales Cancelados";
			rEnca[11].Value = "Doc. No Fiscales Emitidos";
			rEnca[12].Value = "Doc. No Fiscales Cancelados";
			rEnca[13].Value = "Crédito Diaria";
			rEnca[14].Value = "IVA Diario";
			rEnca[15].Value = "No Gravado";
			rEnca[16].Value = "Percepciones";
			rEnca[17].Value = "Ultima NC A Emitida";
			rEnca[18].Value = "Ultima NC B/C Emitida";
			rEnca[19].Value = "Notas de Crédito Emitidas";
			int[] anchos = new int[20] { 73, 65, 87, 87, 87, 87, 93, 110, 125, 158, 178,
				167, 188, 95, 95, 95, 95, 131, 145, 174};
			for (int i = 0; i < anchos.Length; i++)
			{
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				reporte.Columns(i).Width = anchos[i] / 1.333333;
			}
			Row rInicial = null, rFinal = null;
			int numZAnt = 0, numZ = 0;
			foreach (DataRow rZeta in dtZCierres.Rows)
			{
				ZCierre z = new ZCierre(rZeta);
				Row r = reporte.AddRow();
				if (rInicial == null)
				{
					rInicial = r;
					if (!int.TryParse(z.NumeroZ, out numZAnt))
						numZAnt = 0;
					if (z.Fecha.Date > dde)
						error1 = "El primer Z no tiene la misma fecha que se especificó como 'Desde'.";
				}
				else
				{
					if (!int.TryParse(z.NumeroZ, out numZ))
						numZ = 0;
					if (numZ != 0 && numZAnt != 0)
					{
						for (int k = numZAnt + 1; k < numZ; k++)
							zFaltantes.Add(k);
					}
					numZAnt = numZ;
				}
				rFinal = r;
				r[0].Value = z.Fecha.ToShortDateString();
				r[1].Value = z.NumeroZ;
				r[2].Value = z.MontoVentasFiscal;
				r[3].Value = z.MontoIVAFiscal + z.MontoIVANoInscriptoFiscal;
				r[4].Value = z.MontoImpIntFiscal;
				r[5].Value = z.MontoVentasFiscal - (z.MontoIVAFiscal + z.MontoIVANoInscriptoFiscal + z.MontoImpIntFiscal);//FORMULA
				r[6].Value = z.MontoPercepcionesFiscal;
				r[7].Value = z.UltimoFiscalA;
				r[8].Value = z.UltimoFiscalBC;
				r[9].Value = z.CantDFEmitidos;
				r[10].Value = z.CantDFCancelados;
				r[11].Value = z.CantDNFEmitidos;
				r[12].Value = "-";
				r[13].Value = z.MontoVentasNotaCredito;
				r[14].Value = z.MontoIVANotaCredito + z.MontoIVANoInscriptoNotaCredito;
				r[15].Value = z.MontoImpIntNotaCredito;
				r[16].Value = z.MontoPercepcionesNotaCredito;
				r[17].Value = z.UltimaNotaCreditoA;
				r[18].Value = z.UltimaNotaCreditoBC;
				r[19].Value = z.CantNCAEmitidos + z.CantNCBCEmitidos;
			}
			if (dtZCierres.Rows.Count == 0)
				return error1;
			Row rTotales = reporte.AddRow();
			//rTotales[0].Value = "Fecha";
			rTotales[1].Value = "TOTALES";
			rTotales[1].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rTotales[1].Style.Font.Bold = true;
			for (int i = 2; i <= 6; i++)
			{
				rTotales[i].Value = FormulaHelper.Formula("SUM", new Range(rInicial[i], rFinal[i]));
				rTotales[i].Style = rInicial[i].Style;
				rTotales[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			}
			for (int i = 9; i <= 16; i++)
			{
				rTotales[i].Value = FormulaHelper.Formula("SUM", new Range(rInicial[i], rFinal[i]));
				rTotales[i].Style = rInicial[i].Style;
				rTotales[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			}
			rTotales[19].Value = FormulaHelper.Formula("SUM", new Range(rInicial[19], rFinal[19]));
			rTotales[19].Style = rInicial[19].Style;
			rTotales[19].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			string errFaltantes = "";
			if (zFaltantes.Count > 0)
			{
				errFaltantes = "Faltan los siguientes Z's: ";
				foreach (int n in zFaltantes)
				{
					errFaltantes += n.ToString() + ", ";
				}
				errFaltantes = errFaltantes.Substring(0, errFaltantes.Length - 2);
			}
			if (error1 != "" && errFaltantes != "")
				return error1 + " " + errFaltantes;
			else
				return error1 + errFaltantes;
		}

		public void HacerReporteXls(DataTable dt, string nombre)
		{
			Nombre = nombre;
			_libro = new Yogesh.ExcelXml.ExcelXmlWorkbook();
			Worksheet reporte = _libro.Add("Reporte");
			//
			//Estilos
			XmlStyle estiloFecha = new Yogesh.ExcelXml.XmlStyle();
			estiloFecha.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estiloFecha.CustomFormatString = "dd/mm/yyyy";
			XmlStyle estiloMoneda = new Yogesh.ExcelXml.XmlStyle();
			estiloMoneda.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Currency;
			estiloMoneda.Alignment.Horizontal = HorizontalAlignment.Right;
			XmlStyle estiloNum = new Yogesh.ExcelXml.XmlStyle();
			estiloNum.DisplayFormat = Yogesh.ExcelXml.DisplayFormatType.Custom;
			estiloNum.CustomFormatString = "0";
			estiloNum.Alignment.Horizontal = HorizontalAlignment.Right;
			//
			int i = 0;
			foreach (DataColumn c in dt.Columns)
			{
				if (c.DataType == typeof(DateTime))
					reporte.Columns(i).Style = estiloFecha;
				else if (c.DataType.Name.ToLower().Contains("int"))
					reporte.Columns(i).Style = estiloNum;
				else if (c.DataType == typeof(Decimal))
					reporte.Columns(i).Style = estiloMoneda;
				else
					reporte.Columns(i);
			}
			//encabezados
			Yogesh.ExcelXml.Row rEnca = reporte.AddRow();
			XmlStyle estilo = new Yogesh.ExcelXml.XmlStyle();
			estilo.Font.Bold = true;
			rEnca.Style = estilo;
			i = 0;
			foreach (DataColumn c in dt.Columns)
			{
				rEnca[i].Value = c.ColumnName;
				rEnca[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				rEnca[i].Alignment.Horizontal = HorizontalAlignment.Center;
				i++;
			}
			Row rInicial = null, rFinal = null;
			foreach (DataRow filaDt in dt.Rows)
			{
				Row r = reporte.AddRow();
				if (rInicial == null)
					rInicial = r;
				rFinal = r;
				i = 0;
				foreach (DataColumn c in dt.Columns)
				{
					r[i].Value = filaDt[c];
					i++;
				}
			}
			if (dt.Rows.Count == 0)
				return;
			Row rTotales = reporte.AddRow();
			rTotales[0].Value = "TOTALES";
			rTotales[0].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
			rTotales[0].Style.Font.Bold = true;
			for (i = 1; i < dt.Columns.Count; i++)
			{
				DataColumn c = dt.Columns[i];
				if (c.DataType == typeof(Decimal))
				{
					rTotales[i].Value = FormulaHelper.Formula("SUM", new Range(rInicial[i], rFinal[i]));
					rTotales[i].Style = rInicial[i].Style;
					rTotales[i].Border.Sides = Yogesh.ExcelXml.BorderSides.All;
				}
			}
		}

		public string Grabar()
		{
			return Grabar(AppConfig.CarpetaReportes + "\\" + Nombre + ".xls");
		}

		public string Grabar(string filePath)
		{
			_libro.Export(filePath);
			return filePath;
		}

		/// <summary>
		/// Imprime una linea A con los FactItems
		/// </summary>
		Row PrintA(Worksheet hoja, FactEncabezado fact)
		{
			Row r = hoja.AddRow();
			r[0].Value = fact.Fecha.ToShortDateString();
			r[1].Value = fact.Letra;
			int num;
			if (int.TryParse(fact.Numero, out num))
				r[2].Value = num;
			else
				r[2].Value = fact.Numero;
			Cliente cli = Cliente.GetSingleE(fact.IdCliente);
			r[3].Value = cli.Nombre;
			r[4].Value = cli.CUIT;
			decimal neto = 0, iva = 0, total = 0, ii = 0;
			foreach (FactItem it in fact.Cuerpo)
			{
				neto += it.Precio * it.Cantidad;
				iva += it.IVA * it.Cantidad;
				total += it.Monto;
				ii += it.Impuestos * it.Cantidad;
			}
			PrintMontos(r, neto, ii, iva, total);
			return r;
		}

		/// <summary>
		/// Imprime una linea B con los FactItems
		/// </summary>
		Row PrintB(Worksheet hoja, FactEncabezado[] factsB)
		{
			decimal neto = 0, iva = 0, total = 0, ii = 0;
			foreach (FactEncabezado f in factsB)
			{
				foreach (FactItem it in f.Cuerpo)
				{
					neto += it.Precio * it.Cantidad;
					iva += it.IVA * it.Cantidad;
					total += it.Monto;
					ii += it.Impuestos * it.Cantidad;
				}
			}
			if (factsB.Length > 0)
			{
				Row r = hoja.AddRow();
				r[0].Value = factsB[0].Fecha.ToShortDateString();
				r[1].Value = "B";
				int num, num2;
				if (int.TryParse(factsB[0].Numero, out num) && int.TryParse(factsB[factsB.Length - 1].Numero, out num2))
					r[2].Value = num + "/" + num2;
				else
					r[2].Value = factsB[0].Numero + "/" + factsB[factsB.Length - 1].Numero;
				Cliente cli = Cliente.ConsFinal;
				r[3].Value = cli.Nombre;
				r[4].Value = cli.CUIT;
				PrintMontos(r, neto, ii, iva, total);
				return r;
			}
			else return null;
		}

		/// <summary>
		/// Imprime una linea A con los FactItemsFiscales
		/// </summary>
		Row PrintA(Worksheet hoja, FactEncabezado fact, DataTable dtFiscales)
		{
			Row r = hoja.AddRow();
			r[0].Value = fact.Fecha.ToShortDateString();
			r[1].Value = fact.Letra;
			int num;
			if (int.TryParse(fact.Numero, out num))
				r[2].Value = num;
			else
				r[2].Value = fact.Numero;
			Cliente cli = Cliente.GetSingleE(fact.IdCliente);
			r[3].Value = cli.Nombre;
			r[4].Value = cli.CUIT;
			decimal neto = 0, iva = 0, ii = 0, cant = 0;
			decimal k, iiAux, montoBase, ivaAux;
			foreach (DataRow it in dtFiscales.Rows)
			{
				switch (it["ModoImpresion"].ToString())
				{
					case "A":
						cant = decimal.Parse(it["Cantidad"].ToString());
						neto += decimal.Parse(it["Monto"].ToString()) * cant;
						iva += decimal.Parse(it["Monto"].ToString()) * decimal.Parse(it["IVA"].ToString()) / 100 * cant;
						ii += decimal.Parse(it["ImpInt"].ToString()) * cant;
						//total = neto + iva + ii;
						break;
					case "D":
						cant = decimal.Parse(it["Cantidad"].ToString());
						neto += decimal.Parse(it["Monto"].ToString()) * cant;
						iva += decimal.Parse(it["Monto"].ToString()) * decimal.Parse(it["IVA"].ToString()) / 100 * cant;
						//double k = 1 / (1 + impint / 100);
						// 1 + ii/100 = 1/k
						// ii = 100/k - 100
						k = decimal.Parse(it["ImpInt"].ToString());// *cant;
						ii += decimal.Parse(it["Monto"].ToString()) / 100 * (100 / k - 100) * cant;
						//total = neto + iva + ii;
						break;
					case "E":
						//double k = 1 / (1 + impint / 100);
						// 1 + ii/100 = 1/k
						// ii = 100/k - 100
						k = decimal.Parse(it["ImpInt"].ToString());
						iiAux = 100 / k - 100;//en %
						string ctAux = it["Descripcion"].ToString();
						cant = decimal.Parse(ctAux.Split('/')[0].Replace("M3", "").Replace("Lts", ""));
						montoBase = decimal.Parse(it["Monto"].ToString()) / (100 + iiAux + decimal.Parse(it["IVA"].ToString()))
							* 100;//cant incluida
						neto += montoBase;
						iva += montoBase * decimal.Parse(it["IVA"].ToString()) / 100;
						ii += montoBase * iiAux / 100;
						break;
					case "F":
						ivaAux = decimal.Parse(it["IVA"].ToString());
						cant = decimal.Parse(it["Cantidad"].ToString());
						iiAux = decimal.Parse(it["ImpInt"].ToString());
						ii += iiAux * cant;
						montoBase = decimal.Parse(it["Monto"].ToString()) - iiAux;
						montoBase = montoBase / (100 + ivaAux) * 100;
						neto += montoBase * cant;
						iva += montoBase * ivaAux / 100 * cant;
						break;
				}
			}
			//PrintMontos(r, neto, ii, iva, total);
			PrintMontos(r, neto, ii, iva, neto + iva + ii);
			return r;
		}

		/// <summary>
		/// Imprime una linea B con los FactItemsFiscales
		/// </summary>
		Row PrintB(Worksheet hoja, IDictionary<FactEncabezado, DataTable> factsB)
		{
			decimal neto = 0, iva = 0, ii = 0, cant = 0, k = 0, iiAux, montoBase, ivaAux;
			FactEncabezado pri = null, ult = null;
			foreach (KeyValuePair<FactEncabezado, DataTable> f in factsB)
			{
				if (pri == null)
					pri = f.Key;
				ult = f.Key;
				foreach (DataRow it in f.Value.Rows)
				{
					switch (it["ModoImpresion"].ToString())
					{
						case "A":
							cant = decimal.Parse(it["Cantidad"].ToString());
							neto += decimal.Parse(it["Monto"].ToString()) * cant;
							iva += decimal.Parse(it["Monto"].ToString()) * decimal.Parse(it["IVA"].ToString()) / 100 * cant;
							ii += decimal.Parse(it["ImpInt"].ToString()) * cant;
							//total = neto + iva + ii;
							break;
						case "D":
							cant = decimal.Parse(it["Cantidad"].ToString());
							neto += decimal.Parse(it["Monto"].ToString()) * cant;
							iva += decimal.Parse(it["Monto"].ToString()) * decimal.Parse(it["IVA"].ToString()) / 100 * cant;
							//double k = 1 / (1 + impint / 100);
							// 1 + ii/100 = 1/k
							// ii = 100/k - 100					
							k = decimal.Parse(it["ImpInt"].ToString());// *cant;
							ii += decimal.Parse(it["Monto"].ToString()) / 100 * (100 / k - 100) * cant;
							//total = neto + iva + ii;
							break;
						case "E":
							//double k = 1 / (1 + impint / 100);
							// 1 + ii/100 = 1/k
							// ii = 100/k - 100
							k = decimal.Parse(it["ImpInt"].ToString());
							iiAux = 100 / k - 100;//en %
							string ctAux = it["Descripcion"].ToString();
							cant = decimal.Parse(ctAux.Split('/')[0].Replace("M3", "").Replace("Lts", ""));
							montoBase = decimal.Parse(it["Monto"].ToString()) / (100 + iiAux + decimal.Parse(it["IVA"].ToString()))
								* 100;//cant incluida
							neto += montoBase;
							iva += montoBase * decimal.Parse(it["IVA"].ToString()) / 100;
							ii += montoBase * iiAux / 100;
							break;
						case "F":
							ivaAux = decimal.Parse(it["IVA"].ToString());
							cant = decimal.Parse(it["Cantidad"].ToString());
							iiAux = decimal.Parse(it["ImpInt"].ToString());
							ii += iiAux * cant;
							montoBase = decimal.Parse(it["Monto"].ToString()) - iiAux;
							montoBase = montoBase / (100 + ivaAux) * 100;
							neto += montoBase * cant;
							iva += montoBase * ivaAux / 100 * cant;
							break;
					}
				}
			}
			if (factsB.Count > 0)
			{
				Row r = hoja.AddRow();
				r[0].Value = pri.Fecha.ToShortDateString();
				r[1].Value = "B";
				int num, num2;
				if (int.TryParse(pri.Numero, out num) && int.TryParse(ult.Numero, out num2))
					r[2].Value = num + "/" + num2;
				else
					r[2].Value = pri.Numero + "/" + ult.Numero;
				Cliente cli = Cliente.ConsFinal;
				r[3].Value = cli.Nombre;
				r[4].Value = cli.CUIT;
				PrintMontos(r, neto, ii, iva, neto + iva + ii);
				return r;
			}
			else return null;
		}

		/// <summary>
		/// Imprime los montos
		/// </summary>
		void PrintMontos(Row r, decimal neto, decimal ii, decimal iva, decimal total)
		{
			r[5].Value = decimal.Round(neto, 2);
			r[6].Value = decimal.Round(ii, 2);
			r[7].Value = decimal.Round(iva, 2);
			r[8].Value = decimal.Round(total, 2);
		}

		/// <summary>
		/// Imprime los totales
		/// </summary>
		void PrintTotales(Worksheet hoja)
		{
			int rFinal = hoja.RowCount;
			Row r = hoja.AddRow();
			r[3].Value = "TOTALES";
			r[4].Value = "";
			for (int i = 5; i <= 8; i++)
			{
				r[i].Value = FormulaHelper.Formula("SUM", new Range(hoja[i, 1], hoja[i, rFinal - 1]));
			}
			for (int i = 3; i <= 8; i++)
			{
				r[i].Border.Sides = BorderSides.Top | BorderSides.Bottom;
				r[i].Style.Font.Bold = true;
				r[i].DisplayFormat = DisplayFormatType.Currency;
			}
		}
	}
}
