using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using BackEnd.Reports;
using FrontEnd.Adapters;
using FrontEnd.UserControls;
using FrontEnd.Utilities;
using System.Drawing.Imaging;

namespace FrontEnd.Forms
{
    /// <summary>
    /// The Main Program Dashboard That All Other Controls And Screens Are Housed In
    /// </summary>
    internal partial class Dashboard : Form
    {
        /// <summary>
        /// Creates The Top-Level Root Manager Object, And Pulls From Long-Term Storage If It Exists
        /// </summary>
        private RootManager RootManagerInstance = DataContinuityController.StartupDataContinuity();

        /// <summary>
        /// The Panel That Serves As The Viewport For Displaying Controls
        /// </summary>
        private Panel ViewPortPanel;

        /// <summary>
        /// The Current Active User Of The Program
        /// </summary>
        private User? CurrentActiveUser;

        /// <summary>
        /// Initializes The Dashboard Form
        /// </summary>
        internal Dashboard()
        {
            InitializeComponent();

            ViewPortPanel = this.pnlDashboard;

            InitializeVisuals();
            Wire();
        }

        /// <summary>
        /// Initializes Visual State Of The Dashboard
        /// </summary>
        private void InitializeVisuals()
        {
            this.WindowState = FormWindowState.Maximized;

            // Clears Any Existing Controls In The Viewport
            ViewPortPanel.Controls.Clear();

            // Enables Or Disables Menu Options Based On Current State
            tsmiBuildingSelect.Enabled = (CurrentActiveUser != null);
            tsmiTopDown.Enabled = (CurrentActiveUser?.ActiveBuilding != null);
            tsmiBuildingReport.Enabled = (CurrentActiveUser?.ActiveBuilding != null);
        }

        /// <summary>
        /// Wires Backend Events To UI Handlers
        /// </summary>
        private void Wire()
        {
            RootManagerInstance.ActiveUserChanged += RootManagerInstance_ActiveUserChanged;
        }

        /// <summary>
        /// Handles Dashboard Load Event For Visualization That Requires Load First To Be Accurate
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void Dashboard_Load(object sender, EventArgs e)
        {
            // Defers Execution Until UI Is Fully Loaded
            this.BeginInvoke(() =>
            {
                UserSelection();
            });
        }

        /// <summary>
        /// Displays User Selection Control
        /// </summary>
        private void UserSelection()
        {
            AdapterSelection SelectionAdapter = new AdapterSelection(ref RootManagerInstance, RootManagerInstance.UserList, "User");

            Selection NewControl = new Selection(SelectionAdapter);

            // Wires Selection Control Events
            NewControl.SelectClicked += SelectionControl_SelectClicked;
            NewControl.ModifyClicked += SelectionControl_ModifyClicked;
            NewControl.AddClicked += SelectionControl_AddClicked;
            NewControl.DeleteClicked += SelectionControl_DeleteClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "UserSelection";

            // Centers Control In Viewport
            NewControl.Left = ViewPortPanel.ClientSize.Width / 2 - NewControl.Width / 2;
            NewControl.Top = ViewPortPanel.ClientSize.Height / 2 - NewControl.Height / 2;

            // Displays Control
            ViewPortPanel.Controls.Add(NewControl);
        }

        /// <summary>
        /// Displays Building Selection Control
        /// </summary>
        private void BuildingSelection()
        {
            AdapterSelection SelectionAdapter = new AdapterSelection(ref RootManagerInstance, CurrentActiveUser.BuildingList, "Building");

            Selection NewControl = new Selection(SelectionAdapter);

            // Wires Selection Control Events
            NewControl.SelectClicked += SelectionControl_SelectClicked;
            NewControl.ModifyClicked += SelectionControl_ModifyClicked;
            NewControl.AddClicked += SelectionControl_AddClicked;
            NewControl.DeleteClicked += SelectionControl_DeleteClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "BuildingSelection";

            // Centers Control In Viewport
            NewControl.Left = ViewPortPanel.ClientSize.Width / 2 - NewControl.Width / 2;
            NewControl.Top = ViewPortPanel.ClientSize.Height / 2 - NewControl.Height / 2;

            // Displays Control
            ViewPortPanel.Controls.Add(NewControl);
        }

