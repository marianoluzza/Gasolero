using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Negocio;

namespace Gasolero
{
	public partial class CGastos : UserControl
	{
		public CGastos()
		{
			InitializeComponent();
		}

		private void lblFecha_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			dtpFecha.Value = DateTime.Now;
		}

		private void bCargar_Click(object sender, EventArgs e)
		{
			Movimiento mov = new Movimiento();
			mov.Fecha = dtpFecha.Value;
			//
		}
	}
}
