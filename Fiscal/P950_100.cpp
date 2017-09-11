#include "P950_100.h"


std::string ImpresorFiscalP950_100::Version ("SMH/P-950F - Versi�n 1.00");


// ############################################################ //
//																//
//			M E T O D O S      L O C A L E S					//
//																//
// ############################################################ //

//
// Constructor
//
ImpresorFiscalP950_100::ImpresorFiscalP950_100 ()
{
	// Inicializaci�n de variables de uso general
	MaxFiscalTextLinesInvoice		= 3;	// Por seguridad !!!
	MaxFiscalTextLinesTicket		= 3;

	// Inicializaci�n de variables de tama�o de campos
	PrintNonFiscalTextTicketSize	= 40;
	PrintNonFiscalTextSlipSize		= 80;
	FantasyNameSize					= 40;
	HeaderTrailerSize				= 40;
	PrintFiscalTextTicketSize		= 28;
	PrintFiscalTextSlipSize			= 28;	// Por seguridad !!
	PrintItemTextTicketSize			= 20;
	PrintItemTextSlipSize			= 20;	// Por seguridad !!
	TotalTenderTextSize				= 24;
	VouchImportSize					= 15;	// Por seguridad !!!
	VouchCompNumSize				= 8;	// Por seguridad !!!

	// Inicializaci�n de variables locales de este modelo
	LastIVA							= -1;
	LastII							= -1;
}


//
// Obtener la Descripci�n del Modelo Seleccionado
//
std::string
ImpresorFiscalP950_100::DescripcionModelo () const
{
	return 	(Version);
}


// ############################################################ //
//																//
//			M E T O D O S   G E N E R A L E S					//
//																//
// ############################################################ //

//
// Obtener Datos de Inicializaci�n
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ObtenerDatosDeInicializacion (RTA_ObtenerDatosDeInicializacion * /* R */) throw (Excepcion)
{
    // printf ("Comando ObtenerDatosDeInicializacion ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Obtener Datos de Inicializaci�n");
}


//
// Obtener Datos de Memoria de Trabajo
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ObtenerDatosMemoriaDeTrabajo (RTA_ObtenerDatosMemoriaDeTrabajo * /* R */) throw (Excepcion)
{
    // printf ("Comando ObtenerDatosMemoriaDeTrabajo ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Obtener Datos de Memoria de Trabajo");
}


//
// Estado del Controlador Fiscal
//
// Nota: este m�todo no es v�lido para este modelo.
//
ImpresorFiscal::TiposDeEstadoControlador
ImpresorFiscalP950_100::EstadoControlador () throw(Excepcion)
{
    // printf ("Comando EstadoControlador ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Estado del Controlador Fiscal");
}


//
// Descripci�n del Estado del Controlador Fiscal
//
// Nota: este m�todo no es v�lido para este modelo.
//
std::string
ImpresorFiscalP950_100::DescripcionEstadoControlador () throw(Excepcion)
{
    // printf ("Comando DescripcionEstadoControlador ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Descripci�n del Estado del Controlador Fiscal");
}


//
// Cortar Papel
//
void
ImpresorFiscalP950_100::CortarPapel () throw (Excepcion)
{
    // printf ("Comando CortarPapel ejecutado en P950_100\n");

	// Enviamos el comando fiscal y evaluamos los status
	EnviarComandoFiscal(OpCode(CMD_CUT_NON_FISCAL_RECEIPT));
}


//
// Imprimir C�digo de Barras
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ImprimirCodigoDeBarras (TiposDeCodigoDeBarras /* Tipo */,
										const std::string & /* Codigo */,
										bool /* ImprimeNumeros */,
										bool /* ImprimeAhora */)  throw(Excepcion)
{
    // printf ("Comando ImprimirCodigoDeBarras ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Imprimir C�digo de Barras");
}


// Capacidad de impresi�n en estaci�n Slip / Ticket
void
ImpresorFiscalP950_100::CapacidadEstacion (bool &SoportaTicket, bool &SoportaSlip)
{
    // printf ("Comando CapacidadEstacion ejecutado en P950_100\n");

	SoportaTicket = true;
	SoportaSlip = true;
}


// ############################################################ //
//																//
//			M E T O D O S   D E    C O N F I G U R A C I O N	//
//																//
// ############################################################ //

//
// Especificar L�nea de Nombre de Fantas�a
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::EspecificarNombreDeFantasia (unsigned /* Linea */,
												 const std::string & /* Texto */) throw (Excepcion)
{
    // printf ("Comando EspecificarNombreDeFantasia ejecutado en P950_100\n");
	
	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Especificar L�nea de Nombre de Fantas�a");
}


