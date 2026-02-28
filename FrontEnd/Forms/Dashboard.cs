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

        private void OpenUserSelection()
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

        private void OpenAddNewUser()
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

        private void OpenBuildingSelection()
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
    
        private void OpenAddNewBuilding()
        {
            AddNewBuilding NewControl = new AddNewBuilding();

            NewControl.AddConfirmed += AddNewBuildingControl_AddConfirmed;
            NewControl.AddCanceled += AddNewBuildingControl_AddCanceled;

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["BuildingSelection"].Left + (ViewPortPanel.Controls["BuildingSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["BuildingSelection"].Top + (ViewPortPanel.Controls["BuildingSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "AddNewBuilding";

            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        private void OpenTopDownBuildingView()
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
            string? _ErrorMessage;

            switch (_CurrentType)
            {
                case Type CurrentType when _CurrentType == typeof(User):
                    if (RootManagerInstance.TryChangeActiveUser(_SelectedObject as User, out _ErrorMessage))
                    {
                        OpenBuildingSelection();
                    }
                    else
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                case Type CurrentType when _CurrentType == typeof(Building):
                    if (RootManagerInstance.ActiveUser.TryChangeActiveBuilding(_SelectedObject as Building, out _ErrorMessage))
                    {
                        OpenTopDownBuildingView();
                    }
                    else
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
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
            string? _ErrorMessage;

            switch (_CurrentType)
            {
                case Type CurrentType when _CurrentType == typeof(User):
                    if(!RootManagerInstance.TryRemoveUser(_SelectedObject as User, out _ErrorMessage))
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case Type CurrentType when _CurrentType == typeof(Building):
                    if (!RootManagerInstance.ActiveUser.TryRemoveBuilding(_SelectedObject as Building, out _ErrorMessage))
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        private void AddNewUserControl_AddConfirmed(AddNewUser _CurrentControl, string _AddedUsername)
        {
            string? _ErrorMessage;

            if (RootManagerInstance.TryAddUser(_AddedUsername, out _ErrorMessage))
            {
                AddNewUserControl_AddCanceled(_CurrentControl);
            }
            else
            {
                MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AddNewUserControl_AddCanceled(AddNewUser _CurrentControl)
        {
            _CurrentControl.AddConfirmed -= AddNewUserControl_AddConfirmed;
            _CurrentControl.AddCanceled -= AddNewUserControl_AddCanceled;

            ViewPortPanel.Controls["UserSelection"].Enabled = true;
            ViewPortPanel.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        private void AddNewBuildingControl_AddConfirmed(AddNewBuilding _CurrentControl, (string _Name, int _Height, int _Width) _BuildingValues)
        {
            string? _ErrorMessage = null;

            if(RootManagerInstance.ActiveUser.TryAddBuilding(_BuildingValues._Name, _BuildingValues._Height, _BuildingValues._Width, out _ErrorMessage))
            {
                AddNewBuildingControl_AddCanceled(_CurrentControl);
            }
            else
            {
                MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void AddNewBuildingControl_AddCanceled(AddNewBuilding _CurrentControl)
        {
            _CurrentControl.AddConfirmed -= AddNewBuildingControl_AddConfirmed;
            _CurrentControl.AddCanceled -= AddNewBuildingControl_AddCanceled;

            ViewPortPanel.Controls["BuildingSelection"].Enabled = true;

            ViewPortPanel.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

    }
}
