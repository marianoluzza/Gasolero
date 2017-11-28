using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FormCom;
using Negocio;
using MarUtils.Entidades;
using MarUtils.Soporte;
using MarUtils.Controles;
using System.Reflection;
using System.IO;

namespace Gasolero
{
	public partial class FrmMain : Form
	{
		static List<IABMTool> _abms = new List<IABMTool>();
		Control _contenedorPrincipal = null;
		PanelLogin _login = new PanelLogin();
		CVentas _ventas = new CVentas();
		CCierre _cierres = new CCierre();
		static FrmMain _frm;

		public static FrmMain FMain
		{
			get { return FrmMain._frm; }
		}
		/// <summary>
		/// Barra de mensaje
		/// </summary>
		public string Mensaje
		{
			get { return statMensaje.Text; }
			set { statMensaje.Text = value; }
		}

		public FrmMain()
		{
			InitializeComponent();
			_contenedorPrincipal = tabPage1;
			_tpAnterior = tabPage1;
			Reglas.Confirmar += Confirmar;
			if (Program.SinFiscal)
				Reglas.FechaActual += Program.Fecha;
			else
			{
				bool fiscalOn = false;
				try
				{
					fiscalOn = Fiscal.Instalado();
				}
				catch { }
				if (fiscalOn)
					Reglas.FechaActual += Fiscal.Fecha;
				else
					Reglas.FechaActual += Program.Fecha;
			}
			_frm = this;
			FrmMain.Texto = "Gasolero - Turno: " + Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero + 1);
			_login.Visible = false;
			_login.LoggedIn += new EventHandler(Login_LoggedIn);
			_login.LoggedOut += new EventHandler(Login_LoggedOut);
			//splitMain.Panel2.Controls.Add(_login);
			_contenedorPrincipal.Controls.Add(_login);
			_ventas.Visible = false;
			_ventas.Dock = DockStyle.Fill;
			//splitMain.Panel2.Controls.Add(_ventas);
			_contenedorPrincipal.Controls.Add(_ventas);
			Login_LoggedOut(this, EventArgs.Empty);
			_cierres.Visible = false;
			_cierres.Dock = DockStyle.Fill;
			//splitMain.Panel2.Controls.Add(_cierres);
			_contenedorPrincipal.Controls.Add(_cierres);
			splitMain.SplitterDistance = AppConfig.SplitDist;
			confimarValesToolStripMenuItem.Checked = AppConfig.ConfirmarVales;
			confimarValesToolStripMenuItem.CheckOnClick = true;
			CrearAtajos();
			ventasPorClienteToolStripMenuItem.Visible = false;
		}

		#region Seguridad
		private void BAcceso_Click(object sender, EventArgs e)
		{
			OcultarTodo();
			_login.Visible = true;
			_login.Focus();
			tabPage1.ImageKey = "Acceso";
		}

		void Login_LoggedOut(object sender, EventArgs e)
		{
			BVentas.Enabled = false;
			ventasToolStripMenuItem.Enabled = false;
			BABMArticulos.Enabled = false;
			articulosToolStripMenuItem.Enabled = false;
			BABMVendedores.Enabled = false;
			vendedoresToolStripMenuItem.Enabled = false;
			BCierres.Enabled = false;
			cerrarTurnoToolStripMenuItem.Enabled = false;
			revisarCierresToolStripMenuItem.Enabled = false;
			BABMClientes.Enabled = false;
			clientesToolStripMenuItem.Enabled = false;
			BABMPuestos.Enabled = false;
			puestosToolStripMenuItem.Enabled = false;
			bImprimirVales.Enabled = false;
			//bImprimirRpt1.Enabled = false;
			//bImprimirRpt2.Enabled = false;
			principalToolStripMenuItem.Enabled = false;
			modoImprimirToolStripMenuItem.Enabled = false;
			modo1ºCierreZToolStripMenuItem.Enabled = false;
			margenesDeCierreToolStripMenuItem.Enabled = false;
			hacerNCToolStripMenuItem.Visible = false;
			reportesToolStripMenuItem.Enabled = false;
			reportesToolStripMenuItem.Visible = false;
			autorizadosDeCCToolStripMenuItem.Enabled = false;
			while (TABs.TabPages.Count > 1)
				TABs.TabPages.RemoveAt(1);
			BAcceso_Click(BAcceso, EventArgs.Empty);
		}

		public void LogOut()
		{
			_login.LogOut();
		}

		void Login_LoggedIn(object sender, EventArgs e)
		{
			bool acceso = Reglas.VendedorActual.EsRol(enPermisos.Vendedor) > 0;
			bool super = Reglas.VendedorActual.EsRol(enPermisos.Encargado) > 0;
			BVentas.Enabled = true;
			ventasToolStripMenuItem.Enabled = true;
			BABMArticulos.Enabled = super;
			articulosToolStripMenuItem.Enabled = super;
			BABMVendedores.Enabled = acceso;
			vendedoresToolStripMenuItem.Enabled = acceso;
			BABMClientes.Enabled = acceso;
			clientesToolStripMenuItem.Enabled = acceso;
			BABMPuestos.Enabled = super;
			puestosToolStripMenuItem.Enabled = super;
			bImprimirVales.Enabled = true;
			bImprimirRpt1.Enabled = true;
			bImprimirRpt2.Enabled = true;
			//
			BCierres.Enabled = true;
			cerrarTurnoToolStripMenuItem.Enabled = true;
			revisarCierresToolStripMenuItem.Enabled = Reglas.VendedorActual.EsRol(enPermisos.SysAdmin) == 0;
			resolucionToolStripMenuItem.Visible = Reglas.VendedorActual.EsRol(enPermisos.SysAdmin) == 0;
			petroResToolStripMenuItem.Visible = super;
			confimarValesToolStripMenuItem.Visible = super;
			entregaDeCajaToolStripMenuItem.Enabled = acceso;
			formasDePagoToolStripMenuItem.Enabled = super;
			gastosToolStripMenuItem.Visible = false;
			conceptosToolStripMenuItem.Visible = false;
			cierreDePeriodoToolStripMenuItem.Visible = false;
			//	
			principalToolStripMenuItem.Enabled = true;
			//parchesToolStripMenuItem.Visible = super;
			//
			establerInvervaloToolStripMenuItem.Enabled = acceso;
			maxFacturableToolStripMenuItem.Visible = acceso;
			modoFiscalToolStripMenuItem.Visible = super &&
				!Reglas.VendedorActual.TienePermiso(enPermisos.Administrador);
			//-------------- ZEEB ---------------
			editarSalidaVOXToolStripMenuItem.Enabled = super;
			//-------------- /ZEEB ---------------

			modeloFiscalToolStripMenuItem.Enabled = super;
			marcaParcialToolStripMenuItem.Visible = super;
			imprimirParcialFinalToolStripMenuItem.Visible = super;
			cierreXToolStripMenuItem.Enabled = true;
			cierreZToolStripMenuItem.Enabled = true;
			cierreZPorFechasToolStripMenuItem.Enabled = super;
			cierreZporNumToolStripMenuItem.Enabled = super;
			cierreZporNumToolStripMenuItem.Checked = AppConfig.ZGlobal;
			cierreZPeriodoToolStripMenuItem.Enabled = super;
			modoImprimirToolStripMenuItem.Enabled = super;
			modo1ºCierreZToolStripMenuItem.Enabled = super;
			margenesDeCierreToolStripMenuItem.Enabled = acceso;
			hacerNCToolStripMenuItem.Visible = super;
			reportesToolStripMenuItem.Enabled = super;
			reportesToolStripMenuItem.Visible = super;
			accessToolStripMenuItem.Visible = super;
			fixVehiculosToolStripMenuItem.Visible = super && Reglas.VerFix;
			clienteParaNCBToolStripMenuItem.Enabled = super;
			autorizadosDeCCToolStripMenuItem.Enabled = acceso;
			//
			_cierres.ReveerSeguridad();
			_cierres.Refrescar();
			//
			AppConfig.UltimoUsuario = Reglas.VendedorActual.IdVendedor;
		}

