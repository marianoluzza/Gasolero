using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using X.Listados;
using MarUtils.Listados;
using System.Data;
using Negocio;

namespace Gasolero
{
	public class RptCCMov
	{
		ListView lista = new ListView();
		public ListView Lista
		{
			get
			{
				return lista;
			}
		}
		List<ListadoEsquema> esquemas = new List<ListadoEsquema>();
		public ListadoBinding Bind
		{
			get
			{
				return bind;
			}
		}
		ListadoBinding bind;
		DataTable dtCCMov;
		DateTime _desde, _hasta;

		public RptCCMov(DateTime dde, DateTime hta)
		{
			_desde = dde;
			_hasta = hta;
			DataTable movimientos = Negocio.CCMovimiento.GetMovimientosDetallados(dde, hta);
			ListadoEsquema esq = new ListadoEsquema();
			esq.Dato = "Fecha";
			esq.Alineacion = "I";
			esq.Posicion = 1;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Cliente";
			esq.Alineacion = "I";
			esq.Posicion = 2;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Vendedor";
			esq.Alineacion = "I";
			esq.Posicion = 3;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Factura";
			esq.Alineacion = "I";
			esq.Posicion = 4;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Descripcion";
			esq.Alineacion = "I";
			esq.Posicion = 5;
			esquemas.Add(esq);
			//esq = new ListadoEsquema();
			//esq.Dato = "Forma";
			//esq.Alineacion = "I";
			//esq.Posicion = 6;
			//esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Monto";
			esq.Alineacion = "D";
			esq.Posicion = 6;
			esquemas.Add(esq);
			//
			bind = new ListadoBinding(lista, esquemas);
			bind.DatoGrupo = "Tipo";
			dtCCMov = new DataTable("CCMovimientos");
			dtCCMov.Columns.Add("Fecha");
			dtCCMov.Columns.Add("Cliente");
			dtCCMov.Columns.Add("Vendedor");
			dtCCMov.Columns.Add("Factura");
			dtCCMov.Columns.Add("Descripcion");
			//dtCCMov.Columns.Add("Forma");
			dtCCMov.Columns.Add("Monto");
			dtCCMov.Columns.Add("Tipo");
			//
			bind.Grupos.Add(new ListadoGrupo("Ingresos", "I"));
			bind.Grupos.Add(new ListadoGrupo("Ingresos Tarjeta", "IT"));
			bind.Grupos.Add(new ListadoGrupo("Egresos", "E"));
			//bind.Grupos.Add(new ListadoGrupo("I", "Ingresos"));
			uint pfT = Negocio.Reglas.PagoTarjeta, pfCC = Negocio.Reglas.PagoCC;
			foreach (DataRow r in movimientos.Rows)
			{
				//Negocio.CCMovimiento cc = new Negocio.CCMovimiento(r);
				string d = r["Descripcion"].ToString(), d2 = r["NombreAutorizado"].ToString() + " " + r["DNIAutorizado"].ToString() + " " + r["PatenteAutorizado"].ToString();
				if (d2.Trim().Length > 0)
					d += " - " + d2.Trim();
				decimal monto = decimal.Parse(r["Monto"].ToString());
				uint pf = uint.Parse(r["IdPagoForma"].ToString());
				dtCCMov.Rows.Add(r["Fecha"], r["Cliente"], r["Vendedor"], r["Factura"], d, monto/*.ToString("$ 0.00")*/, (monto < 0 ? "E" : (pf == pfT ? "IT" : "I")));
			}
			#region Grupo Totalizador
			bind.DatosTotalizadores["Descripcion"] = "TOTAL";
			bind.DatosTotalizadores["Monto"] = "@Monto";
			//
			bind.GrupoTotalizador.Nombre = "Resumen";
			bind.GrupoTotalizador.Clave = "Resumen";
			bind.GrupoTotalizadorDatos.AddFila(0);
			bind.GrupoTotalizadorDatos.AddFila(1);
			bind.GrupoTotalizadorDatos.AddFila(2);
			//bind.GrupoTotalizadorDatos.AddFila(3);
			//bind.GrupoTotalizadorDatos.AddFila(4);
			//
			var ccResumen = Negocio.CCMovimiento.GetResumen(dde, hta);
			decimal ccCredito = decimal.Parse(ccResumen.Rows[0]["Credito"].ToString());
			decimal ccDebito = decimal.Parse(ccResumen.Rows[0]["Debito"].ToString());
			decimal ccSaldo = decimal.Parse(ccResumen.Rows[0]["Saldo"].ToString());
			decimal ccCreditoTarjeta = decimal.Parse(ccResumen.Rows[0]["CreditoTarjeta"].ToString());
			//
			int filaGTotal = 0;
			bind.GrupoTotalizadorDatos["Fecha", filaGTotal] = "Ingresos";
			bind.GrupoTotalizadorDatos["Cliente", filaGTotal] = ccCredito.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Vendedor", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Factura", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Descripcion", filaGTotal] = "Ingreso Tarjeta";
			bind.GrupoTotalizadorDatos["Monto", filaGTotal] = ccCreditoTarjeta.ToString("$ 0.00");
			filaGTotal++;
			//
			bind.GrupoTotalizadorDatos["Fecha", filaGTotal] = "Egresos";
			bind.GrupoTotalizadorDatos["Cliente", filaGTotal] = Math.Abs(ccDebito).ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Vendedor", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Factura", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Descripcion", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Monto", filaGTotal] = "";
			filaGTotal++;
			//
			bind.GrupoTotalizadorDatos["Fecha", filaGTotal] = "Saldo";
			bind.GrupoTotalizadorDatos["Cliente", filaGTotal] = ccSaldo.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Vendedor", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Factura", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Descripcion", filaGTotal] = "Saldo Efectivo";
			bind.GrupoTotalizadorDatos["Monto", filaGTotal] = (ccSaldo - ccCreditoTarjeta).ToString("$ 0.00");
			filaGTotal++;
			//
			//bind.GrupoTotalizadorDatos["Fecha", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Cliente", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Vendedor", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Factura", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Descripcion", filaGTotal] = "Saldo";
			//bind.GrupoTotalizadorDatos["Monto", filaGTotal] = ccSaldo.ToString("$ 0.00");
			//filaGTotal++;
			////
			//bind.GrupoTotalizadorDatos["Fecha", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Cliente", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Vendedor", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Factura", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Descripcion", filaGTotal] = "Saldo S/T";
			//bind.GrupoTotalizadorDatos["Monto", filaGTotal] = "";
			//filaGTotal++;
			#endregion
			//
			bind.ActualizarTabla(dtCCMov);
			bind.TablaMain = "CCMovimientos";
			bind.ResetBinding();
			lista.Dock = DockStyle.Fill;
			lista.View = View.Details;
			//PanelListado.Controls.Add(lista);
			bind.AutoResizeLista();
			//Ajustar();
		}

