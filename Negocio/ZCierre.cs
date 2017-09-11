using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class ZCierre : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "ZCierres"; } }
		public static string NombreEntidad
		{ get { return "ZCierre"; } }
		public static string NombreClave
		{ get { return "IdZCierre"; } }

		#endregion

		#region Atributos y Propiedades - Sistema
		System.UInt32 _IdZCierre;
		public System.UInt32 IdZCierre
		{
			get { return _IdZCierre; }
			set { _IdZCierre = value; }
		}

		System.UInt32 _IdVendedor;
		public System.UInt32 IdVendedor
		{
			get { return _IdVendedor; }
			set { _IdVendedor = value; }
		}

		System.DateTime _Fecha;
		public System.DateTime Fecha
		{
			get { return _Fecha; }
			set { _Fecha = value; }
		}
		#endregion

		#region Atributos y Propiedades - Dominio
		string _numeroZ = "";
		public string NumeroZ
		{
			get { return _numeroZ; }
			set { _numeroZ = value; }
		}
		uint _cantDFCancelados = 0;
		public uint CantDFCancelados
		{
			get { return _cantDFCancelados; }
			set { _cantDFCancelados = value; }
		}
		uint _cantDNFHEmitidos = 0;
		public uint CantDNFHEmitidos
		{
			get { return _cantDNFHEmitidos; }
			set { _cantDNFHEmitidos = value; }
		}
		uint _cantDNFEmitidos = 0;
		public uint CantDNFEmitidos
		{
			get { return _cantDNFEmitidos; }
			set { _cantDNFEmitidos = value; }
		}
		uint _cantDFEmitidos = 0;
		public uint CantDFEmitidos
		{
			get { return _cantDFEmitidos; }
			set { _cantDFEmitidos = value; }
		}
		string _ultimoFiscalBC = "";
		public string UltimoFiscalBC
		{
			get { return _ultimoFiscalBC; }
			set { _ultimoFiscalBC = value; }
		}
		string _ultimoFiscalA = "";
		public string UltimoFiscalA
		{
			get { return _ultimoFiscalA; }
			set { _ultimoFiscalA = value; }
		}
		decimal _montoVentasFiscal = 0;
		public decimal MontoVentasFiscal
		{
			get { return _montoVentasFiscal; }
			set { _montoVentasFiscal = value; }
		}
		decimal _montoIVAFiscal = 0;
		public decimal MontoIVAFiscal
		{
			get { return _montoIVAFiscal; }
			set { _montoIVAFiscal = value; }
		}
		decimal _montoImpIntFiscal = 0;
		public decimal MontoImpIntFiscal
		{
			get { return _montoImpIntFiscal; }
			set { _montoImpIntFiscal = value; }
		}
		decimal _montoPercepcionesFiscal = 0;
		public decimal MontoPercepcionesFiscal
		{
			get { return _montoPercepcionesFiscal; }
			set { _montoPercepcionesFiscal = value; }
		}
		decimal _montoIVANoInscriptoFiscal = 0;
		public decimal MontoIVANoInscriptoFiscal
		{
			get { return _montoIVANoInscriptoFiscal; }
			set { _montoIVANoInscriptoFiscal = value; }
		}
		string _ultimaNotaCreditoBC = "";
		public string UltimaNotaCreditoBC
		{
			get { return _ultimaNotaCreditoBC; }
			set { _ultimaNotaCreditoBC = value; }
		}
		string _ultimaNotaCreditoA = "";
		public string UltimaNotaCreditoA
		{
			get { return _ultimaNotaCreditoA; }
			set { _ultimaNotaCreditoA = value; }
		}
		decimal _montoVentasNotaCredito = 0;
		public decimal MontoVentasNotaCredito
		{
			get { return _montoVentasNotaCredito; }
			set { _montoVentasNotaCredito = value; }
		}
		decimal _montoIVANotaCredito = 0;
		public decimal MontoIVANotaCredito
		{
			get { return _montoIVANotaCredito; }
			set { _montoIVANotaCredito = value; }
		}
		decimal _montoImpIntNotaCredito = 0;
		public decimal MontoImpIntNotaCredito
		{
			get { return _montoImpIntNotaCredito; }
			set { _montoImpIntNotaCredito = value; }
		}
		decimal _montoPercepcionesNotaCredito = 0;
		public decimal MontoPercepcionesNotaCredito
		{
			get { return _montoPercepcionesNotaCredito; }
			set { _montoPercepcionesNotaCredito = value; }
		}
		decimal _montoIVANoInscriptoNotaCredito = 0;
		public decimal MontoIVANoInscriptoNotaCredito
		{
			get { return _montoIVANoInscriptoNotaCredito; }
			set { _montoIVANoInscriptoNotaCredito = value; }
		}
		string _ultimoRemito = "";
		public string UltimoRemito
		{
			get { return _ultimoRemito; }
			set { _ultimoRemito = value; }
		}
		uint _cantNCCanceladas = 0;
		public uint CantNCCanceladas
		{
			get { return _cantNCCanceladas; }
			set { _cantNCCanceladas = value; }
		}
		uint _cantDFBCEmitidos = 0;
		public uint CantDFBCEmitidos
		{
			get { return _cantDFBCEmitidos; }
			set { _cantDFBCEmitidos = value; }
		}
		uint _cantDFAEmitidos = 0;
		public uint CantDFAEmitidos
		{
			get { return _cantDFAEmitidos; }
			set { _cantDFAEmitidos = value; }
		}
		uint _cantNCBCEmitidos = 0;
		public uint CantNCBCEmitidos
		{
			get { return _cantNCBCEmitidos; }
			set { _cantNCBCEmitidos = value; }
		}
		uint _cantNCAEmitidos = 0;
		public uint CantNCAEmitidos
		{
			get { return _cantNCAEmitidos; }
			set { _cantNCAEmitidos = value; }
		}
		#endregion

		#region Constructores
		public ZCierre() { }

		public ZCierre(DataRow r)
		{
			IdZCierre = uint.Parse(r["IdZCierre"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			Fecha = DateTime.Parse(r["Fecha"].ToString());
			//
			NumeroZ = r["NumeroZ"].ToString();
			CantDFCancelados = uint.Parse(r["CantDFCancelados"].ToString());
			CantDNFHEmitidos = uint.Parse(r["CantDNFHEmitidos"].ToString());
			CantDNFEmitidos = uint.Parse(r["CantDNFEmitidos"].ToString());
			CantDFEmitidos = uint.Parse(r["CantDFEmitidos"].ToString());
			UltimoFiscalBC = r["UltimoFiscalBC"].ToString();
			UltimoFiscalA = r["UltimoFiscalA"].ToString();
			MontoVentasFiscal = decimal.Parse(r["MontoVentasFiscal"].ToString());
			MontoIVAFiscal = decimal.Parse(r["MontoIVAFiscal"].ToString());
			MontoImpIntFiscal = decimal.Parse(r["MontoImpIntFiscal"].ToString());
			MontoPercepcionesFiscal = decimal.Parse(r["MontoPercepcionesFiscal"].ToString());
			MontoIVANoInscriptoFiscal = decimal.Parse(r["MontoIVANoInscriptoFiscal"].ToString());
			UltimaNotaCreditoBC = r["UltimaNotaCreditoBC"].ToString();
			UltimaNotaCreditoA = r["UltimaNotaCreditoA"].ToString();
			MontoVentasNotaCredito = decimal.Parse(r["MontoVentasNotaCredito"].ToString());
			MontoIVANotaCredito = decimal.Parse(r["MontoIVANotaCredito"].ToString());
			MontoImpIntNotaCredito = decimal.Parse(r["MontoImpIntNotaCredito"].ToString());
			MontoPercepcionesNotaCredito = decimal.Parse(r["MontoPercepcionesNotaCredito"].ToString());
			MontoIVANoInscriptoNotaCredito = decimal.Parse(r["MontoIVANoInscriptoNotaCredito"].ToString());
			UltimoRemito = r["UltimoRemito"].ToString();
			CantNCCanceladas = uint.Parse(r["CantNCCanceladas"].ToString());
			CantDFBCEmitidos = uint.Parse(r["CantDFBCEmitidos"].ToString());
			CantDFAEmitidos = uint.Parse(r["CantDFAEmitidos"].ToString());
			CantNCBCEmitidos = uint.Parse(r["CantNCBCEmitidos"].ToString());
			CantNCAEmitidos = uint.Parse(r["CantNCAEmitidos"].ToString());
		}

		public ZCierre(DateTime fecha, object[] r)
		{
			IdZCierre = 0;
			IdVendedor = Reglas.VendedorActual.IdVendedor;
			Fecha = fecha;
			//
			NumeroZ = r[0].ToString();
			CantDFCancelados = uint.Parse(r[1].ToString());
			CantDNFHEmitidos = uint.Parse(r[2].ToString());
			CantDNFEmitidos = uint.Parse(r[3].ToString());
			CantDFEmitidos = uint.Parse(r[4].ToString());
			UltimoFiscalBC = r[5].ToString();
			UltimoFiscalA = r[6].ToString();
			MontoVentasFiscal = decimal.Parse(r[7].ToString());
			MontoIVAFiscal = decimal.Parse(r[8].ToString());
			MontoImpIntFiscal = decimal.Parse(r[9].ToString());
			MontoPercepcionesFiscal = decimal.Parse(r[10].ToString());
			MontoIVANoInscriptoFiscal = decimal.Parse(r[11].ToString());
			UltimaNotaCreditoBC = r[12].ToString();
			UltimaNotaCreditoA = r[13].ToString();
			MontoVentasNotaCredito = decimal.Parse(r[14].ToString());
			MontoIVANotaCredito = decimal.Parse(r[15].ToString());
			MontoImpIntNotaCredito = decimal.Parse(r[16].ToString());
			MontoPercepcionesNotaCredito = decimal.Parse(r[17].ToString());
			MontoIVANoInscriptoNotaCredito = decimal.Parse(r[18].ToString());
			UltimoRemito = r[19].ToString();
			CantNCCanceladas = uint.Parse(r[20].ToString());
			CantDFBCEmitidos = uint.Parse(r[21].ToString());
			CantDFAEmitidos = uint.Parse(r[22].ToString());
			CantNCBCEmitidos = uint.Parse(r[23].ToString());
			CantNCAEmitidos = uint.Parse(r[24].ToString());
		}
		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.ZCierres);
		}
		#endregion

		#region Consultas
		public static DataTable GetListaXFechas(DateTime dde, DateTime hta)
		{
			DataTable dt = Datos.ZCierresGetForFechas(dde, hta);
			return dt;
		}

		public static bool HayCierres(DateTime f)
		{
			DataTable cierres = GetListaXFechas(new DateTime(f.Year, f.Month, f.Day,
				0, 0, 0), new DateTime(f.Year, f.Month, f.Day, 23, 59, 59));
			return cierres.Rows.Count > 0;
		}
		#endregion

		#region ABM
		public static ABMResultado RegistrarCierre(DateTime fecha, object[] datos)
		{
			ZCierre z = new ZCierre(fecha, datos);
			ABMResultado res = z.Alta();
			return res;
		}

		public override ABMResultado Alta()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.ZCierres, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdZCierre = res.IdInsercion;
			return res;
		}

		public override ABMResultado Modificacion()
		{
			//ABMResultado res;
			bool exito = false;
			bool empeceTransa = DAL.Datos.InitTransa();
			try
			{
				return DAL.Datos.ABM(DAL.enTablas.ZCierres, enABMFuncion.Modificacion, ProtocoloDatos());
			}
			catch (Exception ex)
			{
				exito = false;
				return new ABMResultado(enErrores.ErrorBD, ex.Message);
			}
			finally
			{
				if (empeceTransa)
					DAL.Datos.FinTransa(exito);
			}
		}

		public override ABMResultado Baja()
		{
			return DAL.Datos.ABM(DAL.enTablas.ZCierres, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion

	}
}