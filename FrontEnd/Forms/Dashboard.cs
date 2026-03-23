using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using BackEnd.Utilities;
using FrontEnd.Adapters;
using FrontEnd.UserControls;
using FrontEnd.Utilities;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.PeerToPeer;
using System.Windows.Forms;
using System.Text.Json;
using BackEnd.Reports;
using System.Drawing.Imaging;

namespace FrontEnd.Forms
{
    internal partial class Dashboard : Form
    {
        private RootManager RootManagerInstance = DataContinuityController.StartupDataContinuity();
        private Panel ViewPortPanel;

        private User? CurrentActiveUser;

        internal Dashboard()
        {
            InitializeComponent();

            ViewPortPanel = this.pnlDashboard;

            InitializeVisuals();
            Wire();

            UserSelection();
        }

        private void InitializeVisuals()
        {
            this.WindowState = FormWindowState.Maximized;

            ViewPortPanel.Controls.Clear();

            tsmiBuildingSelect.Enabled = (RootManagerInstance.ActiveUser != null);
            tsmiTopDown.Enabled = (RootManagerInstance.ActiveUser?.ActiveBuilding != null);
        }

        private void Wire()
        {
            RootManagerInstance.ActiveUserChanged += RootManagerInstance_ActiveUserChanged;
        }

        private void UserSelection()
        {
            AdapterSelection SelectionAdapter = new AdapterSelection(ref RootManagerInstance, RootManagerInstance.UserList, "User");

            Selection NewControl = new Selection(SelectionAdapter);

            NewControl.SelectClicked += SelectionControl_SelectClicked;
            NewControl.ModifyClicked += SelectionControl_ModifyClicked;
            NewControl.AddClicked += SelectionControl_AddClicked;
            NewControl.DeleteClicked += SelectionControl_DeleteClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "UserSelection";

            ViewPortPanel.Controls.Add(NewControl);
        }

        private void BuildingSelection()
        {
            AdapterSelection SelectionAdapter = new AdapterSelection(ref RootManagerInstance, RootManagerInstance.ActiveUser.BuildingList, "Building");

            Selection NewControl = new Selection(SelectionAdapter);

            NewControl.SelectClicked += SelectionControl_SelectClicked;
            NewControl.ModifyClicked += SelectionControl_ModifyClicked;
            NewControl.AddClicked += SelectionControl_AddClicked;
            NewControl.DeleteClicked += SelectionControl_DeleteClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "BuildingSelection";

            ViewPortPanel.Controls.Add(NewControl);
        }