		void Ajustar()
		{
			//anchos de planilla
			lista.Columns[0].Width = 80;
			lista.Columns[1].Width = 90;
			lista.Columns[2].Width = 90;
			lista.Columns[3].Width = 90;
			lista.Columns[4].Width = 90;
			lista.Columns[5].Width = 80;
			//lista.Columns[6].Width = 60;
			lista.Columns[6].Width = 40;
		}

		public void Imprimir()
		{
			MarUtils.Listados.ListViewPrinterBase p = new MarUtils.Listados.ListViewPrinterBase();
			//p.Watermark = Reglas.MarcaParcial;
			//p.WatermarkTransparency = 25;
			p.HeaderFormat = new BlockFormat();
			p.HeaderFormat.BackgroundColor = System.Drawing.Color.White;
			p.HeaderFormat.Font = new System.Drawing.Font(lista.Font.FontFamily, lista.Font.Size * 2);
			DataTable dtTurnos = Turno.GetForFechas(_desde.AddSeconds(1), _hasta);
			if (dtTurnos.Rows.Count > 1)
			{
				p.Header = "Cuenta Corriente\\t\\t" + _desde.ToShortDateString() + " " + _desde.ToShortTimeString() + " - " + _hasta.ToShortDateString() + " " + _hasta.ToShortTimeString();
			}
			else
			{
				Turno t = dtTurnos.Rows.Count > 0 ? new Turno(dtTurnos.Rows[dtTurnos.Rows.Count - 1]) : Reglas.TurnoUltimo;
				p.Header = "Cuenta Corriente\\tTurno " + Turno.GetTurnoNbre(t.Numero) + "\\t" + Reglas.TurnoUltimo.FechaFinal.ToShortDateString();
			}
			p.ListView = lista;
			p.Print();
		}
	}
}
