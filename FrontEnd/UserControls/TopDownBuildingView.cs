using BackEnd.ModelClasses;
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
        Building CurrentBuilding;

        private const int DefaultPixelsPerUnit = 10;
        private float currentZoom = 1.0f;
        private const int BuildingOffsetBuffer = 25;

        internal TopDownBuildingView(ref RootManager _ProgramRoot)
        {

            InitializeComponent();

            RootManagerInstance = _ProgramRoot;
            CurrentBuilding = RootManagerInstance.ActiveUser.ActiveBuilding;

            InitializeVisuals();
            Wire();
        }

        private void InitializeVisuals()
        {
            Panel BufferPanel = splTopView.Panel1.Controls["pnlTopViewCamera"].Controls["pnlBuildingVisualEdgeBuffer"] as Panel;
            BufferPanel.Padding = new Padding(BuildingOffsetBuffer);

            BuildingControl DisplayedBuilding = new BuildingControl(ref CurrentBuilding, DefaultPixelsPerUnit);

            DisplayedBuilding.Dock = DockStyle.None;
            DisplayedBuilding.Name = "DisplayedBuilding";
            DisplayedBuilding.Location = new Point(BuildingOffsetBuffer, BuildingOffsetBuffer);
            DisplayedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            BufferPanel.Controls.Add(DisplayedBuilding);
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
   
            Panel CameraPanel = splTopView.Panel1.Controls["pnlTopViewCamera"] as Panel;

            int BufferedWidth = CameraPanel.Controls["pnlBuildingVisualEdgeBuffer"].Width;
            int BufferedHeight = CameraPanel.Controls["pnlBuildingVisualEdgeBuffer"].Height;

            int CameraWidth = CameraPanel.ClientSize.Width;
            int CameraHeight = CameraPanel.ClientSize.Height;

            int ViewLeftBound = (BufferedWidth - CameraWidth) / 2;
            int ViewTopBound = (BufferedHeight - CameraHeight) / 2;

            CameraPanel.AutoScrollPosition = new Point(ViewLeftBound, ViewTopBound);
        }

        private void ScaleCameraView(float ScaleModifier)
        {
            currentZoom *= ScaleModifier;

            //Set building
            BuildingControl CurrentBuildingControl = splTopView.Panel1.Controls["pnlTopViewCamera"].Controls["pnlBuildingVisualEdgeBuffer"].Controls["DisplayedBuilding"] as BuildingControl;

            CurrentBuildingControl.Width = Convert.ToInt32(CurrentBuildingControl.InitialDisplayWidth * currentZoom);
            CurrentBuildingControl.Height = Convert.ToInt32(CurrentBuildingControl.InitialDisplayHeight * currentZoom);

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
            splTopView.Panel1.Enabled = false;

        }

        private void TopDownBuildingView_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(CenterCameraView);
        }

        private void tsbtnScale_Click(object sender, EventArgs e)
        {
            ToolStripButton CurrentButton = sender as ToolStripButton;

            if (CurrentButton.Name == "tsbtnScaleDown")
            {
                ScaleCameraView(.9f);
            }
            else if (CurrentButton.Name == "tsbtnScaleUp")
            {
                ScaleCameraView(1.1f);
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

        private void tsbtnCenter_Click(object sender, EventArgs e)
        {
            CenterCameraView();
        }

        private void tsbtnAddRoom_Click(object sender, EventArgs e)
        {
            OpenAddNewRoom();
        }

        private void AddNewRoomControl_AddConfirmed(AddNewRoom _CurrentControl, (string Name, int Height, int Width, int CenterX, int CenterY, int ColorValue) _RoomValues)
        {
            string? _ErrorMessage;

            if (CurrentBuilding.TryAddRoom(_RoomValues.Name, _RoomValues.Height, _RoomValues.Width, _RoomValues.CenterX, _RoomValues.CenterY, _RoomValues.ColorValue, out _ErrorMessage))
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
            splTopView.Panel1.Enabled = true;

            splTopView.Panel2.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }


    }
}
