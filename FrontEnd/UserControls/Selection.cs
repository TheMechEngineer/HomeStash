using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using FrontEnd.Adapters;
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
    internal partial class Selection : UserControl
    {
        internal event Action<Selection, Type, object>? ChooseSelection;
        internal event Action<Selection, Type>? AddNewSelection;
        internal event Action<Type, object>? DeleteSelection;

        private ASelection SelectionAdapter;
        private int InitialFLPClientWidth;

        private Color UnselectedLabelColor = Color.White;
        private Color SelectedLabelColor = Color.Beige;
        private Label? SelectedLabel;

        internal Selection(ASelection _SelectionAdapter)
        {
            InitializeComponent();

            SelectionAdapter = _SelectionAdapter;
            InitialFLPClientWidth = flpSelectionList.ClientSize.Width;

            InitializeVisuals();
            Wire();
        }

        private void InitializeVisuals()
        {
            SetDisplayText();
            PopulateSelectionList();
        }

        private void Wire()
        {
            SelectionAdapter.SourceUpdated += PopulateSelectionList;
            this.HandleDestroyed += UnWire;
        }
        private void UnWire(object? sender, EventArgs e)
        {
            SelectionAdapter.SourceUpdated -= PopulateSelectionList;
            this.HandleDestroyed -= UnWire;
        }
        private void SetDisplayText() {
            string ControlText = SelectionAdapter.ButtonText;

            lblSelectionTitle.Text = ControlText + " Selection Menu";
            btnAdd.Text = "Add " + ControlText;
            btnDelete.Text = "Delete " + ControlText;
            btnSelect.Text = "Select " + ControlText;
        }
        private void PopulateSelectionList()
        {
            string DisplayName;

            flpSelectionList.Controls.Clear();

            foreach (ASelectionItem CurrentSelection in SelectionAdapter.GetAList())
            {
                DisplayName = CurrentSelection.DisplayText;

                Label NewUser = new Label();
                NewUser.Name = DisplayName;
                NewUser.Text = DisplayName;
                NewUser.Margin = new Padding(3);
                NewUser.Width = InitialFLPClientWidth - NewUser.Margin.Left - NewUser.Margin.Right;
                NewUser.TextAlign = ContentAlignment.MiddleCenter;
                NewUser.BackColor = Color.White;
                NewUser.Height = 40;
                NewUser.Click += Label_Click!;
                NewUser.Tag = CurrentSelection.Value;
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
                ChooseSelection?.Invoke(this, SelectionAdapter.SelectionType, SelectedLabel.Tag);
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddNewSelection?.Invoke(this, SelectionAdapter.SelectionType);
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedLabel != null)
            {
                string MessagePrompt = $"Do You Want To Delete {SelectedLabel.Text}\nThis Is Permanent And Cannot Be Undone";

                if (MessageBox.Show(MessagePrompt, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteSelection?.Invoke(SelectionAdapter.SelectionType, SelectedLabel.Tag);
                }
                else
                {
                    SelectedLabel.BackColor = UnselectedLabelColor;
                }

                SelectedLabel = null;
            }

        }

        private void Label_Click(object sender, EventArgs e)
        {

            // This is pattern matching. Alternate Approach:
            // Label ClickedLabel = sender as Label
            if (sender is Label ClickedLabel)
            {
                if (SelectedLabel != null)
                {
                    SelectedLabel.BackColor = UnselectedLabelColor;
                }

                SelectedLabel = ClickedLabel;
                SelectedLabel.BackColor = SelectedLabelColor;

                // Get index in the FlowLayoutPanel
                //int index = flpUserList.Controls.IndexOf(ClickedLabel);
                //MessageBox.Show($"Clicked label at index {index}: {ClickedLabel.Text}");
            }
        }
    }
}
