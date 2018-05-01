namespace DataWolf
{
	partial class InfoOkno
	{

		private System.ComponentModel.IContainer components = null;
		protected System.Windows.Forms.Button button2;
		protected System.Windows.Forms.Button button1;

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		private void InitializeComponent()
		{
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(15, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(90, 25);
			this.button2.TabIndex = 7;
			this.button2.Text = "Nový";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(125, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 25);
			this.button1.TabIndex = 8;
			this.button1.Text = "Ulož";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// InfoOkno
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 361);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button2);
			this.Name = "InfoOkno";
			this.Text = "InfoOkno";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InfoOknoFormClosing);
			this.Load += new System.EventHandler(this.InfoOknoLoad);
			this.ResumeLayout(false);

		}
	}
}
