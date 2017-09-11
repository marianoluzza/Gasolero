namespace Gasolero
{
	partial class FactFiltro
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
			this.label2 = new System.Windows.Forms.Label();
			this.gbCliente = new System.Windows.Forms.GroupBox();
			this.radExtra = new System.Windows.Forms.RadioButton();
			this.txtClienteNombre = new System.Windows.Forms.TextBox();
			this.radClienteBuscar = new System.Windows.Forms.RadioButton();
			this.radClientesTodos = new System.Windows.Forms.RadioButton();
			this.gbFecha = new System.Windows.Forms.GroupBox();
			this.dtpHasta = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.dtpDesde = new System.Windows.Forms.DateTimePicker();
			this.bFiltrar = new System.Windows.Forms.Button();
			this.gbDetalle = new System.Windows.Forms.GroupBox();
			this.lblDetalle = new System.Windows.Forms.Label();
			this.bInforme = new System.Windows.Forms.Button();
			this.gbCliente.SuspendLayout();
			this.gbFecha.SuspendLayout();
			this.gbDetalle.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Desde";
			// 
			// gbCliente
			// 
			this.gbCliente.Controls.Add(this.radExtra);
			this.gbCliente.Controls.Add(this.txtClienteNombre);
			this.gbCliente.Controls.Add(this.radClienteBuscar);
			this.gbCliente.Controls.Add(this.radClientesTodos);
			this.gbCliente.Location = new System.Drawing.Point(3, 88);
			this.gbCliente.Name = "gbCliente";
			this.gbCliente.Size = new System.Drawing.Size(168, 68);
			this.gbCliente.TabIndex = 2;
			this.gbCliente.TabStop = false;
			this.gbCliente.Text = "Cliente";
			// 
			// radExtra
			// 
			this.radExtra.AutoSize = true;
			this.radExtra.Location = new System.Drawing.Point(6, 46);
			this.radExtra.Name = "radExtra";
			this.radExtra.Size = new System.Drawing.Size(41, 17);
			this.radExtra.TabIndex = 3;
			this.radExtra.Text = "NO";
			this.radExtra.UseVisualStyleBackColor = true;
			this.radExtra.Visible = false;
			this.radExtra.CheckedChanged += new System.EventHandler(this.radExtra_CheckedChanged);
			// 
			// txtClienteNombre
			// 
			this.txtClienteNombre.Location = new System.Drawing.Point(6, 45);
			this.txtClienteNombre.Name = "txtClienteNombre";
			this.txtClienteNombre.ReadOnly = true;
			this.txtClienteNombre.Size = new System.Drawing.Size(155, 20);
			this.txtClienteNombre.TabIndex = 2;
			// 
			// radClienteBuscar
			// 
			this.radClienteBuscar.AutoSize = true;
			this.radClienteBuscar.Location = new System.Drawing.Point(6, 28);
			this.radClienteBuscar.Name = "radClienteBuscar";
			this.radClienteBuscar.Size = new System.Drawing.Size(58, 17);
			this.radClienteBuscar.TabIndex = 1;
			this.radClienteBuscar.Text = "Buscar";
			this.radClienteBuscar.UseVisualStyleBackColor = true;
			this.radClienteBuscar.Click += new System.EventHandler(this.radClienteBuscar_Click);
			// 
			// radClientesTodos
			// 
			this.radClientesTodos.AutoSize = true;
			this.radClientesTodos.Checked = true;
			this.radClientesTodos.Location = new System.Drawing.Point(6, 12);
			this.radClientesTodos.Name = "radClientesTodos";
			this.radClientesTodos.Size = new System.Drawing.Size(55, 17);
			this.radClientesTodos.TabIndex = 0;
			this.radClientesTodos.TabStop = true;
			this.radClientesTodos.Text = "Todos";
			this.radClientesTodos.UseVisualStyleBackColor = true;
			this.radClientesTodos.CheckedChanged += new System.EventHandler(this.radClientesTodos_CheckedChanged);
			// 
			// gbFecha
			// 
			this.gbFecha.Controls.Add(this.dtpHasta);
			this.gbFecha.Controls.Add(this.label1);
			this.gbFecha.Controls.Add(this.dtpDesde);
			this.gbFecha.Controls.Add(this.label2);
			this.gbFecha.Location = new System.Drawing.Point(3, 3);
			this.gbFecha.Name = "gbFecha";
			this.gbFecha.Size = new System.Drawing.Size(168, 85);
			this.gbFecha.TabIndex = 3;
			this.gbFecha.TabStop = false;
			this.gbFecha.Text = "Fecha";
			// 
			// dtpHasta
			// 
			this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpHasta.Location = new System.Drawing.Point(6, 61);
			this.dtpHasta.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
			this.dtpHasta.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dtpHasta.Name = "dtpHasta";
			this.dtpHasta.Size = new System.Drawing.Size(155, 20);
			this.dtpHasta.TabIndex = 4;
			this.dtpHasta.ValueChanged += new System.EventHandler(this.dtpHasta_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Hasta";
			// 
			// dtpDesde
			// 
			this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpDesde.Location = new System.Drawing.Point(6, 28);
			this.dtpDesde.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
			this.dtpDesde.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dtpDesde.Name = "dtpDesde";
			this.dtpDesde.Size = new System.Drawing.Size(155, 20);
			this.dtpDesde.TabIndex = 2;
			this.dtpDesde.ValueChanged += new System.EventHandler(this.dtpDesde_ValueChanged);
			// 
			// bFiltrar
			// 
			this.bFiltrar.Location = new System.Drawing.Point(3, 159);
			this.bFiltrar.Name = "bFiltrar";
			this.bFiltrar.Size = new System.Drawing.Size(168, 23);
			this.bFiltrar.TabIndex = 4;
			this.bFiltrar.Text = "Filtrar";
			this.bFiltrar.UseVisualStyleBackColor = true;
			this.bFiltrar.Click += new System.EventHandler(this.bFiltrar_Click);
			// 
			// gbDetalle
			// 
			this.gbDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gbDetalle.Controls.Add(this.lblDetalle);
			this.gbDetalle.Location = new System.Drawing.Point(3, 205);
			this.gbDetalle.Name = "gbDetalle";
			this.gbDetalle.Size = new System.Drawing.Size(168, 83);
			this.gbDetalle.TabIndex = 5;
			this.gbDetalle.TabStop = false;
			this.gbDetalle.Text = "Detalle";
			// 
			// lblDetalle
			// 
			this.lblDetalle.AutoSize = true;
			this.lblDetalle.Location = new System.Drawing.Point(6, 13);
			this.lblDetalle.Name = "lblDetalle";
			this.lblDetalle.Size = new System.Drawing.Size(62, 13);
			this.lblDetalle.TabIndex = 0;
			this.lblDetalle.Text = "Elija factura";
			// 
			// bInforme
			// 
			this.bInforme.Location = new System.Drawing.Point(3, 184);
			this.bInforme.Name = "bInforme";
			this.bInforme.Size = new System.Drawing.Size(168, 23);
			this.bInforme.TabIndex = 6;
			this.bInforme.Text = "Informe";
			this.bInforme.UseVisualStyleBackColor = true;
			this.bInforme.Click += new System.EventHandler(this.bInforme_Click);
			// 
			// FactFiltro
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.bInforme);
			this.Controls.Add(this.gbDetalle);
			this.Controls.Add(this.bFiltrar);
			this.Controls.Add(this.gbFecha);
			this.Controls.Add(this.gbCliente);
			this.Name = "FactFiltro";
			this.Size = new System.Drawing.Size(177, 288);
			this.gbCliente.ResumeLayout(false);
			this.gbCliente.PerformLayout();
			this.gbFecha.ResumeLayout(false);
			this.gbFecha.PerformLayout();
			this.gbDetalle.ResumeLayout(false);
			this.gbDetalle.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox gbCliente;
		private System.Windows.Forms.GroupBox gbFecha;
		private System.Windows.Forms.DateTimePicker dtpHasta;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DateTimePicker dtpDesde;
		private System.Windows.Forms.TextBox txtClienteNombre;
		private System.Windows.Forms.RadioButton radClienteBuscar;
		private System.Windows.Forms.RadioButton radClientesTodos;
		private System.Windows.Forms.Button bFiltrar;
		private System.Windows.Forms.GroupBox gbDetalle;
		private System.Windows.Forms.Label lblDetalle;
		private System.Windows.Forms.RadioButton radExtra;
		private System.Windows.Forms.Button bInforme;
	}
}
