using System;
using System.Collections.Generic;
using System.Text;
using Negocio;
using FiscalPrinterLib;
using MarUtils.Controles;
using MarUtils.Soporte;
using System.Windows.Forms;

namespace Gasolero
{
	[Obsolete("Impresiones viejas, usar Fiscal solo", true)]
	public class FiscalOLd
	{
		static HASAR hasarCte = null;
		const int TICKET_LONG = 44;

		#region Imprimir
		public static ABMResultado Imprimir(Cliente cli, FactEncabezado fact)
		{
			bool exito;
			ABMResultado res;
			try
			{
				if (!Reglas.UsarFiscal)
				{
					exito = true;
				}
				else
				{
					string letra = "X";//Settings.Default.Imprimir
					switch (letra)
					{
						case "B":
							exito = ImprimirB(cli, fact);
							break;
						case "C":
							exito = ImprimirC(cli, fact);
							break;
						case "D":
							exito = ImprimirD(cli, fact);
							break;
						case "E":
							exito = ImprimirE(cli, fact);
							break;
						case "F":
							exito = ImprimirF(cli, fact);
							break;
						case "H":
						case "H1":
							exito = ImprimirH(cli, fact, false);
							break;
						case "H2":
							exito = ImprimirH(cli, fact, true);
							break;
						default:
							exito = ImprimirE(cli, fact);
							break;
					}
				}
				res = fact.ConfirmarAlta(exito);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				res = fact.ConfirmarAlta(false);
			}
			return res;
		}

