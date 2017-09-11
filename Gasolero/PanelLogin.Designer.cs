namespace Gasolero
{
	partial class PanelLogin
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.GBMain = new System.Windows.Forms.GroupBox();
			this.linkCambioPass = new System.Windows.Forms.LinkLabel();
			this.BLogin = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.TBPass = new System.Windows.Forms.TextBox();
			this.CBUsuarios = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.GBMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// GBMain
			// 
			this.GBMain.Controls.Add(this.linkCambioPass);
			this.GBMain.Controls.Add(this.BLogin);
			this.GBMain.Controls.Add(this.label2);
			this.GBMain.Controls.Add(this.TBPass);
			this.GBMain.Controls.Add(this.CBUsuarios);
			this.GBMain.Controls.Add(this.label1);
			this.GBMain.Location = new System.Drawing.Point(3, 3);
			this.GBMain.Name = "GBMain";
			this.GBMain.Size = new System.Drawing.Size(257, 121);
			this.GBMain.TabIndex = 0;
			this.GBMain.TabStop = false;
			this.GBMain.Text = "Acceso";
			// 
			// linkCambioPass
			// 
			this.linkCambioPass.AutoSize = true;
			this.linkCambioPass.BackColor = System.Drawing.SystemColors.MenuBar;
			this.linkCambioPass.Location = new System.Drawing.Point(116, 53);
			this.linkCambioPass.Name = "linkCambioPass";
			this.linkCambioPass.Size = new System.Drawing.Size(102, 13);
			this.linkCambioPass.TabIndex = 6;
			this.linkCambioPass.TabStop = true;
			this.linkCambioPass.Text = "Cambiar Contraseña";
			this.linkCambioPass.Visible = false;
			this.linkCambioPass.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCambioPass_LinkClicked);
			// 
			// BLogin
			// 
			this.BLogin.Location = new System.Drawing.Point(9, 76);
			this.BLogin.Name = "BLogin";
			this.BLogin.Size = new System.Drawing.Size(238, 33);
			this.BLogin.TabIndex = 3;
			this.BLogin.Text = "Entrar";
			this.BLogin.UseVisualStyleBackColor = true;
			this.BLogin.Click += new System.EventHandler(this.BLogin_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Contraseña";
			// 
			// TBPass
			// 
			this.TBPass.Location = new System.Drawing.Point(113, 50);
			this.TBPass.Name = "TBPass";
			this.TBPass.PasswordChar = '*';
			this.TBPass.Size = new System.Drawing.Size(134, 20);
			this.TBPass.TabIndex = 2;
			this.TBPass.Leave += new System.EventHandler(this.TBPass_Leave);
			this.TBPass.Enter += new System.EventHandler(this.TBPass_Enter);
			// 
			// CBUsuarios
			// 
			this.CBUsuarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBUsuarios.FormattingEnabled = true;
			this.CBUsuarios.Location = new System.Drawing.Point(113, 23);
			this.CBUsuarios.Name = "CBUsuarios";
			this.CBUsuarios.Size = new System.Drawing.Size(134, 21);
			this.CBUsuarios.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Usuario";
			// 
			// PanelLogin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.GBMain);
			this.Name = "PanelLogin";
			this.Size = new System.Drawing.Size(264, 127);
			this.GBMain.ResumeLayout(false);
			this.GBMain.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox GBMain;
		private System.Windows.Forms.Button BLogin;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TBPass;
		private System.Windows.Forms.ComboBox CBUsuarios;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkCambioPass;
	}
}
