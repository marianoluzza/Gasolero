using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MarUtils.Controles;
using Negocio;
using MarUtils.Soporte;
using System.Reflection;
using FormCom;
using MarUtils.Entidades;

namespace Gasolero
{
	public partial class CVentas : UserControl
	{
		MoneyTextBox MoneyCantidad = new MoneyTextBox(8, 3, " ", ",");
		MoneyTextBox MoneyMonto = new MoneyTextBox(8, 3, "$", ",");
		MoneyTextBox MoneyPrecioUni = new MoneyTextBox(8, 3, "$", ",");
		MoneyTextBox MoneyTotal = new MoneyTextBox(8, 3, "$", ",");
		FactEncabezado _factura = new FactEncabezado();
		DataTable dtItems = Articulo.GetAll();
		DataTable dtPagoFormas = PagoForma.GetHabilitados();
		DataTable dtEsq = FactItem.Esquema();
		DataSet _info = new DataSet();

		Cliente _cliente;
		Articulo _itemActual = new Articulo();
		ABMForm _busqClientes;

		/// <summary>
		/// Control de solo lectura?
		/// </summary>
		public bool SoloLectura
		{
			get { return _factura.IdFactEncabezado > 0; }
		}

		public CVentas()
		{
			InitializeComponent();
			_info.Tables.Add(dtItems);
			_info.Tables.Add(dtPagoFormas);
			//Cantidad
			MoneyCantidad.Location = txtCantidad.Location;
			MoneyCantidad.Size = txtCantidad.Size;
			MoneyCantidad.TabIndex = txtCantidad.TabIndex;
			MoneyCantidad.ValorCambiado += new EventHandler(MoneyCantidad_ValorCambiado);
			MoneyCantidad.KeyDown += new KeyEventHandler(MoneyCtMonto_KeyDown);
			txtCantidad.Parent.Controls.Add(MoneyCantidad);
			//Monto
			MoneyMonto.Location = txtMonto.Location;
			MoneyMonto.Size = txtMonto.Size;
			MoneyMonto.TabIndex = txtMonto.TabIndex;
			MoneyMonto.ValorCambiado += new EventHandler(MoneyMonto_ValorCambiado);
			MoneyMonto.KeyDown += new KeyEventHandler(MoneyCtMonto_KeyDown);
			txtMonto.Parent.Controls.Add(MoneyMonto);
			//Precio x u
			MoneyPrecioUni.Location = txtPrecioUni.Location;
			MoneyPrecioUni.Size = txtPrecioUni.Size;
			MoneyPrecioUni.TabIndex = txtPrecioUni.TabIndex;
			MoneyPrecioUni.TabStop = txtTotal.TabStop;
			MoneyPrecioUni.ReadOnly = true;
			txtPrecioUni.Parent.Controls.Add(MoneyPrecioUni);
			//Total
			MoneyTotal.Location = txtTotal.Location;
			MoneyTotal.Size = txtTotal.Size;
			MoneyTotal.TabIndex = txtTotal.TabIndex;
			MoneyTotal.ReadOnly = true;
			txtTotal.Parent.Controls.Add(MoneyTotal);
			//
			Grilla.AutoGenerateColumns = false;
			BindSrc.DataSource = _factura.Cuerpo;
			Grilla.DataSource = BindSrc;
			//
			CBArticulos.DisplayMember = "Codigo";
			CBArticulos.ValueMember = dtItems.PrimaryKey[0].ColumnName;
			CBArticulos.DataSource = new DataView(dtItems);
			//
			cbPagoForma.DisplayMember = "Descripcion";
			cbPagoForma.ValueMember = dtPagoFormas.PrimaryKey[0].ColumnName;
			cbPagoForma.DataSource = new DataView(dtPagoFormas);
			//
			BConsFinal_Click(BConsFinal, EventArgs.Empty);
			//Limpiar();
			CrearAtajos();
		}

		public CVentas(FactEncabezado fact)
			: this()
		{
			//GBCliente.Enabled = false;
			BConsFinal.Enabled = false;
			BClienteBuscar.Enabled = false;
			GBItem.Enabled = false;
			//BotoneraInf.Enabled = false;
			if (fact.Anula > 0 && fact.Total < 0
				|| fact.Anula == 0)
			{
				BEmitir.Text = "Anular";
				BEmitir.Click -= new EventHandler(BEmitir_Click);
				BEmitir.Click += new EventHandler(BAnular_Click);
			}
			else
			{
				BEmitir.Text = "Ya Anulada";
				BEmitir.Enabled = false;
			}
			BLimpiar.Enabled = true;
			BLimpiar.Text = "Eliminar";
			BLimpiar.Tag = "DEL";
			BLimpiar.Visible = false;//no mostrar más el botón de eliminar factura
			//
			cbPagoForma.SelectedValue = fact.IdPagoForma > 0 ? fact.IdPagoForma : 1;
			cbPagoForma.Enabled = false;
			//
			BotoneraAtajos.Enabled = false;
			//
			Cliente c = Cliente.GetSingleE(fact.IdCliente);
			uint idC = c.IdCliente;
			c.IdCliente = 100;
			SetCliente(c);
			c.IdCliente = idC;
			//
			lblTotal.Text = "Total Pagado";
			txtCCDesc.ReadOnly = true;
			if (fact.IdPagoForma == Reglas.PagoCC)
			{
				CCMovimiento ccMov = CCMovimiento.GetSingleForFactura(fact.IdFactEncabezado);
				txtCCDesc.Text = ccMov.Descripcion;
				tbCCAutorizadoDNI.Text = ccMov.DNIAutorizado;
				tbCCAutorizadoPatente.Text = ccMov.PatenteAutorizado;
			}
			_factura = fact;
			BindSrc.DataSource = _factura.Cuerpo;
		}

