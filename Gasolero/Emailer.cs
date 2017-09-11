using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using Negocio;
using System.Threading;

namespace Gasolero
{
	public class Emailer
	{
		public string Host { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool EnableSsl { get; set; }
		public int Puerto { get; set; }

		public static void Informar(CCMovimiento ccMov)
		{
			try
			{
				if (ccMov == null || !Reglas.EnviarEmail)
					return;
				string usuario = Reglas.Sede == "SL" ? "info.summa@oikos-grupo.com.ar" : "info.lacruz@oikos-grupo.com.ar";
				string clave = Reglas.Sede == "SL" ? "summazero0" : "lacruzdelsur0";
				Cliente c = Cliente.GetSingleE(ccMov.IdCliente);
				string monto = ccMov.Monto < 0 ? "Monto de la carga de combustible: " : "Monto cargado en cuenta corriente: ";
				monto += ccMov.Monto.ToString("$ 0.00");
				string itemsVendidos = "Detalle de Venta: ";
				if (ccMov.IdFactEncabezado > 0)
				{
					FactEncabezado fact = FactEncabezado.GetSingleE(ccMov.IdFactEncabezado);
					//fact.GetFilas();//ya lo hace getsinlgeE
					foreach (var item in fact.Cuerpo)
					{
						Articulo art = Articulo.GetSingleE(item.IdArticulo);
						itemsVendidos += art.Descripcion + ": " + item.Cantidad + art.Medida + Environment.NewLine;
					}
					if (c.Descuento > 0)
						monto += " (incluye " + c.Descuento.ToString("0.###") + "% de descuento)";
				}
				if (c.Email != "")
				{
					Emailer e = new Emailer(usuario, clave);
					Thread hilo = new Thread(() =>
						e.SendAndMbox(c.Email, "Nuevo Movimiento de Cuenta Corriente",
							"Fecha: " + ccMov.Fecha.ToString() + Environment.NewLine +
							monto + Environment.NewLine +
							"Saldo a favor del cliente: " + CCMovimiento.GetSaldoForCliente(c.IdCliente, Reglas.Now).ToString("$ 0.00") + Environment.NewLine +
							"Descripción: " + ccMov.Descripcion + (ccMov.Monto > 0 ? "" : (Environment.NewLine +//detalle de venta y campos de autorizados para ventas (montos negativos)
							itemsVendidos +
							"Autorizado: " + ccMov.NombreAutorizado + Environment.NewLine +
							"DNI: " + ccMov.DNIAutorizado + Environment.NewLine +
							"Patente: " + ccMov.PatenteAutorizado.ToString()
							))));
					hilo.Start();
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
			}
		}

		public static void Informar(uint idCliente, string adjunto, string asunto)
		{
			Cliente c = Cliente.GetSingleE(idCliente);
			Informar(c, adjunto, asunto);
		}

		public static void Informar(Cliente c, string adjunto, string asunto)
		{
			try
			{
				if (c == null || !Reglas.EnviarEmail || c.Email == "")
					return;
				else
				{
					string usuario = Reglas.Sede == "SL" ? "info.summa@oikos-grupo.com.ar" : "info.lacruz@oikos-grupo.com.ar";
					string clave = Reglas.Sede == "SL" ? "summazero0" : "lacruzdelsur0";
					//string usuario = "mariano@logastore.com";
					//string clave = "13579013";
					Emailer e = new Emailer(usuario, clave);
					Thread hilo = new Thread(() =>
						e.SendAndMbox(c.Email, asunto, "Informe adjunto.", adjunto));
					hilo.Start();
				}
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
			}
		}

		//public Emailer(string usuario, string clave, string host = "smtp-mail.outlook.com", bool ssl = true, int port = 587)
		public Emailer(string usuario, string clave, string host = "mail.oikos-grupo.com.ar", bool ssl = false, int port = 587)
		{
			Host = host;
			UserName = usuario;
			Password = clave;
			EnableSsl = ssl;
			Puerto = port;
		}

		void SendAndMbox(string destinatario, string asunto, string cuerpo, string adjunto = "")
		{
			string res = Send(destinatario, asunto, cuerpo, adjunto);
			if (res != "")
				FrmMain.FMain.Invoke(new Action(() =>
				{
					if (System.Windows.Forms.MessageBox.Show(FrmMain.FMain, res + Environment.NewLine + Environment.NewLine + "¿Desea reintentar envío?", "Error al notificar por email", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error)
						== System.Windows.Forms.DialogResult.Yes)
						SendAndMbox(destinatario, asunto, cuerpo, adjunto);
				}));
			else
				FrmMain.FMain.Invoke(new Action(() => { FrmMain.FMain.Mensaje = "Informe enviado a " + destinatario + " exitosamente!"; }));
		}

		public string Send(string destinatario, string asunto, string cuerpo, string adjunto = "")
		{
			try
			{
				MailMessage msg = new MailMessage();
				msg.From = new MailAddress(UserName);
				msg.To.Add(destinatario);
				msg.Subject = asunto;
				if (!String.IsNullOrWhiteSpace(adjunto))
					msg.Attachments.Add(new Attachment(adjunto));
				msg.Body = cuerpo;
				SmtpClient smt = new SmtpClient();
				smt.Host = Host;//"smtp.gmail.com";
				System.Net.NetworkCredential ntcd = new NetworkCredential();
				ntcd.UserName = UserName;// "nilusilu3@gmail.com";
				ntcd.Password = Password;
				smt.Credentials = ntcd;
				smt.EnableSsl = EnableSsl;
				smt.Port = Puerto;// 587;
				//smt.DeliveryMethod = SmtpDeliveryMethod.Network;
				//smt.UseDefaultCredentials = false;
				smt.Send(msg);
				return "";
			}
			catch (Exception ex)
			{
				Reglas.AddLog(ex);
				return ex.Message;
			}
		}
	}
}
