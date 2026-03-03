using BackEnd.ModelClasses;
using FrontEnd.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd.UserControls
{
    internal partial class TopDownBuildingView : UserControl
    {
        private RootManager RootManagerInstance;
        private Building CurrentBuilding;
        private BuildingControlBuffer CurrentBufferedBuilding;

        private Panel CameraPanel;

        internal TopDownBuildingView(ref RootManager _ProgramRoot)
        {

            InitializeComponent();

            this.RootManagerInstance = _ProgramRoot;
            this.CurrentBuilding = RootManagerInstance.ActiveUser.ActiveBuilding;

            InitializeVisuals();
            Wire();
        }

        private void InitializeVisuals()
        {
            this.CameraPanel = splTopView.Panel1.Controls["pnlTopViewCamera"] as Panel;

            this.CurrentBufferedBuilding = new BuildingControlBuffer(CurrentBuilding);

            CurrentBufferedBuilding.Dock = DockStyle.None;
            CurrentBufferedBuilding.Name = "CurrentBufferedBuilding";
            CurrentBufferedBuilding.Location = new Point(0, 0);
            CurrentBufferedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.tsnudHGridCount.Value = CurrentBufferedBuilding.HGridCount;
            this.tsnudVGridCount.Value = CurrentBufferedBuilding.VGridCount;

            this.CameraPanel.Controls.Add(CurrentBufferedBuilding);
        }

        private void Wire()
        {
            this.Load += TopDownBuildingView_Load;
            this.HandleDestroyed += UnWire;
        }
        private void UnWire(object? sender, EventArgs e)
        {
            this.Load -= TopDownBuildingView_Load;
            this.HandleDestroyed -= UnWire;
        }

        private void CenterCameraView()
        {
            int BufferedWidth = this.CurrentBufferedBuilding.Width;
            int BufferedHeight = this.CurrentBufferedBuilding.Height;

            int CameraWidth = this.CameraPanel.ClientSize.Width;
            int CameraHeight = this.CameraPanel.ClientSize.Height;

            int ViewLeftBound = (BufferedWidth - CameraWidth) / 2;
            int ViewTopBound = (BufferedHeight - CameraHeight) / 2;

            this.CameraPanel.AutoScrollPosition = new Point(ViewLeftBound, ViewTopBound);
        }

        private void FitBuildingToScreen()
        {
            float PercentOfScreenToFill = 0.95f;

            float BufferedControlWidth = Convert.ToSingle(this.CurrentBufferedBuilding.Width);
            float BuildingControlWidth = Convert.ToSingle(BufferedControlWidth - (2 * CurrentBufferedBuilding.BuildingOffsetBuffer));

            float BufferedControlHeight = Convert.ToSingle(this.CurrentBufferedBuilding.Height);
            float BuildingControlHeight = Convert.ToSingle(BufferedControlHeight - (2 * CurrentBufferedBuilding.BuildingOffsetBuffer));

            float DesiredBufferWidth = PercentOfScreenToFill * Convert.ToSingle(this.CameraPanel.ClientSize.Width);
            float WidthLinearIncrease = DesiredBufferWidth - BufferedControlWidth;

            float DesiredBufferHeight = PercentOfScreenToFill * Convert.ToSingle(this.CameraPanel.ClientSize.Height);
            float HeightLinearIncrease = DesiredBufferHeight - BufferedControlHeight;

            float RequiredWidthScale = (WidthLinearIncrease + BuildingControlWidth) / BuildingControlWidth;
            float RequiredHeightScale = (HeightLinearIncrease + BuildingControlHeight) / BuildingControlHeight;

            float SelectedScale = Math.Min(RequiredWidthScale, RequiredHeightScale);

            CurrentBufferedBuilding.ScaleBuilding(SelectedScale);
        }

        private void OpenAddNewRoom()
        {
            AddNewRoom NewControl = new AddNewRoom();

            NewControl.AddConfirmed += AddNewRoomControl_AddConfirmed;
            NewControl.AddCanceled += AddNewRoomControl_AddCanceled;

            NewControl.Dock = DockStyle.Fill;
            NewControl.Name = "AddNewRoom";

            splTopView.SplitterDistance = splTopView.ClientSize.Width - NewControl.Width;
            splTopView.Panel2.Controls.Add(NewControl);

            tsrTopDown.Enabled = false;

            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        private void TopDownBuildingView_Load(object sender, EventArgs e)
        {

            this.BeginInvoke(() =>
            {
                FitBuildingToScreen();
                CenterCameraView();
            });

        }

        private void tsbtnScale_Click(object sender, EventArgs e)
        {
            ToolStripButton CurrentButton = sender as ToolStripButton;

            if (CurrentButton.Name == "tsbtnScaleDown")
            {
                CurrentBufferedBuilding.ScaleBuilding(.9f);
            }
            else if (CurrentButton.Name == "tsbtnScaleUp")
            {
                CurrentBufferedBuilding.ScaleBuilding(1.1f);
            }
        }

        private void ClickHoldTimer_Tick(object sender, EventArgs e)
        {
            ToolStripButton CurrentButton = ClickHoldTimer.Tag as ToolStripButton;
            tsbtnScale_Click(CurrentButton, e);
        }

        private void tsbtnScale_MouseDown(object sender, MouseEventArgs e)
        {
            ClickHoldTimer.Tag = sender as ToolStripButton;
            ClickHoldTimer.Start();
        }

        private void tsbtnScale_MouseUp(object sender, MouseEventArgs e)
        {
            ClickHoldTimer.Tag = null;
            ClickHoldTimer.Stop();
        }

        private void tsbtnFitToScreen_Click(object sender, EventArgs e)
        {
            FitBuildingToScreen();
        }

        private void tsbtnCenter_Click(object sender, EventArgs e)
        {
            CenterCameraView();
        }

        private void tsbtnAddRoom_Click(object sender, EventArgs e)
        {
            OpenAddNewRoom();
        }

        private void AddNewRoomControl_AddConfirmed(AddNewRoom _CurrentControl, (string Name, float Width, float Height, float CenterX, float CenterY, int ColorValue) _RoomValues)
        {
            string? _ErrorMessage;

            if (CurrentBuilding.TryAddRoom(_RoomValues.Name, _RoomValues.Width, _RoomValues.Height, _RoomValues.CenterX, _RoomValues.CenterY, _RoomValues.ColorValue, out _ErrorMessage))
            {
                AddNewRoomControl_AddCanceled(_CurrentControl);
            }
            else
            {
                MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddNewRoomControl_AddCanceled(AddNewRoom _CurrentControl)
        {
            _CurrentControl.AddConfirmed -= AddNewRoomControl_AddConfirmed;
            _CurrentControl.AddCanceled -= AddNewRoomControl_AddCanceled;

            tsrTopDown.Enabled = true;

            TransparentPanel BlockerPanel = this.CameraPanel.Controls["Blocker"] as TransparentPanel;
            this.CameraPanel.Controls.Remove(BlockerPanel);
            BlockerPanel.Dispose();

            splTopView.Panel2.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        private void tsbtnScale_MouseLeave(object sender, EventArgs e)
        {
            ClickHoldTimer.Tag = null;
            ClickHoldTimer.Stop();
        }

        private void tsnudHGridCount_ValueChanged(object sender, EventArgs e)
        {

            CurrentBufferedBuilding.HGridCount = Convert.ToInt32((sender as ToolStripNumericUpDown).Value);
        }

        private void tsnudVGridCount_ValueChanged(object sender, EventArgs e)
        {
            CurrentBufferedBuilding.VGridCount = Convert.ToInt32((sender as ToolStripNumericUpDown).Value);
        }


    }
}