		public void ActualizarTabla(DataTable dt)
		{
			if (_info.Tables.Contains(dt.TableName))
			{
				_info.Tables[dt.TableName].Rows.Clear();
				_info.Tables[dt.TableName].AcceptChanges();
				_info.Tables[dt.TableName].Merge(dt);
				_info.Tables[dt.TableName].AcceptChanges();
			}
			if (_busqClientes != null && _busqClientes.ExisteTabla(dt.TableName))
				_busqClientes.ActualizarTabla(dt);
			if (dt.TableName.ToLower() == Articulo.NombreTabla.ToLower())
			{
				if (ArticuloSeleccionado != null)
					CBArticulos_SelectedValueChanged(CBArticulos, EventArgs.Empty);
			}
		}

		void MoneyMonto_ValorCambiado(object sender, EventArgs e)
		{
			if (ArticuloSeleccionado == null)
				return;
			if (ArticuloSeleccionado.Precio > 0)
				MoneyCantidad.Valor = MoneyMonto.Valor / ArticuloSeleccionado.Precio;
			if (MoneyMonto.Focused)
				MoneyMonto.SelectAll();
		}

		void MoneyCantidad_ValorCambiado(object sender, EventArgs e)
		{
			if (ArticuloSeleccionado == null)
				return;
			if (ArticuloSeleccionado.Precio > 0)
				MoneyMonto.Valor = MoneyCantidad.Valor * ArticuloSeleccionado.Precio;
			if (MoneyCantidad.Focused)
				MoneyCantidad.SelectAll();
		}

