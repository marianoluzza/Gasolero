using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Negocio;
using MarUtils.Soporte;
using MarUtils.Controles;

namespace Gasolero
{
	public partial class PanelLogin : UserControl
	{
		DataTable dtUsers = Vendedor.GetAll();
		public event EventHandler LoggedIn;
		public event EventHandler LoggedOut;

		public bool Logged
		{
			get { return Negocio.Reglas.VendedorActual != null; }
		}

		public PanelLogin()
		{
			InitializeComponent();
			CBUsuarios.DisplayMember = "Nombre";
			CBUsuarios.ValueMember = dtUsers.PrimaryKey[0].ColumnName;
			CBUsuarios.DataSource = new DataView(dtUsers);
			CBUsuarios.SelectedValue = AppConfig.UltimoUsuario;
		}

		private void BLogin_Click(object sender, EventArgs e)
		{
			if (Logged)
			{
				LogOut();
			}
			else
			{
				if (CBUsuarios.SelectedIndex < 0)
					return;
				Vendedor v = new Vendedor(dtUsers.Rows[CBUsuarios.SelectedIndex]);
				LogIn(v, TBPass.Text);
			}
		}

		public bool LogIn(Vendedor v, string pass)
		{
			bool res = Reglas.Login(v, pass);
			if (res)
			{
				CBUsuarios.Enabled = false;
				TBPass.Enabled = false;
				BLogin.Text = "Salir";
				linkCambioPass.Visible = true;
				if (LoggedIn != null)
					LoggedIn(this, EventArgs.Empty);
			}
			else
			{
				MessageBox.Show("Contraseña incorrecta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			TBPass.Text = "";
			return res;
		}

		public void LogOut()
		{
			Reglas.Logout();
			CBUsuarios.Enabled = true;
			linkCambioPass.Visible = false;
			TBPass.Enabled = true;
			BLogin.Text = "Entrar";
			if (LoggedOut != null)
				LoggedOut(this, EventArgs.Empty);
			TBPass.Text = "";
		}

		private void linkCambioPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			FrmMain.FMain.AdminPass(Reglas.VendedorActual);
		}

		private void TBPass_Enter(object sender, EventArgs e)
		{
			TBPass.Tag = ParentForm.AcceptButton;
			ParentForm.AcceptButton = BLogin;
		}

		private void TBPass_Leave(object sender, EventArgs e)
		{
			ParentForm.AcceptButton = TBPass.Tag as IButtonControl;
		}

		public void ActualizarTabla(DataTable dt)
		{
			if (dt.TableName.ToLower() == dtUsers.TableName.ToLower())
			{
				object ultId = CBUsuarios.SelectedValue;
				dtUsers.Clear();
				dtUsers.AcceptChanges();
				dtUsers.Merge(dt);
				dtUsers.AcceptChanges();
				CBUsuarios.SelectedValue = ultId;
			}
		}

		public void FocusTB()
		{
			TBPass.Focus();
		}
	}
}
