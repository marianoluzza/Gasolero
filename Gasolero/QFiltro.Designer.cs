namespace Gasolero
{
	partial class QFiltro
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
			this.GBFiltro = new System.Windows.Forms.GroupBox();
			this.BCancelar = new System.Windows.Forms.Button();
			this.BAceptar = new System.Windows.Forms.Button();
			this.lbl = new System.Windows.Forms.Label();
			this.TBFiltro = new System.Windows.Forms.TextBox();
			this.lblMensaje = new System.Windows.Forms.Label();
			this.GBFiltro.SuspendLayout();
			this.SuspendLayout();
			// 
			// GBFiltro
			// 
			this.GBFiltro.Controls.Add(this.lblMensaje);
			this.GBFiltro.Controls.Add(this.BCancelar);
			this.GBFiltro.Controls.Add(this.BAceptar);
			this.GBFiltro.Controls.Add(this.lbl);
			this.GBFiltro.Controls.Add(this.TBFiltro);
			this.GBFiltro.Location = new System.Drawing.Point(3, 3);
			this.GBFiltro.Name = "GBFiltro";
			this.GBFiltro.Size = new System.Drawing.Size(176, 175);
			this.GBFiltro.TabIndex = 0;
			this.GBFiltro.TabStop = false;
			this.GBFiltro.Text = "Filtro";
			// 
			// BCancelar
			// 
			this.BCancelar.Location = new System.Drawing.Point(6, 122);
			this.BCancelar.Name = "BCancelar";
			this.BCancelar.Size = new System.Drawing.Size(164, 27);
			this.BCancelar.TabIndex = 3;
			this.BCancelar.Text = "Cancelar";
			this.BCancelar.UseVisualStyleBackColor = true;
			this.BCancelar.Click += new System.EventHandler(this.BCancelar_Click);
			// 
			// BAceptar
			// 
			this.BAceptar.Location = new System.Drawing.Point(6, 89);
			this.BAceptar.Name = "BAceptar";
			this.BAceptar.Size = new System.Drawing.Size(164, 27);
			this.BAceptar.TabIndex = 2;
			this.BAceptar.Text = "Aceptar";
			this.BAceptar.UseVisualStyleBackColor = true;
			this.BAceptar.Click += new System.EventHandler(this.BAceptar_Click);
			// 
			// lbl
			// 
			this.lbl.AutoSize = true;
			this.lbl.Location = new System.Drawing.Point(6, 42);
			this.lbl.Name = "lbl";
			this.lbl.Size = new System.Drawing.Size(35, 13);
			this.lbl.TabIndex = 1;
			this.lbl.Text = "label1";
			// 
			// TBFiltro
			// 
			this.TBFiltro.Location = new System.Drawing.Point(6, 58);
			this.TBFiltro.Name = "TBFiltro";
			this.TBFiltro.Size = new System.Drawing.Size(164, 20);
			this.TBFiltro.TabIndex = 0;
			this.TBFiltro.TextChanged += new System.EventHandler(this.TBFiltro_TextChanged);
			this.TBFiltro.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TBFiltro_KeyDown);
			// 
			// lblMensaje
			// 
			this.lblMensaje.AutoSize = true;
			this.lblMensaje.Location = new System.Drawing.Point(6, 152);
			this.lblMensaje.Name = "lblMensaje";
			this.lblMensaje.Size = new System.Drawing.Size(37, 13);
			this.lblMensaje.TabIndex = 4;
			this.lblMensaje.Text = "----------";
			// 
			// QFiltro
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.GBFiltro);
			this.Name = "QFiltro";
			this.Size = new System.Drawing.Size(182, 181);
			this.Load += new System.EventHandler(this.QFiltro_Load);
			this.GBFiltro.ResumeLayout(false);
			this.GBFiltro.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox GBFiltro;
		private System.Windows.Forms.Label lbl;
		private System.Windows.Forms.TextBox TBFiltro;
		private System.Windows.Forms.Button BCancelar;
		private System.Windows.Forms.Button BAceptar;
		private System.Windows.Forms.Label lblMensaje;
	}
}