		void MoneyCtMonto_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				AddItemActual();
				BEmitir.Focus();
			}
		}

		private void BAdd_Click(object sender, EventArgs e)
		{
			AddItemActual();
		}

		public Articulo ArticuloSeleccionado
		{
			get
			{
				return _itemActual;
			}
			set
			{
				_itemActual = value;
				if (value != null)
					CBArticulos.SelectedValue = value.IdArticulo;
			}
		}

		private void BindSrc_DataSourceChanged(object sender, EventArgs e)
		{
			if (BindSrc.DataSource == null)
				return;
			EsquemaCollection esquemas = Esquema.GetListaEsquema(dtEsq);
			Grilla.Columns.Clear();
			//
			//lista de columnas para ordenar por posicion
			SortedList<int, DataGridViewColumn> columnas = new SortedList<int, DataGridViewColumn>(esquemas.Count);
			foreach (PropertyDescriptor prop in BindSrc.GetItemProperties(null))
			{
				Esquema esq;
				if (!esquemas.Contains(prop.Name))
					continue;
				esq = esquemas[prop.Name];
				DataGridViewColumn col;
				switch (esq.Tipo)
				{
					case Esquema.enControles.CheckBox:
						col = new DataGridViewCheckBoxColumn();
						break;
					case Esquema.enControles.ComboBox:
						DataGridViewComboBoxColumn cbCol = new DataGridViewComboBoxColumn();
						cbCol.DisplayMember = esq.TablaDisplay;
						cbCol.ValueMember = esq.TablaId;
						cbCol.DataSource = new DataView(_info.Tables[esq.Tabla]);
						cbCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
						col = cbCol;
						break;
					case Esquema.enControles.Money:
						col = new DataGridViewTextBoxColumn();
						col.CellTemplate.ValueType = typeof(decimal);
						col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
						string formato;
						int indicePto = esq.Mascara.IndexOf('.');
						if (indicePto >= 0)
						{
							formato = esq.Mascara.Substring(0, indicePto).Replace('9', '#');
							if (indicePto + 1 < esq.Mascara.Length)
								formato += ".";
							for (int i = indicePto + 1; i < esq.Mascara.Length; i++)
								formato += "0";
						}
						else
							formato = esq.Mascara;
						if (formato.StartsWith("%"))
							formato = "\\" + formato;
						col.DefaultCellStyle.Format = formato;
						break;
					default:
						col = new DataGridViewTextBoxColumn();
						break;
				}
				col.DataPropertyName = prop.Name;
				col.HeaderText = esq.Alias != "" ? esq.Alias : esq.Dato;
				col.Name = col.DataPropertyName;
				col.ToolTipText = esq.Comentario;
				col.Visible = esq.Visible;
				columnas.Add(esq.Posicion, col);
			}
			foreach (DataGridViewColumn col in columnas.Values)
			{
				Grilla.Columns.Add(col);
			}
		}

		private void BindSrc_AddingNew(object sender, AddingNewEventArgs e)
		{
			FactItem factItem = new FactItem();
			//factItem.Coleccion = _factura.Cuerpo;
			factItem.IdArticulo = 1;
			e.NewObject = factItem;
		}

		private void Grilla_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{

		}

		private void CBArticulos_SelectedValueChanged(object sender, EventArgs e)
		{
			MoneyPrecioUni.Valor = 0;
			if (CBArticulos.SelectedValue == null)
				return;
			ArticuloSeleccionado = new Articulo(dtItems.Rows[CBArticulos.SelectedIndex]);
			MoneyPrecioUni.Valor = ArticuloSeleccionado.Precio;
			MoneyCantidad.Valor = 0;
			MoneyMonto.Valor = 0;
		}

		private void BEmitir_Click(object sender, EventArgs e)
		{
			Emitir();
		}

		private void BAnular_Click(object sender, EventArgs e)
		{
			Anular();
		}

		private void BLimpiar_Click(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;
			if ((string)ctrl.Tag == "DEL")
				Eliminar();
			else
				Limpiar();
		}

		private void BindSrc_ListChanged(object sender, ListChangedEventArgs e)
		{
			decimal total = 0;
			foreach (FactItem it in _factura.Cuerpo)
				total += it.Monto;
			decimal dto = 0;
			if (_cliente != null)
				dto = _cliente.Descuento;
			if (SoloLectura)//no aplicar el descuento porque ya se hizo, se está mostrando nada más
			{
				decimal totalPagado = total / (100 - dto) * 100;
				txtDto.Text = (totalPagado - total).ToString("0.000 $");
				MoneyTotal.Valor = total;
			}
			else
			{
				txtDto.Text = (total * dto / 100).ToString("0.000 $");
				MoneyTotal.Valor = total - (total * dto / 100);
			}
		}

		private void BClienteBuscar_Click(object sender, EventArgs e)
		{
			BuscarCliente();
		}

		public void BuscarCliente()
		{
			QFiltro filtroQ;
			if (_busqClientes == null)
			{
				_busqClientes = new ABMForm(true, true);//TH Ajustado
				filtroQ = new QFiltro("Nombre");
				_busqClientes.AddAddOn(filtroQ);
				filtroQ.SeleccionAceptada += new EventHandler(Filtro_SeleccionAceptada);
				filtroQ.SeleccionCancelada += new EventHandler(Filtro_SeleccionCancelada);
				ABMFiltro filtro = _busqClientes.ObtenerAddOn<ABMFiltro>();
				_busqClientes.ActualizarTabla(Cliente.Esquema());
				_busqClientes.TablaEsquema = "Esquemas";
				ABMDatos datos = _busqClientes.ObtenerAddOn<ABMDatos>();
				datos.ABMIntentado += new ABMDatos.ABMIntentadoHandler(ABMClientes_ABMIntentado);
				datos.BusquedaIntentada += new ABMDatos.OperacionIntentadaHandler(ABMClientes_BusquedaIntentada);
				filtro.SeleccionAceptada += new EventHandler(Filtro_SeleccionAceptada);
				filtro.SeleccionCancelada += new EventHandler(Filtro_SeleccionCancelada);
				_busqClientes.Load += new EventHandler(delegate
				{
					new System.Threading.Thread(delegate()
					{
						System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
						_busqClientes.Invoke((MethodInvoker)delegate
						{
							_busqClientes.Icon = System.Drawing.Icon.FromHandle(Recursos.Cliente.GetHicon());
							_busqClientes.Text = "Buscar Cliente";
							_busqClientes.ReajustarGrilla();
							_busqClientes.Refresh();
						});
					}).Start();
				});
			}
			else
			{
				filtroQ = _busqClientes.ObtenerAddOn<QFiltro>();
			}
			filtroQ.TextoFiltro = "";
			filtroQ.PermitirABM = true;
			_busqClientes.ActualizarTabla(Cliente.GetAll());
			if (_busqClientes.TablaMain != Cliente.NombreTabla)
				_busqClientes.TablaMain = Cliente.NombreTabla;
			_busqClientes.ShowDialog();
			if (!_busqClientes.EstaActivoAddon(filtroQ.GetType()))
				_busqClientes.ActivarAddOn(filtroQ.GetType());
		}

		void Filtro_SeleccionCancelada(object sender, EventArgs e)
		{
			if (sender is ABMForm)
				(sender as ABMForm).Close();
			else
			{
				IABMAddOn filtro = sender as IABMAddOn;
				ABMForm abm = filtro.ABMTool as ABMForm;
				abm.Close();
			}
		}

		void Filtro_SeleccionAceptada(object sender, EventArgs e)
		{
			IABMAddOn filtro = sender as IABMAddOn;
			ABMForm abm = filtro.ABMTool as ABMForm;
			SetCliente(new Cliente(abm.ItemActual.Row));
			abm.Close();
		}

		void ABMClientes_ABMIntentado(object sender, ABMEventeArgs e)
		{
			ABMDatos abmData = sender as ABMDatos;
			IABMTool abm = abmData.ABMTool;
			EntidadBase ent = Reglas.New(abm.TablaMain, e.Valores);
			ABMResultado abmRes;
			if (e.Funcion == enABMFuncion.Alta)
			{
				abmRes = ent.Alta();
			}
			else if (e.Funcion == enABMFuncion.Modificacion)
			{
				abmRes = ent.Modificacion();
			}
			else
			{
				abmRes = ent.Baja();
				//MessageBox.Show("No se puede eliminar clientes desde aquí, hagalo desde la vista de clientes gral.", "Operación Inválida", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				//return;
			}
			if (abmRes.CodigoError != enErrores.Ninguno)
			{
				MessageBox.Show(abmRes.MensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			DataTable dtAll;
			dtAll = ent.GetType().GetMethod("GetAll").Invoke(ent, null) as DataTable;
			uint ultid = 0;
			if (e.Funcion != enABMFuncion.Baja)
			{
				ultid = e.Funcion == enABMFuncion.Alta ? abmRes.IdInsercion :
					uint.Parse(abm.ItemActual.Row[dtAll.PrimaryKey[0].ColumnName].ToString());
			}
			abm.ActualizarTabla(dtAll);
			FrmMain.ActualizarABMs(dtAll);
			abm.SeleccionarItem(ultid);
			Filtro_SeleccionAceptada(abmData, e);
		}

		void ABMClientes_BusquedaIntentada(object sender, Esquema dato)
		{
			ABMDatos abmData = sender as ABMDatos;
			IABMTool abm = abmData.ABMTool;
			if (dato.Dato.ToUpper() == "CUIT")
			{
				string cuit = abmData.GetValor(dato.Dato).ToString().Replace("-", "").Trim();
				string msn = Fiscal.CUITCheck(cuit) ?
					"CUIT correcto" : "CUIT incorrecto";
				MessageBox.Show(msn, "CUIT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void BConsFinal_Click(object sender, EventArgs e)
		{
			SetCliente();
		}

		public void SetArticulo(uint id)
		{
			CBArticulos.SelectedValue = id;
			//MoneyCantidad.Focus();
			MoneyMonto.Focus();
			MoneyMonto.SelectAll();
			MoneyMonto_ValorCambiado(MoneyMonto, EventArgs.Empty);
		}

		public void SetCliente(Cliente c)
		{
			TBClienteNbre.Text = c.Nombre;
			TBCUIT.Text = c.CUIT;
			TBDireccion.Text = c.Direccion;
			TBTel.Text = c.Telefono;
			_cliente = c;
			TBClienteNbre.ReadOnly = c.Id > 1;
			TBCUIT.ReadOnly = c.Id > 1;
			TBDireccion.ReadOnly = c.Id > 1;
			TBTel.ReadOnly = c.Id > 1;
			lblDto.Text = c.Descuento > 0 ? "-" + c.Descuento.ToString("0.###") + "%" : "";
			BindSrc_ListChanged(BindSrc, null);
			//Reglas.AddLog("Cliente seteado: " + _cliente.Id.ToString() + " " + _cliente.Nombre + (_cliente.RespInscripto ? " Rescp" : " NO Resp"));
			if (c.EsConsFinal)
				TBClienteNbre.Focus();
		}

		public void SetCliente()
		{
			SetCliente(Cliente.ConsFinal);
		}

		public void SetPagoForma(uint id)
		{
			cbPagoForma.SelectedValue = id;
		}

		public void AddItemActual()
		{
			if (CBArticulos.SelectedIndex < 0 || ArticuloSeleccionado == null)
				return;
			BAdd.Focus();
			if (ArticuloSeleccionado.VentaMaxima > 0 &&
				MoneyCantidad.Valor > ArticuloSeleccionado.VentaMaxima)
			{
				MessageBox.Show("No se puede vender sobre el límite." + Environment.NewLine + "Limite actual: " + ArticuloSeleccionado.VentaMaxima.ToString("0.00 " + ArticuloSeleccionado.Medida),
					"Límite de venta excedido", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				MoneyMonto.Focus();
				return;
			}
			if (MoneyCantidad.Valor <= 0 || MoneyMonto.Valor <= 0)
			{
				MessageBox.Show("No se puede vender con valor o cantidad cero.",
						"Valores inválidos", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				if (MoneyCantidad.Valor <= 0)
					MoneyCantidad.Focus();
				else
					MoneyMonto.Focus();
				return;
			}
			FactItem item = new FactItem();
			item.IdArticulo = ArticuloSeleccionado.IdArticulo;
			item.Cantidad = MoneyCantidad.Valor;
			item.Monto = MoneyMonto.Valor;
			item.Precio = ArticuloSeleccionado.Precio != 0 ? ArticuloSeleccionado.GetPrecioBase() :
				decimal.Round(item.Monto / item.Cantidad, 2);
			item.IVA = item.Precio / 100 * ArticuloSeleccionado.Alicuota;
			item.Impuestos = ArticuloSeleccionado.Precio <= 0 ? 0 :
				(ArticuloSeleccionado.Precio - item.Precio - item.IVA);
			string desc = ArticuloSeleccionado.Descripcion;
			if (ArticuloSeleccionado.IdArticulo == 1)
			{
				Esquema esq = new Esquema();
				esq.Dato = "Descripción";
				esq.ValorDefectoOriginal = desc;
				Prompt pr = new Prompt(esq);
				pr.Text = "Descripción para el item";
				if (pr.ShowDialog() == DialogResult.OK)
					desc = pr.GetValue().ToString();
			}
			item.DescPlana = desc;
			BindSrc.Add(item);
		}

		public void Emitir()
		{
			if (!_habilitado)
				return;
			ABMResultado abmRes = null;
			decimal dtoK = 1 - _cliente.Descuento / 100;//coeficiente de descuento
			try
			{
				if (Grilla.Rows.Count == 0)
				{
					MessageBox.Show("No hay articulos en la venta", "Error al emitir", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				Descontar(true, dtoK);
				//check cc cliente
				#region Chequeo CC Cliente
				if ((uint)cbPagoForma.SelectedValue == Reglas.PagoCC)//el pago es a cc
				{
					if (_cliente.EsConsFinal)
					{
						MessageBox.Show("No se puede pagar con Cuenta Corriente para Consumidor Final", "Forma de Pago Inválida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					decimal saldoCC = CCMovimiento.GetSaldoForCliente(_cliente.IdCliente, Reglas.Now.AddMinutes(60));
					if (_factura.Total > saldoCC + _cliente.SaldoMaximo)//no le alcanza el saldo
					{
						MessageBox.Show("El cliente no saldo posee disponible en Cuentas Corrientes" + Environment.NewLine +
								"Saldo actual: " + saldoCC.ToString("$ 0.00") + Environment.NewLine +
								"Deuda Máxima: " + _cliente.SaldoMaximo.ToString("$ 0.00") + Environment.NewLine +
								"Esta venta: " + _factura.Total.ToString("$ 0.00"), "Saldo Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					else if (saldoCC > _cliente.SaldoAdvertencia && saldoCC - _factura.Total <= _cliente.SaldoAdvertencia)
					{
						MessageBox.Show("El cliente va a pasar el saldo advertio" + Environment.NewLine +
									"Saldo actual: " + saldoCC.ToString("$ 0.00") + Environment.NewLine +
									"Saldo a advertir: " + _cliente.SaldoAdvertencia.ToString("$ 0.00") + Environment.NewLine +
									"Esta venta: " + _factura.Total.ToString("$ 0.00") + Environment.NewLine +
									"Futuro saldo: " + (saldoCC - _factura.Total).ToString("$ 0.00"), "Advertencia de Saldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					//fin control de saldo
					//control de autorizado
					if (!CCAutorizado.EsAutorizado(_cliente.Id, tbCCAutorizadoPatente.Text, tbCCAutorizadoDNI.Text))
					{
						MessageBox.Show("Persona NO autorizada!", "Autorización", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
				#endregion
				Habilitar(false);
				if (_cliente.EsConsFinal)
				{
					_cliente.Nombre = TBClienteNbre.Text;
					_cliente.Direccion = TBDireccion.Text;
					_cliente.Telefono = TBTel.Text;
					_cliente.RespInscripto = false;
					_cliente.CUIT = TBCUIT.Text;
					Cliente cCuit = Cliente.GetSingleE(_cliente.CUIT);
					#region Cliente Asociado
					if (cCuit != null && _cliente.CUIT.Length > 0 &&
						char.IsDigit(_cliente.CUIT[0]))
					{
						string msn = "Ya existe un cliente con este cuit:" + Environment.NewLine +
							"Nombre: " + cCuit.Nombre + Environment.NewLine +
							"Direccion: " + cCuit.Direccion + Environment.NewLine +
							"Telefono: " + cCuit.Telefono + Environment.NewLine +
							"CUIT: " + cCuit.CUIT + Environment.NewLine +
							"Resp. Inscripto: " + (cCuit.RespInscripto ? "SI" : "NO") + Environment.NewLine +
							"¿Desea utilizarlo para la venta?" + Environment.NewLine + Environment.NewLine +
							"[Si] = Utiliza el cliente ya guardado" + Environment.NewLine +
							"[No] = Utiliza los datos actuales y actualiza el cliente ya guardado" + Environment.NewLine +
							"[Cancelar] = Cancela operación";
						switch (MessageBox.Show(msn, "Cliente existente", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
						{
							case DialogResult.Cancel:
								return;
							case DialogResult.Yes:
								_cliente = cCuit;
								break;
							case DialogResult.No:
								cCuit.Nombre = _cliente.Nombre;
								cCuit.Direccion = _cliente.Direccion;
								cCuit.Telefono = _cliente.Telefono;
								cCuit.RespInscripto = _cliente.RespInscripto;
								ABMResultado abmResMod = cCuit.Modificacion();
								if (abmResMod.CodigoError != enErrores.Ninguno)
								{
									MessageBox.Show(abmResMod.MensajeError, "Error al modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
									return;
								}
								_cliente = cCuit;
								break;
						}
					}
					else if (_cliente.CUIT.Length > 0 && char.IsDigit(_cliente.CUIT[0]) && cCuit == null)
					{//cuit valido pero sin par en la BD, ingresar
						if (MessageBox.Show("El cliente no está en la base de datos" + Environment.NewLine +
							"¿Desea ingresarlo?", "Cliente inexistente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							ABMResultado abmResAlta = _cliente.Alta();
							if (abmResAlta.CodigoError != enErrores.Ninguno)
							{
								MessageBox.Show(abmResAlta.MensajeError, "Error al ingresar cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}
					}
					#endregion
				}
				_factura.IdCliente = _cliente.Id;
				_factura.IdPagoForma = (uint)cbPagoForma.SelectedValue;
				_factura.CCDescripcion = txtCCDesc.Text;
				//
				if (AppConfig.CierreZ1raVenta == AppConfig.enCierreZ1raVenta.Siempre ||
					(AppConfig.CierreZ1raVenta == AppConfig.enCierreZ1raVenta.SoloDia1 && Reglas.Now.Day == 1))
				{
					if (!ZCierre.HayCierres(Reglas.Now))
					{
						Fiscal.CierreZ();
						System.Threading.Thread.Sleep(1000);
					}
				}
				if (!_factura.IniciarAlta())
					throw new Exception("Transacción ya iniciada");
				abmRes = _factura.Alta();
				if (abmRes.CodigoError != enErrores.Ninguno)
				{
					_factura.ConfirmarAlta(false);
					MessageBox.Show(abmRes.MensajeError, "Error al emitir", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else//EMISION SIN ERRORES
				{
					CCMovimiento ccMov = _factura.IdPagoForma == Reglas.PagoCC ? CCMovimiento.GetSingleForFactura(_factura.IdFactEncabezado) : null;
					if (ccMov != null)
					{//actualizar el mov con los datos extra
						ccMov.PatenteAutorizado = tbCCAutorizadoPatente.Text;
						ccMov.DNIAutorizado = tbCCAutorizadoDNI.Text;
						ccMov.NombreAutorizado = CCAutorizado.GetNombre(_cliente.Id, tbCCAutorizadoPatente.Text, tbCCAutorizadoDNI.Text);
						abmRes = ccMov.Modificacion();
						if (abmRes.CodigoError != enErrores.Ninguno)
							MessageBox.Show(abmRes.MensajeError, "Error al actualizar movimiento de CC", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					if (AppConfig.UsarFiscal)
					{
						abmRes = Fiscal.Imprimir(_cliente, _factura);
						if (abmRes.CodigoError != enErrores.Ninguno)
							MessageBox.Show(abmRes.MensajeError, "Error al emitir fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
						else
						{
							Emailer.Informar(ccMov);
							//if (emailErr != "")
							//    MessageBox.Show(emailErr, "Error al Notificar Movimiento via Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
							bool imprimioCompCC = true;
							if (ccMov != null)
							{
								int pausa = Negocio.Reglas.PausaEntreFiscal;
								if (pausa > 0)
									System.Threading.Thread.Sleep(pausa);
								imprimioCompCC = Fiscal.ImprimirCCMov(ccMov);
							}
							if (!imprimioCompCC)//el chequeo de error nuevamente es por el fiscal reciente ^
								MessageBox.Show("No se pudo imprimir el comprobante de cuenta corriente", "Error al emitir comprobante de CC", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
					else//SIN FISCAL
					{
						abmRes = _factura.ConfirmarAlta(true);//en modo fiscal confirma el fiscal
						if (abmRes.CodigoError != enErrores.Ninguno)
						{
							MessageBox.Show(abmRes.MensajeError, "Error al finalizar transacción", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							Emailer.Informar(ccMov);
						}
					}
				}//FIN DE EMISION SIN ERRORES
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Habilitar(true);
				Descontar(false, dtoK);
				if (abmRes != null && abmRes.CodigoError == enErrores.Ninguno)
					Limpiar();
			}
		}

		bool _habilitado = true;
		private void Habilitar(bool p)
		{
			BEmitir.Enabled = p;
			BLimpiar.Enabled = p;
			_habilitado = p;
		}

		void Descontar(bool descontar, decimal dtoK)
		{
			//Poner o quitar los descuentos
			for (int i = 0; i < _factura.Cuerpo.Count; i++)
			{
				_factura.Cuerpo[i].Descuento = _cliente.Descuento;
				_factura.Cuerpo[i].Precio *= descontar ? dtoK : (1 / dtoK);
				_factura.Cuerpo[i].Impuestos *= descontar ? dtoK : (1 / dtoK);
				_factura.Cuerpo[i].IVA *= descontar ? dtoK : (1 / dtoK);
				_factura.Cuerpo[i].Monto *= descontar ? dtoK : (1 / dtoK);
			}
		}

		public void Anular()
		{
			#region Check
			if (Reglas.VendedorActual.EsRol(enPermisos.Vendedor) <= 0)
			{
				MessageBox.Show("No tiene permiso suficiente para anular el comprobante");
				return;
			}
			if (_factura.Anula > 0)
			{
				MessageBox.Show("El comprobante ya está anulado");
				return;
			}
			else if (MessageBox.Show("¿Está seguro que desea anular la factura " + _factura.Letra +
				" " + _factura.Numero + "?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.No)
				return;
			#endregion
			FactEncabezado nc;
			ABMResultado abmRes = _factura.Anular(out nc);
			if (abmRes.CodigoError != enErrores.Ninguno)
				MessageBox.Show(abmRes.MensajeError, "Error al anular", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
			{
				abmRes = Fiscal.Imprimir(Cliente.GetSingleE(nc.IdCliente), nc);
				if (abmRes.CodigoError == enErrores.Ninguno)
				{
					BEmitir.Text = "Ya Anulada";
					BEmitir.Enabled = false;
				}
			}
		}

		public void Eliminar()
		{
			if (Reglas.VendedorActual.EsRol(enPermisos.SuperUsuario) < 0)
			{
				MessageBox.Show("No tiene permiso suficiente para anular el comprobante");
				return;
			}
			if (MessageBox.Show("¿Está seguro que desea eliminar definitivamente la factura " + _factura.Letra +
				" " + _factura.Numero + "?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.No)
				return;
			ABMResultado abmRes = _factura.Baja();
			if (abmRes.CodigoError != enErrores.Ninguno)
				MessageBox.Show(abmRes.MensajeError, "Error al anular", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
			{
				FrmMain.FMain.QuitarPagina();
			}
		}

		public void Limpiar()
		{
			BindSrc.Clear();
			SetCliente();
			CBArticulos.SelectedValue = 0;
			ArticuloSeleccionado = null;
			CBArticulos.Text = "Elija un item";
			cbPagoForma.SelectedIndex = 0;
			MoneyCantidad.Valor = 0;
			MoneyMonto.Valor = 0;
			txtCCDesc.Text = "";
			foreach (var ctrl in BotoneraAtajos.Controls)
			{
				if (ctrl is LinkLabel)
				{
					foreach (LinkLabel.Link l in (ctrl as LinkLabel).Links)
						l.Visited = false;
				}
			}
			_factura = new FactEncabezado();
			BindSrc.DataSource = _factura.Cuerpo;
		}

		public void EmitirNC()
		{
			if (!_habilitado)
				return;
			ABMResultado abmRes = null;
			#region Establecer Descuento
			decimal dtoK = 1 - _cliente.Descuento / 100;//coeficiente de descuento
			//lo ponemos en el Habilitar() para que lo ponga al emitir pero lo quite al salir por error
			//si no, se ponía el dto cada vez que entraba en este método, y no lo sacaba si había error al emitir
			#endregion
			try
			{
				Cursor = Cursors.WaitCursor;
				Descontar(true, dtoK);
				Habilitar(false);
				if (_cliente.EsConsFinal)
				{
					_cliente.Nombre = TBClienteNbre.Text;
					_cliente.Direccion = TBDireccion.Text;
					_cliente.Telefono = TBTel.Text;
					_cliente.RespInscripto = false;
					_cliente.CUIT = TBCUIT.Text;
					Cliente cCuit = Cliente.GetSingleE(_cliente.CUIT);
					#region Ver Cliente Asociado
					if (cCuit != null && _cliente.CUIT.Length > 0 &&
						char.IsDigit(_cliente.CUIT[0]))
					{
						string msn = "Ya existe un cliente con este cuit:" + Environment.NewLine +
							"Nombre: " + cCuit.Nombre + Environment.NewLine +
							"Direccion: " + cCuit.Direccion + Environment.NewLine +
							"Telefono: " + cCuit.Telefono + Environment.NewLine +
							"CUIT: " + cCuit.CUIT + Environment.NewLine +
							"Resp. Inscripto: " + (cCuit.RespInscripto ? "SI" : "NO") + Environment.NewLine +
							"¿Desea utilizarlo para la venta?" + Environment.NewLine + Environment.NewLine +
							"[Si] = Utiliza el cliente ya guardado" + Environment.NewLine +
							"[No] = Utiliza los datos actuales y actualiza el cliente ya guardado" + Environment.NewLine +
							"[Cancelar] = Cancela operación";
						switch (MessageBox.Show(msn, "Cliente existente", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
						{
							case DialogResult.Cancel:
								return;
							case DialogResult.Yes:
								_cliente = cCuit;
								break;
							case DialogResult.No:
								cCuit.Nombre = _cliente.Nombre;
								cCuit.Direccion = _cliente.Direccion;
								cCuit.Telefono = _cliente.Telefono;
								cCuit.RespInscripto = _cliente.RespInscripto;
								ABMResultado abmResMod = cCuit.Modificacion();
								if (abmResMod.CodigoError != enErrores.Ninguno)
								{
									MessageBox.Show(abmResMod.MensajeError, "Error al modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
									return;
								}
								_cliente = cCuit;
								break;
						}
					}
					else if (_cliente.CUIT.Length > 0 && char.IsDigit(_cliente.CUIT[0]) && cCuit == null)
					{//cuit valido pero sin par en la BD, ingresar
						if (MessageBox.Show("El cliente no está en la base de datos" + Environment.NewLine +
							"¿Desea ingresarlo?", "Cliente inexistente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							ABMResultado abmResAlta = _cliente.Alta();
							if (abmResAlta.CodigoError != enErrores.Ninguno)
							{
								MessageBox.Show(abmResAlta.MensajeError, "Error al ingresar cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}
					}
					#endregion Cliente
				}
				for (int i = 0; i < _factura.Cuerpo.Count; i++)
				{
					_factura.Cuerpo[i].Cantidad *= -1;
					_factura.Cuerpo[i].Monto *= -1;
				}
				_factura.IdCliente = _cliente.Id;
				abmRes = _factura.Alta();
				if (abmRes.CodigoError != enErrores.Ninguno)
				{
					MessageBox.Show(abmRes.MensajeError, "Error al emitir", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (AppConfig.UsarFiscal)
				{
					abmRes = Fiscal.Imprimir(_cliente, _factura);
					if (abmRes.CodigoError != enErrores.Ninguno)
					{
						MessageBox.Show(abmRes.MensajeError, "Error al emitir comprobante fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
			}
			finally
			{
				Descontar(false, dtoK);
				Habilitar(true);
				if (abmRes != null && abmRes.CodigoError == enErrores.Ninguno)
				{
					Limpiar();
					abmRes = _factura.ConfirmarAlta(true);
				}
				else
					abmRes = _factura.ConfirmarAlta(false);
				if (abmRes != null && abmRes.CodigoError != enErrores.Ninguno)
					MessageBox.Show(abmRes.MensajeError, "Error al confirmar comprobante", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Cursor = Cursors.Default;
			}
		}

		#region Atajos Link
		void CrearAtajos()
		{
			for (int i = BotoneraAtajos.Controls.Count - 1; i >= 0; i--)
			{
				if (BotoneraAtajos.Controls[i] is LinkLabel)
					BotoneraAtajos.Controls.RemoveAt(i);
			}
			LinkLabel res = new LinkLabel();
			res.AutoSize = true;
			int inicio = 0;
			DataTable dt = Atajo.GetAll();
			foreach (DataRow r in dt.Rows)
			{
				Atajo atajo = new Atajo(r);
				string texto;
				LinkLabel.Link link = new LinkLabel.Link();
				link.Start = inicio;
				link.LinkData = atajo.IdArticulo;
				if (atajo.IdArticulo == 1)
				{
					texto = "Buscar...";
				}
				else
				{
					Articulo art = Articulo.GetSingleE(atajo.IdArticulo);
					texto = art.Codigo;
				}
				texto += "(" + (atajo.Control ? "Ctrl + " : "") + (atajo.Shift ? "Shift + " : "") + (atajo.Alt ? "Alt + " : "") + atajo.Tecla + ")";
				res.Text += texto + " - ";
				link.Length = texto.Length;
				inicio += texto.Length + 3;
				res.Links.Add(link);
			}
			//OJO
			LinkLabel.Link linkTarj = new LinkLabel.Link();
			linkTarj.Start = inicio;
			linkTarj.LinkData = Reglas.PagoTarjeta;
			linkTarj.Tag = "PAGOFORMA";
			res.Text += "Tarjeta (F7) - ";
			linkTarj.Length = "Tarjeta (F7)".Length;
			inicio += linkTarj.Length + 3;
			res.Links.Add(linkTarj);
			LinkLabel.Link linkCC = new LinkLabel.Link();
			linkCC.Start = inicio;
			linkCC.LinkData = Reglas.PagoCC;
			linkCC.Tag = "PAGOFORMA";
			res.Text += "CtaCte (F8)";
			linkCC.Length = "CtaCte (F8)".Length;
			inicio += linkCC.Length + 3;
			res.Links.Add(linkCC);
			//
			res.Text = res.Text.EndsWith(" - ") ? res.Text.Substring(0, res.Text.Length - 3) : res.Text;
			res.LinkClicked += new LinkLabelLinkClickedEventHandler(Atajos_LinkClicked);
			res.Location = new Point(111, 6);
			BotoneraAtajos.Controls.Add(res);
		}

		void Atajos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			uint id = (uint)e.Link.LinkData;
			if ((e.Link.Tag ?? "").ToString() == "PAGOFORMA")
			{
				cbPagoForma.SelectedValue = id;
			}
			else
			{
				if (id == 1)
					BuscarItem();
				else
					SetArticulo(id);
			}
		}

		private void lnk04_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			BuscarItem();
		}

		public void BuscarItem()
		{
			ABMForm busq = new ABMForm(false, false);//TH Ajustado
			QFiltro filtro = new QFiltro("Codigo");
			busq.AddAddOn(filtro);
			filtro.SeleccionAceptada += new EventHandler(BusqArt_SeleccionAceptada);
			filtro.SeleccionCancelada += new EventHandler(Filtro_SeleccionCancelada);
			busq.ActualizarTabla(Articulo.Esquema());
			busq.ActualizarTabla(Articulo.GetAll());
			busq.ActualizarTabla(Producto.GetAll());
			busq.TablaEsquema = "Esquemas";
			busq.TablaMain = Articulo.NombreTabla;
			busq.TeclaPulsada += new KeyEventHandler(delegate(object e, KeyEventArgs k) { if (k.KeyCode == Keys.Enter) BusqArt_SeleccionAceptada(e, k); });
			busq.Load += new EventHandler(delegate
			{
				new System.Threading.Thread(delegate()
				{
					System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
					busq.Invoke((MethodInvoker)delegate
					{
						busq.Icon = System.Drawing.Icon.FromHandle(Recursos.Articulo.GetHicon());
						busq.Text = "Buscar Item";
						busq.ReajustarGrilla();
						busq.Refresh();
					});
				}).Start();
			});
			busq.ShowDialog();
			busq.Dispose();
		}

		void BusqArt_SeleccionAceptada(object sender, EventArgs e)
		{
			QFiltro filtro = sender as QFiltro;
			ABMForm abm = filtro.ABMTool as ABMForm;
			if (abm.ItemActual == null)
			{
				MessageBox.Show("Debe elegir un item", "Seleccion", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Articulo art = new Articulo(abm.ItemActual.Row);
			SetArticulo(art.IdArticulo);
			abm.Close();
		}

		private void lnk03_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SetArticulo(4);
		}

		private void lnk02_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SetArticulo(3);
		}

		private void lnk01_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SetArticulo(2);
		}
		#endregion

		private void CVentas_Load(object sender, EventArgs e)
		{
			if (_factura.IdFactEncabezado == 0)
				Limpiar();
			else
			{
				//Queue<FactItem> items = new Queue<FactItem>(_factura.Cuerpo.Count);
				//foreach (FactItem it in _factura.Cuerpo)
				//    items.Enqueue(it);
				//_factura.Cuerpo.Clear();
				//while (items.Count > 0)
				//    BindSrc.Add(items.Dequeue());
				//BindSrc.DataSource = _factura.Cuerpo;
				Grilla.ReadOnly = true;
				Grilla.AllowUserToAddRows = false;
				Grilla.AllowUserToDeleteRows = false;
			}
			cbPagoForma_SelectedValueChanged(sender, e);
		}

		private void cbPagoForma_SelectedValueChanged(object sender, EventArgs e)
		{
			if (cbPagoForma.SelectedValue == null)
				return;
			uint fp = (uint)cbPagoForma.SelectedValue;
			tbCCAutorizadoDNI.Enabled = fp == Reglas.PagoCC && !SoloLectura;
			tbCCAutorizadoPatente.Enabled = fp == Reglas.PagoCC && !SoloLectura;
			bAutorizadoVer.Enabled = fp == Reglas.PagoCC && !SoloLectura;
			bBuscarAutorizado.Enabled = fp == Reglas.PagoCC && !SoloLectura;
			if (fp == Reglas.PagoCC)
			{
				tbCCAutorizadoDNI.Clear();
				tbCCAutorizadoPatente.Clear();
			}
		}

		private void bAutorizadoVer_Click(object sender, EventArgs e)
		{
			if (CCAutorizado.EsAutorizado(_cliente.Id, tbCCAutorizadoPatente.Text, tbCCAutorizadoDNI.Text))
				MessageBox.Show("Persona autorizada!", "Autorización", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				MessageBox.Show("Persona NO autorizada!", "Autorización", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void bBuscarAutorizado_Click(object sender, EventArgs e)
		{
			CCAutorizado res = null;
			QFiltro filtroQ;
			using (ABMForm busq = new ABMForm(false, false))//TH ajustado
			{
				busq.ActualizarTabla(CCAutorizado.Esquema());
				busq.ActualizarTabla(CCAutorizado.GetForIdCliente(_cliente.Id));
				busq.ActualizarTabla(Cliente.GetSingle(_cliente.Id));
				busq.TablaEsquema = "Esquemas";
				busq.TablaMain = CCAutorizado.NombreTabla;
				filtroQ = new QFiltro("Nombre");
				busq.AddAddOn(filtroQ);
				filtroQ.SeleccionAceptada += new EventHandler(delegate
				{
					ABMForm abm = busq;
					res = new CCAutorizado(abm.ItemActual.Row);
					abm.Close();
				});
				filtroQ.SeleccionCancelada += new EventHandler(delegate
				{
					ABMForm abm = busq;
					abm.Close();
				});
				filtroQ.PermitirABM = false;
				filtroQ.TextoFiltro = "";
				busq.Load += new EventHandler(delegate
				{
					new System.Threading.Thread(delegate()
					{
						System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
						busq.Invoke((MethodInvoker)delegate
						{
							busq.Icon = System.Drawing.Icon.FromHandle(Recursos.Cliente.GetHicon());
							busq.Text = "Buscar Autorizado";
							busq.ReajustarGrilla();
							busq.Refresh();
						});
					}).Start();
				});
				busq.ShowDialog();
			}
			if (res != null)
			{
				tbCCAutorizadoPatente.Text = res.Patente;
				tbCCAutorizadoDNI.Text = res.DNI;
			}
		}

		private void bSaldoCCCliente_Click(object sender, EventArgs e)
		{
			if (_cliente.Id == Cliente.ConsFinal.Id)
			{
				MessageBox.Show("Debe seleccionar un cliente para ver su saldo de cuenta corriente", "Saldo de Cuenta Corriente", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				decimal saldo = CCMovimiento.GetSaldoForCliente(_cliente.Id, Fiscal.Fecha());
				MessageBox.Show("Cliente: " + _cliente.Nombre + Environment.NewLine + "Saldo de cuenta corriente: " + saldo.ToString("$ 0.00"), "Saldo de Cuenta Corriente", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
