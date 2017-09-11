namespace Gasolero
{
	partial class FrmPreg
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
			this.bOpc1 = new System.Windows.Forms.Button();
			this.bOpc2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// bOpc1
			// 
			this.bOpc1.Location = new System.Drawing.Point(12, 12);
			this.bOpc1.Name = "bOpc1";
			this.bOpc1.Size = new System.Drawing.Size(114, 40);
			this.bOpc1.TabIndex = 0;
			this.bOpc1.Text = "Cierre de Turno";
			this.bOpc1.UseVisualStyleBackColor = true;
			this.bOpc1.Click += new System.EventHandler(this.bOpc1_Click);
			// 
			// bOpc2
			// 
			this.bOpc2.Location = new System.Drawing.Point(148, 12);
			this.bOpc2.Name = "bOpc2";
			this.bOpc2.Size = new System.Drawing.Size(114, 40);
			this.bOpc2.TabIndex = 1;
			this.bOpc2.Text = "Cierre Parcial";
			this.bOpc2.UseVisualStyleBackColor = true;
			this.bOpc2.Click += new System.EventHandler(this.bOpc2_Click);
			// 
			// FrmPreg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(274, 65);
			this.Controls.Add(this.bOpc2);
			this.Controls.Add(this.bOpc1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmPreg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "?";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button bOpc1;
		private System.Windows.Forms.Button bOpc2;
	}
}