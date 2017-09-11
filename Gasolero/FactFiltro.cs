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
	public partial class FactFiltro : UserControl, IABMAddOn
	{
		IABMTool _abm;
		uint _idCliente = 0;
		CheckState _entregadas = CheckState.Indeterminate;
		ABMForm _busqClientes = null;
		enFiltrado _tablaFiltrada = enFiltrado.Facturas;
		MarUtils.Controles.MoneyTextBox moneyCCPago = new MarUtils.Controles.MoneyTextBox();
		TextBox txtCCPagoDesc = new TextBox();
		CheckBox chbTipoMonto = new CheckBox();
		uint _idPagoForma = 1;//arranca en efectivo

		public FactFiltro()
			: this(enFiltrado.Facturas)
		{ }

		public FactFiltro(enFiltrado tabla)
		{
			InitializeComponent();
			_tablaFiltrada = tabla;
			Point locationAux = gbDetalle.Location;
			gbDetalle.Location = bInforme.Location;
			bInforme.Visible = false;
			switch (tabla)
			{
				case enFiltrado.Turnos:
					bFiltrar.Top -= gbCliente.Size.Height;
					gbCliente.Visible = false;
					gbDetalle.Visible = false;
					break;
				case enFiltrado.EntregaDeCaja:
					gbCliente.Text = "Entregada";
					radClientesTodos.Text = "-";
					radClienteBuscar.Text = "SI";
					radClienteBuscar.Click -= new EventHandler(radClienteBuscar_Click);
					radClienteBuscar.CheckedChanged += new EventHandler(radClienteBuscar_CheckedChanged);
					radExtra.Visible = true;
					txtClienteNombre.Visible = false;
					lblDetalle.Text = "Elija turno";
					break;
				case enFiltrado.Facturas:
					gbDetalle.Location = locationAux;
					bInforme.Visible = true;
					#region CheckBox para montos
					chbTipoMonto.ThreeState = true;
					chbTipoMonto.CheckState = CheckState.Indeterminate;
					chbTipoMonto.Text = "Todos los montos";
					chbTipoMonto.CheckStateChanged += delegate(object s2, EventArgs e2)
					{
						_entregadas = chbTipoMonto.CheckState;
						switch (chbTipoMonto.CheckState)
						{
							case CheckState.Checked:
								chbTipoMonto.Text = "Montos positivos";
								break;
							case CheckState.Indeterminate:
								chbTipoMonto.Text = "Todos los montos";
								break;
							case CheckState.Unchecked:
								chbTipoMonto.Text = "Montos negativos";
								break;
							default:
								break;
						}
					};
					gbFecha.Controls.Add(chbTipoMonto);
					chbTipoMonto.Visible = true;
					gbFecha.Height += 18;
					chbTipoMonto.Width = dtpDesde.Width;
					chbTipoMonto.Location = new Point(dtpDesde.Location.X, dtpHasta.Location.Y + 18);
					bFiltrar.Top += 24;
					bInforme.Top += 24;
					gbCliente.Top += 24;
					gbDetalle.Top += 24;
					#endregion
					break;
				case enFiltrado.CCMovimientos:
					gbDetalle.Text = "Generar Pago";
					lblDetalle.Text = "Monto";
					moneyCCPago.Location = new Point(lblDetalle.Location.X, lblDetalle.Location.Y + 18);
					moneyCCPago.Width = gbDetalle.Width - lblDetalle.Left * 2;
					moneyCCPago.ConSigno = false;
					lblDetalle.Parent.Controls.Add(moneyCCPago);
					//
					txtCCPagoDesc.Text = "Pago a cuenta";
					txtCCPagoDesc.Tag = txtCCPagoDesc.Text;
					txtCCPagoDesc.Location = new Point(moneyCCPago.Location.X, moneyCCPago.Location.Y + 20);
					txtCCPagoDesc.Width = gbDetalle.Width - lblDetalle.Left * 2;
					lblDetalle.Parent.Controls.Add(txtCCPagoDesc);
					//
					ComboBox cbFP = new ComboBox();
					cbFP.Location = new Point(lblDetalle.Location.X, txtCCPagoDesc.Location.Y + txtCCPagoDesc.Height + 4);
					cbFP.DataSource = PagoForma.GetHabilitados();
					cbFP.DisplayMember = "Descripcion";
					cbFP.ValueMember = "IdPagoForma";
					//cbFP.SelectedIndex = 0;
					cbFP.SelectedValueChanged += new EventHandler(delegate { _idPagoForma = (uint)cbFP.SelectedValue; });
					cbFP.Width = txtCCPagoDesc.Width;
					lblDetalle.Parent.Controls.Add(cbFP);
					//
					Button bGenerarPagoCC = new Button();
					bGenerarPagoCC.Text = "Pagar";
					bGenerarPagoCC.Width = txtCCPagoDesc.Width;
					bGenerarPagoCC.Location = new Point(lblDetalle.Location.X, cbFP.Location.Y + cbFP.Height + 4);
					lblDetalle.Parent.Controls.Add(bGenerarPagoCC);
					bGenerarPagoCC.Click += new EventHandler(bGenerarPagoCC_Click);
					//
					Button bGenerarInforme = new Button();
					bGenerarInforme.Text = "Enviar Informe";
					bGenerarInforme.Width = txtCCPagoDesc.Width;
					bGenerarInforme.Location = new Point(lblDetalle.Location.X, bGenerarPagoCC.Location.Y + bGenerarPagoCC.Height + 2);
					lblDetalle.Parent.Controls.Add(bGenerarInforme);
					bGenerarInforme.Click += new EventHandler(bGenerarInforme_Click);
					//
					#region CheckBox para montos
					chbTipoMonto.ThreeState = true;
					chbTipoMonto.CheckState = CheckState.Indeterminate;
					chbTipoMonto.Text = "Todos los montos";
					chbTipoMonto.CheckStateChanged += delegate(object s2, EventArgs e2)
					{
						_entregadas = chbTipoMonto.CheckState;
						switch (chbTipoMonto.CheckState)
						{
							case CheckState.Checked:
								chbTipoMonto.Text = "Montos positivos";
								break;
							case CheckState.Indeterminate:
								chbTipoMonto.Text = "Todos los montos";
								break;
							case CheckState.Unchecked:
								chbTipoMonto.Text = "Montos negativos";
								break;
							default:
								break;
						}
					};
					gbFecha.Controls.Add(chbTipoMonto);
					chbTipoMonto.Visible = true;
					gbFecha.Height += 18;
					chbTipoMonto.Width = dtpDesde.Width;
					chbTipoMonto.Location = new Point(dtpDesde.Location.X, dtpHasta.Location.Y + 18);
					bFiltrar.Top += 24;
					gbCliente.Top += 24;
					gbDetalle.Top += 24;
					#endregion
					break;
			}
		}

		void radClienteBuscar_CheckedChanged(object sender, EventArgs e)
		{//SOLO PARA EntregaDeCaja.
			if (!radClienteBuscar.Checked)
				return;
			_entregadas = CheckState.Checked;
		}

		private void radClienteBuscar_Click(object sender, EventArgs e)
		{//SOLO PARA FACT
			BuscarCliente();
		}

		private void radClientesTodos_CheckedChanged(object sender, EventArgs e)
		{
			if (!radClientesTodos.Checked)
				return;
			switch (_tablaFiltrada)
			{
				case enFiltrado.Facturas:
				case enFiltrado.CCMovimientos:
					_idCliente = 0;
					txtClienteNombre.Clear();
					break;
				case enFiltrado.EntregaDeCaja:
					_entregadas = CheckState.Indeterminate;
					break;
			}
		}

		private void radExtra_CheckedChanged(object sender, EventArgs e)
		{
			if (!radExtra.Checked)
				return;
			_entregadas = CheckState.Unchecked;
		}

		private void bFiltrar_Click(object sender, EventArgs e)
		{
			DateTime dde = new DateTime(dtpDesde.Value.Year, dtpDesde.Value.Month, dtpDesde.Value.Day, 0, 0, 0);
			DateTime hta = new DateTime(dtpHasta.Value.Year, dtpHasta.Value.Month, dtpHasta.Value.Day, 23, 59, 59);
			Filtrar(dde, hta, _entregadas);
		}

		private void bInforme_Click(object sender, EventArgs e)
		{
			DateTime dde = new DateTime(dtpDesde.Value.Year, dtpDesde.Value.Month, dtpDesde.Value.Day, 0, 0, 0);
			DateTime hta = new DateTime(dtpHasta.Value.Year, dtpHasta.Value.Month, dtpHasta.Value.Day, 23, 59, 59);
			Informe(dde, hta, _idCliente);
		}

		void bGenerarPagoCC_Click(object sender, EventArgs e)
		{
			if (_idCliente == 0)
			{
				MessageBox.Show("Debe elegir un cliente", "Falta Cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (moneyCCPago.Valor <= 0)
			{
				MessageBox.Show("Debe establecer un monto mayor a cero", "Monto Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			CCMovimiento cc = new CCMovimiento();
			cc.Fecha = Reglas.Now;
			cc.Descripcion = txtCCPagoDesc.Text;
			cc.IdCliente = _idCliente;
			cc.IdFactEncabezado = 0;
			cc.IdVendedor = Reglas.VendedorActual.IdVendedor;
			cc.Monto = moneyCCPago.Valor;//es un pago del cliente por ello es positivo
			cc.IdPagoForma = _idPagoForma;
			Cliente cli = Cliente.GetSingleE(_idCliente);
			var abmRes = cc.Alta();
			if (abmRes.CodigoError != MarUtils.Soporte.enErrores.Ninguno)
			{
				MessageBox.Show(abmRes.MensajeError, abmRes.CodigoError.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				if (cc.Monto > 0)//positivo es saldo para el cliente => recibo
					Fiscal.ImprimirRecibo(Cliente.GetSingleE(cc.IdCliente), cc.Descripcion, (double)cc.Monto);
				Emailer.Informar(cc);
				MessageBox.Show("Pago acreditado", "Pago acreditado", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtCCPagoDesc.Text = txtCCPagoDesc.Tag.ToString();
				moneyCCPago.Valor = 0;
				bFiltrar_Click(sender, e);
			}
		}

		void bGenerarInforme_Click(object sender, EventArgs e)
		{
			if (_idCliente == 0)
			{
				MessageBox.Show("Debe elegir un cliente", "Falta Cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//genera informe
			Cliente c = Cliente.GetSingleE(_idCliente);
			if (c.Email == "")
			{
				MessageBox.Show("El cliente no tiene email declarado", "Cliente Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				try
				{
					bFiltrar_Click(sender, e);
					Exportador ex = _abm.ObtenerAddOn<Exportador>();
					string asunto = "Resumen de Movimientos de Cuenta Corriente " + (dtpDesde.Value.Date == dtpHasta.Value.Date ? "de " + dtpDesde.Value.ToString("dd-MM-yyyy") : "entre " + dtpDesde.Value.ToString("dd-MM-yyyy") + " y " + dtpHasta.Value.ToString("dd-MM-yyyy"));
					string file = System.IO.Path.GetTempPath();
					file = System.IO.Path.Combine(file, "(" + c.Nombre + ") " + asunto + ".pdf");
					ex.Titulo = "Movimientos de Cuenta Corriente de " + c.Nombre;
					ex.ToPDF_CC(false, file);
					Emailer.Informar(c, file, asunto);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					MessageBox.Show(ex.Message, "Error al enviar informe", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void dtpDesde_ValueChanged(object sender, EventArgs e)
		{
			if (dtpDesde.Value > dtpHasta.Value)
				dtpHasta.Value = dtpDesde.Value;
		}

		private void dtpHasta_ValueChanged(object sender, EventArgs e)
		{
			if (dtpHasta.Value < dtpDesde.Value)
				dtpDesde.Value = dtpHasta.Value;
		}

		public void BuscarCliente()
		{
			QFiltro filtroQ;
			if (_busqClientes == null)
			{
				_busqClientes = new ABMForm(true, false);//TH Ajustado
				filtroQ = new QFiltro("Nombre");
				_busqClientes.AddAddOn(filtroQ);
				filtroQ.SeleccionAceptada += new EventHandler(Filtro_SeleccionAceptada);
				filtroQ.SeleccionCancelada += new EventHandler(Filtro_SeleccionCancelada);
				ABMFiltro filtro = _busqClientes.ObtenerAddOn<ABMFiltro>();
				_busqClientes.ActualizarTabla(Cliente.Esquema());
				_busqClientes.TablaEsquema = "Esquemas";
				_busqClientes.Load += delegate(object sender, EventArgs e)
				{
					new System.Threading.Thread(delegate()
					{
						System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
						_busqClientes.Invoke((MethodInvoker)delegate
						{
							_busqClientes.Icon = System.Drawing.Icon.FromHandle(Recursos.Cliente.GetHicon());
							_busqClientes.Text = "Buscar cliente";
							_busqClientes.ReajustarGrilla();
							_busqClientes.Refresh();
						});
					}).Start();
				};
				filtro.SeleccionAceptada += new EventHandler(Filtro_SeleccionAceptada);
				filtro.SeleccionCancelada += new EventHandler(Filtro_SeleccionCancelada);
			}
			else
			{
				filtroQ = _busqClientes.ObtenerAddOn<QFiltro>();
			}
			filtroQ.TextoFiltro = "";
			filtroQ.PermitirABM = false;
			_busqClientes.ActualizarTabla(_abm.ObtenerTabla(Cliente.NombreTabla));
			if (_busqClientes.TablaMain != Cliente.NombreTabla)
				_busqClientes.TablaMain = Cliente.NombreTabla;
			_busqClientes.ShowDialog();
			if (!_busqClientes.EstaActivoAddon(filtroQ.GetType()))
				_busqClientes.ActivarAddOn(filtroQ.GetType());
		}

		public void Filtrar(DateTime dde, DateTime hta, CheckState check)
		{
			string filtroABM = "";
			DataTable dtAux = null;
			switch (_tablaFiltrada)
			{
				case enFiltrado.EntregaDeCaja:
					_abm.ActualizarTabla(Turno.GetEntregasForFechas(dde, hta));
					switch (check)
					{
						case CheckState.Indeterminate:
							_abm.Filtro = "";
							break;
						case CheckState.Checked:
							_abm.Filtro = "NOT(Color = 'Red')";
							break;
						case CheckState.Unchecked:
							_abm.Filtro = "Color = 'Red'";
							break;
					}
					break;
				case enFiltrado.Turnos:
					_abm.ActualizarTabla(Turno.GetForFechas(dde, hta));
					break;
				case enFiltrado.Facturas:
					_abm.ActualizarTabla(FactEncabezado.GetListForFechasIdCliente(
						dde, hta, _idCliente));
					switch (check)
					{
						case CheckState.Indeterminate:
							filtroABM = "";
							break;
						case CheckState.Checked:
							filtroABM = "Total >= 0";//facturas
							break;
						case CheckState.Unchecked:
							filtroABM = "Total < 0";//nc
							break;
					}
					_abm.Filtro = filtroABM;
					break;
				case enFiltrado.CCMovimientos:
					dtAux = CCMovimiento.GetMovimientos(dde, hta, _idCliente);
					if (_idCliente == 0 || _idCliente == Cliente.ConsFinal.Id)
					{
						dtAux.Rows.RemoveAt(0);
						dtAux.Columns.RemoveAt(dtAux.Columns.Count - 1);
					}
					_abm.ActualizarTabla(dtAux);
					switch (check)
					{
						case CheckState.Indeterminate:
							filtroABM = "";
							break;
						case CheckState.Checked:
							filtroABM = "Monto >= 0";//pagos
							break;
						case CheckState.Unchecked:
							filtroABM = "Monto < 0";//ventas
							break;
					}
					_abm.Filtro = filtroABM;
					break;
			}
			new System.Threading.Thread(delegate()
			{
				System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
				_abm.Control.Invoke((MethodInvoker)delegate
				{
					_abm.ReajustarGrilla();
					_abm.Control.Refresh();
				});
			}).Start();
		}

		public void Informe(DateTime dde, DateTime hta, uint idCli)
		{
			switch (_tablaFiltrada)
			{
				case enFiltrado.Facturas:
					Reporte r = new Reporte();
					r.HacerReporteVentasCliente(dde, hta, idCli, "Reporte");
					r.Grabar();
					//r.Grabar(@"D:\X\z.xls");
					break;
			}
		}

		#region IABMAddOn Members

		public IABMTool ABMTool
		{
			get { return _abm; }
			internal set { _abm = value; }
		}

		public Control Control
		{
			get { return this; }
		}

		public string Nombre
		{
			get { return "Filtro " + _tablaFiltrada.ToString(); }
		}

		public void OnFinit(AddOnQuitInfo info)
		{
			switch (_tablaFiltrada)
			{
				case enFiltrado.EntregaDeCaja:
				case enFiltrado.Facturas:
					_abm.ItemCorrienteCambiado -= new EventHandler(Abm_ItemCorrienteCambiado);
					break;
			}
		}

		public void OnInit(IABMTool abm)
		{
			_abm = abm;
			switch (_tablaFiltrada)
			{
				case enFiltrado.EntregaDeCaja:
				case enFiltrado.Facturas:
					_abm.ItemCorrienteCambiado += new EventHandler(Abm_ItemCorrienteCambiado);
					break;
			}
			chbTipoMonto.CheckState = CheckState.Indeterminate;
		}

		void Abm_ItemCorrienteCambiado(object sender, EventArgs e)
		{
			lblDetalle.Text = "";
			if (_abm.ItemActual == null)
				return;
			switch (_tablaFiltrada)
			{
				case enFiltrado.EntregaDeCaja:
					DescEntregas();
					break;
				case enFiltrado.Facturas:
					DescFacturas();
					break;
			}
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
			
			return new Size(177, 266);//177
		}
		#endregion

		void DescFacturas()
		{
			SortedList<uint, decimal> valores = new SortedList<uint, decimal>();
			DataTable fSel = _abm.ObtenerSeleccion();
			foreach (DataRow r in fSel.Rows)
			{
				FactEncabezado f = new FactEncabezado(r);
				f.GetFilas();
				foreach (FactItem fi in f.Cuerpo)
				{
					if (!valores.ContainsKey(fi.IdArticulo))
						valores.Add(fi.IdArticulo, 0);
					valores[fi.IdArticulo] += fi.Monto;
				}
			}
			foreach (KeyValuePair<uint, decimal> par in valores)
			{
				Articulo art = Articulo.GetSingleE(par.Key);
				lblDetalle.Text += art.Codigo + ": $ " + par.Value.ToString("0.00") + Environment.NewLine;
			}
		}

		void DescEntregas()
		{
			SortedList<uint, decimal> valores = new SortedList<uint, decimal>();
			int ctSinEntrega = 0;
			DataTable fSel = _abm.ObtenerSeleccion();
			foreach (DataRow r in fSel.Rows)
			{
				Turno t = new Turno(r);
				CajaEntrega f = CajaEntrega.GetSingleForTurno(t.IdTurno);
				if (f == null)
				{
					ctSinEntrega++;
					continue;
				}
				f.GetFilas();
				foreach (CajaEntregaItem fi in f.Coleccion)
				{
					if (!valores.ContainsKey(fi.IdPagoForma))
						valores.Add(fi.IdPagoForma, 0);
					valores[fi.IdPagoForma] += fi.Monto;
				}
			}
			if (fSel.Rows.Count > 1)
			{
				lblDetalle.Text += "Turnos sin Entregar: " + ctSinEntrega.ToString() + Environment.NewLine + Environment.NewLine;
			}
			decimal total = 0;
			foreach (KeyValuePair<uint, decimal> par in valores)
			{
				PagoForma art = PagoForma.GetSingleE(par.Key);
				lblDetalle.Text += art.Descripcion + ": $ " + par.Value.ToString("0.00").PadLeft(7) + Environment.NewLine;
				total += art.SumaAlTotal ? par.Value : 0;
			}
			if (fSel.Rows.Count > 1)
			{
				lblDetalle.Text += Environment.NewLine + "Total: $ " + total.ToString("0.00").PadLeft(7) + Environment.NewLine;
			}
		}

		#region Busquedas del Control - Clientes, etc.
		void Filtro_SeleccionCancelada(object sender, EventArgs e)
		{
			if (sender is ABMForm)
				(sender as ABMForm).Close();
			else
			{
				IABMAddOn filtro = sender as IABMAddOn;
				ABMForm abm = filtro.ABMTool as ABMForm;
				abm.Close();
				if (_idCliente == 0)
				{
					radClientesTodos.Checked = true;
					txtClienteNombre.Text = "";
				}
			}
		}

		void Filtro_SeleccionAceptada(object sender, EventArgs e)
		{
			IABMAddOn filtro = sender as IABMAddOn;
			ABMForm abm = filtro.ABMTool as ABMForm;
			Cliente c = new Cliente(abm.ItemActual.Row);
			txtClienteNombre.Text = c.Nombre;
			_idCliente = c.IdCliente;
			abm.Close();
		}
		#endregion

		public enum enFiltrado
		{
			Facturas,
			Turnos,
			EntregaDeCaja,
			CCMovimientos
		}
	}
}
