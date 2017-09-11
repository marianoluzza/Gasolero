using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using System.Data;
using MarUtils.Entidades;
using MarUtils.Soporte;

namespace Negocio
{
	public class Operacion : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Operaciones"; } }
		public static string NombreEntidad
		{ get { return "Operacion"; } }
		public static string NombreClave
		{ get { return "IdOperacion"; } }
		#endregion

		#region Consultas
		public static SortedList<int, decimal> GetValesPorProducto(DateTime dde, DateTime hta)
		{
			SortedList<int, decimal> res = new SortedList<int, decimal>(5);
			try
			{
				DataTable dt = DatosMdb.OperacionesGetForFechas(dde, hta);
				//DataTable dt = DatosMdb.OperacionesGetAll();
				//Fecha, CodProducto, Cantidad
				foreach (DataRow r in dt.Rows)
				{
					int cod = 0;
					try
					{
						cod = int.Parse(r["CodProducto"].ToString());
						if (res.ContainsKey(cod))
							res[cod] += decimal.Parse(r["Cantidad"].ToString());
						else
							res.Add(cod, decimal.Parse(r["Cantidad"].ToString()));
					}
					catch { }
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
			}
			return res;
		}

		public static SortedList<int, decimal> GetValesPorProducto(uint desde, uint hasta)
		{
			List<Operacion> ops = new List<Operacion>();
			return GetValesPorProducto(desde, hasta, out ops);
		}

		public static SortedList<int, decimal> GetValesPorProducto(uint desde, uint hasta, out List<Operacion> operaciones)
		{
			operaciones = new List<Operacion>();
			SortedList<int, decimal> res = new SortedList<int, decimal>(5);
			try
			{
				DataTable dt = DatosMdb.OperacionesGetParaImprimir(desde);
				foreach (DataRow r in dt.Rows)
				{
					int cod = 0;
					try
					{
						Operacion operacion = new Operacion(r);
						if (operacion.IdOperacion > hasta)
							break;
						operaciones.Add(operacion);
						cod = operacion.CodProducto;
						if (res.ContainsKey(cod))
							res[cod] += operacion.Cantidad;
						else
							res.Add(cod, operacion.Cantidad);
					}
					catch { }
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
			}
			return res;
		}

		public static DataTable GetVales(DateTime dde, DateTime hta)
		{
			return DatosMdb.OperacionesGetForFechas(dde, hta);
		}

		public static DataTable GetVales(uint idDesde, uint idHasta = uint.MaxValue)
		{
			return DatosMdb.OperacionesGetParaImprimir(idDesde, idHasta);
		}

		public Vehiculo GetConsumidor()
		{
			return Vehiculo.GetConsumidor(IdVehiculo);
		}
		#endregion


		public uint IdOperacion
		{
			get;
			set;
		}

		public DateTime Fecha
		{
			get;
			set;
		}

		public int CodProducto
		{
			get;
			set;
		}

		public decimal Cantidad
		{
			get;
			set;
		}

		public uint IdVehiculo
		{
			get;
			set;
		}

		public uint IdLlave
		{
			get;
			set;
		}

		public int Odometro
		{
			get;
			set;
		}

		public uint IdValeEntrega
		{
			get;
			set;
		}

		public Operacion(DataRow r)
		{
			IdOperacion = uint.Parse(r["IdOperacion"].ToString());
			Fecha = DateTime.Parse(r["Fecha"].ToString());
			CodProducto = int.Parse(r["CodProducto"].ToString());
			Cantidad = decimal.Parse(r["Cantidad"].ToString());
			IdVehiculo = uint.Parse(r["IdVehiculo"].ToString());
			IdLlave = uint.Parse(r["IdLlave"].ToString());
			if (r.Table.Columns.Contains("IdValeEntrega"))
				IdValeEntrega = uint.Parse(r["IdValeEntrega"].ToString());
			else
				IdValeEntrega = 1;
			Odometro = int.Parse(r["Odometro"].ToString());
		}

		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.Operaciones);
		}

		public static DataTable GeSingle(uint id)
		{
			return Datos.GetSingle(enTablas.Operaciones, NombreClave, id);
		}

		public static DataTable GetForFechas(DateTime dde, DateTime hta)
		{
			return Datos.OperacionesGetForFechas(dde, hta);
		}

		public override ABMResultado Alta()
		{
			ABMResultado abmRes = Datos.ABM(enTablas.Operaciones, enABMFuncion.Alta, ProtocoloDatos());
			return abmRes;
		}

		public override ABMResultado Modificacion()
		{
			return null;
		}

		public override ABMResultado Baja()
		{
			return null;
		}
	}
}
