using System;
using System.Collections.Generic;
using System.Text;
using MarUtils.Entidades;
using System.Reflection;
using System.IO;
using System.Collections;
using MarUtils.Soporte;
using System.Data;

namespace Negocio
{
	public class Reglas
	{
		public delegate bool CommHandler(string msj);
		public delegate DateTime DateHandler();

		public static CommHandler Confirmar;
		public static DateHandler FechaActual;

		static Vendedor _vdorCte;
		static Puesto[] _puestos;

		public static Vendedor VendedorActual
		{
			get { return _vdorCte; }
			internal set { _vdorCte = value; }
		}

		/// <summary>
		/// Turno anterior al actual, ya que el actual aun no cierra
		/// </summary>
		public static Turno TurnoUltimo
		{
			get { return Turno.GetUltimo(); }
		}

		public static int CantidadPuestos
		{
			get { return _puestos.Length; }
		}

		public static Puesto[] Puestos
		{
			get { return _puestos; }
		}

		public static DateTime Now
		{
			get 
			{
				if (FechaActual != null)
					return FechaActual();
				else
					return DAL.Datos.Now();
			}
		}

		public static DateTime NowServidor
		{
			get
			{
				return DAL.Datos.Now();
			}
		}

		public static string FactPuestoDeVenta
		{
			get { return DAL.Datos.FactPuestoDeVenta; }
			set { DAL.Datos.FactPuestoDeVenta = value; }
		}

		/// <summary>
		/// Cliente para notas de credito B
		/// </summary>
		public static uint ClienteNCB
		{
			get { return uint.Parse(DAL.Datos.SettingsGet("ClienteNCB")); }
			set { DAL.Datos.SettingsSet("ClienteNCB", value.ToString()); }
		}

		/// <summary>
		/// Cliente para vales excedidos (alias fiado)
		/// </summary>
		public static uint ClienteGobierno
		{
			get 
			{ 
				uint aux = uint.Parse(DAL.Datos.SettingsGet("ClienteGobierno"));
				return aux == 0 ? uint.MaxValue : aux;
			}
			set { DAL.Datos.SettingsSet("ClienteGobierno", value.ToString()); }
		}

		/// <summary>
		/// Forma de pago para cuenta corriente
		/// </summary>
		public static uint PagoCC
		{
			get { return uint.Parse(DAL.Datos.SettingsGet("PagoCC")); }
			set { DAL.Datos.SettingsSet("PagoCC", value.ToString()); }
		}

		/// <summary>
		/// Forma de pago para tarjeta
		/// </summary>
		public static uint PagoTarjeta
		{
			get { return uint.Parse(DAL.Datos.SettingsGet("PagoTarjeta")); }
			set { DAL.Datos.SettingsSet("PagoTarjeta", value.ToString()); }
		}

		public static bool UsarFiscal
		{
			get { return DAL.Datos.SettingsGet("UsarFiscal") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("UsarFiscal", value.ToString()); }
		}

		public static bool NotaCreditoEnDiferencia
		{
			//get { return DAL.Datos.SettingsGet("NotaCreditoEnDiferencia") == bool.TrueString; }
			get { return Reglas.VendedorActual.EsRol(enPermisos.Encargado) >= 0; }
			set { DAL.Datos.SettingsSet("NotaCreditoEnDiferencia", value.ToString()); }
		}

		public static bool VerDiferencia
		{
			get { return DAL.Datos.SettingsGet("VerDiferencia") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("VerDiferencia", value.ToString()); }
		}

		public static bool ReporteXZALog
		{
			get { return DAL.Datos.SettingsGet("ReporteXZALog") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("ReporteXZALog", value.ToString()); }
		}

		/// <summary>
		/// Porcentaje de margen inferior para los cierres de puesto
		/// </summary>
		public static int MargenInf
		{
			get { return int.Parse(DAL.Datos.SettingsGet("MargenInf")); }
			set { DAL.Datos.SettingsSet("MargenInf", value.ToString()); }
		}

		/// <summary>
		/// Porcentaje de margen superior para los cierres de puesto
		/// </summary>
		public static int MargenSup
		{
			get { return int.Parse(DAL.Datos.SettingsGet("MargenSup")); }
			set { DAL.Datos.SettingsSet("MargenSup", value.ToString()); }
		}

		/// <summary>
		/// Cantidad de turnos para calcular promedio de los cierres de puesto
		/// </summary>
		public static int TurnosParaPromedio
		{
			get { return int.Parse(DAL.Datos.SettingsGet("TurnosParaPromedio")); }
			set { DAL.Datos.SettingsSet("TurnosParaPromedio", value.ToString()); }
		}

