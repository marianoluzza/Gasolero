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
	public class RptRendicion
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

		public RptRendicion(Turno turno)
		{
			SortedList<uint, Articulo> _articulos = new SortedList<uint, Articulo>();
			SortedList<uint, decimal> vtaTotalXComb = new SortedList<uint, decimal>();
			SortedList<uint, decimal> valesTotalXComb = new SortedList<uint, decimal>();
			SortedList<uint, decimal> ctTotalXComb = new SortedList<uint, decimal>();
			DataTable dtRendiciones = new DataTable("Rendiciones");
			dtRendiciones.Columns.Add("Detalle");
			dtRendiciones.Columns.Add("Cantidad");
			dtRendiciones.Columns.Add("Ingreso");
			dtRendiciones.Columns.Add("Egreso");
			dtRendiciones.Columns.Add("Diferencia");
			dtRendiciones.Columns.Add("Grupo");
			//
			ListadoEsquema esq = new ListadoEsquema();
			esq.Dato = "Detalle";
			esq.Alineacion = "I";
			esq.Posicion = 1;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Cantidad";
			esq.Alineacion = "D";
			esq.Posicion = 2;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Ingreso";
			esq.Alineacion = "D";
			esq.Posicion = 3;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Egreso";
			esq.Alineacion = "D";
			esq.Posicion = 4;
			esquemas.Add(esq);
			esq = new ListadoEsquema();
			esq.Dato = "Diferencia";
			esq.Alineacion = "D";
			esq.Posicion = 5;
			esquemas.Add(esq);
			//
			bind = new ListadoBinding(lista, esquemas);
			bind.DatoGrupo = "Grupo";
			bind.Grupos.Add(new ListadoGrupo("Combustibles", "Combustibles"));
			//_esParcial = false;
			decimal totalVenta = 0;//$ todo lo vendido segun surtidores
			decimal totalVales = 0;//$ todo lo que se dió con vales, es decir sin pagar (se decuenta para rendir)
			for (int i = 0; i < Reglas.CantidadPuestos; i++)
			{
				//todos los cierres del surtidor/puesto
				Cierre[] cierres = Cierre.GetListaXSurtidorTurno((uint)i + 1, turno.Numero);
				if (cierres.Length == 0)
					continue;
				decimal dif = 0, venta = 0;
				//Puesto pto = Reglas.Puestos[i];
				foreach (Cierre c in cierres)
				{
					dif += c.GetDiferencia();//cantidad vendida x cierre en el surtidor
					venta += c.GetDiferencia() * c.Costo;//monto vendido con costo del cierre, acumulado
				}
				uint idArt = Reglas.Puestos[i].IdArticulo;//que articulo vende el puesto
				if (!_articulos.ContainsKey(idArt))
				{
					Articulo artAux = Articulo.GetSingleE(idArt);
					_articulos.Add(idArt, artAux);
					vtaTotalXComb.Add(idArt, 0);
					ctTotalXComb.Add(idArt, 0);
					valesTotalXComb.Add(idArt, 0);
					//crear un grupo para el articulo. NO => grupo combustibles
					//bind.Grupos.Add(new ListadoGrupo(artAux.Codigo, artAux.Codigo));
				}
				vtaTotalXComb[idArt] += venta;
				ctTotalXComb[idArt] += dif;
				totalVenta += venta;//todo lo vendido
			}//fin ventas surtidores en todos los cierres del turno (todos incluye parciales)
			//descontar vales
			foreach (DataRow dr in ValeEntrega.GetForTurno(turno.Numero).Rows)
			{
				ValeEntrega v = new ValeEntrega(dr);
				if (!_articulos.ContainsKey(v.IdArticulo))
					continue;
				Articulo art = _articulos[v.IdArticulo];
				decimal vtaVale = decimal.Round(v.Cantidad * v.Precio, 2);
				valesTotalXComb[v.IdArticulo] += vtaVale;
				totalVales += vtaVale;
			}
			//SortedList<uint, ValeVenta> itemVales = new SortedList<uint, ValeVenta>();
			DateTime fchaTurnoDde = Turno.GetForNumero(turno.Numero - 1).FechaFinal;
			DateTime fchaTurnoHta = Turno.GetForNumero(turno.Numero).FechaFinal;
			foreach (var art in _articulos)
			{
				//ver ventas al cliente de fiado (vale excedido)
				foreach (DataRow r in FactEncabezado.GetListForFechasIdClienteIdArticulo(fchaTurnoDde.AddSeconds(1), fchaTurnoHta, Reglas.ClienteGobierno, art.Key).Rows)
				{
					//vv.CantFiada += decimal.Parse(r["Cantidad"].ToString());//vv era un VentaVale
					//ventaVales += decimal.Parse(r["Total"].ToString());
					//vv.Venta += decimal.Parse(r["Total"].ToString());
					decimal ttAux = decimal.Parse(r["Total"].ToString());
					valesTotalXComb[art.Key] += ttAux;
					totalVales += ttAux;
				}
				//
				dtRendiciones.Rows.Add(art.Value.Codigo, ctTotalXComb[art.Key], vtaTotalXComb[art.Key].ToString("0.00"), valesTotalXComb[art.Key].ToString("0.00"), (vtaTotalXComb[art.Key] - valesTotalXComb[art.Key]).ToString("0.00"), "Combustibles");
			}
			SortedList<uint, PagoForma> pagoFormas = new SortedList<uint, PagoForma>();
			SortedList<uint, uint> pagoFormasOrdenadas = new SortedList<uint, uint>();
			SortedList<uint, decimal> pfCCIngresos = new SortedList<uint, decimal>();
			SortedList<uint, decimal> pfCCEgresos = new SortedList<uint, decimal>();
			SortedList<uint, decimal> rendido = new SortedList<uint, decimal>();
			foreach (DataRow item in PagoForma.GetAll().Rows)
			{
				PagoForma pf = new PagoForma(item);
				pagoFormas.Add(pf.IdPagoForma, pf);
				pagoFormasOrdenadas.Add(pf.Orden, pf.IdPagoForma);
				pfCCIngresos.Add(pf.IdPagoForma, 0);
				pfCCEgresos.Add(pf.IdPagoForma, 0);
				rendido.Add(pf.IdPagoForma, 0);
			}
			bind.Grupos.Add(new ListadoGrupo("Cuenta Corriente", "CC"));
			DataTable ccMovimientos = Negocio.CCMovimiento.GetMovimientosDetallados(fchaTurnoDde.AddSeconds(1), fchaTurnoHta);
			foreach (DataRow r in ccMovimientos.Rows)
			{
				//Negocio.CCMovimiento cc = new Negocio.CCMovimiento(r);
				decimal monto = decimal.Parse(r["Monto"].ToString());
				uint pf = uint.Parse(r["IdPagoForma"].ToString());
				if (monto > 0)
					pfCCIngresos[pf] += monto;
				else
					pfCCEgresos[pf] += monto;
			}
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
			}
			//
			foreach (var item in pagoFormasOrdenadas)
			{
				decimal ing = pfCCIngresos[item.Value];
				decimal eg = pfCCEgresos[item.Value];
				dtRendiciones.Rows.Add(pagoFormas[item.Value].Descripcion, "", ing != 0 ? ing.ToString("0.00") : "", eg != 0 ? eg.ToString("0.00") : "", (ing - eg).ToString("0.00"), "CC");
			}
			bind.DatosTotalizadores["Detalle"] = "TOTAL";
			bind.DatosTotalizadores["Cantidad"] = "";
			//bind.DatosTotalizadores["Ingreso"] = "@Ingreso";
			//bind.DatosTotalizadores["Egreso"] = "@Egreso";
			//bind.DatosTotalizadores["Diferencia"] = "@Diferencia";
			#region Grupo Totalizador
			var ccResumen = Negocio.CCMovimiento.GetResumen(Turno.GetForNumero(turno.Numero - 1).FechaFinal.AddSeconds(1), turno.FechaFinal);
			decimal ccCredito = decimal.Parse(ccResumen.Rows[0]["Credito"].ToString());
			decimal ccDebito = decimal.Parse(ccResumen.Rows[0]["Debito"].ToString());
			decimal ccSaldo = decimal.Parse(ccResumen.Rows[0]["Saldo"].ToString());
			decimal ccCreditoTarjeta = decimal.Parse(ccResumen.Rows[0]["CreditoTarjeta"].ToString());
			decimal vtaTarjeta = FactEncabezado.GetTotalVentas(Turno.GetForNumero(turno.Numero - 1).FechaFinal.AddSeconds(1), turno.FechaFinal, Reglas.PagoTarjeta);
			decimal vtaCC = FactEncabezado.GetTotalVentas(Turno.GetForNumero(turno.Numero - 1).FechaFinal.AddSeconds(1), turno.FechaFinal, Reglas.PagoCC);
			decimal dtosEfectivo = 0, dtosTarjeta = 0, dtosCC = 0, dtosTotal = 0;
			foreach (DataRow r in FactEncabezado.GetVentasDto(Turno.GetForNumero(turno.Numero - 1).FechaFinal.AddSeconds(1), turno.FechaFinal).Rows)
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
			bind.Grupos.Add(new ListadoGrupo("Rendición", "Final"));
			decimal sumaPagosNoTarjeta = 0;
			foreach (var item in pagoFormasOrdenadas)
			{
				//if (rendido[item.Value] == 0)
				//    continue;
				if (item.Value == Reglas.PagoTarjeta)
					dtRendiciones.Rows.Add(pagoFormas[item.Value].Descripcion, "", rendido[item.Value].ToString("$ 0.00"), (vtaTarjeta + ccCreditoTarjeta).ToString("$ 0.00"), (rendido[item.Value] - (vtaTarjeta + ccCreditoTarjeta)).ToString("$ 0.00"), "Final");
				else
					sumaPagosNoTarjeta += rendido[item.Value];
			}
			//la venta segun surtidores - los vales - los descuentos en efectivo + ingresos de CC en efectivo - vta en tarj - vta en cc - dto cc
			decimal rendirEfectivo = totalVenta - totalVales - dtosEfectivo + pfCCIngresos[1] - vtaTarjeta - (vtaCC + dtosCC);
			dtRendiciones.Rows.Add("Efe/Resto", "", sumaPagosNoTarjeta.ToString("$ 0.00"), rendirEfectivo.ToString("$ 0.00"), (sumaPagosNoTarjeta - rendirEfectivo).ToString("$ 0.00"), "Final");
			#endregion
			//
			bind.ActualizarTabla(dtRendiciones);
			bind.TablaMain = "Rendiciones";
			bind.ResetBinding();
			lista.Dock = DockStyle.Fill;
			lista.View = View.Details;
			//en el grupo vale no totalizar la venta, poner TOTAL en esa celda
			//var grpVales = lista.Groups[lista.Groups.Count - 2].Items;
			//string aux = grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 3].Text;
			//grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 3].Text = "";
			//grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 2].Text = aux;
			//
			//arreglar monedas
			//var itTotal = lista.Groups[lista.Groups.Count - 1].Items[1].SubItems[2];
			//var itRendir = lista.Groups[lista.Groups.Count - 1].Items[1].SubItems[4];
			//decimal ventaTotal = decimal.Parse(itTotal.Text);
			//el vale se resta 2 veces porque esta sumado al total, y en realidad está incluido en las ventas, la otra es porque no es efectivo
			//el saldo de cc se debe sumar, porque las ventas ya están en negativo - se resta el credito en cc en tarjeta también
			//hay que restar el dto de cc para que de con el $ de las ventas realizadas
			//también hay que restar los dtosTarjeta porque la venta está con el descuento aplicado y no da el $ de las ventas total ej. 100$ vta tt -> 90$ en tarj + 10$ de su dto
			//se resta también TODOS los descuentos realizados, porque esa $ no está y en las ventas totales se computa sin descuentos 
			//el de tarjeta ya está incluido en vtaTarj pero se debe restar, por lo que 
			//itRendir.Text = (ventaTotal - ventaVales * 2 + ccSaldo - (vtaTarjeta + dtosTarjeta) - ccCreditoTarjeta - dtosEfectivo - dtosCC).ToString("$ 0.00");
			//itTotal.Text = (ventaTotal - ventaVales).ToString("$ 0.00");
			//lista.Groups[lista.Groups.Count - 1].Items[2].SubItems[2].Text = dtosTotal.ToString("$ 0.00");//cambiamos de lugar los textos
			////arreglar monedas en grupos, menos el ultimo
			//for (int i = 0; i < lista.Groups.Count - 1; i++)
			//{
			//    decimal tt = 0;
			//    grpVales = lista.Groups[i].Items;
			//    aux = grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 1].Text;
			//    if (decimal.TryParse(aux, out tt))
			//        grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 1].Text = tt.ToString("$ 0.00");
			//}
			//
			foreach (ListViewGroup grupo in lista.Groups)
			{
				if (grupo.Items.Count <= 1)
					continue;
				decimal d1 = 0, d2 = 0, d3 = 0;
				for (int i = 0; i < grupo.Items.Count - 1; i++)
				{
					var it = grupo.Items[i];
					decimal aux = 0;
					if (decimal.TryParse(it.SubItems[it.SubItems.Count - 3].Text.ToString().Replace('$', ' ').Trim(), out aux))
					{
						d1 += aux;
						it.SubItems[it.SubItems.Count - 3].Text = aux.ToString("$ 0.00");
					}
					//
					if (decimal.TryParse(it.SubItems[it.SubItems.Count - 2].Text.ToString().Replace('$', ' ').Trim(), out aux))
					{
						d2 += aux;
						it.SubItems[it.SubItems.Count - 2].Text = aux.ToString("$ 0.00");
					}
					//
					if (decimal.TryParse(it.SubItems[it.SubItems.Count - 1].Text.ToString().Replace('$', ' ').Trim(), out aux))
					{
						d3 += aux;
						it.SubItems[it.SubItems.Count - 1].Text = aux.ToString("$ 0.00");
					}
				}
				ListViewItem itFinal = grupo.Items[grupo.Items.Count - 1];
				itFinal.SubItems[itFinal.SubItems.Count - 3].Text = d1.ToString("$ 0.00");
				itFinal.SubItems[itFinal.SubItems.Count - 2].Text = d2.ToString("$ 0.00");
				itFinal.SubItems[itFinal.SubItems.Count - 1].Text = d3.ToString("$ 0.00");
			}
			bind.AutoResizeLista();
			Ajustar();
		}

		void Ajustar()
		{
			//anchos de planilla
			lista.Columns[0].Width = 80;
			lista.Columns[1].Width = 90;
			lista.Columns[2].Width = 100;
			lista.Columns[3].Width = 100;
			lista.Columns[4].Width = 120;
			//lista.Columns[5].Width = 80;
			//lista.Columns[6].Width = 40;
		}

		public void Imprimir()
		{
			MarUtils.Listados.ListViewPrinterBase p = new MarUtils.Listados.ListViewPrinterBase();
			//if (_esParcial)
			//{
			//    p.Watermark = Reglas.MarcaParcial;
			//    p.WatermarkTransparency = 25;
			//}
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
