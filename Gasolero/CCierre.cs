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
using FormCom;

namespace Gasolero
{
	public partial class CCierre : UserControl
	{
		SortedList<uint, decimal> _totalesxId = new SortedList<uint, decimal>();
		SortedList<string, CCDatos> _cierreDatos = new SortedList<string, CCDatos>(20);
		MoneyTextBox moneyPropina;

		uint _nroTurno = 0;
		bool _modoEdicion;
		bool _hayCierresParciales = false;

		public CCierre()
			: this(false)
		{ }

		public CCierre(bool edicion)
		{
			_modoEdicion = edicion;
			InitializeComponent();
			InitMas(edicion);
			if (edicion)//si edita, el q ve es el ult cerrado
			{
				BCerrarTurno.Click -= new EventHandler(BCerrarTurno_Click);
				BCerrarTurno.Click += new EventHandler(BPlanillaActual_Click);
				BCerrarTurno.Text = "Ver Planilla";
				_nroTurno = Reglas.TurnoUltimo.Numero;
				ActualizarCierres();
			}
			else//si no, es el q va a cerrar
				lblTurno.Text = Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero + 1);
			BTurnoAnt.Visible = edicion;
			BTurnoSig.Visible = edicion;
			lblCierresParciales.Visible = edicion;
			//lblFechaParcial.Visible = edicion;
			lblContTurno.Visible = edicion;
		}