		/// <summary>
		/// Cantidad de turnos para calcular promedio de los cierres de puesto
		/// </summary>
		public static enModoMargen ModoMargen
		{
			get { return (enModoMargen)int.Parse(DAL.Datos.SettingsGet("ModoMargen")); }
			set { DAL.Datos.SettingsSet("ModoMargen", ((int)value).ToString()); }
		}

		/// <summary>
		/// Sede donde corre el sistema
		/// </summary>
		public static string Sede
		{
			get { return DAL.Datos.SettingsGet("Sede"); }
			set { DAL.Datos.SettingsSet("Sede", value); }
		}

		public static bool AnularSiempre
		{
			get { return DAL.Datos.SettingsGet("AnularSiempre") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("AnularSiempre", value.ToString()); }
		}

		public static int PuertoFiscal
		{
			get { return int.Parse(DAL.Datos.SettingsGet("PuertoFiscal")); }
			set { DAL.Datos.SettingsSet("PuertoFiscal", value.ToString()); }
		}

#warning ver esto
		public static int PausaEntreFiscal
		{
			get { return int.Parse(DAL.Datos.SettingsGet("PausaEntreFiscal")); }
			set { DAL.Datos.SettingsSet("PausaEntreFiscal", value.ToString()); }
		}

		public static int ModeloFiscal
		{
			get { return int.Parse(DAL.Datos.SettingsGet("ModeloFiscal")); }
			set { DAL.Datos.SettingsSet("ModeloFiscal", value.ToString()); }
		}

		public static string MarcaParcial
		{
			get { return DAL.Datos.SettingsGet("MarcaParcial"); }
			set { DAL.Datos.SettingsSet("MarcaParcial", value); }
		}

		public static bool VerErrorFiscal
		{
			get { return DAL.Datos.SettingsGet("VerErrorFiscal").ToLower() == bool.TrueString.ToLower(); }
			set { DAL.Datos.SettingsSet("VerErrorFiscal", value.ToString().Trim()); }
		}

		public static bool ImprimirFinalComoParcial
		{
			get { return DAL.Datos.SettingsGet("ImprimirFinalComoParcial").ToLower() == bool.TrueString.ToLower(); }
			set { DAL.Datos.SettingsSet("ImprimirFinalComoParcial", value.ToString().Trim()); }
		}

		public static string Resolucion
		{
			get { return DAL.Datos.SettingsGet("Resolucion"); }
			set { DAL.Datos.SettingsSet("Resolucion", value); }
		}

		/// <summary>
		/// Ultimo id de operacion impreso
		/// </summary>
		public static uint UltimoIdImpreso
		{
			get { return uint.Parse(DAL.Datos.SettingsGet("UltimoIdImpreso")); }
			set { DAL.Datos.SettingsSet("UltimoIdImpreso", value.ToString()); }
		}

		/// <summary>
		/// Ultimo id de operacion incluido en cierre
		/// </summary>
		public static uint UltimoIdIncluido
		{
			get { return uint.Parse(DAL.Datos.SettingsGet("UltimoIdIncluido")); }
			set { DAL.Datos.SettingsSet("UltimoIdIncluido", value.ToString()); }
		}

		/// <summary>
		/// Muestra los menues para correr fix
		/// </summary>
		public static bool VerFix
		{
			get { return DAL.Datos.SettingsGet("VerFix") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("VerFix", value.ToString()); }
		}

		/// <summary>
		/// Emitir comprobantes para movimientos de CC?
		/// </summary>
		public static bool CCFiscal
		{
			get { return DAL.Datos.SettingsGet("CCFiscal") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("CCFiscal", value.ToString()); }
		}

		/// <summary>
		/// Permitir envíos de email?
		/// </summary>
		public static bool EnviarEmail
		{
			get { return DAL.Datos.SettingsGet("EnviarEmail") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("EnviarEmail", value.ToString()); }
		}

		public static bool EditarSalidaVOX
		{
			get { return DAL.Datos.SettingsGet("EditarSalidaVOX") == bool.TrueString; }
			set { DAL.Datos.SettingsSet("EditarSalidaVOX", value.ToString()); }
		}

		/// <summary>
		/// Init
		/// </summary>
		public static void Init(string pathConfig)
		{
			if (pathConfig != null)//pudo ya haber sido inicializado
				AppConfig.Init(pathConfig);
			//Crypto.HashXOR
			DAL.Datos.Init();
			string petroRedMdb = AppConfig.PetroRed;
			DAL.DatosMdb.Init(petroRedMdb);
			DAL.DatosXml.Init();
			DataTable dtPuestos = DAL.Datos.PuestosGetAll();
			_puestos = new Puesto[dtPuestos.Rows.Count];
			for (int i = 0; i < CantidadPuestos; i++)
			{
				_puestos[i] = new Puesto(dtPuestos.Rows[i]);
			}
			Cliente cli = new Cliente();
			cli.Id = 1;
			cli.Nombre = "CONSUMIDOR FINAL";
			cli.RespInscripto = false;
			Cliente.ConsFinal = cli;
		}

