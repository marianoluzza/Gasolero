using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Parcial : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Parciales"; } }
		public static string NombreEntidad
		{ get { return "Parcial"; } }
		public static string NombreClave
		{ get { return "IdParcial"; } }

		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdParcial;
		public System.UInt32 IdParcial
		{
			get { return _IdParcial; }
			set { _IdParcial = value; }
		}

		System.DateTime _Fecha;
		public System.DateTime Fecha
		{
			get { return _Fecha; }
			set { _Fecha = value; }
		}

		System.UInt32 _Turno;
		public System.UInt32 Turno
		{
			get { return _Turno; }
			set { _Turno = value; }
		}
		#endregion

		#region Constructores
		public Parcial() { }

		public Parcial(DataRow r)
		{
			IdParcial = uint.Parse(r["IdParcial"].ToString());
			Fecha = DateTime.Parse(r["Fecha"].ToString());
			Turno = uint.Parse(r["Turno"].ToString());
		}
		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Parciales);
		}

		public override string ToString()
		{
			return "Cierre Parcial " + Fecha;
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.Parciales);
		}

		public static DataTable GetForTurno(uint turno)
		{
			DataTable dt = DAL.Datos.ParcialesGetListForTurno(turno);
			//agregar cierre final como parcial si existe, y si hubo parciales
			if (turno <= Reglas.TurnoUltimo.Numero && dt.Rows.Count > 0)
			{
				Turno t = new Turno();
				t.Numero = turno;
				t = t.GetForNumero();
				dt.Rows.Add(0, t.FechaFinal, t.Numero, "Final");
			}
			return dt;
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.Parciales, NombreClave, id);
		}

		public static Parcial GetSingleE(uint id)
		{
			return new Parcial(GetSingle(id).Rows[0]);
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.Parciales, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdParcial = res.IdInsercion;
			return res;
		}

		public override ABMResultado Modificacion()
		{
			return DAL.Datos.ABM(DAL.enTablas.Parciales, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return DAL.Datos.ABM(DAL.enTablas.Parciales, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion

	}
}