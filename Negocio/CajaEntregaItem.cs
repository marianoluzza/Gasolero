using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class CajaEntregaItem : EntidadBase, IItem<CajaEntrega>
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "CajaEntregaItems"; } }
		public static string NombreEntidad
		{ get { return "CajaEntregaItem"; } }
		public static string NombreClave
		{ get { return "IdCajaentregaitem"; } }
		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdCajaEntregaItem;
		public System.UInt32 IdCajaEntregaItem
		{
			get { return _IdCajaEntregaItem; }
			set { _IdCajaEntregaItem = value; }
		}

		System.UInt32 _IdPagoForma;
		public System.UInt32 IdPagoForma
		{
			get { return _IdPagoForma; }
			set { _IdPagoForma = value; }
		}

		System.Decimal _Monto;
		public System.Decimal Monto
		{
			get { return _Monto; }
			set { _Monto = value; }
		}

		System.UInt32 _IdCajaEntrega;
		public System.UInt32 IdCajaEntrega
		{
			get { return _IdCajaEntrega; }
			set { _IdCajaEntrega = value; }
		}
		#endregion

		#region Constructores
		public CajaEntregaItem() { }

		public CajaEntregaItem(DataRow r)
		{
			 IdCajaEntregaItem = uint.Parse(r["IdCajaEntregaItem"].ToString());
			 IdPagoForma = uint.Parse(r["IdPagoForma"].ToString());
			 Monto = decimal.Parse(r["Monto"].ToString());
			 IdCajaEntrega = uint.Parse(r["IdCajaEntrega"].ToString());
		}

		#endregion

		#region Metodos
		public static void LlenarColeccion(CajaEntrega enca)
		{
			DataTable dtFilas = GetList(enca.IdCajaEntrega);
			foreach (DataRow r in dtFilas.Rows)
			{
				enca.Coleccion.Add(new CajaEntregaItem(r));
			}
		}

		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.CajaEntregaItems);
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.CajaEntregaItems);
		}

		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.CajaEntregaItems, NombreClave, id);
		}

		public static DataTable GetList(uint idCajaEntrega)
		{
			return Datos.CajaEntregaItemsGetListForIdCajaEntrega(idCajaEntrega);
		}

		public static CajaEntregaItem GetSingleE(uint id)
		{
			DataTable dt = GetSingle(id);
			if (dt.Rows.Count == 0)
				return null;
			else
				return new CajaEntregaItem(dt.Rows[0]);
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado abmRes;
			if (Monto == 0)
				return new ABMResultado(0);
			abmRes = Datos.ABM(enTablas.CajaEntregaItems, enABMFuncion.Alta, this.ProtocoloDatos());
			if (abmRes.CodigoError == enErrores.Ninguno)
				IdCajaEntregaItem = abmRes.IdInsercion;
			return abmRes;
		}

		public override ABMResultado Modificacion()
		{
			ABMResultado abmRes;
			if (Monto == 0)
				return Baja();
			abmRes = Datos.ABM(enTablas.CajaEntregaItems, enABMFuncion.Modificacion, this.ProtocoloDatos());
			return abmRes;
		}

		public override ABMResultado Baja()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.CajaEntregaItems, enABMFuncion.Baja, this.ProtocoloDatos());
			return abmRes;
		}
		#endregion

		#region IItem<CajaEntrega> Members
		CajaEntrega _enca = null;
		public CajaEntrega Coleccion
		{
			get
			{
				return _enca;
			}
			set
			{
				_enca = value;
			}
		}

		#endregion
	}
}