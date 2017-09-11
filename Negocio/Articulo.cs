using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Articulo : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Articulos"; } }
		public static string NombreEntidad
		{ get { return "Articulo"; } }
		public static string NombreClave
		{ get { return "IdArticulo"; } }

		#endregion

		#region Atributos y Propiedades
		System.String _Codigo = "";
		public System.String Codigo
		{
			get { return _Codigo; }
			set { _Codigo = value; }
		}

		System.String _Descripcion = "";
		public System.String Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}

		System.UInt32 _IdArticulo = 0;
		public System.UInt32 IdArticulo
		{
			get { return _IdArticulo; }
			set { _IdArticulo = value; }
		}

		System.Decimal _Precio = 0;
		public System.Decimal Precio
		{
			get { return _Precio; }
			set { _Precio = value; }
		}

		System.Decimal _Alicuota = 0;
		public System.Decimal Alicuota
		{
			get { return _Alicuota; }
			set { _Alicuota = value; }
		}

		System.Decimal _Impuesto = 0;
		public System.Decimal Impuesto
		{
			get { return _Impuesto; }
			set { _Impuesto = value; }
		}

		System.Boolean _ImpuestoPorcentual = false;
		public System.Boolean ImpuestoPorcentual
		{
			get { return _ImpuestoPorcentual; }
			set { _ImpuestoPorcentual = value; }
		}

		System.Decimal _VentaMaxima = 0;
		public System.Decimal VentaMaxima
		{
			get { return _VentaMaxima; }
			set { _VentaMaxima = value; }
		}

		int _CodProducto = 0;
		public int CodProducto
		{
			get { return _CodProducto; }
			set { _CodProducto = value; }
		}

		public Boolean Facturar { get; set; }

		public string Medida
		{
			get
			{
				return Descripcion.ToLower().Contains("gnc") ? "Mts3" : "Lts";
			}
		}
		#endregion

		#region Constructores
		public Articulo() { }

		public Articulo(DataRow r)
		{
			Codigo = r["Codigo"].ToString();
			Descripcion = r["Descripcion"].ToString();
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			Precio = decimal.Parse(r["Precio"].ToString());
			Impuesto = decimal.Parse(r["Impuesto"].ToString());
			Alicuota = decimal.Parse(r["Alicuota"].ToString());
			ImpuestoPorcentual = r["ImpuestoPorcentual"].ToString() == "1" || r["ImpuestoPorcentual"].ToString().ToLower() == "true";
			VentaMaxima = decimal.Parse(r["VentaMaxima"].ToString());
			CodProducto = int.Parse(r["CodProducto"].ToString());
			Facturar = r["Facturar"].ToString() == "1" || r["Facturar"].ToString().ToLower() == "true";
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Articulos);
		}

		/// <summary>
		/// Devuelve el porcentaje de impuesto interno que tiene el articulo (en decimal)
		/// </summary>
		/// <returns></returns>
		public decimal GetImpInt()
		{
			if (ImpuestoPorcentual)
				return Impuesto / 100;
			else
			{
				decimal pBase = Precio - Impuesto;
				pBase = pBase / (1 + Alicuota / 100);
				return Impuesto / pBase;
			}
		}

		/// <summary>
		/// Devuelve el precio base del articulo, sin impuestos
		/// </summary>
		/// <returns></returns>
		public decimal GetPrecioBase()
		{
			if (ImpuestoPorcentual)
			{
				decimal tasa = Alicuota + Impuesto;
				return (Precio / (100 + tasa)) * 100;
			}
			else
			{
				decimal precio = Precio - Impuesto;
				return (precio / (100 + Alicuota)) * 100;
			}
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			DataTable dt = DAL.Datos.GetAll(DAL.enTablas.Articulos);
			return dt;
		}

		public static List<uint> GetListaIds()
		{
			List<uint> res = new List<uint>(10);
			foreach (DataRow r in GetAll().Rows)
			{
				Articulo art = new Articulo(r);
				res.Add(art.IdArticulo);
			}
			return res;
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.Articulos, NombreClave, id);
		}

		public static Articulo GetSingleE(uint id)
		{
			return new Articulo(GetSingle(id).Rows[0]);
		}

		public static string GetDescXCodProd(int id)
		{
			DataTable dt = GetAll();
			string res = "";
			foreach (DataRow r in dt.Rows)
			{
				Articulo art = new Articulo(r);
				if (art.CodProducto == id)
					res = art.Descripcion;
			}
			return res;
		}

		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.Articulos, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdArticulo = res.IdInsercion;
			else
				return res;
			//
			// Insertar nueva densidad
			Densidad dens = new Densidad();
			dens.IdArticulo = IdArticulo;
			dens.Valor = 1;
			dens.Alta();
			//
			return res;
		}

		public override ABMResultado Modificacion()
		{
			if (IdArticulo == 0)
				return new ABMResultado(enErrores.LogicaInvalida, "No se puede modificar un ítem inexistente. Use ingresar. ");
			return DAL.Datos.ABM(DAL.enTablas.Articulos, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.Articulos, enABMFuncion.Baja, ProtocoloDatos());
			if (res.CodigoError != enErrores.Ninguno)
				return res;
			// Borrar su densidad
			Densidad dens = new Densidad();
			dens.IdArticulo = IdArticulo;
			dens.Baja();
			//
			return res;
		}

		#endregion

	}
}