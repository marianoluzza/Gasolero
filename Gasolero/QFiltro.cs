using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FormCom;

namespace Gasolero
{
	public partial class QFiltro : UserControl, IABMAddOn
	{
		IABMTool _abm;
		string _dato;
		ToolStripItem _msnIt;

		public event EventHandler SeleccionAceptada;
		public event EventHandler SeleccionCancelada;

		public QFiltro(string dato)
		{
			InitializeComponent();
			_dato = dato;
			lbl.Text = dato;
			lblMensaje.Text = "";
			_msnIt = new ToolStripLabel("");
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
			get { return "Filtro Rapido"; }
		}

		public void OnFinit(AddOnQuitInfo info)
		{
			_abm.RemoverMenu(_msnIt, enMenuPosicion.BarraEstado);
		}

		public void OnInit(IABMTool abm)
		{
			_abm = abm;
			_abm.MultiSeleccion = false;
			_abm.AgregarMenu(_msnIt, enMenuPosicion.BarraEstado);
		}

		public ABMAddOnPosicion Posicion
		{
			get { return ABMAddOnPosicion.PanelDerecho; }
		}

		#endregion

		Keys _teclaBM = Keys.F3;
		Keys _teclaAlta = Keys.F2;

		public Keys TeclaAlta
		{
			get { return _teclaAlta; }
			set { _teclaAlta = value; }
		}

		public Keys TeclaBM
		{
			get { return _teclaBM; }
			set { _teclaBM = value; }
		}

		bool _permitirABM = false;

		public bool PermitirABM
		{
			get { return _permitirABM; }
			set 
			{
				_permitirABM = value;
				lblMensaje.Text = value ? TeclaAlta.ToString() + " > Nuevo ||  " + TeclaBM.ToString() + " > Modificar" : "";
			}
		}

		enComodin _comodines = enComodin.Ambos;

		public enComodin Comodines
		{
			get { return _comodines; }
			set { _comodines = value; }
		}

		public string TextoFiltro
		{
			get { return TBFiltro.Text; }
			set { TBFiltro.Text = value; }
		}

		private void TBFiltro_TextChanged(object sender, EventArgs e)
		{
			string texto = TBFiltro.Text;
			switch (Comodines)
			{ 
				case enComodin.Ambos:
					texto = "*" + texto + "*";
					break;
				case enComodin.Final:
					texto += "*";
					break;
			}
			if (TBFiltro.Text != "")
				_abm.Filtro = _dato + " LIKE '" + texto + "'";
			else
				_abm.Filtro = "";
		}

		private void TBFiltro_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter && SeleccionAceptada != null)
				SeleccionAceptada(this, e);
			else if (e.KeyData == Keys.Escape && SeleccionCancelada != null)
				SeleccionCancelada(this, e);
			else if (e.KeyValue == 38)
				ABMTool.SeleccionarAnterior(true);
			else if (e.KeyValue == 40)
				ABMTool.SeleccionarSiguiente(true);
			else if (e.KeyValue == 36)
				ABMTool.SeleccionarPrimero();
			else if (e.KeyValue == 35)
				ABMTool.SeleccionarUltimo();
			else if ((e.KeyData == TeclaBM || e.KeyData == TeclaAlta) && PermitirABM)
			#region Abrir ABM
			{
				ABMDatos data = _abm.ObtenerAddOn<ABMDatos>();
				_abm.ActivarAddOn(data.GetType());
				_msnIt.Text = "Atajos ABM: Alt + I = Ingresar; Alt + A = Actualizar; Alt + E = Eliminar; Alt + L = Limpiar";
				data.Focus();
				if (e.KeyData == TeclaAlta)
				{
					data.Limpiar();
					try
					{
						data.SetDato("Nombre", TBFiltro.Text);
						data.Focus("Nombre");
					}
					catch { }
				}
			}
			#endregion
		}

		private void QFiltro_Load(object sender, EventArgs e)
		{
			System.Threading.Thread th = new System.Threading.Thread(
				new System.Threading.ThreadStart(SFocus));
			th.Start();
			_msnIt.Text = "Enter = Acepta; Esc = Cancela";
		}

		void SFocus()
		{
			System.Threading.Thread.Sleep(500);
			if (TBFiltro.InvokeRequired)
			{
				TBFiltro.Invoke((MethodInvoker)delegate { TBFiltro.Focus(); });
			}
			else
			{
				TBFiltro.Focus();
			}
		}

		private void BAceptar_Click(object sender, EventArgs e)
		{
			if (SeleccionAceptada != null)
				SeleccionAceptada(this, e);
		}

		private void BCancelar_Click(object sender, EventArgs e)
		{
			if (SeleccionCancelada != null)
				SeleccionCancelada(this, e);
		}

		public enum enComodin
		{
			Ninguno,
			Final,
			Ambos
		}	
	}
}
