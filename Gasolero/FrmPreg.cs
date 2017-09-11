using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gasolero
{
	public partial class FrmPreg : Form
	{
		public enum enOpciones
		{ 
			A,
			B,
			Ninguna
		}

		public enOpciones Opcion { get; set; }

		public FrmPreg(string opc1="Cierre de Turno", string opc2="Cierre Parcial", string header = "?")
		{
			InitializeComponent();
			bOpc1.Text = opc1;
			bOpc2.Text = opc2;
			Text = header;
			Opcion = enOpciones.Ninguna;
		}

		private void bOpc1_Click(object sender, EventArgs e)
		{
			Opcion = enOpciones.A;
			Close();
		}

		private void bOpc2_Click(object sender, EventArgs e)
		{
			Opcion = enOpciones.B;
			Close();
		}
	}
}
