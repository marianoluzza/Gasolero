using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using FormCom;
using System.Windows.Forms;
using Negocio;

namespace Gasolero
{
	public partial class AccessQuery : Form
	{
		ABMGrilla operaciones;

		public AccessQuery()
		{
			InitializeComponent();
			operaciones = new ABMGrilla(false, false);
			operaciones.ActualizarTabla(Operacion.Esquema());
			operaciones.ActualizarTabla(Operacion.GetVales(DateTime.Today, DateTime.Today.AddDays(1)));
			operaciones.ActualizarTabla(Vehiculo.GetConsumidores());
			operaciones.ActualizarTabla(Articulo.GetAll());
			operaciones.TablaEsquema = "Esquemas";
			operaciones.TablaMain = "Operaciones";
			operaciones.TerminacionSolicitada += delegate(object s2, EventArgs e2)
			{
				Close();
			};
			operaciones.Load += delegate(object sender, EventArgs e)
			{
				operaciones.ReajustarGrilla();
			};
			operaciones.ItemDobleClicked += new ItemClickedEventHandler(ABM_ItemDoubleClick);
			operaciones.Dock = DockStyle.Fill;
			pnlABM.Controls.Add(operaciones);
		}

		DataTable Tabla()
		{
			DataTable dt = new DataTable("Operaciones");
			dt.Columns.Add("IdOperacion", typeof(uint));
			dt.Columns.Add("Fecha", typeof(DateTime));
			dt.Columns.Add("CodProducto", typeof(uint));
			dt.Columns.Add("IdOperacion", typeof(uint));
			dt.Columns.Add("IdOperacion", typeof(uint));
			return dt;
		}

		private void bBuscarVales_Click(object sender, EventArgs e)
		{
			try
			{
				DataTable dtops = Operacion.GetVales(dtpValesDde.Value, dtpValesHta.Value);
				operaciones.ActualizarTabla(dtops);
				operaciones.ReajustarGrilla();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void bBuscarPorId_Click(object sender, EventArgs e)
		{
			try
			{
				operaciones.ActualizarTabla(Operacion.GetVales((uint)numId.Value));
				operaciones.ReajustarGrilla();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ABM_ItemDoubleClick(IABMTool abm, ItClickEventArgs e)
		{
			try
			{
				DataRow r = e.Item.Row;
				if (MessageBox.Show("¿Imprimir vale " + r[0].ToString() + "?", "Imprimir vale", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					!= System.Windows.Forms.DialogResult.Yes)
					return;
				Operacion op = new Operacion(r);
				Fiscal.ImprimirVale(op);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
		}
	}
}
