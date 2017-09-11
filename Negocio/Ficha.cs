using System;
using System.Collections.Generic;
using System.Data;
using MarUtils.Soporte;
using MarUtils.Entidades;
using DAL;

namespace Negocio
{
	public class Ficha : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Fichas"; } }
		public static string NombreEntidad
		{ get { return "Ficha"; } }
		public static string NombreClave
		{ get { return "IdFicha"; } }
		#endregion

		#region Atributos y Propiedades
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
		string _Nombre;
		public string Nombre
		{
			get
			{
				return _Nombre;
			}
			set
			{
				_Nombre = value;
			}
		}
		uint _StockPreferido;
		public uint StockPreferido
		{
			get
			{
				return _StockPreferido;
			}
			set
			{
				_StockPreferido = value;
			}
		}
		decimal _Precio;
		public decimal Precio
		{
			get
			{
				return _Precio;
			}
			set
			{
				_Precio = value;
			}
		}
		#endregion Atributos y Propiedades

		#region Constructores
		public Ficha() { }

		public Ficha(DataRow r)
		{
			IdFicha = uint.Parse(r["IdFicha"].ToString());
			Nombre = r["Nombre"].ToString();
			StockPreferido = uint.Parse(r["StockPreferido"].ToString());
			Precio = decimal.Parse(r["Precio"].ToString());
		}
		#endregion Constructores

		#region Métodos
		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.Fichas, "IdFicha", id);
		}

		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.Fichas);
		}

		public static Ficha GetSingleE(uint id)
		{
			return new Ficha(GetSingle(id).Rows[0]);
		}

		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.Fichas);
		}
		#endregion Métodos

		#region ABM
		public override ABMResultado Alta()
		{
			return Datos.ABM(enTablas.Fichas, enABMFuncion.Alta, ProtocoloDatos());
		}

		public override ABMResultado Modificacion()
		{
			return Datos.ABM(enTablas.Fichas, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return Datos.ABM(enTablas.Fichas, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion ABM
	}
}