		void AdminPermisos(Vendedor v)
		{
			if (Reglas.VendedorActual.Permisos <= v.Permisos)
			{
				MessageBox.Show("No tiene permiso para modificar los permisos", "Permisos Insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			EsquemaCollection esquemas = new EsquemaCollection();
			DataTable dtRoles = new DataTable("Roles");
			dtRoles.Columns.Add("Id");
			dtRoles.Columns.Add("Nombre");
			dtRoles.PrimaryKey = new DataColumn[] { dtRoles.Columns["Id"] };
			dtRoles.Rows.Add((int)enPermisos.Administrador, "Administrador");
			dtRoles.Rows.Add((int)enPermisos.SuperUsuario, "Super Usuario");
			dtRoles.Rows.Add((int)enPermisos.Encargado, "Encargado");
			dtRoles.Rows.Add((int)enPermisos.Vendedor, "Vendedor");
			Esquema esq = new Esquema();
			esq.Dato = "Rol";
			esq.Tipo = Esquema.enControles.ComboBox;
			esq.Tabla = "Roles";
			esq.TablaDisplay = "Nombre";
			esq.TablaId = "Id";
			if (v.EsRol(enPermisos.Administrador) == 0)
				esq.ValorDefectoOriginal = (int)enPermisos.Administrador;
			else if (v.EsRol(enPermisos.SuperUsuario) == 0)
				esq.ValorDefectoOriginal = (int)enPermisos.SuperUsuario;
			else if (v.EsRol(enPermisos.Encargado) == 0)
				esq.ValorDefectoOriginal = (int)enPermisos.Encargado;
			else
				esq.ValorDefectoOriginal = (int)enPermisos.Vendedor;
			esquemas.Add(esq);
			Prompt prompt = new Prompt(esq, new DataView(dtRoles));
			if (prompt.ShowDialog() != DialogResult.OK)
				return;
			v.SetPermiso((enPermisos)int.Parse(prompt.GetValue().ToString()), false);
			ABMResultado abmRes = v.Modificacion();
			if (abmRes.CodigoError == enErrores.Ninguno)
			{
				MessageBox.Show("Permisos Actualizados", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
				MessageBox.Show(abmRes.MensajeError, "Fallo", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void AdminPass(Vendedor v)
		{
			if (Reglas.VendedorActual.IdVendedor != v.IdVendedor && Reglas.VendedorActual.Permisos <= v.Permisos)
			{
				MessageBox.Show("No tiene permiso para modificar las contraseñas", "Permisos Insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			Esquema esq = new Esquema();
			esq.Dato = "Contraseña";
			esq.Tipo = Esquema.enControles.TextBox;
			esq.Mascara = "*";
			Prompt prompt = new Prompt(esq);
			if (prompt.ShowDialog() != DialogResult.OK)
				return;
			ABMResultado abmRes = v.CambiarPass(prompt.GetValue().ToString());
			if (abmRes.CodigoError == enErrores.Ninguno)
			{
				MessageBox.Show("Contraseña Actualizada", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
				_login.ActualizarTabla(Vendedor.GetAll());
			}
			else
				MessageBox.Show(abmRes.MensajeError, "Fallo", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		#endregion Seguridad

		#region ABM
		void ABM_EdicionIntentada(object sender, Esquema dato)
		{
			ABMDatos abmData = sender as ABMDatos;
			if (dato.Dato.ToLower() == "permisos")
			{
				Vendedor v = new Vendedor(abmData.ABMTool.ItemActual.Row);
				AdminPermisos(v);
				DataTable dtVendedores = Vendedor.GetAll();
				abmData.ABMTool.ActualizarTabla(dtVendedores);
				abmData.ABMTool.SeleccionarItem(v.IdVendedor);
				_login.ActualizarTabla(dtVendedores);
			}
			else if (dato.Dato.ToLower() == "password")
			{
				Vendedor v = new Vendedor(abmData.ABMTool.ItemActual.Row);
				AdminPass(v);
				DataTable dtVendedores = Vendedor.GetAll();
				abmData.ABMTool.ActualizarTabla(dtVendedores);
				abmData.ABMTool.SeleccionarItem(v.IdVendedor);
				_login.ActualizarTabla(dtVendedores);
			}
			else if (dato.Dato.ToLower() == "idcliente")
			{
				Cliente cc = Program.BuscarCliente();
				if (cc != null)
				{
					abmData.SetDato(dato.Dato, cc.IdCliente);
				}
			}
			else if (dato.Dato.ToLower() == "descuento")
			{
				if (!Reglas.VendedorActual.TienePermiso(enPermisos.SuperUsuario))
				{
					MessageBox.Show("No tiene permisos para modificar el descuento del cliente", "Permisos Insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
				dato.ValorDefectoActual = abmData.GetValor(dato.Dato);
				Prompt prDto = new Prompt(dato);
				if (prDto.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return;
				decimal dto = (decimal)prDto.GetValue();
				if (dto < 0)
				{
					MessageBox.Show("No tiene permisos para modificar el descuento del cliente", "Permisos Insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
				abmData.SetDato(dato.Dato, dto);
			}
			else if (dato.Dato.ToLower().StartsWith("saldo"))
			{
				if (!Reglas.VendedorActual.TienePermiso(enPermisos.SuperUsuario))
				{
					MessageBox.Show("No tiene permisos para modificar el saldo del cliente", "Permisos Insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
				Prompt prSaldoAdv = new Prompt(dato);
				if (prSaldoAdv.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return;
				decimal saldo = (decimal)prSaldoAdv.GetValue();
				if (saldo < 0)
				{
					MessageBox.Show("No tiene permisos para modificar el saldo del cliente", "Permisos Insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
				abmData.SetDato(dato.Dato, saldo);
			}
			else if (dato.Dato.ToUpper() == "CUIT")
			{
				string msn = Fiscal.CUITCheck(abmData.GetValores()[dato.Dato].ToString()) ?
					"CUIT correcto" : "CUIT incorrecto";
				MessageBox.Show(msn, "CUIT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		void FrmMain_ABMIntentado(object sender, ABMEventeArgs e)
		{
			ABMDatos abmData = sender as ABMDatos;
			IABMTool abm = abmData.ABMTool;
			EntidadBase ent = Reglas.New(abm.TablaMain, e.Valores);
			ABMResultado abmRes;
			uint id = ent.GetProp<uint>(ent.GetPK());
			string entNbre = ent.GetNombreEntidad();
			if (e.Funcion == enABMFuncion.Alta)
			{
				if (id > 0 && MessageBox.Show("Está intentando dar de alta un/a " + entNbre + " existente." + Environment.NewLine + "¿Está seguro?", "Confirmar Alta",
					 MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return;
				abmRes = ent.Alta();
			}
			else if (e.Funcion == enABMFuncion.Modificacion)
			{
				if (id == 0 && MessageBox.Show("Está intentando modificar un/a " + entNbre + " inexistente." + Environment.NewLine + "¿Desea darle de alta en su lugar?", "Confirmar Alta",
					 MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return;
				abmRes = id == 0 ? ent.Alta() : ent.Modificacion();
			}
			else
			{
				if (id == 0)
				{
					MessageBox.Show("Debe elegir un/a " + entNbre + " existente para realizar una baja.", "Error al eliminar",
					 MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (MessageBox.Show("¿Está seguro que desea eliminar el/a " + entNbre + "?", "Confirmar Baja",
					 MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return;
				abmRes = ent.Baja();
			}
			statMensaje.Text = abmRes.CodigoError == enErrores.Ninguno ? "Operación realizada exitosamente" :
				("Operación fallida: " + abmRes.MensajeError);
			if (abmRes.CodigoError != enErrores.Ninguno)
				return;
			DataTable dtAll;
			dtAll = ent.GetType().GetMethod("GetAll").Invoke(ent, null) as DataTable;
			uint ultid = e.Funcion == enABMFuncion.Alta ? abmRes.IdInsercion :
				uint.Parse(abm.ItemActual.Row[dtAll.PrimaryKey[0].ColumnName].ToString());
			//abm.ActualizarTabla(dtAll);
			FrmMain.ActualizarABMs(dtAll);
			_ventas.ActualizarTabla(dtAll);
			_login.ActualizarTabla(dtAll);
			if (abm.TablaMain == Articulo.NombreTabla)
				_cierres.ActualizarArticulo(ultid);
			if (e.Funcion != enABMFuncion.Baja && abmRes.CodigoError == enErrores.Ninguno)
				abm.SeleccionarItem(ultid);
		}

		void ABM_TablaInvalidada(IABMTool abm, string tabla)
		{
			DataTable dt = Reglas.GetAll(tabla);
			if (dt != null)
			{
				abm.ActualizarTabla(dt);
				foreach (var item in _abms)
				{
					if (item != abm && item.ExisteTabla(tabla))
						item.ActualizarTabla(dt);
				}
			}
		}
		#endregion

		#region Facturacion y Cierres
		private void facturasToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Facturas"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(FactEncabezado.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(Cliente.GetAll());
			abm.ActualizarTabla(PagoForma.GetAll());
			abm.ActualizarTabla(FactEncabezado.GetListForFechas(Reglas.Now.Date, Reglas.Now.Date.AddDays(1)));
			//abm.ActualizarTabla(FactEncabezado.GetSingle(1));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = FactEncabezado.NombreTabla;
			abm.AddAddOn(new FactFiltro());
			ABMFiltro filtro = abm.ObtenerAddOn<ABMFiltro>();
			filtro.SeleccionAceptada += new EventHandler(Facturas_SeleccionAceptada);
			TabPage tp = CrearTabPage(abm, "Facturas", "Facturas");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
			//
			//FactEncabezado fact = FactEncabezado.GetSingleE(1);
			//CrearTabPage(new CVentas(fact), "Fact");
		}

		void Facturas_SeleccionAceptada(object sender, EventArgs e)
		{
			ABMFiltro filtro = sender as ABMFiltro;
			ABMGrilla abm = filtro.ABMTool as ABMGrilla;
			if (abm.ItemActual == null)
				return;
			object idO = abm.ItemActual[FactEncabezado.NombreClave];
			if (idO == null)
				return;
			uint id;
			if (!uint.TryParse(idO.ToString(), out id))
				return;
			FactEncabezado fact = FactEncabezado.GetSingleE(id);
			if (!TABs.TabPages.ContainsKey(fact.ToString()))
				CrearTabPage(new CVentas(fact), fact.ToString(), fact.ToString());
			SeleccionarPagina(fact.ToString());
		}

		private void revisarCierresToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!SeleccionarPagina("Cierres Anteriores"))
			{
				CCierre ccierre = new CCierre(true);
				ccierre.ReveerSeguridad();
				SeleccionarPagina(CrearTabPage(ccierre, "Cierres Anteriores", "Cierres Anteriores"));
			}
		}

		private void formasDePagoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Formas de Pago"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(PagoForma.Esquema());
			abm.ActualizarTabla(PagoForma.GetAll());
			ABMDatos abmData = abm.ObtenerAddOn<ABMDatos>();
			abmData.ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abmData.BusquedaIntentada += new ABMDatos.OperacionIntentadaHandler(ABM_EdicionIntentada);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = PagoForma.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Formas de Pago", "Formas de Pago");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void entregaDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Entregas de Caja"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			abm.ActualizarTabla(Turno.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(Turno.GetEntregasForFechas(DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Turno.NombreTabla;
			abm.AddAddOn(new CEntregas());
			abm.Load += delegate(object s2, EventArgs e2)
			{
				abm.Colorear(true);
			};
			TabPage tp = CrearTabPage(abm, "Entregas de Caja", "Entregas de Caja");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void cierreDePeriodoToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void gastosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Gastos"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(FactEncabezado.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(Cliente.GetAll());
			abm.ActualizarTabla(FactEncabezado.GetListForFechas(Reglas.Now.Date, Reglas.Now.Date.AddDays(1)));
			//abm.ActualizarTabla(FactEncabezado.GetSingle(1));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = FactEncabezado.NombreTabla;
			abm.AddAddOn(new FactFiltro());
			ABMFiltro filtro = abm.ObtenerAddOn<ABMFiltro>();
			filtro.SeleccionAceptada += new EventHandler(Facturas_SeleccionAceptada);
			TabPage tp = CrearTabPage(abm, "Gastos", "Gastos");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void conceptosToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void imprimirValesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string res = "";
			try
			{
				int ct = 0;
				DataTable dt = Operacion.GetVales(Reglas.UltimoIdImpreso + 1);
				foreach (DataRow r in dt.Rows)
				{
					Operacion op = new Operacion(r);
					if (Fiscal.ImprimirVale(op))
						ct++;
				}
				switch (ct)
				{
					case 0:
						res = "No se imprimió ningún vale";
						break;
					case 1:
						res = "Se imprimió un vale satisfactoriamente";
						break;
					default:
						res = "Se imprimieron " + ct + " vales satisfactoriamente";
						break;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al imprimir vales", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			if (res != "")
				statMensaje.Text = res;
		}
		#endregion

		#region Menu Principal
		private void BVentas_Click(object sender, EventArgs e)
		{
			OcultarTodo();
			_ventas.Visible = true;
			ventasMenu.Visible = true;
			_ventas.Focus();
			tabPage1.ImageKey = "Venta";
		}

		private void BCierres_Click(object sender, EventArgs e)
		{
			OcultarTodo();
			_cierres.Visible = true;
			_cierres.Focus();
			tabPage1.ImageKey = "Cierre";
		}

		private void BABMVendedores_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Vendedores"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(Vendedor.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			ABMDatos abmData = abm.ObtenerAddOn<ABMDatos>();
			abmData.ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abmData.EdicionIntentada += new ABMDatos.OperacionIntentadaHandler(ABM_EdicionIntentada);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Vendedor.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Vendedores", "Vendedores");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void BABMClientes_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Clientes"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(Cliente.Esquema());
			abm.ActualizarTabla(Cliente.GetAll());
			ABMDatos abmData = abm.ObtenerAddOn<ABMDatos>();
			abmData.ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abmData.BusquedaIntentada += new ABMDatos.OperacionIntentadaHandler(ABM_EdicionIntentada);
			abmData.EdicionIntentada += new ABMDatos.OperacionIntentadaHandler(ABM_EdicionIntentada);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Cliente.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Clientes", "Clientes");
			abm.TablaInvalidada += new ABMTablaInvalidada(ABM_TablaInvalidada);
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void BABMArticulos_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Articulos"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(Articulo.Esquema());
			abm.ActualizarTabla(Articulo.GetAll());
			abm.ActualizarTabla(Producto.GetAll());
			abm.ObtenerAddOn<ABMDatos>().ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Articulo.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Articulos", "Articulos");
			abm.TablaInvalidada += new ABMTablaInvalidada(ABM_TablaInvalidada);
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void fichasToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Fichas"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(Ficha.Esquema());
			abm.ActualizarTabla(Ficha.GetAll());
			abm.ObtenerAddOn<ABMDatos>().ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Ficha.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Fichas", "Fichas");
			abm.TablaInvalidada += new ABMTablaInvalidada(ABM_TablaInvalidada);
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void BABMPuestos_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Puestos"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(Puesto.Esquema());
			abm.ActualizarTabla(Puesto.GetAll());
			abm.ActualizarTabla(Articulo.GetAll());
			abm.ObtenerAddOn<ABMDatos>().ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Puesto.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Puestos", "Puestos");
			abm.TablaInvalidada += new ABMTablaInvalidada(ABM_TablaInvalidada);
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}
		#endregion

		#region Cuentas Corrientes
		private void movimientosDeCCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Cuenta Corriente"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			abm.ActualizarTabla(CCMovimiento.Esquema());
			abm.ActualizarTabla(CCMovimiento.GetMovimientos(DateTime.Today, DateTime.Today.AddDays(1)));
			abm.ActualizarTabla(PagoForma.GetAll());
			abm.ActualizarTabla(Cliente.GetAll());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = CCMovimiento.NombreTabla;
			FactFiltro ff = new FactFiltro(FactFiltro.enFiltrado.CCMovimientos);
			abm.AddAddOn(ff);
			abm.AddAddOn(new Exportador(true, true, "Movimientos de Cuenta Corriente"));
			abm.Load += delegate(object s2, EventArgs e2)
			{
				ff.Filtrar(DateTime.Today, DateTime.Today.AddDays(1), CheckState.Indeterminate);
			};
			TabPage tp = CrearTabPage(abm, "Cuenta Corriente", "Cuenta Corriente");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void movimientosDelDiaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//if (MessageBox.Show("¿Imprimir movimientos de Cuenta Corriente del día?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
			ImprimirCCTurno(false);
		}

		public void ImprimirCCTurno(bool imprimir)
		{
			//ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			//abm.ActualizarTabla(CCMovimiento.Esquema());
			//Turno tAnt = Turno.GetForNumero(Reglas.TurnoUltimo.Numero - 1);
			//DataTable dtMovs = CCMovimiento.GetMovimientos(tAnt.FechaFinal, Reglas.TurnoUltimo.FechaFinal);
			//dtMovs.Rows.RemoveAt(0);
			//dtMovs.Columns.RemoveAt(dtMovs.Columns.Count - 1);
			//abm.ActualizarTabla(dtMovs);
			//abm.ActualizarTabla(Cliente.GetAll());
			//abm.ActualizarTabla(PagoForma.GetAll()); 
			//abm.ActualizarTabla(Vendedor.GetAll());
			//FactFiltro ff = new FactFiltro(FactFiltro.enFiltrado.CCMovimientos);
			//abm.AddAddOn(ff);
			//abm.TablaEsquema = "Esquemas";
			//abm.TablaMain = CCMovimiento.NombreTabla;
			//abm.Load += delegate(object s2, EventArgs e2)
			//{
			//    abm.Imprimir("Movimientos de CC del Turno " + Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero));
			//    QuitarPagina(TABs.TabPages["Movimientos de CC del Turno"]);
			//};
			//TabPage tp = CrearTabPage(abm, "Movimientos de CC del Turno", "Movimientos de CC del Turno");
			//abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			//{
			//    QuitarPagina(tp);
			//};
			//SeleccionarPagina(tp);
			Turno tAnt = Turno.GetForNumero(Reglas.TurnoUltimo.Numero - 1);
			RptCCMov x = new RptCCMov(tAnt.FechaFinal, Reglas.TurnoUltimo.FechaFinal);
			//x.Imprimir();
			LVVisor l = new LVVisor();
			Turno t = Reglas.TurnoUltimo;
			//l.Text = "Movimientos de CC del Turno " + Turno.GetTurnoNbre(t.Numero) + "\\t\\t" + Reglas.Now.ToShortDateString();
			l.Text = "Movimientos de CC del Turno " + Turno.GetTurnoNbre(t.Numero);
			l.CargarRpt(x);
			if (imprimir)
			{
				l.Width = 660;
				l.Show();
				l.Imprimir();
				l.Close();
			}
			else
			{
				l.WindowState = FormWindowState.Maximized;
				l.ShowDialog();
			}
		}

		private void autorizadosDeCCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Autorizados CC"))
				return;
			ABMGrilla abm = CrearABM(true, true) as ABMGrilla;
			abm.ActualizarTabla(CCAutorizado.Esquema());
			abm.ActualizarTabla(Cliente.GetAll());
			abm.ActualizarTabla(CCAutorizado.GetAll());
			ABMDatos abmData = abm.ObtenerAddOn<ABMDatos>();
			abmData.ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abmData.BusquedaIntentada += new ABMDatos.OperacionIntentadaHandler(ABM_EdicionIntentada);
			abmData.EdicionIntentada += new ABMDatos.OperacionIntentadaHandler(ABM_EdicionIntentada);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = CCAutorizado.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Autorizados CC", "Autorizados CC");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}
		#endregion

		#region Control ABM - Pestañas
		public static void ActualizarABMs(DataTable dt)
		{
			foreach (IABMTool abm in _abms)
			{
				if (abm.ExisteTabla(dt.TableName))
					abm.ActualizarTabla(dt);
			}
		}

		public IABMTool CrearABM(bool conFiltro, bool conDatos)
		{
			ABMGrilla abm = new ABMGrilla(conFiltro, conDatos);
			//ABMGrillaView abm = new ABMGrillaView(conFiltro, conDatos);
			abm.Load += delegate(object sender, EventArgs e)
			{
				new System.Threading.Thread(delegate()
				{
					System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
					abm.Invoke((MethodInvoker)delegate
					{
						abm.ReajustarGrilla();
						abm.Refresh();
					});
				}).Start();
			};
			_abms.Add(abm);
			return abm;
		}

		public TabPage CrearTabPage(Control ctrl, string txt)
		{
			return CrearTabPage(ctrl, "", txt);
		}

		public TabPage CrearTabPage(Control ctrl, string key, string txt)
		{
			if (key != "")
				TABs.TabPages.Add(key, txt);
			else
				TABs.TabPages.Add(txt);
			TabPage res = TABs.TabPages[TABs.TabPages.Count - 1];
			if (imgTab.Images.ContainsKey(key))
				res.ImageKey = key;
			ctrl.Dock = DockStyle.Fill;
			res.Tag = ctrl;
			res.Controls.Add(ctrl);
			return res;
		}

		public void SeleccionarPagina(TabPage tp)
		{
			TABs.SelectedTab = tp;
		}

		public bool SeleccionarPagina(string key)
		{
			bool res = TABs.TabPages.ContainsKey(key);
			if (res)
				TABs.SelectedTab = TABs.TabPages[key];
			return res;
		}

		public void QuitarPagina(TabPage tp)
		{
			if (tp == tabPage1)
			{
				Close();
				return;
			}
			if (tp.Tag is IABMTool)
			{
				IABMTool abm = tp.Tag as IABMTool;
				_abms.Remove(abm);
			}
			if (tp != _tpAnterior && TABs.TabPages.Contains(_tpAnterior))
				TABs.SelectedTab = _tpAnterior;
			TABs.TabPages.Remove(tp);
		}

		public void QuitarPagina()
		{
			if (TABs.SelectedTab != tabPage1)
				QuitarPagina(TABs.SelectedTab);
		}

		#endregion Control ABM - Pestañas

		#region Control Interno
		public static string Texto
		{
			get { return _frm.Text; }
			set { _frm.Text = value; }
		}

		void OcultarTodo()
		{
			//foreach (Control c in splitMain.Panel2.Controls)
			foreach (Control c in _contenedorPrincipal.Controls)
				c.Visible = false;
			ventasMenu.Visible = false;
			SeleccionarPagina(tabPage1);
			//if (Reglas.VendedorActual != null)
			//{
			//    _contenedorPrincipal.Controls.Clear();
			//    panelBotonera.Controls.Clear();
			//    splitMain.Panel1Collapsed = true;
			//}
		}

		public bool Confirmar(string s)
		{
			return MessageBox.Show(s, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{
			_login.FocusTB();
			//set chequeds
			envioDeEmailsToolStripMenuItem.CheckedChanged -= new EventHandler(envioDeEmailsToolStripMenuItem_CheckedChanged);
			modoFiscalToolStripMenuItem.CheckedChanged -= new EventHandler(modoFiscalToolStripMenuItem_CheckedChanged);
			movDeCCAFiscalToolStripMenuItem.CheckedChanged -= new EventHandler(movDeCCAFiscalToolStripMenuItem_CheckedChanged);
			editarSalidaVOXToolStripMenuItem.CheckedChanged -= new EventHandler(editarSalidaVOXToolStripMenuItem_CheckedChanged);
			envioDeEmailsToolStripMenuItem.Checked = Reglas.EnviarEmail;
			modoFiscalToolStripMenuItem.Checked = AppConfig.UsarFiscal;
			movDeCCAFiscalToolStripMenuItem.Checked = Reglas.CCFiscal;
			editarSalidaVOXToolStripMenuItem.Checked = Reglas.EditarSalidaVOX;
			envioDeEmailsToolStripMenuItem.CheckedChanged += new EventHandler(envioDeEmailsToolStripMenuItem_CheckedChanged);
			modoFiscalToolStripMenuItem.CheckedChanged += new EventHandler(modoFiscalToolStripMenuItem_CheckedChanged);
			movDeCCAFiscalToolStripMenuItem.CheckedChanged += new EventHandler(movDeCCAFiscalToolStripMenuItem_CheckedChanged);
			editarSalidaVOXToolStripMenuItem.CheckedChanged += new EventHandler(editarSalidaVOXToolStripMenuItem_CheckedChanged);
		}

		private void BCerrar_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void xToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (TABs.SelectedIndex > 0)
			{
				QuitarPagina(TABs.TabPages[TABs.SelectedIndex]);
			}
			else
				Close();
		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
#warning Chequeos de salida
			if (TABs.TabPages.Count > 1)
			{
				if (MessageBox.Show("Tiene otras pestañas abiertas." + Environment.NewLine + "¿Desea cerrar de todas formas?",
					"Confirmar Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					e.Cancel = true;
			}
		}

		private void splitMain_SplitterMoved(object sender, SplitterEventArgs e)
		{
			////int diferencia = splitMain.SplitterDistance - AppConfig.SplitDist;
			//if (splitMain.SplitterDistance > 251)
			//{
			//    splitMain.SplitterDistance = 251;

			//}
			//else
			//{
			//    AppConfig.SplitDist = splitMain.SplitterDistance;
			//    //imgBotonera.ImageSize = new Size(imgBotonera.ImageSize.Width + diferencia, imgBotonera.ImageSize.Height + diferencia);
			//    Image[] imgs = new Image[imgOriginales.Images.Count];
			//    int i = 0;
			//    foreach (Image item in imgOriginales.Images)
			//    {
			//        imgs[i++] = item;
			//    }
			//    imgBotonera.ImageSize = new Size(AppConfig.SplitDist + 5, AppConfig.SplitDist + 5);
			//    imgBotonera.Images.AddRange(imgs);
			//    int inicio = 0;
			//    int fin;
			//    SortedList<int, Control> controles = new SortedList<int, Control>();
			//    foreach (Control item in panelBotonera.Controls)
			//    {
			//        controles.Add(item.Top, item);
			//    }
			//    foreach (Control item in controles.Values)
			//    {
			//        fin = item.Bottom;
			//        item.Height = AppConfig.SplitDist - 7;
			//        if ((item.Anchor & AnchorStyles.Top) > 0)
			//            item.Top = inicio;
			//        else
			//            item.Top = fin - item.Height;
			//        inicio += item.Height;
			//    }
			//}
		}

		TabPage _tpAnterior = null;
		private void TABs_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.Action == TabControlAction.Deselecting)
				_tpAnterior = e.TabPage;
			else if (e.Action == TabControlAction.Selecting)
			{
				if (e.TabPage.Tag is IABMTool)
				{
					IABMTool abm = e.TabPage.Tag as IABMTool;
					//abm.Control.Invalidate();
					buscarABMToolStripMenuItem.Visible = true;
				}
				else
					buscarABMToolStripMenuItem.Visible = false;
			}
		}

		private void buscarABMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tab = TABs.SelectedTab;
			if (tab.Tag is IABMTool)
			{
				IABMTool abm = tab.Tag as IABMTool;
				if (abm.ContieneAddOn(typeof(FormCom.ABMFiltro)))
				{
					Esquema esq = null;
					string[] camposPosibles = new String[] { "Nombre", "Descripcion" };
					foreach (var campo in camposPosibles)
					{
						foreach (var item in abm.Esquemas)
						{
							if (item.Dato.ToLower() == campo.ToLower() || item.Alias.ToLower() == campo.ToLower())
							{
								esq = item;
								break;
							}
							if (esq != null)
								break;
						}
					}
					if (esq != null)
					{
						Prompt pr = new Prompt(esq);
						if (pr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						{
							var filtro = abm.ObtenerAddOn<FormCom.ABMFiltro>();
							string valor = (pr.GetValue() ?? "").ToString();
							filtro.SetDato(esq.Dato, valor != "" ? "*" + valor + "*" : "");
						}
					}
				}
			}
		}
		#endregion Control Interno

		#region Settings - Opciones
		private void establerInvervaloToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Esquema esq = new Esquema();
			esq.Tipo = Esquema.enControles.TextBox;
			esq.Mascara = "00";
			esq.Dato = "Dias";
			esq.Comentario = "Cada cuantos dias?";
			esq.ValorDefectoOriginal = AppConfig.IntervaloBackup;
			Prompt p = new Prompt(esq);
			p.Text = "Ingrese intervalo de respaldo";
			uint dias = 0;
			do
			{
				if (p.ShowDialog() == DialogResult.OK)
				{
					if (uint.TryParse(p.GetValue().ToString(), out dias))
					{
						AppConfig.IntervaloBackup = dias;
						Program.CheckBackup();
					}
				}
				else
					break;
			} while (dias < 0);
		}

		private void modo1ºCierreZToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Esquema esq = new Esquema();
			esq.Tipo = Esquema.enControles.TextBox;
			esq.Mascara = "0";
			esq.Dato = "Modo";
			esq.Comentario = "0=nunca,1=siempre,2=solo dia 1,3=00hs,5=3+2";
			esq.ValorDefectoOriginal = (int)AppConfig.CierreZ1raVenta;
			Prompt p = new Prompt(esq);
			p.Text = "Ingrese modo de 1º cierre Z";
			uint modo = 6;
			do
			{
				if (p.ShowDialog() == DialogResult.OK)
				{
					if (uint.TryParse(p.GetValue().ToString(), out modo))
					{
						AppConfig.CierreZ1raVenta = (AppConfig.enCierreZ1raVenta)modo;
					}
				}
				else
					break;
			} while (modo == 4 || modo > 5);
		}

		private void hacerUnoAhoraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Program.HacerBackup();
		}

		private void reiniciarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Program.Reiniciar();
		}

		private void marcaParcialToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Esquema esq = new Esquema();
			esq.Dato = "Marca de Agua";
			esq.ValorDefectoOriginal = Reglas.MarcaParcial;
			Prompt pr = new Prompt(esq);
			if (pr.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				string res = pr.GetValue().ToString();
				Reglas.MarcaParcial = res;
			}
			catch { }
		}

		private void imprimirParcialFinalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Esquema esq = new Esquema();
			esq.Dato = "Imprimir Parcial Final";
			esq.Tipo = Esquema.enControles.CheckBox;
			esq.ValorDefectoOriginal = Reglas.ImprimirFinalComoParcial;
			Prompt pr = new Prompt(esq);
			if (pr.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				bool res = bool.Parse(pr.GetValue().ToString());
				Reglas.ImprimirFinalComoParcial = res;
			}
			catch { }
		}
		#endregion

		#region Atajos Fiscales
		private void modoFiscalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			bool check = modoFiscalToolStripMenuItem.Checked;
			string msj = check ? "¿Está seguro que quiere activar el modo fiscal?" :
				"¿Está seguro que quiere desactivar el modo fiscal?";
			if (MessageBox.Show(msj, "Modo Fiscal", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
				AppConfig.UsarFiscal = check;
			else
			{
				modoFiscalToolStripMenuItem.CheckedChanged -= new EventHandler(modoFiscalToolStripMenuItem_CheckedChanged);
				modoFiscalToolStripMenuItem.Checked = !check;
				modoFiscalToolStripMenuItem.CheckedChanged += new EventHandler(modoFiscalToolStripMenuItem_CheckedChanged);
			}
		}

		private void movDeCCAFiscalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			bool check = movDeCCAFiscalToolStripMenuItem.Checked;
			string msj = check ? "¿Está seguro que quiere activar el modo fiscal para movimientos de cuenta corriente?" :
				"¿Está seguro que quiere desactivar el modo fiscal para movimientos de cuenta corriente?";
			if (MessageBox.Show(msj, "Modo Fiscal para Cta.Cte.", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
				Reglas.CCFiscal = check;
			else
			{
				movDeCCAFiscalToolStripMenuItem.CheckedChanged -= new EventHandler(movDeCCAFiscalToolStripMenuItem_CheckedChanged);
				movDeCCAFiscalToolStripMenuItem.Checked = !check;
				movDeCCAFiscalToolStripMenuItem.CheckedChanged += new EventHandler(movDeCCAFiscalToolStripMenuItem_CheckedChanged);
			}
		}

		private void editarSalidaVOXToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			bool check = editarSalidaVOXToolStripMenuItem.Checked;
			string msj = check ? "¿Está seguro que quiere activar la edición de los campos salida de la planilla de cierre que carga la VOX?" :
				"¿Está seguro que quiere desactivar la edición de los campos salida de la planilla de cierre que carga la VOX?";
			if (MessageBox.Show(msj, "Edición salida VOX", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
				Reglas.EditarSalidaVOX = check;
			else
			{
				editarSalidaVOXToolStripMenuItem.CheckedChanged -= new EventHandler(editarSalidaVOXToolStripMenuItem_CheckedChanged);
				editarSalidaVOXToolStripMenuItem.Checked = !check;
				editarSalidaVOXToolStripMenuItem.CheckedChanged += new EventHandler(editarSalidaVOXToolStripMenuItem_CheckedChanged);
			}
		}

		private void maxFacturableToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Esquema esq = new Esquema();
			esq.Dato = "Monto";
			esq.Tipo = Esquema.enControles.Money;
			esq.Mascara = "$00000000.00";
			esq.ValorDefectoOriginal = FactEncabezado.GetTotalMaximo();
			Prompt pr = new Prompt(esq);
			if (pr.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				FactEncabezado.SetTotalMaximo(decimal.Parse(pr.GetValue().ToString()));
			}
			catch { }
		}

		private void margenesDeCierreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EsquemaCollection esqCol = new EsquemaCollection();
			Esquema esq = new Esquema();
			esq.Dato = "Margen Inf %";
			esq.Tipo = Esquema.enControles.Money;
			esq.Mascara = "000";
			esq.ValorDefectoOriginal = Reglas.MargenInf;
			esqCol.Add(esq);
			esq = new Esquema();
			esq.Dato = "Margen Sup %";
			esq.Tipo = Esquema.enControles.Money;
			esq.Mascara = "000";
			esq.ValorDefectoOriginal = Reglas.MargenSup;
			esqCol.Add(esq);
			esq = new Esquema();
			esq.Dato = "Turnos para Promedio";
			esq.Tipo = Esquema.enControles.Money;
			esq.Mascara = "000";
			esq.ValorDefectoOriginal = Reglas.TurnosParaPromedio;
			esqCol.Add(esq);
			esq = new Esquema();
			esq.Dato = "Modo Chequeo";
			esq.Tipo = Esquema.enControles.Money;
			esq.Mascara = "0";
			esq.ValorDefectoOriginal = (int)Reglas.ModoMargen;
			esqCol.Add(esq);
			Prompt pr = new Prompt(esqCol, null);
			if (pr.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				Reglas.MargenInf = int.Parse(pr.GetValue("Margen Inf %").ToString());
				Reglas.MargenSup = int.Parse(pr.GetValue("Margen Sup %").ToString());
				Reglas.TurnosParaPromedio = int.Parse(pr.GetValue("Turnos para Promedio").ToString());
				Reglas.ModoMargen = (enModoMargen)int.Parse(pr.GetValue("Modo Chequeo").ToString());
			}
			catch { }
		}

		private void cierreZToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Fiscal.CierreZ();
		}

		private void cierreZPorFechasToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Fiscal.CierreZPorFechas();
		}

		private void cierreZporNumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Fiscal.CierreZPorNum();
		}

		private void cierreZGlobalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			AppConfig.ZGlobal = cierreZporNumToolStripMenuItem.Checked;
		}

		private void cierreZPeriodoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Fiscal.CierreZPorFechasXls();
		}

		private void cierreXToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Fiscal.CierreX();
		}

		private void puntoDecimalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Prompt p = new Prompt();
			try
			{
				p.ShowDialog();
				MessageBox.Show("Puso: " + decimal.Parse(p.GetValue().ToString()).ToString());
			}
			catch
			{
				MessageBox.Show("No puso nada");
			}
		}

		private void modoImprimirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Esquema esq = new Esquema();
			esq.Dato = "Modo";
			//esq.ValorDefectoOriginal = Settings.Default.Imprimir;
			Prompt p = new Prompt(esq);
			if (p.ShowDialog() != DialogResult.OK)
				return;
			//Settings.Default.Imprimir = p.GetValue().ToString();
			//Settings.Default.Save();
		}

		private void modeloFiscalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Esquema esq = new Esquema();
			esq.Dato = "Modelo Impresora Fiscal";
			esq.Tipo = Esquema.enControles.Money;
			esq.Mascara = "000000000";
			esq.ValorDefectoOriginal = AppConfig.ModeloFiscal;
			Prompt pr = new Prompt(esq);
			if (pr.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				AppConfig.ModeloFiscal = int.Parse(pr.GetValue().ToString());
			}
			catch { }
		}

		#endregion

		#region Atajos de CVentas
		private void atajosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Atajos"))
				return;
			ABMGrilla abm = CrearABM(false, true) as ABMGrilla;
			abm.ActualizarTabla(Atajo.Esquema());
			abm.ActualizarTabla(Atajo.GetAll());
			abm.ActualizarTabla(Articulo.GetAll());
			ABMDatos abmData = abm.ObtenerAddOn<ABMDatos>();
			abmData.ABMIntentado += new ABMDatos.ABMIntentadoHandler(FrmMain_ABMIntentado);
			abmData.BusquedaIntentada += new ABMDatos.OperacionIntentadaHandler(ABM_EdicionIntentada);
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Atajo.NombreTabla;
			TabPage tp = CrearTabPage(abm, "Atajos", "Atajos");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		void CrearAtajos()
		{
			SortedList<string, Keys> teclas = new SortedList<string, Keys>(50);
			string aux;
			int err = 0;
			foreach (var k in Enum.GetValues(typeof(Keys)))
			{
				try
				{
					aux = k.ToString();
					teclas.Add(k.ToString(), (Keys)k);
				}
				catch
				{
					err++;
				}
			}

			DataTable dt = Atajo.GetAll();
			foreach (DataRow r in dt.Rows)
			{
				Atajo atajo = new Atajo(r);
				string texto;
				if (atajo.IdArticulo == 1)
				{
					texto = "Buscar...";
				}
				else
				{
					Articulo art = Articulo.GetSingleE(atajo.IdArticulo);
					texto = art.Codigo;
				}
				//texto += "(" + (atajo.Control ? "Ctrl + " : "") + (atajo.Shift ? "Shift + " : "") + (atajo.Alt ? "Alt + " : "") + atajo.Tecla + ")";
				Keys teclado = teclas[atajo.Tecla.ToUpper()];
				if (atajo.Control) teclado |= Keys.Control;
				if (atajo.Shift) teclado |= Keys.Shift;
				if (atajo.Alt) teclado |= Keys.Alt;
				//
				ToolStripMenuItem item = new ToolStripMenuItem(texto, null, atajoToolStripMenuItem_Click, teclado);
				item.Tag = atajo.IdArticulo;
				articuloToolStripMenuItem.DropDownItems.Add(item);
			}
		}

		private void atajoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			uint id = (uint)(sender as ToolStripMenuItem).Tag;
			if (id == 1)
				_ventas.BuscarItem();
			else
				_ventas.SetArticulo(id);
		}

		private void buscarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
			{
				//_ventas.BuscarCliente();
				Cliente c = Program.BuscarCliente();
				if (c != null)
					_ventas.SetCliente(c);
			}
			else if (TABs.SelectedTab.Tag is IABMTool)
			{
				IABMTool abm = TABs.SelectedTab.Tag as IABMTool;
				var idObj = abm.ItemActual != null ? abm.ItemActual[abm.TablaMainKey] : "";
				ABM_TablaInvalidada(abm, abm.TablaMain);
				uint id = 0;
				if (uint.TryParse(idObj.ToString(), out id))
					abm.SeleccionarItem(id);
			}
		}

		private void consumidorFinalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.SetCliente();
		}

		private void superToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.SetArticulo(2);
		}

		private void gasOilToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.SetArticulo(3);
		}

		private void gNCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.SetArticulo(4);
		}

		private void añadirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.AddItemActual();
		}

		private void oToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.BuscarItem();
		}

		private void pagoTarjetaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.SetPagoForma(Reglas.PagoTarjeta);
		}

		private void pagoCtaCteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.SetPagoForma(Reglas.PagoCC);
		}

		private void limpiarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.Limpiar();
		}

		private void emitirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.Emitir();
		}

		private void hacerNCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_ventas.Visible)
				_ventas.EmitirNC();
		}
		#endregion

		#region REPORTES
		private void ventasPorPeriodoMontoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DateTime tempo = DateTime.Now, fin = DateTime.Now;
			try
			{
				EsquemaCollection esquemas = new EsquemaCollection();
				Esquema esq = new Esquema();
				esq.Dato = "Desde";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				//
				esq = new Esquema();
				esq.Dato = "Hasta";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				//
				esq = new Esquema();
				esq.Dato = "zAbrir";
				esq.Alias = "Abrir";
				esq.Tipo = Esquema.enControles.CheckBox;
				esq.ValorDefectoOriginal = AppConfig.AbrirReportes;
				esquemas.Add(esq);
				//
				Prompt prm = new Prompt(esquemas, null);
				if (prm.ShowDialog() == DialogResult.Cancel)
					return;
				DateTime dde = DateTime.Parse(prm.GetValue("Desde").ToString());
				DateTime hta = DateTime.Parse(prm.GetValue("Hasta").ToString());
				Cursor = Cursors.WaitCursor;
				Reporte rte = new Reporte();
				rte.HacerReporteVentas(dde, hta);
				fin = DateTime.Now;
				Cursor = Cursors.Default;
				if (File.Exists(AppConfig.CarpetaReportes + "\\" + rte.Nombre + ".xls"))
				{
					if (MessageBox.Show("Ya existe un reporte \"" + rte.Nombre + "\"" + Environment.NewLine + "¿Desea reemplazarlo?", "Reporte existente", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.No)
						return;
				}
				rte.Grabar();
				if (prm.GetValue("Abrir").ToString().ToLower() == "true")
					System.Diagnostics.Process.Start(AppConfig.CarpetaReportes + "\\" + rte.Nombre + ".xls");
				else
					MessageBox.Show("Reporte creado exitosamente", "Reporte creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrio un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
				statMensaje.Text = "Reporte generado en " + ((fin - tempo).TotalMilliseconds / 1000.0).ToString() + " segundos";
			}
		}

		private void ventasPorPeriodoCtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DateTime tempo = DateTime.Now, fin = DateTime.Now;
			try
			{
				EsquemaCollection esquemas = new EsquemaCollection();
				Esquema esq = new Esquema();
				esq.Dato = "Desde";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				//
				esq = new Esquema();
				esq.Dato = "Hasta";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				//
				esq = new Esquema();
				esq.Dato = "zAbrir";
				esq.Alias = "Abrir";
				esq.Tipo = Esquema.enControles.CheckBox;
				esq.ValorDefectoOriginal = AppConfig.AbrirReportes;
				esquemas.Add(esq);
				//
				Prompt prm = new Prompt(esquemas, null);
				if (prm.ShowDialog() == DialogResult.Cancel)
					return;
				DateTime dde = DateTime.Parse(prm.GetValue("Desde").ToString());
				DateTime hta = DateTime.Parse(prm.GetValue("Hasta").ToString());
				Cursor = Cursors.WaitCursor;
				Reporte rte = new Reporte();
				rte.HacerReporteCombustibles(dde, hta);
				//rte.HacerReporteCombustiblesViejo(dde, hta, "Reporte");
				fin = DateTime.Now;
				Cursor = Cursors.Default;
				if (File.Exists(AppConfig.CarpetaReportes + "\\" + rte.Nombre + ".xls"))
				{
					if (MessageBox.Show("Ya existe un reporte \"" + rte.Nombre + "\"" + Environment.NewLine + "¿Desea reemplazarlo?", "Reporte existente", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.No)
						return;
				}
				rte.Grabar();
				if (prm.GetValue("Abrir").ToString().ToLower() == "true")
					System.Diagnostics.Process.Start(AppConfig.CarpetaReportes + "\\" + rte.Nombre + ".xls");
				else
					MessageBox.Show("Reporte creado exitosamente", "Reporte creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrio un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
				statMensaje.Text = "Reporte generado en " + ((fin - tempo).TotalMilliseconds / 1000.0).ToString() + " segundos";
			}
		}

		private void ventasPorCierresZToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Cierres Z"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			abm.ActualizarTabla(ZCierre.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(ZCierre.GetListaXFechas(DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));
			//abm.ActualizarTabla(FactEncabezado.GetSingle(1));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = ZCierre.NombreTabla;
			abm.AddAddOn(new ZCierresFiltro(ZCierresFiltro.enFiltro.ZCierres));
			TabPage tp = CrearTabPage(abm, "Cierres Z", "Cierres Z");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void ventasPorClienteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Ventas x Cliente"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			//abm.ActualizarTabla(Cliente.Esquema());
			abm.ActualizarTabla(Cliente.GetAll());
			abm.ActualizarTabla(ZCierre.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(ZCierre.GetListaXFechas(DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = ZCierre.NombreTabla;
			abm.AddAddOn(new ZCierresFiltro(ZCierresFiltro.enFiltro.Clientes));
			TabPage tp = CrearTabPage(abm, "Ventas x Cliente", "Ventas x Cliente");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void valesPorPeriodoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Vales"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			abm.ActualizarTabla(Operacion.Esquema());
			abm.ActualizarTabla(Articulo.GetAll());
			abm.ActualizarTabla(Vehiculo.GetAll());
			abm.ActualizarTabla(Operacion.GetForFechas(DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));
			//abm.ActualizarTabla(Operacion.GetForFechas(new DateTime(2016, 1, 1), DateTime.Now.Date.AddDays(1)));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Operacion.NombreTabla;
			abm.AddAddOn(new Exportador(true, true));
			abm.AddAddOn(new ZCierresFiltro(ZCierresFiltro.enFiltro.Operaciones));
			TabPage tp = CrearTabPage(abm, "Vales", "Vales");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void ventasGobiernoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DateTime tempo = DateTime.Now, fin = DateTime.Now;
			try
			{
				EsquemaCollection esquemas = new EsquemaCollection();
				Esquema esq = new Esquema();
				esq.Dato = "Desde";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				//
				esq = new Esquema();
				esq.Dato = "Hasta";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				//
				esq = new Esquema();
				esq.Dato = "zAbrir";
				esq.Alias = "Abrir";
				esq.Tipo = Esquema.enControles.CheckBox;
				esq.ValorDefectoOriginal = AppConfig.AbrirReportes;
				esquemas.Add(esq);
				//
				Prompt prm = new Prompt(esquemas, null);
				if (prm.ShowDialog() == DialogResult.Cancel)
					return;
				DateTime dde = DateTime.Parse(prm.GetValue("Desde").ToString());
				DateTime hta = DateTime.Parse(prm.GetValue("Hasta").ToString());
				Cursor = Cursors.WaitCursor;
				Reporte rte = new Reporte();
				rte.HacerReporteGobierno(dde, hta);
				fin = DateTime.Now;
				Cursor = Cursors.Default;
				if (File.Exists(AppConfig.CarpetaReportes + "\\" + rte.Nombre + ".xls"))
				{
					if (MessageBox.Show("Ya existe un reporte \"" + rte.Nombre + "\"" + Environment.NewLine + "¿Desea reemplazarlo?", "Reporte existente", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.No)
						return;
				}
				rte.Grabar();
				if (prm.GetValue("Abrir").ToString().ToLower() == "true")
					System.Diagnostics.Process.Start(AppConfig.CarpetaReportes + "\\" + rte.Nombre + ".xls");
				else
					MessageBox.Show("Reporte creado exitosamente", "Reporte creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrio un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
				statMensaje.Text = "Reporte generado en " + ((fin - tempo).TotalMilliseconds / 1000.0).ToString() + " segundos";
			}
		}

		private void libroIVAToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Libro IVA"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			abm.ActualizarTabla(ZCierre.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(ZCierre.GetListaXFechas(DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = ZCierre.NombreTabla;
			abm.AddAddOn(new ZCierresFiltro(ZCierresFiltro.enFiltro.LibroIVA));
			TabPage tp = CrearTabPage(abm, "Libro IVA", "Libro IVA");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void informesZToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Informes Z"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			abm.ActualizarTabla(ZCierre.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(ZCierre.GetListaXFechas(DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = ZCierre.NombreTabla;
			abm.AddAddOn(new ZCierresFiltro(ZCierresFiltro.enFiltro.InformesZ));
			TabPage tp = CrearTabPage(abm, "Informes Z", "Informes Z");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void turnosPorPeriodoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Turnos"))
				return;
			ABMGrilla abm = CrearABM(false, false) as ABMGrilla;
			abm.ActualizarTabla(Turno.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.ActualizarTabla(Turno.GetForFechas(DateTime.Now.Date, DateTime.Now.Date.AddDays(1)));
			//abm.ActualizarTabla(FactEncabezado.GetSingle(1));
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Turno.NombreTabla;
			abm.AddAddOn(new ZCierresFiltro(ZCierresFiltro.enFiltro.Turnos));
			TabPage tp = CrearTabPage(abm, "Turnos", "Turnos");
			abm.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				QuitarPagina(tp);
			};
			SeleccionarPagina(tp);
		}

		private void gráficoDeBarrasToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Gráficos de Barras"))
				return;
			CBarChart chart = new CBarChart();
			TabPage tp = CrearTabPage(chart, "Gráficos de Barras", "Gráficos de Barras");
			SeleccionarPagina(tp);
		}

		private void carpataDeReportesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog f = new FolderBrowserDialog();
			f.SelectedPath = AppConfig.CarpetaReportes;
			if (f.ShowDialog() != DialogResult.OK)
				return;
			AppConfig.CarpetaReportes = f.SelectedPath;
		}

		private void accessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AccessQuery().Show();
		}

		private void fixVehiculosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				ABMResultado abmRes = Vehiculo.FixVehiculos();
				if (abmRes.CodigoError == enErrores.Ninguno)
					MessageBox.Show("Se insertaron " + abmRes.FilasAfectadas.ToString() + " vehículos.", "Exito!", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
					MessageBox.Show(abmRes.MensajeError, "Error: " + abmRes.CodigoError.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al hacer Fix", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion REPORTES

		#region Menú Opciones
		private void envioDeEmailsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			Reglas.EnviarEmail = envioDeEmailsToolStripMenuItem.Checked;
		}

		private void confimarValesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			AppConfig.ConfirmarVales = confimarValesToolStripMenuItem.Checked;
		}

		private void petroResToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog f = new OpenFileDialog();
			string file = AppConfig.PetroRed;
			if (String.IsNullOrWhiteSpace(file))
			{
				f.InitialDirectory = "C:\\";
			}
			else
			{
				f.InitialDirectory = new FileInfo(file).Directory.FullName;
				f.FileName = file;
			}
			if (f.ShowDialog() == DialogResult.OK)
			{
				AppConfig.PetroRed = f.FileName;
				Reglas.Init(null);
			}
		}

		private void resolucionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog f = new OpenFileDialog();
			string file = Reglas.Resolucion;
			if (String.IsNullOrWhiteSpace(file))
			{
				f.InitialDirectory = "C:\\";
			}
			else
			{
				f.InitialDirectory = new FileInfo(file).Directory.FullName;
				f.FileName = file;
			}
			if (f.ShowDialog() == DialogResult.OK)
			{
				Reglas.Resolucion = f.FileName;
				Reglas.Init(null);
			}
		}

		private void clienteParaNCBToolStripMenuItem_Click(object sender, EventArgs e)
		{
			QFiltro filtroQ;
			ABMForm busqClientes = new ABMForm(true, false);//TH ajustado
			busqClientes.Tag = null;
			filtroQ = new QFiltro("Nombre");
			busqClientes.AddAddOn(filtroQ);
			filtroQ.SeleccionAceptada += delegate(object s2, EventArgs e2) { busqClientes.Tag = ""; busqClientes.Close(); };
			filtroQ.SeleccionCancelada += delegate(object s2, EventArgs e2) { busqClientes.Close(); };
			ABMFiltro filtro = busqClientes.ObtenerAddOn<ABMFiltro>();
			busqClientes.ActualizarTabla(Cliente.Esquema());
			busqClientes.TablaEsquema = "Esquemas";
			filtro.SeleccionAceptada += delegate(object s2, EventArgs e2) { busqClientes.Tag = ""; busqClientes.Close(); };
			filtro.SeleccionCancelada += delegate(object s2, EventArgs e2) { busqClientes.Close(); };
			filtroQ.TextoFiltro = "";
			filtroQ.PermitirABM = false;
			busqClientes.ActualizarTabla(Cliente.GetAll());
			if (busqClientes.TablaMain != Cliente.NombreTabla)
				busqClientes.TablaMain = Cliente.NombreTabla;
			busqClientes.Load += delegate(object s2, EventArgs e2)
			{
				new System.Threading.Thread(delegate()
				{
					System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
					busqClientes.Invoke((MethodInvoker)delegate
					{
						busqClientes.Icon = System.Drawing.Icon.FromHandle(Recursos.Cliente.GetHicon());
						busqClientes.Text = "Seleccione cliente para Notas de Crédito B";
						busqClientes.ReajustarGrilla();
						busqClientes.SeleccionarItem(Reglas.ClienteNCB);
						busqClientes.Refresh();
					});
				}).Start();
			};
			busqClientes.ShowDialog();
			//
			if (busqClientes.Tag == null)
				return;
			try
			{
				Cliente c = new Cliente(busqClientes.ObtenerSeleccion().Rows[0]);
				if (c.RespInscripto)
				{
					MessageBox.Show("El cliente no puede ser Responsable Inscripto", "Cliente Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else if (Fiscal.CUITLimpio(c.CUIT) == "")
				{
					MessageBox.Show("El cliente no puede tener un CUIT inválido", "Cliente Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					Reglas.ClienteNCB = c.Id;
					MessageBox.Show("Settings Actualizadas", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void clienteGobiernoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			QFiltro filtroQ;
			ABMForm busqClientes = new ABMForm(true, false);//TH ajustado
			busqClientes.Tag = null;
			filtroQ = new QFiltro("Nombre");
			busqClientes.AddAddOn(filtroQ);
			filtroQ.SeleccionAceptada += delegate(object s2, EventArgs e2) { busqClientes.Tag = ""; busqClientes.Close(); };
			filtroQ.SeleccionCancelada += delegate(object s2, EventArgs e2) { busqClientes.Close(); };
			ABMFiltro filtro = busqClientes.ObtenerAddOn<ABMFiltro>();
			busqClientes.ActualizarTabla(Cliente.Esquema());
			busqClientes.TablaEsquema = "Esquemas";
			filtro.SeleccionAceptada += delegate(object s2, EventArgs e2) { busqClientes.Tag = ""; busqClientes.Close(); };
			filtro.SeleccionCancelada += delegate(object s2, EventArgs e2) { busqClientes.Close(); };
			filtroQ.TextoFiltro = "";
			filtroQ.PermitirABM = false;
			busqClientes.ActualizarTabla(Cliente.GetAll());
			if (busqClientes.TablaMain != Cliente.NombreTabla)
				busqClientes.TablaMain = Cliente.NombreTabla;
			busqClientes.Load += delegate(object s2, EventArgs e2)
			{
				new System.Threading.Thread(delegate()
				{
					System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
					busqClientes.Invoke((MethodInvoker)delegate
					{
						busqClientes.Icon = System.Drawing.Icon.FromHandle(Recursos.Cliente.GetHicon());
						busqClientes.Text = "Seleccione cliente de Gobierno";
						busqClientes.ReajustarGrilla();
						busqClientes.SeleccionarItem(Reglas.ClienteGobierno);
						busqClientes.Refresh();
					});
				}).Start();
			};
			busqClientes.ShowDialog();
			//
			if (busqClientes.Tag == null)
				return;
			try
			{
				Cliente c = new Cliente(busqClientes.ObtenerSeleccion().Rows[0]);
				if (c.IdCliente == 1)
				{
					MessageBox.Show("El cliente no puede el consumidor final", "Cliente Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					Reglas.ClienteGobierno = c.Id;
					MessageBox.Show("Settings Actualizadas", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void enargasToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SeleccionarPagina("Enargas"))
				return;
			WebBrowser web = new WebBrowser();
			web.NewWindow += delegate(object sender2, CancelEventArgs e2) { e2.Cancel = true; };
			web.AllowWebBrowserDrop = false;
			web.IsWebBrowserContextMenuEnabled = false;
			web.WebBrowserShortcutsEnabled = false;
			web.Url = new Uri("http://www.enargas.gov.ar");
			TabPage tp = CrearTabPage(web, "Enargas", "Enargas");
			SeleccionarPagina(tp);
		}

		private void fechaFiscalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show(Reglas.Now.ToString());
		}
		#endregion

		private void bImprimirRpt1_Click(object sender, EventArgs e)
		{
			_cierres.ImprimirPlanilla(Reglas.TurnoUltimo, 1);
			DateTime periodoDde = Reglas.TurnoUltimo.FechaFinal.AddSeconds(-1);
			DateTime periodoHta = Reglas.TurnoUltimo.FechaFinal.AddSeconds(1);
			RptCierre rptCierre = new RptCierre(periodoDde, periodoHta);
			rptCierre.Imprimir();
		}

		private void bImprimirRpt2_Click(object sender, EventArgs e)
		{
			_cierres.ImprimirPlanilla(Reglas.TurnoUltimo, 2);
		}
	}
}