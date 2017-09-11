namespace Gasolero
{
	partial class CGastos
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
			this.gbGasto = new System.Windows.Forms.GroupBox();
			this.bCargar = new System.Windows.Forms.Button();
			this.cbUsuarioReceptor = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.numMonto = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.lblFecha = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.dtpFecha = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.cbConcepto = new System.Windows.Forms.ComboBox();
			this.gbFactura = new System.Windows.Forms.GroupBox();
			this.label21 = new System.Windows.Forms.Label();
			this.dtpFactFecha = new System.Windows.Forms.DateTimePicker();
			this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
			this.tbNumero = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.tbTipo = new System.Windows.Forms.TextBox();
			this.tbEmpresa = new System.Windows.Forms.TextBox();
			this.numExento = new System.Windows.Forms.NumericUpDown();
			this.label18 = new System.Windows.Forms.Label();
			this.numGanancias = new System.Windows.Forms.NumericUpDown();
			this.label19 = new System.Windows.Forms.Label();
			this.numPerIB = new System.Windows.Forms.NumericUpDown();
			this.label16 = new System.Windows.Forms.Label();
			this.numPerIVA = new System.Windows.Forms.NumericUpDown();
			this.label17 = new System.Windows.Forms.Label();
			this.numNG27 = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.numNG21 = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.numNG105 = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.numIVA27 = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.numIVA21 = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.numIVA105 = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.radIngreso = new System.Windows.Forms.RadioButton();
			this.radEgreso = new System.Windows.Forms.RadioButton();
			this.radTransferencia = new System.Windows.Forms.RadioButton();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label22 = new System.Windows.Forms.Label();
			this.gbGasto.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMonto)).BeginInit();
			this.gbFactura.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numExento)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numGanancias)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPerIB)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPerIVA)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numNG27)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numNG21)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numNG105)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numIVA27)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numIVA21)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numIVA105)).BeginInit();
			this.SuspendLayout();
			// 
			// gbGasto
			// 
			this.gbGasto.Controls.Add(this.comboBox1);
			this.gbGasto.Controls.Add(this.label22);
			this.gbGasto.Controls.Add(this.label4);
			this.gbGasto.Controls.Add(this.radTransferencia);
			this.gbGasto.Controls.Add(this.radEgreso);
			this.gbGasto.Controls.Add(this.radIngreso);
			this.gbGasto.Controls.Add(this.bCargar);
			this.gbGasto.Controls.Add(this.cbUsuarioReceptor);
			this.gbGasto.Controls.Add(this.numMonto);
			this.gbGasto.Controls.Add(this.label2);
			this.gbGasto.Controls.Add(this.lblFecha);
			this.gbGasto.Controls.Add(this.label3);
			this.gbGasto.Controls.Add(this.textBox1);
			this.gbGasto.Controls.Add(this.dtpFecha);
			this.gbGasto.Controls.Add(this.label1);
			this.gbGasto.Controls.Add(this.cbConcepto);
			this.gbGasto.Location = new System.Drawing.Point(3, 3);
			this.gbGasto.Name = "gbGasto";
			this.gbGasto.Size = new System.Drawing.Size(609, 123);
			this.gbGasto.TabIndex = 0;
			this.gbGasto.TabStop = false;
			this.gbGasto.Text = "Carga de Movimiento";
			// 
			// bCargar
			// 
			this.bCargar.Location = new System.Drawing.Point(418, 12);
			this.bCargar.Name = "bCargar";
			this.bCargar.Size = new System.Drawing.Size(183, 105);
			this.bCargar.TabIndex = 5;
			this.bCargar.Text = "Cargar";
			this.bCargar.UseVisualStyleBackColor = true;
			this.bCargar.Click += new System.EventHandler(this.bCargar_Click);
			// 
			// cbUsuarioReceptor
			// 
			this.cbUsuarioReceptor.FormattingEnabled = true;
			this.cbUsuarioReceptor.Location = new System.Drawing.Point(251, 67);
			this.cbUsuarioReceptor.Name = "cbUsuarioReceptor";
			this.cbUsuarioReceptor.Size = new System.Drawing.Size(158, 21);
			this.cbUsuarioReceptor.TabIndex = 4;
			this.cbUsuarioReceptor.Visible = false;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(200, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(46, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Trans. a";
			this.label4.Visible = false;
			// 
			// numMonto
			// 
			this.numMonto.DecimalPlaces = 2;
			this.numMonto.Location = new System.Drawing.Point(251, 97);
			this.numMonto.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numMonto.Name = "numMonto";
			this.numMonto.Size = new System.Drawing.Size(158, 20);
			this.numMonto.TabIndex = 3;
			this.numMonto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(205, 99);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Monto";
			// 
			// lblFecha
			// 
			this.lblFecha.AutoSize = true;
			this.lblFecha.Location = new System.Drawing.Point(7, 40);
			this.lblFecha.Name = "lblFecha";
			this.lblFecha.Size = new System.Drawing.Size(37, 13);
			this.lblFecha.TabIndex = 6;
			this.lblFecha.TabStop = true;
			this.lblFecha.Text = "Fecha";
			this.lblFecha.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblFecha_LinkClicked);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(205, 43);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Detalle";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(251, 40);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(158, 20);
			this.textBox1.TabIndex = 2;
			// 
			// dtpFecha
			// 
			this.dtpFecha.Enabled = false;
			this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpFecha.Location = new System.Drawing.Point(66, 39);
			this.dtpFecha.Name = "dtpFecha";
			this.dtpFecha.Size = new System.Drawing.Size(121, 20);
			this.dtpFecha.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 99);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Concepto";
			// 
			// cbConcepto
			// 
			this.cbConcepto.FormattingEnabled = true;
			this.cbConcepto.Location = new System.Drawing.Point(66, 96);
			this.cbConcepto.Name = "cbConcepto";
			this.cbConcepto.Size = new System.Drawing.Size(121, 21);
			this.cbConcepto.TabIndex = 1;
			// 
			// gbFactura
			// 
			this.gbFactura.Controls.Add(this.label21);
			this.gbFactura.Controls.Add(this.dtpFactFecha);
			this.gbFactura.Controls.Add(this.maskedTextBox1);
			this.gbFactura.Controls.Add(this.tbNumero);
			this.gbFactura.Controls.Add(this.label20);
			this.gbFactura.Controls.Add(this.tbTipo);
			this.gbFactura.Controls.Add(this.tbEmpresa);
			this.gbFactura.Controls.Add(this.numExento);
			this.gbFactura.Controls.Add(this.label18);
			this.gbFactura.Controls.Add(this.numGanancias);
			this.gbFactura.Controls.Add(this.label19);
			this.gbFactura.Controls.Add(this.numPerIB);
			this.gbFactura.Controls.Add(this.label16);
			this.gbFactura.Controls.Add(this.numPerIVA);
			this.gbFactura.Controls.Add(this.label17);
			this.gbFactura.Controls.Add(this.numNG27);
			this.gbFactura.Controls.Add(this.label8);
			this.gbFactura.Controls.Add(this.numNG21);
			this.gbFactura.Controls.Add(this.label9);
			this.gbFactura.Controls.Add(this.numNG105);
			this.gbFactura.Controls.Add(this.label10);
			this.gbFactura.Controls.Add(this.label15);
			this.gbFactura.Controls.Add(this.label14);
			this.gbFactura.Controls.Add(this.label11);
			this.gbFactura.Controls.Add(this.label12);
			this.gbFactura.Controls.Add(this.label13);
			this.gbFactura.Controls.Add(this.numIVA27);
			this.gbFactura.Controls.Add(this.label7);
			this.gbFactura.Controls.Add(this.numIVA21);
			this.gbFactura.Controls.Add(this.label6);
			this.gbFactura.Controls.Add(this.numIVA105);
			this.gbFactura.Controls.Add(this.label5);
			this.gbFactura.Location = new System.Drawing.Point(3, 132);
			this.gbFactura.Name = "gbFactura";
			this.gbFactura.Size = new System.Drawing.Size(609, 126);
			this.gbFactura.TabIndex = 1;
			this.gbFactura.TabStop = false;
			this.gbFactura.Text = "Datos de Factura";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(6, 99);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(37, 13);
			this.label21.TabIndex = 48;
			this.label21.Text = "Fecha";
			// 
			// dtpFactFecha
			// 
			this.dtpFactFecha.Enabled = false;
			this.dtpFactFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpFactFecha.Location = new System.Drawing.Point(60, 95);
			this.dtpFactFecha.Name = "dtpFactFecha";
			this.dtpFactFecha.Size = new System.Drawing.Size(121, 20);
			this.dtpFactFecha.TabIndex = 10;
			// 
			// maskedTextBox1
			// 
			this.maskedTextBox1.Location = new System.Drawing.Point(60, 70);
			this.maskedTextBox1.Mask = "00-00000000-0";
			this.maskedTextBox1.Name = "maskedTextBox1";
			this.maskedTextBox1.Size = new System.Drawing.Size(177, 20);
			this.maskedTextBox1.TabIndex = 47;
			// 
			// tbNumero
			// 
			this.tbNumero.Location = new System.Drawing.Point(155, 18);
			this.tbNumero.Name = "tbNumero";
			this.tbNumero.Size = new System.Drawing.Size(82, 20);
			this.tbNumero.TabIndex = 46;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(112, 21);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(19, 13);
			this.label20.TabIndex = 45;
			this.label20.Text = "Nº";
			// 
			// tbTipo
			// 
			this.tbTipo.Location = new System.Drawing.Point(60, 18);
			this.tbTipo.Name = "tbTipo";
			this.tbTipo.Size = new System.Drawing.Size(46, 20);
			this.tbTipo.TabIndex = 44;
			// 
			// tbEmpresa
			// 
			this.tbEmpresa.Location = new System.Drawing.Point(60, 44);
			this.tbEmpresa.Name = "tbEmpresa";
			this.tbEmpresa.Size = new System.Drawing.Size(177, 20);
			this.tbEmpresa.TabIndex = 10;
			// 
			// numExento
			// 
			this.numExento.DecimalPlaces = 2;
			this.numExento.Location = new System.Drawing.Point(532, 97);
			this.numExento.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numExento.Name = "numExento";
			this.numExento.Size = new System.Drawing.Size(68, 20);
			this.numExento.TabIndex = 42;
			this.numExento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(254, 99);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(58, 13);
			this.label18.TabIndex = 43;
			this.label18.Text = "Ganancias";
			// 
			// numGanancias
			// 
			this.numGanancias.DecimalPlaces = 2;
			this.numGanancias.Location = new System.Drawing.Point(318, 97);
			this.numGanancias.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numGanancias.Name = "numGanancias";
			this.numGanancias.Size = new System.Drawing.Size(68, 20);
			this.numGanancias.TabIndex = 40;
			this.numGanancias.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(450, 99);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(76, 13);
			this.label19.TabIndex = 41;
			this.label19.Text = "Exento/No Gr.";
			// 
			// numPerIB
			// 
			this.numPerIB.DecimalPlaces = 2;
			this.numPerIB.Location = new System.Drawing.Point(532, 71);
			this.numPerIB.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numPerIB.Name = "numPerIB";
			this.numPerIB.Size = new System.Drawing.Size(68, 20);
			this.numPerIB.TabIndex = 38;
			this.numPerIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(477, 73);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(49, 13);
			this.label16.TabIndex = 39;
			this.label16.Text = "Per. IIBB";
			// 
			// numPerIVA
			// 
			this.numPerIVA.DecimalPlaces = 2;
			this.numPerIVA.Location = new System.Drawing.Point(318, 71);
			this.numPerIVA.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numPerIVA.Name = "numPerIVA";
			this.numPerIVA.Size = new System.Drawing.Size(68, 20);
			this.numPerIVA.TabIndex = 36;
			this.numPerIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(266, 73);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(46, 13);
			this.label17.TabIndex = 37;
			this.label17.Text = "Per. IVA";
			// 
			// numNG27
			// 
			this.numNG27.DecimalPlaces = 2;
			this.numNG27.Location = new System.Drawing.Point(532, 45);
			this.numNG27.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numNG27.Name = "numNG27";
			this.numNG27.Size = new System.Drawing.Size(68, 20);
			this.numNG27.TabIndex = 34;
			this.numNG27.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(499, 47);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(27, 13);
			this.label8.TabIndex = 35;
			this.label8.Text = "27%";
			// 
			// numNG21
			// 
			this.numNG21.DecimalPlaces = 2;
			this.numNG21.Location = new System.Drawing.Point(425, 45);
			this.numNG21.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numNG21.Name = "numNG21";
			this.numNG21.Size = new System.Drawing.Size(68, 20);
			this.numNG21.TabIndex = 32;
			this.numNG21.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(392, 47);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(27, 13);
			this.label9.TabIndex = 33;
			this.label9.Text = "21%";
			// 
			// numNG105
			// 
			this.numNG105.DecimalPlaces = 2;
			this.numNG105.Location = new System.Drawing.Point(318, 45);
			this.numNG105.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numNG105.Name = "numNG105";
			this.numNG105.Size = new System.Drawing.Size(68, 20);
			this.numNG105.TabIndex = 30;
			this.numNG105.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(276, 47);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(36, 13);
			this.label10.TabIndex = 31;
			this.label10.Text = "10,5%";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(243, 47);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(26, 13);
			this.label15.TabIndex = 29;
			this.label15.Text = "NG:";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(243, 21);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(27, 13);
			this.label14.TabIndex = 28;
			this.label14.Text = "IVA:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 73);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(32, 13);
			this.label11.TabIndex = 27;
			this.label11.Text = "CUIT";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(6, 47);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(48, 13);
			this.label12.TabIndex = 25;
			this.label12.Text = "Empresa";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(6, 21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(28, 13);
			this.label13.TabIndex = 23;
			this.label13.Text = "Tipo";
			// 
			// numIVA27
			// 
			this.numIVA27.DecimalPlaces = 2;
			this.numIVA27.Location = new System.Drawing.Point(532, 19);
			this.numIVA27.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numIVA27.Name = "numIVA27";
			this.numIVA27.Size = new System.Drawing.Size(68, 20);
			this.numIVA27.TabIndex = 14;
			this.numIVA27.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(499, 21);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(27, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "27%";
			// 
			// numIVA21
			// 
			this.numIVA21.DecimalPlaces = 2;
			this.numIVA21.Location = new System.Drawing.Point(425, 19);
			this.numIVA21.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numIVA21.Name = "numIVA21";
			this.numIVA21.Size = new System.Drawing.Size(68, 20);
			this.numIVA21.TabIndex = 12;
			this.numIVA21.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(392, 21);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(27, 13);
			this.label6.TabIndex = 13;
			this.label6.Text = "21%";
			// 
			// numIVA105
			// 
			this.numIVA105.DecimalPlaces = 2;
			this.numIVA105.Location = new System.Drawing.Point(318, 19);
			this.numIVA105.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
			this.numIVA105.Name = "numIVA105";
			this.numIVA105.Size = new System.Drawing.Size(68, 20);
			this.numIVA105.TabIndex = 10;
			this.numIVA105.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(276, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(36, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "10,5%";
			// 
			// radIngreso
			// 
			this.radIngreso.AutoSize = true;
			this.radIngreso.Location = new System.Drawing.Point(9, 19);
			this.radIngreso.Name = "radIngreso";
			this.radIngreso.Size = new System.Drawing.Size(60, 17);
			this.radIngreso.TabIndex = 10;
			this.radIngreso.TabStop = true;
			this.radIngreso.Text = "Ingreso";
			this.radIngreso.UseVisualStyleBackColor = true;
			// 
			// radEgreso
			// 
			this.radEgreso.AutoSize = true;
			this.radEgreso.Location = new System.Drawing.Point(288, 19);
			this.radEgreso.Name = "radEgreso";
			this.radEgreso.Size = new System.Drawing.Size(58, 17);
			this.radEgreso.TabIndex = 11;
			this.radEgreso.TabStop = true;
			this.radEgreso.Text = "Egreso";
			this.radEgreso.UseVisualStyleBackColor = true;
			// 
			// radTransferencia
			// 
			this.radTransferencia.AutoSize = true;
			this.radTransferencia.Location = new System.Drawing.Point(130, 19);
			this.radTransferencia.Name = "radTransferencia";
			this.radTransferencia.Size = new System.Drawing.Size(90, 17);
			this.radTransferencia.TabIndex = 12;
			this.radTransferencia.TabStop = true;
			this.radTransferencia.Text = "Transferencia";
			this.radTransferencia.UseVisualStyleBackColor = true;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(66, 67);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 13;
			this.comboBox1.Visible = false;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(10, 70);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(38, 13);
			this.label22.TabIndex = 14;
			this.label22.Text = "Desde";
			this.label22.Visible = false;
			// 
			// CGastos
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gbFactura);
			this.Controls.Add(this.gbGasto);
			this.Name = "CGastos";
			this.Size = new System.Drawing.Size(1045, 494);
			this.gbGasto.ResumeLayout(false);
			this.gbGasto.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMonto)).EndInit();
			this.gbFactura.ResumeLayout(false);
			this.gbFactura.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numExento)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numGanancias)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPerIB)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPerIVA)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numNG27)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numNG21)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numNG105)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numIVA27)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numIVA21)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numIVA105)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbGasto;
		private System.Windows.Forms.ComboBox cbUsuarioReceptor;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numMonto;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel lblFecha;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.DateTimePicker dtpFecha;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbConcepto;
		private System.Windows.Forms.Button bCargar;
		private System.Windows.Forms.GroupBox gbFactura;
		private System.Windows.Forms.NumericUpDown numPerIB;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.NumericUpDown numPerIVA;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.NumericUpDown numNG27;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown numNG21;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown numNG105;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.NumericUpDown numIVA27;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown numIVA21;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numIVA105;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown numExento;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.NumericUpDown numGanancias;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.DateTimePicker dtpFactFecha;
		private System.Windows.Forms.MaskedTextBox maskedTextBox1;
		private System.Windows.Forms.TextBox tbNumero;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox tbTipo;
		private System.Windows.Forms.TextBox tbEmpresa;
		private System.Windows.Forms.RadioButton radTransferencia;
		private System.Windows.Forms.RadioButton radEgreso;
		private System.Windows.Forms.RadioButton radIngreso;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label22;
	}
}