        /// <summary>
        /// Displays Add User Control
        /// </summary>
        private void AddNewUser()
        {
            UserInfo NewControl = new UserInfo();

            // Wires Control Events
            NewControl.ConfirmClicked += UserInfo_ConfirmClicked;
            NewControl.CancelClicked += UserInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "AddNewUser";

            // Positions Control Over Selection Control
            NewControl.Left = ViewPortPanel.Controls["UserSelection"].Left + (ViewPortPanel.Controls["UserSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["UserSelection"].Top + (ViewPortPanel.Controls["UserSelection"].Height - NewControl.Height) / 2;

            // Displays Control, And Ensures It Is At The Front Of Other Controls
            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        /// <summary>
        /// Displays Add Building Control
        /// </summary>
        private void AddNewBuilding()
        {
            BuildingInfo NewControl = new BuildingInfo();

            // Wires Control Events
            NewControl.ConfirmClicked += BuildingInfo_ConfirmClicked;
            NewControl.CancelClicked += BuildingInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "AddNewBuilding";

            // Positions Control Over Selection Control
            NewControl.Left = ViewPortPanel.Controls["BuildingSelection"].Left + (ViewPortPanel.Controls["BuildingSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["BuildingSelection"].Top + (ViewPortPanel.Controls["BuildingSelection"].Height - NewControl.Height) / 2;

            // Displays Control, And Ensures It Is At The Front Of Other Controls
            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        /// <summary>
        /// Displays Modify User Control
        /// </summary>
        /// <param name="_UsertoModify">The User Object To Modify</param>
        private void ModifyUser(User _UsertoModify)
        {
            UserInfo NewControl = new UserInfo(_UsertoModify);

            // Wires Control Events
            NewControl.ConfirmClicked += UserInfo_ConfirmClicked;
            NewControl.CancelClicked += UserInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "ModifyUser";

            // Positions Control Over Selection Control
            NewControl.Left = ViewPortPanel.Controls["UserSelection"].Left + (ViewPortPanel.Controls["UserSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["UserSelection"].Top + (ViewPortPanel.Controls["UserSelection"].Height - NewControl.Height) / 2;

            // Displays Control, And Ensures It Is At The Front Of Other Controls
            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        /// <summary>
        /// Displays Modify Building Control
        /// </summary>
        private void ModifyBuilding(Building _BuildingtoModify)
        {
            BuildingInfo NewControl = new BuildingInfo(_BuildingtoModify);

            // Wires Control Events
            NewControl.ConfirmClicked += BuildingInfo_ConfirmClicked;
            NewControl.CancelClicked += BuildingInfo_CancelClicked;

            NewControl.Dock = DockStyle.None;
            NewControl.Name = "ModifyBuilding";

            // Positions Control Over Selection Control
            NewControl.Left = ViewPortPanel.Controls["BuildingSelection"].Left + (ViewPortPanel.Controls["BuildingSelection"].Width - NewControl.Width) / 2;
            NewControl.Top = ViewPortPanel.Controls["BuildingSelection"].Top + (ViewPortPanel.Controls["BuildingSelection"].Height - NewControl.Height) / 2;
            
            // Displays Control, And Ensures It Is At The Front Of Other Controls
            ViewPortPanel.Controls.Add(NewControl);
            NewControl.BringToFront();
        }

        /// <summary>
        /// Opens Top-Down Building View Control
        /// </summary>
        private void OpenTopDownBuildingView()
        {
            TopDownBuildingView NewControl = new TopDownBuildingView(ref RootManagerInstance);

            NewControl.Dock = DockStyle.Fill;
            NewControl.Name = "TopDownBuildingView";

            // Displays Control
            ViewPortPanel.Controls.Add(NewControl);
        }

        /// <summary>
        /// Handles User Selection Tool Strip Menu Click
        /// </summary>
        private void tsmiUserSelect_Click(object sender, EventArgs e)
        {
            ViewPortPanel.Controls.Clear();
            UserSelection();
        }

        /// <summary>
        /// Handles Building Selection Tool Strip Menu Click
        /// </summary>
        private void tsmiBuildingSelect_Click(object sender, EventArgs e)
        {
            ViewPortPanel.Controls.Clear();
            BuildingSelection();
        }

        /// <summary>
        /// Handles Top-Down View Tool Strip Menu Click
        /// </summary>
        private void tsmiTopDown_Click(object sender, EventArgs e)
        {
            ViewPortPanel.Controls.Clear();
            OpenTopDownBuildingView();
        }

        /// <summary>
        /// Handles Save Tool Strip Menu Click
        /// </summary>
        private void tsmiSave_Click(object sender, EventArgs e)
        {
            // Serializes And Stores Live Session Data In JSON
            DataContinuityController.ShutdownDataContinuity(RootManagerInstance);
        }

        /// <summary>
        /// Handles Generate Building Report Tool Strip Menu Click
        /// </summary>
        private void tsmiBuildingReport_Click(object sender, EventArgs e)
        {
            if (sfdBuildingReport.ShowDialog() == DialogResult.OK)
            {
                // Converts The Program Logo To Byte Array To Be Used In The Report
                byte[] ImageData;

                using (MemoryStream CurrentMemoryStream = new MemoryStream())
                {
                    Properties.Resources.HomeCheckerFull.Save(CurrentMemoryStream, ImageFormat.Png);
                    ImageData = CurrentMemoryStream.ToArray();
                }

                ReportGenerator.GenerateListReport(RootManagerInstance, sfdBuildingReport.FileName, ImageData);
            }
        }

        /// <summary>
        /// Handles Active User Change Event
        /// </summary>
        private void RootManagerInstance_ActiveUserChanged()
        {
            // Unwires Current Active User Event, If There Is A Current Active User
            if (CurrentActiveUser != null)
            {
                CurrentActiveUser.ActiveBuildingChanged -= ActiveUser_ActiveBuildingChanged;
            }

            // Sets The New Active User
            CurrentActiveUser = RootManagerInstance.ActiveUser;

            // Wires Current Active User Event, If There Is A Current Active User
            if (CurrentActiveUser != null)
            {
                CurrentActiveUser.ActiveBuildingChanged += ActiveUser_ActiveBuildingChanged;
            }

            tsmiBuildingSelect.Enabled = CurrentActiveUser != null;
            ActiveUser_ActiveBuildingChanged();
        }

        /// <summary>
        /// Handles Active Building Change Event
        /// </summary>
        private void ActiveUser_ActiveBuildingChanged()
        {
            tsmiTopDown.Enabled = (CurrentActiveUser?.ActiveBuilding != null);
            tsmiBuildingReport.Enabled = (CurrentActiveUser?.ActiveBuilding != null);
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
                    if (CurrentActiveUser.TryChangeActiveBuilding(_SelectedObject as Building, out _ErrorMessage))
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
                    if (!CurrentActiveUser.TryRemoveBuilding(_SelectedObject as Building, out _ErrorMessage))
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
                if (CurrentActiveUser.TryAddBuilding(_BuildingValues._Name, _BuildingValues._Width, _BuildingValues._Height, out _ErrorMessage))
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
                if (CurrentActiveUser.TryModifyBuilding(_CurrentBuilding, _BuildingValues._Name, _BuildingValues._Width, _BuildingValues._Height, out _ErrorMessage))
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
