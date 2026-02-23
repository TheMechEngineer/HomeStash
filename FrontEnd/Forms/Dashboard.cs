using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using BackEnd.Utilities;
using FrontEnd.Adapters;
using FrontEnd.UserControls;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.PeerToPeer;
using System.Windows.Forms;

namespace FrontEnd.Forms
{
    internal partial class Dashboard : Form
    {
        private List<Item> _items;

        private RootManager RootManagerInstance = Startup.TempItemStatup2();
        private Panel ViewPortPanel;

        internal Dashboard()
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
            ASelection SelectionAdapter = new ASelection(ref RootManagerInstance, RootManagerInstance.UserList, "User");

            Selection NewControl = new Selection(SelectionAdapter);

            NewControl.ChooseSelection += SelectionControl_ChooseSelection;
            NewControl.AddNewSelection += SelectionControl_AddNewSelection;
            NewControl.DeleteSelection += SelectionControl_DeleteSelection;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "UserSelection";

            ViewPortPanel.Controls.Add(NewControl);
        }

        public void OpenAddNewUser()
        {
            AddNewUser NewControl = new AddNewUser();

            NewControl.AddConfirmed += AddNewUserControl_AddConfirmed;
            NewControl.AddCanceled += AddNewUserControl_AddCanceled;

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["UserSelection"].Left + (ViewPortPanel.Controls["UserSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["UserSelection"].Top + (ViewPortPanel.Controls["UserSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "AddNewUser";

            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        public void OpenBuildingSelection()
        {
            ASelection SelectionAdapter = new ASelection(ref RootManagerInstance, RootManagerInstance.ActiveUser.BuildingList, "Building");

            Selection NewControl = new Selection(SelectionAdapter);

            NewControl.ChooseSelection += SelectionControl_ChooseSelection;
            NewControl.AddNewSelection += SelectionControl_AddNewSelection;
            NewControl.DeleteSelection += SelectionControl_DeleteSelection;

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

        private void SelectionControl_ChooseSelection(Selection _CurrentControl, Type _CurrentType, object _SelectedObject)
        {
            switch (_CurrentType)
            {
                case Type CurrentType when _CurrentType == typeof(User):
                    //need to unwire the populate list event
                    RootManagerInstance.ActiveUser = _SelectedObject as User;
                    OpenBuildingSelection();

                    break;
                case Type CurrentType when _CurrentType == typeof(Building):
                    //need to unwire the populate list event
                    RootManagerInstance.ActiveUser.ActiveBuilding = _SelectedObject as Building;
                    OpenTopDownBuildingView();
                    break;
            }

            _CurrentControl.ChooseSelection -= SelectionControl_ChooseSelection;
            _CurrentControl.AddNewSelection -= SelectionControl_AddNewSelection;
            _CurrentControl.DeleteSelection -= SelectionControl_DeleteSelection;

            ViewPortPanel.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        private void SelectionControl_AddNewSelection(Selection _CurrentControl, Type _CurrentType)
        {
            switch (_CurrentType)
            {
                case Type CurrentType when _CurrentType == typeof(User):
                    OpenAddNewUser();
                    break;
                case Type CurrentType when _CurrentType == typeof(Building):
                    OpenAddNewBuilding();
                    break;
            }

            _CurrentControl.Enabled = false;
        }

        private void SelectionControl_DeleteSelection(Type _CurrentType, object _SelectedObject)
        {
            switch (_CurrentType)
            {
                case Type CurrentType when _CurrentType == typeof(User):
                    RootManagerInstance.RemoveUser(_SelectedObject as User);

                    break;
                case Type CurrentType when _CurrentType == typeof(Building):
                    RootManagerInstance.ActiveUser.RemoveBuilding(_SelectedObject as Building);
                    break;
            }
        }

        private void AddNewUserControl_AddConfirmed(AddNewUser _CurrentControl, User _AddedUser)
        {
            RootManagerInstance.AddUser(_AddedUser);
            AddNewUserControl_AddCanceled(_CurrentControl);
        }

        private void AddNewUserControl_AddCanceled(AddNewUser _CurrentControl)
        {
            ViewPortPanel.Controls["UserSelection"].Enabled = true;
            ViewPortPanel.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

    }
}
