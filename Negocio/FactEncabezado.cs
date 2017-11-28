using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class FactEncabezado : EntidadBase, IComparable
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "FactEncabezados"; } }
		public static string NombreEntidad
		{ get { return "FactEncabezado"; } }
		public static string NombreClave
		{ get { return "IdFactEncabezado"; } }

		#endregion

		#region Atributos y Propiedades
		bool _antigua = false;

		System.DateTime _Fecha;
		public System.DateTime Fecha
		{
			get { return _Fecha; }
			set
			{
				if (!_antigua)
					_Fecha = value;
			}
		}

		System.UInt32 _IdFactEncabezado = 0;
		public System.UInt32 IdFactEncabezado
		{
			get { return _IdFactEncabezado; }
			set { _IdFactEncabezado = value; }
		}

		System.UInt32 _IdVendedor;
		public System.UInt32 IdVendedor
		{
			get { return _IdVendedor; }
			set { _IdVendedor = value; }
		}

		System.UInt32 _IdCliente;
		public System.UInt32 IdCliente
		{
			get { return _IdCliente; }
			set { _IdCliente = value; }
		}

		System.String _Letra;
		public System.String Letra
		{
			get { return _Letra; }
			set { _Letra = value; }
		}

		System.String _Numero;
		public System.String Numero
		{
			get { return _Numero; }
			set { _Numero = value; }
		}

		System.UInt32 _Anula = 0;
		/// <summary>
		/// Dice a quien anula o quien me anula. Si total es menor a 0 es NC
		/// </summary>
		public System.UInt32 Anula
		{
			get { return _Anula; }
			set { _Anula = value; }
		}

		public uint IdPagoForma { get; set; }

		public decimal Total
		{
			get
			{
				if (Cuerpo == null)
					return 0;
				decimal res = 0;
				foreach (FactItem i in Cuerpo)
					res += i.Monto;
				return res;
			}
		}
		#endregion

		FactItemCollection _Cuerpo;
		public FactItemCollection Cuerpo
		{
			get { return _Cuerpo; }
		}

		public string CCDescripcion { get; set; }

		#region Constructores
		public FactEncabezado()
		{
			_Cuerpo = new FactItemCollection(this);
			IdPagoForma = 1;
		}

		public FactEncabezado(DateTime fecha)
		{
			_Cuerpo = new FactItemCollection(this);
			Fecha = fecha;
			IdPagoForma = 1;
			_antigua = true;
		}

		public FactEncabezado(DataRow r)
			: this()
		{
			Fecha = DateTime.Parse(r["Fecha"].ToString());
			IdFactEncabezado = uint.Parse(r["IdFactEncabezado"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			uint valor = 0;
			if (r["Anula"] != null && uint.TryParse(r["Anula"].ToString(), out valor))
				Anula = valor;
			else
				Anula = 0;
			this.IdCliente = uint.Parse(r["IdCliente"].ToString());
			this.Letra = r["Letra"].ToString();
			this.Numero = r["Numero"].ToString();
			if (r["IdPagoForma"] != null && uint.TryParse(r["IdPagoForma"].ToString(), out valor))
				IdPagoForma = valor;
			else
				IdPagoForma = 1;
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.FactEncabezados);
		}

		public void GetFilas()
		{
			FactItem.GetListForFactEncabezado(this);
		}

		public override string ToString()
		{
			return Numero + " " + Letra + " " + Fecha.ToShortDateString();
		}
		#endregion

		#region Consultas
		public static string GetNumero()
		{
			return DAL.Datos.FactEncabezadosGetNumero();
		}

		public static decimal GetTotalMaximo(uint idPagoForma = 1)
		{
			if (idPagoForma == Reglas.PagoTarjeta)
				return DAL.Datos.FactEncabezadosGetTotalMaximo();
			else
				return DAL.Datos.FactEncabezadosGetTotalMaximo();
		}

		public static decimal GetTotalVentas(DateTime dde, DateTime hta, uint idPagoForma)
		{
			return DAL.Datos.FactEncabezadosGeVentatTotalForFechasIdPagoForma(dde, hta, idPagoForma);
		}

		public static DataTable GetVentasDto(DateTime dde, DateTime hta)
		{
			return DAL.Datos.FactEncabezadosTotalizadosConDtoGetForFechas(dde, hta);
		}

		public static ABMResultado SetTotalMaximo(decimal valor)
		{
			return DAL.Datos.FactEncabezadosSetTotalMaximo(valor);
		}

		public static DataTable GetPlusUno(DataTable dt)
		{
			DataRow r = dt.Rows.Add();
			r["Fecha"] = new DateTime(2000, 1, 1).ToShortDateString();
			r["IdFactEncabezado"] = (uint)1;
			r["IdVendedor"] = Reglas.VendedorActual.IdVendedor;
			r["Anula"] = 0;
			r["IdCliente"] = Reglas.ClienteNCB;
			r["Letra"] = "-";
			r["Numero"] = "-";
			r["IdPagoForma"] = (uint)1;
			dt.Rows.Add(r);
			return dt;
		}

		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.FactEncabezados);
		}

		public static DataTable GetListForFechasIdCliente(DateTime dde, DateTime hta, uint idCliente)
		{
			return DAL.Datos.FactEncabezadosGetForFechas(dde, hta, idCliente);
		}

		public static DataTable GetListForFechasIdClienteIdArticulo(DateTime dde, DateTime hta, uint idCliente, uint idArticulo)
		{
			return DAL.Datos.FactEncabezadosGetForFechas(dde, hta, idCliente, idArticulo);
		}

		public static DataTable GetTotalxArticuloForFechas(DateTime dde, DateTime hta, uint idArticulo)
		{
			return DAL.Datos.FactEncabezadosGetTotalxArticuloForFechas(dde, hta, idArticulo);
		}

		public static DataTable GetListForFechas(DateTime dde, DateTime hta)
		{
			return DAL.Datos.FactEncabezadosGetForFechas(dde, hta, 0);
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.FactEncabezados, NombreClave, id);
		}

		public static FactEncabezado GetSingleE(uint id)
		{
			FactEncabezado res = new FactEncabezado(GetSingle(id).Rows[0]);
			res.GetFilas();
			return res;
		}
		#endregion

		#region ABM
		/// <summary>
		/// Inicia el proceso de alta que termina con ConfirmarAlta
		/// </summary>
		public bool IniciarAlta()
		{
			bool empeceTransa = Datos.InitTransa();
			return empeceTransa;
		}

		/// <summary>
		/// Da de alta una factura
		/// </summary>
		public override ABMResultado Alta()
		{
			//bool empeceTransa = Datos.InitTransa();
			//bool exito = false;
			ABMResultado res;
			try
			{
				Cliente cli = Cliente.GetSingleE(IdCliente);
				Letra = cli.RespInscripto ? "A" : "B";
				Numero = GetNumero();
				IdVendedor = Reglas.VendedorActual.IdVendedor;
				Fecha = Reglas.Now;
				if (Fecha.AddSeconds(-Fecha.Second) <= Reglas.TurnoUltimo.FechaFinal)
					Fecha = Fecha.AddSeconds(-Fecha.Second).AddMinutes(1);
				if (cli.IdCliente == Cliente.ConsFinal.IdCliente)
				{
					decimal totalMaximo = GetTotalMaximo();
					if (totalMaximo > 0 && Total > totalMaximo)
						return new ABMResultado(enErrores.LogicaInvalida, "El total de la factura supera los $" + totalMaximo.ToString("0.00") + " permitidos");
				}
				if (IdPagoForma > 3 && !Reglas.VendedorActual.TienePermiso(enPermisos.Encargado))
					return new ABMResultado(enErrores.Cancelado, "No tiene permiso suficiente para la forma de pago seleccionada.");
				res = DAL.Datos.ABM(DAL.enTablas.FactEncabezados, enABMFuncion.Alta, ProtocoloDatos());
				if (res.CodigoError == enErrores.Ninguno)
					IdFactEncabezado = res.IdInsercion;
				else
					return res;
				//
				if (IdPagoForma == Reglas.PagoCC)//pago a cc
				{
					CCMovimiento mov = new CCMovimiento(this);
					res = mov.Alta();
					if (res.CodigoError != enErrores.Ninguno)
						return res;
				}
				foreach (FactItem it in Cuerpo)
				{
					res = it.Alta();
					if (res.CodigoError != enErrores.Ninguno)
						return res;
				}
				//exito = true;
				return res;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				//if (empeceTransa)
				//    Datos.FinTransa(exito);
			}
		}

		/// <summary>
		/// Termina la transaccion de alta de factura
		/// </summary>
		public ABMResultado ConfirmarAlta(bool exito)
		{
			bool empeceTransa = Datos.InitTransa();
			ABMResultado res;
			try
			{
				if (empeceTransa)//la transaccion ya tendría que estar abierta
					return new ABMResultado(enErrores.LogicaInvalida, "La transacción fue terminada antes de lo debido");
				if (exito)
				{
					res = Modificacion();//por el numero
					if (res.CodigoError != enErrores.Ninguno)
					{
						exito = false;
						return new ABMResultado(enErrores.ErrorBD, "No se pudo modificar el número de la factura." + Environment.NewLine + Environment.NewLine + res.MensajeError); ;
					}
					return new ABMResultado(IdFactEncabezado);
				}
				else
					return new ABMResultado(enErrores.Cancelado, "No se pudo emitir el comprobante fiscal");
			}
			catch (Exception ex)
			{
				exito = false;
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				Datos.FinTransa(exito);
			}
		}

		public ABMResultado Anular(out FactEncabezado factNC)
		{
			bool empeceTransa = Datos.InitTransa();
			bool exito = false;
			ABMResultado res;
			factNC = new FactEncabezado();
			try
			{
				//si esta dentro de un cierre de turno no se puede anular
				Turno tUlt = Reglas.TurnoUltimo;
				if (!Reglas.AnularSiempre && tUlt.FechaFinal >= Fecha)
					return new ABMResultado(enErrores.LogicaInvalida, "No se puede anular una factura emitida en un turno que ya se cerró.");
				//
				Cliente cli = Cliente.GetSingleE(IdCliente);
				factNC.Letra = cli.RespInscripto ? "A" : "B";
				factNC.Numero = Numero;
				factNC.IdCliente = IdCliente;
				factNC.IdVendedor = Reglas.VendedorActual.IdVendedor;
				factNC.Fecha = Reglas.Now;
				factNC.Anula = IdFactEncabezado;
				factNC.IdPagoForma = IdPagoForma;
				decimal totalMaximo = GetTotalMaximo();
				//if (totalMaximo > 0 && Total > totalMaximo)//no hace falta controlar, porque ya se emitió
				//    return new ABMResultado(enErrores.LogicaInvalida, "El total de la factura supera los $" + totalMaximo.ToString("0.00") + " permitidos");
				res = DAL.Datos.ABM(DAL.enTablas.FactEncabezados, enABMFuncion.Alta, factNC.ProtocoloDatos());
				if (res.CodigoError == enErrores.Ninguno)
					factNC.IdFactEncabezado = res.IdInsercion;
				else
					return res;
				//
				Anula = res.IdInsercion;
				res = Modificacion();
				if (res.CodigoError != enErrores.Ninguno)
					return res;
				//
				foreach (FactItem it in Cuerpo)
				{
					FactItem it2 = it.Clonar();
					it2.Cantidad *= -1;
					it2.Monto *= -1;
					factNC.Cuerpo.Add(it2);
					res = it2.Alta();
					if (res.CodigoError != enErrores.Ninguno)
						return res;
				}
				//ver si cc
				if (factNC.IdPagoForma == Reglas.PagoCC)
				{
					CCMovimiento cc = new CCMovimiento(factNC);
					//cc.Monto = this.Total;//así derecho, o sea en positivo porque es a favor del cliente
					//cc.Descripcion = "Reembolso por Nota de crédito";
					res = cc.Alta();
					if (res.CodigoError == enErrores.Ninguno)
						factNC.IdFactEncabezado = res.IdInsercion;
					else
						return res;
				}
				exito = true;
				return res;
			}
			catch (Exception ex)
			{
				factNC = null;
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				if (empeceTransa)
					Datos.FinTransa(exito);
			}
		}

		/// <summary>
		/// Emite diferencias para los cierres totales ya que revisa todos los surtidores (y sus ventas respectivas)
		/// </summary>
		/// <param name="difs">facturas para las diferencias</param>
		/// <returns></returns>
		public static ABMResultado EmitirDiferencia(out FactEncabezado[] difs)
		{
			return EmitirDiferencia(out difs, new SortedList<uint, string>());
		}

		/// <summary>
		/// Emite diferencias para los cierres parciales
		/// </summary>
		/// <param name="difs">facturas para las diferencias</param>
		/// <param name="cierres">solo se revisan los surtidores (y sus ventas respectivas) que esten en estos cierres </param>
		/// <returns></returns>
		public static ABMResultado EmitirDiferencia(out FactEncabezado[] difs, Cierre[] cierres)
		{
			SortedList<uint, string> surtidoresCerrados = new SortedList<uint, string>();
			foreach (Cierre c in cierres)
				surtidoresCerrados.Add(c.Surtidor, "");
			return EmitirDiferencia(out difs, surtidoresCerrados);
		}

		/// <summary>
		/// Emite diferencias para los todos los casos
		/// </summary>
		/// <param name="difs">facturas para las diferencias</param>
		/// <param name="surtidoresCerrados">vacia = todos, si no, que surtidores tener en cuenta</param>
		/// <returns></returns>
		private static ABMResultado EmitirDiferenciaPuaj(out FactEncabezado[] difs, SortedList<uint, string> surtidoresCerrados)
		{
			//bool empeceTransa = Datos.InitTransa();
			//bool exito = false;
			difs = new FactEncabezado[0];
			try
			{//TODOS LOS VALORES SON CANTIDADES 
				SortedList<uint, decimal> facturadoXItem = new SortedList<uint, decimal>();//no tiene la densidad aplicada AHORA
				SortedList<uint, decimal> vendidoXItem = new SortedList<uint, decimal>();
				List<FactEncabezado> factDiferencias = new List<FactEncabezado>();
				foreach (var idArt in Articulo.GetListaIds())
				{
					foreach (Cierre c in Cierre.GetListaXArticuloTurno(idArt, Reglas.TurnoUltimo.Numero + 1))
					{
						if (surtidoresCerrados.Count > 0 && !surtidoresCerrados.ContainsKey(c.Surtidor))
							continue;
						if (!vendidoXItem.ContainsKey(idArt))
						{
							vendidoXItem.Add(idArt, 0);
							facturadoXItem.Add(idArt, 0);
						}
						decimal dif = c.GetDiferencia();
						vendidoXItem[idArt] += dif;
					}
				}
				foreach (DataRow r in FactEncabezado.GetListForFechas(Reglas.TurnoUltimo.FechaFinal, Reglas.Now).Rows)
				{
					FactEncabezado f = new FactEncabezado(r);
					f.GetFilas();
					foreach (FactItem factIt in f.Cuerpo)
					{
						if (facturadoXItem.ContainsKey(factIt.IdArticulo))
							facturadoXItem[factIt.IdArticulo] += factIt.Cantidad;
					}
				}
				foreach (KeyValuePair<uint, decimal> kvp in vendidoXItem)
				{
					//si vendido = facturado => no hago nada
					if (kvp.Value == facturadoXItem[kvp.Key])
						continue;
					decimal noFact;
					//si vendido > facturado => ver c/densidad
					if (kvp.Value > facturadoXItem[kvp.Key])
					{
						Densidad dens = Densidad.GetSingleForArt(kvp.Key);
						//si vendido * densidad <= facturado => no hago nada
						if (Decimal.Round(kvp.Value * dens.Valor, 3) <= facturadoXItem[kvp.Key])
							continue;
						else//entonces aún con la densidad falta facturar
						{
							noFact = kvp.Value * dens.Valor - facturadoXItem[kvp.Key];
							//esNegativo debería ser FALSE
						}
					}
					else//entonces se facturo de más => NC
					{
						noFact = kvp.Value - facturadoXItem[kvp.Key];
						//esNegativo debería ser TRUE
					}
					bool esNegativo = noFact < 0;//para hacer Notas de Credito
					//
					Articulo art = Articulo.GetSingleE(kvp.Key);
					decimal ttMax = GetTotalMaximo();
					for (decimal montoRestante = Math.Abs(art.Precio * noFact); montoRestante > 0;
						montoRestante -= ttMax)
					{
						FactItem item = new FactItem();
						item.Monto = Math.Min(montoRestante, ttMax) * (esNegativo ? -1 : 1);
						item.Cantidad = item.Monto / art.Precio;
						item.DescPlana = art.Descripcion;
						item.IdArticulo = art.IdArticulo;
						item.Precio = art.GetPrecioBase();
						item.IVA = item.Precio * (art.Alicuota / 100);
						item.Impuestos = art.Precio - item.IVA - item.Precio;
						//
						FactEncabezado encaDif = new FactEncabezado();
						encaDif.IdCliente = Cliente.ConsFinal.Id;
						encaDif.Cuerpo.Add(item);
						if (encaDif.Total < 0)
						{
							if (!Reglas.NotaCreditoEnDiferencia)
								return new ABMResultado(enErrores.LogicaInvalida, "Hay facturado más de lo vendido, corrija y vuelva a intentar el cierre." + Environment.NewLine +
									"La diferencia en la facturación es de $" + Math.Abs(art.Precio * noFact).ToString("0.00") + " en " + art.Codigo);
						}
						encaDif.Alta();
						factDiferencias.Add(encaDif);
					}
				}
				difs = factDiferencias.ToArray();
				string factMsj = "";
				foreach (FactEncabezado f in factDiferencias)
				{
					Articulo art = Articulo.GetSingleE(f.Cuerpo[0].IdArticulo);
					factMsj += Environment.NewLine + "\t" + art.Codigo.PadRight(8) + ": \t$ " + f.Total.ToString("0.00").PadLeft(8);
				}
				if (factDiferencias.Count > 0 && !Reglas.Confirmar("Se emitiran las siguientes facturas/notas de crédito por las diferencias:" + Environment.NewLine + factMsj + Environment.NewLine + Environment.NewLine + "¿Desea continuar?"))
					return new ABMResultado(enErrores.Cancelado, "Se canceló la operación");
				//exito = true;
				return new ABMResultado(0);
			}
			catch (Exception ex)
			{
				difs = new FactEncabezado[0];
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				//if (empeceTransa)
				//    Datos.FinTransa(exito);
			}
		}

		/// <summary>
		/// Emite diferencias para los todos los casos
		/// </summary>
		/// <param name="difs">facturas para las diferencias</param>
		/// <param name="surtidoresCerrados">vacia = todos, si no, que surtidores tener en cuenta</param>
		/// <returns></returns>
		private static ABMResultado EmitirDiferencia(out FactEncabezado[] difs, SortedList<uint, string> surtidoresCerrados)
		{
			//bool empeceTransa = Datos.InitTransa();
			//bool exito = false;
			difs = new FactEncabezado[0];
			try
			{//TODOS LOS VALORES SON CANTIDADES 
				SortedList<uint, decimal> facturadoXItem = new SortedList<uint, decimal>();//no tiene la densidad aplicada AHORA
				SortedList<uint, decimal> vendidoXItem = new SortedList<uint, decimal>();
				List<FactEncabezado> factDiferencias = new List<FactEncabezado>();
				foreach (Puesto p in Reglas.Puestos)
				{
					if (surtidoresCerrados.Count > 0 && !surtidoresCerrados.ContainsKey(p.IdPuesto))
						continue;
					foreach (Cierre c in Cierre.GetListaXSurtidorTurno(p.IdPuesto, Reglas.TurnoUltimo.Numero + 1))
					{
						//if (!facturadoXItem.ContainsKey(p.IdArticulo))
						//    facturadoXItem.Add(p.IdArticulo, 0);
						if (!vendidoXItem.ContainsKey(p.IdArticulo))
						{
							vendidoXItem.Add(p.IdArticulo, 0);
							facturadoXItem.Add(p.IdArticulo, 0);
						}
						decimal dif = c.GetDiferencia();
						//Densidad dens = Densidad.GetSingleForArt(p.IdArticulo);
						//facturadoXItem[p.IdArticulo] -= dif * dens.Valor;
						vendidoXItem[p.IdArticulo] += dif;
					}
				}
				foreach (DataRow r in FactEncabezado.GetListForFechas(Reglas.TurnoUltimo.FechaFinal.AddSeconds(1), Reglas.Now).Rows)
				{
					FactEncabezado f = new FactEncabezado(r);
					f.GetFilas();
					foreach (FactItem factIt in f.Cuerpo)
					{
						if (facturadoXItem.ContainsKey(factIt.IdArticulo))
							facturadoXItem[factIt.IdArticulo] += factIt.Cantidad;
					}
				}
				foreach (KeyValuePair<uint, decimal> kvp in vendidoXItem)
				{
					//si vendido = facturado => no hago nada
					if (kvp.Value == facturadoXItem[kvp.Key])
						continue;
					Articulo art = Articulo.GetSingleE(kvp.Key);
					//si no hay que facturar la diferencia => no hago nada
					if (!art.Facturar)
						continue;
					decimal noFact;
					//si vendido > facturado => ver c/densidad
					if (kvp.Value > facturadoXItem[kvp.Key])
					{
						Densidad dens = Densidad.GetSingleForArt(kvp.Key);
						//si vendido * densidad <= facturado => no hago nada
						if (Decimal.Round(kvp.Value * dens.Valor, 3) <= facturadoXItem[kvp.Key])
							continue;
						else//entonces aún con la densidad falta facturar
						{
							noFact = kvp.Value * dens.Valor - facturadoXItem[kvp.Key];
							//esNegativo debería ser FALSE
						}
					}
					else//entonces se facturo de más => NC
					{
						noFact = kvp.Value - facturadoXItem[kvp.Key];
						//esNegativo debería ser TRUE
					}
					bool esNegativo = noFact < 0;//para hacer Notas de Credito
					//
					decimal ttMax = GetTotalMaximo();
					for (decimal montoRestante = Math.Abs(art.Precio * noFact); montoRestante > 0;
						montoRestante -= ttMax)
					{
						FactItem item = new FactItem();
						item.Monto = Math.Min(montoRestante, ttMax) * (esNegativo ? -1 : 1);
						item.Cantidad = item.Monto / art.Precio;
						item.DescPlana = art.Descripcion;
						item.IdArticulo = art.IdArticulo;
						item.Precio = art.GetPrecioBase();
						item.IVA = item.Precio * (art.Alicuota / 100);
						item.Impuestos = art.Precio - item.IVA - item.Precio;
						//
						FactEncabezado encaDif = new FactEncabezado();
						encaDif.IdCliente = Cliente.ConsFinal.Id;
						encaDif.Cuerpo.Add(item);
						if (encaDif.Total < 0)
						{
							if (!Reglas.NotaCreditoEnDiferencia)
								return new ABMResultado(enErrores.LogicaInvalida, "Hay facturado más de lo vendido, corrija y vuelva a intentar el cierre." + Environment.NewLine +
									"La diferencia en la facturación es de $" + Math.Abs(art.Precio * noFact).ToString("0.00") + " en " + art.Codigo);
						}
						encaDif.Alta();
						factDiferencias.Add(encaDif);
					}
				}
				difs = factDiferencias.ToArray();
				SortedList<uint, decimal> difTotalPorItem = new SortedList<uint, decimal>();
				foreach (FactEncabezado f in factDiferencias)
				{
					if (!difTotalPorItem.ContainsKey(f.Cuerpo[0].IdArticulo))
						difTotalPorItem.Add(f.Cuerpo[0].IdArticulo, 0);
					difTotalPorItem[f.Cuerpo[0].IdArticulo] += f.Total;
				}
				string factMsj = "";
				foreach (var x in difTotalPorItem)
				{
					Articulo art = Articulo.GetSingleE(x.Key);
					if (art.Facturar)//solo mostrar si hay que facturar la dif.
						factMsj += Environment.NewLine + "\t" + art.Codigo.PadRight(8) + ": \t$ " + x.Value.ToString("0.00").PadLeft(8);
				}
				if (factDiferencias.Count > 0 && !Reglas.Confirmar("Se emitiran las siguientes facturas/notas de crédito por las diferencias:" + Environment.NewLine + factMsj + Environment.NewLine + Environment.NewLine + "¿Desea continuar?"))
					return new ABMResultado(enErrores.Cancelado, "Se canceló la operación");
				//exito = true;
				return new ABMResultado(0);
			}
			catch (Exception ex)
			{
				difs = new FactEncabezado[0];
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				//if (empeceTransa)
				//    Datos.FinTransa(exito);
			}
		}

		public override ABMResultado Modificacion()
		{
			return DAL.Datos.ABM(DAL.enTablas.FactEncabezados, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			bool empeceTransa = Datos.InitTransa();
			bool exito = false;
			ABMResultado res;
			try
			{
				if (Anula > 0 && Total < 0)
				{
					FactEncabezado anulada = FactEncabezado.GetSingleE(Anula);
					anulada.Anula = 0;
					res = anulada.Modificacion();
					if (res.CodigoError != enErrores.Ninguno)
						return res;
				}
				res = DAL.Datos.ABM(DAL.enTablas.FactEncabezados, enABMFuncion.Baja, ProtocoloDatos());
				if (res.CodigoError != enErrores.Ninguno)
					return res;
				exito = true;
				return res;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.ErrorBD, ex.Message);
			}
			finally
			{
				if (empeceTransa)
					Datos.FinTransa(exito);
			}
		}

		#endregion


		#region IComparable Members

		public int CompareTo(object obj)
		{
			FactEncabezado b = obj as FactEncabezado;
			int res = this.Numero.CompareTo(b.Numero);
			if (res == 0)
				res = this.Letra.CompareTo(b.Letra);
			return res;
		}

		#endregion
	}
}