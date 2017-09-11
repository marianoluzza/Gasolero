using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class CajaEntrega : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "CajaEntregas"; } }
		public static string NombreEntidad
		{ get { return "CajaEntrega"; } }
		public static string NombreClave
		{ get { return "IdCajaEntrega"; } }
		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdCajaEntrega;
		public System.UInt32 IdCajaEntrega
		{
			get { return _IdCajaEntrega; }
			set { _IdCajaEntrega = value; }
		}

		System.UInt32 _IdVendedor = 0;
		public System.UInt32 IdVendedor
		{
			get { return _IdVendedor; }
			set { _IdVendedor = value; }
		}

		System.DateTime _Fecha;
		public System.DateTime Fecha
		{
			get { return _Fecha; }
			set { _Fecha = value; }
		}

		System.UInt32 _IdTurno;
		public System.UInt32 IdTurno
		{
			get { return _IdTurno; }
			set { _IdTurno = value; }
		}

		#endregion

		#region Coleccion
		CajaEntregaItemCollection _coleccion;
		public CajaEntregaItemCollection Coleccion
		{
			get { return _coleccion; }
			set { _coleccion = value; }
		}
		#endregion

		#region Constructores
		public CajaEntrega()
		{
			_coleccion = new CajaEntregaItemCollection(this);
		}

		public CajaEntrega(DataRow r)
		{
			_coleccion = new CajaEntregaItemCollection(this);
			IdCajaEntrega = uint.Parse(r["IdCajaEntrega"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			Fecha = DateTime.Parse(r["Fecha"].ToString());
			IdTurno = uint.Parse(r["IdTurno"].ToString());
		}

		#endregion

		#region Metodos
		public void GetFilas()
		{
			CajaEntregaItem.LlenarColeccion(this);
		}

		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.CajaEntregas);
		}

		public void AddFila(uint idPagoForma, decimal monto)
		{
			CajaEntregaItem i = new CajaEntregaItem();
			i.IdPagoForma = idPagoForma;
			i.Monto = monto;
			Coleccion.Add(i);
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.CajaEntregas);
		}

		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.CajaEntregas, NombreClave, id);
		}

		public static CajaEntrega GetSingleForTurno(uint idTurno)
		{
			DataTable dt = DAL.Datos.CajaEntregaGetSingleForIdTurno(idTurno);
			if (dt.Rows.Count == 0)
				return null;
			else
				return new CajaEntrega(dt.Rows[0]);
		}

		public static CajaEntrega GetSingleE(uint id)
		{
			DataTable dt = GetSingle(id);
			if (dt.Rows.Count == 0)
				return null;
			else
				return new CajaEntrega(dt.Rows[0]);
		}

		bool ChequearTotal()
		{
			decimal totalEntrega = 0;
			foreach (CajaEntregaItem it in Coleccion)
			{
				PagoForma pf = PagoForma.GetSingleE(it.IdPagoForma);
				if (!pf.SumaAlTotal)
					continue;
				totalEntrega += it.Monto;
			}
			Turno t = Turno.GetSingleE(this.IdTurno);
			decimal totalPlanilla = 0;
			foreach (Puesto p in Reglas.Puestos)
			{
				foreach (Cierre c in Cierre.GetListaXSurtidorTurno(p.IdPuesto, t.Numero))
				{
					totalPlanilla += c.GetDiferencia() * c.Costo;
				}
			}
			totalEntrega = Decimal.Round(totalEntrega, 2);
			totalPlanilla = Decimal.Round(totalPlanilla, 2);
			if (totalEntrega != totalPlanilla)
				return Reglas.Confirmar("Hay diferencia entre la entrega de caja y el cierre del turno:" + Environment.NewLine +
				Environment.NewLine + "Entrega: $" + totalEntrega.ToString("0.00") +
				Environment.NewLine + "Cierre: $" + totalPlanilla.ToString("0.00") + Environment.NewLine +
				Environment.NewLine + "¿Desea realizar la entrega de todas formas?");
			else
				return true;
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			bool empeceTransa = Datos.InitTransa();
			bool exito = false;
			ABMResultado abmRes, abmResEnca;
			try
			{
				if (IdVendedor == 0)
					IdVendedor = Reglas.VendedorActual.IdVendedor;
				Fecha = Reglas.Now;
				//if (!ChequearTotal())//chequea lo entregado vs. el cierre de surtidores
				//    return new ABMResultado(enErrores.Cancelado, "Operación Cancelada");
				if (!empeceTransa)//la transaccion NO tendría que estar abierta antes
					return new ABMResultado(enErrores.LogicaInvalida, "La transacción fue empezada antes de lo debido");
				IdTurno = Reglas.TurnoUltimo.IdTurno;//se rinde el último
				IdVendedor = Reglas.VendedorActual.IdVendedor;
				abmRes = Datos.ABM(enTablas.CajaEntregas, enABMFuncion.Alta, this.ProtocoloDatos());
				if (abmRes.CodigoError == enErrores.Ninguno)
					IdCajaEntrega = abmRes.IdInsercion;
				else
					return abmRes;
				abmResEnca = abmRes;
				foreach (CajaEntregaItem it in Coleccion)
				{
					it.IdCajaEntrega = IdCajaEntrega;
					abmRes = it.Alta();
					if (abmRes.CodigoError != enErrores.Ninguno)
						return abmRes;
				}
				exito = true;
				return abmResEnca;
			}
			catch (Exception ex)
			{
				exito = false;
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				Datos.FinTransa(exito);
			}
		}

		public override ABMResultado Modificacion()
		{
			bool empeceTransa = Datos.InitTransa();
			bool exito = false;
			ABMResultado abmRes, abmResEnca;
			try
			{
				//if (!ChequearTotal())
				//    return new ABMResultado(enErrores.Cancelado, "Operación Cancelada");
				if (!empeceTransa)//la transaccion NO tendría que estar abierta antes
					return new ABMResultado(enErrores.LogicaInvalida, "La transacción fue empezada antes de lo debido");
				abmRes = Datos.ABM(enTablas.CajaEntregas, enABMFuncion.Modificacion, this.ProtocoloDatos());
				if (abmRes.CodigoError != enErrores.Ninguno)
					return abmRes;
				abmResEnca = abmRes;
				//
				foreach (CajaEntregaItem it in Coleccion)
				{
					it.IdCajaEntrega = IdCajaEntrega;
					abmRes = it.IdCajaEntregaItem == 0 ? it.Alta() : it.Modificacion();
					if (abmRes.CodigoError != enErrores.Ninguno)
						return abmRes;
				}
				exito = true;
				return abmResEnca;
			}
			catch (Exception ex)
			{
				exito = false;
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				Datos.FinTransa(exito);
			}
		}

		public override ABMResultado Baja()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.CajaEntregas, enABMFuncion.Baja, this.ProtocoloDatos());
			return abmRes;
		}
		#endregion
	}
}