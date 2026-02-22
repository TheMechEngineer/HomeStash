namespace FrontEnd.Forms
{
    partial class Dashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            mnsDashboard = new MenuStrip();
            testToolStripMenuItem = new ToolStripMenuItem();
            tsmiUserSelect = new ToolStripMenuItem();
            tsmiBuildingSelect = new ToolStripMenuItem();
            test2ToolStripMenuItem = new ToolStripMenuItem();
            pnlDashboard = new Panel();
            mnsDashboard.SuspendLayout();
            SuspendLayout();
            // 
            // mnsDashboard
            // 
            mnsDashboard.Items.AddRange(new ToolStripItem[] { testToolStripMenuItem, test2ToolStripMenuItem });
            mnsDashboard.Location = new Point(0, 0);
            mnsDashboard.Name = "mnsDashboard";
            mnsDashboard.Size = new Size(800, 24);
            mnsDashboard.TabIndex = 0;
            mnsDashboard.Text = "menuStrip1";
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmiUserSelect, tsmiBuildingSelect });
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new Size(40, 20);
            testToolStripMenuItem.Text = "Test";
            testToolStripMenuItem.DropDownOpening += testToolStripMenuItem_DropDownOpening;
            // 
            // tsmiUserSelect
            // 
            tsmiUserSelect.Name = "tsmiUserSelect";
            tsmiUserSelect.Size = new Size(180, 22);
            tsmiUserSelect.Text = "User Select";
            tsmiUserSelect.Click += tsmiUserSelect_Click;
            // 
            // tsmiBuildingSelect
            // 
            tsmiBuildingSelect.Name = "tsmiBuildingSelect";
            tsmiBuildingSelect.Size = new Size(180, 22);
            tsmiBuildingSelect.Text = "Building Select";
            tsmiBuildingSelect.Click += tsmiBuildingSelect_Click;
            // 
            // test2ToolStripMenuItem
            // 
            test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            test2ToolStripMenuItem.Size = new Size(46, 20);
            test2ToolStripMenuItem.Text = "Test2";
            // 
            // pnlDashboard
            // 
            pnlDashboard.Dock = DockStyle.Fill;
            pnlDashboard.Location = new Point(0, 24);
            pnlDashboard.Name = "pnlDashboard";
            pnlDashboard.Size = new Size(800, 426);
            pnlDashboard.TabIndex = 1;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlDashboard);
            Controls.Add(mnsDashboard);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mnsDashboard;
            Name = "Dashboard";
            Text = "HomeStash";
            mnsDashboard.ResumeLayout(false);
            mnsDashboard.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip mnsDashboard;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem test2ToolStripMenuItem;
        private Panel pnlDashboard;
        private ToolStripMenuItem tsmiUserSelect;
        private ToolStripMenuItem tsmiBuildingSelect;
    }
}
