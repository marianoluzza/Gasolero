using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MarUtils.Datos;
using System.Data.OleDb;
using System.IO;
using MarUtils.Soporte;

namespace DAL
{
	public static class DatosMdb
	{
		static BDAdmin _admin;

		public static void Init(string mdb)
		{
			//string sConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdb + ";Jet OLEDB:Database Password=guijulbru;";//No funciona en x64
			string sConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdb + ";Jet OLEDB:Database Password=guijulbru;";
			_admin = new BDAdmin(new OleDbConnection(sConn));
		}

		public static DataTable ProductosGetAll()
		{
			OleDbCommand comm = _admin.GetComando("SELECT CodProducto, DesProducto FROM Productos") as OleDbCommand;
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "Productos";
			return dt;
		}

		public static DataTable OperacionesGetAll()
		{
			OleDbCommand comm = _admin.GetComando("SELECT IdOperacion, IdConsumidor as IdVehiculo, IDLlave, Odometro, Fecha, CodProducto, Cantidad FROM Operaciones") as OleDbCommand;
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "Operaciones";
			return dt;
		}

		public static DataTable OperacionesGetForFechas(DateTime dde, DateTime hta)
		{
			OleDbCommand comm = _admin.GetComando("SELECT IdOperacion, IdConsumidor as IdVehiculo, IDLlave, Odometro, Fecha, CodProducto, Cantidad FROM Operaciones WHERE Fecha >= @Desde AND Fecha <= @Hasta") as OleDbCommand;
			OleDbParameter p = new OleDbParameter("Desde", dde.ToString("yyyy/MM/dd HH:mm"));
			p.DbType = DbType.DateTime;
			comm.Parameters.Add(p);
			p = new OleDbParameter("Hasta", hta.ToString("yyyy/MM/dd HH:mm"));
			p.DbType = DbType.DateTime;
			comm.Parameters.Add(p);
			//comm.Parameters.AddWithValue("Desde", dde);
			//comm.Parameters.AddWithValue("Hasta", hta);
			DataTable dt = _admin.ExecuteReader(comm);
			dt.PrimaryKey = new DataColumn[] {dt.Columns[0]};
			dt.TableName = "Operaciones";
			return dt;
		}

		/// <summary>
		/// Devuelve las operaciones desde el id dado inclusive hasta el id dado inclusive
		/// </summary>
		/// <param name="idDesde"></param>
		/// <param name="idHasta">opcional</param>
		/// <returns></returns>
		public static DataTable OperacionesGetParaImprimir(uint idDesde, uint idHasta = uint.MaxValue)
		{
			string q = "SELECT IdOperacion, IdConsumidor as IdVehiculo, IDLlave, Odometro, Fecha, CodProducto, Cantidad FROM Operaciones WHERE IdOperacion >= @Desde ";
			if (idHasta < uint.MaxValue)
				q += "AND IdOperacion <= @Hasta ";
			q += "ORDER BY IdOperacion ASC";
			OleDbCommand comm = _admin.GetComando(q) as OleDbCommand;
			comm.Parameters.AddWithValue("Desde", idDesde);
			if (idHasta < uint.MaxValue)
				comm.Parameters.AddWithValue("Hasta", idHasta);
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "Operaciones";
			return dt;
		}

		public static DataTable VehiculosGetAll()
		{
			OleDbCommand comm = _admin.GetComando("SELECT IdVehiculo, Descripcion, IDinterna FROM Vehiculos") as OleDbCommand;
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "Vehiculos";
			return dt;
		}

		public static DataTable VehiculosGetSingle(uint idVehiculo)
		{
			OleDbCommand comm = _admin.GetComando("SELECT IdVehiculo, Descripcion, IDinterna FROM Vehiculos WHERE IdVehiculo = @idVehiculo") as OleDbCommand;
			comm.Parameters.AddWithValue("idVehiculo", idVehiculo);
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "Vehiculos";
			return dt;
		}
	}
}
