#include "P715_201.h"


std::string ImpresorFiscalP715_201::Version ("SMH/P-715F - Versión 02.01");


// ############################################################ //
//																//
//			M E T O D O S      L O C A L E S					//
//																//
// ############################################################ //

//
// Constructor
//
ImpresorFiscalP715_201::ImpresorFiscalP715_201 ()
{
	// Inicialización de variables de uso general

	// Inicialización de variables de tamaño de campos
	PriceDecimals					= 4;
}


//
// Obtener la Descripción del Modelo Seleccionado
//
std::string
ImpresorFiscalP715_201::DescripcionModelo () const
{
	return 	(Version);
}


// ############################################################ //
//																//
//			M E T O D O S   G E N E R A L E S					//
//																//
// ############################################################ //

//
// Cambiar Fecha de Inicio de Actividades
//
void
ImpresorFiscalP715_201::CambiarFechaInicioActividades (const FECHA &Fecha) throw(Excepcion)
{
	// printf ("Comando CambiarFechaInicioActividades ejecutado en P715_201\n");

	// Enviamos el comando fiscal y evaluamos los status
	EnviarComandoFiscal(OpCode(CMD_CHG_INI_ACT_DATE) + FS + Fecha.ToString());
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