//
// Configurar Controlador por Par�metros Individuales
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ConfigurarControlador (ParametrosDeConfiguracion /* Parametro */,
										   const std::string & /* Valor */) throw (Excepcion)
{
    // printf ("Comando ConfigurarControlador ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Configurar Controlador por Par�metros Individuales");
}


//
// Configurar Controlador por Bloque de Par�metros
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ConfigurarControladorPorBloque (
							double /* LimiteConsumidorFinal */,
							double /* LimiteTicketFactura */,
							double /* PorcentajeIVANoInscripto */,
							NumerosDeCopias /* NumeroDeCopiasMaximo */,
							bool /* ImpresionCambio */,
							bool /* ImpresionLeyendasOpcionales */,
							TiposDeCorteDePapel /* TipoDeCorte */) throw (Excepcion)
{
    // printf ("Comando ConfigurarControladorPorBloque ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Configurar Controlador por Bloque de Par�metros");
}


//
// Obtener Configuracion del Controlador
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ObtenerConfiguracion (RTA_ObtenerConfiguracion * /* R */) throw (Excepcion)
{
    // printf ("Comando ObtenerConfiguracion ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Obtener Configuracion del Controlador");
}


//
// Obtener Tabla de IVAs
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ObtenerTablaIVAs (TablaIVAs & /* Tabla */) throw (Excepcion)
{
    // printf ("Comando ObtenerTablaIVAs ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Obtener Tabla de IVAs");
}


// ############################################################ //
//																//
//			M E T O D O S   D E    R E P O R T E S				//
//																//
// ############################################################ //

//
// Reporte de Cierre Z Individual por Fecha
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ReporteZIndividualPorFecha (FECHA /* FechaZ */, RTA_ReporteZIndividual * /* R */) throw (Excepcion)
{
    // printf ("Comando ReporteZIndividualPorFecha ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Reporte de Cierre Z Individual por Fecha");
}


//
// Reporte de Cierre Z Individual por N�mero
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ReporteZIndividualPorNumero (unsigned /* NumeroZ */, RTA_ReporteZIndividual * /* R */) throw (Excepcion)
{
    // printf ("Comando ReporteZIndividualPorNumero ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Reporte de Cierre Z Individual por N�mero");
}


// ############################################################ //
//																//
//			M E T O D O S   D E    D F							//
//																//
// ############################################################ //

//
// Abrir DF
// Env�a el comando de Apertura de DF en la estaci�n indicada
//
void
ImpresorFiscalP950_100::AbrirDF (DocumentosFiscales Tipo) throw (Excepcion)
{
    // printf ("Comando AbrirDF ejecutado en P950_100\n");

	// Verificamos si el Tipo de DF es v�lido para este modelo
	if ( Tipo != TICKET_C )
		throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Abrir DF");

	// Ejecutamos el m�todo de la SuperClase 'ImpresorFiscal8Bits'
	ImpresorFiscal8Bits::AbrirDF (Tipo);

	// Reinicializamos 'FirstDocNum', 'LastIVA' y 'LastII'
	FirstDocNum = 0;
	LastIVA = -1;
	LastII	= -1;
}


//
// Imprimir Item
//
void
ImpresorFiscalP950_100::ImprimirItem (      const std::string &Texto,
							double Cantidad, double Monto,
							double IVA, double ImpuestosInternos,
							bool EnNegativo) throw (Excepcion)
{
    // printf ("Comando ImprimirItem ejecutado en P950_100\n");

	// Ejecutamos el m�todo de la superclase 'ImpresorFiscal'
	ImpresorFiscal::ImprimirItem (Texto, Cantidad, Monto, IVA, ImpuestosInternos, EnNegativo);

	// Almacenamos IVA e II de la venta para un potencial
	// posterior Descuento a UltimoItem.
	LastIVA = IVA;
	LastII	= ImpuestosInternos;
}


//
// Descuento a Ultimo Item
//
// Nota: en este modelo (por no contar con el comando asociado)
//		 este m�todo se implementa como una venta.
//
void
ImpresorFiscalP950_100::DescuentoUltimoItem (const std::string &Texto, double Monto, bool EnNegativo) throw (Excepcion)
{
    // printf ("Comando DescuentoUltimoItem ejecutado en P950_100\n");

	unsigned PrintItemTextSize = PrintItemTextTicketSize;

	assert(PrintItemTextSize != 0);

	// Verificamos si existi� una venta previa.
	if ( LastIVA == -1 || LastII == -1 )
		throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_ESTADO_NO_VALIDO, "Descuento a Ultimo Item");

	// Ejecutamos el m�todo de la superclase 'ImpresorFiscal'
	ImpresorFiscal::ImprimirItem (Texto, 1.00, Monto, LastIVA, LastII, EnNegativo);

	// Reinicializamos 'LastIVA' y 'LastII'
	LastIVA = -1;
	LastII	= -1;
}


