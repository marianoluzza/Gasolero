#include "PPL9_100.h"


std::string ImpresorFiscalPPL9_100::Version ("HASAR SMH/PL-9F V: 01.00" /* " - 15/02/2005" */);


// ############################################################ //
//																//
//			M E T O D O S      L O C A L E S					//
//																//
// ############################################################ //

//
// Constructor
//
ImpresorFiscalPPL9_100::ImpresorFiscalPPL9_100 ()
{
	// Inicialización de variables de uso general

	// Inicialización de variables de tamaño de campos
	PrintNonFiscalTextTicketSize	= 120;	// Por seguridad !!!	
	PrintFiscalTextTicketSize		= 50;	// Por seguridad !!!
	PrintItemTextTicketSize			= 50;	// Por seguridad !!!
	RemitoCantDecimals				= 10;
	ReciboTextTicketSize			= 106;	// Por seguridad !!!
}


//
// Obtener la Descripción del Modelo Seleccionado
//
std::string
ImpresorFiscalPPL9_100::DescripcionModelo () const
{
	return 	(Version);
}


// ############################################################ //
//																//
//			M E T O D O S   G E N E R A L E S					//
//																//
// ############################################################ //

//
// Obtener Datos de Memoria de Trabajo
//
void
ImpresorFiscalPPL9_100::ObtenerDatosMemoriaDeTrabajo (RTA_ObtenerDatosMemoriaDeTrabajo *R) throw (Excepcion)
{
    // printf ("Comando ObtenerDatosMemoriaDeTrabajo ejecutado en PPL9_100\n");

	// Ejecutamos el método de la superclase 'ImpresorFiscal16Bits'
	ImpresorFiscal16Bits::ObtenerDatosMemoriaDeTrabajo (R);

	// Completamos los parámetros correspondientes a los campos
	// de respuesta particulares que se agregan en este modelo
	// más alla de los comunes a todos los modelos
	if (R != NULL)
		R->CantidadNCCanceladas			= RespuestaInt (20, "Obtener Datos de Memoria de Trabajo");
}


// ############################################################ //
//																//
//			M E T O D O S   D E    C O N F I G U R A C I O N	//
//																//
// ############################################################ //



// ############################################################ //
//																//
//			M E T O D O S   D E    R E P O R T E S				//
//																//
// ############################################################ //



// ############################################################ //
//																//
//			M E T O D O S   D E    D F							//
//																//
// ############################################################ //

//
// Abrir DF
// Envía el comando de Apertura de DF en la estación indicada
//
void
ImpresorFiscalPPL9_100::AbrirDF (DocumentosFiscales Tipo) throw (Excepcion)
{
    // printf ("Comando AbrirDF ejecutado en PPL9_100\n");

	// Verificamos si el Tipo de DF es válido para este modelo
	if ( !(	Tipo == FACTURA_A		||
			Tipo == FACTURA_B		||
			Tipo == NOTA_DEBITO_A	||
			Tipo == NOTA_DEBITO_B	||
			Tipo == RECIBO_A		||
			Tipo == RECIBO_B ) )
		throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Abrir DF");

	// Ejecutamos el método de la SuperClase 'ImpresorFiscal16Bits'
	ImpresorFiscal16Bits::AbrirDF (Tipo);
}


//
// Especificar Monto de IVA No Inscripto
//
void
ImpresorFiscalPPL9_100::EspecificarIVANoInscripto (double /* Monto */) throw (Excepcion)
{
    // printf ("Comando EspecificarIVANoInscripto ejecutado en PPL9_100\n");

	throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Especificar Monto de IVA No Inscripto");
}


//
// Cerrar DF
//
unsigned
ImpresorFiscalPPL9_100::CerrarDF (unsigned Copias) throw (Excepcion)
{
    // printf ("Comando CerrarDF ejecutado en PPL9_100\n");

	// Ejecutamos el método de la superclase 'ImpresorFiscal16Bits'
	unsigned NumeroDFRecienEmitido = ImpresorFiscal16Bits::CerrarDF (Copias);

	// Recuperamos el Número de Hojas Numeradas del DF
	// recién emitido de la respuesta.
	NumberOfPages = RespuestaInt(3, "Cerrar DF");

	// Retornamos el Número de DF recién emitido
	return NumeroDFRecienEmitido;
}


