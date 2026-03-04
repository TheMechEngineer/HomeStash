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
        internal event Action<Selection, Type, object>? SelectClicked;
        internal event Action<Selection, Type>? AddClicked;
        internal event Action<Selection, Type, object>? ModifyClicked;
        internal event Action<Type, object>? DeleteClicked;

        private AdapterSelection SelectionAdapter;
        private int InitialFLPClientWidth;

        private Color UnselectedLabelColor = Color.White;
        private Color SelectedLabelColor = Color.Beige;
        private Label? SelectedLabel;

        internal Selection(AdapterSelection _SelectionAdapter)
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
        private void SetDisplayText()
        {
            string ControlText = SelectionAdapter.ButtonText;

            lblSelectionTitle.Text = ControlText + " Selection Menu";
            btnSelect.Text = "Select " + ControlText;
            btnModify.Text = "Modify " + ControlText;
            btnAdd.Text = "Add " + ControlText;
            btnDelete.Text = "Delete " + ControlText;

        }
        private void PopulateSelectionList()
        {
            string DisplayName;

            flpSelectionList.Controls.Clear();

            foreach (AdapterSelectionItem CurrentSelection in SelectionAdapter.GetAList())
            {
                DisplayName = CurrentSelection.DisplayText;

                Label NewSelectionItem = new Label();
                NewSelectionItem.Name = DisplayName;
                NewSelectionItem.Text = DisplayName;
                NewSelectionItem.Margin = new Padding(3);
                NewSelectionItem.Width = InitialFLPClientWidth - NewSelectionItem.Margin.Left - NewSelectionItem.Margin.Right;
                NewSelectionItem.TextAlign = ContentAlignment.MiddleCenter;
                NewSelectionItem.BackColor = Color.White;
                NewSelectionItem.Height = 40;
                NewSelectionItem.Click += Label_Click!;
                NewSelectionItem.Tag = CurrentSelection.Value;
                flpSelectionList.Controls.Add(NewSelectionItem);

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
                SelectClicked?.Invoke(this, SelectionAdapter.SelectionType, SelectedLabel.Tag);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (SelectedLabel != null)
            {
                ModifyClicked?.Invoke(this, SelectionAdapter.SelectionType, SelectedLabel.Tag);
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddClicked?.Invoke(this, SelectionAdapter.SelectionType);
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedLabel != null)
            {
                string MessagePrompt = $"Do You Want To Delete {SelectedLabel.Text}\nThis Is Permanent And Cannot Be Undone";

                if (MessageBox.Show(MessagePrompt, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteClicked?.Invoke(SelectionAdapter.SelectionType, SelectedLabel.Tag);
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
