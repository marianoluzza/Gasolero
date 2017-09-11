using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FormCom;
using Negocio;
using MarUtils.Controles;
using MarUtils.Soporte;
using MarUtils.Listados;

namespace Gasolero
{
	public partial class CEntregas : UserControl, IABMAddOn
	{
		FactFiltro _filtro = new FactFiltro(FactFiltro.enFiltrado.EntregaDeCaja);
		bool _datosPendientes = false;
		Control _ultLbl, _ultNum, _lblTotal;
		MoneyTextBox _moneyTotal;
		int _ctCampos = 1;
		SortedList<PagoForma, decimal> _formas = new SortedList<PagoForma, decimal>(10);
		CajaEntrega _caja = null;

		public CEntregas()
		{
			InitializeComponent();
			//
			//1º money
			MoneyTextBox money = new MoneyTextBox();
			money.TabIndex = 0;
			money.ValorCambiado += new EventHandler(Money_ValorCambiado);
			money.Name = "money0";
			money.Size = numericUpDown1.Size;
			money.Location = new Point(numericUpDown1.Location.X, numericUpDown1.Location.Y);
			numericUpDown1.Parent.Controls.Remove(numericUpDown1);
			panelDatos.Controls.Add(money);
			//
			//ultimos campos
			_ultLbl = lbl0;
			_ultNum = money;
			//
			//totales -> label
			_lblTotal = new Label();
			(_lblTotal as Label).AutoSize = true;
			_lblTotal.Text = "Total";
			_lblTotal.Location = new Point(_ultLbl.Location.X, _ultLbl.Location.Y + 26);
			panelDatos.Controls.Add(_lblTotal);
			//
			//totales -> money
			_moneyTotal = new MoneyTextBox();
			_moneyTotal.ReadOnly = true;
			_moneyTotal.TabStop = false;
			_moneyTotal.Name = "moneyTotal";
			_moneyTotal.Size = numericUpDown1.Size;
			_moneyTotal.Location = new Point(numericUpDown1.Location.X, numericUpDown1.Location.Y + 26);
			panelDatos.Controls.Add(_moneyTotal);
			//
			tpFiltro.Controls.Add(_filtro);
		}

		void abm_ItemCorrienteCambiado(object sender, EventArgs e)
		{
			if (tabs.SelectedIndex == 1)
				MostrarDatos();
			else
				_datosPendientes = true;
		}