		void InitMas(bool edicion)
		{
			Label lbl, lbl3Dif;
			MoneyTextBox tb1In;//, tb3Dif;
			MoneyTextBox tb2Out, tb4Vta;
			Button bm;
			CheckBox chb;
			Cierre ultCierre;
			SortedList<uint, Articulo> arts = new SortedList<uint, Articulo>();
			//
			moneyPropina = new MoneyTextBox(4, 2, "$", ".");
			moneyPropina.Location = MTBPropinaAux.Location;
			moneyPropina.Size = MTBPropinaAux.Size;
			MTBPropinaAux.Parent.Controls.Add(moneyPropina);
			moneyPropina.ReadOnly = edicion;
			#region Controles
			for (int i = 0; i < Reglas.CantidadPuestos; i++)
			{
				string name = i.ToString();
				ultCierre = Cierre.GetUltimo((uint)(i + 1));
				uint idArt = Reglas.Puestos[i].IdArticulo;
				if (!arts.ContainsKey(idArt))
					arts.Add(idArt, Articulo.GetSingleE(idArt));
				Articulo artCte = arts[idArt];
				if (!_totalesxId.ContainsKey(artCte.IdArticulo))
					_totalesxId.Add(artCte.IdArticulo, 0);
				//
				lbl = new Label();
				lbl.Name = name;
				lbl.Text = ((int)(i + 1)).ToString() + " (" + artCte.Codigo + ")";
				lbl.Location = new Point(label1.Location.X, label1.Location.Y + (label1.Size.Height + 8) * i);
				lbl.AutoSize = false;
				lbl.Size = label1.Size;
				lbl.TextAlign = label1.TextAlign;
				label1.Parent.Controls.Add(lbl);
				//
				//Entrada
				tb1In = new MoneyTextBox(8, 3, "", ",", ".");
				tb1In.ReadOnly = !edicion;
				tb1In.Size = txtEntrada.Size;
				tb1In.Name = name;//nombre = puesto
				//tb1In.TextAlign = txtEntrada.TextAlign;
				//tb1In.Text = ultCierre.Salida.ToString("0.000");//la salida anterior es mi entrada
				tb1In.Valor = ultCierre.Salida;
				tb1In.ValorCambiado += new EventHandler(TBSalida_ValorCambiado);
				//tb1In.TextChanged += new EventHandler(TBSalida_ValorCambiado);
				tb1In.TabStop = false;
				tb1In.Location = new Point(txtEntrada.Location.X, txtEntrada.Location.Y + (txtEntrada.Size.Height + 8) * i);
				txtEntrada.Parent.Controls.Add(tb1In);
				//
				//Salida
				tb2Out = new MoneyTextBox(8, 3, "", ",", ".");
				//tb2Out = new MoneyTextBox(8, 3, "", ".");
				tb2Out.Name = name;
				tb2Out.ReadOnly = false;
				tb2Out.Size = txtSalida.Size;
				tb2Out.TabIndex = 2 + i * 2;
				tb2Out.Location = new Point(txtSalida.Location.X, txtSalida.Location.Y + (txtSalida.Size.Height + 8) * i);
				tb2Out.ValorCambiado += new EventHandler(TBSalida_ValorCambiado);
				//tb2Out.TextChanged += new EventHandler(TBSalida_ValorCambiado);
				txtSalida.Parent.Controls.Add(tb2Out);
				//
				//Boton de modificar entrada
				bm = new Button();
				bm.Name = name;
				bm.Size = BMod.Size;
				bm.Text = BMod.Text;
				bm.TabIndex = Reglas.CantidadPuestos * 2 + 5 + i;
				bm.Location = new Point(BMod.Location.X, BMod.Location.Y + (BMod.Size.Height + 8) * i);
				bm.Click += new EventHandler(BMod_Click);
				BMod.Parent.Controls.Add(bm);
				//
				//Check ¿cerrar puesto?
				chb = new CheckBox();
				chb.Name = name;
				chb.AutoSize = false;
				chb.Size = chbCerrar.Size;
				chb.Text = chbCerrar.Text;
				chb.TabIndex = 3 + i * 2;
				chb.Location = new Point(chbCerrar.Location.X, chbCerrar.Location.Y + (chbCerrar.Size.Height + 4) * i);
				chbCerrar.Parent.Controls.Add(chb);
				//
				//tb3Dif = new TextBox();
				lbl3Dif = new Label();
				lbl3Dif.AutoSize = false;
				lbl3Dif.Size = txtDiferencia.Size;
				lbl3Dif.Name = name;
				//lbl3Dif.TextAlign = txtDiferencia.TextAlign;
				lbl3Dif.TextAlign = ContentAlignment.MiddleRight;
				lbl3Dif.Text = ((decimal)(0)).ToString("0.000");
				lbl3Dif.TabStop = false;
				lbl3Dif.Location = new Point(txtDiferencia.Location.X, txtDiferencia.Location.Y + (txtDiferencia.Size.Height + 8) * i);
				txtDiferencia.Parent.Controls.Add(lbl3Dif);
				//
				tb4Vta = new MoneyTextBox(8, 3, "$", ",", ".");
				//tb4Vta = new MoneyTextBox(8, 3, "$", ".");
				tb4Vta.ReadOnly = true;
				tb4Vta.Size = txtVenta.Size;
				tb4Vta.Name = name;
				tb4Vta.TabStop = false;
				tb4Vta.Location = new Point(txtVenta.Location.X, txtVenta.Location.Y + (txtVenta.Size.Height + 8) * i);
				tb4Vta.ValorCambiado += new EventHandler(MoneyVta_ValorCambiado);
				txtDiferencia.Parent.Controls.Add(tb4Vta);
				//
				//Edicion
				//boton "ver cierre anterior" pero individual, es decir, de los cierres parciales
				Button bCAnt = new Button();
				bCAnt.Name = name;
				bCAnt.Size = BAnt.Size;
				bCAnt.Text = BAnt.Text;
				bCAnt.Location = new Point(BAnt.Location.X, BAnt.Location.Y + (BAnt.Size.Height + 8) * i);
				bCAnt.TabStop = false;
				bCAnt.Visible = edicion;
				bCAnt.Click += new EventHandler(BCierreAnt_Click);
				BAnt.Parent.Controls.Add(bCAnt);
				//
				//boton "ver cierre siguiente" pero individual, es decir, de los cierres parciales
				Button bCSig = new Button();
				bCSig.Name = name;
				bCSig.Size = BSig.Size;
				bCSig.Text = BSig.Text;
				bCSig.Location = new Point(BSig.Location.X, BSig.Location.Y + (BSig.Size.Height + 8) * i);
				bCSig.TabStop = false;
				bCSig.Visible = edicion;
				bCSig.Click += new EventHandler(BCierreSig_Click);
				BSig.Parent.Controls.Add(bCSig);
				//
				//txt x / y , señaliza el nro de cierre q se esta viendo
				Label lblNC = new Label();
				lblNC.AutoSize = false;
				lblNC.Visible = edicion;
				lblNC.Size = lblCont.Size;
				lblNC.TextAlign = lblCont.TextAlign;
				lblNC.Location = new Point(lblCont.Location.X, lblCont.Location.Y + (lblCont.Size.Height + 8) * i);
				lblCont.Parent.Controls.Add(lblNC);
				//
				//txt *, señaliza si hubo un cambio en el cierre
				Label lblMod = new Label();
				lblMod.AutoSize = false;
				lblMod.Visible = false;
				lblMod.Size = lblModificado.Size;
				lblMod.Font = lblModificado.Font;
				lblMod.ForeColor = lblModificado.ForeColor;
				lblMod.Text = lblModificado.Text;
				lblMod.TextAlign = lblModificado.TextAlign;
				lblMod.Location = new Point(lblModificado.Location.X, lblModificado.Location.Y + (lblModificado.Size.Height + 8) * i);
				lblModificado.Parent.Controls.Add(lblMod);
				//
				//Boton aplicar cambios
				Button bGuardar = new Button();
				bGuardar.Name = name;
				bGuardar.Size = BGuardarCierre.Size;
				bGuardar.Text = BGuardarCierre.Text;
				bGuardar.Image = BGuardarCierre.Image;
				bGuardar.Visible = edicion;
				bGuardar.Location = new Point(BGuardarCierre.Location.X, BGuardarCierre.Location.Y + (BGuardarCierre.Size.Height + 8) * i);
				bGuardar.Click += new EventHandler(BGuardarCierre_Click);
				BGuardarCierre.Parent.Controls.Add(bGuardar);
				//
				//Boton reset cambios
				Button bReset = new Button();
				bReset.Name = name;
				bReset.Size = BResetCierre.Size;
				bReset.Text = BResetCierre.Text;
				bReset.Image = BResetCierre.Image;
				bReset.Visible = edicion;
				bReset.Location = new Point(BResetCierre.Location.X, BResetCierre.Location.Y + (BResetCierre.Size.Height + 8) * i);
				bReset.Click += new EventHandler(BResetCierre_Click);
				BResetCierre.Parent.Controls.Add(bReset);
				//
				CCDatos cc = new CCDatos();
				cc.check = chb;
				cc.diferencia = lbl3Dif;
				cc.entrada = tb1In;
				cc.salida = tb2Out;
				cc.txt = lbl;
				cc.txtNroCierre = lblNC;
				cc.venta = tb4Vta;
				cc.bmod = bm;
				cc.modificado = lblMod;
				cc.Articulo = artCte;
				cc.Editando = edicion;
				_cierreDatos.Add(i.ToString(), cc);
			}
			#endregion Controles
		}

		bool _actualizandoItem = false;
		public void ActualizarArticulo(uint id)
		{
			_actualizandoItem = true;
			Articulo art = Articulo.GetSingleE(id);
			decimal total = 0;
			foreach (CCDatos cc in _cierreDatos.Values)
			{
				if (cc.Articulo.IdArticulo == id)
				{
					cc.Articulo = art;
					total += cc.venta.Valor;
				}
			}
			_actualizandoItem = false;
			_totalesxId[id] = total;
			EscribirTotales();
		}

		public void ActualizarCierres(uint turno)
		{
			if (_nroTurno != turno)
				return;
			ActualizarCierres();
		}

