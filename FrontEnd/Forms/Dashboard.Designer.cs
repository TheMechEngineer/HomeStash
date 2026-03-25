namespace FrontEnd.Forms
{
    internal partial class Dashboard
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
            tsddbFile = new ToolStripMenuItem();
            tsmiSave = new ToolStripMenuItem();
            tsmiBuildingReport = new ToolStripMenuItem();
            tsddbMenus = new ToolStripMenuItem();
            tsmiUserSelect = new ToolStripMenuItem();
            tsmiBuildingSelect = new ToolStripMenuItem();
            tsmiTopDown = new ToolStripMenuItem();
            pnlDashboard = new Panel();
            sfdBuildingReport = new SaveFileDialog();
            mnsDashboard.SuspendLayout();
            SuspendLayout();
            // 
            // mnsDashboard
            // 
            mnsDashboard.Items.AddRange(new ToolStripItem[] { tsddbFile, tsddbMenus });
            mnsDashboard.Location = new Point(0, 0);
            mnsDashboard.Name = "mnsDashboard";
            mnsDashboard.Size = new Size(800, 24);
            mnsDashboard.TabIndex = 0;
            mnsDashboard.Text = "menuStrip1";
            // 
            // tsddbFile
            // 
            tsddbFile.DropDownItems.AddRange(new ToolStripItem[] { tsmiSave, tsmiBuildingReport });
            tsddbFile.Name = "tsddbFile";
            tsddbFile.Size = new Size(37, 20);
            tsddbFile.Text = "File";
            // 
            // tsmiSave
            // 
            tsmiSave.Name = "tsmiSave";
            tsmiSave.Size = new Size(206, 22);
            tsmiSave.Text = "Save";
            tsmiSave.Click += tsmiSave_Click;
            // 
            // tsmiBuildingReport
            // 
            tsmiBuildingReport.Name = "tsmiBuildingReport";
            tsmiBuildingReport.Size = new Size(206, 22);
            tsmiBuildingReport.Text = "Generate Building Report";
            tsmiBuildingReport.Click += tsmiBuildingReport_Click;
            // 
            // tsddbMenus
            // 
            tsddbMenus.DropDownItems.AddRange(new ToolStripItem[] { tsmiUserSelect, tsmiBuildingSelect, tsmiTopDown });
            tsddbMenus.Name = "tsddbMenus";
            tsddbMenus.Size = new Size(55, 20);
            tsddbMenus.Text = "Menus";
            // 
            // tsmiUserSelect
            // 
            tsmiUserSelect.Name = "tsmiUserSelect";
            tsmiUserSelect.Size = new Size(169, 22);
            tsmiUserSelect.Text = "User Selection";
            tsmiUserSelect.Click += tsmiUserSelect_Click;
            // 
            // tsmiBuildingSelect
            // 
            tsmiBuildingSelect.Name = "tsmiBuildingSelect";
            tsmiBuildingSelect.Size = new Size(169, 22);
            tsmiBuildingSelect.Text = "Building Selection";
            tsmiBuildingSelect.Click += tsmiBuildingSelect_Click;
            // 
            // tsmiTopDown
            // 
            tsmiTopDown.Name = "tsmiTopDown";
            tsmiTopDown.Size = new Size(169, 22);
            tsmiTopDown.Text = "Top-Down View";
            tsmiTopDown.Click += tsmiTopDown_Click;
            // 
            // pnlDashboard
            // 
            pnlDashboard.Dock = DockStyle.Fill;
            pnlDashboard.Location = new Point(0, 24);
            pnlDashboard.Name = "pnlDashboard";
            pnlDashboard.Size = new Size(800, 426);
            pnlDashboard.TabIndex = 1;
            // 
            // sfdBuildingReport
            // 
            sfdBuildingReport.DefaultExt = "pdf";
            sfdBuildingReport.Filter = "PDF files (*.pdf)|*.pdf";
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
            Load += Dashboard_Load;
            mnsDashboard.ResumeLayout(false);
            mnsDashboard.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip mnsDashboard;
        private ToolStripMenuItem tsddbMenus;
        private ToolStripMenuItem tsddbFile;
        private Panel pnlDashboard;
        private ToolStripMenuItem tsmiUserSelect;
        private ToolStripMenuItem tsmiBuildingSelect;
        private ToolStripMenuItem tsmiSave;
        private ToolStripMenuItem tsmiTopDown;
        private ToolStripMenuItem tsmiBuildingReport;
        private SaveFileDialog sfdBuildingReport;
    }
}
