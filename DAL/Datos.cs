using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MarUtils.Datos;
using MySql.Data.MySqlClient;
using System.IO;
using MarUtils.Soporte;

namespace DAL
{
	public class Datos
	{
		static BDAdmin _admin;

		public static void Init()
		{
			string sConn = File.ReadAllText("conn.ini");
			_admin = new BDAdmin(new MySqlConnection(sConn));
		}

		public static bool InitTransa()
		{
			return _admin.InicioTransaccion();
		}

		public static void FinTransa(bool exito)
		{
			_admin.FinTransaccion(exito);
		}

		#region Consultas Genericas
		public static DataTable GetAll(enTablas tabla)
		{
			DataTable dt = _admin.ExecuteReader(_admin.GetComando(tabla.ToString() + "GetAll"));
			return dt;
		}

		public static DataTable GetSingle(enTablas tabla, string clave, uint id)
		{
			MySqlCommand comm = _admin.GetComando(tabla.ToString() + "GetSingleForId") as MySqlCommand;
			comm.Parameters.AddWithValue("_" + clave, id);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static ABMResultado ABM(enTablas tabla, enABMFuncion func, PDatosFull param)
		{
			MySqlCommand comm;
			string sFunc = "";
			switch (func)
			{
				case enABMFuncion.Alta:
					sFunc = "InsertSingle";
					break;
				case enABMFuncion.Baja:
					sFunc = "DeleteSingle";
					break;
				case enABMFuncion.Modificacion:
					sFunc = "UpdateSingle";
					break;
			}
			comm = _admin.GetComando(tabla.ToString() + sFunc) as MySqlCommand;
			CargaParametros(comm, param);
			ABMResultado res = _admin.ExecuteNonQuery(comm);
			if (func == enABMFuncion.Alta && res.CodigoError == enErrores.Ninguno)
			{
				res.IdInsercion = uint.Parse(comm.Parameters["_" + param.NombreClave].Value.ToString());
			}
			return res;
		}

		public static DataTable Esquema(enTablas tabla)
		{
			MySqlCommand comm = _admin.GetComando("EsquemasGetListForTabla") as MySqlCommand;
			comm.Parameters.AddWithValue("_TableName", tabla.ToString());
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DateTime Now()
		{
			MySqlCommand comm = _admin.GetComando("select now()") as MySqlCommand;
			DateTime res = DateTime.Parse(_admin.ExecuteReader<string>(comm));
			return res;
		}

		public static string SettingsGet(string nbre)
		{
			MySqlCommand comm = _admin.GetComando("SettingsGetSingleForNombre") as MySqlCommand;
			comm.Parameters.AddWithValue("_Nombre", nbre);
			string res = _admin.ExecuteReader<string>(comm);
			return res;
		}

		public static ABMResultado SettingsSet(string nbre, string valor)
		{
			MySqlCommand comm = _admin.GetComando("SettingsUpdateSingle") as MySqlCommand;
			comm.Parameters.AddWithValue("_Nombre", nbre);
			comm.Parameters.AddWithValue("_Valor", valor);
			ABMResultado res = _admin.ExecuteNonQuery(comm);
			return res;
		}
		#endregion

		#region Consultas Especificas
		public static DataTable CajaEntregaGetSingleForIdTurno(uint idTurno)
		{
			MySqlCommand comm = _admin.GetComando("cajaEntregasGetSingleForIdTurno") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdTurno", idTurno);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CajaEntregaItemsGetListForIdCajaEntrega(uint IdCajaEntrega)
		{
			MySqlCommand comm = _admin.GetComando("cajaentregaitemsGetListForIdCajaEntrega") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdCajaEntrega", IdCajaEntrega);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CCAutorizadosGetListForIdCliente(uint IdCliente)
		{
			MySqlCommand comm = _admin.GetComando("CCAutorizadosGetListForIdCliente") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdCliente", IdCliente);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CCMovimientosGetSingleForIdFactEncabezado(uint IdFactEncabezado)
		{
			MySqlCommand comm = _admin.GetComando("CCMovimientosGetSingleForIdFactEncabezado") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdFactEncabezado", IdFactEncabezado);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CCMovimientosGetListForIdCliente(uint IdCliente)
		{
			MySqlCommand comm = _admin.GetComando("CCMovimientosGetListForIdCliente") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdCliente", IdCliente);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CCMovimientosGetListExtendidaForFechas(DateTime Desde, DateTime Hasta)
		{
			MySqlCommand comm = _admin.GetComando("CCMovimientosGetListExtendidaForFecha") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(Desde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(Hasta));
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "CCMovimientos";
			dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
			return dt;
		}

		public static decimal CCMovimientosGetSaldoForIdClienteFecha(uint IdCliente, DateTime Hasta)
		{
			MySqlCommand comm = _admin.GetComando("CCMovimientosGetSaldoForIdClienteFecha") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdCliente", IdCliente);
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(Hasta));
			decimal res = _admin.ExecuteReader<decimal>(comm);
			return res;
		}

		public static DataTable CCMovimientosGetResumenForIdClienteFechas(DateTime Desde, DateTime Hasta, uint IdCliente = 0)
		{
			MySqlCommand comm = _admin.GetComando("CCMovimientosGetResumenForIdClienteFechas") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdCliente", IdCliente);
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(Desde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(Hasta));
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "CCResumen";
			//dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
			return dt;
		}

		public static DataTable CCMovimientosGetMovimientosForIdClienteFecha(uint IdCliente, DateTime Desde, DateTime Hasta)
		{
			MySqlCommand comm = _admin.GetComando("CCMovimientosGetMovimientosForIdClienteFecha") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdCliente", IdCliente);
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(Desde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(Hasta));
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "CCMovimientos";
			dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
			return dt;
		}

		public static DataTable CCMovimientosGetListForFechas(DateTime Desde, DateTime Hasta)
		{
			MySqlCommand comm = _admin.GetComando("CCMovimientosGetListForFechas") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(Desde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(Hasta));
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CierresGetUltimo(uint surtidor)
		{
			MySqlCommand comm = _admin.GetComando("CierresGetUltimoBySurtidor") as MySqlCommand;
			comm.Parameters.AddWithValue("_Surtidor", surtidor);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CierresGetListForIdParcial(uint turno, uint idParcial)
		{
			MySqlCommand comm = _admin.GetComando("cierresGetListForIdParcial") as MySqlCommand;
			comm.Parameters.AddWithValue("_Turno", turno); 
			comm.Parameters.AddWithValue("_IdParcial", idParcial);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CierresGetListForSurtidorTurno(uint surtidor, uint turno)
		{
			MySqlCommand comm = _admin.GetComando("CierresGetListForSurtidorTurno") as MySqlCommand;
			comm.Parameters.AddWithValue("_Surtidor", surtidor);
			comm.Parameters.AddWithValue("_Turno", turno);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable CierresGetListForIdArticuloTurno(uint idArticulo, uint turno)
		{
			MySqlCommand comm = _admin.GetComando("cierresGetListForIdArticuloTurno") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdArticulo", idArticulo);
			comm.Parameters.AddWithValue("_Turno", turno);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable ClientesGetListForCuit(string cuit)
		{
			MySqlCommand comm = _admin.GetComando("ClientesGetListForCUIT") as MySqlCommand;
			comm.Parameters.AddWithValue("_CUIT", cuit);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable ClientesGetListForNombre(string nbre)
		{
			MySqlCommand comm = _admin.GetComando("ClientesGetListForNombre") as MySqlCommand;
			comm.Parameters.AddWithValue("_Nombre", nbre);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		static string _puesto = "";
		public static string FactPuestoDeVenta
		{
			get { return _puesto; }
			set { _puesto = value; }
		}

		public static string FactEncabezadosGetNumero()
		{
			MySqlCommand comm = _admin.GetComando("FactEncabezadosGetNumero") as MySqlCommand;
			string res = _admin.ExecuteReader<string>(comm);
			uint num = 0;
			//string pto = "";
			if (res.Contains("-"))
			{
				string[] partes = res.Split('-');
				//pto = partes[0] + "-";
				uint.TryParse(partes[1], out num);
			}
			else
				uint.TryParse(res, out num);
			num++;
			//return pto + num.ToString().PadLeft(8, '0');
			return FactPuestoDeVenta + "-" + num.ToString().PadLeft(8, '0');
		}

		public static decimal FactEncabezadosGetTotalMaximo()
		{
			MySqlCommand comm = _admin.GetComando("SettingsGetSingleForNombre") as MySqlCommand;
			comm.Parameters.AddWithValue("_Nombre", "TotalMaximo");
			string res = _admin.ExecuteReader<string>(comm);
			decimal num = (res == null ? 0 : decimal.Parse(res));
			return num;
		}

		public static ABMResultado FactEncabezadosSetTotalMaximo(decimal valor)
		{
			MySqlCommand comm = _admin.GetComando("SettingsUpdateSingle") as MySqlCommand;
			comm.Parameters.AddWithValue("_Nombre", "TotalMaximo");
			comm.Parameters.AddWithValue("_Valor", valor.ToString());
			ABMResultado res = _admin.ExecuteNonQuery(comm);
			return res;
		}

		public static DataTable FactEncabezadosGetForFechas(DateTime dde, DateTime hta, uint idCliente)
		{
			MySqlCommand comm = _admin.GetComando("FactEncabezadosTotalizadosGetForFechasIdCliente") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(hta));
			comm.Parameters.AddWithValue("_IdCliente", idCliente);
			DataTable dt = _admin.ExecuteReader(comm);
			dt.PrimaryKey = new DataColumn[] { dt.Columns["IdFactEncabezado"] };
			return dt;
		}

		public static DataTable FactEncabezadosGetForFechas(DateTime dde, DateTime hta, uint idCliente, uint idArticulo)
		{
			MySqlCommand comm = _admin.GetComando("FactEncabezadosTotalizadosGetForFechasIdClienteIdArticulo") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(hta));
			comm.Parameters.AddWithValue("_IdCliente", idCliente);
			comm.Parameters.AddWithValue("_IdArticulo", idArticulo);
			DataTable dt = _admin.ExecuteReader(comm);
			dt.PrimaryKey = new DataColumn[] { dt.Columns["IdFactEncabezado"] };
			return dt;
		}

		public static DataTable FactEncabezadosGetTotalxArticuloForFechas(DateTime dde, DateTime hta, uint idArticulo)
		{
			MySqlCommand comm = _admin.GetComando("FactEncabezadosTotalizadosGetForFechasIdArticulo") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(hta));
			comm.Parameters.AddWithValue("_IdArticulo", idArticulo);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static decimal FactEncabezadosGeVentatTotalForFechasIdPagoForma(DateTime dde, DateTime hta, uint idPagoForma)
		{
			MySqlCommand comm = _admin.GetComando("FactEncabezadosVentaTotalGetForFechasIdPagoForma") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdPagoForma", idPagoForma);
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(hta));
			string res = _admin.ExecuteReader<string>(comm);
			decimal num = (res == null ? 0 : decimal.Parse(res));
			return num;
		}

		public static DataTable FactEncabezadosTotalizadosConDtoGetForFechas(DateTime dde, DateTime hta)
		{
			MySqlCommand comm = _admin.GetComando("FactEncabezadosTotalizadosConDtoGetForFechas") as MySqlCommand;
			//comm.Parameters.AddWithValue("_IdPagoForma", idPagoForma);
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(hta));
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "FactEncabezados";
			return dt;
		}

		public static DataTable FactItemsGetListForIdFactEnca(uint idFactEncabezado)
		{
			MySqlCommand comm = _admin.GetComando("FactItemsGetListForIdFactEncabezado") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdFactEncabezado", idFactEncabezado);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable FactItemFiscalFiscalesGetListForIdFactEnca(uint idFactEncabezado)
		{
			MySqlCommand comm = _admin.GetComando("FactItemFiscalesGetListForIdFactEncabezado") as MySqlCommand;
			comm.Parameters.AddWithValue("_IdFactEncabezado", idFactEncabezado);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable OperacionesGetForFechas(DateTime dde, DateTime hta)
		{
			MySqlCommand comm = _admin.GetComando("OperacionesGetForFechas") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", dde);
			comm.Parameters.AddWithValue("_Hasta", hta);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable PagoFormasGetHabilitados(bool habilitado)
		{
			MySqlCommand comm = _admin.GetComando("PagoFormasGetListForHabilitado") as MySqlCommand;
			comm.Parameters.AddWithValue("_Habilitado", habilitado);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable ParcialesGetListForTurno(uint turno)
		{
			MySqlCommand comm = _admin.GetComando("parcialesGetListForTurno") as MySqlCommand;
			comm.Parameters.AddWithValue("_Turno", turno);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable PuestosGetAll()
		{
			MySqlCommand comm = _admin.GetComando("PuestosGetAll") as MySqlCommand;
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable TurnosGetUltimo()
		{
			MySqlCommand comm = _admin.GetComando("TurnosGetUltimo") as MySqlCommand;
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable TurnosGetForNumero(uint numero)
		{
			MySqlCommand comm = _admin.GetComando("TurnosGetForNumero") as MySqlCommand;
			comm.Parameters.AddWithValue("_Numero", numero);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable TurnosGetForDesdeHasta(DateTime dde, DateTime hta)
		{
			MySqlCommand comm = _admin.GetComando("TurnosGetForDesdeHasta") as MySqlCommand;
			comm.Parameters.AddWithValue("_FechaDesde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_FechaHasta", ConversorTipos(hta));
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable TurnosEntregadosGetForDesdeHasta(DateTime dde, DateTime hta)
		{
			MySqlCommand comm = _admin.GetComando("TurnosEntregadosGetForDesdeHasta") as MySqlCommand;
			comm.Parameters.AddWithValue("_FechaDesde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_FechaHasta", ConversorTipos(hta));
			DataTable dt = _admin.ExecuteReader(comm);
			dt.TableName = "Turnos";
			dt.PrimaryKey = new DataColumn[] { dt.Columns["IdTurno"] };
			return dt;
		}

		public static DataTable ValeEntregasGetForTurno(uint nroTurno)
		{
			MySqlCommand comm = _admin.GetComando("ValeEntregasGetForTurno") as MySqlCommand;
			comm.Parameters.AddWithValue("_Turno", nroTurno);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable ValeEntregasGetForParcial(uint turno, uint idParcial)
		{
			MySqlCommand comm = _admin.GetComando("valeentregasGetForParcial") as MySqlCommand;
			comm.Parameters.AddWithValue("_Turno", turno); 
			comm.Parameters.AddWithValue("_IdParcial", idParcial);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable ValeEntregasGetForFechasIdArticulo(DateTime dde, DateTime hta, uint idArticulo)
		{
			MySqlCommand comm = _admin.GetComando("valeentregasGetForFechasIdArticulo") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", dde);
			comm.Parameters.AddWithValue("_Hasta", hta);
			comm.Parameters.AddWithValue("_IdArticulo", idArticulo);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable VendedoresGetListForNombre(string nbre)
		{
			MySqlCommand comm = _admin.GetComando("VendedoresGetListForNombre") as MySqlCommand;
			comm.Parameters.AddWithValue("_Nombre", nbre);
			DataTable dt = _admin.ExecuteReader(comm);
			return dt;
		}

		public static DataTable ZCierresGetForFechas(DateTime dde, DateTime hta)
		{
			MySqlCommand comm = _admin.GetComando("zcierresGetListForDesdeHasta") as MySqlCommand;
			comm.Parameters.AddWithValue("_Desde", ConversorTipos(dde));
			comm.Parameters.AddWithValue("_Hasta", ConversorTipos(hta));
			DataTable dt = _admin.ExecuteReader(comm);
			dt.PrimaryKey = new DataColumn[] { dt.Columns["IdZCierre"] };
			return dt;
		}
		#endregion

		#region Extras
		/// <summary>
		/// Carga los parametros de un SP en el comando.
		/// </summary>
		/// <param name="comm">comando que se va a ejecutar</param>
		/// <param name="protoDatos">protocolo de datos</param>
		static void CargaParametros(MySqlCommand comm, PDatosFull protoDatos)
		{
			MySqlParameter p = null;
			#region Parametros
			foreach (PDatosUnit pd in protoDatos.Propiedades)
			{
				try
				{
					p = new MySqlParameter("_" + pd.Nombre, Datos.ConversorTipos(pd.Tipo.ToString()));
					string valTrimLower = (pd.Valor == null) ? null : pd.Valor.ToString().Trim().ToLower();
					switch (p.MySqlDbType)
					{
						case MySqlDbType.Bit:
							p.Value = (valTrimLower == "1" || valTrimLower == bool.TrueString.ToLower()) ? 1 : 0;
							break;
						case MySqlDbType.DateTime:
							p.Value = Datos.ConversorTipos(DateTime.Parse(valTrimLower));
							break;
						case MySqlDbType.Double:
						case MySqlDbType.Float:
						case MySqlDbType.Decimal:
							p.MySqlDbType = MySqlDbType.Decimal;
							p.Value = Convert.ToDecimal(valTrimLower);
							break;
						case MySqlDbType.Set:
							//para los tipos no soportados, pasar de largo
							continue;
						default:
							if (protoDatos.NombreClave.Trim().ToLower() == pd.Nombre.Trim().ToLower())
							{
								p.Direction = ParameterDirection.InputOutput;
							}
							p.Value = pd.Valor;
							break;
					}
					comm.Parameters.Add(p);
				}
				catch (Exception ex)
				{
					throw new Exception("Error en carga de parametros, con parametro " + pd.Nombre + ".", ex);
				}
			}
			#endregion
		}

		/// <summary>
		/// Convierte un tipo comun a uno de MySQL
		/// </summary>
		/// <param name="tipo"></param>
		/// <returns></returns>
		static MySqlDbType ConversorTipos(string tipo)
		{
			switch (tipo)
			{
				case "byte":
				case "System.Byte":
					return MySqlDbType.Byte;
				case "short":
				case "System.Int16":
					return MySqlDbType.Int16;
				case "int":
				case "System.Int32":
					return MySqlDbType.Int32;
				case "uint":
				case "System.UInt32":
					return MySqlDbType.Int32;
				case "long":
				case "System.Int64":
					return MySqlDbType.Int64;
				case "bool":
				case "System.Boolean":
					return MySqlDbType.Bit;
				case "string":
				case "System.String":
					return MySqlDbType.VarChar;
				case "System.DateTime":
					return MySqlDbType.DateTime;
				case "float":
				case "System.Single":
					return MySqlDbType.Float;
				case "double":
				case "System.Double":
					return MySqlDbType.Double;
				case "decimal":
				case "System.Decimal":
					return MySqlDbType.Decimal;
				default:
					return MySqlDbType.Set;
			}
		}

		/// <summary>
		/// Cambia el formato DD/MM/YYYY HH:MM:SS a YYYY/MM/DD HH:MM:SS
		/// </summary>
		/// <param name="fecha"></param>
		/// <returns></returns>
		static string ConversorTipos(DateTime fecha)
		{
			string resultado = "";
			resultado += fecha.Year.ToString();			//YYYY
			resultado += "/" + fecha.Month.ToString();	//MM
			resultado += "/" + fecha.Day.ToString();	//DD
			//
			resultado += " " + fecha.Hour.ToString();	//HH
			resultado += ":" + fecha.Minute.ToString(); //MM
			resultado += ":" + fecha.Second.ToString();	//SS
			//
			return resultado;
		}

		/// <summary>
		/// Cambia el formato DD/MM/YYYY HH:MM:SS a YYYY/MM/DD HH:MM:SS
		/// </summary>
		/// <param name="fecha"></param>
		/// <param name="conHoraCero">Dice si hay que poner la hora en cero</param>
		/// <returns></returns>
		static string ConversorTipos(DateTime fecha, bool conHoraCero)
		{
			if (!conHoraCero)
				return ConversorTipos(fecha);
			string resultado = "";
			resultado += fecha.Year.ToString();			//YYYY
			resultado += "/" + fecha.Month.ToString();	//MM
			resultado += "/" + fecha.Day.ToString();	//DD
			//
			resultado += " 00:00:00";
			//
			return resultado;
		}
		#endregion
	}
}
