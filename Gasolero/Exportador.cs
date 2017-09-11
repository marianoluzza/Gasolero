using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormCom;
using OfficeOpenXml;
using System.Data;
using MarUtils.Soporte;
using System.Windows.Forms;
using Gios.Pdf;
using System.Drawing;
using System.Diagnostics;

namespace Gasolero
{
	public class Exportador : IABMAddOn
	{
		SortedList<string, DataView> vistas = new SortedList<string, DataView>();
		SortedList<string, SortedList<uint, string>> fks = new SortedList<string, SortedList<uint, string>>();
		List<Esquema> esquemas;
		bool _menuPDF = true;
		bool _menuXLSX = true;

		public string Titulo
		{
			get;
			set;
		}

		public Exportador()
		{
			Titulo = "";
		}

		public Exportador(bool aPDF, bool aXLS, string titulo = "")
		{
			_menuPDF = aPDF;
			_menuXLSX = aXLS;
			Titulo = titulo;
		}

		#region IABMAddOn
		public IABMTool ABMTool
		{
			get;
			private set;
		}

		public System.Windows.Forms.Control Control
		{
			get { return null; }
		}

		public System.Drawing.Size GetPreferredSize(System.Drawing.Size tamPropuesto)
		{
			return tamPropuesto;
		}

		public string Nombre
		{
			get { return "Exportar"; }
		}

		public void OnFinit(AddOnQuitInfo info)
		{
			throw new NotImplementedException();
		}

		public void OnInit(IABMTool abm)
		{
			ABMTool = abm;
			//esquemas = abm.Esquemas.SortByPosicion();
			esquemas = abm.Esquemas.SortByPosicion().Where(esq => esq.Visible).ToList();
			ToolStripMenuItem menuExportar = new ToolStripMenuItem("Exportar");
			ToolStripMenuItem item = new ToolStripMenuItem("Exportar a Excel", Recursos.Excel, Excel);
			menuExportar.DropDownItems.Add(item);
			item = new ToolStripMenuItem("Exportar a PDF", Recursos.PDF, PDF);
			menuExportar.DropDownItems.Add(item);
			abm.AgregarMenu(menuExportar, enMenuPosicion.BarraHerramientas);
		}

		public ABMAddOnPosicion Posicion
		{
			get { return ABMAddOnPosicion.Ninguna; }
		}
		#endregion IABMAddOn

		void Excel(object sender, EventArgs e)
		{
			ToExcel();
		}

		void PDF(object sender, EventArgs e)
		{
			ToPDF();
		}

		void ClearFks()
		{
			foreach (var x in fks)
				x.Value.Clear();
		}

		object GetValue(object o, Esquema esq)
		{
			string aux;
			if (o == null)
				return "";
			aux = o.ToString();
			switch (esq.Tipo)
			{
				case Esquema.enControles.CheckBox:
					aux = aux.ToLower().Trim();
					if (aux == "si" || aux == "1" || aux == "true")
						return "SI";
					else
						return "NO";
				case Esquema.enControles.ComboBox:
					if (!vistas.ContainsKey(esq.Tabla))
					{
						vistas.Add(esq.Tabla, ABMTool.ObtenerVista(esq.Tabla));
						fks.Add(esq.Tabla, new SortedList<uint, string>());
					}
					DataView v = vistas[esq.Tabla];
					uint id = uint.Parse(aux);
					if (fks[esq.Tabla].ContainsKey(id))
						return fks[esq.Tabla][id];
					v.Sort = esq.TablaId + " ASC";
					int find = v.Find(id);
					if (find >= 0)
					{
						aux = v[find][esq.TablaDisplay].ToString();
						fks[esq.Tabla].Add(id, aux);
						return aux;
					}
					else
						return "";
				case Esquema.enControles.Money:
				case Esquema.enControles.DateTimePicker:
					return o;
				default:
					return aux;
			}
		}

