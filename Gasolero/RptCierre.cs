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
	public class RptCierre
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
		DataTable dtCierres;

		Dictionary<uint, Articulo> _articulos = new Dictionary<uint, Articulo>(5);
		DateTime _desde, _hasta;

		RptCierre()
		{
			ListadoEsquema esq = new ListadoEsquema();
			esq.Dato = "Surtidor";
			esq.Alineacion = "C";
			esq.Posicion = 1;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Salida";
			esq.Alineacion = "D";
			esq.Posicion = 2;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Entrada";
			esq.Alineacion = "D";
			esq.Posicion = 3;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Diferencia";
			esq.Alineacion = "D";
			esq.Posicion = 4;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Venta";
			esq.Alineacion = "D";
			esq.Posicion = 5;
			esquemas.Add(esq);
			//
			bind = new ListadoBinding(lista, esquemas);
			bind.DatoGrupo = "IdArticulo";
			dtCierres = new DataTable("Cierres");
			//dtCierres.Columns.Add("Surtidor", typeof(int));//hay strings!
			dtCierres.Columns.Add("Surtidor");
			dtCierres.Columns.Add("Salida");
			dtCierres.Columns.Add("Entrada");
			dtCierres.Columns.Add("Diferencia");
			dtCierres.Columns.Add("Venta");
			dtCierres.Columns.Add("IdArticulo");
		}

		public RptCierre(DateTime dde, DateTime hta)
			: this()
		{
			List<Turno> turnos = new List<Turno>();
			_desde = dde;
			_hasta = hta;
			foreach (DataRow item in Turno.GetForFechas(dde, hta).Rows)
			{
				turnos.Add(new Turno(item));
			}
			for (int i = 0; i < Reglas.CantidadPuestos; i++)
			{
				decimal dif = 0, venta = 0;
				decimal salidaFinal = 0, entradaFinal = 0;
				int nTurno = 1;//vamos a arrancar por el 1° turno
				uint idArt = Reglas.Puestos[i].IdArticulo;
				foreach (Turno turno in turnos)
				{
					Cierre[] cierres = Cierre.GetListaXSurtidorTurno((uint)i + 1, turno.Numero);
					if (cierres.Length == 0)
						continue;
					foreach (Cierre c in cierres)
					{
						dif += c.GetDiferencia();
						venta += c.GetDiferencia() * c.Costo;
					}
					if (!_articulos.ContainsKey(idArt))
					{
						Articulo artAux = Articulo.GetSingleE(idArt);
						_articulos.Add(idArt, artAux);
						bind.Grupos.Add(new ListadoGrupo(artAux.Codigo, artAux.IdArticulo.ToString()));
					}
					if (nTurno == turnos.Count)//ultimo turno
						salidaFinal = cierres[cierres.Length - 1].Salida;
					else if (nTurno == 1)//primer turno
						entradaFinal = cierres[0].Entrada;
					nTurno++;
				}
				dtCierres.Rows.Add(i + 1, salidaFinal, entradaFinal, dif, decimal.Round(venta, 2), idArt);
			}
			//
			bind.Grupos.Add(new ListadoGrupo("Vales", int.MaxValue.ToString()));//grupo de vales
			//
			SortedList<uint, ValeVenta> itemVales = new SortedList<uint, ValeVenta>();
			decimal ventaValesTotal = 0;
			foreach (var turno in turnos)
			{
				//decimal ventaValesTurno = 0;
				foreach (DataRow dr in ValeEntrega.GetForTurno(turno.Numero).Rows)
				{
					ValeEntrega v = new ValeEntrega(dr);
					if (!_articulos.ContainsKey(v.IdArticulo))
						continue;
					Articulo art = _articulos[v.IdArticulo];
					decimal vtaVale = decimal.Round(v.Cantidad * v.Precio, 2);
					ventaValesTotal += vtaVale;
					if (!itemVales.ContainsKey(art.IdArticulo))
						itemVales.Add(art.IdArticulo, new ValeVenta(art.IdArticulo, art.Codigo));
					itemVales[art.IdArticulo].Cantidad += v.Cantidad;
					itemVales[art.IdArticulo].Venta += vtaVale;
				}
			}
			DateTime fchaTurnoDde = Turno.GetForNumero(turnos[0].Numero - 1).FechaFinal;//el turno anterior al primero
			_desde = fchaTurnoDde;
			DateTime fchaTurnoHta = hta;// Turno.GetForNumero(turno.Numero).FechaFinal;
			foreach (var x in itemVales)//ventas al cliente de gobierno en el periodo dado
			{
				ValeVenta vv = x.Value;
				//ver ventas al cliente de fiado (vale excedido)
				foreach (DataRow r in FactEncabezado.GetListForFechasIdClienteIdArticulo(fchaTurnoDde.AddSeconds(1), fchaTurnoHta, Reglas.ClienteGobierno, vv.IdArticulo).Rows)
				{
					vv.CantFiada += decimal.Parse(r["Cantidad"].ToString());
					ventaValesTotal += decimal.Parse(r["Total"].ToString());
					vv.Venta += decimal.Parse(r["Total"].ToString());
				}
				dtCierres.Rows.Add(vv.Codigo, "", vv.CantFiada.ToString("0.000"), vv.Cantidad.ToString("0.000"), vv.Venta.ToString("0.00"), int.MaxValue);
			}
			#region Grupo Totalizador
			bind.DatosTotalizadores["Entrada"] = "TOTAL";
			bind.DatosTotalizadores["Diferencia"] = "@Diferencia";
			bind.DatosTotalizadores["Venta"] = "@Venta";
			//
			bind.GrupoTotalizador.Nombre = "Resumen";
			bind.GrupoTotalizador.Clave = "Resumen";
			bind.GrupoTotalizadorDatos.AddFila(0);
			bind.GrupoTotalizadorDatos.AddFila(1);
			bind.GrupoTotalizadorDatos.AddFila(2);
			bind.GrupoTotalizadorDatos.AddFila(3);
			bind.GrupoTotalizadorDatos.AddFila(4);
			//bind.GrupoTotalizadorDatos.AddFila(5);//una más para cc pero sacamos propina
			var ccResumen = Negocio.CCMovimiento.GetResumen(fchaTurnoDde.AddSeconds(1), hta);
			decimal ccCredito = decimal.Parse(ccResumen.Rows[0]["Credito"].ToString());
			decimal ccDebito = decimal.Parse(ccResumen.Rows[0]["Debito"].ToString());
			decimal ccSaldo = decimal.Parse(ccResumen.Rows[0]["Saldo"].ToString());
			decimal ccCreditoTarjeta = decimal.Parse(ccResumen.Rows[0]["CreditoTarjeta"].ToString());
			//decimal vtaTarjeta = FactEncabezado.GetTotalVentas(fchaTurnoDde.AddSeconds(1), hta, Reglas.PagoTarjeta);
			decimal vtaTarjeta = 0;//igual a rendido por default
			decimal vtaCC = FactEncabezado.GetTotalVentas(fchaTurnoDde.AddSeconds(1), hta, Reglas.PagoCC);
			decimal dtosEfectivo = 0, dtosTarjeta = 0, dtosCC = 0, dtosTotal = 0;
			foreach (DataRow r in FactEncabezado.GetVentasDto(fchaTurnoDde.AddSeconds(1), hta).Rows)
			{
				decimal ttAux = decimal.Parse(r["Total"].ToString());
				decimal dtoAux = decimal.Parse(r["Descuento"].ToString());
				uint idPFAux = uint.Parse(r["IdPagoForma"].ToString());
				decimal valorDescontado = ttAux / (1 - dtoAux / 100) - ttAux;
				if (idPFAux == 1)//efectivo
					dtosEfectivo += valorDescontado;
				else if (idPFAux == Reglas.PagoTarjeta)
					dtosTarjeta += valorDescontado;
				else if (idPFAux == Reglas.PagoCC)
					dtosCC += valorDescontado;
				dtosTotal += valorDescontado;
			}
			//
			int filaGTotal = 0;
			#region Rendiciones de cada Turno
			SortedList<uint, decimal> rendido = new SortedList<uint, decimal>();
			foreach (DataRow item in PagoForma.GetAll().Rows)
			{
				PagoForma pf = new PagoForma(item);
				rendido.Add(pf.IdPagoForma, 0);
			}
			decimal rendidoEfectivo = 0, rendidoTarjeta = 0;
			foreach (var turno in turnos)
			{
				CajaEntrega cajaRendicion = CajaEntrega.GetSingleForTurno(turno.IdTurno);
				if (cajaRendicion != null)
					cajaRendicion.GetFilas();
				else
				{
					cajaRendicion = new CajaEntrega();
					cajaRendicion.AddFila(1, 0);
					cajaRendicion.AddFila(Reglas.PagoTarjeta, 0);
				}
				//ver rendido x forma de pago
				foreach (var item in cajaRendicion.Coleccion)
				{
					rendido[item.IdPagoForma] += item.Monto;
					if (item.IdPagoForma == Reglas.PagoTarjeta)
						rendidoTarjeta += item.Monto;
					else
						rendidoEfectivo += item.Monto;
				}
			}
			#endregion
			vtaTarjeta = rendidoTarjeta;
			//FILA 1
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Declarado Efectivo";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = rendidoEfectivo.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";//rendidoTarjeta.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "TOTAL";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "@Venta";//fila 1
			filaGTotal++;//fin FILA 1
			//FILA 2
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Diferencia Efectivo";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "";//poner luego la diferencia de efectivo
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";//poner luego la diferencia de tarjeta
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Rendir Efectivo:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "";//IMPORTANTE!!!!!!!! Hay que usar todas las celdas de los renglones del grupo total
			filaGTotal++;
			//FILA 3
			//Vendedor vendTurno = Vendedor.GetSingleE(turno.IdVendedor);
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Vendedor: -VARIOS-";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "Total Descuentos:";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Vta C/Cta.Cte.:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = vtaCC.ToString("$ 0.00");
			filaGTotal++;
			//FILA 4
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "Total Tarjeta";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Vta C/Tarjeta:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = vtaTarjeta.ToString("$ 0.00");//IMPORTANTE!!!!!!!! Hay que usar todas las celdas de los renglones del grupo total
			filaGTotal++;
			//FILA 5
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "FIRMA";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "Total Cta.Cte.";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "CC C/Tarjeta:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = ccCreditoTarjeta.ToString("$ 0.00");
			filaGTotal++;
			#endregion
			//
			bind.ActualizarTabla(dtCierres);
			bind.TablaMain = "Cierres";
			bind.ResetBinding();
			lista.Dock = DockStyle.Fill;
			lista.View = View.Details;
			//en el grupo vale no totalizar la venta, poner TOTAL en esa celda
			var grpVales = lista.Groups[lista.Groups.Count - 2].Items;
			string aux = grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 3].Text;
			grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 3].Text = "";
			grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 2].Text = aux;
			//
			//arreglar monedas
			var grpResumen = lista.Groups[lista.Groups.Count - 1].Items;
			var itTotal = grpResumen[0].SubItems[4];//FILA 1 COL 5 o última celda
			var itRendir = grpResumen[1].SubItems[4];//FILA 2 COL 5 o última celda
			decimal ventaTotal = decimal.Parse(itTotal.Text);
			//el vale se resta 2 veces porque esta sumado al total, y en realidad está incluido en las ventas, la otra es porque no es efectivo
			//el saldo de cc se debe sumar, porque las ventas ya están en negativo - se resta el credito en cc en tarjeta también
			//hay que restar el dto de cc para que de con el $ de las ventas realizadas
			//también hay que restar los dtosTarjeta porque la venta está con el descuento aplicado y no da el $ de las ventas total ej. 100$ vta tt -> 90$ en tarj + 10$ de su dto
			//se resta también TODOS los descuentos realizados, porque esa $ no está y en las ventas totales se computa sin descuentos 
			//el de tarjeta ya está incluido en vtaTarj pero se debe restar, por lo que 
			decimal rendirEfectivoReal = ventaTotal - ventaValesTotal * 2 + ccSaldo - (vtaTarjeta + dtosTarjeta) - ccCreditoTarjeta - dtosEfectivo - dtosCC;
			itRendir.Text = (rendirEfectivoReal).ToString("$ 0.00");
			itTotal.Text = (ventaTotal - ventaValesTotal).ToString("$ 0.00");
			grpResumen[2].SubItems[2].Text = dtosTotal.ToString("$ 0.00");//cambiamos de lugar los textos
			grpResumen[1].SubItems[1].Text = (rendidoEfectivo - rendirEfectivoReal).ToString("$ 0.00");//FILA 2 COL 2
			grpResumen[1].SubItems[2].Text = (rendidoTarjeta - (vtaTarjeta + /*dtosTarjeta +*/ ccCreditoTarjeta)).ToString("$ 0.00");//FILA 2 COL 3 sin dto, no se tiene que rendir
			//grpResumen[3].SubItems[2].Text = (vtaTarjeta + ccCreditoTarjeta).ToString("$ 0.00");//FILA 4 COL 3 total tarjeta
			grpResumen[4].SubItems[2].Text = (ccSaldo).ToString("$ 0.00");//FILA 5 COL 3 total cc
			//arreglar monedas en grupos, menos el ultimo
			for (int i = 0; i < lista.Groups.Count - 1; i++)
			{
				decimal tt = 0;
				grpVales = lista.Groups[i].Items;
				aux = grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 1].Text;
				if (decimal.TryParse(aux, out tt))
					grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 1].Text = tt.ToString("$ 0.00");
			}
			//
			bind.AutoResizeLista();
			Ajustar();
		}

		void Ajustar()
		{
			//anchos de planilla
			lista.Columns[0].Width = 100;
			lista.Columns[1].Width = 110;
			lista.Columns[2].Width = 110;
			lista.Columns[3].Width = 110;
			lista.Columns[4].Width = 110;//550
		}

		public void Imprimir()
		{
			MarUtils.Listados.ListViewPrinterBase p = new MarUtils.Listados.ListViewPrinterBase();
			//p.Watermark = Reglas.MarcaParcial;
			//p.WatermarkTransparency = 25;
			p.HeaderFormat = new BlockFormat();
			p.HeaderFormat.BackgroundColor = System.Drawing.Color.White;
			p.HeaderFormat.Font = new System.Drawing.Font(lista.Font.FontFamily, lista.Font.Size * 2);
			p.Header = "Cierre Acumulado\\t\\t" + _desde.ToShortDateString() + " " + _desde.ToShortTimeString() + " - " + _hasta.ToShortDateString() + " " + _hasta.ToShortTimeString();
			p.ListView = lista;
			p.Print();
		}

		class ValeVenta
		{
			public uint IdArticulo = 0;
			public string Codigo = "";
			public decimal CantFiada = 0;
			public decimal Cantidad = 0;
			public decimal Venta = 0;

			public ValeVenta(uint idArt, string cod)
			{
				IdArticulo = idArt;
				Codigo = cod;
			}
		}
	}
}
