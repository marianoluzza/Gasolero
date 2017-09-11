using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using X.Listados;
using MarUtils.Listados;
using Negocio;

namespace Gasolero
{
	public partial class LVVisor : Form
	{
		ListView lista = new ListView();
		ListadoBinding bind = null;
		
		bool _previsualizar = false;
		public bool Previsualizar
		{
			get { return _previsualizar; }
			set
			{
				_previsualizar = value;
				BImprimir.Text = value ? "Cerrar" : "Imprimir";
			}
		}

		public void CargarRpt(RptCCMov rpt)
		{
			lista = rpt.Lista;
			bind = rpt.Bind;
			lista.Dock = DockStyle.Fill;
			lista.View = View.Details;
			PanelListado.Controls.Add(lista);
			//bind.AutoResizeLista();
			Ajustar();
		}

		public void Ajustar()
		{
			//anchos de planilla TT Max 627.0
			lista.Columns[0].Width = 75;
			lista.Columns[1].Width = 106;
			lista.Columns[2].Width = 95;
			lista.Columns[3].Width = 101;
			lista.Columns[4].Width = 170;
			lista.Columns[5].Width = 80;
		}

		public LVVisor()
		{
			InitializeComponent();
		}

		private void BImprimir_Click(object sender, EventArgs e)
		{
			if (Previsualizar)
			{
				Close();
			}
			else
			{
				Imprimir();
			}
		}

		public void Imprimir()
		{
			MarUtils.Listados.ListViewPrinterBase p = new MarUtils.Listados.ListViewPrinterBase();
			//X.Listados.ListViewPrinterBase p = new X.Listados.ListViewPrinterBase();
			p.ListView = lista;
			p.Header = Text;
			p.HeaderFormat.Font = new System.Drawing.Font(p.HeaderFormat.Font.FontFamily, 16);
			p.HeaderFormat.TextColor = Color.Black;
			p.HeaderFormat.BackgroundColor = Color.White;
			p.Print();
		}

		private void LVVisor_Load(object sender, EventArgs e)
		{
			
		}

		private void bAjustar_Click(object sender, EventArgs e)
		{
			Ajustar();
		}
	}
}