		public void ToExcel(string nbre = "")
		{
			string reportePath = "";
			ClearFks();
			try
			{
				ExcelPackage pak = new ExcelPackage();
				var hoja = pak.Workbook.Worksheets.Add("Reporte");
				DataTable tabla = ABMTool.ObtenerVista(ABMTool.TablaMain).ToTable();
				int col = 1, row = 2;
				foreach (Esquema esq in esquemas)
				{
					//if (!esq.Visible)
					//    continue;
					hoja.Cells[1, col].Value = string.IsNullOrWhiteSpace(esq.Alias) ? esq.Dato : esq.Alias;
					hoja.Cells[1, col].Style.Font.Bold = true;
					hoja.Cells[1, col].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					hoja.Cells[1, col].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					hoja.Cells[1, col].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					hoja.Cells[1, col].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					switch (esq.Tipo)
					{
						case Esquema.enControles.DateTimePicker:
							hoja.Column(col).Style.Numberformat.Format = "dd/MM/yyyy";
							break;
						case Esquema.enControles.Money:
							hoja.Column(col).Style.Numberformat.Format = esq.Mascara;
							break;
					}
					col++;
				}
				//hoja.Cells[1, 1, 1, col - 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				//hoja.Cells[1, 1, 1, col - 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				//hoja.Cells[1, 1, 1, col - 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				//hoja.Cells[1, 1, 1, col - 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				//
				foreach (DataRow dr in tabla.Rows)
				{
					int i = 0;
					foreach (Esquema esq in esquemas)
					{
						//if (!esq.Visible)
						//    continue;
						hoja.Cells[row, i + 1].Value = GetValue(dr[esq.Dato], esq);
						i++;
					}
					row++;
				}
				hoja.Cells.AutoFitColumns(0);
				SaveFileDialog save = new SaveFileDialog();
				save.AddExtension = true;
				save.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
				save.FilterIndex = 1;
				if (nbre != "")
					save.FileName = nbre + ".xlsx";
				else
					save.FileName = ABMTool.TablaMain + ".xlsx";
				if (save.ShowDialog() != DialogResult.OK)
					return;
				pak.SaveAs(new System.IO.FileInfo(save.FileName));
				reportePath = save.FileName;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al crear reporte xlsx", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			try
			{
				if (reportePath != "")
					Process.Start(reportePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al crear abrir el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void ToPDF(bool iniciar = true, string archivoGuardar = "")
		{
			ClearFks();
			try
			{
				DataTable tabla = ABMTool.ObtenerVista().ToTable();
				DataTable dtPDF = new DataTable(tabla.TableName);
				foreach (Esquema esq in esquemas)
				{
					//dtPDF.Columns.Add(string.IsNullOrEmpty(esq.Alias) ? esq.Dato : esq.Alias, tabla.Columns[esq.Dato].DataType);
					dtPDF.Columns.Add(string.IsNullOrEmpty(esq.Alias) ? esq.Dato : esq.Alias);
				}
				foreach (DataRow dr in tabla.Rows)
				{
					DataRow r = dtPDF.NewRow();
					int i = 0;
					foreach (Esquema esq in esquemas)
					{
						object o = GetValue(dr[esq.Dato], esq);
						if (o is DateTime)
							r[i++] = ((DateTime)o).ToShortDateString();
						else
							r[i++] = o;
					}
					dtPDF.Rows.Add(r);
				}
				PdfDocument doc = new PdfDocument(PdfDocumentFormat.A4);
				PdfTable myPdfTable = doc.NewTable(new Font("Verdana", 8), tabla.Rows.Count, esquemas.Count, 2);
				myPdfTable.ImportDataTable(dtPDF);
				myPdfTable.HeadersRow.SetColors(Color.White, Color.Navy);
				myPdfTable.SetColors(Color.Black, Color.White, Color.Gainsboro);
				myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
				//
				myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
				for (int i = 0; i < esquemas.Count; i++)
				{
					Esquema esq = esquemas[i];
					//myPdfTable.Columns[2].SetContentFormat("{0:dd/MM/yyyy}");
					switch (esq.Tipo)
					{
						case Esquema.enControles.DateTimePicker:
							myPdfTable.Columns[i].SetContentFormat("{0:dd/MM/yyyy}");
							break;
						case Esquema.enControles.Money:
							myPdfTable.Columns[i].SetContentFormat("{0:" + esq.Mascara + "}");
							myPdfTable.Columns[i].SetContentAlignment(ContentAlignment.MiddleRight);
							break;
						case Esquema.enControles.CheckBox:
							myPdfTable.Columns[i].SetContentAlignment(ContentAlignment.MiddleCenter);
							break;
					}
				}
				int[] sizes = ABMTool.ObtenerAnchos();
				int total = sizes.Sum();
				sizes = sizes.Select(i => (int)decimal.Round(decimal.Divide(i * 100, total))).ToArray();
				myPdfTable.SetColumnsWidth(sizes);
				//
				while (!myPdfTable.AllTablePagesCreated)
				{
					// we create a new page to put the generation of the new TablePage:

					PdfPage newPdfPage = doc.NewPage();
					PdfTablePage newPdfTablePage =
					  myPdfTable.CreateTablePage(new PdfArea(doc, 44, 120, 508, 670));

					// we also put a Label 

					PdfTextArea pta = new PdfTextArea(new Font("Verdana", 16, FontStyle.Bold),
					  Color.Navy, new PdfArea(doc, 0, 20, 595, 100),
					  ContentAlignment.MiddleCenter, String.IsNullOrWhiteSpace(Titulo) ? ABMTool.TablaMain : Titulo);

					// nice thing: we can put all the objects
					// in the following lines, so we can have
					// a great control of layer sequence... 

					newPdfPage.Add(newPdfTablePage);
					newPdfPage.Add(pta);

					// Now we create a loop for serching for people born in 1968. If we find
					// one, we will draw a circle over
					// the birthday cell. This is possible using the
					// the CellArea, that is the Area occupied by a rasterized Cell.

					//for (int index = newPdfTablePage.FirstRow; index <= newPdfTablePage.LastRow; index++)
					//    if (((DateTime)myPdfTable.Rows[index][2].Content).Year == 1968)
					//    {
					//        PdfCircle pc = newPdfTablePage.CellArea(index, 2).InnerCircle(Color.Blue, 2);
					//        pc.StrokeWidth = 3.5;
					//        newPdfPage.Add(pc);
					//    }

					// we save each generated page before start rendering the next.
					newPdfPage.SaveToDocument();
				}
				if (archivoGuardar == "")
				{
					SaveFileDialog save = new SaveFileDialog();
					save.AddExtension = true;
					save.Filter = "Archivos PDF (*.pdf)|*.pdf";
					save.FilterIndex = 1;
					save.FileName = String.IsNullOrWhiteSpace(Titulo) ? ABMTool.TablaMain : Titulo + ".pdf";
					if (save.ShowDialog() != DialogResult.OK)
						return;
					archivoGuardar = save.FileName;
				}
				doc.SaveToFile(archivoGuardar);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al crear reporte pdf", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			try
			{
				if (archivoGuardar != "" && iniciar)
					Process.Start(archivoGuardar);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al crear abrir el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void ToPDF_CC(bool iniciar = true, string archivoGuardar = "")
		{
			ClearFks();
			try
			{
				DataTable tabla = ABMTool.ObtenerVista().ToTable();
				DataTable dtPDF = new DataTable(tabla.TableName);
				foreach (Esquema esq in esquemas)
				{
					dtPDF.Columns.Add(string.IsNullOrEmpty(esq.Alias) ? esq.Dato : esq.Alias);
				}
				dtPDF.Columns.Add("Datos Autorizado");
				foreach (DataRow dr in tabla.Rows)
				{
					DataRow r = dtPDF.NewRow();
					int i = 0;
					foreach (Esquema esq in esquemas)
					{
						object o = GetValue(dr[esq.Dato], esq);
						if (o is DateTime)
							r[i++] = ((DateTime)o).ToShortDateString();
						else if (esq.Dato.ToLower() == "factura")
							r[i++] = o.ToString().Replace('-', ' ');
						//else if (esq.Dato.ToLower() == "monto" || esq.Dato.ToLower() == "saldo")
						//    r[i++] = "$ " + o.ToString();
						else
							r[i++] = o;
					}
					r["Datos Autorizado"] = r["Autorizado"] + " " + r["DNI"] + " " + r["Patente"];
					dtPDF.Rows.Add(r);
				}
				dtPDF.Columns.RemoveAt(dtPDF.Columns.Count - 3);
				dtPDF.Columns.RemoveAt(dtPDF.Columns.Count - 3);
				dtPDF.Columns.RemoveAt(dtPDF.Columns.Count - 3);
				dtPDF.Columns.RemoveAt(dtPDF.Columns.Count - 2);//forma de pago
				dtPDF.Columns.RemoveAt(1);//cliente
				dtPDF.Columns.RemoveAt(1);//vendedor
				PdfDocument doc = new PdfDocument(PdfDocumentFormat.A4);
				PdfTable myPdfTable = doc.NewTable(new Font("Verdana", 8), tabla.Rows.Count, dtPDF.Columns.Count, 2);
				myPdfTable.ImportDataTable(dtPDF);
				myPdfTable.HeadersRow.SetColors(Color.White, Color.Navy);
				myPdfTable.SetColors(Color.Black, Color.White, Color.Gainsboro);
				myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
				//
				myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
				//myPdfTable.Columns[i].SetContentFormat("{0:" + esq.Mascara + "}");
				myPdfTable.Columns[myPdfTable.Columns.Count() - 2].SetContentAlignment(ContentAlignment.MiddleRight);
				myPdfTable.Columns[myPdfTable.Columns.Count() - 3].SetContentAlignment(ContentAlignment.MiddleRight);
				//for (int i = 0; i < esquemas.Count; i++)
				//{
				//    Esquema esq = esquemas[i];
				//    //myPdfTable.Columns[2].SetContentFormat("{0:dd/MM/yyyy}");
				//    switch (esq.Tipo)
				//    {
				//        case Esquema.enControles.DateTimePicker:
				//            myPdfTable.Columns[i].SetContentFormat("{0:dd/MM/yyyy}");
				//            break;
				//        case Esquema.enControles.Money:
				//            myPdfTable.Columns[i].SetContentFormat("{0:" + esq.Mascara + "}");
				//            myPdfTable.Columns[i].SetContentAlignment(ContentAlignment.MiddleRight);
				//            break;
				//        case Esquema.enControles.CheckBox:
				//            myPdfTable.Columns[i].SetContentAlignment(ContentAlignment.MiddleCenter);
				//            break;
				//    }
				//}
				List<int> sizesPre = new List<int>(ABMTool.ObtenerAnchos());
				sizesPre.Add(sizesPre[sizesPre.Count - 2] + sizesPre[sizesPre.Count - 3] + sizesPre[sizesPre.Count - 4]);
				sizesPre.RemoveAt(sizesPre.Count - 3);
				sizesPre.RemoveAt(sizesPre.Count - 3);
				sizesPre.RemoveAt(sizesPre.Count - 3);
				sizesPre.RemoveAt(sizesPre.Count - 2);//forma de pago
				sizesPre.RemoveAt(1);//cliente
				sizesPre.RemoveAt(1);//vendedor
				int[] sizes = sizesPre.ToArray();
				int total = sizes.Sum();
				sizes = sizes.Select(i => (int)decimal.Round(decimal.Divide(i * 100, total))).ToArray();
				myPdfTable.SetColumnsWidth(sizes);
				//
				while (!myPdfTable.AllTablePagesCreated)
				{
					// we create a new page to put the generation of the new TablePage:

					PdfPage newPdfPage = doc.NewPage();
					PdfTablePage newPdfTablePage =
					  myPdfTable.CreateTablePage(new PdfArea(doc, 44, 120, 508, 670));

					// we also put a Label 

					PdfTextArea pta = new PdfTextArea(new Font("Verdana", 16, FontStyle.Bold),
					  Color.Navy, new PdfArea(doc, 0, 20, 595, 100),
					  ContentAlignment.MiddleCenter, String.IsNullOrWhiteSpace(Titulo) ? ABMTool.TablaMain : Titulo);

					// nice thing: we can put all the objects
					// in the following lines, so we can have
					// a great control of layer sequence... 

					newPdfPage.Add(newPdfTablePage);
					newPdfPage.Add(pta);

					// Now we create a loop for serching for people born in 1968. If we find
					// one, we will draw a circle over
					// the birthday cell. This is possible using the
					// the CellArea, that is the Area occupied by a rasterized Cell.

					//for (int index = newPdfTablePage.FirstRow; index <= newPdfTablePage.LastRow; index++)
					//    if (((DateTime)myPdfTable.Rows[index][2].Content).Year == 1968)
					//    {
					//        PdfCircle pc = newPdfTablePage.CellArea(index, 2).InnerCircle(Color.Blue, 2);
					//        pc.StrokeWidth = 3.5;
					//        newPdfPage.Add(pc);
					//    }

					// we save each generated page before start rendering the next.
					newPdfPage.SaveToDocument();
				}
				if (archivoGuardar == "")
				{
					SaveFileDialog save = new SaveFileDialog();
					save.AddExtension = true;
					save.Filter = "Archivos PDF (*.pdf)|*.pdf";
					save.FilterIndex = 1;
					save.FileName = String.IsNullOrWhiteSpace(Titulo) ? ABMTool.TablaMain : Titulo + ".pdf";
					if (save.ShowDialog() != DialogResult.OK)
						return;
					archivoGuardar = save.FileName;
				}
				doc.SaveToFile(archivoGuardar);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al crear reporte pdf", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			try
			{
				if (archivoGuardar != "" && iniciar)
					Process.Start(archivoGuardar);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al crear abrir el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
