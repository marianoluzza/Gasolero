using System;
using System.Collections.Generic;
using System.Text;
using MarUtils.Soporte;
using DAL;
using System.Data;
using MarUtils.Entidades;

namespace Negocio
{
	public class Densidad : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Densidades"; } }
		public static string NombreEntidad
		{ get { return "Densidad"; } }
		public static string NombreClave
		{ get { return "IdDensidad"; } }

		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdDensidad;
		public System.UInt32 IdDensidad
		{
			get { return _IdDensidad; }
			set { _IdDensidad = value; }
		}

		System.UInt32 _IdArticulo;
		public System.UInt32 IdArticulo
		{
			get { return _IdArticulo; }
			set { _IdArticulo = value; }
		}

		System.Decimal _Valor;
		public System.Decimal Valor
		{
			get { return _Valor; }
			set { _Valor = value; }
		}

		#endregion

		#region Constructores
		public Densidad() { }

		public Densidad(DataRow r)
		{
			IdDensidad = uint.Parse(r["IdDensidad"].ToString());
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			Valor = decimal.Parse(r["Valor"].ToString());
			if (Valor >= 10)
				throw new Exception("Error de punto decimal");
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Densidades);
		}
		#endregion

		#region Consultas
		public static Densidad GetSingleForArt(uint idArt)
		{
			Densidad res = new Densidad();
			res._IdArticulo = idArt;
			res._IdDensidad = 1;
			res._Valor = DatosXml.DensidadGetForIdArticulo(idArt);
			return res;
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			return DatosXml.DensidadInsertSingle(IdArticulo, Valor);
		}

		public override ABMResultado Modificacion()
		{
			return new ABMResultado(enErrores.LogicaInvalida, "No se puede modificar una densidad");
		}

		public override ABMResultado Baja()
		{
			return DatosXml.DensidadDeleteForIdArticulo(IdArticulo);
		}
		#endregion
	}
}
