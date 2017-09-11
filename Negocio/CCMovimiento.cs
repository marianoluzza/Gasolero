using System;
using System.Collections.Generic;
using System.Data;
using MarUtils.Soporte;
using MarUtils.Entidades;
using DAL;

namespace Negocio
{
	public class CCMovimiento : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "CCMovimientos"; } }
		public static string NombreEntidad
		{ get { return "CCMovimiento"; } }
		public static string NombreClave
		{ get { return "IdCCMovimiento"; } }
		#endregion

		#region Atributos y Propiedades
		uint _IdCCMovimiento;
		public uint IdCCMovimiento
		{
			get
			{
				return _IdCCMovimiento;
			}
			set
			{
				_IdCCMovimiento = value;
			}
		}
		DateTime _Fecha;
		public DateTime Fecha
		{
			get
			{
				return _Fecha;
			}
			set
			{
				_Fecha = value;
			}
		}
		/// <summary>
		/// Positivo = saldo a favor del cliente - Negativo = extracción, venta
		/// </summary>
		public decimal Monto
		{
			get;
			set;
		}
		uint _IdCliente;
		public uint IdCliente
		{
			get
			{
				return _IdCliente;
			}
			set
			{
				_IdCliente = value;
			}
		}
		uint _IdVendedor;
		public uint IdVendedor
		{
			get
			{
				return _IdVendedor;
			}
			set
			{
				_IdVendedor = value;
			}
		}
		uint _IdFactEncabezado;
		public uint IdFactEncabezado
		{
			get
			{
				return _IdFactEncabezado;
			}
			set
			{
				_IdFactEncabezado = value;
			}
		}
		string _Descripcion;
		public string Descripcion
		{
			get
			{
				return _Descripcion;
			}
			set
			{
				_Descripcion = value;
			}
		}

		public string DNIAutorizado { get; set; }
		public string PatenteAutorizado { get; set; }
		public string NombreAutorizado { get; set; }

		public uint IdPagoForma { get; set; }
		#endregion Atributos y Propiedades

		#region Constructores
		public CCMovimiento() { }

		public CCMovimiento(DataRow r)
		{
			IdCCMovimiento = uint.Parse(r["IdCCMovimiento"].ToString());
			Fecha = DateTime.Parse(r["Fecha"].ToString());
			Monto = decimal.Parse(r["Monto"].ToString());
			IdCliente = uint.Parse(r["IdCliente"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			uint i = 0;
			if (!uint.TryParse(r["IdFactEncabezado"].ToString(), out i))
				i = 0;
			IdFactEncabezado = i;
			Descripcion = r["Descripcion"].ToString();
			DNIAutorizado = r["DNIAutorizado"].ToString();
			NombreAutorizado = r["NombreAutorizado"].ToString();
			PatenteAutorizado = r["PatenteAutorizado"].ToString();
			IdPagoForma = uint.Parse(r["IdPagoForma"].ToString());
		}

		public CCMovimiento(FactEncabezado f)
		{
			IdCCMovimiento = 0;
			Fecha = f.Fecha;
			Monto = -f.Total;//si es positivo es factura y es un monto en contra de la cc del cliente
			IdCliente = f.IdCliente;
			IdVendedor = f.IdVendedor;
			IdFactEncabezado = f.IdFactEncabezado;
			string s = f.Total < 0 ? "(NC) " : "Venta de ";
			if (f.Cuerpo.Count > 0)
				s += Articulo.GetSingleE(f.Cuerpo[0].IdArticulo).Descripcion;
			Descripcion = f.CCDescripcion + " - " + s;
			DNIAutorizado = "";
			NombreAutorizado = "";
			PatenteAutorizado = "";
			IdPagoForma = Reglas.PagoCC;
		}
		#endregion Constructores

		#region Métodos
		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.CCMovimientos);
		}

		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.CCMovimientos, "IdCCMovimiento", id);
		}

		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.CCMovimientos);
		}

		public static DataTable GetListForFechas(DateTime dde, DateTime hta)
		{
			return Datos.CCMovimientosGetListForFechas(dde, hta);
		}

		public static DataTable GetMovimientosDetallados(DateTime dde, DateTime hta)
		{
			return Datos.CCMovimientosGetListExtendidaForFechas(dde, hta);
		}

		public static DataTable GetMovimientos(DateTime dde, DateTime hta, uint idCliente = 0)
		{
			return Datos.CCMovimientosGetMovimientosForIdClienteFecha(idCliente, dde, hta);
		}

		public static DataTable GetListForCliente(uint idCliente)
		{
			return Datos.CCMovimientosGetListForIdCliente(idCliente);
		}

		public static decimal GetSaldoForCliente(uint idCliente, DateTime fechaHasta)
		{
			return Datos.CCMovimientosGetSaldoForIdClienteFecha(idCliente, fechaHasta);
		}

		public static DataTable GetResumen(DateTime fechaDesde, DateTime fechaHasta)
		{
			return Datos.CCMovimientosGetResumenForIdClienteFechas(fechaDesde, fechaHasta);
		}

		public static CCMovimiento GetSingleForFactura(uint idFactEncabezado)
		{
			DataTable dt = Datos.CCMovimientosGetSingleForIdFactEncabezado(idFactEncabezado);
			switch (dt.Rows.Count)
			{
				case 0:
					throw new Exception("La factura no tiene un movimiento en Cuentas Corrientes");
				case 1:
					return new CCMovimiento(dt.Rows[0]);
				default:
					throw new Exception("La factura tiene más de un movimiento en Cuentas Corrientes");
			}
		}

		public static CCMovimiento GetSingleE(uint id)
		{
			return new CCMovimiento(GetSingle(id).Rows[0]);
		}
		#endregion Métodos

		#region ABM
		public override ABMResultado Alta()
		{
			if (Monto > 0 && IdPagoForma == Reglas.PagoCC)//pago a cuenta en fp CC ¿? :/
				return new ABMResultado(enErrores.Cancelado, "No puede utilizar la forma de pago seleccionada.");
			if (IdPagoForma > 3 && !Reglas.VendedorActual.TienePermiso(enPermisos.Encargado))
				return new ABMResultado(enErrores.Cancelado, "No tiene permiso suficiente para la forma de pago seleccionada.");
			return Datos.ABM(enTablas.CCMovimientos, enABMFuncion.Alta, ProtocoloDatos());
		}

		public override ABMResultado Modificacion()
		{
			return Datos.ABM(enTablas.CCMovimientos, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return Datos.ABM(enTablas.CCMovimientos, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion ABM
	}
}