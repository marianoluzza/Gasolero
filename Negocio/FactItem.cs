using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class FactItem : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "FactItems"; } }
		public static string NombreEntidad
		{ get { return "FactItem"; } }
		public static string NombreClave
		{ get { return "IdFactItem"; } }

		#endregion

		#region Atributos y Propiedades
		System.Decimal _Cantidad;
		public System.Decimal Cantidad
		{
			get { return _Cantidad; }
			set { _Cantidad = decimal.Round(value, 3); }
		}

		System.UInt32 _IdArticulo;
		public System.UInt32 IdArticulo
		{
			get { return _IdArticulo; }
			set { _IdArticulo = value; }
		}

		System.UInt32 _IdFactItem;
		public System.UInt32 IdFactItem
		{
			get { return _IdFactItem; }
			set { _IdFactItem = value; }
		}

		System.Decimal _Monto;
		/// <summary>
		/// Monto total del item (cantidad x precio)
		/// </summary>
		public System.Decimal Monto
		{
			get { return _Monto; }
			set { _Monto = decimal.Round(value, 3); }
		}

		System.Decimal _Precio;
		/// <summary>
		/// Precio base del item, sin imp, x unidad
		/// </summary>
		public System.Decimal Precio
		{
			get { return _Precio; }
			set { _Precio = decimal.Round(value, 3); }
		}

		System.Decimal _Impuestos;
		/// <summary>
		/// Impuestos del item x unidad
		/// </summary>
		public System.Decimal Impuestos
		{
			get { return _Impuestos; }
			set { _Impuestos = decimal.Round(value, 3); }
		}

		System.Decimal _IVA;
		/// <summary>
		/// IVA del item x unidad
		/// </summary>
		public System.Decimal IVA
		{
			get { return _IVA; }
			set { _IVA = decimal.Round(value, 3); }
		}

		string _Descripcion = "";
		/// <summary>
		/// Desc del item
		/// </summary>
		public string Descripcion
		{
			get { return _Descripcion; }
			set 
			{
				if (IdArticulo == 1)
					_Descripcion = value;
			}
		}

		/// <summary>
		/// Desc del item
		/// </summary>
		public string DescPlana
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}

		/// <summary>
		/// % de descuento aplicado en el ítem
		/// </summary>
		public decimal Descuento { get; set; }
		#endregion

		#region Coleccion
		FactItemCollection _coleccion;
		/// <summary>
		/// 
		/// </summary>
		public FactItemCollection Coleccion
		{
			get { return _coleccion; }
			set { _coleccion = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public uint IdFactEncabezado
		{
			get { return Coleccion == null ? 0 : Coleccion.Encabezado.IdFactEncabezado; }
		}
		#endregion

		#region Constructores
		public FactItem() { }

		public FactItem(DataRow r)
		{
			Cantidad = decimal.Parse(r["Cantidad"].ToString());
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			IdFactItem = uint.Parse(r["IdFactItem"].ToString());
			Monto = decimal.Parse(r["Monto"].ToString());
			Precio = decimal.Parse(r["Precio"].ToString());
			Impuestos = decimal.Parse(r["Impuestos"].ToString());
			IVA = decimal.Parse(r["IVA"].ToString());
			DescPlana = r["Descripcion"].ToString();
			Descuento = decimal.Parse(r["Descuento"].ToString());
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.FactItems);
		}

		public FactItem Clonar()
		{
			FactItem res = new FactItem();
			res._Cantidad = _Cantidad;
			res._Descripcion = _Descripcion;
			res._IdArticulo = _IdArticulo;
			res._IdFactItem = _IdFactItem;
			res._Impuestos = _Impuestos;
			res._IVA = _IVA;
			res._Monto = _Monto;
			res._Precio = _Precio;
			res.Descuento = Descuento;
			return res;
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.FactItems);
		}

		public static void GetListForFactEncabezado(FactEncabezado enca)
		{
			enca.Cuerpo.Clear();
			DataTable dtFilas = DAL.Datos.FactItemsGetListForIdFactEnca(enca.IdFactEncabezado);
			foreach (DataRow r in dtFilas.Rows)
			{
				enca.Cuerpo.Add(new FactItem(r));
			}
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.FactItems, NombreClave, id);
		}

		public static FactItem GetSingleE(uint id)
		{
			return new FactItem(GetSingle(id).Rows[0]);
		}

		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.FactItems, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdFactItem = res.IdInsercion;
			return res;
		}

		public override ABMResultado Modificacion()
		{
			return DAL.Datos.ABM(DAL.enTablas.FactItems, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return DAL.Datos.ABM(DAL.enTablas.FactItems, enABMFuncion.Baja, ProtocoloDatos());
		}

		#endregion

	}
}