//
// Descuento General
//
// Nota: en este modelo (por no contar con el comando asociado)
//		 este m�todo se implementa como una venta.
//
void
ImpresorFiscalP950_100::DescuentoGeneral (const std::string &Texto, double Monto, bool EnNegativo) throw (Excepcion)
{
    // printf ("Comando DescuentoGeneral ejecutado en P950_100\n");

	unsigned PrintItemTextSize = PrintItemTextTicketSize;

	assert(PrintItemTextSize != 0);

	// Enviamos el comando fiscal y evaluamos los status
	EnviarComandoFiscal(OpCode(CMD_PRINT_LINE_ITEM) + FS +
								Cadena (Texto, PrintItemTextSize) + FS +
								Numero (1, 10) + FS +
								Numero (Monto, PriceDecimals) + FS +
								Cadena ("**.**", 5) + FS +
								(EnNegativo ? "m" : "M") + FS +
								Numero (0, 2) + FS +
								Caracter (ModoDisplay) + FS +
								Caracter (ModalidadPrecio));

	// Reinicializamos 'LastIVA' y 'LastII'
	LastIVA = -1;
	LastII	= -1;
}


//
// Bonificaci�n, Recargo y Devoluci�n de Envases
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::DevolucionDescuento (
							const std::string & /* Texto */,
							double /* Monto */, double /* IVA */,
							double /* ImpuestosInternos */,
							bool /* EnNegativo */,
							TiposDeDescuento /* Tipo */) throw (Excepcion)
{
    // printf ("Comando DevolucionDescuento ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Bonificaci�n, Recargo y Devoluci�n de Envases");
}


// ############################################################ //
//																//
//			M E T O D O S   D E    D N F						//
//																//
// ############################################################ //

//
// Abrir DNF
// Env�a el comando de Apertura de DNF en la estaci�n indicada
//
void
ImpresorFiscalP950_100::AbrirDNF (TiposDeEstacion EstacionDeImpresion) throw (Excepcion)
{
    // printf ("Comando AbrirDNF ejecutado en P950_100\n");

	// Ejecutamos el m�todo de la SuperClase 'ImpresorFiscal'.
	ImpresorFiscal::AbrirDNF (EstacionDeImpresion);
}


// ############################################################ //
//																//
//			M E T O D O S   D E    D N F H						//
//																//
// ############################################################ //

//
// Emitir un DNFH de Farmacia
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::DNFHFarmacia (unsigned /* Copias */) throw (Excepcion)
{
    // printf ("Comando DNFHFarmacia ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Emitir un DNFH de Farmacia");
}


//
// Emitir un DNFH de Reparto
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::DNFHReparto (unsigned /* Copias */) throw (Excepcion)
{
    // printf ("Comando DNFHReparto ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Emitir un DNFH de Reparto");
}


//
// Imprimir Voucher de Tarjeta
//
// Nota: este m�todo no es v�lido para este modelo.
//
void
ImpresorFiscalP950_100::ImprimirVoucher (
							const std::string & /* NombreCliente */,
							const std::string & /* NombreTarjeta */,
							TiposDeVouchers /* TipoVoucher */,
							const std::string & /* NumeroDeTarjeta */,
							FECHA /* FechaDeVencimiento */,
							TiposDeTarjetas /* TipoTarjeta */,
							unsigned /* Cuotas */,
							const std::string & /* CodigoDeComercio */,
							unsigned /* NumeroDeTerminal */,
							unsigned /* NumeroDeLote */,
							unsigned /* NumeroCupon */,
							TiposDeIngresoDeTarjeta /* TipoIngreso */,
							TiposOperacionDeTarjeta /* TipoOperacion */,
							unsigned /* NumeroAutorizacion */,
							const std::string & /* Monto */,
							const std::string & /* NumeroComprobanteAsociado */,
							unsigned /* Copias */,
							const std::string & /* Vendedor */,
							TiposDeEstacion /* Estacion */) throw (Excepcion)
{
    // printf ("Comando ImprimirVoucher ejecutado en P950_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Imprimir Voucher de Tarjeta");
}


