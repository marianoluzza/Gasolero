using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FormCom;
using Negocio;

namespace Gasolero
{
	public partial class ZCierresFiltro : UserControl, IABMAddOn
	{
		public enum enFiltro { ZCierres, Turnos, LibroIVA, InformesZ, Clientes, Operaciones };

		IABMTool _abm;
		DateTime filtroDde, filtroHta;
		bool necesitaReajuste = false;
		enFiltro filtrado;

		public ZCierresFiltro(enFiltro filtro)
		{
			InitializeComponent();
			int chbTopFondo = chbAbrirAlCrear.Top;
			filtrado = filtro;
			filtroDde = DateTime.Now.Date;
			filtroHta = DateTime.Now.Date;
			chbAbrirAlCrear.Checked = AppConfig.AbrirReportes;
			chbAbrirAlCrear.Top = bReportar.Top + bReportar.Height + 4;
			if (filtrado == enFiltro.Turnos)
			{
				bReportar.Text = "Reporte de Ventas";
				bReporte2.Visible = true;
				bReporte2.Text = "Reporte de Vales";
				bReporte3.Visible = true;
				bReporte3.Text = "Reporte de CC";
				bReporte4.Visible = true;
				bReporte4.Text = "Cierres Acumulados";
				gbFiltro.Text = "Filtro de Turnos";
				radFechaDde.Text = "Elija un turno";
				radFechaHta.Text = "Elija un turno";
				chbAbrirAlCrear.Top = chbTopFondo;
			}
			else if (filtrado == enFiltro.Operaciones)
			{
				chbAbrirAlCrear.Visible = false;
				gbPeriodo.Visible = false;
			}	
		}

		#region IABMAddOn Members

		public IABMTool ABMTool
		{
			get { return _abm; }
		}

		public Control Control
		{
			get { return this; }
		}

		public string Nombre
		{
			get { return "Filtro por Fechas"; }
		}

		public void OnFinit(AddOnQuitInfo info)
		{

		}

		public void OnInit(IABMTool abm)
		{
			_abm = abm;
			ToolStripItem lbl = new ToolStripMenuItem("Refrescar");
			lbl.Size = lbl.GetPreferredSize(new Size(120, 22));
			lbl.Click += new EventHandler(refrescarToolStripMenuItem_Click);
			abm.AgregarMenu(lbl, enMenuPosicion.Contextual);
			//
			lbl = new ToolStripLabel("Haga doble click en un item para elegirlo");
			lbl.Size = lbl.GetPreferredSize(new Size(120, 22));
			abm.AgregarMenu(lbl, enMenuPosicion.BarraEstado);
			//
			abm.ItemDobleClicked += new ItemClickedEventHandler(ABM_ItemDobleClicked);
			abm.MultiSeleccion = false;
			necesitaReajuste = abm.ObtenerTabla(abm.TablaMain).Rows.Count == 0;

		}

		void ABM_ItemDobleClicked(IABMTool abm, ItClickEventArgs e)
		{
			DateTime fecha = DateTime.Now;
			object item = null;
			try
			{
				switch (filtrado)
				{
					case enFiltro.Turnos:
						Turno t = new Turno(e.Item.Row);
						item = t;
						fecha = t.FechaFinal;
						break;
					case enFiltro.Operaciones:
						fecha = DateTime.Parse(e.Item.Row["Fecha"].ToString());
						break;
					default:
						ZCierre zc = new ZCierre(e.Item.Row);
						fecha = zc.Fecha;
						break;
				}
				if (radFechaDde.Checked)
				{
					radFechaDde.Text = fecha.ToString();
					radFechaDde.Tag = item;
					radFechaHta.Checked = true;
				}
				else
				{
					radFechaHta.Text = fecha.ToString();
					radFechaHta.Tag = item;
					radFechaDde.Checked = true;
				}
			}
			catch { }
		}

		public ABMAddOnPosicion Posicion
		{
			get { return ABMAddOnPosicion.PanelDerecho; }
		}

