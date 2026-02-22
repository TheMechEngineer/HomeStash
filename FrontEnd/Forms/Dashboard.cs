using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using BackEnd.Utilities;
using FrontEnd.UserControls;
using System.Net.PeerToPeer;
using System.Windows.Forms;

namespace FrontEnd.Forms
{
    public partial class Dashboard : Form
    {
        private List<Item> _items;

        private RootManager RootManagerInstance = Startup.TempItemStatup2();
        private Panel ViewPortPanel;

        public Dashboard()
        {
            InitializeComponent();

            ViewPortPanel = this.pnlDashboard;

            InitializeVisuals();
            Wire();

            OpenUserSelection();
        }
        
        private void InitializeVisuals()
        {
            this.WindowState = FormWindowState.Maximized;

            ViewPortPanel.Controls.Clear();

            tsmiBuildingSelect.Enabled = (RootManagerInstance.ActiveUser != null);
        }

        private void Wire()
        {
            RootManagerInstance.ActiveUserChanged += RootManagerInstance_ActiveUserChanged;
        }

        public void OpenUserSelection()
        {
            Selection NewControl = new Selection(ref RootManagerInstance, RootManagerInstance.UserList);

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "UserSelection";

            ViewPortPanel.Controls.Add(NewControl);
        }

        public void OpenAddNewUser()
        {
            AddNewUser NewControl = new AddNewUser(ref RootManagerInstance);

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["UserSelection"].Left + (ViewPortPanel.Controls["UserSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["UserSelection"].Top + (ViewPortPanel.Controls["UserSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "AddNewUser";

            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        public void OpenBuildingSelection()
        {
            Selection NewControl = new Selection(ref RootManagerInstance, RootManagerInstance.ActiveUser.BuildingList);

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "BuildingSelection";

            ViewPortPanel.Controls.Add(NewControl);
        }

        public void OpenAddNewBuilding()
        {
            AddNewBuilding NewControl = new AddNewBuilding(ref RootManagerInstance);

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["BuildingSelection"].Left + (ViewPortPanel.Controls["BuildingSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["BuildingSelection"].Top + (ViewPortPanel.Controls["BuildingSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "AddNewBuilding";

            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        public void OpenTopDownBuildingView()
        {
            TopDownBuildingView NewControl = new TopDownBuildingView(ref RootManagerInstance);

            NewControl.Dock = DockStyle.Fill;
            NewControl.Name = "TopDownBuildingView";

            ViewPortPanel.Controls.Add(NewControl);
        }

        private void tsmiUserSelect_Click(object sender, EventArgs e)
        {
            ViewPortPanel.Controls.Clear();
            OpenUserSelection();
        }

        private void tsmiBuildingSelect_Click(object sender, EventArgs e)
        {
            ViewPortPanel.Controls.Clear();
            OpenBuildingSelection();
        }

        private void RootManagerInstance_ActiveUserChanged()
        {
            tsmiBuildingSelect.Enabled = (RootManagerInstance.ActiveUser != null);
        }

    }
}