		/// <summary>
		/// ImpInt Fijos
		/// </summary>
		static bool ImprimirA(Cliente cli, FactEncabezado fact)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				//hasar.Puerto = 4;// (int)NumPuerto.Value;
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = true;// false;
				hasar.kIVA = false;
				hasar.ImpuestoInternoFijo = true;
				hasar.ImpuestoInternoPorMonto = true;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				//
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				bool fiscal = true;
				if (!cli.RespInscripto)
				{
					Cliente consFinal = Cliente.ConsFinal;
					cli.CUIT = cli.CUIT.Trim();
					string cuit = CUITLimpio(cli.CUIT);
					if (fact.Total >= 0)
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_B);
					}
					else
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						else
						{//si era el cli CONS FINAL, usar el cliente esp para este caso
							Cliente cliNCB = cli;
							try { cliNCB = Cliente.GetSingleE(Reglas.ClienteNCB); }
							catch { }
							hasar.DatosCliente(cliNCB.Nombre, CUITLimpio(cliNCB.CUIT), TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cliNCB.Direccion);
						}
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(1, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, fact.Numero);
						//hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, "");
						fiscal = false;
					}
				}
				else
				{
					TiposDeResponsabilidades tResp = cli.RespInscripto ? TiposDeResponsabilidades.RESPONSABLE_INSCRIPTO : TiposDeResponsabilidades.RESPONSABLE_EXENTO;
					hasar.DatosCliente(cli.Nombre, CUITLimpio(cli.CUIT), TiposDeDocumento.TIPO_CUIT, tResp, cli.Direccion);
					if (fact.Total >= 0)
					{
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_A);
					}
					else
					{
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(1, "0000-0000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_A, fact.Numero);
						fiscal = false;
					}
				}
				//hasar.set_Encabezado(1, "Encabezado Mio");
				//hasar.set_Encabezado(2, "Encabezado Otro");
				decimal pago = 0;
				foreach (FactItem item in fact.Cuerpo)
				{
					Articulo art = Articulo.GetSingleE(item.IdArticulo);
					//hasar.ImprimirItem(item.Descripcion, (double)Math.Abs(item.Cantidad), (double)item.Precio, (double)decimal.Round(item.IVA / item.Precio * 100, 3), (double)item.Impuestos);
					double cant = (double)Math.Abs(item.Cantidad);
					double precio = (double)art.GetPrecioBase();//(double)art.Precio;//item.Precio;
					double alicuota = (double)art.Alicuota;//decimal.Round(item.IVA / item.Precio, 1);
					double impint;// = (double)item.Impuestos;
					if (art.ImpuestoPorcentual)
						impint = precio * (double)art.Impuesto / 100;
					else
						impint = (double)art.Impuesto;
					hasar.ImprimirItem(item.Descripcion, cant, precio, alicuota, impint);
					//
					FactItemFiscal itemFiscal = new FactItemFiscal();
					itemFiscal.Cantidad = (decimal)cant;
					itemFiscal.Descripcion = item.Descripcion;
					itemFiscal.IdArticulo = item.IdArticulo;
					itemFiscal.IdFactEncabezado = fact.IdFactEncabezado;
					itemFiscal.ImpInt = (decimal)impint;
					itemFiscal.IVA = (decimal)alicuota;
					itemFiscal.ModoImpresion = "A";
					itemFiscal.Monto = (decimal)precio;
					itemFiscal.Alta();
					//
					pago += Math.Abs(item.Monto);
				}
				object resto;
				hasar.ImprimirPago("Efectivo", (double)pago, "", out resto);
				try
				{
					double restoDouble = -1;
					if (double.TryParse(resto.ToString(), out restoDouble) &&
						restoDouble > 0)
						hasar.ImprimirPago("Efectivo", restoDouble, "", out resto);
				}
				catch { }
				object n;
				if (fiscal)
					hasar.CerrarComprobanteFiscal(1, out n);
				else
					hasar.CerrarDNFH(1, out n);
				if (n != null)
				{
					object[] vals = new object[8];
					object ptoVta;
					hasar.ObtenerDatosDeInicializacion(out vals[0], out vals[1], out vals[2], out vals[3],
						out ptoVta, out vals[4], out vals[5], out vals[6]);
					if (ptoVta != null)
						fact.Numero = ptoVta.ToString().PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
					else
						fact.Numero = n.ToString().PadLeft(8, '0');
					fact.Modificacion();
				}
				return true;
			}
			catch (Exception e)
			{
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// ImpInt %
		/// </summary>
		static bool ImprimirB(Cliente cli, FactEncabezado fact)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = false;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				//
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				bool fiscal = true;
				if (!cli.RespInscripto)
				{
					Cliente consFinal = Cliente.ConsFinal;
					cli.CUIT = cli.CUIT.Trim();
					string cuit = CUITLimpio(cli.CUIT);
					if (fact.Total >= 0)
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_B);
					}
					else
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						else
						{//si era el cli CONS FINAL, usar el cliente esp para este caso
							Cliente cliNCB = cli;
							try { cliNCB = Cliente.GetSingleE(Reglas.ClienteNCB); }
							catch { }
							hasar.DatosCliente(cliNCB.Nombre, CUITLimpio(cliNCB.CUIT), TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cliNCB.Direccion);
						}
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
							hasar.set_DocumentoDeReferencia(1, ".");
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, fact.Numero);
						//hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, "");
						fiscal = false;
					}
				}
				else
				{
					TiposDeResponsabilidades tResp = cli.RespInscripto ? TiposDeResponsabilidades.RESPONSABLE_INSCRIPTO : TiposDeResponsabilidades.RESPONSABLE_EXENTO;
					hasar.DatosCliente(cli.Nombre, CUITLimpio(cli.CUIT), TiposDeDocumento.TIPO_CUIT, tResp, cli.Direccion);
					if (fact.Total >= 0)
					{
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_A);
					}
					else
					{
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
							hasar.set_DocumentoDeReferencia(1, ".");
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_A, fact.Numero);
						fiscal = false;
					}
				}
				//hasar.set_Encabezado(1, "Encabezado Mio");
				//hasar.set_Encabezado(2, "Encabezado Otro");
				decimal pago = 0;
				foreach (FactItem item in fact.Cuerpo)
				{
					Articulo art = Articulo.GetSingleE(item.IdArticulo);
					//hasar.ImprimirItem(item.Descripcion, (double)Math.Abs(item.Cantidad), (double)item.Precio, (double)decimal.Round(item.IVA / item.Precio * 100, 3), (double)item.Impuestos);
					double cant = (double)Math.Abs(item.Cantidad);
					double precio = (double)art.Precio;//(double)item.Precio;
					double alicuota = (double)art.Alicuota;
					double impint = (double)art.Impuesto;
					if (!art.ImpuestoPorcentual)
						impint = impint / (double)art.GetPrecioBase() * 100;
					hasar.ImprimirItem(item.Descripcion, cant, precio, alicuota, impint);
					//
					FactItemFiscal itemFiscal = new FactItemFiscal();
					itemFiscal.Cantidad = (decimal)cant;
					itemFiscal.Descripcion = item.Descripcion;
					itemFiscal.IdArticulo = item.IdArticulo;
					itemFiscal.IdFactEncabezado = fact.IdFactEncabezado;
					itemFiscal.ImpInt = (decimal)impint;
					itemFiscal.IVA = (decimal)alicuota;
					itemFiscal.ModoImpresion = "B";
					itemFiscal.Monto = (decimal)precio;
					itemFiscal.Alta();
					//
					pago += Math.Abs(item.Monto);
				}
				object resto;
				hasar.ImprimirPago("Efectivo", (double)pago, "", out resto);
				try
				{
					double restoDouble = -1;
					if (double.TryParse(resto.ToString(), out restoDouble) &&
						restoDouble > 0)
						hasar.ImprimirPago("Efectivo", restoDouble, "", out resto);
				}
				catch { }
				object n;
				if (fiscal)
					hasar.CerrarComprobanteFiscal(1, out n);
				else
					hasar.CerrarDNFH(1, out n);
				if (n != null)
				{
					object[] vals = new object[8];
					object ptoVta;
					hasar.ObtenerDatosDeInicializacion(out vals[0], out vals[1], out vals[2], out vals[3],
						out ptoVta, out vals[4], out vals[5], out vals[6]);
					if (ptoVta != null)
						fact.Numero = ptoVta.ToString().PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
					else
						fact.Numero = n.ToString().PadLeft(8, '0');
					fact.Modificacion();
				}
				return true;
			}
			catch (Exception e)
			{
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// Modo A o B según ii de la venta
		/// </summary>
		static bool ImprimirC(Cliente cli, FactEncabezado fact)
		{
			Articulo art = Articulo.GetSingleE(fact.Cuerpo[0].IdArticulo);
			if (art.ImpuestoPorcentual)
				return ImprimirB(cli, fact);
			else
				return ImprimirA(cli, fact);
		}

		/// <summary>
		/// ImpInt K
		/// </summary>
		static bool ImprimirD(Cliente cli, FactEncabezado fact)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = true;
				hasar.kIVA = true;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				bool fiscal = true;
				if (!cli.RespInscripto)
				{
					Cliente consFinal = Cliente.ConsFinal;
					cli.CUIT = cli.CUIT.Trim();
					string cuit = CUITLimpio(cli.CUIT);
					if (fact.Total >= 0)
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_B);
					}
					else
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						else
						{//si era el cli CONS FINAL, usar el cliente esp para este caso
							Cliente cliNCB = cli;
							try { cliNCB = Cliente.GetSingleE(Reglas.ClienteNCB); }
							catch { }
							hasar.DatosCliente(cliNCB.Nombre, CUITLimpio(cliNCB.CUIT), TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cliNCB.Direccion);
						}
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							//hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, fact.Numero);
						//hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, "");
						fiscal = false;
					}
				}
				else
				{
					TiposDeResponsabilidades tResp = cli.RespInscripto ? TiposDeResponsabilidades.RESPONSABLE_INSCRIPTO : TiposDeResponsabilidades.RESPONSABLE_EXENTO;
					hasar.DatosCliente(cli.Nombre, CUITLimpio(cli.CUIT), TiposDeDocumento.TIPO_CUIT, tResp, cli.Direccion);
					if (fact.Total >= 0)
					{
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_A);
					}
					else
					{
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							//hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_A, fact.Numero);
						fiscal = false;
					}
				}
				//hasar.set_Encabezado(1, "Encabezado Mio");
				//hasar.set_Encabezado(2, "Encabezado Otro");
				decimal pago = 0;
				foreach (FactItem item in fact.Cuerpo)
				{
					Articulo art = Articulo.GetSingleE(item.IdArticulo);
					//
					double cant = (double)Math.Abs(item.Cantidad);
					double precio = (double)art.GetPrecioBase();
					double alicuota = (double)art.Alicuota;
					double impint = (double)art.Impuesto;
					if (!art.ImpuestoPorcentual)
						impint = impint / precio * 100;
					double k = 1 / (1 + impint / 100);
					double monto = precio * cant;
					string desc = Math.Abs(item.Cantidad).ToString() + (art.IdArticulo == 4 ? "M3" : "Lts") + "/$" + decimal.Round(art.Precio, 3).ToString() + " " + item.Descripcion;
					hasar.ImprimirItem(desc, 1, monto, alicuota, k);
					//hasar.ImprimirTextoNoFiscal("PF x " + (art.IdArticulo == 4 ? "M3" : "Lts") + ": " + Math.Abs(item.Monto).ToString());
					//
					FactItemFiscal itemFiscal = new FactItemFiscal();
					itemFiscal.Cantidad = 1;
					itemFiscal.Descripcion = desc;
					itemFiscal.IdArticulo = item.IdArticulo;
					itemFiscal.IdFactEncabezado = fact.IdFactEncabezado;
					itemFiscal.ImpInt = (decimal)k;
					itemFiscal.IVA = (decimal)alicuota;
					itemFiscal.ModoImpresion = "D";
					itemFiscal.Monto = (decimal)monto;
					itemFiscal.Alta();
					//
					pago += Math.Abs(item.Monto);
				}
				if (fiscal)
				{
					object resto;
					hasar.ImprimirPago("Efectivo", (double)pago, "", out resto);
					try
					{
						double restoDouble = -1;
						if (double.TryParse(resto.ToString(), out restoDouble) &&
							restoDouble > 0)
							hasar.ImprimirPago("Efectivo", restoDouble, "", out resto);
					}
					catch { }
				}
				object n;
				if (fiscal)
					hasar.CerrarComprobanteFiscal(1, out n);
				else
					hasar.CerrarDNFH(1, out n);
				if (n != null)
				{
					string ptoVta = PuestoDeVenta();
					if (ptoVta != "")
						fact.Numero = ptoVta.PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
					else
						fact.Numero = n.ToString().PadLeft(8, '0');
					ABMResultado abmRes = fact.Modificacion();
					if (abmRes.CodigoError != enErrores.Ninguno)
						Reglas.AddLog("Error en Fiscal - Mod num.||" + abmRes.CodigoError.ToString() + "||" + abmRes.MensajeError);
				}
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error en modulo fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// ImpInt K
		/// </summary>
		static bool ImprimirE(Cliente cli, FactEncabezado fact)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = true;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				bool fiscal = true;
				#region Apertura Comprobante
				if (!cli.RespInscripto)
				{
					Cliente consFinal = Cliente.ConsFinal;
					cli.CUIT = cli.CUIT.Trim();
					string cuit = CUITLimpio(cli.CUIT);
					if (fact.Total >= 0)
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_B);
					}
					else
					{
						if (cli.Nombre != consFinal.Nombre || Reglas.ClienteNCB == 1)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion ?? "-");
						//hasar.DatosCliente(cli.Nombre, "00000000", TiposDeDocumento.TIPO_DNI, TiposDeResponsabilidades.CONSUMIDOR_FINAL, ".");
						else
						{//si era el cli CONS FINAL, usar el cliente esp para este caso
							Cliente cliNCB = cli;
							try { cliNCB = Cliente.GetSingleE(Reglas.ClienteNCB); }
							catch { }
							hasar.DatosCliente(cliNCB.Nombre, CUITLimpio(cliNCB.CUIT), TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cliNCB.Direccion);
						}
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							//hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							//hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							//hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, fact.Numero);
						//hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, "");
						fiscal = false;
					}
				}
				else
				{
					TiposDeResponsabilidades tResp = cli.RespInscripto ? TiposDeResponsabilidades.RESPONSABLE_INSCRIPTO : TiposDeResponsabilidades.RESPONSABLE_EXENTO;
					hasar.DatosCliente(cli.Nombre, CUITLimpio(cli.CUIT), TiposDeDocumento.TIPO_CUIT, tResp, cli.Direccion);
					if (fact.Total >= 0)
					{
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_A);
					}
					else
					{
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							//hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_A, fact.Numero);
						fiscal = false;
					}
				}
				#endregion Apertura Comprobante
				//hasar.set_Encabezado(1, "Encabezado Mio");
				//hasar.set_Encabezado(2, "Encabezado Otro");
				decimal pago = 0;
				foreach (FactItem item in fact.Cuerpo)
				{
					Articulo art = Articulo.GetSingleE(item.IdArticulo);
					//
					double cant = (double)Math.Abs(item.Cantidad);
					double precio = (double)art.GetPrecioBase();
					double alicuota = (double)art.Alicuota;
					double impint = (double)art.Impuesto;
					if (!art.ImpuestoPorcentual)
						impint = impint / precio * 100;
					double k = 1 / (1 + impint / 100);
					//double monto = precio * cant;
					double monto = (double)item.Monto;
					string desc = Math.Abs(item.Cantidad).ToString() + (art.IdArticulo == 4 ? "M3" : "Lts") + "/$" + decimal.Round(art.Precio, 3).ToString() + " " + item.Descripcion;
					hasar.ImprimirItem(desc, 1, Math.Abs(monto), alicuota, k);
					//
					FactItemFiscal itemFiscal = new FactItemFiscal();
					itemFiscal.Cantidad = 1;
					itemFiscal.Descripcion = desc;
					itemFiscal.IdArticulo = item.IdArticulo;
					itemFiscal.IdFactEncabezado = fact.IdFactEncabezado;
					itemFiscal.ImpInt = (decimal)k;
					itemFiscal.IVA = (decimal)alicuota;
					itemFiscal.ModoImpresion = "E";
					itemFiscal.Monto = (decimal)monto;
					itemFiscal.Alta();
					//
					pago += Math.Abs(item.Monto);
				}
				if (fiscal)
				{
					object resto;
					hasar.ImprimirPago("Efectivo", (double)pago, "", out resto);
					try
					{
						double restoDouble = -1;
						if (double.TryParse(resto.ToString(), out restoDouble) &&
							restoDouble > 0)
							hasar.ImprimirPago("Efectivo", restoDouble, "", out resto);
					}
					catch { }
				}
				object n;
				if (fiscal)
					hasar.CerrarComprobanteFiscal(1, out n);
				else
					hasar.CerrarDNFH(1, out n);
				if (n != null)
				{
					string ptoVta = PuestoDeVenta();
					if (ptoVta != "")
						fact.Numero = ptoVta.PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
					else
						fact.Numero = n.ToString().PadLeft(8, '0');
					ABMResultado abmRes = fact.Modificacion();
					if (abmRes.CodigoError != enErrores.Ninguno)
						Reglas.AddLog("Error en Fiscal - Mod num.||" + abmRes.CodigoError.ToString() + "||" + abmRes.MensajeError);
				}
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error en modulo fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// ImpInt Fijos y Precio Final
		/// </summary>
		static bool ImprimirF(Cliente cli, FactEncabezado fact)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = false;
				hasar.ImpuestoInternoFijo = true;
				hasar.ImpuestoInternoPorMonto = true;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				//
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				bool fiscal = true;
				#region Apertura Comprobante
				if (!cli.RespInscripto)
				{
					Cliente consFinal = Cliente.ConsFinal;
					cli.CUIT = cli.CUIT.Trim();
					string cuit = CUITLimpio(cli.CUIT);
					if (fact.Total >= 0)
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_B);
					}
					else
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						else
						{//si era el cli CONS FINAL, usar el cliente esp para este caso
							Cliente cliNCB = cli;
							try { cliNCB = Cliente.GetSingleE(Reglas.ClienteNCB); }
							catch { }
							hasar.DatosCliente(cliNCB.Nombre, CUITLimpio(cliNCB.CUIT), TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cliNCB.Direccion);
						}
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, fact.Numero);
						fiscal = false;
					}
				}
				else
				{
					TiposDeResponsabilidades tResp = cli.RespInscripto ? TiposDeResponsabilidades.RESPONSABLE_INSCRIPTO : TiposDeResponsabilidades.RESPONSABLE_EXENTO;
					hasar.DatosCliente(cli.Nombre, CUITLimpio(cli.CUIT), TiposDeDocumento.TIPO_CUIT, tResp, cli.Direccion);
					if (fact.Total >= 0)
					{
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_A);
					}
					else
					{
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_A, fact.Numero);
						fiscal = false;
					}
				}
				#endregion Apertura Comprobante
				//hasar.set_Encabezado(1, "Encabezado Mio");
				//hasar.set_Encabezado(2, "Encabezado Otro");
				decimal pago = 0;
				foreach (FactItem item in fact.Cuerpo)
				{
					Articulo art = Articulo.GetSingleE(item.IdArticulo);
					double cant = (double)Math.Abs(item.Cantidad);
					double precio = (double)art.Precio;
					double alicuota = (double)art.Alicuota;
					double impint = (double)art.Impuesto;
					if (art.ImpuestoPorcentual)
						impint = precio / (100 + impint + alicuota) * impint;
					//
					hasar.ImprimirItem(item.Descripcion, cant, precio, alicuota, impint);
					//
					FactItemFiscal itemFiscal = new FactItemFiscal();
					itemFiscal.Cantidad = (decimal)cant;
					itemFiscal.Descripcion = item.Descripcion;
					itemFiscal.IdArticulo = item.IdArticulo;
					itemFiscal.IdFactEncabezado = fact.IdFactEncabezado;
					itemFiscal.ImpInt = (decimal)impint;
					itemFiscal.IVA = (decimal)alicuota;
					itemFiscal.ModoImpresion = "F";
					itemFiscal.Monto = (decimal)precio;
					itemFiscal.Alta();
					//
					pago += Math.Abs(item.Monto);
				}
				object resto;
				hasar.ImprimirPago("Efectivo", (double)pago, "", out resto);
				try
				{
					double restoDouble = -1;
					if (double.TryParse(resto.ToString(), out restoDouble) &&
						restoDouble > 0)
						hasar.ImprimirPago("Efectivo", restoDouble, "", out resto);
				}
				catch { }
				object n;
				if (fiscal)
					hasar.CerrarComprobanteFiscal(1, out n);
				else
					hasar.CerrarDNFH(1, out n);
				if (n != null)
				{
					string ptoVta = PuestoDeVenta();
					if (ptoVta != "")
						fact.Numero = ptoVta.PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
					else
						fact.Numero = n.ToString().PadLeft(8, '0');
					ABMResultado abmRes = fact.Modificacion();
					if (abmRes.CodigoError != enErrores.Ninguno)
						Reglas.AddLog("Error en Fiscal - Mod num.||" + abmRes.CodigoError.ToString() + "||" + abmRes.MensajeError);
				}
				return true;
			}
			catch (Exception e)
			{
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// Fiscal proj
		/// </summary>
		static bool ImprimirG(Cliente cli, FactEncabezado fact)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = true;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				bool fiscal = true;
				#region Apertura Comprobante
				if (!cli.RespInscripto)
				{
					Cliente consFinal = Cliente.ConsFinal;
					cli.CUIT = cli.CUIT.Trim();
					string cuit = CUITLimpio(cli.CUIT);
					if (fact.Total >= 0)
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_B);
					}
					else
					{
						if (cli.Nombre != consFinal.Nombre || Reglas.ClienteNCB == 1)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion ?? "-");
						//hasar.DatosCliente(cli.Nombre, "00000000", TiposDeDocumento.TIPO_DNI, TiposDeResponsabilidades.CONSUMIDOR_FINAL, ".");
						else
						{//si era el cli CONS FINAL, usar el cliente esp para este caso
							Cliente cliNCB = cli;
							try { cliNCB = Cliente.GetSingleE(Reglas.ClienteNCB); }
							catch { }
							hasar.DatosCliente(cliNCB.Nombre, CUITLimpio(cliNCB.CUIT), TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cliNCB.Direccion);
						}
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							//hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, fact.Numero);
						//hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, "");
						fiscal = false;
					}
				}
				else
				{
					TiposDeResponsabilidades tResp = cli.RespInscripto ? TiposDeResponsabilidades.RESPONSABLE_INSCRIPTO : TiposDeResponsabilidades.RESPONSABLE_EXENTO;
					hasar.DatosCliente(cli.Nombre, CUITLimpio(cli.CUIT), TiposDeDocumento.TIPO_CUIT, tResp, cli.Direccion);
					if (fact.Total >= 0)
					{
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_A);
					}
					else
					{
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							//hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_A, fact.Numero);
						fiscal = false;
					}
				}
				#endregion Apertura Comprobante
				//hasar.set_Encabezado(1, "Encabezado Mio");
				//hasar.set_Encabezado(2, "Encabezado Otro");
				decimal pago = 0;
				foreach (FactItem item in fact.Cuerpo)
				{
					Articulo art = Articulo.GetSingleE(item.IdArticulo);
					//
					double cant = (double)Math.Abs(item.Cantidad);
					double precio = (double)art.GetPrecioBase();
					double alicuota = (double)art.Alicuota;
					double impint = (double)art.Impuesto;
					if (!art.ImpuestoPorcentual)
						impint = impint / precio * 100;
					double k = 1 / (1 + impint / 100);
					//double monto = precio * cant;
					double monto = (double)item.Monto;
					string desc = Math.Abs(item.Cantidad).ToString() + (art.IdArticulo == 4 ? "M3" : "Lts") + "/$" + decimal.Round(art.Precio, 3).ToString() + " " + item.Descripcion;
					hasar.ImprimirItem(desc, 1, Math.Abs(monto), alicuota, k);
					//
					FactItemFiscal itemFiscal = new FactItemFiscal();
					itemFiscal.Cantidad = 1;
					itemFiscal.Descripcion = desc;
					itemFiscal.IdArticulo = item.IdArticulo;
					itemFiscal.IdFactEncabezado = fact.IdFactEncabezado;
					itemFiscal.ImpInt = (decimal)k;
					itemFiscal.IVA = (decimal)alicuota;
					itemFiscal.ModoImpresion = "G";
					itemFiscal.Monto = (decimal)monto;
					itemFiscal.Alta();
					//
					pago += Math.Abs(item.Monto);
				}
				if (fiscal)
				{
					object resto;
					hasar.ImprimirPago("Efectivo", (double)pago, "", out resto);
					try
					{
						double restoDouble = -1;
						if (double.TryParse(resto.ToString(), out restoDouble) &&
							restoDouble > 0)
							hasar.ImprimirPago("Efectivo", restoDouble, "", out resto);
					}
					catch { }
				}
				object n;
				if (fiscal)
					hasar.CerrarComprobanteFiscal(1, out n);
				else
					hasar.CerrarDNFH(1, out n);
				if (n != null)
				{
					string ptoVta = PuestoDeVenta();
					if (ptoVta != "")
						fact.Numero = ptoVta.PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
					else
						fact.Numero = n.ToString().PadLeft(8, '0');
					ABMResultado abmRes = fact.Modificacion();
					if (abmRes.CodigoError != enErrores.Ninguno)
						Reglas.AddLog("Error en Fiscal - Mod num.||" + abmRes.CodigoError.ToString() + "||" + abmRes.MensajeError);
				}
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error en modulo fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// ImpInt K, modo E con exento
		/// </summary>
		static bool ImprimirH(Cliente cli, FactEncabezado fact, bool exentoFiscal)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = true;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				bool fiscal = true;
				#region Apertura Comprobante
				if (!cli.RespInscripto)
				{
					Cliente consFinal = Cliente.ConsFinal;
					cli.CUIT = cli.CUIT.Trim();
					string cuit = CUITLimpio(cli.CUIT);
					if (fact.Total >= 0)
					{
						if (cli.Nombre != consFinal.Nombre)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_B);
					}
					else
					{
						if (cli.Nombre != consFinal.Nombre || Reglas.ClienteNCB == 1)
							hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion ?? "-");
						//hasar.DatosCliente(cli.Nombre, "00000000", TiposDeDocumento.TIPO_DNI, TiposDeResponsabilidades.CONSUMIDOR_FINAL, ".");
						else
						{//si era el cli CONS FINAL, usar el cliente esp para este caso
							Cliente cliNCB = cli;
							try { cliNCB = Cliente.GetSingleE(Reglas.ClienteNCB); }
							catch { }
							hasar.DatosCliente(cliNCB.Nombre, CUITLimpio(cliNCB.CUIT), TiposDeDocumento.TIPO_CUIT, TiposDeResponsabilidades.CONSUMIDOR_FINAL, cliNCB.Direccion);
						}
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							//hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, fact.Numero);
						//hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_B, "");
						fiscal = false;
					}
				}
				else
				{
					TiposDeResponsabilidades tResp = cli.RespInscripto ? TiposDeResponsabilidades.RESPONSABLE_INSCRIPTO : TiposDeResponsabilidades.RESPONSABLE_EXENTO;
					hasar.DatosCliente(cli.Nombre, CUITLimpio(cli.CUIT), TiposDeDocumento.TIPO_CUIT, tResp, cli.Direccion);
					if (fact.Total >= 0)
					{
						hasar.AbrirComprobanteFiscal(DocumentosFiscales.TICKET_FACTURA_A);
					}
					else
					{
						if (fact.Anula > 0)
						{
							FactEncabezado fOrig = FactEncabezado.GetSingleE(fact.Anula);
							//hasar.set_DocumentoDeReferencia(0, fOrig.Numero);
							hasar.set_DocumentoDeReferencia(1, fOrig.Numero);
						}
						else
						{
							//hasar.set_DocumentoDeReferencia(0, "0000-00000000");
							hasar.set_DocumentoDeReferencia(1, ".");
						}
						hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_NOTA_CREDITO_A, fact.Numero);
						fiscal = false;
					}
				}
				#endregion Apertura Comprobante
				//hasar.set_Encabezado(1, "Encabezado Mio");
				//hasar.set_Encabezado(2, "Encabezado Otro");
				decimal pago = 0;
				decimal dtoK = 1 - cli.Descuento / 100;
				foreach (FactItem item in fact.Cuerpo)
				{
					Articulo art = Articulo.GetSingleE(item.IdArticulo);
					//
					double cant = (double)Math.Abs(item.Cantidad);
					double precio = (double)art.GetPrecioBase();
					double alicuota = (double)art.Alicuota;
					double impint = (double)art.Impuesto;
					if (!art.ImpuestoPorcentual)
						impint = impint / precio * 100;
					double k = 1 / (1 + impint / 100);
					//double monto = precio * cant;
					//double monto = (double)item.Monto;//tiene el dto
					double monto = (double)item.Monto / (double)dtoK;//el dto sumado -> monto 100%
					string desc = Math.Abs(item.Cantidad).ToString() + (art.IdArticulo == 4 ? "M3" : "Lts") + "/$" + decimal.Round(art.Precio, 3).ToString() + " " + item.Descripcion;
					hasar.ImprimirItem(desc, 1, Math.Abs(monto), alicuota, k);
					//
					FactItemFiscal itemFiscal = new FactItemFiscal();
					itemFiscal.Cantidad = 1;
					itemFiscal.Descripcion = desc;
					itemFiscal.IdArticulo = item.IdArticulo;
					itemFiscal.IdFactEncabezado = fact.IdFactEncabezado;
					itemFiscal.ImpInt = (decimal)k;
					itemFiscal.IVA = (decimal)alicuota;
					itemFiscal.ModoImpresion = "H";
					itemFiscal.Monto = (decimal)monto;
					itemFiscal.Descuento = cli.Descuento;
					itemFiscal.Alta();
					//
					pago += Math.Abs(item.Monto);
				}
				//if (cli.Exento)//mostrar EXENTO//NO FUNCIONÓ ACÁ, da error fiscal.
				//{
				//    if (exentoFiscal)
				//        hasar.ImprimirTextoFiscal("Responsable Exento");
				//    else
				//        hasar.ImprimirTextoNoFiscal("Responsable Exento");
				//}
				double dto = (double)(pago * (cli.Descuento / 100));
