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

        private const int DefaultScaleFactor = 10;
        private float currentZoom = 1.0f;



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
            BuildingControl DisplayedBuilding = new BuildingControl(ref CurrentBuilding, DefaultScaleFactor);

            DisplayedBuilding.Dock = DockStyle.None;
            DisplayedBuilding.Name = "DisplayedBuilding";
            DisplayedBuilding.Location = new Point(0, 0);
            DisplayedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            splTopView.Panel1.Controls["pnlTopViewCamera"].Controls.Add(DisplayedBuilding);
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

            int BuildingWidth = CameraPanel.Controls["DisplayedBuilding"].Width;
            int BuildingHeight = CameraPanel.Controls["DisplayedBuilding"].Height;

            int CameraWidth = CameraPanel.ClientSize.Width;
            int CameraHeight = CameraPanel.ClientSize.Height;

            int CenterX = (BuildingWidth - CameraWidth) / 2;
            int CenterY = (BuildingHeight - CameraHeight) / 2;

            CameraPanel.AutoScrollPosition = new Point(CenterX, CenterY);
        }

        private void ScaleCameraView(float ScaleModifier)
        {
            currentZoom *= ScaleModifier;

            //Set building
            BuildingControl CurrentBuilding = splTopView.Panel1.Controls["pnlTopViewCamera"].Controls["DisplayedBuilding"] as BuildingControl;

            CurrentBuilding.Width = Convert.ToInt32(CurrentBuilding.InitialDisplayWidth * currentZoom);
            CurrentBuilding.Height = Convert.ToInt32(CurrentBuilding.InitialDisplayHeight * currentZoom);

        }

        private void OpenAddNewRoom()
        {
            AddNewRoom NewControl = new AddNewRoom();

            //Need To Unwire These
            NewControl.AddConfirmed += AddNewRoomControl_AddConfirmed;
            NewControl.AddCanceled += AddNewRoomControl_AddCanceled;

            NewControl.Dock = DockStyle.Fill;
            NewControl.Name = "AddNewRoom";

            splTopView.SplitterDistance = splTopView.ClientSize.Width - NewControl.Width;
            splTopView.Panel2.Controls.Add(NewControl);

            tsrTopDown.Enabled = false;
            splTopView.Panel1.Enabled = false;
            //need to diable panel one and the toolstrip
            //need to name the controls in addnew room
        }

        private void CloseAddNewRoom()
        {
            tsrTopDown.Enabled = true;
            splTopView.Panel1.Enabled = true;

            if (splTopView.Panel2.Controls.ContainsKey("AddNewRoom"))
            {
                AddNewRoom RemovedControl = splTopView.Panel2.Controls["AddNewRoom"] as AddNewRoom;
                splTopView.Panel2.Controls.Remove(RemovedControl);
                RemovedControl.Dispose();
            }
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
            throw new NotImplementedException();
        }

        private void AddNewRoomControl_AddCanceled(AddNewRoom _CurrentControl)
        {
            throw new NotImplementedException();
        }


    }
}
