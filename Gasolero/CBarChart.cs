using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BarChart;
using Negocio;

namespace Gasolero
{
	public partial class CBarChart : UserControl
	{
		HBarChart _grafico;
		List<Color> _colores;
		int _cantidad = 5;

		public CBarChart()
		{
			InitializeComponent();
			_grafico = new HBarChart();
			_grafico.Dock = DockStyle.Fill;
			_grafico.SizingMode = HBarChart.BarSizingMode.Normal;
			_grafico.Values.Mode = CValueProperty.ValueMode.Digit;
			_grafico.Background.PaintingMode = CBackgroundProperty.PaintingModes.LinearGradient;
			panelGraf.Controls.Add(_grafico);
			radClientes.Checked = true;
			radVendedores.Checked = true;
			_colores = new List<Color>(10);
			_colores.Add(Color.Blue);
			_colores.Add(Color.Red);
			_colores.Add(Color.DarkGreen);
			_colores.Add(Color.Yellow);
			_colores.Add(Color.Turquoise);
			_colores.Add(Color.Silver);
			_colores.Add(Color.SandyBrown);
			_colores.Add(Color.Plum);
			_colores.Add(Color.PapayaWhip);
			_colores.Add(Color.Orange);
			//
		}

		private void trackBarWidthBar_ValueChanged(object sender, EventArgs e)
		{
			chbAjustarAncho.Checked = false;
			_grafico.BarWidth = trackBarWidthBar.Value;
			_grafico.RedrawChart();
		}

		private void chbAjustarAncho_CheckedChanged(object sender, EventArgs e)
		{
			_grafico.SizingMode = chbAjustarAncho.Checked ? HBarChart.BarSizingMode.AutoScale : HBarChart.BarSizingMode.Normal;
			_grafico.RedrawChart();
		}

		private void chValoresPorcentuales_CheckedChanged(object sender, EventArgs e)
		{
			_grafico.Values.Mode = chValoresPorcentuales.Checked ? CValueProperty.ValueMode.Percent : CValueProperty.ValueMode.Digit;
			_grafico.RedrawChart();
		}

		private void radVendedores_CheckedChanged(object sender, EventArgs e)
		{
			gbVendedores.Enabled = radVendedores.Checked;
		}

