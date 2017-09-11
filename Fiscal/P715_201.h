#ifndef IMPRESOR_FISCAL_P715_201_H
#define IMPRESOR_FISCAL_P715_201_H

#include "P715.h"

class ImpresorFiscalP715_201 : public ImpresorFiscalP715
{
public:

	// Constructor
	ImpresorFiscalP715_201();

	// String de Versión
	static std::string Version;

	// Métodos locales
	std::string DescripcionModelo () const;

	// Métodos generales
	void CambiarFechaInicioActividades (const FECHA &Fecha) throw(Excepcion);

	// Métodos de configuración

	// Métodos de reportes

	// Métodos de documentos fiscales

	// Métodos de documentos no fiscales

	// Métodos de documentos no fiscales homologados

protected:

}; 

#endif