		private void tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabs.SelectedIndex == 1 && _datosPendientes)
			{
				_datosPendientes = false;
				MostrarDatos();
			}
		}

		void Money_ValorCambiado(object sender, EventArgs e)
		{
			MoneyTextBox m = sender as MoneyTextBox;
			if (m.Tag == null)
				return;
			PagoForma pagoForma = m.Tag as PagoForma;
			_formas[pagoForma] = m.Valor;
			_moneyTotal.Valor = 0;
			foreach (KeyValuePair<PagoForma, decimal> kvp in _formas)
			{
				if (!kvp.Key.SumaAlTotal)
					continue;
				_moneyTotal.Valor += kvp.Value;
			}
		}

		private void bGuardar_Click(object sender, EventArgs e)
		{
			Guardar();
		}

		private void bImprimir_Click(object sender, EventArgs e)
		{
			Imprimir();
		}

		#region IABMAddOn Members
		IABMTool _abm;
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
			get { return "Entregas de Caja"; }
		}

		public void OnFinit(AddOnQuitInfo info)
		{
			_abm.ItemCorrienteCambiado -= new EventHandler(abm_ItemCorrienteCambiado);
			_filtro.OnFinit(info);
		}

		public void OnInit(IABMTool abm)
		{
			_abm = abm;
			abm.ItemCorrienteCambiado += new EventHandler(abm_ItemCorrienteCambiado);
			_filtro.OnInit(abm);
		}

		public ABMAddOnPosicion Posicion
		{
			get { return ABMAddOnPosicion.PanelDerecho; }
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			return new Size(190, 266);
		}

		public override string ToString()
		{
			return Nombre;
		}
		#endregion

		void InitMasCtrl()
		{
			DataTable dt = PagoForma.GetAll();
			bGuardar.TabIndex = dt.Rows.Count + 10;
			if (dt.Rows.Count == _ctCampos)
			{
				//nadas
			}
			else if (dt.Rows.Count > _ctCampos)
			{
				for (; _ctCampos < dt.Rows.Count; _ctCampos++)
				{
					Label lbl = new Label();
					lbl.Name = "lbl" + _ctCampos;
					lbl.AutoSize = true;
					lbl.Location = new Point(_ultLbl.Location.X, _ultLbl.Location.Y + 26);
					MoneyTextBox money = new MoneyTextBox();
					money.TabIndex = _ctCampos;
					money.ValorCambiado += new EventHandler(Money_ValorCambiado);
					money.Name = "money" + _ctCampos;
					money.Size = _ultNum.Size;
					money.Location = new Point(_ultNum.Location.X, _ultNum.Location.Y + 26);
					//
					_ultLbl = lbl;
					_ultNum = money;
					panelDatos.Controls.Add(lbl);
					panelDatos.Controls.Add(money);
				}
			}
			else
			{
				for (int i = _ctCampos; i > dt.Rows.Count; i--)
				{
					panelDatos.Controls["lbl" + (i - 1)].Visible = false;
					panelDatos.Controls["money" + (i - 1)].Visible = false;
				}
			}
			_moneyTotal.Location = new Point(_moneyTotal.Location.X, numericUpDown1.Location.Y + 26 * dt.Rows.Count);
			_lblTotal.Location = new Point(_lblTotal.Location.X, lbl0.Location.Y + 26 * dt.Rows.Count);
			bGuardar.Location = new Point(bGuardar.Location.X, numericUpDown1.Location.Y + 26 * (dt.Rows.Count + 1));
		}

		public void MostrarDatos()
		{
			Turno t;
			if (_abm.ItemActual == null)
				t = new Turno();
			else
				t = new Turno(_abm.ItemActual.Row);
			//
			InitMasCtrl();
			_caja = CajaEntrega.GetSingleForTurno(t.IdTurno);
			_formas.Clear();
			int i = 0;
			MoneyTextBox money;
			Label lbl;
			if (_caja != null)
			{
				bGuardar.Enabled = Reglas.VendedorActual.TienePermiso(enPermisos.SuperUsuario);
				bImprimir.Enabled = true;
				Vendedor vdor = Vendedor.GetSingleE(_caja.IdVendedor);
				lblVendedor.Text = "Realizada por " + vdor.Nombre;
				lblFecha.Text = _caja.Fecha.ToShortDateString() + " " + _caja.Fecha.ToShortTimeString();
				_caja.GetFilas();
				foreach (CajaEntregaItem it in _caja.Coleccion)
				{
					PagoForma pf = PagoForma.GetSingleE(it.IdPagoForma);
					_formas.Add(pf, it.Monto);
					lbl = panelDatos.Controls["lbl" + i] as Label;
					lbl.Text = pf.Descripcion;
					lbl.Font = new Font(lbl.Font, pf.Habilitado ? FontStyle.Regular : FontStyle.Italic);
					money = panelDatos.Controls["money" + i] as MoneyTextBox;
					money.Tag = pf;
					money.Valor = it.Monto;
					i++;
				}
			}
			else
			{
				bImprimir.Enabled = false;
				bGuardar.Enabled = t.IdTurno > 0;
				lblVendedor.Text = "Sin realizar entrega";
				lblFecha.Text = "--/--/-- --:--";
			}
			DataTable dtFormas = PagoForma.GetAll();
			foreach (DataRow r in dtFormas.Rows)
			{
				PagoForma pf = new PagoForma(r);
				if (_formas.ContainsKey(pf))
					continue;
				_formas.Add(pf, 0);
				lbl = panelDatos.Controls["lbl" + i] as Label;
				lbl.Text = pf.Descripcion;
				lbl.Font = new Font(lbl.Font, pf.Habilitado ? FontStyle.Regular : FontStyle.Italic);
				lbl.Visible = true;
				money = panelDatos.Controls["money" + i] as MoneyTextBox;
				money.Tag = pf;
				money.Valor = 0;
				money.Visible = true;
				i++;
			}
		}

		public void Guardar()
		{
			SortedList<uint, CajaEntregaItem> pf_item = new SortedList<uint, CajaEntregaItem>();
			if (_caja != null)
			{
				foreach (CajaEntregaItem it in _caja.Coleccion)
				{
					pf_item.Add(it.IdPagoForma, it);
				}
			}
			else
			{
				_caja = new CajaEntrega();
				_caja.Fecha = Reglas.Now;
				_caja.IdTurno = uint.Parse(_abm.ItemActual["IdTurno"].ToString());
			}
			_caja.IdVendedor = Reglas.VendedorActual.IdVendedor;
			foreach (KeyValuePair<PagoForma, decimal> kvp in _formas)
			{
				//si es viejo y 0 => no lo cuento
				if (!pf_item.ContainsKey(kvp.Key.IdPagoForma) && kvp.Value == 0)
					continue;
				CajaEntregaItem it;
				if (pf_item.ContainsKey(kvp.Key.IdPagoForma))
				{
					it = pf_item[kvp.Key.IdPagoForma];
				}
				else
				{
					it = new CajaEntregaItem();
					it.IdPagoForma = kvp.Key.IdPagoForma;
					_caja.Coleccion.Add(it);
				}
				it.Monto = kvp.Value;
			}
			ABMResultado abmRes;
			if (_caja.IdCajaEntrega == 0)
				abmRes = _caja.Alta();
			else
				abmRes = _caja.Modificacion();
			if (abmRes.CodigoError == enErrores.Ninguno)
			{
				MostrarDatos();
				MessageBox.Show("Datos guardados correctamente", "Entrega de Caja", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
				MessageBox.Show(abmRes.MensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public void Imprimir()
		{
			ListView lista = new ListView();
			List<ListadoEsquema> esquemas = new List<ListadoEsquema>();
			ListadoBinding bind;
			ListadoEsquema esq = new ListadoEsquema();
			esq.Dato = "Forma de Pago";
			esq.Alineacion = "I";
			esq.Posicion = 1;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Monto";
			esq.Alineacion = "D";
			esq.Posicion = 2;
			esquemas.Add(esq);
			//
			bind = new ListadoBinding(lista, esquemas);
			bind.DatoGrupo = "SumaAlTotal";
			bind.Grupos.Add(new ListadoGrupo("Formas de Pago que Suman al Total", true.ToString()));
			bind.Grupos.Add(new ListadoGrupo("Otras Formas de Pago", false.ToString()));
			DataTable dtEntregas = new DataTable("Entregas");
			dtEntregas.Columns.Add("Forma de Pago");
			dtEntregas.Columns.Add("Monto");
			dtEntregas.Columns.Add("SumaAlTotal");
			dtEntregas.Columns.Add("Habilitado");
			dtEntregas.Columns.Add("Orden");
			foreach (KeyValuePair<PagoForma, decimal> kvp in _formas)
			{
				dtEntregas.Rows.Add(kvp.Key.Descripcion, decimal.Round(kvp.Value, 2).ToString("0.00"), kvp.Key.SumaAlTotal, kvp.Key.Habilitado, kvp.Key.Orden);
			}
			bind.DatosTotalizadores["Forma de Pago"] = "TOTAL";
			bind.DatosTotalizadores["Monto"] = "@Monto";
			//
			bind.GrupoTotalizador.Nombre = "Resumen";
			bind.GrupoTotalizador.Clave = "Resumen";
			bind.GrupoTotalizadorDatos.AddFila(0);
			bind.GrupoTotalizadorDatos.AddFila(1);
			bind.GrupoTotalizadorDatos.AddFila(2);
			//
			bind.GrupoTotalizadorDatos["Forma de Pago", 0] = "Fecha";
			bind.GrupoTotalizadorDatos["Monto", 0] = _caja.Fecha.ToString();
			//
			bind.GrupoTotalizadorDatos["Forma de Pago", 1] = "Entrega";
			bind.GrupoTotalizadorDatos["Monto", 1] = Reglas.VendedorActual.Nombre;
			//
			bind.GrupoTotalizadorDatos["Forma de Pago", 2] = "Firma";
			bind.GrupoTotalizadorDatos["Monto", 2] = Environment.NewLine;
			//
			dtEntregas.DefaultView.Sort = "Orden";
			bind.ActualizarTabla(dtEntregas.DefaultView.ToTable());
			bind.TablaMain = "Entregas";
			bind.ResetBinding();
			//lista.Dock = DockStyle.Fill;
			lista.View = View.Details;
			//PanelListado.Controls.Add(lista);
			bind.AutoResizeLista();
			lista.Columns[0].Width = 400;
			lista.Columns[1].Width = 150;
			ListViewPrinterBase printer = new ListViewPrinterBase();
			printer.Header = "Entrega de Caja";
			printer.ListView = lista;
			printer.Print();
		}

		public void Filtrar(DateTime dde, DateTime hta)
		{
			_filtro.Filtrar(dde, hta, CheckState.Indeterminate);
		}
	}
}
