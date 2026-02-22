using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using FrontEnd.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd.UserControls
{
    public partial class Selection : UserControl
    {
        private RootManager RootManagerInstance;
        private IReadOnlyList<object> SelectionList;
        private Type SelectionType;

        Color UnselectedLabelColor = Color.White;
        Color SelectedLabelColor = Color.Beige;

        Label? SelectedLabel;

        int InitialFLPClientWidth;

        public Selection(ref RootManager _ProgramRoot, IReadOnlyList<object> _SelectionList)
        {
            InitializeComponent();
            
            RootManagerInstance = _ProgramRoot;
            SelectionList = _SelectionList;
            SelectionType = _SelectionList.GetType().GetGenericArguments()[0];

            InitializeForm();
            WireUpForm();
        }

        private void InitializeForm()
        {
            InitialFLPClientWidth = flpSelectionList.ClientSize.Width;

            SetDisplayText();
            PopulateSelectionList();
        }

        private void WireUpForm()
        {
            
            switch (SelectionType)
            {
                case Type CurrentType when SelectionType == typeof(User):
                    RootManagerInstance.UserListChanged += PopulateSelectionList;
                    break;
                case Type CurrentType when SelectionType == typeof(Building):
                    RootManagerInstance.ActiveUser.BuildingListChanged += PopulateSelectionList;
                    break;
            }
        }

        private void SetDisplayText() {
            string ControlText;

            switch (SelectionType)
            {
                case Type CurrentType when SelectionType == typeof(User):
                    ControlText = "User";
                    break;
                case Type CurrentType when SelectionType == typeof(Building):
                    ControlText = "Building";
                    break;
                default:
                    ControlText = "Default";
                    break;
            }

            lblSelectionTitle.Text = ControlText + " Selection Menu";
            btnAdd.Text = "Add " + ControlText;
            btnDelete.Text = "Delete " + ControlText;
            btnSelect.Text = "Select " + ControlText;
        }

        private void PopulateSelectionList()
        {
            string DisplayName;

            flpSelectionList.Controls.Clear();

            foreach (object CurrentSelection in SelectionList)
            {
                switch (CurrentSelection)
                {
                    case User CurrentUser:
                        DisplayName = CurrentUser.UserName;
                        break;
                    case Building CurrentBuilding:
                        DisplayName = CurrentBuilding.Name;
                        break;
                    default:
                        DisplayName = "Invalid Class";
                        break;
                }

                Label NewUser = new Label();
                NewUser.Name = DisplayName;
                NewUser.Text = DisplayName;
                NewUser.Margin = new Padding(3);
                NewUser.Width = InitialFLPClientWidth - NewUser.Margin.Left - NewUser.Margin.Right;
                NewUser.TextAlign = ContentAlignment.MiddleCenter;
                NewUser.BackColor = Color.White;
                NewUser.Height = 40;
                NewUser.Click += Label_Click!;
                NewUser.Tag = CurrentSelection;
                flpSelectionList.Controls.Add(NewUser);

                if (flpSelectionList.HorizontalScroll.Visible == true && flpSelectionList.VerticalScroll.Visible == true)
                {
                    flpSelectionList.ClientSize = new Size(InitialFLPClientWidth + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, flpSelectionList.ClientSize.Height + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight);
                }
                else if (flpSelectionList.VerticalScroll.Visible == false && flpSelectionList.Controls[0].Width < flpSelectionList.Width - flpSelectionList.Controls[0].Margin.Left - flpSelectionList.Controls[0].Margin.Right)
                {
                    flpSelectionList.ClientSize = new Size(InitialFLPClientWidth, flpSelectionList.ClientSize.Height);
                }
            }

        }
        private void buttonSelect_Click(object sender, EventArgs e)
        {

            if (SelectedLabel != null)
            {
                switch (SelectionType)
                {
                    case Type CurrentType when SelectionType == typeof(User):
                        RootManagerInstance.ActiveUser = SelectedLabel.Tag as User;
                        
                        (this.FindForm() as Dashboard).OpenBuildingSelection();

                        this.Parent.Controls.Remove(this);
                        this.Dispose();

                        break;
                    case Type CurrentType when SelectionType == typeof(Building):
                        RootManagerInstance.ActiveUser.ActiveBuilding = SelectedLabel.Tag as Building;

                        (this.FindForm() as Dashboard).OpenTopDownBuildingView();

                        this.Parent.Controls.Remove(this);
                        this.Dispose();

                        break;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            switch (SelectionType)
            {
                case Type CurrentType when SelectionType == typeof(User):
                    (this.FindForm() as Dashboard).OpenAddNewUser();

                    break;
                case Type CurrentType when SelectionType == typeof(Building):
                    (this.FindForm() as Dashboard).OpenAddNewBuilding();
                    break;
            }
            
            this.Enabled = false;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedLabel != null)
            {
                string MessagePrompt = $"Do You Want To Delete {SelectedLabel.Text}\nThis Is Permanent And Cannot Be Undone";

                if (MessageBox.Show(MessagePrompt, "Confirm Deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    switch (SelectionType)
                    {
                        case Type CurrentType when SelectionType == typeof(User):
                            RootManagerInstance.RemoveUser(SelectedLabel.Tag as User);

                            break;
                        case Type CurrentType when SelectionType == typeof(Building):
                            RootManagerInstance.ActiveUser.RemoveBuilding(SelectedLabel.Tag as Building);
                            break;
                    }
                    
                }

                SelectedLabel = null;
            }

        }

        private void Label_Click(object sender, EventArgs e)
        {

            // This is pattern matching. Alternate Approach:
            // Label clickedLabel = sender as Label
            if (sender is Label clickedLabel)
            {
                if (SelectedLabel != null)
                {
                    SelectedLabel.BackColor = UnselectedLabelColor;
                }

                SelectedLabel = clickedLabel;
                SelectedLabel.BackColor = SelectedLabelColor;

                // Get index in the FlowLayoutPanel
                //int index = flpUserList.Controls.IndexOf(clickedLabel);
                //MessageBox.Show($"Clicked label at index {index}: {clickedLabel.Text}");
            }

        }
    }
}