#warning VER DESCUENTO
				if (dto > 0)
					hasar.DescuentoGeneral("Descuento", dto, true);
				if (fiscal)
				{
					object resto;
					string strFormaDePago = "Efectivo";
					try
					{
						PagoForma formaDePago = PagoForma.GetSingleE(fact.IdPagoForma);
						strFormaDePago = formaDePago.Descripcion;
					}
					catch (Exception exFP)
					{
						Reglas.AddLog(exFP);
					}
					hasar.ImprimirPago(strFormaDePago, (double)pago - dto, "", out resto);
					try
					{
						double restoDouble = -1;
						if (double.TryParse(resto.ToString(), out restoDouble) &&
							restoDouble > 0)
							hasar.ImprimirPago(strFormaDePago, restoDouble, "", out resto);
					}
					catch { }
				}
				object n;
				if (fiscal)
					hasar.CerrarComprobanteFiscal(1, out n);
				else
					hasar.CerrarDNFH(1, out n);
				if (n != null)
				{
					string ptoVta = PuestoDeVenta();
					if (ptoVta != "")
						fact.Numero = ptoVta.PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
					else
						fact.Numero = n.ToString().PadLeft(8, '0');
					ABMResultado abmRes = fact.Modificacion();
					if (abmRes.CodigoError != enErrores.Ninguno)
						Reglas.AddLog("Error en Fiscal - Mod num.||" + abmRes.CodigoError.ToString() + "||" + abmRes.MensajeError);
				}
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error en modulo fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// Imprimir vale de carga. Comprobante no fiscal
		/// </summary>
		public static bool ImprimirVale(Operacion op, Vehiculo veh = null)
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = new HASARClass();
			bool seImprimio = false;
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = true;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				//
				hasar.AbrirComprobanteNoFiscal();
				string desc = Articulo.GetDescXCodProd(op.CodProducto);
				desc = op.Cantidad.ToString("0.00") + " " + (desc.ToLower().Contains("gnc") ? "M3" : "Lts") + " de " + desc;
				hasar.ImprimirTextoNoFiscal(op.Fecha.ToString());
				hasar.ImprimirTextoNoFiscal(desc);
				Vehiculo v = veh;
				if (veh == null)
				{
					v = op.GetConsumidor();
					try { v.AltaSegura(); }
					catch (Exception ex)
					{
						Reglas.AddLog(ex);
					}
				}
				hasar.ImprimirTextoNoFiscal("N. Oblea: " + op.IdLlave);//oblea, llave o tarjeta
				hasar.ImprimirTextoNoFiscal("N. Int. Movil: " + v.IdInterna);
				for (int i = 0; i < v.Descripcion.Length; i += TICKET_LONG)
				{
					int len = Math.Min(TICKET_LONG, v.Descripcion.Length - i);
					hasar.ImprimirTextoNoFiscal(v.Descripcion.Substring(i, len));
				}
				hasar.ImprimirTextoNoFiscal("Firma:");
				hasar.ImprimirTextoNoFiscal("");
				hasar.ImprimirTextoNoFiscal("Aclaración:");
				hasar.ImprimirTextoNoFiscal("Documento:");
				hasar.CerrarComprobanteNoFiscal();
				seImprimio = true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error en modulo fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Reglas.AddLog(e);
				seImprimio = false;
			}
			finally
			{
				try
				{
					hasar.Finalizar();
					if (seImprimio)
					{
						Reglas.UltimoIdImpreso = op.IdOperacion;
						while (Reglas.UltimoIdImpreso != op.IdOperacion && MessageBox.Show("Ocurrió un error al guardar el ID del vale impreso" + Environment.NewLine + "¿Desea reintentar?", "Error en BD", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
							Reglas.UltimoIdImpreso = op.IdOperacion;
						//
						//ABMResultado abmResOp = op.Alta();
						//if(abmResOp.CodigoError != enErrores.Ninguno)
						//    MessageBox.Show(abmResOp.MensajeError, "Error al guardar vale", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
			return seImprimio;
		}

		public static bool ImprimirRecibo(Cliente cli, string txtRecibo, double monto)
		{
			if (!Reglas.UsarFiscal || !Reglas.CCFiscal)
				return true;
			HASAR hasar = new HASARClass();
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = true;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				#region Apertura Comprobante
				Cliente consFinal = Cliente.ConsFinal;
				cli.CUIT = cli.CUIT.Trim();
				string cuit = CUITLimpio(cli.CUIT);
				if (cli.Nombre != consFinal.Nombre)
					hasar.DatosCliente(cli.Nombre, cuit, cuit == "" ? TiposDeDocumento.TIPO_NINGUNO : TiposDeDocumento.TIPO_CUIT, cli.Exento ? TiposDeResponsabilidades.RESPONSABLE_EXENTO : TiposDeResponsabilidades.CONSUMIDOR_FINAL, cli.Direccion);
				hasar.AbrirDNFH(DocumentosNoFiscales.TICKET_RECIBO_X, "1");
				#endregion Apertura Comprobante
				object n = null;
				hasar.ImprimirItem("Recibimos", 1, monto, 0, 0);
				hasar.DetalleRecibo(txtRecibo);//en concepto de...
#warning ver forma de pago
				hasar.CerrarDNFH(1, out n);
				//if (n != null)
				//{
				//    string ptoVta = PuestoDeVenta();
				//    if (ptoVta != "")
				//        fact.Numero = ptoVta.PadLeft(4, '0') + "-" + n.ToString().PadLeft(8, '0');
				//    else
				//        fact.Numero = n.ToString().PadLeft(8, '0');
				//    ABMResultado abmRes = fact.Modificacion();
				//    if (abmRes.CodigoError != enErrores.Ninguno)
				//        Reglas.AddLog("Error en Fiscal - Mod num.||" + abmRes.CodigoError.ToString() + "||" + abmRes.MensajeError);
				//}
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error en modulo fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Reglas.AddLog(e);
				return false;
			}
			finally
			{
				hasar.Finalizar();
			}
		}

		/// <summary>
		/// Imprimir movimiento de cuenta cliente. Comprobante no fiscal
		/// </summary>
		public static bool ImprimirCCMov(CCMovimiento mov)
		{
			if (!Reglas.UsarFiscal || !Reglas.CCFiscal)
				return true;
			HASAR hasar = new HASARClass();
			bool seImprimio = false;
			hasarCte = hasar;
			try
			{
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.PrecioBase = false;
				hasar.kIVA = true;
				hasar.ImpuestoInternoFijo = false;
				hasar.ImpuestoInternoPorMonto = false;
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.EventosIndividuales = true;
				hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
				hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
				//
				Cliente c = Cliente.GetSingleE(mov.IdCliente);
				string saldo = CCMovimiento.GetSaldoForCliente(mov.IdCliente, mov.Fecha.AddSeconds(2)).ToString("$ 0.00");
				hasar.AbrirComprobanteNoFiscal();
				hasar.ImprimirTextoNoFiscal("Movimiento de Cuenta Corriente (" + mov.IdCCMovimiento + ")");
				hasar.ImprimirTextoNoFiscal(mov.Fecha.ToString());
				hasar.ImprimirTextoNoFiscal("Cliente: " + c.Nombre);
				for (int i = 0; i < mov.Descripcion.Length; i += TICKET_LONG)
				{
					int len = Math.Min(TICKET_LONG, mov.Descripcion.Length - i);
					hasar.ImprimirTextoNoFiscal(mov.Descripcion.Substring(i, len));
				}
				hasar.ImprimirTextoNoFiscal("Monto: " + mov.Monto.ToString("$ 0.00"));
				hasar.ImprimirTextoNoFiscal("Saldo a la fecha: " + saldo);
				hasar.ImprimirTextoNoFiscal("Firma:");
				hasar.ImprimirTextoNoFiscal("");
				hasar.ImprimirTextoNoFiscal("Aclaración: " + mov.NombreAutorizado);
				hasar.ImprimirTextoNoFiscal("Documento: " + mov.DNIAutorizado);
				hasar.ImprimirTextoNoFiscal("Patente: " + mov.PatenteAutorizado);
				hasar.CerrarComprobanteNoFiscal();
				seImprimio = true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error en modulo fiscal", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Reglas.AddLog(e);
				seImprimio = false;
			}
			finally
			{
				try
				{
					hasar.Finalizar();
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
			return seImprimio;
		}
		#endregion

		static void hasar_ErrorImpresora(int Flags)
		{
			if (hasarCte != null)
				System.Windows.Forms.MessageBox.Show(hasarCte.DescripcionStatusImpresor(Flags), "Error Impresora");
			else
				System.Windows.Forms.MessageBox.Show(Flags.ToString(), "Error Impresora");
		}

		static void hasar_ErrorFiscal(int Flags)
		{
			if (hasarCte != null)
				System.Windows.Forms.MessageBox.Show(hasarCte.DescripcionStatusFiscal(Flags), "Error Fiscal");
			else
				System.Windows.Forms.MessageBox.Show(Flags.ToString(), "Error Fiscal");
		}

		public static bool CierreZ()
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = null;
			try
			{
				hasar = new HASARClass();
				object[] r = new object[50];
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				hasar.ReporteZ(out r[0], out r[1], out r[2], out r[3], out r[4], out r[5],
					out r[6], out r[7], out r[8], out r[9], out r[10], out r[11],
					out r[12], out r[13], out r[14], out r[15], out r[16], out r[17],
					out r[18], out r[19], out r[20], out r[21], out r[22], out r[23], out r[24]);
				//
				ABMResultado abmRes = null;
				bool reintentar = false;
				do
				{
					abmRes = ZCierre.RegistrarCierre(Fecha(), r);
					if (abmRes.CodigoError != enErrores.Ninguno)
					{
						Reglas.AddLog("ZCierre.RegistrarCierre FALLO");
						Reglas.AddLog(abmRes.MensajeError);
						Reglas.AddLog("ZCierre.RegistrarCierre FIN FALLO");
						reintentar = MessageBox.Show("No se pudo guardar la información del cierre Z" + Environment.NewLine + "¿Desea intentar nuevamente?", "Error en la BD",
							MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
					}
				} while (abmRes.CodigoError != enErrores.Ninguno && reintentar);
				return true;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return false;
			}
			finally
			{
				if (hasar != null)
					hasar.Finalizar();
			}
		}

		public static bool CierreZHastaAhora()
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = null;
			try
			{
				hasar = new HASARClass();
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				DateTime fIni = AppConfig.UltimoZ;
				DateTime fFin = Reglas.Now;
				bool global = AppConfig.ZGlobal;
				hasar.ReporteZPorFechas(fIni, fFin, global);
				AppConfig.UltimoZ = fFin;
				return true;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return false;
			}
			finally
			{
				if (hasar != null)
					hasar.Finalizar();
			}
		}

		public static bool CierreZPorFechas()
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = null;
			try
			{
				hasar = new HASARClass();
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				//object[] retornos = new object[20];
				EsquemaCollection esquemas = new EsquemaCollection();
				Esquema esq = new Esquema();
				esq.Dato = "Desde";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				esq = new Esquema();
				esq.Dato = "Hasta";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				esq = new Esquema();
				esq.Dato = "Global";
				esq.Tipo = Esquema.enControles.CheckBox;
				esquemas.Add(esq);
				Prompt pr = new Prompt(esquemas, new Dictionary<Esquema, System.Data.DataView>());
				if (pr.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return false;
				DateTime fIni = DateTime.Parse(pr.GetValue("Desde").ToString()).Date;
				DateTime fFin = DateTime.Parse(pr.GetValue("Hasta").ToString()).Date.AddDays(1).AddSeconds(-1);
				bool global = pr.GetValue("Global").ToString().ToUpper() == "SI" || pr.GetValue("Global").ToString() == "1" || pr.GetValue("Global").ToString().ToLower() == "true";
				hasar.ReporteZPorFechas(fIni, fFin, global);
				return true;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return false;
			}
			finally
			{
				if (hasar != null)
					hasar.Finalizar();
			}
		}

		public static bool CierreZPorNum()
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = null;
			try
			{
				hasar = new HASARClass();
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				//object[] retornos = new object[20];
				EsquemaCollection esquemas = new EsquemaCollection();
				Esquema esq = new Esquema();
				esq.Dato = "Desde";
				esq.Tipo = Esquema.enControles.Money;
				esq.Mascara = "00000";
				esquemas.Add(esq);
				esq = new Esquema();
				esq.Dato = "Hasta";
				esq.Tipo = Esquema.enControles.Money;
				esq.Mascara = "00000";
				esquemas.Add(esq);
				esq = new Esquema();
				esq.Dato = "Global";
				esq.Tipo = Esquema.enControles.CheckBox;
				esquemas.Add(esq);
				Prompt pr = new Prompt(esquemas, new Dictionary<Esquema, System.Data.DataView>());
				if (pr.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return false;
				int fIni = int.Parse(pr.GetValue("Desde").ToString());
				int fFin = int.Parse(pr.GetValue("Hasta").ToString());
				bool global = pr.GetValue("Global").ToString().ToUpper() == "SI" || pr.GetValue("Global").ToString() == "1" || pr.GetValue("Global").ToString().ToLower() == "true";
				hasar.ReporteZPorNumeros(fIni, fFin, global);
				return true;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return false;
			}
			finally
			{
				if (hasar != null)
					hasar.Finalizar();
			}
		}

		public static bool CierreZPorFechasXls()
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = null;
			try
			{
				hasar = new HASARClass();
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				//object[] retornos = new object[20];
				EsquemaCollection esquemas = new EsquemaCollection();
				Esquema esq = new Esquema();
				esq.Dato = "Desde";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				esq = new Esquema();
				esq.Dato = "Hasta";
				esq.Tipo = Esquema.enControles.DateTimePicker;
				esquemas.Add(esq);
				Prompt pr = new Prompt(esquemas, new Dictionary<Esquema, System.Data.DataView>());
				if (pr.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return false;
				DateTime fIni = DateTime.Parse(pr.GetValue("Desde").ToString());
				DateTime fFin = DateTime.Parse(pr.GetValue("Hasta").ToString()).AddDays(1).AddSeconds(-1);
				object[] retornos = new object[25];
				System.Data.DataTable dt = new System.Data.DataTable();
				dt.Columns.Add("Fecha", typeof(DateTime));
				dt.Columns.Add("Num. Z", typeof(int));
				dt.Columns.Add("UltimoDocFisBC");
				dt.Columns.Add("UltimoDocFisA");
				dt.Columns.Add("Monto Vta Doc Fiscal", typeof(decimal));
				dt.Columns.Add("Monto IVA Doc Fiscal", typeof(decimal));
				dt.Columns.Add("Monto ImpInt Doc Fiscal", typeof(decimal));
				dt.Columns.Add("Monto Percepciones Doc Fiscal", typeof(decimal));
				dt.Columns.Add("Monto IVA No Insc. Doc Fiscal", typeof(decimal));
				dt.Columns.Add("UltimaNCBC");
				dt.Columns.Add("UltimoNCA");
				dt.Columns.Add("Monto Vta NC", typeof(decimal));
				dt.Columns.Add("Monto IVA NC", typeof(decimal));
				dt.Columns.Add("Monto ImpInt NC", typeof(decimal));
				dt.Columns.Add("Monto Percepciones NC", typeof(decimal));
				dt.Columns.Add("Monto IVA No Insc. NC", typeof(decimal));
				dt.Columns.Add("UltimoRemito");
				for (DateTime d = fIni.Date; d <= fFin.Date; d = d.AddDays(1))
				{
					hasar.ReporteZIndividualPorFecha(d, out retornos[0], out retornos[1], out retornos[2],
						out retornos[3], out retornos[4], out retornos[5],
						out retornos[6], out retornos[7],
						out retornos[8], out retornos[9],
						out retornos[10], out retornos[11],
						out retornos[12], out retornos[13],
						out retornos[14], out retornos[15],
						out retornos[16]);
					dt.Rows.Add(retornos);
				}
				Reporte rpt = new Reporte();
				rpt.HacerReporteXls(dt, "Reportes Z " + fIni.ToShortDateString() + " - " + fFin.ToShortDateString());
				var x = rpt.Grabar();
				System.Diagnostics.Process.Start(x);
				MessageBox.Show("Reporte: " + x, "Reporte creado exitosamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return true;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return false;
			}
			finally
			{
				if (hasar != null)
					hasar.Finalizar();
			}
		}

		public static bool CierreX()
		{
			if (!Reglas.UsarFiscal)
				return true;
			HASAR hasar = null;
			try
			{
				hasar = new HASARClass();
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				object[] retornos = new object[25];
				hasar.ReporteX(out retornos[0], out retornos[1], out retornos[2],
					out retornos[3], out retornos[4], out retornos[5],
					out retornos[6], out retornos[7],
					out retornos[8], out retornos[9],
					out retornos[10], out retornos[11],
					out retornos[12], out retornos[13],
					out retornos[14], out retornos[15],
					out retornos[16], out retornos[17], out retornos[18],
					out retornos[19], out retornos[20], out retornos[21],
					out retornos[22], out retornos[23], out retornos[24]);
				if (Reglas.ReporteXZALog)
				{
					Reglas.AddLog("***********************************************");
					Reglas.AddLog("Reporte X");
					foreach (object o in retornos)
					{
						if (o != null)
							Reglas.AddLog(o.ToString());
					}
					Reglas.AddLog("***********************************************");
				}
				return true;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return false;
			}
			finally
			{
				if (hasar != null)
					hasar.Finalizar();
			}
		}

		public static DateTime Fecha()
		{
			try
			{
				if (!Reglas.UsarFiscal)
					return Reglas.NowServidor;
				HASAR hasar = null;
				try
				{
					hasar = new HASARClass();
					hasar.Puerto = Reglas.PuertoFiscal;
					hasar.Modelo = ModeloFiscal();
					hasar.Comenzar();
					hasar.TratarDeCancelarTodo();
					return hasar.FechaHoraFiscal;
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					return Reglas.NowServidor;
				}
				finally
				{
					if (hasar != null)
						hasar.Finalizar();
				}
			}
			catch
			{
				return Reglas.NowServidor;
			}
		}

		public static ModelosDeImpresoras ModeloFiscal()
		{
			ModelosDeImpresoras res = ModelosDeImpresoras.MODELO_715;
			try
			{
				int modelo = Reglas.ModeloFiscal;
				switch (modelo)
				{
					case 441:
						res = ModelosDeImpresoras.MODELO_P441;
						break;
					case 441201:
						res = ModelosDeImpresoras.MODELO_P441_201;
						break;
					case 435:
						res = ModelosDeImpresoras.MODELO_P435;
						break;
					case 715:
						res = ModelosDeImpresoras.MODELO_715;
						break;
					case 715201:
						res = ModelosDeImpresoras.MODELO_715_201;
						break;
					case 715302:
						res = ModelosDeImpresoras.MODELO_715_302;
						break;
					case 715403:
						res = ModelosDeImpresoras.MODELO_715_403;
						break;
					default:
						res = (ModelosDeImpresoras)modelo;
						break;
				}
			}
			catch
			{
			}
			return res;
		}

		public static bool Instalado()
		{
			try
			{
				HASAR hasar = null;
				hasar = new HASARClass();
				hasar.Puerto = Reglas.PuertoFiscal;
				hasar.Modelo = ModeloFiscal();
				hasar.Comenzar();
				hasar.TratarDeCancelarTodo();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string PuestoDeVenta()
		{
			if (!String.IsNullOrEmpty(Reglas.FactPuestoDeVenta))
				return Reglas.FactPuestoDeVenta;
			string res = "";
			object[] vals = new object[8];
			object ptoVta;
			if (!Reglas.UsarFiscal)
			{
				return "0000";
			}
			else if (hasarCte == null)
			{
				try
				{
					HASAR hasar = new HASARClass();
					hasar.ErrorFiscal += new _FiscalEvents_ErrorFiscalEventHandler(hasar_ErrorFiscal);
					hasar.ErrorImpresora += new _FiscalEvents_ErrorImpresoraEventHandler(hasar_ErrorImpresora);
					hasarCte = hasar;
				}
				catch
				{
					return "0000";
				}
			}
			try
			{
				hasarCte.Puerto = Reglas.PuertoFiscal;
				hasarCte.Modelo = ModeloFiscal();
				hasarCte.PrecioBase = false;
				hasarCte.kIVA = true;
				hasarCte.ImpuestoInternoFijo = false;
				hasarCte.ImpuestoInternoPorMonto = false;
				hasarCte.Comenzar();
				hasarCte.TratarDeCancelarTodo();
				hasarCte.EventosIndividuales = true;
				hasarCte.ObtenerDatosDeInicializacion(out vals[0], out vals[1], out vals[2], out vals[3],
				out ptoVta, out vals[4], out vals[5], out vals[6]);
				if (ptoVta != null)
					res = ptoVta.ToString();
				else
					res = "";
				Reglas.FactPuestoDeVenta = res;
				return res;
			}
			catch (Exception e)
			{
				Reglas.AddLog(e);
				return "0000";
			}
			finally
			{
				if (hasarCte != null)
					hasarCte.Finalizar();
			}
		}

		public static bool CUITCheck(string cuit)
		{
			try
			{
				string slimpio = cuit.Replace("-", "").Trim();
				int[] S = new int[11];
				int i = 0;
				foreach (char c in slimpio)
					S[i++] = int.Parse(c.ToString());
				int v2 = (S[0] * 5 +
					S[1] * 4 +
					S[2] * 3 +
					S[3] * 2 +
					S[4] * 7 +
					S[5] * 6 +
					S[6] * 5 +
					S[7] * 4 +
					S[8] * 3 +
					S[9] * 2) % 11;
				int v3 = 11 - v2;
				switch (v3)
				{
					case 11:
						v3 = 0;
						break;
					case 10:
						v3 = 9;
						break;
				}
				return S[10] == v3;
			}
			catch
			{
				return false;
			}
		}

		public static string CUITLimpio(string cuit)
		{
			string res = cuit.Replace("-", "");
			return res.Trim();
		}
	}
}
