using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Negocio;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using FormCom;

namespace Gasolero
{
	static class Program
	{
		public static bool SinFiscal = false;

		static Thread _thBackup;
		static Semaphore _semaforo;
		static Mutex _mutexUnico = null;
		static Dictionary<string, string> _args = null;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			bool seguir;
			//XmlDocument xdoc = new XmlDocument();
			//xdoc.Load(@"E:\Descargas\z.xml");
			VerArgs(args);
			try
			{
				bool mRes = true;
				_mutexUnico = new Mutex(true, "GASOLERO - MUTEX", out mRes);
				if (!mRes)
				{
					MessageBox.Show("La aplicación ya está corriendo", "Error al iniciar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error al iniciar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			try
			{
				string pathConfig = _args.ContainsKey("-config") ? _args["-config"] : Path.Combine(RutaInicial, "config.ini");
				Reglas.Init(pathConfig);
				SinFiscal = !AppConfig.UsarFiscal;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show("Ocurrio un error al iniciar" + Environment.NewLine +
					"Verifique la conexión al servidor y la base de datos", "E R R O R",
					 MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				Fiscal.PuestoDeVenta();
			}
			catch
			{
				//sin controlador
				SinFiscal = true;
			}
			try
			{
				_thBackup = new Thread(new ThreadStart(GuardiaCte));
				_semaforo = new Semaphore(1, 1);
				_thBackup.Start();
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show("Ocurrio un error con el guardia de resguardo");
			}
			do
			{
				try
				{
					if (String.IsNullOrEmpty(AppConfig.CarpetaReportes))
					{
						AppConfig.CarpetaReportes = Program.RutaInicial;
					}
					CheckBackup();
					seguir = false;
					Application.Run(new FrmMain());
				}
				catch (Exception ex)
				{
					Reglas.AddLog(ex);
					seguir = MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine +
						"¿Desea volver a cargar el programa?", "Ocurrió un error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;
				}
			} while (seguir);
			try
			{
				if (_thBackup != null)
					_thBackup.Abort();
			}
			catch { }
		}

		public static void CheckBackup()
		{
			//cero => nunca
			if (AppConfig.IntervaloBackup == 0)
				return;
			TimeSpan tiempo = DateTime.Now - AppConfig.UltimoBackup;
			if (tiempo.Days * 24 + tiempo.Hours > (AppConfig.IntervaloBackup * 24))
			{
				HacerBackup();
			}
		}

		public static void HacerBackup()
		{
			try
			{
				_semaforo.WaitOne();
				string param;
				if (File.Exists("dump.ini"))
					param = File.ReadAllText("dump.ini");
				else
					param = "--user=root --password=sa";
				DateTime fecha = DateTime.Now;
				string fechaSt = String.Format("{0:0000}{1:00}{2:00} {3:00}{4:00}",
					fecha.Year, fecha.Month, fecha.Day, fecha.Hour, fecha.Minute);
				File.WriteAllText("bak.bat", "mysqldump --opt " + param + " gasolero >> \"Gasolero Datos " + fechaSt + "\".sql");
				//File.AppendAllText("bak.bat", Environment.NewLine + "pause");
				Process prDump = Process.Start("bak.bat");
				prDump.WaitForExit(3000);
				if (!prDump.HasExited)
					prDump.Kill();
				File.Delete("bak.bat");
				AppConfig.UltimoBackup = fecha;
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				MessageBox.Show("Ocurrio un error al crear el respaldo");
			}
			finally
			{
				_semaforo.Release();
			}
		}

		public static string RutaInicial
		{
			get
			{
				return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
			}
		}

		static void GuardiaCte()
		{
			try
			{
				do
				{
					DateTime ahora = Reglas.Now;
					//DateTime ahora = new DateTime(2010, 5, 10, 23, 30, 10);
					TimeSpan falta = new DateTime(ahora.Year, ahora.Month, ahora.Day,
						23, 59, 30) - ahora;
					if (falta.TotalMinutes < 59)
					{
						Thread.Sleep(falta);
						if (AppConfig.CierreZ1raVenta == AppConfig.enCierreZ1raVenta.Medianoche00)
							Fiscal.CierreZ();
						else if (AppConfig.CierreZ1raVenta == AppConfig.enCierreZ1raVenta.SoloDia1EnMedianoche00
							&& Reglas.Now.AddMinutes(5).Day == 1)
							Fiscal.CierreZ();
					}
					else
					{
						Thread.Sleep(new TimeSpan(1, 0, 0));
					}
					CheckBackup();
				} while (true);
			}
			catch { }
		}

		static void VerArgs(string[] argumentos)
		{
			_args = new Dictionary<string, string>();
			for (int i = 0; i < argumentos.Length; i++)
			{
				if (argumentos[i].Trim().StartsWith("-"))
				{
					_args[argumentos[i].Trim().ToLower()] = (i + 1 < argumentos.Length) ? argumentos[i + 1] : "";
					i++;//ya vi el valor del argumento
				}
			}
		}

		public static void Reiniciar()
		{
			if (_mutexUnico != null)
			{
				_mutexUnico.ReleaseMutex();
				_mutexUnico = null;
			}
			Application.Restart();
		}

		public static DateTime Fecha()
		{
			return DateTime.Now;
		}

		public static Cliente BuscarCliente()
		{
			Cliente res = null;
			QFiltro filtroQ;
			using (ABMForm busqClientes = new ABMForm(false, false))//TH ajustado
			{
				busqClientes.ActualizarTabla(Cliente.Esquema());
				busqClientes.ActualizarTabla(Cliente.GetAll());
				busqClientes.TablaEsquema = "Esquemas";
				busqClientes.TablaMain = Cliente.NombreTabla;
				filtroQ = new QFiltro("Nombre");
				busqClientes.AddAddOn(filtroQ);
				filtroQ.SeleccionAceptada += new EventHandler(delegate
				{
					ABMForm abm = busqClientes;
					res = new Cliente(abm.ItemActual.Row);
					abm.Close();
				});
				filtroQ.SeleccionCancelada += new EventHandler(delegate
				{
					ABMForm abm = busqClientes;
					abm.Close();
				});
				filtroQ.PermitirABM = false;
				filtroQ.TextoFiltro = "";
				busqClientes.Load += new EventHandler(delegate
				{
					new System.Threading.Thread(delegate()
					{
						System.Threading.Thread.Sleep(AppConfig.DemoraRefresh);
						busqClientes.Invoke((MethodInvoker)delegate
						{
							busqClientes.Icon = System.Drawing.Icon.FromHandle(Recursos.Cliente.GetHicon());
							busqClientes.Text = "Buscar Cliente";
							busqClientes.ReajustarGrilla();
							busqClientes.Refresh();
						});
					}).Start();
				});
				busqClientes.ShowDialog();
			}
			return res;
		}
	}
}