		public override string ToString()
		{
			return Nombre;
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			return new Size(194, 486);
		}
		#endregion

		public string NombreReporte
		{
			get { return tbNombre.Text; }
			set { tbNombre.Text = value; }
		}

		public void Filtrar()
		{
			if (_abm == null)
				return;
			switch (filtrado)
			{
				case enFiltro.Turnos:
					_abm.ActualizarTabla(Turno.GetForFechas(filtroDde, filtroHta.AddDays(1)));
					break;
				case enFiltro.Operaciones:
					//_abm.ActualizarTabla(Operacion.GetVales(filtroDde, filtroHta.AddDays(1)));
					_abm.ActualizarTabla(Operacion.GetForFechas(filtroDde, filtroHta.AddDays(1)));
					tbNombre.Text = "Vales " + filtroDde.ToShortDateString().Replace('/', '-') + " a " + filtroHta.ToShortDateString().Replace('/', '-');
					break;
				default:
					_abm.ActualizarTabla(ZCierre.GetListaXFechas(filtroDde, filtroHta.AddDays(1)));
					break;
			}
			if (necesitaReajuste)
			{
				_abm.ReajustarGrilla();
				_abm.Control.Refresh(); 
				necesitaReajuste = _abm.ObtenerTabla(_abm.TablaMain).Rows.Count == 0;
			}
		}

		private void refrescarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Filtrar();
		}

		private void dtpDde_ValueChanged(object sender, EventArgs e)
		{
			DateTime fecha = dtpDde.Value.Date;
			if (fecha == filtroDde)
				return;
			filtroDde = fecha;
			if (filtroDde > filtroHta)
			{
				dtpHta.Value = filtroDde;
			}
			else
			{
				Filtrar();
			}
		}

		private void dtpHta_ValueChanged(object sender, EventArgs e)
		{
			DateTime fecha = dtpHta.Value.Date;
			if (fecha == filtroHta)
				return;
			filtroHta = fecha;
			if (filtroHta < filtroDde)
			{
				dtpDde.Value = filtroHta;
			}
			else
			{
				Filtrar();
			}
		}