// ############################################################ //
//																//
//			M E T O D O S   D E    D N F						//
//																//
// ############################################################ //

//
// Abrir DNF
// Envía el comando de Apertura de DNF en la estación indicada
//
void
ImpresorFiscalPPL9_100::AbrirDNF (TiposDeEstacion EstacionDeImpresion) throw (Excepcion)
{
    // printf ("Comando AbrirDNF ejecutado en PPL9_100\n");

	// Ejecutamos el método de la SuperClase 'ImpresorFiscal'.
	ImpresorFiscal::AbrirDNF (ESTACION_SLIP);
}


//
// Cerrar DNF
//
void
ImpresorFiscalPPL9_100::CerrarDNF (unsigned Copias) throw (Excepcion)
{
    // printf ("Comando CerrarDNF ejecutado en PPL9_100\n");

	// Ejecutamos el método de la superclase 'ImpresorFiscal16Bits'.
	ImpresorFiscal16Bits::CerrarDNF (Copias);
}


// ############################################################ //
//																//
//			M E T O D O S   D E    D N F H						//
//																//
// ############################################################ //

//
// Abrir DNFH
// Envía el comando de Apertura de DNFH en la estación indicada
//
void
ImpresorFiscalPPL9_100::AbrirDNFH (DocumentosNoFiscalesHomologados Tipo, const std::string &Nro) throw (Excepcion)
{
    // printf ("Comando AbrirDNFH ejecutado en PPL9_100\n");

	// Verificamos si el Tipo de DF es válido para este modelo
	if ( !(	Tipo == NOTA_CREDITO_A	         ||
			Tipo == NOTA_CREDITO_B	         ||
			Tipo == REMITO			         ||
			Tipo == RECIBO_X		         ||
			Tipo == ORDEN_SALIDA	         ||
			Tipo == RESUMEN_CUENTA	         ||
			Tipo == CARGO_HABITACION         ||
			Tipo == COTIZACION               ||
            Tipo == CLAUSULA_CREDITO         ||
            Tipo == CLAUSULA_SEGURO          ||
            Tipo == PAGARE                   ||
            Tipo == POLIZA_SEGURO            ||
            Tipo == RECORDATORIO             ||
            Tipo == SOLICITUD_CREDITO        ||
            Tipo == COMUNICACION_CLIENTE	 ||
            Tipo == OFRECIMIENTO_CREDITO	 ||
            Tipo == OFRECIMIENTO_TARJETA	 ||
            Tipo == MINUTA_CREDITO			 ||
            Tipo == OFRECIMIENTO_PASAPORTE	 ||
            Tipo == RENOVACION_CREDITO		 ||
            Tipo == ADELANTO_REMUNERACION	 ||
            Tipo == SOLICITUD_TARJETA_DEBITO ||
            Tipo == SOLICITUD_CLAVE_TARJETA	 ||
            Tipo == RESCATE_MERCADERIA		 ||
            Tipo == INGRESO_EGRESO_SUCURSAL	) )
		throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Abrir DNFH");

	// Ejecutamos el método de la SuperClase 'ImpresorFiscal16Bits'
	ImpresorFiscal16Bits::AbrirDNFH (Tipo, Nro);
}


//
// Cerrar DNFH
//
unsigned
ImpresorFiscalPPL9_100::CerrarDNFH (unsigned Copias) throw (Excepcion)
{
    // printf ("Comando CerrarDNFH ejecutado en PPL9_100\n");

	// Ejecutamos el método de la superclase 'ImpresorFiscal16Bits'
	unsigned NumeroDNFHRecienEmitido = ImpresorFiscal16Bits::CerrarDNFH (Copias);

	// Recuperamos el Número de Hojas Numeradas del DNFH
	// recién emitido de la respuesta.
	NumberOfPages = RespuestaInt(3, "Cerrar DNFH");

	// Retornamos el Número de DNFH recién emitido
	return NumeroDNFHRecienEmitido;
}