		public bool ActualizarCierres()
		{
			Turno t = Turno.GetForNumero(_nroTurno);
			if (t == null)
				return false;
			_hayCierresParciales = false;
			for (uint i = 0; i < Reglas.CantidadPuestos; i++)
			{
				Cierre[] cierres = Cierre.GetListaXSurtidorTurno(i + 1, _nroTurno);
				CCDatos cc = _cierreDatos[i.ToString()];
				cc.LoadCierres(cierres);
				if (cierres.Length > 1)
					_hayCierresParciales = true;
			}
			moneyPropina.Valor = t.Propina;
			lblContTurno.Text = "T" + Turno.GetTurnoNbre(t.Numero)[0].ToString() + ": " + t.FechaFinal.ToShortDateString();
			lblTurno.Text = "Turno " + Turno.GetTurnoNbre(t.Numero) + ", fecha de cierre: " + t.FechaFinal.ToString();
			BTurnoSig.Enabled = _nroTurno != Reglas.TurnoUltimo.Numero;
			BTurnoAnt.Enabled = Turno.GetForNumero(_nroTurno - 1) != null;
			lblMensaje.Text = "Sin Comentarios";
			return true;
		}

		void MoneyVta_ValorCambiado(object sender, EventArgs e)
		{
			if (_actualizandoItem)
				return;
			MoneyTextBox money = sender as MoneyTextBox;
			CCDatos ccSender = _cierreDatos[money.Name];
			Articulo art = ccSender.Articulo;
			decimal total = 0;
			foreach (CCDatos cc in _cierreDatos.Values)
			{
				if (cc.Articulo.IdArticulo == art.IdArticulo)
					total += cc.venta.Valor;
			}
			_totalesxId[art.IdArticulo] = total;
			EscribirTotales();
		}

		private void EscribirTotales()
		{
			lblTotales.Text = "Totales --> ";
			SortedList<uint, Articulo> vistos = new SortedList<uint, Articulo>();
			string totales = "";
			decimal totalTot = 0;
			foreach (CCDatos cc in _cierreDatos.Values)
			{
				Articulo artM = cc.Articulo;
				if (!vistos.ContainsKey(artM.IdArticulo))
				{
					vistos.Add(artM.IdArticulo, artM);
					totales += artM.Codigo + ": " + _totalesxId[artM.IdArticulo].ToString("0.00 $") + "  ";
					totalTot += _totalesxId[artM.IdArticulo];
				}
			}
			lblTotales.Text += totales + "| TOTAL: " + totalTot.ToString("0.000 $");
		}

		void TBSalida_ValorCambiado(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;
			CCDatos ccSender = _cierreDatos[ctrl.Name];
			MoneyTextBox money = ccSender.salida;
			ccSender.check.Checked = true;
			int pto = int.Parse(money.Name);
			if (money.Valor >= Reglas.Puestos[pto].Ciclo)
			{
				money.Valor = Reglas.Puestos[pto].Ciclo - 1;
				return;
			}
			decimal dif = money.Valor - ccSender.entrada.Valor;
			if (dif < 0)
			{
				dif += Reglas.Puestos[pto].Ciclo;
			}
			ccSender.diferencia.Text = dif.ToString("0.000");
			ccSender.diferencia.ForeColor = dif <= Reglas.Puestos[pto].Margen ? SystemColors.ControlText : Color.Red;
			ccSender.venta.Valor = ccSender.Articulo.Precio * dif;
			//
			//edicion
			if (ccSender.Editando)
			{
				bool distinto = ccSender.CierreActual.Entrada != ccSender.entrada.Valor;
				distinto |= ccSender.CierreActual.Salida != ccSender.salida.Valor;
				ccSender.modificado.Visible = distinto;
			}
		}

