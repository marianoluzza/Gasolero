using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MarUtils.Listados;
using Negocio;

namespace Gasolero
{
	public partial class Planilla : Form
	{
		ListView lista = new ListView();
		List<ListadoEsquema> esquemas = new List<ListadoEsquema>();
		ListadoBinding bind;
		DataTable dtCierres;
		SortedList<uint, Articulo> _articulos = new SortedList<uint, Articulo>();

		decimal ventaVales = 0;

		bool _previsualizar = false;
		bool _esParcial;

		public bool Previsualizar
		{
			get { return _previsualizar; }
			set
			{
				_previsualizar = value;
				BImprimir.Text = value ? "Cerrar" : "Imprimir";
			}
		}

		public void Ajustar()
		{
			//anchos de planilla
			lista.Columns[0].Width = 100;
			lista.Columns[1].Width = 110;
			lista.Columns[2].Width = 110;
			lista.Columns[3].Width = 110;
			lista.Columns[4].Width = 110;
		}

		Planilla()
		{
			InitializeComponent();
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
			//esq = new ListadoEsquema();
			//esq.Dato = "IdArticulo";
			//esq.Alineacion = "I";
			//esq.Posicion = 6;
			//esquemas.Add(esq);
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

		public Planilla(Turno turno)
			: this()
		{
			Puesto pto;
			_esParcial = false;
			for (int i = 0; i < Reglas.CantidadPuestos; i++)
			{
				Cierre[] cierres = Cierre.GetListaXSurtidorTurno((uint)i + 1, turno.Numero);
				if (cierres.Length == 0)
					continue;
				decimal dif = 0, venta = 0;
				pto = Reglas.Puestos[i];
				foreach (Cierre c in cierres)
				{
					dif += c.GetDiferencia();
					venta += c.GetDiferencia() * c.Costo;
				}
				uint idArt = Reglas.Puestos[i].IdArticulo;
				if (!_articulos.ContainsKey(idArt))
				{
					Articulo artAux = Articulo.GetSingleE(idArt);
					_articulos.Add(idArt, artAux);
					bind.Grupos.Add(new ListadoGrupo(artAux.Codigo, artAux.IdArticulo.ToString()));
				}
				//precio = arts[idArt].Precio;//esto no es el precio en todos los cierres
				//dtCierres.Rows.Add(i + 1, cierres[0].Salida, cierres[cierres.Length - 1].Entrada, dif, decimal.Round(dif * precio, 2), idArt);
				dtCierres.Rows.Add(i + 1, cierres[cierres.Length - 1].Salida, cierres[0].Entrada, dif, decimal.Round(venta, 2), idArt);
			}
			//dtCierres.DefaultView.Sort = "Surtidor";
			//dtCierres = dtCierres.DefaultView.ToTable();
			GrupoVales();
			SortedList<uint, ValeVenta> itemVales = new SortedList<uint, ValeVenta>();
			foreach (DataRow dr in ValeEntrega.GetForTurno(turno.Numero).Rows)
			{
				ValeEntrega v = new ValeEntrega(dr);
				if (!_articulos.ContainsKey(v.IdArticulo))
					continue;
				Articulo art = _articulos[v.IdArticulo];
				decimal vtaVale = decimal.Round(v.Cantidad * v.Precio, 2);
				//decimal vtaVale = decimal.Round(v.Cantidad * preciosParciales[v.IdParcial][v.IdArticulo], 2);
				ventaVales += vtaVale;
				if (!itemVales.ContainsKey(art.IdArticulo))
					itemVales.Add(art.IdArticulo, new ValeVenta(art.IdArticulo, art.Codigo));
				itemVales[art.IdArticulo].Cantidad += v.Cantidad;
				itemVales[art.IdArticulo].Venta += vtaVale;
			}
			DateTime fchaTurnoDde = Turno.GetForNumero(turno.Numero - 1).FechaFinal;
			DateTime fchaTurnoHta = Turno.GetForNumero(turno.Numero).FechaFinal;
			foreach (var x in itemVales)
			{
				ValeVenta vv = x.Value;
				//ver ventas al cliente de fiado (vale excedido)
				foreach (DataRow r in FactEncabezado.GetListForFechasIdClienteIdArticulo(fchaTurnoDde.AddSeconds(1), fchaTurnoHta, Reglas.ClienteGobierno, vv.IdArticulo).Rows)
				{
					vv.CantFiada += decimal.Parse(r["Cantidad"].ToString());
					ventaVales += decimal.Parse(r["Total"].ToString());
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
			//
			int filaGTotal = 0;
			//bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Mov Cta. Cte.";
			//bind.GrupoTotalizadorDatos["Salida", filaGTotal] = Math.Abs(ccDebito).ToString("$ 0.00");
			//bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = ccCredito.ToString("$ 0.00");
			//bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = ccSaldo.ToString("$ 0.00");
			//bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "";//fila 1
			#region Rendicion
			SortedList<uint, decimal> rendido = new SortedList<uint, decimal>();
			foreach (DataRow item in PagoForma.GetAll().Rows)
			{
				PagoForma pf = new PagoForma(item);
				rendido.Add(pf.IdPagoForma, 0);
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
			decimal rendidoEfectivo = 0, rendidoTarjeta = 0;
			foreach (var item in cajaRendicion.Coleccion)
			{
				rendido[item.IdPagoForma] += item.Monto;
				if (item.IdPagoForma == Reglas.PagoTarjeta)
					rendidoTarjeta += item.Monto;
				else
					rendidoEfectivo += item.Monto;
			}
			#endregion
			vtaTarjeta = rendidoTarjeta;//sin errores en rendicion de tarjeta
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Declarado Efectivo";//"Declarado Efectivo/Tarjeta";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = rendidoEfectivo.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";//rendidoTarjeta.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "TOTAL";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "@Venta";//fila 1
			filaGTotal++;//fin FILA 1
			//FILA 2
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Diferencia Efectivo/Tarjeta";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "";//poner luego la diferencia de efectivo
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";//poner luego la diferencia de tarjeta
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Rendir Efectivo:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "";

			//bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Firma Turno Anterior:";
			//bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "TOTAL";
			//bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "@Venta";
			//bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Rendir Efectivo:";
			//bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "";//IMPORTANTE!!!!!!!! Hay que usar todas las celdas de los renglones del grupo total
			filaGTotal++;
			//FILA 3
			Vendedor vendTurno = Vendedor.GetSingleE(turno.IdVendedor);
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Vendedor: " + vendTurno.Nombre;
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "Total Descuentos:";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Vta C/Cta.Cte.:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = vtaCC.ToString("$ 0.00");
			filaGTotal++;
			//FILA 4
			//bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Fecha Salida Turno:";
			//bind.GrupoTotalizadorDatos["Salida", filaGTotal] = turno.FechaFinal.ToShortDateString();
			//bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "Turno " + GetTurnoNbre(turno.Numero);
			//bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Vta C/Tarjeta:";
			//bind.GrupoTotalizadorDatos["Venta", filaGTotal] = vtaTarjeta.ToString("$ 0.00");

			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "Total Tarjeta";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Vta C/Tarjeta:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = (vtaTarjeta-dtosTarjeta-ccCreditoTarjeta).ToString("$ 0.00");// vtaTarjeta.ToString("$ 0.00");//IMPORTANTE!!!!!!!! Hay que usar todas las celdas de los renglones del grupo total

			filaGTotal++;
			//FILA 5
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "FIRMA";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "Total Cta.Cte.";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "CC C/Tarjeta:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = ccCreditoTarjeta.ToString("$ 0.00");
			filaGTotal++;
			//
			//bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Propina";
			//bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "$ " + turno.Propina.ToString("0.00");
			#endregion
			//
			bind.ActualizarTabla(dtCierres);
			bind.TablaMain = "Cierres";
			bind.ResetBinding();
			lista.Dock = DockStyle.Fill;
			lista.View = View.Details;
			PanelListado.Controls.Add(lista);
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
			decimal rendirEfectivoReal = ventaTotal - ventaVales * 2 + ccSaldo - (vtaTarjeta + dtosTarjeta) - ccCreditoTarjeta - dtosEfectivo - dtosCC;
			itRendir.Text = (rendirEfectivoReal).ToString("$ 0.00");
			itTotal.Text = (ventaTotal - ventaVales).ToString("$ 0.00");
			grpResumen[2].SubItems[2].Text = dtosTotal.ToString("$ 0.00");//cambiamos de lugar los textos
			grpResumen[1].SubItems[1].Text = (rendidoEfectivo - rendirEfectivoReal).ToString("$ 0.00");//FILA 2 COL 2
			//grpResumen[1].SubItems[2].Text = (rendidoTarjeta - (vtaTarjeta + /*dtosTarjeta +*/ ccCreditoTarjeta)).ToString("$ 0.00");//FILA 2 COL 3 sin dto, no se tiene que rendir
			grpResumen[3].SubItems[2].Text = (vtaTarjeta + ccCreditoTarjeta).ToString("$ 0.00");//FILA 4 COL 3 total tarjeta
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

		public Planilla(Cierre[] cierresXPuesto, decimal propina, ValeEntrega[] vales, bool incluirParciales = true)
			: this()
		{
			Puesto pto;
			_esParcial = true;
			foreach (Cierre cierrePuesto in cierresXPuesto)
			{
				decimal dif = 0, venta = 0;
				decimal salida = cierrePuesto.Salida, entrada = cierrePuesto.Entrada;
				pto = Reglas.Puestos[cierrePuesto.Surtidor - 1];
				dif = cierrePuesto.GetDiferencia();
				uint idArt = pto.IdArticulo;
				if (!_articulos.ContainsKey(idArt))
				{
					Articulo artAux = Articulo.GetSingleE(idArt);
					_articulos.Add(idArt, artAux);
					bind.Grupos.Add(new ListadoGrupo(artAux.Codigo, artAux.IdArticulo.ToString()));
				}
				//el precio no es el actual, es el que diga el cierre
				//xventa = dif * _articulos[idArt].Precio; 
				venta = dif * cierrePuesto.Costo;
				#region incluirParciales
				if (incluirParciales)
				{
					uint idCierreMenor = uint.MaxValue;
					foreach (Cierre cierreParcial in Cierre.GetListaXSurtidorTurno(cierrePuesto.Surtidor, Reglas.TurnoUltimo.Numero + 1))
					{
						if (cierreParcial.IdCierre < idCierreMenor)
						{//me quedo con el menor para mostrar el valor de la entrada
							idCierreMenor = cierreParcial.IdCierre;
							entrada = cierreParcial.Entrada;
						}
						dif += cierreParcial.GetDiferencia();//suma cada dif de los cierres parciales
						venta += cierreParcial.GetDiferencia() * cierreParcial.Costo;//se hicieron con costos diferentes c/parcial
					}
				}
				#endregion
				dtCierres.Rows.Add(cierrePuesto.Surtidor.ToString().PadLeft(2, ' '), salida, entrada, dif, decimal.Round(venta, 2), idArt);
			}
			dtCierres.DefaultView.Sort = "Surtidor";
			dtCierres = dtCierres.DefaultView.ToTable();
			GrupoVales();
			SortedList<uint, ValeVenta> itemVales = new SortedList<uint, ValeVenta>();
			foreach (var v in vales)
			{
				Articulo art;
				if (_articulos.ContainsKey(v.IdArticulo))
					art = _articulos[v.IdArticulo];
				else
				{
					art = Articulo.GetSingleE(v.IdArticulo);
					_articulos.Add(v.IdArticulo, art);
				}
				decimal vtaVale;
				//no sirve ese precio porque NO SIEMPRE son los actuales
				//xvtaVale = decimal.Round(v.Cantidad * art.Precio, 2);
				//
				vtaVale = decimal.Round(v.Cantidad * v.Precio, 2);
				ventaVales += vtaVale;
				//
				if (!itemVales.ContainsKey(art.IdArticulo))
					itemVales.Add(art.IdArticulo, new ValeVenta(art.IdArticulo, art.Codigo));
				itemVales[art.IdArticulo].Cantidad += v.Cantidad;
				itemVales[art.IdArticulo].Venta += vtaVale;
			}
			if (incluirParciales)
			{
				foreach (DataRow dr in ValeEntrega.GetForTurno(Reglas.TurnoUltimo.Numero + 1).Rows)
				{
					ValeEntrega v = new ValeEntrega(dr);
					Articulo art = _articulos[v.IdArticulo];
					decimal vtaVale = decimal.Round(v.Cantidad * v.Precio, 2);
					ventaVales += vtaVale;
					if (!itemVales.ContainsKey(art.IdArticulo))
						itemVales.Add(art.IdArticulo, new ValeVenta(art.IdArticulo, art.Codigo));
					itemVales[art.IdArticulo].Cantidad += v.Cantidad;
					itemVales[art.IdArticulo].Venta += vtaVale;
				}
			}
			foreach (var x in itemVales)
			{
				ValeVenta vv = x.Value;
				//ver ventas al cliente de fiado (vale excedido)
				foreach (DataRow r in FactEncabezado.GetListForFechasIdClienteIdArticulo(Reglas.TurnoUltimo.FechaFinal.AddSeconds(1), Reglas.Now, Reglas.ClienteGobierno, vv.IdArticulo).Rows)
				{
					vv.CantFiada += decimal.Parse(r["Cantidad"].ToString());
					vv.Venta += decimal.Parse(r["Total"].ToString());
					//sumar el total vendido fiado al total de vales, para descontar de la rendición
					ventaVales += decimal.Parse(r["Total"].ToString());
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
			var ccResumen = Negocio.CCMovimiento.GetResumen(Reglas.TurnoUltimo.FechaFinal, Reglas.Now);
			decimal ccCredito = decimal.Parse(ccResumen.Rows[0]["Credito"].ToString());
			decimal ccDebito = decimal.Parse(ccResumen.Rows[0]["Debito"].ToString());
			decimal ccSaldo = decimal.Parse(ccResumen.Rows[0]["Saldo"].ToString());
			decimal ccCreditoTarjeta = decimal.Parse(ccResumen.Rows[0]["CreditoTarjeta"].ToString());
			decimal vtaTarjeta = FactEncabezado.GetTotalVentas(Reglas.TurnoUltimo.FechaFinal, Reglas.Now, Reglas.PagoTarjeta);
			decimal vtaCC = FactEncabezado.GetTotalVentas(Reglas.TurnoUltimo.FechaFinal, Reglas.Now, Reglas.PagoCC);
			decimal dtosEfectivo = 0, dtosTarjeta = 0, dtosCC = 0, dtosTotal = 0;
			foreach (DataRow r in FactEncabezado.GetVentasDto(Reglas.TurnoUltimo.FechaFinal, Reglas.Now).Rows)
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
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Mov Cta. Cte.";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = Math.Abs(ccDebito).ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = ccCredito.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = ccSaldo.ToString("$ 0.00");
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "";
			filaGTotal++;
			//
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Firma Turno Anterior:";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "TOTAL";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "@Venta";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Rendir Efectivo:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "";
			filaGTotal++;
			//
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "Total descuentos:";
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Vta C/Cta.Cte.:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = vtaCC.ToString("$ 0.00");
			filaGTotal++;
			//
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Fecha Salida Turno:";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = Reglas.Now.ToShortDateString();
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "Turno " + GetTurnoNbre(Reglas.TurnoUltimo.Numero + 1);
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Vta C/Tarjeta:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = vtaTarjeta.ToString("$ 0.00");
			filaGTotal++;
			//
			bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "Vendedor:";
			bind.GrupoTotalizadorDatos["Salida", filaGTotal] = Reglas.VendedorActual.Nombre;
			bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "Firma";
			bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "CC C/Tarjeta:";
			bind.GrupoTotalizadorDatos["Venta", filaGTotal] = ccCreditoTarjeta.ToString("$ 0.00");
			//filaGTotal++;
			//
			//bind.GrupoTotalizadorDatos["Surtidor", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Salida", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Entrada", filaGTotal] = "";
			//bind.GrupoTotalizadorDatos["Diferencia", filaGTotal] = "Propina";
			//bind.GrupoTotalizadorDatos["Venta", filaGTotal] = "$ " + propina.ToString("0.00");
			//filaGTotal++;//ultima fila
			#endregion
			//
			bind.ActualizarTabla(dtCierres);
			bind.TablaMain = "Cierres";
			bind.ResetBinding();
			lista.Dock = DockStyle.Fill;
			lista.View = View.Details;
			PanelListado.Controls.Add(lista);
			//en el grupo vale no totalizar la venta, poner TOTAL en esa celda
			var grpVales = lista.Groups[lista.Groups.Count - 2].Items;
			string aux = grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 3].Text;
			grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 3].Text = "";
			grpVales[grpVales.Count - 1].SubItems[grpVales[grpVales.Count - 1].SubItems.Count - 2].Text = aux;
			//
			//arreglar monedas
			//var itTotal = lista.Items[lista.Items.Count - 5].SubItems[lista.Items[lista.Items.Count - 5].SubItems.Count - 3];
			var itTotal = lista.Groups[lista.Groups.Count - 1].Items[1].SubItems[2];
			//var itRendir = lista.Items[lista.Items.Count - 5].SubItems[lista.Items[lista.Items.Count - 5].SubItems.Count - 1];
			var itRendir = lista.Groups[lista.Groups.Count - 1].Items[1].SubItems[4];
			decimal ventaTotal = decimal.Parse(itTotal.Text);
			//el vale se resta 2 veces porque esta sumado al total, y en realidad está incluido en las ventas, la otra es porque no es efectivo
			//el saldo de cc se debe sumar, porque las ventas ya están en negativo - se resta el credito en cc en tarjeta también
			//hay que restar el dto de cc para que de con el $ de las ventas realizadas
			//también hay que restar los dtosTarjeta porque la venta está con el descuento aplicado y no da el $ de las ventas total ej. 100$ vta tt -> 90$ en tarj + 10$ de su dto
			//se resta también TODOS los descuentos realizados, porque esa $ no está y en las ventas totales se computa sin descuentos 
			//el de tarjeta ya está incluido en vtaTarj pero se debe restar, por lo que 
			itRendir.Text = (ventaTotal - ventaVales * 2 + ccSaldo - (vtaTarjeta + dtosTarjeta) - ccCreditoTarjeta - dtosEfectivo - dtosCC).ToString("$ 0.00");
			lista.Groups[lista.Groups.Count - 1].Items[2].SubItems[2].Text = dtosTotal.ToString("$ 0.00");
			itTotal.Text = (ventaTotal - ventaVales).ToString("$ 0.00");
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

		void GrupoVales()
		{
			bind.Grupos.Add(new ListadoGrupo("Vales", int.MaxValue.ToString()));
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
			ListViewPrinterBase p = new ListViewPrinterBase();
			p.HeaderFormat = new BlockFormat();
			p.HeaderFormat.BackgroundColor = Color.White;
			p.HeaderFormat.Font = new Font(this.Font.FontFamily, this.Font.Size * 2);
			if (_esParcial)
			{
				p.Header = "Cierre Parcial\\tTurno " + Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero + 1) + "\\t" + Reglas.Now.ToShortDateString();
				p.Watermark = Reglas.MarcaParcial;
				p.WatermarkTransparency = 25;
			}
			else
				p.Header = "Cierre de Turno\\tTurno " + Turno.GetTurnoNbre(Reglas.TurnoUltimo.Numero) + "\\t" + Reglas.TurnoUltimo.FechaFinal.ToShortDateString();
			p.ListView = lista;
			p.Print();
		}

		private void Planilla_Load(object sender, EventArgs e)
		{
			//anchos de planilla
			//lista.Columns[0].Width = 100;
			//lista.Columns[1].Width = 110;
			//lista.Columns[2].Width = 110;
			//lista.Columns[3].Width = 110;
			//lista.Columns[4].Width = 110;
			//
			//Size = lista.GetPreferredSize(new Size(1000,1000)); 
		}

		string GetTurnoNbre(uint tNum)
		{
			return Turno.GetTurnoNbre(tNum);
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