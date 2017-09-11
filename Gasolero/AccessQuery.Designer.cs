namespace Gasolero
{
	partial class AccessQuery
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.bBuscarVales = new System.Windows.Forms.Button();
			this.dtpValesHta = new System.Windows.Forms.DateTimePicker();
			this.dtpValesDde = new System.Windows.Forms.DateTimePicker();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.numId = new System.Windows.Forms.NumericUpDown();
			this.bBuscarPorId = new System.Windows.Forms.Button();
			this.pnlABM = new System.Windows.Forms.Panel();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numId)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.bBuscarVales);
			this.groupBox1.Controls.Add(this.dtpValesHta);
			this.groupBox1.Controls.Add(this.dtpValesDde);
			this.groupBox1.Location = new System.Drawing.Point(12, 348);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(466, 55);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Ver Vales";
			// 
			// bBuscarVales
			// 
			this.bBuscarVales.Location = new System.Drawing.Point(363, 26);
			this.bBuscarVales.Name = "bBuscarVales";
			this.bBuscarVales.Size = new System.Drawing.Size(97, 23);
			this.bBuscarVales.TabIndex = 2;
			this.bBuscarVales.Text = "Buscar";
			this.bBuscarVales.UseVisualStyleBackColor = true;
			this.bBuscarVales.Click += new System.EventHandler(this.bBuscarVales_Click);
			// 
			// dtpValesHta
			// 
			this.dtpValesHta.CustomFormat = "dd/MM/yyyy HH:mm:ss";
			this.dtpValesHta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpValesHta.Location = new System.Drawing.Point(180, 29);
			this.dtpValesHta.Name = "dtpValesHta";
			this.dtpValesHta.Size = new System.Drawing.Size(177, 20);
			this.dtpValesHta.TabIndex = 1;
			// 
			// dtpValesDde
			// 
			this.dtpValesDde.CustomFormat = "dd/MM/yyyy HH:mm:ss";
			this.dtpValesDde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpValesDde.Location = new System.Drawing.Point(6, 29);
			this.dtpValesDde.Name = "dtpValesDde";
			this.dtpValesDde.Size = new System.Drawing.Size(168, 20);
			this.dtpValesDde.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.bBuscarPorId);
			this.groupBox2.Controls.Add(this.numId);
			this.groupBox2.Location = new System.Drawing.Point(484, 348);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(207, 55);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Vales para Imprimir";
			// 
			// numId
			// 
			this.numId.Location = new System.Drawing.Point(6, 29);
			this.numId.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numId.Name = "numId";
			this.numId.Size = new System.Drawing.Size(92, 20);
			this.numId.TabIndex = 0;
			this.numId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// bBuscarPorId
			// 
			this.bBuscarPorId.Location = new System.Drawing.Point(104, 26);
			this.bBuscarPorId.Name = "bBuscarPorId";
			this.bBuscarPorId.Size = new System.Drawing.Size(97, 23);
			this.bBuscarPorId.TabIndex = 3;
			this.bBuscarPorId.Text = "Buscar";
			this.bBuscarPorId.UseVisualStyleBackColor = true;
			this.bBuscarPorId.Click += new System.EventHandler(this.bBuscarPorId_Click);
			// 
			// pnlABM
			// 
			this.pnlABM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlABM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlABM.Location = new System.Drawing.Point(12, 12);
			this.pnlABM.Name = "pnlABM";
			this.pnlABM.Size = new System.Drawing.Size(843, 330);
			this.pnlABM.TabIndex = 3;
			// 
			// AccessQuery
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(867, 415);
			this.Controls.Add(this.pnlABM);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "AccessQuery";
			this.Text = "AccessQuery";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numId)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DateTimePicker dtpValesHta;
		private System.Windows.Forms.DateTimePicker dtpValesDde;
		private System.Windows.Forms.Button bBuscarVales;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button bBuscarPorId;
		private System.Windows.Forms.NumericUpDown numId;
		private System.Windows.Forms.Panel pnlABM;
	}
}