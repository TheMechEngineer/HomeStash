namespace FrontEnd.UserControls
{
    partial class TopDownBuildingView
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
            components = new System.ComponentModel.Container();
            tsrTopDown = new ToolStrip();
            tsbtnScaleDown = new ToolStripButton();
            tsbtnScaleUp = new ToolStripButton();
            tsbtnCenter = new ToolStripButton();
            splTopView = new SplitContainer();
            pnlTopViewCamera = new Panel();
            ClickHoldTimer = new System.Windows.Forms.Timer(components);
            tsrTopDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splTopView).BeginInit();
            splTopView.Panel1.SuspendLayout();
            splTopView.SuspendLayout();
            SuspendLayout();
            // 
            // tsrTopDown
            // 
            tsrTopDown.BackColor = SystemColors.ControlDark;
            tsrTopDown.GripStyle = ToolStripGripStyle.Hidden;
            tsrTopDown.Items.AddRange(new ToolStripItem[] { tsbtnScaleDown, tsbtnScaleUp, tsbtnCenter });
            tsrTopDown.Location = new Point(0, 0);
            tsrTopDown.Name = "tsrTopDown";
            tsrTopDown.Size = new Size(897, 25);
            tsrTopDown.TabIndex = 0;
            tsrTopDown.Text = "toolStrip1";
            // 
            // tsbtnScaleDown
            // 
            tsbtnScaleDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnScaleDown.Image = Properties.Resources.downarrow;
            tsbtnScaleDown.ImageTransparentColor = Color.Magenta;
            tsbtnScaleDown.Name = "tsbtnScaleDown";
            tsbtnScaleDown.Size = new Size(23, 22);
            tsbtnScaleDown.Text = "toolStripButton1";
            tsbtnScaleDown.ToolTipText = "Scale Building View Down (Hold For Auto Scale)";
            tsbtnScaleDown.Click += tsbtnScale_Click;
            tsbtnScaleDown.MouseDown += tsbtnScale_MouseDown;
            tsbtnScaleDown.MouseUp += tsbtnScale_MouseUp;
            // 
            // tsbtnScaleUp
            // 
            tsbtnScaleUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnScaleUp.Image = Properties.Resources.uparrow;
            tsbtnScaleUp.ImageTransparentColor = Color.Magenta;
            tsbtnScaleUp.Name = "tsbtnScaleUp";
            tsbtnScaleUp.Size = new Size(23, 22);
            tsbtnScaleUp.Text = "toolStripButton2";
            tsbtnScaleUp.ToolTipText = "Scale Building View Up (Hold For Auto Scale)";
            tsbtnScaleUp.Click += tsbtnScale_Click;
            tsbtnScaleUp.MouseDown += tsbtnScale_MouseDown;
            tsbtnScaleUp.MouseUp += tsbtnScale_MouseUp;
            // 
            // tsbtnCenter
            // 
            tsbtnCenter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnCenter.Image = Properties.Resources.focus;
            tsbtnCenter.ImageTransparentColor = Color.Magenta;
            tsbtnCenter.Name = "tsbtnCenter";
            tsbtnCenter.Size = new Size(23, 22);
            tsbtnCenter.Text = "toolStripButton1";
            tsbtnCenter.ToolTipText = "Center View If Building Larger Than View";
            tsbtnCenter.Click += tsbtnCenter_Click;
            // 
            // splTopView
            // 
            splTopView.BackColor = SystemColors.ControlDark;
            splTopView.Dock = DockStyle.Fill;
            splTopView.Location = new Point(0, 25);
            splTopView.Name = "splTopView";
            // 
            // splTopView.Panel1
            // 
            splTopView.Panel1.Controls.Add(pnlTopViewCamera);
            // 
            // splTopView.Panel2
            // 
            splTopView.Panel2.BackColor = SystemColors.Window;
            splTopView.Size = new Size(897, 511);
            splTopView.SplitterDistance = 666;
            splTopView.SplitterWidth = 5;
            splTopView.TabIndex = 1;
            // 
            // pnlTopViewCamera
            // 
            pnlTopViewCamera.AutoScroll = true;
            pnlTopViewCamera.BackColor = SystemColors.Control;
            pnlTopViewCamera.Dock = DockStyle.Fill;
            pnlTopViewCamera.Location = new Point(0, 0);
            pnlTopViewCamera.Name = "pnlTopViewCamera";
            pnlTopViewCamera.Size = new Size(666, 511);
            pnlTopViewCamera.TabIndex = 0;
            // 
            // ClickHoldTimer
            // 
            ClickHoldTimer.Tick += ClickHoldTimer_Tick;
            // 
            // TopDownBuildingView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(splTopView);
            Controls.Add(tsrTopDown);
            Name = "TopDownBuildingView";
            Size = new Size(897, 536);
            tsrTopDown.ResumeLayout(false);
            tsrTopDown.PerformLayout();
            splTopView.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splTopView).EndInit();
            splTopView.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip tsrTopDown;
        private ToolStripButton tsbtnScaleDown;
        private SplitContainer splTopView;
        private Panel pnlTopViewCamera;
        private ToolStripButton tsbtnScaleUp;
        private System.Windows.Forms.Timer ClickHoldTimer;
        private ToolStripButton tsbtnCenter;
    }
}
