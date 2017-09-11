#include "P425.h"



// ############################################################ //
//																//
//			M E T O D O S      L O C A L E S					//
//																//
// ############################################################ //

//
// Constructor
//
ImpresorFiscalP425::ImpresorFiscalP425 ()
{
	// Inicialización de variables de uso general

	// Inicialización de variables de tamaño de campos
	PrintNonFiscalTextTicketSize	= 45;
	PrintFiscalTextTicketSize		= 31;
	PrintItemTextTicketSize			= 23;
	RemitoCantDecimals				= 10;
	ReciboTextTicketSize			= 40;
}


// ############################################################ //
//																//
//			M E T O D O S   G E N E R A L E S					//
//																//
// ############################################################ //

//
// Abrir Cajón de Dinero
//
void
ImpresorFiscalP425::AbrirCajon () throw(Excepcion)
{
	// printf ("Comando AbrirCajon ejecutado en P425\n");

	// Enviamos el comando fiscal y evaluamos los status
	EnviarComandoFiscal(OpCode(CMD_OPEN_DRAWER));
}


//
// Avanzar papel en estación especificada
//
void
ImpresorFiscalP425::AvanzarPapel(TiposDePapel Papel, unsigned Lineas) throw (Excepcion)
{
	unsigned Comando;

	// printf ("Comando AvanzarPapel ejecutado en P425\n");
	
	switch (Papel)
	{
		case PAPEL_TICKET_Y_DIARIO:
			Comando = CMD_FEED_RECEIPT_JOURNAL;
			break;
		case PAPEL_DIARIO:
			Comando = CMD_FEED_JOURNAL;
			break;
		case PAPEL_TICKET:
		default:	
			Comando = CMD_FEED_RECEIPT;
			break;
	}

	// Verificamos que la cantidad de líneas no supere
	// el máximo valor aceptado por el CF
	if (Lineas > 99)
		Lineas = 99;

	// Enviamos el comando fiscal y evaluamos los status
	EnviarComandoFiscal(OpCode(ImpresorFiscal::OpCodes(Comando)) + FS + Numero(Lineas));
}


// Capacidad de impresión de documentos en estación Ticket
void
ImpresorFiscalP425::CapacidadDocumentoTicket (bool &SoportaFA, bool &SoportaNC)
{
	// printf ("Comando CapacidadDocumentoTicket ejecutado en P425\n");

	SoportaFA = true;
	SoportaNC = true;
}


// Capacidad de impresión en estación Slip / Ticket
void
ImpresorFiscalP425::CapacidadEstacion (bool &SoportaTicket, bool &SoportaSlip)
{
	// printf ("Comando CapacidadEstacion ejecutado en P425\n");

	SoportaTicket = true;
	SoportaSlip = true;
}


// Capacidad de manejo y soporte de Cajón de Dinero
void
ImpresorFiscalP425::CapacidadCajon (bool &SoportaCajon)
{
	// printf ("Comando CapacidadCajon ejecutado en P425\n");

	SoportaCajon = true;
}


// ############################################################ //
//																//
//			M E T O D O S   D E    C O N F I G U R A C I O N	//
//																//
// ############################################################ //

//
// Obtener Configuracion del Controlador
//
void
ImpresorFiscalP425::ObtenerConfiguracion (RTA_ObtenerConfiguracion *R) throw (Excepcion)
{
	// printf ("Comando ObtenerConfiguracion ejecutado en P425\n");

	// Ejecutamos el método de la superclase 'ImpresorFiscal16Bits'
	ImpresorFiscal16Bits::ObtenerConfiguracion (R);

	// Completamos los parámetros correspondientes a los campos
	// de respuesta particulares que se agregan en este modelo
	// más alla de los comunes a todos los modelos
	if (R != NULL)
	{
		R->AltoHoja						= static_cast<ImpresorFiscal::TiposDeAltoHoja>     (RespuestaString (13, "Obtener Configuracion del Controlador") [0]);
		R->AnchoHoja					= static_cast<ImpresorFiscal::TiposDeAnchoHoja>    (RespuestaString (14, "Obtener Configuracion del Controlador") [0]);
		R->EstacionImpresionReportesXZ	= static_cast<ImpresorFiscal::TiposDeEstacion>     (RespuestaString (15, "Obtener Configuracion del Controlador") [0]);
		R->ModoImpresion				= static_cast<ImpresorFiscal::TiposDeModoImpresion>(RespuestaString (16, "Obtener Configuracion del Controlador") [0]);
	}
}


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
ImpresorFiscalP425::AbrirDF (DocumentosFiscales Tipo) throw (Excepcion)
{
	// printf ("Comando AbrirDF ejecutado en P425\n");

	// Verificamos si el Tipo de DF es válido para este modelo
	if ( !(	Tipo == FACTURA_A		     ||
            Tipo == FACTURA_B		     ||
			Tipo == NOTA_DEBITO_A	     ||
			Tipo == NOTA_DEBITO_B	     ||
			Tipo == RECIBO_A		     ||
			Tipo == RECIBO_B             ||
            Tipo == TICKET_FACTURA_A     ||
            Tipo == TICKET_FACTURA_B     ||
			Tipo == TICKET_NOTA_DEBITO_A ||
			Tipo == TICKET_NOTA_DEBITO_B ||
            Tipo == TICKET_C ) )
		throw Excepcion(Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO, "Abrir DF");

	// Ejecutamos el método de la SuperClase 'ImpresorFiscal16Bits'
	ImpresorFiscal16Bits::AbrirDF (Tipo);
}


// ############################################################ //
//																//
//			M E T O D O S   D E    D N F						//
//																//
// ############################################################ //



// ############################################################ //
//																//
//			M E T O D O S   D E    D N F H						//
//																//
// ############################################################ //


