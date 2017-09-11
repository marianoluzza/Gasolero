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
	public partial class ValeEntrega : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "ValeEntregas"; } }
		public static string NombreEntidad
		{ get { return "ValeEntrega"; } }
		public static string NombreClave
		{ get { return "IdValeEntrega"; } }
		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdValeEntrega;
		public System.UInt32 IdValeEntrega
		{
			get { return _IdValeEntrega; }
			set { _IdValeEntrega = value; }
		}

		System.UInt32 _IdArticulo;
		public System.UInt32 IdArticulo
		{
			get { return _IdArticulo; }
			set { _IdArticulo = value; }
		}

		System.Decimal _Cantidad;
		public System.Decimal Cantidad
		{
			get { return _Cantidad; }
			set { _Cantidad = value; }
		}

		System.UInt32 _Turno;
		/// <summary>
		/// Nro de turno
		/// </summary>
		public System.UInt32 Turno
		{
			get { return _Turno; }
			set { _Turno = value; }
		}

		System.UInt32 _IdParcial = 0;
		public System.UInt32 IdParcial
		{
			get { return _IdParcial; }
			set { _IdParcial = value; }
		}

		DateTime _Fecha;
		public DateTime Fecha
		{
			get { return _Fecha; }
			set { _Fecha = value; }
		}

		System.Decimal _Precio;
		public System.Decimal Precio
		{
			get { return _Precio; }
			set { _Precio = value; }
		}

		public List<Operacion> Operaciones
		{ get; set; }
		#endregion

		#region Constructores
		public ValeEntrega() { }

		public ValeEntrega(DataRow r)
		{
			IdValeEntrega = uint.Parse(r["IdValeEntrega"].ToString());
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			Cantidad = decimal.Parse(r["Cantidad"].ToString());
			Turno = uint.Parse(r["Turno"].ToString());
			IdParcial = uint.Parse(r["IdParcial"].ToString());
			Fecha = DateTime.Parse(r["Fecha"].ToString());
			Precio = decimal.Parse(r["Precio"].ToString());
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.ValeEntregas);
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.ValeEntregas);
		}

		public static DataTable GetSingle(uint id)
		{
			return Datos.GetSingle(enTablas.ValeEntregas, NombreClave, id);
		}

		public static DataTable GetForTurno(uint nroTurno)
		{
			return Datos.ValeEntregasGetForTurno(nroTurno);
		}

		public static DataTable GetForFechasArticulo(DateTime dde, DateTime hta, uint idArticulo)
		{
			return Datos.ValeEntregasGetForFechasIdArticulo(dde, hta, idArticulo);
		}

		public static ValeEntrega[] GetForParcial(uint turno, uint idParcial)
		{
			DataTable dt = Datos.ValeEntregasGetForParcial(turno, idParcial);
			ValeEntrega[] res = new ValeEntrega[dt.Rows.Count];
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				res[i] = new ValeEntrega(dt.Rows[i]);
			}
			return res;
		}

		public static ValeEntrega GetSingleE(uint id)
		{
			DataTable dt = GetSingle(id);
			if (dt.Rows.Count == 0)
				return null;
			else
				return new ValeEntrega(dt.Rows[0]);
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado abmRes = new ABMResultado( enErrores.Otro, "¡?");
			bool empeceTransa = Datos.InitTransa();
			bool huboError = true;
			try
			{
				abmRes = Datos.ABM(enTablas.ValeEntregas, enABMFuncion.Alta, this.ProtocoloDatos());
				if (abmRes.CodigoError == enErrores.Ninguno)
					IdValeEntrega = abmRes.IdInsercion;
#warning las operaciones (vales) se guardan en los cierres
				if (Operaciones != null)
				{
					foreach (Operacion op in Operaciones)
					{
						op.IdValeEntrega = IdValeEntrega;
						abmRes = op.Alta();
						if (abmRes.CodigoError != enErrores.Ninguno)
							break;
					}
				}
				if (abmRes.CodigoError == enErrores.Ninguno)
				huboError = false;
			}
			catch (Exception ex)
			{
				abmRes = new ABMResultado(enErrores.Otro, ex.Message);
			}
			finally
			{
				if (empeceTransa)
					Datos.FinTransa(!huboError);
			}
			return abmRes;
		}

		public override ABMResultado Modificacion()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.ValeEntregas, enABMFuncion.Modificacion, this.ProtocoloDatos());
			return abmRes;
		}

		public override ABMResultado Baja()
		{
			ABMResultado abmRes;
			abmRes = Datos.ABM(enTablas.ValeEntregas, enABMFuncion.Baja, this.ProtocoloDatos());
			return abmRes;
		}
		#endregion
	}
}