		private void BMod_Click(object sender, EventArgs e)
		{
			if (!Reglas.VendedorActual.TienePermiso(enPermisos.SuperUsuario))
			{
				MessageBox.Show("Permiso insuficiente");
				return;
			}
			Button b = sender as Button;
			CCDatos cc = _cierreDatos[b.Name];
			Esquema esq = new Esquema();
			esq.Tipo = Esquema.enControles.Money;
			esq.Mascara = "00000000.000";
			esq.ValorDefectoOriginal = cc.entrada.Valor;
			Prompt pr = new Prompt(esq);
			if (pr.ShowDialog() != DialogResult.OK)
				return;
			decimal nvoValor = 0;
			object objAux = pr.GetValue();
			if (objAux == null || !decimal.TryParse(objAux.ToString(), out nvoValor))
			{
				MessageBox.Show("Valor incorrecto");
				return;
			}
			Cierre ultCierre = Cierre.GetUltimo(uint.Parse(b.Name) + 1);
			decimal difCt = ultCierre.Salida - nvoValor;
			decimal difMoney = Math.Abs(difCt) * ultCierre.Costo;
			if (difCt == 0)
				return;
			else if (difCt > 0 && MessageBox.Show("Se emitirá una nota de crédito por la diferencia ($ " + difMoney.ToString("0.00") + ")" + Environment.NewLine + "¿Desea continuar?", "Confirmar",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;
			else if (difCt < 0 && MessageBox.Show("Se emitirá una factura por la diferencia ($ " + difMoney.ToString("0.00") + ")" + Environment.NewLine + "¿Desea continuar?", "Confirmar",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;
			FactEncabezado[] factDif = null;
			Cursor = Cursors.WaitCursor;
			ABMResultado abmRes;
			if (MessageBox.Show("¿Desea imprimir el comprobante de la diferencia?", "Confirmar",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				abmRes = ultCierre.ModificarSalida(nvoValor, out factDif);
			}
			else
			{
				ultCierre.Salida = nvoValor;
				abmRes = ultCierre.Modificacion();
			}
			if (abmRes.CodigoError == enErrores.Ninguno)
			{
				cc.entrada.Valor = nvoValor;
				if (factDif != null && factDif.Length > 0)
				{
					foreach (FactEncabezado f in factDif)
						Fiscal.Imprimir(Cliente.ConsFinal, f);
				}
				Cursor = Cursors.Default;
				MessageBox.Show("Entrada cambiada");
			}
			else
			{
				Cursor = Cursors.Default;
				MessageBox.Show(abmRes.MensajeError, abmRes.CodigoError.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void BCierreAnt_Click(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;
			CCDatos cc = _cierreDatos[ctrl.Name];
			foreach (CCDatos otro in _cierreDatos.Values)
			{
				if (otro.Articulo.IdArticulo == cc.Articulo.IdArticulo)
					otro.CierreAnt();
			}
		}

		void BCierreSig_Click(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;
			CCDatos cc = _cierreDatos[ctrl.Name];
			foreach (CCDatos otro in _cierreDatos.Values)
			{
				if (otro.Articulo.IdArticulo == cc.Articulo.IdArticulo)
					otro.CierreSig();
			}
		}

		private void BCerrarTodo_Click(object sender, EventArgs e)
		{
			foreach (CCDatos cc in _cierreDatos.Values)
			{
				cc.check.Checked = true;
				decimal v = cc.entrada.Valor;
				if (cc.salida.Valor == 0)
					cc.salida.Valor = v;
			}
		}

		private void BCerrarTurno_Click(object sender, EventArgs e)
		{
			if (BCerrarTurno.Text.ToLower().Contains("cerrar"))
			{
				if (CerrarTurno())
					BCerrarTurno.Text = "Rendir Turno";
			}
			else if (BCerrarTurno.Text.ToLower().Contains("rendir"))//rendir
			{
				if (RendirTurno())
					BCerrarTurno.Text = "Cerrar Turno";
			}
			else
				MessageBox.Show("Error en lógica de cierre", "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		bool CerrarTurno()
		{
			ABMResultado abmRes;
			uint ultOpIncluida = Reglas.UltimoIdIncluido;
			SortedList<uint, Articulo> arts = new SortedList<uint, Articulo>();
			List<Cierre> cierres = new List<Cierre>();
			List<ValeEntrega> vales = null;
			foreach (CCDatos cc in _cierreDatos.Values)
			{
				if (!cc.check.Checked)
					continue;
				int numero = int.Parse(cc.entrada.Name);
				decimal entrada = cc.entrada.Valor;
				decimal costo = 0;
				//
				uint idArt = Reglas.Puestos[numero].IdArticulo;
				if (!arts.ContainsKey(idArt))
					arts.Add(idArt, Articulo.GetSingleE(idArt));
				Articulo artCte = arts[idArt];
				costo = artCte.Precio;
				Cierre cierre = new Cierre();
				cierre.IdArticulo = idArt;
				cierre.Turno = Reglas.TurnoUltimo.Numero + 1;
				cierre.IdVendedor = Reglas.VendedorActual.IdVendedor;
				cierre.Surtidor = (uint)numero + 1;//numero de surtidor
				cierre.Costo = costo;
				cierre.Entrada = entrada;
				cierre.Salida = cc.salida.Valor;
				enTipoMargenViolado margenViolado = cierre.VigilarMargen();
				string mayorOmenor = margenViolado.ToString().ToLower().Contains("mayor") ? "MAYOR" : "MENOR";
				switch (margenViolado)
				{
					case enTipoMargenViolado.SupMayor:
					case enTipoMargenViolado.SupMenor:
					case enTipoMargenViolado.MaxMargenMayor:
						if (Reglas.VendedorActual.EsRol(enPermisos.Encargado) >= 0)
							goto case enTipoMargenViolado.InfMenor;//margen violado, puede ser pasado por el encargado
						else
						{
							MessageBox.Show("La diferencia entre la salida y la entrada" +
								Environment.NewLine + "es " + mayorOmenor + " al margen permitido." +
								Environment.NewLine + "Si esta diferencia es correcta" +
								Environment.NewLine + "pida al encargado que realice el cierre.",
								"Margen Sospechoso Pto " + ((int)numero + 1).ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}//o pasa al sig o rechaza
					//case enTipoMargenViolado.Cero://¿verificar por cero?
					case enTipoMargenViolado.InfMayor:
					case enTipoMargenViolado.InfMenor:
						//string msgMargen = margenViolado == enTipoMargenViolado.Cero ? "es cero (0)!" : "es " + mayorOmenor + " al margen permitido.";
						string msgMargen = "es " + mayorOmenor + " al margen permitido.";
						if (MessageBox.Show("La diferencia entre la salida y la entrada" +
							Environment.NewLine + msgMargen +
							Environment.NewLine + "¿Está seguro que desea cerrar así el puesto " + ((int)numero + 1).ToString() + "?",
							"Margen Sospechoso Pto " + ((int)numero + 1).ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
							== DialogResult.No)
							return false;
						break;
				}//fin chequeo de cierre por margen
				cierres.Add(cierre);
			}
#warning previsualizar planilla - quitar
			if (cierres.Count == Reglas.CantidadPuestos)
			{
				vales = AnotarVales(out ultOpIncluida);
				if (vales == null)
					return false;
				//Planilla plaPrev = new Planilla(cierres.ToArray(), moneyPropina.Valor, vales.ToArray());
				//plaPrev.Previsualizar = true;
				//plaPrev.WindowState = FormWindowState.Maximized;
				//plaPrev.ShowDialog();
				//if (MessageBox.Show("¿Confirma cierre de turno?", "Cierre de Turno",
				//    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				//    return;
			}
			else if (cierres.Count == 0)
			{
				MessageBox.Show("Debe elegir al menos un surtidor para cerrar", "Cierre Inválido",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else //es un cierre parcial
			{
				if (MessageBox.Show("¿Está seguro que quiere hacer un cierre parcial de turno?", "Cierre Parcial de Turno",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return false;
				vales = AnotarVales(out ultOpIncluida);
				if (vales == null)
					return false;
			}
			//
			FactEncabezado[] difs;
			abmRes = Turno.CerrarTurno(out difs, cierres.ToArray(), moneyPropina.Valor, vales.ToArray());
			if (abmRes.CodigoError != enErrores.Ninguno)
			{
				MessageBox.Show(abmRes.MensajeError, abmRes.CodigoError.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			VerDiferencia(difs);
			foreach (FactEncabezado f in difs)
				Fiscal.Imprimir(Cliente.ConsFinal, f);
			//
			if (cierres.Count == Reglas.CantidadPuestos)
			{
				//Fiscal.CierreX();
				if (Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero) == enTurnos.Tarde.ToString())
				{
					bool exito = false, reintentar = false; ;
					do
					{
						exito = Fiscal.CierreZ();
						System.Threading.Thread.Sleep(500);
						if (!exito)
							reintentar = MessageBox.Show("Hubo un error al imprimir el cierre Z" + Environment.NewLine + "¿Desea intentar nuevamente?", "Error Fiscal",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
					} while (!exito && reintentar);
				}
				FrmMain.Texto = "Gasolero - Turno: " + Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero + 1);
			}
			if (abmRes.CodigoError == enErrores.Ninguno)//siempre true
			{
				Reglas.UltimoIdIncluido = ultOpIncluida;
				Refrescar();
				bool cierreParcial = cierres.Count != Reglas.CantidadPuestos;
				//if (MessageBox.Show("¿Imprimir Planilla?", cierreParcial ? "Cierre Parcial" : "Cierre de Turno", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				//    == DialogResult.Yes)//NO imprimir planilla aun
				{
					//imprimir planilla --- NO!
					if (cierreParcial)
					{
						ImprimirPlanilla(cierres.ToArray(), moneyPropina.Valor, vales.ToArray());
						return false;
					}
					else
					{
						Turno tUlt = Turno.GetUltimo();
						//ImprimirPlanilla(tUlt);
						DataTable dtParcialesUltTurno = Parcial.GetForTurno(tUlt.Numero);
						//if (dtParcialesUltTurno.Rows.Count > 0 && Reglas.ImprimirFinalComoParcial)//NO IMPRIMIR MAS
						//{
						//    Cierre[] cierresUltParcial = Cierre.GetListaXParcial(tUlt.Numero, 0);
						//    ValeEntrega[] valesUltParcial = ValeEntrega.GetForParcial(tUlt.Numero, 0);
						//    ImprimirPlanilla(cierresUltParcial, moneyPropina.Valor, valesUltParcial);
						//}
					}
					//
					//imprimir ccMovs --- NO! Todo junto al rendir $
					//FrmMain.FMain.ImprimirCCTurno(true);
				}
				//if (!cierreParcial)
				//    FrmMain.FMain.LogOut();
			}
			return abmRes.CodigoError == enErrores.Ninguno;
		}

		List<ValeEntrega> AnotarVales(out uint idHasta)
		{
			idHasta = Reglas.UltimoIdIncluido;//si no hay vales queda el mismo
			uint aux = 0;
			Cursor cur = Cursor;
			Cursor = Cursors.WaitCursor;
			try
			{
				List<ValeEntrega> res = new List<ValeEntrega>();
				List<Articulo> articulos = new List<Articulo>();
				EsquemaCollection esqColl = new EsquemaCollection();
				DataTable dtArt = Articulo.GetAll();
				//
				#region Elegir Ultimo Vale a Incluir
				DataTable dt = Operacion.GetVales(idHasta + 1);
				if (dt.Rows.Count == 1)//si hay uno preguntar si incluirlo
				{
					Operacion opUnica = new Operacion(dt.Rows[0]);
					switch (MessageBox.Show("¿Incluir el ultimo vale?" + Environment.NewLine + opUnica.Fecha.ToString() + " " + Articulo.GetDescXCodProd(opUnica.CodProducto) + " " + opUnica.Cantidad +
						Environment.NewLine + "Elija NO si no quiere incluir el vale en el cierre" + Environment.NewLine + "Elija CANCELAR para cancelar el cierre",
						"Incluir vale en cierre", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
					{
						case DialogResult.Yes:
							idHasta = opUnica.IdOperacion;
							break;
						case DialogResult.Cancel:
							return null;
					}
				}
				else if (dt.Rows.Count > 1)//si hay mas de uno que elija
				{
					ABMForm operacionesForm = new ABMForm(false, false);//TH ajustado
					operacionesForm.ActualizarTabla(Operacion.Esquema());
					operacionesForm.ActualizarTabla(dt);
					operacionesForm.ActualizarTabla(Vehiculo.GetConsumidores());
					operacionesForm.ActualizarTabla(Articulo.GetAll());
					operacionesForm.TablaEsquema = "Esquemas";
					operacionesForm.TablaMain = "Operaciones";
					operacionesForm.MultiSeleccion = false;
					operacionesForm.Load += delegate(object sender, EventArgs e)
					{
						new System.Threading.Thread(delegate()
						{
							System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
							operacionesForm.Invoke((MethodInvoker)delegate
							{
								operacionesForm.ReajustarGrilla();
								operacionesForm.SeleccionarUltimo();
								operacionesForm.Text = "Elija (2xClic) hasta que vale incluir en el cierre";
								operacionesForm.Refresh();
							});
						}).Start();
					};
					operacionesForm.TeclaPulsada += delegate(object sender, KeyEventArgs e)
					{
						DataTable dtSeleccion = operacionesForm.ObtenerSeleccion();
						if (e.KeyCode == Keys.Enter && dtSeleccion.Rows.Count == 1)
						{
							Operacion op = new Operacion(dtSeleccion.Rows[0]);
							aux = op.IdOperacion;
							operacionesForm.Close();
						}
					};
					operacionesForm.ItemDobleClicked += delegate(IABMTool abm, ItClickEventArgs e)
					{
						DataRow r = e.Item.Row;
						Operacion op = new Operacion(r);
						aux = op.IdOperacion;
						operacionesForm.Close();
					};
					operacionesForm.Dock = DockStyle.Fill;
					operacionesForm.ShowDialog();
					if (aux == 0)
						return null;
					idHasta = aux;
				}
				#endregion
				//
				List<Operacion> operaciones;
				SortedList<int, decimal> valesRealizados = Operacion.GetValesPorProducto(Reglas.UltimoIdIncluido + 1, idHasta, out operaciones);
				for (int i = 1; i < dtArt.Rows.Count; i++)
				{
					Articulo art = new Articulo(dtArt.Rows[i]);
					articulos.Add(art);
					Esquema esq = new Esquema();
					esq.Dato = art.IdArticulo.ToString();
					esq.Alias = art.Descripcion;
					esq.Tipo = Esquema.enControles.Money;
					esq.Mascara = "00000.000";
					esq.ValorDefectoOriginal = valesRealizados.ContainsKey(art.CodProducto) ? valesRealizados[art.CodProducto] : 0;
					esqColl.Add(esq);
				}
				Prompt p = new Prompt(esqColl, null);
				p.Text = "Ingrese Vales";
				//si hay que confirmar los vales y se cancela, devolver null
				if (AppConfig.ConfirmarVales && p.ShowDialog() != DialogResult.OK)
					return null;
				foreach (var art in articulos)
				{
					ValeEntrega vale = new ValeEntrega();
					vale.Cantidad = AppConfig.ConfirmarVales ?
						decimal.Parse(p.GetValue(art.IdArticulo.ToString()).ToString()) :
						(valesRealizados.ContainsKey(art.CodProducto) ? valesRealizados[art.CodProducto] : 0);
					vale.IdArticulo = art.IdArticulo;
					vale.Precio = art.Precio;
					vale.Operaciones = new List<Operacion>();
					foreach (var item in operaciones)
					{
						if (item.CodProducto == art.CodProducto)
							vale.Operaciones.Add(item);
					}
					res.Add(vale);
				}
				return res;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al verificar vales", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
			finally
			{
				Cursor = cur;
			}
		}

		public bool RendirTurno()
		{
			Turno tUlt = Turno.GetUltimo();
			//Ver si el último turno no se rindió
			if (CajaEntrega.GetSingleForTurno(tUlt.IdTurno) != null)
			{
				MessageBox.Show("El último turno ya fue rendido", "Error al Rendir Turno", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
			//pedir montos
			EsquemaCollection esquemas = new EsquemaCollection();
			Esquema esq = new Esquema();
			esq.Tipo = Esquema.enControles.Money;
			esq.Dato = "Efectivo";
			esquemas.Add(esq);
			esq = new Esquema();
			esq.Tipo = Esquema.enControles.Money;
			esq.Dato = "Tarjeta";
			esquemas.Add(esq);
			Prompt pr = new Prompt(esquemas, null);
			if (pr.ShowDialog() != DialogResult.OK)
				return false;//no puso los valores
			//
			//obtener los valores
			decimal rendirEfectivo = (decimal)pr.GetValue("Efectivo");
			decimal rendirTarjeta = (decimal)pr.GetValue("Tarjeta");
			CajaEntrega rendicion = new CajaEntrega();
			rendicion.AddFila(1, rendirEfectivo);
			rendicion.AddFila(Reglas.PagoTarjeta, rendirTarjeta);
			ABMResultado abmRes = rendicion.Alta();
			if (abmRes.CodigoError != enErrores.Ninguno)
			{
				MessageBox.Show(abmRes.MensajeError, abmRes.CodigoError.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else
			{
				//Rendición finalizada
				//con los valores -> imprimir las planillas
				ImprimirPlanilla(tUlt, 1);
				FrmMain.FMain.LogOut();
				return true;
			}
		}

		private void BPlanillaActual_Click(object sender, EventArgs e)
		{
			if (!_hayCierresParciales)
				VerPlanilla(_nroTurno);
			else
			{
				FrmPreg prg = new FrmPreg();
				prg.ShowDialog();
				switch (prg.Opcion)
				{
					case FrmPreg.enOpciones.A:
						VerPlanilla(_nroTurno);
						break;
					case FrmPreg.enOpciones.B:
						uint idParcial;
						if (!SeleccionarCierreParcial(_nroTurno, out idParcial))
							break;
						Cierre[] cierres = Cierre.GetListaXParcial(_nroTurno, idParcial);
						ValeEntrega[] vales = ValeEntrega.GetForParcial(_nroTurno, idParcial);
						Planilla plaPrev = new Planilla(cierres, moneyPropina.Valor, vales, false);
						plaPrev.Previsualizar = true;
						plaPrev.WindowState = FormWindowState.Maximized;
						plaPrev.ShowDialog();
						if (MessageBox.Show("¿Imprimir planilla?", "Planilla de Cierre Parcial", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
							== DialogResult.Yes)
							ImprimirPlanilla(cierres, moneyPropina.Valor, vales);
						break;
				}
			}
		}

		private void BPlanillaUltCierre_Click(object sender, EventArgs e)
		{
			if (_modoEdicion)
			{
				_nroTurno = Reglas.TurnoUltimo.Numero;
				ActualizarCierres();
			}
			else
			{
				VerPlanilla(Reglas.TurnoUltimo);
			}
		}

		private void BPlanillaOtroCierre_Click(object sender, EventArgs e)
		{
			ABMForm abm = new ABMForm(true, false);//TH ajustado
			DateTime now = Reglas.Now;
			abm.ActualizarTabla(Turno.GetForFechas(now.Date, now.Date.AddDays(1)));
			abm.ActualizarTabla(Turno.Esquema());
			abm.ActualizarTabla(Vendedor.GetAll());
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = Turno.NombreTabla;
			abm.AddAddOn(new FactFiltro(FactFiltro.enFiltrado.Turnos));
			//
			ABMFiltro filtro = abm.ObtenerAddOn<ABMFiltro>();
			filtro.FiltroVisible = false;
			Turno turnoSeleccionado = null;
			filtro.SeleccionAceptada += delegate(object s2, EventArgs e2)
			{
				if (abm.ItemActual != null)
				{
					turnoSeleccionado = new Turno(abm.ItemActual.Row);
					abm.Close();
				}
			};
			filtro.SeleccionCancelada += delegate(object s2, EventArgs e2)
			{
				abm.Close();
			};
			abm.Load += delegate(object sender2, EventArgs e2)
			{
				new System.Threading.Thread(delegate()
				{
					System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
					abm.Invoke((MethodInvoker)delegate
					{
						abm.ReajustarGrilla();
						abm.Text = "Elija (2xClic) el cierre";
						abm.Refresh();
					});
				}).Start();
			};
			abm.ShowDialog();
			if (turnoSeleccionado != null)
			{
				if (_modoEdicion)
				{
					_nroTurno = turnoSeleccionado.Numero;
					ActualizarCierres();
				}
				else
				{
					VerPlanilla(turnoSeleccionado);
				}
			}
		}

		public void VerPlanilla(uint nroTurno)
		{
			Turno t = Turno.GetForNumero(nroTurno);
			if (t == null)
				MessageBox.Show("El turno elegido no existe");
			else
				VerPlanilla(t);
		}

		public void VerPlanilla(Turno t)
		{
			//Planilla pla = new Planilla(t);
			//pla.WindowState = FormWindowState.Maximized;
			//pla.ShowDialog();
			//VerDiferencia(t);
			ImprimirPlanilla(t);
		}

		public void ImprimirPlanilla(Turno t, int hoja = 3)
		{
			if ((hoja & 1) > 0)
			{
				Planilla pla = new Planilla(t);
				pla.Imprimir();
			}
			//RptRendicion rptRendicion = new RptRendicion(t);
			//rptRendicion.Imprimir();
			if ((hoja & 2) > 0)
			{
				DateTime fchaTurnoDde = Turno.GetForNumero(t.Numero - 1).FechaFinal;
				DateTime fchaTurnoHta = t.FechaFinal;
				RptCCMov rptCC = new RptCCMov(fchaTurnoDde, fchaTurnoHta);
				rptCC.Imprimir();
			}
		}

		public void ImprimirPlanilla(Cierre[] cierres, decimal propina, ValeEntrega[] vales)
		{
			Planilla pla = new Planilla(cierres, propina, vales, false);
			pla.Imprimir();
		}

		void VerDiferencia(Turno turno)
		{
			if (!Reglas.VerDiferencia)
				return;
			if (turno.Numero <= 1)
			{
				MessageBox.Show("No hay diferencias para el 1er turno", "Turno Inválido");
				return;
			}
			Turno tAnt = Turno.GetForNumero(turno.Numero - 1);
			SortedList<uint, decimal> facturadoXItem = new SortedList<uint, decimal>();
			foreach (Puesto p in Reglas.Puestos)
			{
				if (!facturadoXItem.ContainsKey(p.IdArticulo))
					facturadoXItem.Add(p.IdArticulo, 0);
				foreach (Cierre c in Cierre.GetListaXSurtidorTurno(p.IdPuesto, turno.Numero))
				{
					Densidad dens = Densidad.GetSingleForArt(p.IdArticulo);
					decimal dif = c.GetDiferencia();
					facturadoXItem[p.IdArticulo] -= dif * dens.Valor;
				}
			}
			foreach (DataRow r in FactEncabezado.GetListForFechas(tAnt.FechaFinal, turno.FechaFinal).Rows)
			{
				FactEncabezado f = new FactEncabezado(r);
				f.GetFilas();
				foreach (FactItem factIt in f.Cuerpo)
				{
					if (facturadoXItem.ContainsKey(factIt.IdArticulo))
						facturadoXItem[factIt.IdArticulo] += factIt.Cantidad;
				}
			}
			string msn = "Cantidades no facturadas: " + Environment.NewLine;
			foreach (KeyValuePair<uint, decimal> kvp in facturadoXItem)
			{
				Articulo art = Articulo.GetSingleE(kvp.Key);
				Densidad dens = Densidad.GetSingleForArt(kvp.Key);
				decimal noFact = kvp.Value * -1;
				msn += art.Codigo + ": " + noFact.ToString("0.00").PadLeft(7) +
					"\tDensidad: " + dens.Valor.ToString("0.000").PadLeft(5) + "\tTotal: " + decimal.Multiply(noFact, dens.Valor).ToString("0.00").PadLeft(7) + Environment.NewLine;
			}
			MessageBox.Show(msn, "Diferencia de Facturación");
		}

		bool VerDiferencia(FactEncabezado[] difs)
		{
			if (!Reglas.VerDiferencia)
				return true;
			string msn = "Cantidades no facturadas: " + Environment.NewLine;
			SortedList<uint, decimal> noFacturadoXItem = new SortedList<uint, decimal>(5);
			foreach (FactEncabezado fact in difs)
			{
				foreach (FactItem it in fact.Cuerpo)
				{
					if (noFacturadoXItem.ContainsKey(it.IdArticulo))
						noFacturadoXItem[it.IdArticulo] += it.Cantidad;
					else
						noFacturadoXItem.Add(it.IdArticulo, it.Cantidad);
				}
			}
			foreach (KeyValuePair<uint, decimal> kvp in noFacturadoXItem)
			{
				Articulo art = Articulo.GetSingleE(kvp.Key);
				decimal noFact = kvp.Value;
				string nbre = art.Codigo + ":";
				msn += nbre.PadRight(11) + "\t" + noFact.ToString("0.000").PadLeft(10) + Environment.NewLine;
			}
			return MessageBox.Show(msn + Environment.NewLine + Environment.NewLine + "A continuación se emitirán las facturas correspondientes", "Diferencia de Facturación",
				  MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;
		}

		public void ReveerSeguridad()
		{
			bool esEncargado =
				Reglas.VendedorActual.TienePermiso(enPermisos.Encargado);
			bool esAdmin =
				Reglas.VendedorActual.TienePermiso(enPermisos.SuperUsuario);
			bool esAdminAFIP = Reglas.VendedorActual.EsRol(enPermisos.Administrador) == 0;
			foreach (CCDatos cc in _cierreDatos.Values)
			{
				CheckBox chb = cc.check;
				chb.Enabled = esAdmin;
				chb.Checked = !esAdmin;
				chb.Visible = !_modoEdicion;
				cc.bmod.Enabled = esAdmin;
				cc.bmod.Visible = !esAdminAFIP && !_modoEdicion;
			}
			BCerrarTodo.Visible = esAdmin;
		}

		public void Refrescar()
		{
			lblTurno.Text = Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero + 1);
			ParentForm.Text = "Gasolero - Turno: " + lblTurno.Text;
			RefreshLabels();
			Cierre ultCierre;
			for (int i = 0; i < Reglas.CantidadPuestos; i++)
			{
				decimal zero = 0;
				string name = i.ToString();
				ultCierre = Cierre.GetUltimo((uint)(i + 1));
				CCDatos cc = _cierreDatos[name];
				cc.entrada.Valor = ultCierre.Salida;
				cc.salida.Valor = 0;
				cc.diferencia.Text = zero.ToString("0.00");
				cc.diferencia.ForeColor = SystemColors.ControlText;
				cc.venta.Valor = zero;
			}
		}

		void RefreshLabels()
		{
			SortedList<uint, Articulo> arts = new SortedList<uint, Articulo>(10);
			for (int i = 0; i < Reglas.CantidadPuestos; i++)
			{
				uint idArt = Reglas.Puestos[i].IdArticulo;
				if (!arts.ContainsKey(idArt))
					arts.Add(idArt, Articulo.GetSingleE(idArt));
				Articulo artCte = arts[idArt];
				CCDatos cc = _cierreDatos[i.ToString()];
				cc.txt.Text = ((int)(i + 1)).ToString() + " (" + artCte.Codigo + ")";
			}
		}

		class CCDatos
		{
			public Label txt;
			public Label diferencia;
			public Label txtNroCierre;
			public Label modificado;
			public MoneyTextBox entrada;
			public MoneyTextBox salida;
			public MoneyTextBox venta;
			public CheckBox check;
			public Button bmod;
			Articulo _articulo;
			public bool Editando;

			List<Cierre> _cierres;//solo sirve para el modo edicion, se guardan los cierres q se pueden ver

			/// <summary>
			/// Empieza en 1
			/// </summary>
			public int NroCierre = 1;

			public Articulo Articulo
			{
				get { return _articulo; }
				set
				{
					_articulo = value;
					venta.Valor = GetDiferencia() * value.Precio;
				}
			}

			public Cierre CierreActual
			{
				get { return _cierres[NroCierre - 1]; }
			}

			public int CierresCount
			{
				get { return _cierres.Count; }
			}

			public void LoadCierres(Cierre[] cierres)
			{
				_cierres = new List<Cierre>(cierres);
				if (_cierres.Count > 0)
				{
					NroCierre = 1;
				}
				else
				{
					entrada.ReadOnly = true;
					salida.ReadOnly = true;
					this.entrada.Valor = 0;
					this.salida.Valor = 0;
					this.diferencia.Text = "0";
					this.venta.Valor = 0;
				}
				LoadCierre();
			}

			public void LoadCierre()
			{
				if (_cierres.Count == 0)
				{
					txtNroCierre.Text = "- / -";
					return;
				}
				this.Articulo.Precio = CierreActual.Costo;
				this.entrada.Valor = CierreActual.Entrada;
				this.salida.Valor = CierreActual.Salida;
				txtNroCierre.Text = NroCierre.ToString() + " / " + CierresCount.ToString();
			}

			public void CierreSig()
			{
				if (NroCierre < CierresCount)
				{
					NroCierre++;
					LoadCierre();
				}
			}

			public void CierreAnt()
			{
				if (NroCierre > 1)
				{
					NroCierre--;
					LoadCierre();
				}
			}

			public decimal GetDiferencia()
			{
				return decimal.Parse(diferencia.Text);
			}
		}

		private void BTurnoSig_Click(object sender, EventArgs e)
		{
			_nroTurno++;
			if (!ActualizarCierres())
			{
				_nroTurno--;
				ActualizarCierres();
			}
		}

		private void BTurnoAnt_Click(object sender, EventArgs e)
		{
			_nroTurno--;
			if (!ActualizarCierres())
			{
				_nroTurno++;
				ActualizarCierres();
			}
		}

		private void BGuardarCierre_Click(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;
			CCDatos cc = _cierreDatos[ctrl.Name];
			cc.CierreActual.Entrada = cc.entrada.Valor;
			cc.CierreActual.Salida = cc.salida.Valor;
			ABMResultado abmRes = cc.CierreActual.Modificacion();
			if (abmRes.CodigoError != enErrores.Ninguno)
			{
				MessageBox.Show(abmRes.MensajeError, "Ocurrio un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				lblMensaje.Text = "No se modificó el cierre";
			}
			else
			{
				lblMensaje.Text = "Cierre actualizado";
				cc.modificado.Visible = false;
			}
		}

		private void BResetCierre_Click(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;
			CCDatos cc = _cierreDatos[ctrl.Name];
			cc.entrada.Valor = cc.CierreActual.Entrada;
			cc.salida.Valor = cc.CierreActual.Salida;
			cc.modificado.Visible = false;
		}

		bool SeleccionarCierreParcial(uint nroTurno, out uint idParcial)
		{
			bool res = false;
			uint id = 0;
			DataTable dt = Parcial.GetForTurno(nroTurno);
			ABMForm abm = new ABMForm(false, false);//TH ajustado
			abm.ActualizarTabla(dt);
			abm.ActualizarTabla(Parcial.Esquema());
			abm.TablaEsquema = "Esquemas";
			abm.TablaMain = "Parciales";
			abm.Load += delegate(object sender, EventArgs e)
			{
				new System.Threading.Thread(delegate()
				{
					System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
					abm.Invoke((MethodInvoker)delegate
					{
						abm.ReajustarGrilla();
						abm.SeleccionarUltimo();
						abm.Text = "Elija (2xClic) el cierre parcial a visualizar";
						abm.Refresh();
					});
				}).Start();
			};
			abm.TeclaPulsada += delegate(object sender, KeyEventArgs e)
			{
				DataTable dtSeleccion = abm.ObtenerSeleccion();
				if (e.KeyCode == Keys.Enter && dtSeleccion.Rows.Count == 1)
				{
					Parcial op = new Parcial(dtSeleccion.Rows[0]);
					id = op.IdParcial;
					res = true;
					abm.Close();
				}
			};
			abm.ItemDobleClicked += delegate(IABMTool abm2, ItClickEventArgs e)
			{
				DataRow r = e.Item.Row;
				Parcial op = new Parcial(r);
				id = op.IdParcial;
				res = true;
				abm.Close();
			};
			abm.ShowDialog();
			idParcial = id;
			return res;
		}
	}
}
