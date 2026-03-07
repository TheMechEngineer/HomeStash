using FrontEnd.Controls.Utilities;

namespace FrontEnd.UserControls
{
    internal partial class TopDownBuildingView
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
            splTopView = new SplitContainer();
            pnlTopViewCamera = new Panel();
            ClickHoldTimer = new System.Windows.Forms.Timer(components);
            tsbtnScaleDown = new ToolStripButton();
            tsbtnScaleUp = new ToolStripButton();
            tsbtnCenter = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tsbtnAddRoom = new ToolStripButton();
            tsrTopDown = new ToolStrip();
            tslblBuilding = new ToolStripLabel();
            tsbtnFitToScreen = new ToolStripButton();
            toolStripLabel1 = new ToolStripLabel();
            tsnudHGridCount = new ToolStripNumericUpDown();
            toolStripLabel2 = new ToolStripLabel();
            tsnudVGridCount = new ToolStripNumericUpDown();
            tslblRoom = new ToolStripLabel();
            tsbtnEditRoom = new ToolStripButton();
            tsbtnDeleteRoom = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tslblStored = new ToolStripLabel();
            tsbtnAddItem = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)splTopView).BeginInit();
            splTopView.Panel1.SuspendLayout();
            splTopView.SuspendLayout();
            tsrTopDown.SuspendLayout();
            SuspendLayout();
            // 
            // splTopView
            // 
            splTopView.BackColor = SystemColors.ControlDark;
            splTopView.Dock = DockStyle.Fill;
            splTopView.Location = new Point(0, 26);
            splTopView.Name = "splTopView";
            // 
            // splTopView.Panel1
            // 
            splTopView.Panel1.Controls.Add(pnlTopViewCamera);
            // 
            // splTopView.Panel2
            // 
            splTopView.Panel2.BackColor = SystemColors.Window;
            splTopView.Size = new Size(897, 510);
            splTopView.SplitterDistance = 500;
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
            pnlTopViewCamera.Size = new Size(500, 510);
            pnlTopViewCamera.TabIndex = 0;
            // 
            // ClickHoldTimer
            // 
            ClickHoldTimer.Tick += ClickHoldTimer_Tick;
            // 
            // tsbtnScaleDown
            // 
            tsbtnScaleDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnScaleDown.Image = Properties.Resources.shrink;
            tsbtnScaleDown.ImageTransparentColor = Color.Magenta;
            tsbtnScaleDown.Name = "tsbtnScaleDown";
            tsbtnScaleDown.Size = new Size(23, 23);
            tsbtnScaleDown.Text = "toolStripButton1";
            tsbtnScaleDown.ToolTipText = "Scale Building View Down (Hold For Auto Scale)";
            tsbtnScaleDown.Click += tsbtnScale_Click;
            tsbtnScaleDown.MouseDown += tsbtnScale_MouseDown;
            tsbtnScaleDown.MouseLeave += tsbtnScale_MouseLeave;
            tsbtnScaleDown.MouseUp += tsbtnScale_MouseUp;
            // 
            // tsbtnScaleUp
            // 
            tsbtnScaleUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnScaleUp.Image = Properties.Resources.grow;
            tsbtnScaleUp.ImageTransparentColor = Color.Magenta;
            tsbtnScaleUp.Name = "tsbtnScaleUp";
            tsbtnScaleUp.Size = new Size(23, 23);
            tsbtnScaleUp.Text = "toolStripButton2";
            tsbtnScaleUp.ToolTipText = "Scale Building View Up (Hold For Auto Scale)";
            tsbtnScaleUp.Click += tsbtnScale_Click;
            tsbtnScaleUp.MouseDown += tsbtnScale_MouseDown;
            tsbtnScaleUp.MouseLeave += tsbtnScale_MouseLeave;
            tsbtnScaleUp.MouseUp += tsbtnScale_MouseUp;
            // 
            // tsbtnCenter
            // 
            tsbtnCenter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnCenter.Image = Properties.Resources.focus;
            tsbtnCenter.ImageTransparentColor = Color.Magenta;
            tsbtnCenter.Name = "tsbtnCenter";
            tsbtnCenter.Size = new Size(23, 23);
            tsbtnCenter.Text = "toolStripButton1";
            tsbtnCenter.ToolTipText = "Center View If Building Larger Than View";
            tsbtnCenter.Click += tsbtnCenter_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 26);
            // 
            // tsbtnAddRoom
            // 
            tsbtnAddRoom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnAddRoom.Image = Properties.Resources.add;
            tsbtnAddRoom.ImageTransparentColor = Color.Magenta;
            tsbtnAddRoom.Name = "tsbtnAddRoom";
            tsbtnAddRoom.Size = new Size(23, 23);
            tsbtnAddRoom.Text = "toolStripButton1";
            tsbtnAddRoom.ToolTipText = "Add New Room To Building";
            tsbtnAddRoom.Click += tsbtnAddRoom_Click;
            // 
            // tsrTopDown
            // 
            tsrTopDown.BackColor = SystemColors.ControlDark;
            tsrTopDown.GripStyle = ToolStripGripStyle.Hidden;
            tsrTopDown.Items.AddRange(new ToolStripItem[] { tslblBuilding, tsbtnScaleDown, tsbtnScaleUp, tsbtnCenter, tsbtnFitToScreen, toolStripLabel1, tsnudHGridCount, toolStripLabel2, tsnudVGridCount, toolStripSeparator1, tslblRoom, tsbtnAddRoom, tsbtnEditRoom, tsbtnDeleteRoom, toolStripSeparator2, tslblStored, tsbtnAddItem });
            tsrTopDown.Location = new Point(0, 0);
            tsrTopDown.Name = "tsrTopDown";
            tsrTopDown.Size = new Size(897, 26);
            tsrTopDown.TabIndex = 0;
            tsrTopDown.Text = "toolStrip1";
            // 
            // tslblBuilding
            // 
            tslblBuilding.Name = "tslblBuilding";
            tslblBuilding.Size = new Size(102, 23);
            tslblBuilding.Text = "Building Controls:";
            // 
            // tsbtnFitToScreen
            // 
            tsbtnFitToScreen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnFitToScreen.Image = Properties.Resources.fittoscreen;
            tsbtnFitToScreen.ImageTransparentColor = Color.Magenta;
            tsbtnFitToScreen.Name = "tsbtnFitToScreen";
            tsbtnFitToScreen.Size = new Size(23, 23);
            tsbtnFitToScreen.Text = "toolStripButton1";
            tsbtnFitToScreen.ToolTipText = "Fit Building To Screen";
            tsbtnFitToScreen.Click += tsbtnFitToScreen_Click;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(19, 23);
            toolStripLabel1.Text = "H:";
            toolStripLabel1.ToolTipText = "Set Horizontal Grid Count";
            // 
            // tsnudHGridCount
            // 
            tsnudHGridCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            tsnudHGridCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            tsnudHGridCount.Name = "tsnudHGridCount";
            tsnudHGridCount.Size = new Size(41, 23);
            tsnudHGridCount.Text = "1";
            tsnudHGridCount.ToolTipText = "Set Horizontal Grid Count";
            tsnudHGridCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            tsnudHGridCount.ValueChanged += tsnudHGridCount_ValueChanged;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(17, 23);
            toolStripLabel2.Text = "V:";
            toolStripLabel2.ToolTipText = "Set Vertical Grid Count";
            // 
            // tsnudVGridCount
            // 
            tsnudVGridCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            tsnudVGridCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            tsnudVGridCount.Name = "tsnudVGridCount";
            tsnudVGridCount.Size = new Size(41, 23);
            tsnudVGridCount.Text = "1";
            tsnudVGridCount.ToolTipText = "Set Vertical Grid Count";
            tsnudVGridCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            tsnudVGridCount.ValueChanged += tsnudVGridCount_ValueChanged;
            // 
            // tslblRoom
            // 
            tslblRoom.Name = "tslblRoom";
            tslblRoom.Size = new Size(90, 23);
            tslblRoom.Text = "Room Controls:";
            // 
            // tsbtnEditRoom
            // 
            tsbtnEditRoom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnEditRoom.Image = Properties.Resources.editing;
            tsbtnEditRoom.ImageTransparentColor = Color.Magenta;
            tsbtnEditRoom.Name = "tsbtnEditRoom";
            tsbtnEditRoom.Size = new Size(23, 23);
            tsbtnEditRoom.Text = "toolStripButton1";
            tsbtnEditRoom.ToolTipText = "Edit A Selected Room";
            tsbtnEditRoom.Click += tsbtnEditRoom_Click;
            // 
            // tsbtnDeleteRoom
            // 
            tsbtnDeleteRoom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnDeleteRoom.Image = Properties.Resources.bin;
            tsbtnDeleteRoom.ImageTransparentColor = Color.Magenta;
            tsbtnDeleteRoom.Name = "tsbtnDeleteRoom";
            tsbtnDeleteRoom.Size = new Size(23, 23);
            tsbtnDeleteRoom.Text = "toolStripButton1";
            tsbtnDeleteRoom.Click += tsbtnDeleteRoom_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 26);
            // 
            // tslblStored
            // 
            tslblStored.Name = "tslblStored";
            tslblStored.Size = new Size(82, 23);
            tslblStored.Text = "Item Controls:";
            // 
            // tsbtnAddItem
            // 
            tsbtnAddItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnAddItem.Image = Properties.Resources.add;
            tsbtnAddItem.ImageTransparentColor = Color.Magenta;
            tsbtnAddItem.Name = "tsbtnAddItem";
            tsbtnAddItem.Size = new Size(23, 23);
            tsbtnAddItem.Text = "toolStripButton1";
            tsbtnAddItem.ToolTipText = "Add A New Item";
            tsbtnAddItem.Click += tsbtnAddItem_Click;
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
            splTopView.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splTopView).EndInit();
            splTopView.ResumeLayout(false);
            tsrTopDown.ResumeLayout(false);
            tsrTopDown.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private SplitContainer splTopView;
        private Panel pnlTopViewCamera;
        private System.Windows.Forms.Timer ClickHoldTimer;
        private ToolStripButton tsbtnScaleDown;
        private ToolStripButton tsbtnScaleUp;
        private ToolStripButton tsbtnCenter;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsbtnAddRoom;
        private ToolStrip tsrTopDown;
        private ToolStripNumericUpDown tsnudHGridCount;
        private ToolStripNumericUpDown tsnudVGridCount;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripButton tsbtnFitToScreen;
        private ToolStripButton tsbtnEditRoom;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsbtnAddItem;
        private ToolStripButton tsbtnDeleteRoom;
        private ToolStripLabel tslblBuilding;
        private ToolStripLabel tslblRoom;
        private ToolStripLabel tslblStored;
    }
}
