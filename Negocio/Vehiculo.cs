using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using System.Data;
using MarUtils.Entidades;
using MarUtils.Soporte;

namespace Negocio
{
	public class Vehiculo : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Vehiculos"; } }
		public static string NombreEntidad
		{ get { return "Vehiculo"; } }
		public static string NombreClave
		{ get { return "IdVehiculo"; } }
		#endregion

		public static DataTable GetConsumidores()
		{
			return DatosMdb.VehiculosGetAll();
		}

		public static Vehiculo GetConsumidor(uint id)
		{
			//IdVehiculo, Descripcion, IDinterna
			DataTable dt = DatosMdb.VehiculosGetSingle(id);
			Vehiculo res = null;
			try
			{
				res = new Vehiculo(dt.Rows[0]);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
			}
			return res;
		}

		public Vehiculo GetConsumidor()
		{
			return Vehiculo.GetConsumidor(IdVehiculo);
		}

		public uint IdVehiculo
		{
			get;
			set;
		}

		public string Descripcion
		{
			get;
			set;
		}

		public string IdInterna
		{
			get;
			set;
		}

		public Vehiculo(DataRow r)
		{//IdVehiculo, IdVehiculo as IdConsumidor, Descripcion, IDinterna
			if (r.Table.Columns.Contains("IdConsumidor"))
				IdVehiculo = uint.Parse(r["IdConsumidor"].ToString());
			else
				IdVehiculo = uint.Parse(r["IdVehiculo"].ToString());
			Descripcion = r["Descripcion"].ToString();
			IdInterna = r["IDinterna"].ToString();
		}

		public static DataTable Esquema()
		{
			return Datos.Esquema(enTablas.Vehiculos);
		}

		public static DataTable GetAll()
		{
			return Datos.GetAll(enTablas.Vehiculos);
		}

		public static ABMResultado FixVehiculos()
		{
			ABMResultado abmRes = new ABMResultado( enErrores.Otro, "¿?");
			int ctveh = 0;
			Datos.InitTransa();
			try
			{
				DataTable dt = GetConsumidores();
				foreach (DataRow r in dt.Rows)
				{
					Vehiculo v = new Vehiculo(r);
					abmRes = v.Alta();
					if (abmRes.CodigoError != enErrores.Ninguno)
						break;
					ctveh++;
				}
				//dt = Operacion.GetVales(new DateTime(2000, 1, 1), DateTime.Now.AddDays(30));
				//foreach (DataRow r in dt.Rows)
				//{
				//    Operacion o = new Operacion(r);
				//    abmRes = o.Alta();
				//    if (abmRes.CodigoError != enErrores.Ninguno)
				//        break;
				//}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				abmRes = new ABMResultado(enErrores.Otro, ex.Message);
			}
			Datos.FinTransa(abmRes.CodigoError == enErrores.Ninguno);
			if (abmRes.CodigoError == enErrores.Ninguno)
				abmRes.FilasAfectadas = ctveh;
			return abmRes;
		}

		public ABMResultado AltaSegura()
		{
			if (Datos.GetSingle(enTablas.Vehiculos, NombreClave, IdVehiculo).Rows.Count == 0)
				return Alta();
			else
				return new ABMResultado(0);
		}

		public override ABMResultado Alta()
		{
			ABMResultado abmRes = Datos.ABM(enTablas.Vehiculos, enABMFuncion.Alta, ProtocoloDatos());
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