		private void radClientes_CheckedChanged(object sender, EventArgs e)
		{
			gbClientes.Enabled = radClientes.Checked;
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

		private void bGenerar_Click(object sender, EventArgs e)
		{
			while (_grafico.Items.Count > 0)
				_grafico.RemoveAt(0);
			_grafico.RedrawChart();
			if (radClientes.Checked)
			{
				if (radMejCompras.Checked)
					GenerarMejoresCompras();
				else if (radCtCompras.Checked)
					GenerarCantidadCompras();
			}
			else if (radVendedores.Checked)
			{
				if (radMejPropinas.Checked)
					GenerarMejoresPropinas();
				else if (radMejCierres.Checked)
					GenerarMejoresCierres();
				else if (radMejVentas.Checked)
					GenerarMejoresVentas();
			}
		}

		void GenerarMejoresCompras()
		{
			_grafico.Description.Text = "Mejores Compras";
			try
			{
				Cursor = Cursors.WaitCursor;
				DataTable dt = FactEncabezado.GetListForFechas(dtpDesde.Value.Date, dtpHasta.Value.Date);
				SortedList<uint, decimal> compras = new SortedList<uint, decimal>();
				foreach (DataRow r in dt.Rows)
				{
					FactEncabezado f = new FactEncabezado(r);
					if (f.IdCliente == Cliente.ConsFinal.Id)
						continue;
					f.GetFilas();
					if (compras.ContainsKey(f.IdCliente))
						compras[f.IdCliente] += f.Total;
					else
						compras.Add(f.IdCliente, f.Total);
				}
				List<KeyValuePair<uint, decimal>> valoresOrdenados = new List<KeyValuePair<uint, decimal>>(compras.Count);
				foreach (KeyValuePair<uint, decimal> kvp in compras)
				{
					valoresOrdenados.Add(kvp);
				}
				valoresOrdenados.Sort(new Comparador<decimal>());
				for (int i = 0; i < _cantidad && i < valoresOrdenados.Count; i++)
				{
					KeyValuePair<uint, decimal> kvp = valoresOrdenados[valoresOrdenados.Count - 1 - i];
					Cliente c = Cliente.GetSingleE(kvp.Key);
					double valor = kvp.Value > 0 ? (double)kvp.Value : 0.001;
					_grafico.Add(valor, c.Nombre, _colores[i % _colores.Count]);
				}
				_grafico.RedrawChart();
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		void GenerarCantidadCompras()
		{
			_grafico.Description.Text = "Cantidad de Compras";
			try
			{
				Cursor = Cursors.WaitCursor;
				DataTable dt = FactEncabezado.GetListForFechas(dtpDesde.Value.Date, dtpHasta.Value.Date);
				SortedList<uint, int> compras = new SortedList<uint, int>();
				foreach (DataRow r in dt.Rows)
				{
					FactEncabezado f = new FactEncabezado(r);
					if (f.IdCliente == Cliente.ConsFinal.Id)
						continue;
					if (compras.ContainsKey(f.IdCliente))
						compras[f.IdCliente] += 1;
					else
						compras.Add(f.IdCliente, 1);
				}
				List<KeyValuePair<uint, int>> valoresOrdenados = new List<KeyValuePair<uint, int>>(compras.Count);
				foreach (KeyValuePair<uint, int> kvp in compras)
				{
					valoresOrdenados.Add(kvp);
				}
				valoresOrdenados.Sort(new Comparador<int>());
				for (int i = 0; i < _cantidad && i < valoresOrdenados.Count; i++)
				{
					KeyValuePair<uint, int> kvp = valoresOrdenados[valoresOrdenados.Count - 1 - i];
					Cliente c = Cliente.GetSingleE(kvp.Key);
					double valor = kvp.Value > 0 ? (double)kvp.Value : 0.001;
					_grafico.Add(valor, c.Nombre, _colores[i % _colores.Count]);
				}
				_grafico.RedrawChart();
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		void GenerarMejoresVentas()
		{
			_grafico.Description.Text = "Mejores Ventas";
			try
			{
				Cursor = Cursors.WaitCursor;
				DataTable dt = FactEncabezado.GetListForFechas(dtpDesde.Value.Date, dtpHasta.Value.Date);
				SortedList<uint, decimal> ventas = new SortedList<uint, decimal>();
				foreach (DataRow r in dt.Rows)
				{
					FactEncabezado f = new FactEncabezado(r);
					f.GetFilas();
					if (ventas.ContainsKey(f.IdVendedor))
						ventas[f.IdVendedor] += f.Total;
					else
						ventas.Add(f.IdVendedor, f.Total);
				}
				List<KeyValuePair<uint, decimal>> valoresOrdenados = new List<KeyValuePair<uint, decimal>>(ventas.Count);
				foreach (KeyValuePair<uint, decimal> kvp in ventas)
				{
					valoresOrdenados.Add(kvp);
				}
				valoresOrdenados.Sort(new Comparador<decimal>());
				for (int i = 0; i < _cantidad && i < valoresOrdenados.Count; i++)
				{
					KeyValuePair<uint, decimal> kvp = valoresOrdenados[valoresOrdenados.Count - 1 - i];
					Vendedor v = Vendedor.GetSingleE(kvp.Key);
					double valor = kvp.Value > 0 ? (double)kvp.Value : 0.001;
					_grafico.Add(valor, v.Nombre, _colores[i % _colores.Count]);
				}
				_grafico.RedrawChart();
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		void GenerarMejoresPropinas()
		{
			_grafico.Description.Text = "Mejores propinas";
			try
			{
				Cursor = Cursors.WaitCursor;
				DataTable dt = Turno.GetForFechas(dtpDesde.Value.Date, dtpHasta.Value.Date);
				SortedList<uint, decimal> propinas = new SortedList<uint, decimal>();
				foreach (DataRow r in dt.Rows)
				{
					Turno t = new Turno(r);
					if (propinas.ContainsKey(t.IdVendedor))
						propinas[t.IdVendedor] += t.Propina;
					else
						propinas.Add(t.IdVendedor, t.Propina);
				}
				List<KeyValuePair<uint, decimal>> valoresOrdenados = new List<KeyValuePair<uint, decimal>>(propinas.Count);
				foreach (KeyValuePair<uint, decimal> kvp in propinas)
				{
					valoresOrdenados.Add(kvp);
				}
				valoresOrdenados.Sort(new Comparador<decimal>());
				for (int i = 0; i < _cantidad && i < valoresOrdenados.Count; i++)
				{
					KeyValuePair<uint, decimal> kvp = valoresOrdenados[valoresOrdenados.Count - 1 - i];
					Vendedor v = Vendedor.GetSingleE(kvp.Key);
					double valor = kvp.Value > 0 ? (double)kvp.Value : 0.001;
					_grafico.Add(valor, v.Nombre, _colores[i % _colores.Count]);
				}
				_grafico.RedrawChart();
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		void GenerarMejoresCierres()
		{
			_grafico.Description.Text = "Mejores cierres";
			try
			{
				Cursor = Cursors.WaitCursor;
				DataTable dt = Turno.GetForFechas(dtpDesde.Value.Date, dtpHasta.Value.Date);
				SortedList<uint, decimal> cierres = new SortedList<uint, decimal>();
				foreach (DataRow r in dt.Rows)
				{
					Turno t = new Turno(r);
					for (uint surtidor = 1; surtidor <= 10; surtidor++)
					{
						foreach (Cierre c in Cierre.GetListaXSurtidorTurno(surtidor, t.Numero))
						{
							if (cierres.ContainsKey(t.IdVendedor))
								cierres[t.IdVendedor] += c.GetDiferencia() * c.Costo;
							else
								cierres.Add(t.IdVendedor, c.GetDiferencia() * c.Costo);
						}
					}
				}
				List<KeyValuePair<uint, decimal>> valoresOrdenados = new List<KeyValuePair<uint, decimal>>(cierres.Count);
				foreach (KeyValuePair<uint, decimal> kvp in cierres)
				{
					valoresOrdenados.Add(kvp);
				}
				valoresOrdenados.Sort(new Comparador<decimal>());
				for (int i = 0; i < _cantidad && i < valoresOrdenados.Count; i++)
				{
					KeyValuePair<uint, decimal> kvp = valoresOrdenados[valoresOrdenados.Count - 1 - i];
					Vendedor v = Vendedor.GetSingleE(kvp.Key);
					_grafico.Add((double)kvp.Value, v.Nombre, _colores[i % _colores.Count]);
				}
				_grafico.RedrawChart();
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		class Comparador<TValue> : IComparer<KeyValuePair<uint, TValue>> where TValue : IComparable<TValue>
		{
			#region IComparer<KeyValuePair<uint,TValue>> Members

			public int Compare(KeyValuePair<uint, TValue> x, KeyValuePair<uint, TValue> y)
			{
				return x.Value.CompareTo(y.Value);
			}

			#endregion
		}
	}
}
