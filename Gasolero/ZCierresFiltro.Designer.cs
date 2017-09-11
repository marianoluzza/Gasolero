namespace Gasolero
{
	partial class ZCierresFiltro
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
			this.components = new System.ComponentModel.Container();
			this.gbFiltro = new System.Windows.Forms.GroupBox();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.refrescarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dtpHta = new System.Windows.Forms.DateTimePicker();
			this.dtpDde = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.bReportar = new System.Windows.Forms.Button();
			this.chbAbrirAlCrear = new System.Windows.Forms.CheckBox();
			this.gbPeriodo = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.radFechaHta = new System.Windows.Forms.RadioButton();
			this.contextMenuPeriodo = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.hoyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label3 = new System.Windows.Forms.Label();
			this.radFechaDde = new System.Windows.Forms.RadioButton();
			this.gbNombre = new System.Windows.Forms.GroupBox();
			this.tbNombre = new System.Windows.Forms.TextBox();
			this.bReporte2 = new System.Windows.Forms.Button();
			this.bReporte3 = new System.Windows.Forms.Button();
			this.bReporte4 = new System.Windows.Forms.Button();
			this.gbFiltro.SuspendLayout();
			this.contextMenu.SuspendLayout();
			this.gbPeriodo.SuspendLayout();
			this.contextMenuPeriodo.SuspendLayout();
			this.gbNombre.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbFiltro
			// 
			this.gbFiltro.ContextMenuStrip = this.contextMenu;
			this.gbFiltro.Controls.Add(this.dtpHta);
			this.gbFiltro.Controls.Add(this.dtpDde);
			this.gbFiltro.Controls.Add(this.label2);
			this.gbFiltro.Controls.Add(this.label1);
			this.gbFiltro.Location = new System.Drawing.Point(3, 3);
			this.gbFiltro.Name = "gbFiltro";
			this.gbFiltro.Size = new System.Drawing.Size(184, 126);
			this.gbFiltro.TabIndex = 0;
			this.gbFiltro.TabStop = false;
			this.gbFiltro.Text = "Filtro de Cierres";
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refrescarToolStripMenuItem});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(123, 26);
			// 
			// refrescarToolStripMenuItem
			// 
			this.refrescarToolStripMenuItem.Name = "refrescarToolStripMenuItem";
			this.refrescarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.refrescarToolStripMenuItem.Text = "Refrescar";
			this.refrescarToolStripMenuItem.Click += new System.EventHandler(this.refrescarToolStripMenuItem_Click);
			// 
			// dtpHta
			// 
			this.dtpHta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpHta.Location = new System.Drawing.Point(6, 92);
			this.dtpHta.Name = "dtpHta";
			this.dtpHta.Size = new System.Drawing.Size(100, 20);
			this.dtpHta.TabIndex = 4;
			this.dtpHta.ValueChanged += new System.EventHandler(this.dtpHta_ValueChanged);
			// 
			// dtpDde
			// 
			this.dtpDde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpDde.Location = new System.Drawing.Point(6, 41);
			this.dtpDde.Name = "dtpDde";
			this.dtpDde.Size = new System.Drawing.Size(100, 20);
			this.dtpDde.TabIndex = 3;
			this.dtpDde.ValueChanged += new System.EventHandler(this.dtpDde_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Hasta";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Desde";
			// 
			// bReportar
			// 
			this.bReportar.Location = new System.Drawing.Point(3, 302);
			this.bReportar.Name = "bReportar";
			this.bReportar.Size = new System.Drawing.Size(184, 34);
			this.bReportar.TabIndex = 3;
			this.bReportar.Text = "Hacer Reporte";
			this.bReportar.UseVisualStyleBackColor = true;
			this.bReportar.Click += new System.EventHandler(this.bReportar_Click);
			// 
			// chbAbrirAlCrear
			// 
			this.chbAbrirAlCrear.AutoSize = true;
			this.chbAbrirAlCrear.Location = new System.Drawing.Point(3, 457);
			this.chbAbrirAlCrear.Name = "chbAbrirAlCrear";
			this.chbAbrirAlCrear.Size = new System.Drawing.Size(85, 17);
			this.chbAbrirAlCrear.TabIndex = 4;
			this.chbAbrirAlCrear.Text = "Abrir al crear";
			this.chbAbrirAlCrear.UseVisualStyleBackColor = true;
			this.chbAbrirAlCrear.CheckedChanged += new System.EventHandler(this.chbAbrirAlCrear_CheckedChanged);
			// 
			// gbPeriodo
			// 
			this.gbPeriodo.Controls.Add(this.label4);
			this.gbPeriodo.Controls.Add(this.radFechaHta);
			this.gbPeriodo.Controls.Add(this.label3);
			this.gbPeriodo.Controls.Add(this.radFechaDde);
			this.gbPeriodo.Location = new System.Drawing.Point(3, 135);
			this.gbPeriodo.Name = "gbPeriodo";
			this.gbPeriodo.Size = new System.Drawing.Size(184, 108);
			this.gbPeriodo.TabIndex = 5;
			this.gbPeriodo.TabStop = false;
			this.gbPeriodo.Text = "Periodo Seleccionado";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(35, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Hasta";
			// 
			// radFechaHta
			// 
			this.radFechaHta.AutoSize = true;
			this.radFechaHta.ContextMenuStrip = this.contextMenuPeriodo;
			this.radFechaHta.Location = new System.Drawing.Point(9, 78);
			this.radFechaHta.Name = "radFechaHta";
			this.radFechaHta.Size = new System.Drawing.Size(88, 17);
			this.radFechaHta.TabIndex = 2;
			this.radFechaHta.Text = "Elija un cierre";
			this.radFechaHta.UseVisualStyleBackColor = true;
			// 
			// contextMenuPeriodo
			// 
			this.contextMenuPeriodo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hoyToolStripMenuItem});
			this.contextMenuPeriodo.Name = "contextMenuPeriodo";
			this.contextMenuPeriodo.Size = new System.Drawing.Size(107, 26);
			// 
			// hoyToolStripMenuItem
			// 
			this.hoyToolStripMenuItem.Name = "hoyToolStripMenuItem";
			this.hoyToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
			this.hoyToolStripMenuItem.Text = "Ahora";
			this.hoyToolStripMenuItem.Click += new System.EventHandler(this.hoyToolStripMenuItem_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Desde";
			// 
			// radFechaDde
			// 
			this.radFechaDde.AutoSize = true;
			this.radFechaDde.Checked = true;
			this.radFechaDde.Location = new System.Drawing.Point(9, 39);
			this.radFechaDde.Name = "radFechaDde";
			this.radFechaDde.Size = new System.Drawing.Size(88, 17);
			this.radFechaDde.TabIndex = 0;
			this.radFechaDde.TabStop = true;
			this.radFechaDde.Text = "Elija un cierre";
			this.radFechaDde.UseVisualStyleBackColor = true;
			// 
			// gbNombre
			// 
			this.gbNombre.Controls.Add(this.tbNombre);
			this.gbNombre.Location = new System.Drawing.Point(3, 249);
			this.gbNombre.Name = "gbNombre";
			this.gbNombre.Size = new System.Drawing.Size(184, 47);
			this.gbNombre.TabIndex = 6;
			this.gbNombre.TabStop = false;
			this.gbNombre.Text = "Nombre";
			// 
			// tbNombre
			// 
			this.tbNombre.Location = new System.Drawing.Point(6, 19);
			this.tbNombre.Name = "tbNombre";
			this.tbNombre.Size = new System.Drawing.Size(172, 20);
			this.tbNombre.TabIndex = 0;
			// 
			// bReporte2
			// 
			this.bReporte2.Location = new System.Drawing.Point(3, 340);
			this.bReporte2.Name = "bReporte2";
			this.bReporte2.Size = new System.Drawing.Size(184, 34);
			this.bReporte2.TabIndex = 7;
			this.bReporte2.Text = "Hacer Reporte";
			this.bReporte2.UseVisualStyleBackColor = true;
			this.bReporte2.Visible = false;
			this.bReporte2.Click += new System.EventHandler(this.bReporte2_Click);
			// 
			// bReporte3
			// 
			this.bReporte3.Location = new System.Drawing.Point(3, 378);
			this.bReporte3.Name = "bReporte3";
			this.bReporte3.Size = new System.Drawing.Size(184, 34);
			this.bReporte3.TabIndex = 8;
			this.bReporte3.Text = "Hacer Reporte";
			this.bReporte3.UseVisualStyleBackColor = true;
			this.bReporte3.Visible = false;
			this.bReporte3.Click += new System.EventHandler(this.bReporte3_Click);
			// 
			// bReporte4
			// 
			this.bReporte4.Location = new System.Drawing.Point(3, 418);
			this.bReporte4.Name = "bReporte4";
			this.bReporte4.Size = new System.Drawing.Size(184, 34);
			this.bReporte4.TabIndex = 9;
			this.bReporte4.Text = "Hacer Reporte";
			this.bReporte4.UseVisualStyleBackColor = true;
			this.bReporte4.Visible = false;
			this.bReporte4.Click += new System.EventHandler(this.bReporte4_Click);
			// 
			// ZCierresFiltro
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.bReporte4);
			this.Controls.Add(this.bReporte3);
			this.Controls.Add(this.chbAbrirAlCrear);
			this.Controls.Add(this.bReporte2);
			this.Controls.Add(this.gbNombre);
			this.Controls.Add(this.gbFiltro);
			this.Controls.Add(this.gbPeriodo);
			this.Controls.Add(this.bReportar);
			this.Name = "ZCierresFiltro";
			this.Size = new System.Drawing.Size(190, 477);
			this.gbFiltro.ResumeLayout(false);
			this.gbFiltro.PerformLayout();
			this.contextMenu.ResumeLayout(false);
			this.gbPeriodo.ResumeLayout(false);
			this.gbPeriodo.PerformLayout();
			this.contextMenuPeriodo.ResumeLayout(false);
			this.gbNombre.ResumeLayout(false);
			this.gbNombre.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox gbFiltro;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem refrescarToolStripMenuItem;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bReportar;
		private System.Windows.Forms.CheckBox chbAbrirAlCrear;
		private System.Windows.Forms.GroupBox gbPeriodo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton radFechaHta;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radFechaDde;
		private System.Windows.Forms.ContextMenuStrip contextMenuPeriodo;
		private System.Windows.Forms.ToolStripMenuItem hoyToolStripMenuItem;
		private System.Windows.Forms.DateTimePicker dtpHta;
		private System.Windows.Forms.DateTimePicker dtpDde;
		private System.Windows.Forms.GroupBox gbNombre;
		private System.Windows.Forms.TextBox tbNombre;
		private System.Windows.Forms.Button bReporte2;
		private System.Windows.Forms.Button bReporte3;
		private System.Windows.Forms.Button bReporte4;
	}
}
