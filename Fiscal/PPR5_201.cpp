#include "PPR5_201.h"


std::string ImpresorFiscalPPR5_201::Version ("SMH/P-PR5F - Versión 02.01");


// ############################################################ //
//																//
//			M E T O D O S      L O C A L E S					//
//																//
// ############################################################ //

//
// Constructor
//
ImpresorFiscalPPR5_201::ImpresorFiscalPPR5_201 ()
{
	// Inicialización de variables de uso general

	// Inicialización de variables de tamaño de campos
	PriceDecimals					= 4;
}


//
// Obtener la Descripción del Modelo Seleccionado
//
std::string
ImpresorFiscalPPR5_201::DescripcionModelo () const
{
	return 	(Version);
}


// ############################################################ //
//																//
//			M E T O D O S   G E N E R A L E S					//
//																//
// ############################################################ //



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


