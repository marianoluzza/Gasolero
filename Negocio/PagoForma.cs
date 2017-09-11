using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class PagoForma : EntidadBase, IComparable<PagoForma>
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "PagoFormas"; } }
		public static string NombreEntidad
		{ get { return "PagoForma"; } }
		public static string NombreClave
		{ get { return "IdPagoForma"; } }
		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdPagoForma;
		public System.UInt32 IdPagoForma
		{
			get { return _IdPagoForma; }
			set { _IdPagoForma = value; }
		}

		System.String _Descripcion;
		public System.String Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}

		System.Boolean _SumaAlTotal;
		public System.Boolean SumaAlTotal
		{
			get { return _SumaAlTotal; }
			set { _SumaAlTotal = value; }
		}

		System.UInt32 _Orden;
		public System.UInt32 Orden
		{
			get { return _Orden; }
			set { _Orden = value; }
		}

		System.Boolean _Habilitado;
		public System.Boolean Habilitado
		{
			get { return _Habilitado; }
			set { _Habilitado = value; }
		}

		#endregion

		#region Constructores
		public PagoForma() { }

		public PagoForma(DataRow r)
		{
			 IdPagoForma = uint.Parse(r["IdPagoForma"].ToString());
			 Descripcion = r["Descripcion"].ToString();
			 SumaAlTotal = r["SumaAlTotal"].ToString().ToLower() == "si" || r["SumaAlTotal"].ToString().ToLower() == "true" || r["SumaAlTotal"].ToString() == "1";
			 Orden = uint.Parse(r["Orden"].ToString());
			 Habilitado = r["Habilitado"].ToString().ToLower() == "si" || r["Habilitado"].ToString().ToLower() == "true" || r["Habilitado"].ToString() == "1";
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.PagoFormas);
		}

		public override string ToString()
		{
			return Descripcion ?? "";
		}

		public override int GetHashCode()
		{
			return IdPagoForma.GetHashCode();
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.PagoFormas);
		}

		public static DataTable GetHabilitados()
		{
			return Datos.PagoFormasGetHabilitados(true);
		}

		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.PagoFormas, NombreClave, id);
		}

		public static PagoForma GetSingleE(uint id)
		{
			DataTable dt = GetSingle(id);
			if (dt.Rows.Count == 0)
				return null;
			else
				return new PagoForma(dt.Rows[0]);
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.PagoFormas, enABMFuncion.Alta, this.ProtocoloDatos());
			if (abmRes.CodigoError == enErrores.Ninguno)
				IdPagoForma = abmRes.IdInsercion;
			return abmRes;
		}

		public override ABMResultado Modificacion()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.PagoFormas, enABMFuncion.Modificacion, this.ProtocoloDatos());
			return abmRes;
		}

		public override ABMResultado Baja()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.PagoFormas, enABMFuncion.Baja, this.ProtocoloDatos());
			return abmRes;
		}
		#endregion

		#region IComparable<uint> Members

		public int CompareTo(PagoForma other)
		{
			return this.IdPagoForma.CompareTo(other.IdPagoForma);
		}

		#endregion
	}
}