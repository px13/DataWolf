namespace DataWolf
{
	partial class MainForm
	{
		private System.ComponentModel.IContainer components = null;
		
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.otvorProjektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ulozProjektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.novaFunkciaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.kompilujToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.funkciaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.registreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vstupyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vystupyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.premenujModulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zmazModulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripMenuItem1,
			this.projektToolStripMenuItem,
			this.funkciaToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
			this.menuStrip1.TabIndex = 7;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripMenuItem2,
			this.otvorProjektToolStripMenuItem,
			this.ulozProjektToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(50, 20);
			this.toolStripMenuItem1.Text = "Súbor";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(144, 22);
			this.toolStripMenuItem2.Text = "Nový Projekt";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.NovyProjektDialog);
			// 
			// otvorProjektToolStripMenuItem
			// 
			this.otvorProjektToolStripMenuItem.Name = "otvorProjektToolStripMenuItem";
			this.otvorProjektToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.otvorProjektToolStripMenuItem.Text = "Otvor Projekt";
			this.otvorProjektToolStripMenuItem.Click += new System.EventHandler(this.OtvorProjektDialog);
			// 
			// ulozProjektToolStripMenuItem
			// 
			this.ulozProjektToolStripMenuItem.Name = "ulozProjektToolStripMenuItem";
			this.ulozProjektToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.ulozProjektToolStripMenuItem.Text = "Ulož Projekt";
			this.ulozProjektToolStripMenuItem.Click += new System.EventHandler(this.UlozProjekt);
			// 
			// projektToolStripMenuItem
			// 
			this.projektToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.novaFunkciaToolStripMenuItem,
			this.kompilujToolStripMenuItem});
			this.projektToolStripMenuItem.Name = "projektToolStripMenuItem";
			this.projektToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.projektToolStripMenuItem.Text = "Projekt";
			// 
			// novaFunkciaToolStripMenuItem
			// 
			this.novaFunkciaToolStripMenuItem.Name = "novaFunkciaToolStripMenuItem";
			this.novaFunkciaToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.novaFunkciaToolStripMenuItem.Text = "Nový modul";
			this.novaFunkciaToolStripMenuItem.Click += new System.EventHandler(this.NovyModul);
			// 
			// kompilujToolStripMenuItem
			// 
			this.kompilujToolStripMenuItem.Name = "kompilujToolStripMenuItem";
			this.kompilujToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.kompilujToolStripMenuItem.Text = "Kompiluj";
			this.kompilujToolStripMenuItem.Click += new System.EventHandler(this.KompilujToolStripMenuItemClick);
			// 
			// funkciaToolStripMenuItem
			// 
			this.funkciaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.registreToolStripMenuItem,
			this.vstupyToolStripMenuItem,
			this.vystupyToolStripMenuItem,
			this.premenujModulToolStripMenuItem,
			this.zmazModulToolStripMenuItem});
			this.funkciaToolStripMenuItem.Name = "funkciaToolStripMenuItem";
			this.funkciaToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.funkciaToolStripMenuItem.Text = "Modul";
			// 
			// registreToolStripMenuItem
			// 
			this.registreToolStripMenuItem.Name = "registreToolStripMenuItem";
			this.registreToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.registreToolStripMenuItem.Text = "Registre";
			this.registreToolStripMenuItem.Click += new System.EventHandler(this.RegistreToolStripMenuItemClick);
			// 
			// vstupyToolStripMenuItem
			// 
			this.vstupyToolStripMenuItem.Name = "vstupyToolStripMenuItem";
			this.vstupyToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.vstupyToolStripMenuItem.Text = "Vstupy";
			this.vstupyToolStripMenuItem.Click += new System.EventHandler(this.VstupyToolStripMenuItemClick);
			// 
			// vystupyToolStripMenuItem
			// 
			this.vystupyToolStripMenuItem.Name = "vystupyToolStripMenuItem";
			this.vystupyToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.vystupyToolStripMenuItem.Text = "Výstupy";
			this.vystupyToolStripMenuItem.Click += new System.EventHandler(this.VystupyToolStripMenuItemClick);
			// 
			// premenujModulToolStripMenuItem
			// 
			this.premenujModulToolStripMenuItem.Name = "premenujModulToolStripMenuItem";
			this.premenujModulToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.premenujModulToolStripMenuItem.Text = "Premenuj modul";
			this.premenujModulToolStripMenuItem.Click += new System.EventHandler(this.PremenujModulToolStripMenuItemClick);
			// 
			// zmazModulToolStripMenuItem
			// 
			this.zmazModulToolStripMenuItem.Name = "zmazModulToolStripMenuItem";
			this.zmazModulToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.zmazModulToolStripMenuItem.Text = "Zmaž modul";
			this.zmazModulToolStripMenuItem.Click += new System.EventHandler(this.ZmazModulToolStripMenuItemClick);
			// 
			// tabControl1
			// 
			this.tabControl1.Location = new System.Drawing.Point(12, 27);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1065, 642);
			this.tabControl1.TabIndex = 8;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.VyberModulu);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Location = new System.Drawing.Point(1083, 27);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(169, 642);
			this.panel1.TabIndex = 9;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 681);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "DataWolf";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem otvorProjektToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ulozProjektToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem projektToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem novaFunkciaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem registreToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem kompilujToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem funkciaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem vstupyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem vystupyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem premenujModulToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zmazModulToolStripMenuItem;
	}
}
