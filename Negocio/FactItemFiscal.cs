using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class FactItemFiscal : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "FactItemFiscales"; } }
		public static string NombreEntidad
		{ get { return "FactItemFiscal"; } }
		public static string NombreClave
		{ get { return "IdFactItemFiscal"; } }

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

		System.UInt32 _IdFactEncabezado;
		public System.UInt32 IdFactEncabezado
		{
			get { return _IdFactEncabezado; }
			set { _IdFactEncabezado = value; }
		}

		System.UInt32 _IdFactItemFiscal;
		public System.UInt32 IdFactItemFiscal
		{
			get { return _IdFactItemFiscal; }
			set { _IdFactItemFiscal = value; }
		}

		System.Decimal _Monto;
		public System.Decimal Monto
		{
			get { return _Monto; }
			set { _Monto = decimal.Round(value, 3); }
		}

		System.Decimal _ImpInt;
		public System.Decimal ImpInt
		{
			get { return _ImpInt; }
			set { _ImpInt = decimal.Round(value, 3); }
		}

		System.Decimal _IVA;
		public System.Decimal IVA
		{
			get { return _IVA; }
			set { _IVA = decimal.Round(value, 3); }
		}

		string _Descripcion = "";
		public string Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value;}
		}

		string _ModoImpresion = "";
		public string ModoImpresion
		{
			get { return _ModoImpresion; }
			set { _ModoImpresion = value; }
		}

		/// <summary>
		/// % de descuento aplicado en el ítem
		/// </summary>
		public decimal Descuento { get; set; }
		#endregion

		#region Constructores
		public FactItemFiscal() { }

		public FactItemFiscal(DataRow r)
		{
			Cantidad = decimal.Parse(r["Cantidad"].ToString());
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			IdFactItemFiscal = uint.Parse(r["IdFactItemFiscal"].ToString());
			IdFactEncabezado = uint.Parse(r["IdFactEncabezado"].ToString());
			Monto = decimal.Parse(r["Monto"].ToString());
			ImpInt = decimal.Parse(r["ImpInt"].ToString());
			IVA = decimal.Parse(r["IVA"].ToString());
			Descripcion = r["Descripcion"].ToString();
			ModoImpresion = r["ModoImpresion"].ToString();
			Descuento = decimal.Parse(r["Descuento"].ToString());
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.FactItemFiscales);
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.FactItemFiscales);
		}

		public static DataTable GetListForFactEncabezado(uint idEncabezado)
		{
			return DAL.Datos.FactItemFiscalFiscalesGetListForIdFactEnca(idEncabezado);
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.FactItemFiscales, NombreClave, id);
		}

		public static FactItemFiscal GetSingleE(uint id)
		{
			return new FactItemFiscal(GetSingle(id).Rows[0]);
		}

		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.FactItemFiscales, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdFactItemFiscal = res.IdInsercion;
			return res;
		}

		public override ABMResultado Modificacion()
		{
			return DAL.Datos.ABM(DAL.enTablas.FactItemFiscales, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return DAL.Datos.ABM(DAL.enTablas.FactItemFiscales, enABMFuncion.Baja, ProtocoloDatos());
		}

		#endregion

	}
}