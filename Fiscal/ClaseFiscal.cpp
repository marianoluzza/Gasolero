// ClaseFiscal.cpp : Defines the entry point for the console application.
//

#include "P262_100.h"
#include "P272_100.h"

#include "P950_100.h"
#include "P951_100.h"

#include "PPR4_100.h"

#include "PPR5_100.h"
#include "PPR5_201.h"

#include "P614_100.h"
#include "P615_100.h"

#include "P715_100.h"
#include "P715_201.h"

#include "P320_100.h"

#include "PPL8_100.h"
#include "PPL8_201.h"

#include "PJ20_100.h"

#include "P321_100.h"

#include "P322_100.h"
#include "P322_201.h"

#include "PPL9_100.h"

#include "P330_100.h"
#include "P330_201.h"
#include "P330_202.h"

#include "P425_100.h"
#include "P425_201.h"

#include "P435_100.h"
#include "P435_101.h"


class Eventos : public ManejadorDeEventos
{
public:

	Eventos(ImpresorFiscal *p) : pFiscal(p) {}

	void Evento(unsigned Tipo, unsigned SubTipo)
	{
		printf ("Evento Tipo %2u SubTipo %2u <%s>\n", Tipo, SubTipo,
			pFiscal->DescripcionDeEvento((ImpresorFiscal::TiposDeEvento)Tipo, SubTipo).c_str());
	}

private:
	ImpresorFiscal *pFiscal;
};