		private void bReportar_Click(object sender, EventArgs e)
		{
			if (_abm == null)
				return;
			DateTime periodoDde = DateTime.Now, periodoHta = DateTime.Now;
			if (filtrado != enFiltro.Operaciones)
			{
				if (!DateTime.TryParse(radFechaDde.Text, out periodoDde))
				{
					MessageBox.Show("Debe elegir una fecha inicial", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (!DateTime.TryParse(radFechaHta.Text, out periodoHta))
				{
					MessageBox.Show("Debe elegir una fecha final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (periodoDde > periodoHta)
				{
					if (MessageBox.Show("La fecha de inicio es posterior a la final" + Environment.NewLine +
						"Si continua, las fechas se intercambiarán", "Periodo Inválido", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
						return;
					DateTime aux = periodoHta;
					periodoHta = periodoDde;
					periodoDde = aux;
					radFechaDde.Text = periodoDde.ToString();
					radFechaHta.Text = periodoHta.ToString();
				}
				else if (periodoDde == periodoHta)
				{
					MessageBox.Show("El inicio del periodo coincide con el final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			Reporte rpt = null;
			string error = "";
			try
			{
				//if (MessageBox.Show("¿Desea hacer un reporte desde " + periodoDde + " hasta " + periodoHta + "?", "Confirmar Reporte", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				//    return;
				Cursor = Cursors.WaitCursor;
				switch (filtrado)
				{
					case enFiltro.LibroIVA:
						rpt = new Reporte();
						rpt.HacerLibroIVA(periodoDde, periodoHta, tbNombre.Text.Trim());
						break;
					case enFiltro.ZCierres:
						rpt = new Reporte();
						if (tbNombre.Text.Trim() == "")
							rpt.HacerReporteVentas(periodoDde, periodoHta);
						else
							rpt.HacerReporteVentas(periodoDde, periodoHta, tbNombre.Text.Trim());
						break;
					case enFiltro.InformesZ:
						rpt = new Reporte();
						error = rpt.HacerReporteInformesZ(periodoDde, periodoHta, tbNombre.Text.Trim());
						break;
					case enFiltro.Turnos:
						rpt = new Reporte();
						rpt.HacerReporteTurnos(periodoDde, periodoHta);
						break;
					case enFiltro.Operaciones:
						_abm.ObtenerAddOn<Exportador>().ToExcel(tbNombre.Text);//el reporte es la planilla que se ve a excel
						//rpt = new Reporte();
						//rpt.HacerReporteOperaciones(periodoDde, periodoHta);
						//en lugar de fechas, pasar el listado
						//DataTable dtOperaciones = _abm..ObtenerTabla(_abm.TablaMain)
						break;
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error al crear el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
			if (rpt == null)
				return;
			SaveFileDialog save = new SaveFileDialog();
			save.AddExtension = true;
			save.DefaultExt = "xls";
			save.FileName = rpt.Nombre;
			save.Filter = "Libros Excel (*.xls)|*.xls|Todos los archivos (*.*)|*.*";
			save.FilterIndex = 1;
			save.InitialDirectory = AppConfig.CarpetaReportes;
			save.Title = "Guardar Reporte";
			if (save.ShowDialog() != DialogResult.OK)
				return;
			rpt.Nombre = save.FileName;
			try
			{
				rpt.Grabar(save.FileName);
				if (AppConfig.AbrirReportes)
					System.Diagnostics.Process.Start(save.FileName);
				else
					MessageBox.Show("Reporte creado exitosamente", "Reporte creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
				if (error != "")
				{
					MessageBox.Show(error, "Advertencia de Reporte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				error = "";
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error al guardar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void bReporte2_Click(object sender, EventArgs e)
		{
			if (_abm == null)
				return;
			DateTime periodoDde, periodoHta;
			if (!DateTime.TryParse(radFechaDde.Text, out periodoDde))
			{
				MessageBox.Show("Debe elegir una fecha inicial", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (!DateTime.TryParse(radFechaHta.Text, out periodoHta))
			{
				MessageBox.Show("Debe elegir una fecha final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (periodoDde > periodoHta)
			{
				if (MessageBox.Show("La fecha de inicio es posterior a la final" + Environment.NewLine +
					"Si continua, las fechas se intercambiarán", "Periodo Inválido", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
					return;
				DateTime aux = periodoHta;
				periodoHta = periodoDde;
				periodoDde = aux;
				radFechaDde.Text = periodoDde.ToString();
				radFechaHta.Text = periodoHta.ToString();
			}
			else if (periodoDde == periodoHta)
			{
				MessageBox.Show("El inicio del periodo coincide con el final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Reporte rpt = null;
			try
			{
				Cursor = Cursors.WaitCursor;
				switch (filtrado)
				{
					case enFiltro.LibroIVA:
						break;
					case enFiltro.ZCierres:
						break;
					case enFiltro.InformesZ:
						break;
					case enFiltro.Turnos:
						rpt = new Reporte();
						rpt.HacerReporteVales(periodoDde, periodoHta);
						break;
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error al crear el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
			if (rpt == null)
				return;
			SaveFileDialog save = new SaveFileDialog();
			save.AddExtension = true;
			save.DefaultExt = "xls";
			save.FileName = rpt.Nombre;
			save.Filter = "Libros Excel (*.xls)|*.xls|Todos los archivos (*.*)|*.*";
			save.FilterIndex = 1;
			save.InitialDirectory = AppConfig.CarpetaReportes;
			save.Title = "Guardar Reporte";
			if (save.ShowDialog() != DialogResult.OK)
				return;
			rpt.Nombre = save.FileName;
			try
			{
				rpt.Grabar(save.FileName);
				if (AppConfig.AbrirReportes)
					System.Diagnostics.Process.Start(save.FileName);
				else
					MessageBox.Show("Reporte creado exitosamente", "Reporte creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error al guardar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void bReporte3_Click(object sender, EventArgs e)
		{
			if (_abm == null)
				return;
			DateTime periodoDde, periodoHta;
			if (!DateTime.TryParse(radFechaDde.Text, out periodoDde))
			{
				MessageBox.Show("Debe elegir una fecha inicial", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (!DateTime.TryParse(radFechaHta.Text, out periodoHta))
			{
				MessageBox.Show("Debe elegir una fecha final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (periodoDde > periodoHta)
			{
				if (MessageBox.Show("La fecha de inicio es posterior a la final" + Environment.NewLine +
					"Si continua, las fechas se intercambiarán", "Periodo Inválido", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
					return;
				DateTime aux = periodoHta;
				periodoHta = periodoDde;
				periodoDde = aux;
				radFechaDde.Text = periodoDde.ToString();
				radFechaHta.Text = periodoHta.ToString();
			}
			else if (periodoDde == periodoHta)
			{
				MessageBox.Show("El inicio del periodo coincide con el final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Reporte rpt = null;
			try
			{
				Cursor = Cursors.WaitCursor;
				switch (filtrado)
				{
					case enFiltro.LibroIVA:
						break;
					case enFiltro.ZCierres:
						break;
					case enFiltro.InformesZ:
						break;
					case enFiltro.Turnos:
						rpt = new Reporte();
						rpt.HacerReporteCC(periodoDde, periodoHta);
						break;
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error al crear el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
			if (rpt == null)
				return;
			SaveFileDialog save = new SaveFileDialog();
			save.AddExtension = true;
			save.DefaultExt = "xls";
			save.FileName = rpt.Nombre;
			save.Filter = "Libros Excel (*.xls)|*.xls|Todos los archivos (*.*)|*.*";
			save.FilterIndex = 1;
			save.InitialDirectory = AppConfig.CarpetaReportes;
			save.Title = "Guardar Reporte";
			if (save.ShowDialog() != DialogResult.OK)
				return;
			rpt.Nombre = save.FileName;
			try
			{
				rpt.Grabar(save.FileName);
				if (AppConfig.AbrirReportes)
					System.Diagnostics.Process.Start(save.FileName);
				else
					MessageBox.Show("Reporte creado exitosamente", "Reporte creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error al guardar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void bReporte4_Click(object sender, EventArgs e)
		{
			if (_abm == null)
				return;
			DateTime periodoDde, periodoHta;
			if (!DateTime.TryParse(radFechaDde.Text, out periodoDde))
			{
				MessageBox.Show("Debe elegir una fecha inicial", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (!DateTime.TryParse(radFechaHta.Text, out periodoHta))
			{
				MessageBox.Show("Debe elegir una fecha final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (periodoDde > periodoHta)
			{
				if (MessageBox.Show("La fecha de inicio es posterior a la final" + Environment.NewLine +
					"Si continua, las fechas se intercambiarán", "Periodo Inválido", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
					return;
				DateTime aux = periodoHta;
				periodoHta = periodoDde;
				periodoDde = aux;
				radFechaDde.Text = periodoDde.ToString();
				radFechaHta.Text = periodoHta.ToString();
			}
			else if (periodoDde == periodoHta)
			{
				MessageBox.Show("El inicio del periodo coincide con el final", "Periodo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				Cursor = Cursors.WaitCursor;
				RptCierre rptCierre = new RptCierre(periodoDde, periodoHta);
				rptCierre.Imprimir();
				Turno tDesde = radFechaDde.Tag as Turno;
				Turno tAnt = Turno.GetForNumero(tDesde.Numero - 1);
				RptCCMov rptCC = new RptCCMov(tAnt.FechaFinal, periodoHta);
				rptCC.Imprimir();
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error al crear el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void chbAbrirAlCrear_CheckedChanged(object sender, EventArgs e)
		{
			AppConfig.AbrirReportes = chbAbrirAlCrear.Checked;
		}

		private void hoyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			radFechaHta.Text = Reglas.Now.ToString();
		}
	}
}
