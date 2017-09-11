namespace Gasolero
{
	partial class Planilla
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
			this.PanelListado = new System.Windows.Forms.Panel();
			this.BImprimir = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// PanelListado
			// 
			this.PanelListado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PanelListado.AutoScroll = true;
			this.PanelListado.Location = new System.Drawing.Point(0, 0);
			this.PanelListado.Name = "PanelListado";
			this.PanelListado.Size = new System.Drawing.Size(621, 285);
			this.PanelListado.TabIndex = 0;
			// 
			// BImprimir
			// 
			this.BImprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.BImprimir.Location = new System.Drawing.Point(0, 291);
			this.BImprimir.Name = "BImprimir";
			this.BImprimir.Size = new System.Drawing.Size(75, 23);
			this.BImprimir.TabIndex = 1;
			this.BImprimir.Text = "Imprimir";
			this.BImprimir.UseVisualStyleBackColor = true;
			this.BImprimir.Click += new System.EventHandler(this.BImprimir_Click);
			// 
			// Planilla
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(621, 319);
			this.Controls.Add(this.BImprimir);
			this.Controls.Add(this.PanelListado);
			this.Name = "Planilla";
			this.Text = "Planilla";
			this.Load += new System.EventHandler(this.Planilla_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel PanelListado;
		private System.Windows.Forms.Button BImprimir;
	}
}