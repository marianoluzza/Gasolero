#ifndef IMPRESOR_FISCAL_P715_201_H
#define IMPRESOR_FISCAL_P715_201_H

#include "P715.h"

class ImpresorFiscalP715_201 : public ImpresorFiscalP715
{
public:

	// Constructor
	ImpresorFiscalP715_201();

	// String de Versi�n
	static std::string Version;

	// M�todos locales
	std::string DescripcionModelo () const;

	// M�todos generales
	void CambiarFechaInicioActividades (const FECHA &Fecha) throw(Excepcion);

	// M�todos de configuraci�n

	// M�todos de reportes

	// M�todos de documentos fiscales

	// M�todos de documentos no fiscales

	// M�todos de documentos no fiscales homologados

protected:

}; 

#endif