        private void AddNewUser()
        {
            UserInfo NewControl = new UserInfo();

            NewControl.ConfirmClicked += UserInfo_ConfirmClicked;
            NewControl.CancelClicked += UserInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["UserSelection"].Left + (ViewPortPanel.Controls["UserSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["UserSelection"].Top + (ViewPortPanel.Controls["UserSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "AddNewUser";

            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }
        private void AddNewBuilding()
        {
            BuildingInfo NewControl = new BuildingInfo();

            NewControl.ConfirmClicked += BuildingInfo_ConfirmClicked;
            NewControl.CancelClicked += BuildingInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["BuildingSelection"].Left + (ViewPortPanel.Controls["BuildingSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["BuildingSelection"].Top + (ViewPortPanel.Controls["BuildingSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "AddNewBuilding";

            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        private void ModifyUser(User _UsertoModify)
        {
            UserInfo NewControl = new UserInfo(_UsertoModify);

            NewControl.ConfirmClicked += UserInfo_ConfirmClicked;
            NewControl.CancelClicked += UserInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["UserSelection"].Left + (ViewPortPanel.Controls["UserSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["UserSelection"].Top + (ViewPortPanel.Controls["UserSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "ModifyUser";

            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        private void ModifyBuilding(Building _BuildingtoModify)
        {
            BuildingInfo NewControl = new BuildingInfo(_BuildingtoModify);

            NewControl.ConfirmClicked += BuildingInfo_ConfirmClicked;
            NewControl.CancelClicked += BuildingInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Left = ViewPortPanel.Controls["BuildingSelection"].Left + (ViewPortPanel.Controls["BuildingSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["BuildingSelection"].Top + (ViewPortPanel.Controls["BuildingSelection"].Height - NewControl.Height) / 2;
            NewControl.Name = "ModifyBuilding";

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
            UserSelection();
        }

        private void tsmiBuildingSelect_Click(object sender, EventArgs e)
        {
            ViewPortPanel.Controls.Clear();
            BuildingSelection();
        }

        private void tsmiTopDown_Click(object sender, EventArgs e)
        {
            ViewPortPanel.Controls.Clear();
            OpenTopDownBuildingView();
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            DataContinuityController.ShutdownDataContinuity(RootManagerInstance);
        }

        private void tsmiBuildingReport_Click(object sender, EventArgs e)
        {
            if (sfdBuildingReport.ShowDialog() == DialogResult.OK)
            {
                byte[] ImageData;

                using (MemoryStream CurrentMemoryStream = new MemoryStream())
                {
                    Properties.Resources.HomeCheckerFull.Save(CurrentMemoryStream, ImageFormat.Png);
                    ImageData = CurrentMemoryStream.ToArray();
                }

                ReportGenerator.GenerateListReport(RootManagerInstance, sfdBuildingReport.FileName, ImageData);
            }            
        }

        private void RootManagerInstance_ActiveUserChanged()
        {
            if (CurrentActiveUser != null)
            {
                CurrentActiveUser.ActiveBuildingChanged -= ActiveUser_ActiveBuildingChanged;
            }

            CurrentActiveUser = RootManagerInstance.ActiveUser;

            if (CurrentActiveUser != null)
            {
                CurrentActiveUser.ActiveBuildingChanged += ActiveUser_ActiveBuildingChanged;
            }

            tsmiBuildingSelect.Enabled = CurrentActiveUser != null;
            ActiveUser_ActiveBuildingChanged();
        }

        private void ActiveUser_ActiveBuildingChanged()
        {
            tsmiTopDown.Enabled = (CurrentActiveUser?.ActiveBuilding != null);
        }

        private void SelectionControl_SelectClicked(Selection _CurrentControl, Type _SelectedType, object _SelectedObject)
        {
            string? _ErrorMessage;

            switch (_SelectedType)
            {
                case Type CurrentType when _SelectedType == typeof(User):
                    if (RootManagerInstance.TryChangeActiveUser(_SelectedObject as User, out _ErrorMessage))
                    {
                        BuildingSelection();
                    }
                    else
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                case Type CurrentType when _SelectedType == typeof(Building):
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

            _CurrentControl.SelectClicked -= SelectionControl_SelectClicked;
            _CurrentControl.ModifyClicked -= SelectionControl_ModifyClicked;
            _CurrentControl.AddClicked -= SelectionControl_AddClicked;
            _CurrentControl.DeleteClicked -= SelectionControl_DeleteClicked;

            ViewPortPanel.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        private void SelectionControl_ModifyClicked(Selection _CurrentControl, Type _SelectedType, object _SelectedObject)
        {
            switch (_SelectedType)
            {
                case Type CurrentType when _SelectedType == typeof(User):
                    ModifyUser(_SelectedObject as User);
                    break;
                case Type CurrentType when _SelectedType == typeof(Building):
                    ModifyBuilding(_SelectedObject as Building);
                    break;
            }

            _CurrentControl.Enabled = false;
        }

        private void SelectionControl_AddClicked(Selection _CurrentControl, Type _SelectedType)
        {
            switch (_SelectedType)
            {
                case Type CurrentType when _SelectedType == typeof(User):
                    AddNewUser();
                    break;
                case Type CurrentType when _SelectedType == typeof(Building):
                    AddNewBuilding();
                    break;
            }

            _CurrentControl.Enabled = false;
        }

        private void SelectionControl_DeleteClicked(Type _SelectedType, object _SelectedObject)
        {
            string? _ErrorMessage;

            switch (_SelectedType)
            {
                case Type CurrentType when _SelectedType == typeof(User):
                    if (!RootManagerInstance.TryRemoveUser(_SelectedObject as User, out _ErrorMessage))
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case Type CurrentType when _SelectedType == typeof(Building):
                    if (!RootManagerInstance.ActiveUser.TryRemoveBuilding(_SelectedObject as Building, out _ErrorMessage))
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        private void UserInfo_ConfirmClicked(FormType _FormType, User? _CurrentUser, UserInfo _CurrentControl, string _AddedUsername)
        {
            string? _ErrorMessage;

            if (_FormType == FormType.Add)
            {
                if (RootManagerInstance.TryAddUser(_AddedUsername, out _ErrorMessage))
                {
                    UserInfo_CancelClicked(_CurrentControl);
                }
                else
                {
                    MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (_FormType == FormType.Modify)
            {
                if (RootManagerInstance.TryModifyUser(_CurrentUser, _AddedUsername, out _ErrorMessage))
                {
                    UserInfo_CancelClicked(_CurrentControl);
                }
                else
                {
                    MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UserInfo_CancelClicked(UserInfo _CurrentControl)
        {
            _CurrentControl.ConfirmClicked -= UserInfo_ConfirmClicked;
            _CurrentControl.CancelClicked -= UserInfo_CancelClicked;

            ViewPortPanel.Controls["UserSelection"].Enabled = true;
            ViewPortPanel.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        private void BuildingInfo_ConfirmClicked(FormType _FormType, Building? _CurrentBuilding, BuildingInfo _CurrentControl, (string _Name, float _Width, float _Height) _BuildingValues)
        {
            string? _ErrorMessage = null;

            if (_FormType == FormType.Add)
            {
                if (RootManagerInstance.ActiveUser.TryAddBuilding(_BuildingValues._Name, _BuildingValues._Width, _BuildingValues._Height, out _ErrorMessage))
                {
                    BuildingInfo_CancelClicked(_CurrentControl);
                }
                else
                {
                    MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (_FormType == FormType.Modify)
            {
                if (RootManagerInstance.ActiveUser.TryModifyBuilding(_CurrentBuilding, _BuildingValues._Name, _BuildingValues._Width, _BuildingValues._Height, out _ErrorMessage))
                {
                    BuildingInfo_CancelClicked(_CurrentControl);
                }
                else
                {
                    MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BuildingInfo_CancelClicked(BuildingInfo _CurrentControl)
        {
            _CurrentControl.ConfirmClicked -= BuildingInfo_ConfirmClicked;
            _CurrentControl.CancelClicked -= BuildingInfo_CancelClicked;

            ViewPortPanel.Controls["BuildingSelection"].Enabled = true;

            ViewPortPanel.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }
    }
}
