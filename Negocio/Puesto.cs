using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Puesto : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Puestos"; } }
		public static string NombreEntidad
		{ get { return "Puesto"; } }
		public static string NombreClave
		{ get { return "IdPuesto"; } }

		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdPuesto = 0;
		public System.UInt32 IdPuesto
		{
			get { return _IdPuesto; }
			set { _IdPuesto = value; }
		}

		System.Decimal _Ciclo = 0;
		public System.Decimal Ciclo
		{
			get { return _Ciclo; }
			set { _Ciclo = value; }
		}

		System.Decimal _Margen = 0;
		public System.Decimal Margen
		{
			get { return _Margen; }
			set { _Margen = value; }
		}

		uint _IdArticulo = 0;
		public uint IdArticulo
		{
			get { return _IdArticulo; }
			set { _IdArticulo = value; }
		}

		System.Int32 _SurtidorVOX = 0;
		public System.Int32 SurtidorVOX
		{
			get { return _SurtidorVOX; }
			set { _SurtidorVOX = value; }
		}

		System.Int32 _MangueraVOX = 0;
		public System.Int32 MangueraVOX
		{
			get { return _MangueraVOX; }
			set { _MangueraVOX = value; }
		}
		#endregion

		#region Constructores
		public Puesto() { }

		public Puesto(DataRow r)
		{
			IdPuesto = uint.Parse(r["IdPuesto"].ToString());
			Ciclo = decimal.Parse(r["Ciclo"].ToString());
			Margen = decimal.Parse(r["Margen"].ToString());
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			SurtidorVOX = int.Parse(r["SurtidorVOX"].ToString());
			MangueraVOX = int.Parse(r["MangueraVOX"].ToString());
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Puestos);
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.Puestos);
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.Puestos, NombreClave, id);
		}

		public static Puesto GetSingleE(uint id)
		{
			return new Puesto(GetSingle(id).Rows[0]);
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			//ABMResultado res = DAL.Datos.ABM(DAL.enTablas.Puestos, enABMFuncion.Alta, ProtocoloDatos());
			//if (res.CodigoError == enErrores.Ninguno)
			//    IdPuesto = res.IdInsercion;
			//return res;
			return new ABMResultado(enErrores.LogicaInvalida, "No se pueden ingresar nuevos puestos");
		}

		public override ABMResultado Modificacion()
		{
			if (IdPuesto == 0)
				return new ABMResultado(enErrores.LogicaInvalida, "No se puede modificar un puesto inexistente. Use ingresar. ");
			return DAL.Datos.ABM(DAL.enTablas.Puestos, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return new ABMResultado(enErrores.LogicaInvalida, "No se pueden eliminar los puestos");
			//if (IdPuesto == 1)
			//    return new ABMResultado(enErrores.Cancelado, "No se puede eliminar este usuario");
			//return DAL.Datos.ABM(DAL.enTablas.Puestos, enABMFuncion.Baja, ProtocoloDatos());
		}

		#endregion

	}
}