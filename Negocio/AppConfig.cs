using System;
using System.Collections.Generic;
using System.Text;
using IniParser;
using IniParser.Model;


namespace Negocio
{
	public static class AppConfig
	{
		static IniData _data = null;
		static FileIniDataParser _parser = null;
		static string _archivo = null;

		public static void Init(string file)
		{
			_archivo = file;
			_parser = new FileIniDataParser();
			_data = _parser.ReadFile(file);
		}

		/// <summary>
		/// Dice si existe la clave en el config
		/// </summary>
		public static bool Existe(string key, string seccion = "General")
		{
			try
			{
				string obj = "";
				return _data.TryGetKey(seccion + _data.SectionKeySeparator.ToString() + key, out obj);
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return false;
			}
		}

		/// <summary>
		/// Ultimo usuario de la app
		/// </summary>
		public static uint UltimoUsuario
		{
			get
			{
				uint res = 1;
				try
				{
					res = uint.Parse(_data["General"]["UltimoUsuario"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						UltimoUsuario = 1;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["UltimoUsuario"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Intervalo entre respaldos
		/// </summary>
		public static uint IntervaloBackup
		{
			get
			{
				uint res = 7;
				try
				{
					res = uint.Parse(_data["General"]["IntervaloBackup"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						IntervaloBackup = 7;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["IntervaloBackup"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Fecha de último respaldo
		/// </summary>
		public static DateTime UltimoBackup
		{
			get
			{
				DateTime res = DateTime.Now;
				try
				{
					res = DateTime.Parse(_data["General"]["UltimoBackup"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						UltimoBackup = DateTime.Now;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["UltimoBackup"] = value.ToShortDateString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Demora en el refresh de las pestañas
		/// </summary>
		public static int DemoraRefresh
		{
			get
			{
				int res = 500;
				try
				{
					res = int.Parse(_data["General"]["DemoraRefresh"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						DemoraRefresh = 500;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["DemoraRefresh"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}

		}
		/// <summary>
		/// Distancia del split principal
		/// </summary>
		public static int SplitDist
		{
			get
			{
				int res = 152;
				try
				{
					res = int.Parse(_data["General"]["SplitDist"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						SplitDist = 152;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["SplitDist"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Fecha de último infore Z
		/// </summary>
		public static DateTime UltimoZ
		{
			get
			{
				DateTime res = DateTime.Now;
				try
				{
					res = DateTime.Parse(_data["General"]["UltimoZ"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						UltimoZ = DateTime.Now;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["UltimoZ"] = value.ToShortDateString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Hacer Z global?
		/// </summary>
		public static bool ZGlobal
		{
			get
			{
				bool res = false;
				try
				{
					res = bool.Parse(_data["General"]["ZGlobal"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						ZGlobal = false;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["ZGlobal"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Abrir reportes al crear?
		/// </summary>
		public static bool AbrirReportes
		{
			get
			{
				bool res = true;
				try
				{
					res = bool.Parse(_data["General"]["AbrirReportes"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						AbrirReportes = true;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["AbrirReportes"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Carpeta de reportes
		/// </summary>
		public static string CarpetaReportes
		{
			get
			{
				string res = "";
				try
				{
					res = _data["General"]["CarpetaReportes"];
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						res = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
						CarpetaReportes = res;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["CarpetaReportes"] = value;
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Ubicación de PetroRed
		/// </summary>
		public static string PetroRed
		{
			get
			{
				string res = "";
				try
				{
					res = _data["General"]["PetroRed"];
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						res = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "PetroRed3.mdb");
						PetroRed = res;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["PetroRed"] = value;
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Confirmar Vales?
		/// </summary>
		public static bool ConfirmarVales
		{
			get
			{
				bool res = true;
				try
				{
					res = bool.Parse(_data["General"]["ConfirmarVales"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						ConfirmarVales = true;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["ConfirmarVales"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Modo de cierre Z en la 1° venta
		/// </summary>
		public static enCierreZ1raVenta CierreZ1raVenta
		{
			get
			{
				enCierreZ1raVenta res = enCierreZ1raVenta.SoloDia1EnMedianoche00;
				try
				{
					res = (enCierreZ1raVenta)uint.Parse(_data["General"]["CierreZ1raVenta"]);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						CierreZ1raVenta = enCierreZ1raVenta.SoloDia1EnMedianoche00;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					int valInt = (int)value;
					_data["General"]["CierreZ1raVenta"] = valInt.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		public enum enCierreZ1raVenta
		{
			Nunca = 0,
			Siempre = 1,
			SoloDia1 = 2,
			Medianoche00 = 3,
			SoloDia1EnMedianoche00 = 5
		}

		/// <summary>
		/// IP de la VOX
		/// </summary>
		public static string IPVOX
		{
			get
			{
				string res = "";
				try
				{
					res = _data["General"]["IPVOX"];
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						res = "192.168.5.220";
						IPVOX = res;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["PetroRed"] = value;
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}

		/// <summary>
		/// Devuelve el puerto fiscal de para esta pc o el global en su defecto
		/// </summary>
		public static int PuertoFiscal
		{
			get
			{
				int res = Reglas.PuertoFiscal;
				try
				{
					string nro = "";
					if (_data.TryGetKey("General" + _data.SectionKeySeparator.ToString() + "PuertoFiscal", out nro))
						res = int.Parse(nro);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						PuertoFiscal = Reglas.PuertoFiscal;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["PuertoFiscal"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}

		}

		/// <summary>
		/// Devuelve el modelo de impresora fiscal de para esta pc o el global en su defecto
		/// </summary>
		public static int ModeloFiscal
		{
			get
			{
				int res = Reglas.ModeloFiscal;
				try
				{
					string modelo = "";
					if (_data.TryGetKey("General" + _data.SectionKeySeparator.ToString() + "ModeloFiscal", out modelo))
						res = int.Parse(modelo);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						ModeloFiscal = Reglas.ModeloFiscal;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["ModeloFiscal"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}

		}

		/// <summary>
		/// Usar controlador fiscal en esta pc? (o global en su defecto)
		/// </summary>
		public static bool UsarFiscal
		{
			get
			{
				bool res = Reglas.UsarFiscal;
				try
				{
					string usar = "";
					if (_data.TryGetKey("General" + _data.SectionKeySeparator.ToString() + "UsarFiscal", out usar))
						res = bool.Parse(usar);
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					try
					{
						UsarFiscal = Reglas.UsarFiscal;
					}
					catch (Exception ex2)
					{
						Reglas.AddLog(ex2);
					}
				}
				return res;
			}
			set
			{
				try
				{
					_data["General"]["UsarFiscal"] = value.ToString();
					_parser.WriteFile(_archivo, _data);
				}
				catch (Exception ex2)
				{
					Reglas.AddLog(ex2);
				}
			}
		}
	}
}
