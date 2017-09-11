using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Atajo : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Atajos"; } }
		public static string NombreEntidad
		{ get { return "Atajo"; } }
		public static string NombreClave
		{ get { return "IdAtajo"; } }
		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdAtajo;
		public System.UInt32 IdAtajo
		{
			get { return _IdAtajo; }
			set { _IdAtajo = value; }
		}

		System.UInt32 _IdArticulo;
		public System.UInt32 IdArticulo
		{
			get { return _IdArticulo; }
			set { _IdArticulo = value; }
		}

		System.String _Tecla;
		public System.String Tecla
		{
			get { return _Tecla; }
			set { _Tecla = value; }
		}

		System.Boolean _Control;
		public System.Boolean Control
		{
			get { return _Control; }
			set { _Control = value; }
		}

		System.Boolean _Shift;
		public System.Boolean Shift
		{
			get { return _Shift; }
			set { _Shift = value; }
		}

		System.Boolean _Alt;
		public System.Boolean Alt
		{
			get { return _Alt; }
			set { _Alt = value; }
		}

		#endregion

		#region Constructores
		public Atajo() { }

		public Atajo(DataRow r)
		{
			IdAtajo = uint.Parse(r["IdAtajo"].ToString());
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			Tecla = r["Tecla"].ToString();
			Control = r["Control"].ToString().ToLower() == "si" || r["Control"].ToString().ToLower() == "true" || r["Control"].ToString() == "1";
			Shift = r["Shift"].ToString().ToLower() == "si" || r["Shift"].ToString().ToLower() == "true" || r["Shift"].ToString() == "1";
			Alt = r["Alt"].ToString().ToLower() == "si" || r["Alt"].ToString().ToLower() == "true" || r["Alt"].ToString() == "1";
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.Atajos);
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.Atajos);
		}

		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.Atajos, NombreClave, id);
		}

		public static Atajo GetSingleE(uint id)
		{
			DataTable dt = GetSingle(id);
			if (dt.Rows.Count == 0)
				return null;
			else
				return new Atajo(dt.Rows[0]);
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.Atajos, enABMFuncion.Alta, this.ProtocoloDatos());
			if (abmRes.CodigoError == enErrores.Ninguno)
				IdAtajo = abmRes.IdInsercion;
			return abmRes;
		}

		public override ABMResultado Modificacion()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.Atajos, enABMFuncion.Modificacion, this.ProtocoloDatos());
			return abmRes;
		}

		public override ABMResultado Baja()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.Atajos, enABMFuncion.Baja, this.ProtocoloDatos());
			return abmRes;
		}
		#endregion
	}
}