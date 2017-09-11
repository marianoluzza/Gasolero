namespace Gasolero
{
	partial class CEntregas
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
			this.tabs = new System.Windows.Forms.TabControl();
			this.tpFiltro = new System.Windows.Forms.TabPage();
			this.tpDatos = new System.Windows.Forms.TabPage();
			this.lblFecha = new System.Windows.Forms.Label();
			this.lblVendedor = new System.Windows.Forms.Label();
			this.panelDatos = new System.Windows.Forms.Panel();
			this.bGuardar = new System.Windows.Forms.Button();
			this.lbl0 = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.bImprimir = new System.Windows.Forms.Button();
			this.tabs.SuspendLayout();
			this.tpDatos.SuspendLayout();
			this.panelDatos.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.tpFiltro);
			this.tabs.Controls.Add(this.tpDatos);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(330, 370);
			this.tabs.TabIndex = 0;
			this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
			// 
			// tpFiltro
			// 
			this.tpFiltro.Location = new System.Drawing.Point(4, 22);
			this.tpFiltro.Name = "tpFiltro";
			this.tpFiltro.Padding = new System.Windows.Forms.Padding(3);
			this.tpFiltro.Size = new System.Drawing.Size(322, 344);
			this.tpFiltro.TabIndex = 0;
			this.tpFiltro.Text = "Filtro";
			this.tpFiltro.UseVisualStyleBackColor = true;
			// 
			// tpDatos
			// 
			this.tpDatos.Controls.Add(this.bImprimir);
			this.tpDatos.Controls.Add(this.lblFecha);
			this.tpDatos.Controls.Add(this.lblVendedor);
			this.tpDatos.Controls.Add(this.panelDatos);
			this.tpDatos.Location = new System.Drawing.Point(4, 22);
			this.tpDatos.Name = "tpDatos";
			this.tpDatos.Padding = new System.Windows.Forms.Padding(3);
			this.tpDatos.Size = new System.Drawing.Size(322, 344);
			this.tpDatos.TabIndex = 1;
			this.tpDatos.Text = "Datos";
			this.tpDatos.UseVisualStyleBackColor = true;
			// 
			// lblFecha
			// 
			this.lblFecha.AutoSize = true;
			this.lblFecha.Location = new System.Drawing.Point(6, 20);
			this.lblFecha.Name = "lblFecha";
			this.lblFecha.Size = new System.Drawing.Size(59, 13);
			this.lblFecha.TabIndex = 4;
			this.lblFecha.Text = "--/--/---- --:--";
			// 
			// lblVendedor
			// 
			this.lblVendedor.AutoSize = true;
			this.lblVendedor.Location = new System.Drawing.Point(6, 3);
			this.lblVendedor.Name = "lblVendedor";
			this.lblVendedor.Size = new System.Drawing.Size(68, 13);
			this.lblVendedor.TabIndex = 3;
			this.lblVendedor.Text = "Elija un turno";
			// 
			// panelDatos
			// 
			this.panelDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelDatos.AutoScroll = true;
			this.panelDatos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelDatos.Controls.Add(this.bGuardar);
			this.panelDatos.Controls.Add(this.lbl0);
			this.panelDatos.Controls.Add(this.numericUpDown1);
			this.panelDatos.Location = new System.Drawing.Point(3, 36);
			this.panelDatos.Name = "panelDatos";
			this.panelDatos.Size = new System.Drawing.Size(316, 308);
			this.panelDatos.TabIndex = 0;
			// 
			// bGuardar
			// 
			this.bGuardar.Enabled = false;
			this.bGuardar.Location = new System.Drawing.Point(6, 34);
			this.bGuardar.Name = "bGuardar";
			this.bGuardar.Size = new System.Drawing.Size(154, 23);
			this.bGuardar.TabIndex = 2;
			this.bGuardar.Text = "Guardar";
			this.bGuardar.UseVisualStyleBackColor = true;
			this.bGuardar.Click += new System.EventHandler(this.bGuardar_Click);
			// 
			// lbl0
			// 
			this.lbl0.AutoSize = true;
			this.lbl0.Location = new System.Drawing.Point(3, 10);
			this.lbl0.Name = "lbl0";
			this.lbl0.Size = new System.Drawing.Size(63, 13);
			this.lbl0.TabIndex = 0;
			this.lbl0.Text = "Descripcion";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.DecimalPlaces = 2;
			this.numericUpDown1.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numericUpDown1.Location = new System.Drawing.Point(99, 8);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
			this.numericUpDown1.TabIndex = 1;
			this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown1.Value = new decimal(new int[] {
            912399,
            0,
            0,
            131072});
			// 
			// bImprimir
			// 
			this.bImprimir.Enabled = false;
			this.bImprimir.Location = new System.Drawing.Point(134, 6);
			this.bImprimir.Name = "bImprimir";
			this.bImprimir.Size = new System.Drawing.Size(31, 23);
			this.bImprimir.TabIndex = 5;
			this.bImprimir.UseVisualStyleBackColor = true;
			this.bImprimir.Click += new System.EventHandler(this.bImprimir_Click);
			// 
			// CEntregas
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabs);
			this.Name = "CEntregas";
			this.Size = new System.Drawing.Size(330, 370);
			this.tabs.ResumeLayout(false);
			this.tpDatos.ResumeLayout(false);
			this.tpDatos.PerformLayout();
			this.panelDatos.ResumeLayout(false);
			this.panelDatos.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tpFiltro;
		private System.Windows.Forms.TabPage tpDatos;
		private System.Windows.Forms.Panel panelDatos;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label lbl0;
		private System.Windows.Forms.Button bGuardar;
		private System.Windows.Forms.Label lblVendedor;
		private System.Windows.Forms.Label lblFecha;
		private System.Windows.Forms.Button bImprimir;
	}
}
