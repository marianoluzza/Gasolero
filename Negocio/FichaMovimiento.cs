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
	public class FichaMovimiento : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "FichaMovimientos"; } }
		public static string NombreEntidad
		{ get { return "FichaMovimiento"; } }
		public static string NombreClave
		{ get { return "IdFichaMovimiento"; } }
		#endregion

		#region Atributos y Propiedades
		uint _IdFichaMovimiento;
		public uint IdFichaMovimiento
		{
			get
			{
				return _IdFichaMovimiento;
			}
			set
			{
				_IdFichaMovimiento = value;
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
		uint _Cantidad;
		public uint Cantidad
		{
			get
			{
				return _Cantidad;
			}
			set
			{
				_Cantidad = value;
			}
		}
		#endregion Atributos y Propiedades

		#region Constructores
		public FichaMovimiento() { }

		public FichaMovimiento(DataRow r)
		{
			IdFichaMovimiento = uint.Parse(r["IdFichaMovimiento"].ToString());
			IdFicha = uint.Parse(r["IdFicha"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			Turno = uint.Parse(r["Turno"].ToString());
			Cantidad = uint.Parse(r["Cantidad"].ToString());
		}
		#endregion Constructores

		#region Métodos
		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.FichaMovimientos);
		}

		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.FichaMovimientos, "IdFichaMovimiento", id);
		}

		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.FichaMovimientos);
		}

		public static FichaMovimiento GetSingleE(uint id)
		{
			return new FichaMovimiento(GetSingle(id).Rows[0]);
		}
		#endregion Métodos

		#region ABM
		public override ABMResultado Alta()
		{
			return Datos.ABM(enTablas.FichaMovimientos, enABMFuncion.Alta, ProtocoloDatos());
		}

		public override ABMResultado Modificacion()
		{
			return Datos.ABM(enTablas.FichaMovimientos, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return Datos.ABM(enTablas.FichaMovimientos, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion ABM
	}
}