		/// <summary>
		/// Hace una nueva entidad
		/// </summary>
		public static EntidadBase New(Type tipo, IDictionary<string, object> valores)
		{
			EntidadBase ent = (EntidadBase)tipo.GetConstructor(Type.EmptyTypes).Invoke(null);
			foreach (PropertyInfo prop in tipo.GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.Public))
			{
				if (!valores.ContainsKey(prop.Name))
					continue;
				try
				{
					prop.SetValue(ent, Convert.ChangeType(valores[prop.Name], prop.PropertyType), null);
				}
				catch
				{

				}
			}
			return ent;
		}

		public static EntidadBase New(string nbrTabla, IDictionary<string, object> valores)
		{
			Type tipo = null;
			switch (nbrTabla.ToLower())
			{
				case "articulos":
					tipo = typeof(Articulo);
					break;
				case "vendedores":
					tipo = typeof(Vendedor);
					break;
				case "clientes":
					tipo = typeof(Cliente);
					break;
				case "ccautorizados":
					tipo = typeof(CCAutorizado);
					break;
				case "puestos":
					tipo = typeof(Puesto);
					break;
				case "densidades":
					tipo = typeof(Densidad);
					break;
				case "pagoformas":
					tipo = typeof(PagoForma);
					break;
				case "atajos":
					tipo = typeof(Atajo);
					break;
				case "valeentregas":
					tipo = typeof(ValeEntrega);
					break;
				//case "unidadmedidas":
				//    tipo = typeof(UnidadMedida);
				//    break;
				//case "estadofisicos":
				//    tipo = typeof(EstadoFisico);
				//    break;
				default:
					throw new Exception("No habia ctor para " + nbrTabla);
			}
			return New(tipo, valores);
		}

		public static DataTable GetAll(string tabla)
		{
			switch (tabla.ToLower())
			{
				case "clientes":
					return Cliente.GetAll();
				case "articulos":
					return Articulo.GetAll();
				case "fichas":
					return Ficha.GetAll();
				case "pagoformas":
					return PagoForma.GetAll();
				case "productos":
					return Producto.GetAll();
				case "puestos":
					return Puesto.GetAll();
				case "vehiculos":
					return Vehiculo.GetAll();
				case "vendedor":
					return Vendedor.GetAll();
				default:
					return null;
			}
		}

		static bool _errorEnLog = false;

		/// <summary>
		/// Añade una linea al log del programa.
		/// </summary>
		/// <param name="registro"></param>
		public static void AddLog(string registro)
		{
			if (!_errorEnLog)
			{
				try
				{
					File.AppendAllText("Registro.txt", DateTime.Now.ToString() + " " + registro + "\r\n", Encoding.Default);
				}
				catch
				{
					_errorEnLog = true;
				}
			}
		}

		/// <summary>
		/// Añade una linea al log del programa.
		/// </summary>
		/// <param name="ex">excepcion a reportar</param>
		public static void AddLog(Exception ex)
		{
			Reglas.AddLog("---------------INICIO-----------------");
			Reglas.AddLog("en " + ex.TargetSite + " -> " + ex.Message);
			Reglas.AddLog(" Data en excepcion:");
			foreach (DictionaryEntry diccEntry in ex.Data)
				Reglas.AddLog("\t" + diccEntry.Key + " = " + diccEntry.Value);
			Reglas.AddLog(" Fin Data.---------");
			Reglas.AddLog("------------------");
			if (ex.InnerException != null)
			{
				Reglas.AddLog(" Inner Excepcion:");
				AddLog(ex.InnerException);
				Reglas.AddLog(" Fin Inner Excepcion.---------");
			}
			else
			{
				Reglas.AddLog(" Stack Trace:");
				AddLog(ex.StackTrace);
				Reglas.AddLog(" Fin Stack Trace.---------");
			}
			Reglas.AddLog("---------------FIN-------------------");
		}

		/// <summary>
		/// Loguea un usuario en el sistema
		/// </summary>
		/// <param name="vdor"></param>
		/// <param name="pass">clave sin encriptar</param>
		/// <returns></returns>
		public static bool Login(Vendedor vdor, string pass)
		{
			string passHashed = Crypto.HashU2Hexa(pass);
			if (vdor.Password == "" || passHashed == vdor.Password)
			{
				VendedorActual = vdor;
				return true;
			}
			else
				return false;
		}

		public static void Logout()
		{
			VendedorActual = null;
		}

		/// <summary>
		/// Manda un mensaje al usuario
		/// </summary>
		static internal bool PedirConfirmacion(string msj)
		{
			if (Confirmar != null)
				return Confirmar(msj);
			else
				return true;
		}
	}
}
