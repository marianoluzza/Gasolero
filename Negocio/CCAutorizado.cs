using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public class CCAutorizado : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "CCAutorizados"; } }
		public static string NombreEntidad
		{ get { return "CCAutorizado"; } }
		public static string NombreClave
		{ get { return "IdCCAutorizado"; } }
		#endregion

		#region Propiedades
		public uint IdCCAutorizado { get; set; }
		public uint IdCliente { get; set; }
		public string Nombre { get; set; }
		public string Patente { get; set; }
		public string DNI { get; set; }
		#endregion Propiedades

		#region Constructores
		public CCAutorizado() { }

		public CCAutorizado(DataRow r)
		{
			IdCCAutorizado = uint.Parse(r["IdCCAutorizado"].ToString());
			IdCliente = uint.Parse(r["IdCliente"].ToString());
			Nombre = r["Nombre"].ToString();
			Patente = r["Patente"].ToString();
			DNI = r["DNI"].ToString();
		}
		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.CCAutorizados);
		}

		public static bool EsAutorizado(uint idCliente, string patente, string dni)
		{
			CCAutorizado cca = new CCAutorizado();
			cca.IdCliente = idCliente;
			cca.Patente = patente;
			cca.DNI = dni;
			return cca.EsAturizado();
		}

		public bool EsAturizado()
		{
			var x = DNIAutorizados(this.IdCliente);
			return x.ContainsKey(Hash);
		}

		public static string GetNombre(uint idCliente, string patente, string dni)
		{
			CCAutorizado cca = new CCAutorizado();
			cca.IdCliente = idCliente;
			cca.Patente = patente;
			cca.DNI = dni;
			var x = DNIAutorizados(idCliente);
			var res = x[cca.Hash];
			return res.Nombre;
		}

		string Hash
		{
			get
			{
				return DNI.ToUpper() + "&" + Patente.ToUpper() + "&" + IdCliente.ToString();
			}
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			DataTable dt = DAL.Datos.GetAll(DAL.enTablas.CCAutorizados);
			dt.TableName = NombreTabla;
			dt.PrimaryKey = new DataColumn[] { dt.Columns[NombreClave] };
			return dt;
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.CCAutorizados, NombreClave, id);
		}

		public static Turno GetSingleE(uint id)
		{
			return new Turno(GetSingle(id).Rows[0]);
		}

		public static DataTable GetForIdCliente(uint idCliente)
		{
			return DAL.Datos.CCAutorizadosGetListForIdCliente(idCliente);
		}

		public static SortedList<string, CCAutorizado> DNIAutorizados(uint idCliente)
		{
			SortedList<string, CCAutorizado> res = new SortedList<string, CCAutorizado>();
			foreach (DataRow r in GetForIdCliente(idCliente).Rows)
			{
				var a = new CCAutorizado(r);
				res.Add(a.Hash, a);
			}
			return res;
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			//Limpieza
			DNI = DNI.Trim();
			Nombre = Nombre.Trim();
			Patente = Patente.Trim().ToUpper();
			//
			try
			{
				return Datos.ABM(enTablas.CCAutorizados, enABMFuncion.Alta, ProtocoloDatos());
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.ErrorBD, ex.Message);
			}
		}

		public override ABMResultado Modificacion()
		{
			//Limpieza
			DNI = DNI.Trim();
			Nombre = Nombre.Trim();
			Patente = Patente.Trim().ToUpper();
			//
			try
			{
				return Datos.ABM(enTablas.CCAutorizados, enABMFuncion.Modificacion, ProtocoloDatos());
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.ErrorBD, ex.Message);
			}
		}

		public override ABMResultado Baja()
		{
			return Datos.ABM(enTablas.CCAutorizados, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion ABM
	}
}