int
main (int argc, char* argv[])
{
	try
	{
        // Instanciamos el Impresor Fiscal (en este caso 262_100)
        ImpresorFiscal *pIF = new ImpresorFiscalP262_100;

        // Instanciamos el Impresor Fiscal (en este caso 272_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalP272_100;

        // Instanciamos el Impresor Fiscal (en este caso 950_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalP950_100;

        // Instanciamos el Impresor Fiscal (en este caso 951_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalP951_100;

        // Instanciamos el Impresor Fiscal (en este caso PR4_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalPPR4_100;

        // Instanciamos el Impresor Fiscal (en este caso PR5_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalPPR5_100;

        // Instanciamos el Impresor Fiscal (en este caso PR5_201)
        // ImpresorFiscal *pIF = new ImpresorFiscalPPR5_201;

        // Instanciamos el Impresor Fiscal (en este caso 614_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalP614_100;

        // Instanciamos el Impresor Fiscal (en este caso 615_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalP615_100;

		// Instanciamos el Impresor Fiscal (en este caso 715_100)
		// ImpresorFiscal *pIF = new ImpresorFiscalP715_100;
		
		// Instanciamos el Impresor Fiscal (en este caso 715_201)
		// ImpresorFiscal *pIF = new ImpresorFiscalP715_201;
		
        // Instanciamos el Impresor Fiscal (en este caso 320_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalP320_100;

        // Instanciamos el Impresor Fiscal (en este caso PL8_100)
		// ImpresorFiscal *pIF = new ImpresorFiscalPPL8_100;

		// Instanciamos el Impresor Fiscal (en este caso PL8_201)
		// ImpresorFiscal *pIF = new ImpresorFiscalPPL8_201;

        // Instanciamos el Impresor Fiscal (en este caso PJ20_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalPJ20_100;

        // Instanciamos el Impresor Fiscal (en este caso 321_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalP321_100;

        // Instanciamos el Impresor Fiscal (en este caso 322_100)
		// ImpresorFiscal *pIF = new ImpresorFiscalP322_100;

		// Instanciamos el Impresor Fiscal (en este caso 322_201)
		// ImpresorFiscal *pIF = new ImpresorFiscalP322_201;

        // Instanciamos el Impresor Fiscal (en este caso PL9_100)
        // ImpresorFiscal *pIF = new ImpresorFiscalPPL9_100;

        // Instanciamos el Impresor Fiscal (en este caso 330_100)
		// ImpresorFiscal *pIF = new ImpresorFiscalP330_100;

		// Instanciamos el Impresor Fiscal (en este caso 330_201)
		// ImpresorFiscal *pIF = new ImpresorFiscalP330_201;

		// Instanciamos el Impresor Fiscal (en este caso 330_202)
		// ImpresorFiscal *pIF = new ImpresorFiscalP330_202;

        // Instanciamos el Impresor Fiscal (en este caso 425_100)
		// ImpresorFiscal *pIF = new ImpresorFiscalP425_100;

		// Instanciamos el Impresor Fiscal (en este caso 425_201)
		// ImpresorFiscal *pIF = new ImpresorFiscalP425_201;

        // Instanciamos el Impresor Fiscal (en este caso 435_100)
		// ImpresorFiscal *pIF = new ImpresorFiscalP435_100;

		// Instanciamos el Impresor Fiscal (en este caso 435_101)
		// ImpresorFiscal *pIF = new ImpresorFiscalP435_101;

		// Estableciendo tipo de Transporte
        pIF->EstablecerPuertoSerie (1, 9600);
		// pIF->EstablecerPuertoSerie (1, ImpresorFiscal::AUTODETECTAR_VELOCIDAD);
        // pIF->EstablecerSocketTCP ("192.0.0.100", 5557);
        // pIF->EstablecerSocketUDP ("192.0.2.11", 11000);

		// Estaleciendo manejador y capturador de eventos
		Eventos E (pIF);
		pIF->EstablecerManejadorDeEventos (&E);

		// Informamos la fecha y hora actual
		{
			ImpresorFiscal::FECHA F;
			ImpresorFiscal::HORA  H;
			printf ("Fecha Actual: %02u/%02u/%04u - Hora  Actual: %02u:%02u:%02u\n",
					 F.dia (), F.mes (), F.anio (),
					 H.hora (), H.minutos (), H.segundos ());
		}


// ######################################
//  Prueba de Lectura de Versión del CF
// ######################################

#if 0
		{
			// Obtenemos la versión del CF

			printf ("MODELO: %s\n", pIF->ObtenerVersionDelControlador ().c_str());
		}
#endif


// ############################################
//  Prueba de Lectura de Datos Inicialización
// ############################################

#if 0
		{
			ImpresorFiscal::RTA_ObtenerDatosDeInicializacion R;
			
			pIF->ObtenerDatosDeInicializacion (&R);
					
			printf ("DATOS DE INICIALIZACIÓN\n"); 
			printf ("Nombre:   %s\n", R.RazonSocial.c_str ());
			printf ("C.U.I.T.: %s\n", R.NroCUIT.c_str ());
			printf ("Nº Serie: %s\n", R.NroSerie.c_str ());
			printf ("FechaIni: %02u/%02u/%04u\n", R.FechaIncializacion.dia (), R.FechaIncializacion.mes (), R.FechaIncializacion.anio ());
			printf ("Nº POS:   %s\n", R.NroPOS.c_str ());
			printf ("IngBr:    %s\n", R.CodIngBrutos.c_str ());
			printf ("FeIniAct: %02u/%02u/%04u\n", R.FechaIniActividades.dia (), R.FechaIniActividades.mes (), R.FechaIniActividades.anio ());
			printf ("Resp.IVA: %c\n", R.RespIVA);
		}
#endif


// ######################################################
//  Prueba de Lectura de Datos de la Memoria de trabajo
// ######################################################

#if 0
		{
			ImpresorFiscal::RTA_ObtenerDatosMemoriaDeTrabajo R;
			
			pIF->ObtenerDatosMemoriaDeTrabajo (&R);
					
			printf ("Datos de Memoria de Trabajo\n");
			printf ("\tCantidadDFCancelados             = %u\n",	R.CantidadDFCancelados			);
			printf ("\tCantidadDNFEmitidos              = %u\n",	R.CantidadDNFEmitidos			);
			printf ("\tCantidadDFEmitidos               = %u\n",	R.CantidadDFEmitidos			);
			printf ("\tUltimoDocFiscalBC                = %lu\n",	R.UltimoDocFiscalBC				);
			printf ("\tUltimoDocFiscalA                 = %lu\n",	R.UltimoDocFiscalA				);
			printf ("\tMontoVentasDocFiscal             = %.2f\n",	R.MontoVentasDocFiscal			);
			printf ("\tMontoIVADocFiscal                = %.2f\n",	R.MontoIVADocFiscal				);
			printf ("\tMontoImpInternosDocFiscal        = %.2f\n",	R.MontoImpInternosDocFiscal		);
			printf ("\tMontoPercepcionesDocFiscal       = %.2f\n",	R.MontoPercepcionesDocFiscal	);
			printf ("\tMontoIVANoInscriptoDocFiscal     = %.2f\n",	R.MontoIVANoInscriptoDocFiscal	);
			printf ("\tUltimaNotaCreditoBC              = %lu\n",	R.UltimaNotaCreditoBC			);
			printf ("\tUltimaNotaCreditoA               = %lu\n",	R.UltimaNotaCreditoA			);
			printf ("\tMontoVentasNotaCredito           = %.2f\n",	R.MontoVentasNotaCredito		);
			printf ("\tMontoIVANotaCredito              = %.2f\n",	R.MontoIVANotaCredito			);
			printf ("\tMontoImpInternosNotaCredito      = %.2f\n",	R.MontoImpInternosNotaCredito	);
			printf ("\tMontoPercepcionesNotaCredito     = %.2f\n",	R.MontoPercepcionesNotaCredito	);
			printf ("\tMontoIVANoInscriptoNotaCredito   = %.2f\n",	R.MontoIVANoInscriptoNotaCredito);
			printf ("\tUltimoRemito                     = %lu\n",	R.UltimoRemito					);
			printf ("\tCantidadNCCanceladas             = %u\n",	R.CantidadNCCanceladas			);
		}
#endif


// ##############################
//  Prueba de Dar de Baja al CF
// ##############################

#if 0
		{
			pIF->BajaFiscal ("HHZ12312312");
			pIF->BajaFiscal ();
		}
#endif


// ############################################################
//  Prueba de Cambio de Responsabilidad frentea al IVA del CF
// ############################################################

#if 0
		{
			pIF->CambiarResponsabilidadIVA (ImpresorFiscal::CLASE_RESPONSABLE_INSCRIPTO);
		}
#endif


// #######################################################
//  Prueba de Cambio de Código de Ingresos Brutos del CF
// #######################################################

#if 0
		{
			pIF->CambiarCodigoIngresosBrutos ("1234");
		}
#endif


// ############################################################
//  Prueba de Cambio de Fecha de Inicio de Actividades del CF
// ############################################################

#if 0
		{
			ImpresorFiscal::FECHA F = "050101";

			pIF->CambiarFechaInicioActividades (F);
		}
#endif


// ##########################################
//  Prueba de Reporte de Status de Impresor
// ##########################################

#if 0
		{
			printf ("REPORTE DE BITS DE PRINTER_STATUS\n");
			for (unsigned i = 0; i < 15; i++)
			{
				try
				{
					if (pIF->EstadoImpresor ((ImpresorFiscal::EstadosImpresor)i, false))
						printf ("\tBit %2d (%-40.40s)\n", i, pIF->DescripcionEstadoImpresor ((ImpresorFiscal::EstadosImpresor)i).c_str ());
				}
				catch(Excepcion &e)
				{
					if (e.Codigo != Excepcion::IMPRESOR_FISCAL_ERROR_BIT_NO_VALIDO)
						throw;
				}
			}

		}
#endif


// #####################################
//  Prueba de Reporte de Status Fiscal
// #####################################

#if 0
		{
			printf ("REPORTE DE BITS DE FISCAL_STATUS\n");
			for (unsigned i = 0; i < 15; i++)
			{
				try
				{
					if (pIF->EstadoFiscal ((ImpresorFiscal::EstadosFiscal)i, false))
						printf ("\tBit %2d (%-40.40s)\n", i, pIF->DescripcionEstadoFiscal ((ImpresorFiscal::EstadosFiscal)i).c_str ());
				}
				catch(Excepcion &e)
				{
					if (e.Codigo != Excepcion::IMPRESOR_FISCAL_ERROR_BIT_NO_VALIDO)
						throw;
				}
			}

		}
#endif


// ###################################################
//  Prueba para Obtener los Nº de Ultimos Documentos
// ###################################################

#if 0
		{
			unsigned UltimoDFBC   = pIF->UltimoDocumentoFiscalBC ();
			unsigned UltimoDFA    = pIF->UltimoDocumentoFiscalA ();
			unsigned UltimaNCBC   = pIF->UltimoNotaCreditoBC ();
			unsigned UltimaNCA    = pIF->UltimoNotaCreditoA ();
			
			unsigned UltimoRemito = 0;
			try
			{
				UltimoRemito = pIF->UltimoRemito ();
			}

			catch(Excepcion &e)
		{
				if (e.Codigo == Excepcion::IMPRESOR_FISCAL_ERROR_NO_IMPLEMENTADO)
					printf ("Método UltimoRemito No Soportado por el modelo actual\n");
			}

			printf ("REPORTE DE Nº DE ULTIMOS DOCUMENTOS\n");
			printf ("Ultimo DF BC:  %8lu\n", UltimoDFBC);
			printf ("Ultimo DF A:   %8lu\n", UltimoDFA);
			printf ("Ultima NC BC:  %8lu\n", UltimaNCBC);
			printf ("Ultima NC A:   %8lu\n", UltimaNCA);
			printf ("Ultimo Remito: %8lu\n", UltimoRemito);
		}
#endif


// ############################################################
//  Prueba de Lectura y programación de la Fecha y hora del CF
// ############################################################

#if 0
		{
			ImpresorFiscal::FECHA F;
			ImpresorFiscal::HORA H;

			// Obtenemos la Fecha y Hora antes de Re-programar
			pIF->ObtenerFechaHoraFiscal (F, H);
			printf ("Fecha y Hora Anterior del CF\n");
			printf ("Fecha: %02u/%02u/%04u - Hora: %02u:%02u:%02u\n",
					 F.dia (), F.mes (), F.anio (),
					 H.hora (), H.minutos (), H.segundos ());

			// Re-programamos la Fecha y Hora
			ImpresorFiscal::FECHA FNueva;
			ImpresorFiscal::HORA HNueva;
			pIF->EstablecerFechaHoraFiscal (FNueva, HNueva);

			// Obtenemos la Fecha y Hora después de Re-programar
			pIF->ObtenerFechaHoraFiscal (F, H);
			printf ("Fecha y Hora Nueva del CF (Programada)\n");
			printf ("Fecha: %02u/%02u/%04u - Hora: %02u:%02u:%02u\n",
					 F.dia (), F.mes (), F.anio (),
					 H.hora (), H.minutos (), H.segundos ());
		}
#endif


// #####################################
//  Prueba de Carga de Logo de Cliente
// #####################################

#if 0
		{
			pIF->CargarLogo ();
		}
#endif


// ###########################################################
//  Prueba para Configurar el CF por Parámetros Individuales
// ###########################################################

#if 0
		{
			pIF->ConfigurarControlador (ImpresorFiscal::IMPRESION_CAMBIO, "P");
			pIF->ConfigurarControlador (ImpresorFiscal::IMPRESION_LEYENDAS, "P");
			pIF->ConfigurarControlador (ImpresorFiscal::CORTE_PAPEL, "P");
			pIF->ConfigurarControlador (ImpresorFiscal::IMPRESION_MARCO, "P");
			pIF->ConfigurarControlador (ImpresorFiscal::REIMPRESION_CANCELADOS, "P");
			pIF->ConfigurarControlador (ImpresorFiscal::COPIAS_DOCUMENTOS, "4");
			pIF->ConfigurarControlador (ImpresorFiscal::PAGO_SALDO, "Cuenta corriente");
			pIF->ConfigurarControlador (ImpresorFiscal::SONIDO, "P");
			pIF->ConfigurarControlador (ImpresorFiscal::ALTO_HOJA, "M");
			pIF->ConfigurarControlador (ImpresorFiscal::ANCHO_HOJA, "M");
			pIF->ConfigurarControlador (ImpresorFiscal::ESTACION_REPORTES_XZ, "T");
			pIF->ConfigurarControlador (ImpresorFiscal::MODO_IMPRESION, "A");
		}
#endif


// ########################################################
//  Prueba para Configurar el CF por Bloque de Parámetros
// ########################################################

#if 0
		{
			pIF->ConfigurarControladorPorBloque (2000, 10000, 10,
					ImpresorFiscal::CUADRUPLICADO, false, false,
					ImpresorFiscal::NO_CORTE);
		}
#endif


// ###############################################################
//  Prueba para Configurar el CF Completa (todos los Parámetros)
// ###############################################################

#if 0
		{
 			bool Imprimir = true;
			bool Defaults = false;
			double LimiteConsumidorFinal = 1000;
			double LimiteTicketFactura = 5000;
			double PorcentajeIVANoInscripto = 50;
			ImpresorFiscal::NumerosDeCopias NumeroDeCopiasMaximo = ImpresorFiscal::DUPLICADO;
			bool ImprimeCambio = true;
			bool ImprimeLeyendasOpcionales = true;
			ImpresorFiscal::TiposDeCorteDePapel TipoDeCorte = ImpresorFiscal::CORTE_PARCIAL;
			bool ImprimeMarco = false;
			bool ReImprimeDocumentos = false;
			std::string DescripcionDelMedioDePago = "CuentaCorriente";
			bool Sonido = false;
			ImpresorFiscal::TiposDeAltoHoja AltoHoja = ImpresorFiscal::ALTO_REDUCIDO;
			ImpresorFiscal::TiposDeAnchoHoja AnchoHoja = ImpresorFiscal::ANCHO_REDUCIDO;
			ImpresorFiscal::TiposDeEstacion EstacionImpresionReportesXZ = ImpresorFiscal::ESTACION_TICKET;
			ImpresorFiscal::TiposDeModoImpresion ModoImpresion = ImpresorFiscal::USO_ESTACION_TICKET;

			pIF->ConfigurarControladorCompleto (Imprimir, Defaults,
					true ? &LimiteConsumidorFinal		: NULL,
					true ? &LimiteTicketFactura			: NULL,
					true ? &PorcentajeIVANoInscripto	: NULL,
					true ? &NumeroDeCopiasMaximo		: NULL,
					true ? &ImprimeCambio				: NULL,
					true ? &ImprimeLeyendasOpcionales	: NULL,
					true ? &TipoDeCorte					: NULL,
					true ? &ImprimeMarco				: NULL,
					true ? &ReImprimeDocumentos			: NULL,
					true ? &DescripcionDelMedioDePago	: NULL,
					true ? &Sonido						: NULL,
					true ? &AltoHoja					: NULL,
					true ? &AnchoHoja					: NULL,
					true ? &EstacionImpresionReportesXZ	: NULL,
					true ? &ModoImpresion				: NULL);			
		}
#endif


// #############################################################
//  Prueba para Obtener los Parámetros de Configuración del CF
// #############################################################

#if 0
		{
			ImpresorFiscal::RTA_ObtenerConfiguracion R;
			
			printf ("EJECUTANDO COMANDO OBTENER CONFIGURACION\n");
			pIF->ObtenerConfiguracion (&R);

			printf ("REPORTE DE CONFIGURACION DEL CF\n");
			printf ("LimiteConsumidorFinal:         %.2f\n", R.LimiteConsumidorFinal);
			printf ("LimiteTicketFactura:           %.2f\n", R.LimiteTicketFactura);
			printf ("PorcentajeIVANoInscripto:      %.2f\n", R.PorcentajeIVANoInscripto);
			printf ("NumeroDeCopiasMaximo:          %c\n", R.NumeroDeCopiasMaximo);
			printf ("ImprimeCambio $0.00:           %s\n", R.ImprimeCambio ? "SI" : "NO");
			printf ("ImprimeLeyendasOpcionales:     %s\n", R.ImprimeLeyendasOpcionales ? "SI" : "NO");
			printf ("TipoDeCorte:                   %c\n", R.TipoDeCorte);
			printf ("ImprimeMarco:                  %s\n", R.ImprimeMarco ? "SI" : "NO");
			printf ("ReImprimeDocumentos:           %s\n", R.ReImprimeDocumentos ? "SI" : "NO");
			printf ("DescripcionDelMedioDePago:     %s\n", R.DescripcionDelMedioDePago.c_str());
			printf ("Sonido:                        %s\n", R.Sonido ? "SI" : "NO");
			printf ("AltoHoja:                      %c\n", R.AltoHoja);
			printf ("AnchoHoja:                     %c\n", R.AnchoHoja);
			printf ("EstacionImpresionReportesXZ:   %c\n", R.EstacionImpresionReportesXZ);
			printf ("ModoImpresion:                 %c\n", R.ModoImpresion);
		}
#endif


// #################################
//  Prueba de Emisión un Reporte Z
// #################################

#if 0
		{
			ImpresorFiscal::RTA_ReporteZX R;
			
			pIF->ReporteZ (&R);
					
			printf ("Reporte Z Nº %u\n",							R.NumeroReporte					);
			printf ("\tCantidadDFCancelados             = %u\n",	R.CantidadDFCancelados			);
			printf ("\tCantidadDNFHEmitidos             = %u\n",	R.CantidadDNFHEmitidos			);
			printf ("\tCantidadDNFEmitidos              = %u\n",	R.CantidadDNFEmitidos			);
			printf ("\tCantidadDFEmitidos               = %u\n",	R.CantidadDFEmitidos			);
			printf ("\tUltimoDocFiscalBC                = %lu\n",	R.UltimoDocFiscalBC				);
			printf ("\tUltimoDocFiscalA                 = %lu\n",	R.UltimoDocFiscalA				);
			printf ("\tMontoVentasDocFiscal             = %.2f\n",	R.MontoVentasDocFiscal			);
			printf ("\tMontoIVADocFiscal                = %.2f\n",	R.MontoIVADocFiscal				);
			printf ("\tMontoImpInternosDocFiscal        = %.2f\n",	R.MontoImpInternosDocFiscal		);
			printf ("\tMontoPercepcionesDocFiscal       = %.2f\n",	R.MontoPercepcionesDocFiscal	);
			printf ("\tMontoIVANoInscriptoDocFiscal     = %.2f\n",	R.MontoIVANoInscriptoDocFiscal	);
			printf ("\tUltimaNotaCreditoBC              = %lu\n",	R.UltimaNotaCreditoBC			);
			printf ("\tUltimaNotaCreditoA               = %lu\n",	R.UltimaNotaCreditoA			);
			printf ("\tMontoVentasNotaCredito           = %.2f\n",	R.MontoVentasNotaCredito		);
			printf ("\tMontoIVANotaCredito              = %.2f\n",	R.MontoIVANotaCredito			);
			printf ("\tMontoImpInternosNotaCredito      = %.2f\n",	R.MontoImpInternosNotaCredito	);
			printf ("\tMontoPercepcionesNotaCredito     = %.2f\n",	R.MontoPercepcionesNotaCredito	);
			printf ("\tMontoIVANoInscriptoNotaCredito   = %.2f\n",	R.MontoIVANoInscriptoNotaCredito);
			printf ("\tUltimoRemito                     = %lu\n",	R.UltimoRemito					);
		}
#endif


// #################################
//  Prueba de Emisión un Reporte X
// #################################

#if 0
		{
			ImpresorFiscal::RTA_ReporteZX R;
			
			pIF->ReporteX (&R);
					
			printf ("Reporte X Nº %u\n",							R.NumeroReporteX				);
			printf ("\tCantidadDFCancelados             = %u\n",	R.CantidadDFCancelados			);
			printf ("\tCantidadDNFHEmitidos             = %u\n",	R.CantidadDNFHEmitidos			);
			printf ("\tCantidadDNFEmitidos              = %u\n",	R.CantidadDNFEmitidos			);
			printf ("\tCantidadDFEmitidos               = %u\n",	R.CantidadDFEmitidos			);
			printf ("\tUltimoDocFiscalBC                = %lu\n",	R.UltimoDocFiscalBC				);
			printf ("\tUltimoDocFiscalA                 = %lu\n",	R.UltimoDocFiscalA				);
			printf ("\tMontoVentasDocFiscal             = %.2f\n",	R.MontoVentasDocFiscal			);
			printf ("\tMontoIVADocFiscal                = %.2f\n",	R.MontoIVADocFiscal				);
			printf ("\tMontoImpInternosDocFiscal        = %.2f\n",	R.MontoImpInternosDocFiscal		);
			printf ("\tMontoPercepcionesDocFiscal       = %.2f\n",	R.MontoPercepcionesDocFiscal	);
			printf ("\tMontoIVANoInscriptoDocFiscal     = %.2f\n",	R.MontoIVANoInscriptoDocFiscal	);
			printf ("\tUltimaNotaCreditoBC              = %lu\n",	R.UltimaNotaCreditoBC			);
			printf ("\tUltimaNotaCreditoA               = %lu\n",	R.UltimaNotaCreditoA			);
			printf ("\tMontoVentasNotaCredito           = %.2f\n",	R.MontoVentasNotaCredito		);
			printf ("\tMontoIVANotaCredito              = %.2f\n",	R.MontoIVANotaCredito			);
			printf ("\tMontoImpInternosNotaCredito      = %.2f\n",	R.MontoImpInternosNotaCredito	);
			printf ("\tMontoPercepcionesNotaCredito     = %.2f\n",	R.MontoPercepcionesNotaCredito	);
			printf ("\tMontoIVANoInscriptoNotaCredito   = %.2f\n",	R.MontoIVANoInscriptoNotaCredito);
			printf ("\tUltimoRemito                     = %lu\n",	R.UltimoRemito					);
		}
#endif


// ##################################################################
//  Prueba de Emisión de Reportes de Auditoría por Fechas y Números
// ##################################################################

#if 0
		{
			ImpresorFiscal::FECHA FIni ("990101"), FFin ("501231");
			// FIni.anio = 1999;	FIni.mes  = 1;	FIni.dia  = 1;
			// FFin.anio = 2050;	FFin.mes  = 12;	FFin.dia  = 31;
			// Reporte Global por Rango de Fechas
			pIF->ReporteZPorFechas (FIni, FFin, true);
			// Reporte Individual por Rango de Fechas
			pIF->ReporteZPorFechas (FIni, FFin, false);

			// Reporte Global por Rango de Zetas
			pIF->ReporteZPorNumeros (1, 1800, true);
			// Reporte Individual por Rango de Zetas
			pIF->ReporteZPorNumeros (1, 1800, false);
		}

#endif


// ####################################################################
//  Prueba de Solicitud de Reportes Z Individuales por Fecha y Numero
// ####################################################################

#if 0
		{
			ImpresorFiscal::FECHA FechaZ;
			unsigned NumeroZ;

			ImpresorFiscal::RTA_ReporteZIndividual R;

			// Reporte Z Individual por Fecha
			FechaZ = ImpresorFiscal::FECHA ("041202");	// ("050928");	// ("050727");
			pIF->ReporteZIndividualPorFecha (FechaZ, &R);

			printf ("Reporte Z Individual por Fecha\n");
			printf ("Fecha: %02u/%02u/%04u - Zeta Nº: %04u\n",
					 R.FechaZeta.dia (), R.FechaZeta.mes (), R.FechaZeta.anio (), R.NumeroZeta		);
			printf ("\tUltimoDocFiscalBC                = %lu\n",	R.UltimoDocFiscalBC				);
			printf ("\tUltimoDocFiscalA                 = %lu\n",	R.UltimoDocFiscalA				);
			printf ("\tMontoVentasDocFiscal             = %.2f\n",  R.MontoVentasDocFiscal			);
			printf ("\tMontoIVADocFiscal                = %.2f\n",  R.MontoIVADocFiscal				);
			printf ("\tMontoImpInternosDocFiscal        = %.2f\n",  R.MontoImpInternosDocFiscal		);
			printf ("\tMontoPercepcionesDocFiscal       = %.2f\n",  R.MontoPercepcionesDocFiscal	);
			printf ("\tMontoIVANoInscriptoDocFiscal     = %.2f\n",  R.MontoIVANoInscriptoDocFiscal  );
			printf ("\tUltimaNotaCreditoBC              = %lu\n",	R.UltimaNotaCreditoBC			);
			printf ("\tUltimaNotaCreditoA               = %lu\n",	R.UltimaNotaCreditoA			);
			printf ("\tMontoVentasNotaCredito           = %.2f\n",  R.MontoVentasNotaCredito		);
			printf ("\tMontoIVANotaCredito              = %.2f\n",  R.MontoIVANotaCredito			);
			printf ("\tMontoImpInternosNotaCredito      = %.2f\n",  R.MontoImpInternosNotaCredito	);
			printf ("\tMontoPercepcionesNotaCredito     = %.2f\n",  R.MontoPercepcionesNotaCredito  );
			printf ("\tMontoIVANoInscriptoNotaCredito   = %.2f\n",  R.MontoIVANoInscriptoNotaCredito);
			printf ("\tUltimoRemito                     = %lu\n",	R.UltimoRemito					);

			// Reporte Z Individual por Número
			NumeroZ = 1;
			pIF->ReporteZIndividualPorNumero (NumeroZ, &R);

			printf ("Reporte Z Individual por Número\n");
			printf ("Zeta Nº: %04u - Fecha: %02u/%02u/%04u\n",
					 R.NumeroZeta, R.FechaZeta.dia (), R.FechaZeta.mes (), R.FechaZeta.anio ()		);
			printf ("\tUltimoDocFiscalBC                = %lu\n",	R.UltimoDocFiscalBC				);
			printf ("\tUltimoDocFiscalA                 = %lu\n",	R.UltimoDocFiscalA				);
			printf ("\tMontoVentasDocFiscal             = %.2f\n",  R.MontoVentasDocFiscal			);
			printf ("\tMontoIVADocFiscal                = %.2f\n",  R.MontoIVADocFiscal				);
			printf ("\tMontoImpInternosDocFiscal        = %.2f\n",  R.MontoImpInternosDocFiscal		);
			printf ("\tMontoPercepcionesDocFiscal       = %.2f\n",  R.MontoPercepcionesDocFiscal	);
			printf ("\tMontoIVANoInscriptoDocFiscal     = %.2f\n",  R.MontoIVANoInscriptoDocFiscal  );
			printf ("\tUltimaNotaCreditoBC              = %lu\n",	R.UltimaNotaCreditoBC			);
			printf ("\tUltimaNotaCreditoA               = %lu\n",	R.UltimaNotaCreditoA			);
			printf ("\tMontoVentasNotaCredito           = %.2f\n",  R.MontoVentasNotaCredito		);
			printf ("\tMontoIVANotaCredito              = %.2f\n",  R.MontoIVANotaCredito			);
			printf ("\tMontoImpInternosNotaCredito      = %.2f\n",  R.MontoImpInternosNotaCredito	);
			printf ("\tMontoPercepcionesNotaCredito     = %.2f\n",  R.MontoPercepcionesNotaCredito  );
			printf ("\tMontoIVANoInscriptoNotaCredito   = %.2f\n",  R.MontoIVANoInscriptoNotaCredito);
			printf ("\tUltimoRemito                     = %lu\n",	R.UltimoRemito					);
		}

		// Solicitamos Reportes Z Individuales por Fecha y Numero
		{
			ImpresorFiscal::FECHA FechaZ;
			unsigned NumeroZ;

			ImpresorFiscal::RTA_ReporteZIndividual R;

			// Reporte Z Individual por Fecha
			FechaZ = ImpresorFiscal::FECHA ("041210");	// ("050928");	// ("050727");
			pIF->ReporteZIndividualPorFecha (FechaZ, &R);

			printf ("Reporte Z Individual por Fecha\n");
			printf ("Fecha: %02u/%02u/%04u - Zeta Nº: %04u\n",
					 R.FechaZeta.dia (), R.FechaZeta.mes (), R.FechaZeta.anio (), R.NumeroZeta		);
			printf ("\tUltimoDocFiscalBC                = %lu\n",	R.UltimoDocFiscalBC				);
			printf ("\tUltimoDocFiscalA                 = %lu\n",	R.UltimoDocFiscalA				);
			printf ("\tMontoVentasDocFiscal             = %lu\n",	R.MontoVentasDocFiscal			);
			printf ("\tMontoIVADocFiscal                = %lu\n",	R.MontoIVADocFiscal				);
			printf ("\tMontoImpInternosDocFiscal        = %lu\n",	R.MontoImpInternosDocFiscal		);
			printf ("\tMontoPercepcionesDocFiscal       = %lu\n",	R.MontoPercepcionesDocFiscal	);
			printf ("\tMontoIVANoInscriptoDocFiscal     = %lu\n",	R.MontoIVANoInscriptoDocFiscal  );
			printf ("\tUltimaNotaCreditoBC              = %lu\n",	R.UltimaNotaCreditoBC			);
			printf ("\tUltimaNotaCreditoA               = %lu\n",	R.UltimaNotaCreditoA			);
			printf ("\tMontoVentasNotaCredito           = %lu\n",	R.MontoVentasNotaCredito		);
			printf ("\tMontoIVANotaCredito              = %lu\n",	R.MontoIVANotaCredito			);
			printf ("\tMontoImpInternosNotaCredito      = %lu\n",	R.MontoImpInternosNotaCredito	);
			printf ("\tMontoPercepcionesNotaCredito     = %lu\n",	R.MontoPercepcionesNotaCredito  );
			printf ("\tMontoIVANoInscriptoNotaCredito   = %lu\n",	R.MontoIVANoInscriptoNotaCredito);
			printf ("\tUltimoRemito                     = %lu\n",	R.UltimoRemito					);

			// Reporte Z Individual por Número
			NumeroZ = 10;
			pIF->ReporteZIndividualPorNumero (NumeroZ, &R);

			printf ("Reporte Z Individual por Número\n");
			printf ("Zeta Nº: %04u - Fecha: %02u/%02u/%04u\n",
					 R.NumeroZeta, R.FechaZeta.dia (), R.FechaZeta.mes (), R.FechaZeta.anio ()		);
			printf ("\tUltimoDocFiscalBC                = %lu\n",	R.UltimoDocFiscalBC				);
			printf ("\tUltimoDocFiscalA                 = %lu\n",	R.UltimoDocFiscalA				);
			printf ("\tMontoVentasDocFiscal             = %lu\n",	R.MontoVentasDocFiscal			);
			printf ("\tMontoIVADocFiscal                = %lu\n",	R.MontoIVADocFiscal				);
			printf ("\tMontoImpInternosDocFiscal        = %lu\n",	R.MontoImpInternosDocFiscal		);
			printf ("\tMontoPercepcionesDocFiscal       = %lu\n",	R.MontoPercepcionesDocFiscal	);
			printf ("\tMontoIVANoInscriptoDocFiscal     = %lu\n",	R.MontoIVANoInscriptoDocFiscal  );
			printf ("\tUltimaNotaCreditoBC              = %lu\n",	R.UltimaNotaCreditoBC			);
			printf ("\tUltimaNotaCreditoA               = %lu\n",	R.UltimaNotaCreditoA			);
			printf ("\tMontoVentasNotaCredito           = %lu\n",	R.MontoVentasNotaCredito		);
			printf ("\tMontoIVANotaCredito              = %lu\n",	R.MontoIVANotaCredito			);
			printf ("\tMontoImpInternosNotaCredito      = %lu\n",	R.MontoImpInternosNotaCredito	);
			printf ("\tMontoPercepcionesNotaCredito     = %lu\n",	R.MontoPercepcionesNotaCredito  );
			printf ("\tMontoIVANoInscriptoNotaCredito   = %lu\n",	R.MontoIVANoInscriptoNotaCredito);
			printf ("\tUltimoRemito                     = %lu\n",	R.UltimoRemito					);
		}
#endif


// ###################################################
//  Prueba de Lectura de Zeta actual y las restantes
// ###################################################

#if 0
		{
			ImpresorFiscal::RTA_CapacidadRestante R;
			
			pIF->CapacidadRestante (&R);
			printf ("Total Cierres Z:     %4u\n", R.TotalCierresZeta);
			printf ("Cierre Z actual:     %4u\n", R.ZetaActual);
			printf ("Cierres Z restantes: %4u\n", R.TotalCierresZeta - R.ZetaActual);
		}
#endif


// ###############################################
//	Prueba de Comandos SendFirstIVA y SendNextIVA
// ###############################################

#if 0
		{
			ImpresorFiscal::TablaIVAs Tabla;

			printf ("EJECUTANDO COMANDOS DE RECUPERACION DE TABLA IVAS\n");
			pIF->ObtenerTablaIVAs (Tabla);

			printf ("----------------------------------------------\n");

			printf ("INFORMACION DE DF\n");
			for (std::list<ImpresorFiscal::IVA>::const_iterator idf =
					Tabla.DocFiscales.IVAs.begin ();
						idf != Tabla.DocFiscales.IVAs.end (); idf++)
			{
				printf ("\tAlicuota: %.2f\n", idf->Alicuota);
				printf ("\t\tMontoIVA:            %.2f\n", idf->MontoIVA);
				printf ("\t\tMontoImpInternos:    %.2f\n", idf->MontoImpInternos);
				printf ("\t\tMontoIVANoInscripto: %.2f\n", idf->MontoIVANoInscripto);
				printf ("\t\tMontoNetoSinIVA:     %.2f\n", idf->MontoNetoSinIVA);
			}
			for (std::list<ImpresorFiscal::PERCEPCION>::const_iterator pdf =
					Tabla.DocFiscales.PercepcionesIVA.begin ();
						pdf != Tabla.DocFiscales.PercepcionesIVA.end (); pdf++)
			{
				printf ("\tPercepcion a Alicuota: %.2f\n", pdf->Alicuota);
				printf ("\t\tMonto:               %.2f\n", pdf->Monto);
			}
			for (std::list<double>::const_iterator pgdf =
					Tabla.DocFiscales.PercepcionesGenerales.begin ();
						pgdf != Tabla.DocFiscales.PercepcionesGenerales.end (); pgdf++)
			{
				printf ("\tMonto Percepcion General: %.2f\n", *pgdf);
			}
			
			if (Tabla.DocFiscales.MontoTotal != 0)
				printf ("\tMonto Total: %.2f\n", Tabla.DocFiscales.MontoTotal);

			printf ("----------------------------------------------\n");

			printf ("INFORMACION DE NC\n");
			for (std::list<ImpresorFiscal::IVA>::const_iterator inc =
					Tabla.NotasCredito.IVAs.begin ();
						inc != Tabla.NotasCredito.IVAs.end (); inc++)
			{
				printf ("\tAlicuota: %.2f\n", inc->Alicuota);
				printf ("\t\tMontoIVA:            %.2f\n", inc->MontoIVA);
				printf ("\t\tMontoImpInternos:    %.2f\n", inc->MontoImpInternos);
				printf ("\t\tMontoIVANoInscripto: %.2f\n", inc->MontoIVANoInscripto);
				printf ("\t\tMontoNetoSinIVA:     %.2f\n", inc->MontoNetoSinIVA);
			}
			for (std::list<ImpresorFiscal::PERCEPCION>::const_iterator pnc =
					Tabla.NotasCredito.PercepcionesIVA.begin ();
						pnc != Tabla.NotasCredito.PercepcionesIVA.end (); pnc++)
			{
				printf ("\tPercepcion a Alicuota: %.2f\n", pnc->Alicuota);
				printf ("\t\tMonto:               %.2f\n", pnc->Monto);
			}
			for (std::list<double>::const_iterator pgnc =
					Tabla.NotasCredito.PercepcionesGenerales.begin ();
						pgnc != Tabla.NotasCredito.PercepcionesGenerales.end (); pgnc++)
			{
				printf ("\tMonto Percepcion General: %.2f\n", *pgnc);
			}
			
			if (Tabla.NotasCredito.MontoTotal != 0)
				printf ("\tMonto Total: %.2f\n", Tabla.NotasCredito.MontoTotal);

			printf ("----------------------------------------------\n");
		}
#endif


// ######################################################
//  Prueba de configuración de Propiedades de una Venta
// ######################################################

#if 0
		{
			pIF->EstablecerModalidadDescripcionesLargas (false);
			printf ("Modalidad Descripciones Largas: %s\n", 
				pIF->ObtenerModalidadDescripcionesLargas ()	? "SI" : "NO");

			pIF->EstablecerModalidadPrecioBase (false);
			printf ("Modalidad Precio:               %s\n", 
				pIF->ObtenerModalidadPrecioBase ()	? "PRECIO BASE" 
													: "PRECIO TOTAL");
			
			pIF->EstablecerModalidadDeImpuestosInternos (ImpresorFiscal::II_FIJO_MONTO);
			char * p;
			switch (pIF->ObtenerModalidadDeImpuestosInternos ())
			{
			case ' ':
				p = "II_VARIABLE_KIVA";
				break;
			case '%':
				p = "II_VARIABLE_PORCENTUAL";
				break;
			case '+':
				p = "II_FIJO_KIVA";
				break;
			case '$':
				p = "II_FIJO_MONTO";
				break;
			default:
				p = "II_INVALIDO";
				break;
			}
			printf ("Modalidad Impuestos Internos:   %s\n", p);

			pIF->ObtenerModalidadDescripcionesLargas ();
		}
#endif


// ####################################################################
//  Prueba de métodos 'EnviarComandoCrudo' y 'ObtenerUltimarespuesta'
// ####################################################################

#if 0
		{
			printf ("ENVIANDO COMANDO CRUDO ... ObtenerDatosDeInicializacion\n");
			pIF->EnviarComandoCrudo ("s");
			printf ("Ult.Rta.: %s\n", pIF->ObtenerUltimaRespuesta (0).c_str ());
			printf ("C.U.I.T.: %s\n", pIF->ObtenerUltimaRespuesta (3).c_str ());
			printf ("Nombre:   %s\n", pIF->ObtenerUltimaRespuesta (4).c_str ());
			printf ("Nº Serie: %s\n", pIF->ObtenerUltimaRespuesta (5).c_str ());
			printf ("FechaIni: %s\n", pIF->ObtenerUltimaRespuesta (6).c_str ());
			printf ("Nº POS:   %s\n", pIF->ObtenerUltimaRespuesta (7).c_str ());
			printf ("IngBr:    %s\n", pIF->ObtenerUltimaRespuesta (8).c_str ());
			printf ("FeIniAct: %s\n", pIF->ObtenerUltimaRespuesta (9).c_str ());
			printf ("Resp.IVA: %s\n", pIF->ObtenerUltimaRespuesta (10).c_str ());
		}
#endif


// ####################
//	Pruebas Generales
// ####################

#if 0
		{
			// printf ("Estado Cajón: %s\n",
			// 	pIF->CajonAbierto () ? "ABIERTO" : "CERRADO");
			// pIF->AbrirCajon ();
			// printf ("Estado Cajón: %s\n",
			// 	pIF->CajonAbierto () ? "ABIERTO" : "CERRADO");

			// pIF->AvanzarPapel (ImpresorFiscal::PAPEL_TICKET, 2);
			// pIF->AvanzarPapel (ImpresorFiscal::PAPEL_DIARIO, 2);
			// pIF->AvanzarPapel (ImpresorFiscal::PAPEL_TICKET_Y_DIARIO, 2);

			// printf ("Línea de Fantasía 1 <%s>\n", pIF->ObtenerNombreDeFantasia (1).c_str ());
			// pIF->EspecificarNombreDeFantasia (1, "Línea de Fantasía 1 - Línea de Fantasía 1 - Línea de Fantasía 1 - Línea de Fantasía 1");
			// printf ("Línea de Fantasía 1 <%s>\n", pIF->ObtenerNombreDeFantasia (1).c_str ());

			// printf ("Línea de Header 1 <%s>\n", pIF->ObtenerEncabezadoCola (1).c_str ());
			// pIF->EspecificarEncabezadoCola (1, "Línea de Header 1 - Línea de Header 1 - Línea de Header 1 - Línea de Header 1");
			// printf ("Línea de Header 1 <%s>\n", pIF->ObtenerEncabezadoCola (1).c_str ());

			// printf ("Línea de Trailer 1 <%s>\n", pIF->ObtenerEncabezadoCola (11).c_str ());
			// pIF->EspecificarEncabezadoCola (11, "Línea de Trailer 1 - Línea de Trailer 1 - Línea de Trailer 1 - Línea de Trailer 1");
			// printf ("Línea de Trailer 1 <%s>\n", pIF->ObtenerEncabezadoCola (11).c_str ());

			// pIF->EspecificarInformacionRemitoComprobanteOriginal (1, "Línea 1 de Info Remito / Comprobante Original 1");
			// printf ("Línea de Información Remito 1 <%s>\n", pIF->ObtenerInformacionRemitoComprobanteOriginal (1).c_str ());
			
			// printf ("Modo Display default: %c\n", pIF->ObtenerModoDeDisplay ());
			// pIF->EstablecerModoDeDisplay (ImpresorFiscal::DISPLAY_NO);
			// printf ("Modo Display nuevo:   %c\n", pIF->ObtenerModoDeDisplay ());
			
			// pIF->MostrarMensajeEnDisplay ("Hola Mundo", ImpresorFiscal::LINEA_SUPERIOR		);
			// pIF->MostrarMensajeEnDisplay ("Hola Mundo", ImpresorFiscal::LINEA_INFERIOR		);
			// pIF->MostrarMensajeEnDisplay ("Hola Mundo", ImpresorFiscal::SECCION_DE_REPETICION);

			// printf ("Estado Cajón: %s\n",
			// 	pIF->CajonAbierto () ? "ABIERTO" : "CERRADO");

			// pIF->AbrirCajon ();
			// pIF->AbrirDNF (ImpresorFiscal::ESTACION_TICKET);
			// pIF->ImprimirCodigoDeBarras (ImpresorFiscal::CODIGO_TIPO_EAN_13, "123456789012", true, true);
			// pIF->ImprimirCodigoDeBarras (ImpresorFiscal::CODIGO_TIPO_EAN_13, "123456789012", true, true);
			// for (unsigned i = 0; i <= 100; i += 10) {
			// 	char Buf [200];
			// 	_snprintf (Buf, sizeof Buf, "%*.*s", i, i, "Texto No Fiscal - Texto No Fiscal");
			// 	pIF->ImprimirTextoNoFiscal (Buf);
			// }
			// pIF->CortarPapel ();
			// pIF->CerrarDNF ();
			// pIF->AbrirCajon ();

			// Emitimos un DNF en estación Slip
			// pIF->AbrirDNF (ImpresorFiscal::ESTACION_SLIP);
			// pIF->ImprimirTextoNoFiscal ("Texto No Fiscal");
			// pIF->CerrarDNF ();

			// Re-imprimimos el último comprobante emitido
			// pIF->ReimprimirComprobante ();

			// printf ("Estado CF: <%d> <%s>\n", pIF->EstadoControlador (), pIF->DescripcionEstadoControlador ().c_str ());
			// printf ("Documento en Curso: <0x%02X> <%s>\n", pIF->DocumentoEnCurso (), pIF->DescripcionDocumentoEnCurso ().c_str ());
			// pIF->AbrirDNF (ImpresorFiscal::ESTACION_TICKET);
			// printf ("Estado CF: <%d> <%s>\n", pIF->EstadoControlador (), pIF->DescripcionEstadoControlador ().c_str ());
			// printf ("Documento en Curso: <0x%02X> <%s>\n", pIF->DocumentoEnCurso (), pIF->DescripcionDocumentoEnCurso ().c_str ());
			// pIF->CerrarDNF ();
			// printf ("Flag de Ultimo Documento Cancelado: %s\n",
			// 	(pIF->UltimoDocumentoFueCancelado () == true) ? "TRUE" : "FALSE");

			// pIF->AbrirDNF (ImpresorFiscal::ESTACION_TICKET);
			// pIF->CerrarDNF ();
		}
#endif


// ############################
//	Pruebas de Comandos de DF
// ############################

#if 0
		{
			// pIF->CancelarComprobanteFiscal ();

			printf ("EJECUTANDO COMANDO ABRIR DF\n");
#if 0
			pIF->DatosCliente ("Ricardo Daniel Cárdenes",
				"00000000000", ImpresorFiscal::TIPO_CUIT, 
					ImpresorFiscal::RESPONSABLE_INSCRIPTO,
						"Los Andes 892");
			pIF->AbrirDF (ImpresorFiscal::FACTURA_A);
#else
			pIF->AbrirDF (ImpresorFiscal::FACTURA_B);
#endif
			printf ("Primer Nro. de Doc. en Curso: %lu\n", pIF->PrimerNumeroDeDocumentoActual ());

			printf ("EJECUTANDO COMANDO IMPRIMIR TEXTO FISCAL\n");
			pIF->ImprimirTextoFiscal ("¶Texto Fiscal - Texto Fiscal - Texto Fiscal - Texto Fiscal");
			printf ("Estado CF: <%d> <%s>\n", pIF->EstadoControlador (), pIF->DescripcionEstadoControlador ().c_str ());

			printf ("EJECUTANDO COMANDO IMPRIMIR ITEM\n");
			pIF->ImprimirItem ("Item 1", 1, 1, 21, 100, false);
			pIF->ImprimirItem ("Item 2", 1, 1, 18, 100, false);
			pIF->ImprimirItem ("Item 3", 1, 1, 15, 100, false);
			
			pIF->EstablecerModalidadDescripcionesLargas (true);

			pIF->ImprimirItem ("Item1-Item1-Item1-Item1-Item1 - Item1", 1.234567890, 10.7525, 21, 0, false);
			pIF->ImprimirItem ("Item 1 - Item 1 - Item 1 - Item 1 - Item 1 - Item 1", 1.234567890, 10.7525, 21, 0, false);
			// pIF->ImprimirItem ("Item 2", 1, 1, 21, 100, false);
			
			// pIF->ImprimirItem ("1234567890123456 890123456789012345678901234567890 Item 2", 1, 1, 21, 100, false);
			// pIF->ImprimirItem ("     1234567890 123456789012345678901234567890 1234567890     ", 1, 1, 21, 100, false);
			// pIF->ImprimirItem ("     1234567890 123456789012345678901234567890", 1, 1, 21, 100, false);
			
			// pIF->ImprimirItem ("123456789012345678901234567890 abcdefghijabcdefghijabcdefghij 123456789012345678901234567890 abcdefghijabcdefghijabcdefghij 123456789012345678901234567890 abcdefghijabcdefghijabcdefghij ", 1, 1, 21, 100, false);

			// pIF->ImprimirItem ("     1234567890 123456789012345678901234567890", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++30++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++ 0++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++ 0++++++++40++++++++50++++++ +60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++ 0++++++++40++++++++50++++++ +60++++++++70++++++++80++++ +++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++3 ++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++3 ++++++++40++++++++50+++++++ 60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++3 ++++++++40++++++++50+++++++ 60++++++++70++++++++80+++++ ++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("  ++++++10++++++++20++++++++30++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("  ++++++10++++++++20++++++    ++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++  ", 1, 100, 0, 0);
			// pIF->ImprimirItem ("  ++++++10++++++++20++++++    ++++++++40++++++++50++++    60++++++++70++++++++80++    ++90+++++++100++  ", 1, 100, 0, 0);
			// pIF->ImprimirItem ("  ++++++10++++++++20++++++    ++++++++40++++++++50++++    60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++30 +++++++40++++++++50++++++++60++ +++++70++++++++80++++++++90+++ +++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++30 +++++++40++++++++50++++++++60++ +++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++30 +++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("              +                           +                           +                           +     ", 1, 100, 0, 0);
			// pIF->ImprimirItem ("              +                           +                           +                                 ", 1, 100, 0, 0);
			// pIF->ImprimirItem ("              +                           +                                                             ", 1, 100, 0, 0);
			// pIF->ImprimirItem ("              +                                                                                         ", 1, 100, 0, 0);
			// pIF->ImprimirItem ("                                                                                                        ", 1, 100, 0, 0);
			// pIF->ImprimirItem ("+++++++++10++++++++20++++++++30++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++30++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100+++++", 1, 100, 0, 0);
			// pIF->ImprimirItem (" ++++++++10++++++++20++++++++30++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++", 1, 100, 0, 0);
			// pIF->ImprimirItem ("++++++++10++++++++20++++++++30++++++++40++++++++50++++++++60++++++++70++++++++80++++++++90+++++++100++++ ", 1, 100, 0, 0);

			printf ("EJECUTANDO COMANDO DE DESC/REC ULTIMO ITEM\n");
			pIF->DescuentoUltimoItem ("Desc/Rec Ultimo Item", 1, false);
			
			printf ("EJECUTANDO COMANDO DE BONIF/REC/DEV.ENVASES\n");
			pIF->DevolucionDescuento ("Bonificación/Recargo/DevoluciónEnvases...", 1, 1, 100, false, ImpresorFiscal::DESCUENTO_RECARGO);

			printf ("EJECUTANDO COMANDO DE DESC/REC GENERAL\n");
			pIF->DescuentoGeneral    ("Desc/Rec General ...", 1, false);

			ImpresorFiscal::RTA_Subtotal R;
			
			printf ("EJECUTANDO COMANDO SUBTOTAL ... RECUPERANDO RESPUESTA\n");
			pIF->Subtotal (true, &R);
			printf ("CantidadItemsVendidos:  %.2f\n", R.CantidadItemsVendidos);
			printf ("MontoVentas:            %.2f\n", R.MontoVentas);
			printf ("MontoIVA:               %.2f\n", R.MontoIVA);
			printf ("MontoPagado:            %.2f\n", R.MontoPagado);
			printf ("MontoIVANoInscripto:    %.2f\n", R.MontoIVANoInscripto);
			printf ("MontoImpuestosInternos: %.2f\n", R.MontoImpuestosInternos);

			printf ("EJECUTANDO COMANDO CARGA MONTO IVA NO INSCRIPTO\n");
 			pIF->EspecificarIVANoInscripto (1);

			printf ("EJECUTANDO COMANDOS DE PERCEPCIONES\n");
 			pIF->EspecificarPercepcionGlobal ("Percepción Global - Percepción Global", 1);
 			pIF->EspecificarPercepcionPorIVA ("Percepción por IVA - Percepción por IVA", 1, 21);

			printf ("EJECUTANDO COMANDO DE PAGO\n");
			printf ("Vuelto / Monto por Pagar: %.2f\n", pIF->ImprimirPago ("Efectivo - Efectivo - Efectivo - Efectivo - Efectivo - Efectivo", 1));

			printf ("EJECUTANDO COMANDO CERRAR DF\n");
			printf ("Número de DF recién cerrado: %lu\n", pIF->CerrarDF ());

			// printf ("SOLICITANDO PAGINAS DE ULTIMO DOCUMENTO\n");
			// printf ("Nro. de Páginas: %lu\n", pIF->PaginasDeUltimoDocumento ());

			// pIF->CancelarComprobanteFiscal ();
			// pIF->CancelarComprobante ();

			// ImpresorFiscal::RTA_CAIUltimoDocumento R;

			// printf ("EJECUTANDO LECTURA DE CAI DEL ULTIMO DOCUMENTO A\n");
			// pIF->CAIUltimoDocumentoA (&R);
			// printf ("Documento:     %d\n",  R.Documento);
			// printf ("Primer Número: %lu\n", R.PrimerNumero);
			// printf ("Ultimo Número: %lu\n", R.UltimoNumero);
			// printf ("Código CAI:    %s\n",  R.CAI.c_str ());
		}
#endif


// ############################
//	Pruebas de Comandos de DNF
// ############################

#if 0
		{
#if 0
			// pIF->CancelarComprobante ();

			printf ("EJECUTANDO COMANDO ABRIR DNF EN ESTACION TICKET\n");
			pIF->AbrirDNF (ImpresorFiscal::ESTACION_TICKET);
			pIF->ImprimirTextoNoFiscal ("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
			pIF->ImprimirTextoNoFiscal ("********10********20********30********40********50********60********70********80********90*******100");
			pIF->ImprimirCodigoDeBarras (ImpresorFiscal::CODIGO_TIPO_EAN_13, "123456789012", true, true);

			printf ("EJECUTANDO COMANDO CERRAR DNF\n");
			pIF->CerrarDNF ();
#endif

#if 0
			printf ("EJECUTANDO COMANDO ABRIR DNF EN ESTACION SLIP\n");
			pIF->AbrirDNF (ImpresorFiscal::ESTACION_SLIP);
			pIF->ImprimirTextoNoFiscal ("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
			pIF->ImprimirTextoNoFiscal ("********10********20********30********40********50********60********70********80********90*******100");
			pIF->ImprimirCodigoDeBarras (ImpresorFiscal::CODIGO_TIPO_EAN_13, "123456789012", true, true);
			printf ("EJECUTANDO COMANDO CERRAR DNF\n");
			pIF->CerrarDNF ();
#endif
		}
#endif


// #############################
//	Pruebas de Comandos de DNFH
// #############################

#if 0
		{
			// pIF->CancelarComprobante ();

			printf ("EJECUTANDO COMANDO ABRIR DNFH\n");
#if 0
			pIF->DatosCliente ("Ricardo Daniel Cárdenes",
				"00000000000", ImpresorFiscal::TIPO_CUIT, 
					ImpresorFiscal::RESPONSABLE_INSCRIPTO,
						"Los Andes 892");
			pIF->EspecificarInformacionRemitoComprobanteOriginal (1, "0001-12345678");
			pIF->AbrirDNFH (ImpresorFiscal::NOTA_CREDITO_A);
#else
			pIF->DatosCliente ("Ricardo Daniel Cárdenes",
				"00000000000", ImpresorFiscal::TIPO_CUIT, 
					ImpresorFiscal::RESPONSABLE_EXENTO,
						"Los Andes 892");
			pIF->EspecificarInformacionRemitoComprobanteOriginal (1, "0001-12345678");
			pIF->AbrirDNFH (ImpresorFiscal::NOTA_CREDITO_B);
#endif

			printf ("Estado CF: <%d> <%s>\n", pIF->EstadoControlador (), pIF->DescripcionEstadoControlador ().c_str ());
			printf ("Documento en Curso: <0x%02X> <%s>\n", pIF->DocumentoEnCurso (), pIF->DescripcionDocumentoEnCurso ().c_str ());

			printf ("EJECUTANDO COMANDO IMPRIMIR ITEM\n");
			pIF->ImprimirItem ("Item 1", 1, 1, 21, 100, false);
			pIF->ImprimirItem ("Item 2", 1, 2, 18, 100, false);
			pIF->ImprimirItem ("Item 2", 1, 1, 18, 100, true);

			printf ("EJECUTANDO COMANDOS DE PERCEPCIONES\n");
			pIF->EspecificarPercepcionPorIVA ("Percepción 1 IVA 21 %", 1, 21);
			pIF->EspecificarPercepcionPorIVA ("Percepción 2 IVA 21 %", 2, 21);
			pIF->EspecificarPercepcionPorIVA ("Percepción 1 IVA 18 %", 2, 18);
			pIF->EspecificarPercepcionGlobal ("Percepción Global 1", 1);
			pIF->EspecificarPercepcionGlobal ("Percepción Global 2", 2);

			printf ("EJECUTANDO COMANDO CERRAR DNFH\n");
			printf ("Número de DNFH recién cerrado: %lu\n", pIF->CerrarDNFH ());

			// printf ("SOLICITANDO PAGINAS DE ULTIMO DOCUMENTO\n");
			// printf ("Nro. de Páginas: %lu\n", pIF->PaginasDeUltimoDocumento ());

			// pIF->CancelarComprobante ();

			// ImpresorFiscal::RTA_CAIUltimoDocumento R;

			// printf ("EJECUTANDO LECTURA DE CAI DEL ULTIMO DOCUMENTO A\n");
			// pIF->CAIUltimoDocumentoA (&R);
			// printf ("Documento:     %d\n",  R.Documento);
			// printf ("Primer Número: %lu\n", R.PrimerNumero);
			// printf ("Ultimo Número: %lu\n", R.UltimoNumero);
			// printf ("Código CAI:    %s\n",  R.CAI.c_str ());
		}
#endif


#if 0
		{
			// pIF->AbrirDNFH (ImpresorFiscal::SOLICITUD_CREDITO);
			// pIF->AbrirDNFH (ImpresorFiscal::COMUNICACION_CLIENTE);
			// pIF->AbrirDNFH (ImpresorFiscal::INGRESO_EGRESO_SUCURSAL);
			// pIF->AbrirDNFH (ImpresorFiscal::TICKET_PAGARE);
			pIF->AbrirDNFH (ImpresorFiscal::PAGARE);
			pIF->ImprimirInfoEnDNFH (1, "Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH");
			pIF->ImprimirInfoEnDNFH (2, "Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH");
			pIF->ImprimirInfoEnDNFH (3, "Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH - Texto libre de DNFH");
			printf ("Número de DNFH recién cerrado: %lu\n", pIF->CerrarDNFH ());
		}
#endif


// ####################################################
//	Prueba de Impresión de DNFH de Farmacia y Reparto
// ####################################################

#if 0
		{
			pIF->DNFHFarmacia (1);
			pIF->DNFHReparto (2);
		}
#endif


// ############################################
//	Prueba de Impresión de Voucher de Tarjeta
// ############################################

#if 0
		{
			ImpresorFiscal::FECHA Fecha ("050912");

			printf ("EJECUTANDO COMANDOS DE IMPRESION DE VOUCHER CON NUMERO COMPROBANTE NUMERICO\n");
			pIF->ImprimirVoucher (
				"Abu el Cotur - Abu el Cotur - Abu el Cotur",
				"Truchocard - Truchocard - Truchocard - Truchocard - Truchocard", 
				ImpresorFiscal::VOUCHER_DE_DEVOLUCION,
				"12345678901234567890123456789012345678901234567890",
				Fecha, ImpresorFiscal::TARJETA_CREDITO, 12,
				"123123123123123123123123123123123123123123123123123123123123123",
				100, 200, 300, ImpresorFiscal::INGRESO_DE_TARJETA_MANUAL,
				ImpresorFiscal::OPERACION_TARJETA_ONLINE, 400,
				"$ 723,50 - $ 723,50 - $ 723,50 - $ 723,50 - $ 723,50 - $ 723,50",
				12345678, 1,
				"Juan Larré - Juan Larré - Juan Larré - Juan Larré - Juan Larré",
				ImpresorFiscal::ESTACION_TICKET);

			printf ("EJECUTANDO COMANDOS DE IMPRESION DE VOUCHER CON NUMERO COMPROBANTE ALFANUMERICO\n");
			pIF->ImprimirVoucher ("Abu el Cotur - Abu el Cotur - Abu el Cotur",
				"Truchocard - Truchocard - Truchocard - Truchocard - Truchocard", 
				ImpresorFiscal::VOUCHER_DE_DEVOLUCION,
				"12345678901234567890123456789012345678901234567890",
				Fecha, ImpresorFiscal::TARJETA_CREDITO, 12,
				"123123123123123123123123123123123123123123123123123123123123123",
				100, 200, 300, ImpresorFiscal::INGRESO_DE_TARJETA_MANUAL,
				ImpresorFiscal::OPERACION_TARJETA_ONLINE, 400,
				"$ 723,50 - $ 723,50 - $ 723,50 - $ 723,50 - $ 723,50 - $ 723,50",
				"12345678 - 12345678 - 12345678 - 12345678 - 12345678 - 12345678",
				1,
				"Juan Larré - Juan Larré - Juan Larré - Juan Larré - Juan Larré",
				ImpresorFiscal::ESTACION_TICKET);
		}
#endif


// ####################
//	Prueba de StatPRN
// ####################

#if 0
		{
// #define PROBAR_STAT_PRN

#ifdef PROBAR_STAT_PRN
			pIF->ReintentarErroresImpresora (false);
#endif
			pIF->AbrirDF (ImpresorFiscal::FACTURA_B);
			for ( unsigned i = 0; i < 1; i++ )
				pIF->ImprimirItem ("Item 1", 1, 1, 21, 100, false);
			pIF->CancelarComprobante ();

#ifdef PROBAR_STAT_PRN
			while ( pIF->HuboStatPRN () )
			{
				Sleep (500);
				printf ("Reintentando Ultimo Error Impresora\n");
				pIF->ReintentarUltimoErrorImpresora ();
			}
#endif
		}
#endif


	}
	catch(Excepcion &e)
	{
		printf("Excepción: %s (%s)\n", e.Descripcion.c_str(), e.Contexto.c_str());
	}

	return 0;
}
