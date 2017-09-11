#ifndef PROTOCOLO_FISCAL_H
#define PROTOCOLO_FISCAL_H

#ifdef P_WIN32
#pragma warning(disable:4786) // disable spurious C4786 warnings
#endif

#include <string>
#include <vector>
#include <map>
#include <list>

#include "Transporte.h"
#include "Eventos.h"
#include "Excepcion.h"
#include "Mutex.h"

class ProtocoloFiscal : ManejadorDeEventos
{
public:
	ProtocoloFiscal();
	virtual ~ProtocoloFiscal();
	
	void EstablecerTransporte(Transporte *pTransporte) throw(Excepcion);
	void EliminarTransporte();

	void ConfigurarParametros(unsigned Reintentos, unsigned Timeout);

	virtual void EnviarPaqueteFiscal(const std::string &Paquete) throw(Excepcion);
//	std::string ObtenerRespuesta() const;
	std::vector<std::string> ObtenerCamposRespuesta() const;
	virtual char NumeroComandoUltimaRespuesta() const = 0;
	void Abortar();
	unsigned ContadorImpresoraOcupada () const;
	void ReintentarErroresComunicaciones(bool Reintentar);
	bool ReintentarErroresComunicaciones() const;
	
	void EstablecerManejadorDeEventos(ManejadorDeEventos *);

	enum Eventos
	{
		FALTA_PAPEL,
		IMPRESOR_OCUPADO,
		STAT_PRN,
		IDLE_TIME,
		RETRYING_TIMEOUT
	};

	void Ansi(bool Modo);
	bool Ansi() const;

	static unsigned DEFAULT_RETRIES;
	static unsigned DEFAULT_TIMEOUT;
	static unsigned MIN_RETRIES;
	static unsigned MIN_TIMEOUT;

protected:
	void EnviarPaquete(const std::string &Paquete) throw(Excepcion);
	virtual std::string ArmarPaqueteFiscal(const std::string &Paquete) = 0;
	virtual unsigned char NumeroPaqueteEnRespuesta() const = 0;
	virtual char NumeroComandoEnRespuesta() const = 0;
	virtual void ObtenerNumeroComandoDeRespuesta(const std::string &r) = 0;
	void Evento(unsigned, unsigned);
	void CheckAbortion() throw (Excepcion);

	virtual unsigned OffsetPrimerCampo() const = 0;
	void EsperarRespuesta(bool WaitSTX) throw (Excepcion);
	std::string Checksum(const std::string &s) const;
	bool ChecksumOK(const std::string &RespuestaRecibida, const std::string &ChecksumRecibido); 
	void RaiseEvent(Eventos);

	virtual void RespuestaLista(const std::string &r) throw(Excepcion);

	unsigned char PacketNumber;
	char CommandNumber;
	char CommandNumberInAnswer;
	Mutex SendMutex;
	bool AborcionEnProgreso;
	std::string Respuesta;
	std::vector<std::string> VectorRespuesta;
	Transporte *pTransporte;
	ManejadorDeEventos *pEventos;
	bool ModoAnsi;
	unsigned DC2Count;
	bool RetryTimeouts;
	unsigned ConfiguredRetries;
	unsigned ConfiguredTimeout;
};

class ProtocoloOriginal : public ProtocoloFiscal
{
public:
	char NumeroComandoUltimaRespuesta() const;

protected:
	virtual std::string ArmarPaqueteFiscal(const std::string &Paquete);
	virtual unsigned char NumeroPaqueteEnRespuesta() const;
	virtual char NumeroComandoEnRespuesta() const;
	virtual void ObtenerNumeroComandoDeRespuesta(const std::string &r);
	virtual unsigned OffsetPrimerCampo() const;
};

class ProtocoloStatPRN : public ProtocoloFiscal
{
public:
	ProtocoloStatPRN();

	char NumeroComandoUltimaRespuesta() const;

	void ReintentarErroresImpresora(bool Reintentar);
	bool ReintentarErroresImpresora() const;

	virtual void EnviarPaqueteFiscal(const std::string &Paquete) throw(Excepcion);
	void EnviarStatPRN() throw(Excepcion);
	bool HuboStatPRN();
	unsigned ContadorStatPRN() const;
protected:
	virtual void RespuestaLista(const std::string &r) throw(Excepcion);
	virtual std::string ArmarPaqueteFiscal(const std::string &Paquete);
	virtual unsigned char NumeroPaqueteEnRespuesta() const;
	virtual char NumeroComandoEnRespuesta() const;
	virtual void ObtenerNumeroComandoDeRespuesta(const std::string &r);
	virtual unsigned OffsetPrimerCampo() const;

	const char CMD_STATPRN;
	unsigned StatPRNCount;

	bool WasStatPRN;
	bool Reintentar;
};

#endif

