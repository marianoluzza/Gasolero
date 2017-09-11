using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarUtils.Soporte;
using DAL;
using MarUtils.Entidades;
using System.Data;

namespace Negocio
{
	public class CierreFicha : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "CierreFichas"; } }
		public static string NombreEntidad
		{ get { return "CierreFicha"; } }
		public static string NombreClave
		{ get { return "IdCierreFicha"; } }
		#endregion

		#region Atributos y Propiedades
		uint _IdCierreFicha;
		public uint IdCierreFicha
		{
			get
			{
				return _IdCierreFicha;
			}
			set
			{
				_IdCierreFicha = value;
			}
		}
		uint _IdFicha;
		public uint IdFicha
		{
			get
			{
				return _IdFicha;
			}
			set
			{
				_IdFicha = value;
			}
		}
		uint _IdVendedor;
		public uint IdVendedor
		{
			get
			{
				return _IdVendedor;
			}
			set
			{
				_IdVendedor = value;
			}
		}
		uint _Entrada;
		public uint Entrada
		{
			get
			{
				return _Entrada;
			}
			set
			{
				_Entrada = value;
			}
		}
		uint _Salida;
		public uint Salida
		{
			get
			{
				return _Salida;
			}
			set
			{
				_Salida = value;
			}
		}
		decimal _Costo;
		public decimal Costo
		{
			get
			{
				return _Costo;
			}
			set
			{
				_Costo = value;
			}
		}
		uint _Turno;
		public uint Turno
		{
			get
			{
				return _Turno;
			}
			set
			{
				_Turno = value;
			}
		}
		#endregion Atributos y Propiedades

		#region Constructores
		public CierreFicha() { }

		public CierreFicha(DataRow r)
		{
			IdCierreFicha = uint.Parse(r["IdCierreFicha"].ToString());
			IdFicha = uint.Parse(r["IdFicha"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			Entrada = uint.Parse(r["Entrada"].ToString());
			Salida = uint.Parse(r["Salida"].ToString());
			Costo = decimal.Parse(r["Costo"].ToString());
			Turno = uint.Parse(r["Turno"].ToString());
		}
		#endregion Constructores

		#region Métodos
		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.CierreFichas, "IdCierreFicha", id);
		}

		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.CierreFichas);
		}

		public static CierreFicha GetSingleE(uint id)
		{
			return new CierreFicha(GetSingle(id).Rows[0]);
		}
		#endregion Métodos

		#region ABM
		public override ABMResultado Alta()
		{
			return Datos.ABM(enTablas.CierreFichas, enABMFuncion.Alta, ProtocoloDatos());
		}

		public override ABMResultado Modificacion()
		{
			return Datos.ABM(enTablas.CierreFichas, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return Datos.ABM(enTablas.CierreFichas, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion ABM
	}
}