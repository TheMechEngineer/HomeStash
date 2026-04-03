using BackEnd.ModelClasses;
using FrontEnd.Utilities;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Handles Display And Interaction For Adding Or Modifying A Building
    /// </summary>
    internal partial class BuildingInfo : UserControl
    {
        /// <summary>
        /// Event Triggered When The Confirm Button Is Clicked
        /// </summary>
        internal event Action<FormType, Building?, BuildingInfo, (string Name, float Width, float Height)>? ConfirmClicked;

        /// <summary>
        /// Event Triggered When The Cancel Button Is Clicked
        /// </summary>
        internal event Action<BuildingInfo>? CancelClicked;

        /// <summary>
        /// The Building Object Being Modified, Null If Adding A New Building
        /// </summary>
        private Building? CurrentBuilding;

        /// <summary>
        /// Indicates Whether The Form Is In Add Or Modify Mode
        /// </summary>
        private FormType CurrentFormType;

        /// <summary>
        /// Initializes The BuildingInfo Control In Add Mode
        /// </summary>
        internal BuildingInfo()
        {
            InitializeComponent();

            CurrentFormType = FormType.Add;

            InitializeVisuals();
        }

        /// <summary>
        /// Initializes The BuildingInfo Control In Modify Mode With The Provided Building
        /// </summary>
        /// <param name="_BuildingToModify">The Building Object To Modify</param>
        internal BuildingInfo(Building _BuildingToModify)
        {
            InitializeComponent();

            CurrentFormType = FormType.Modify;
            CurrentBuilding = _BuildingToModify;

            InitializeVisuals();
        }

        /// <summary>
        /// Initializes Visual Elements Of The BuildingInfo Control Based On Form Mode
        /// </summary>
        private void InitializeVisuals()
        {
            if (CurrentFormType == FormType.Add)
            {
                lblTitle.Text = "Add New Building";
            }
            else if (CurrentFormType == FormType.Modify)
            {
                lblTitle.Text = "Modify Building";
                txtNameInput.Text = CurrentBuilding.Name;
                txtWidthInput.Text = CurrentBuilding.Width.ToString();
                txtHeightInput.Text = CurrentBuilding.Height.ToString();
            }
        }

        /// <summary>
        /// Handles BuildingInfo Load Event For Visualization That Requires Load First To Be Accurate
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void BuildingInfo_Load(object sender, EventArgs e)
        {
            SizeForm();

            txtNameInput.Focus();
        }

        /// <summary>
        /// Sizes And Positions Controls Within The Form
        /// </summary>
        private void SizeForm()
        {
            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            int LabelTextSizeDiff = txtNameInput.Height - lblBuildingName.Height;
            int MaxLabelSize = new int[] { lblBuildingName.Width, lblBuildingHeight.Width, lblBuildingWidth.Width }.Max();
            int TextBoxWidth = this.ClientSize.Width - 2 * Gap - MaxLabelSize;

            // Right Align All Labels That Are Not The Title Label Based On The Longest Label
            foreach (Label CurrentLabel in this.Controls.OfType<Label>())
            {
                if (CurrentLabel == lblTitle)
                {
                    continue;
                }
                else
                {
                    CurrentLabel.Left = Gap + (MaxLabelSize - CurrentLabel.Width);
                }
            }

            // Center Title Label
            lblTitle.Left = this.ClientSize.Width / 2 - lblTitle.Width / 2;
            lblTitle.Top = SmallGap;

            // Position Building Name Label And TextBox
            lblBuildingName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtNameInput.Width = TextBoxWidth;
            txtNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtNameInput.Top = lblTitle.Bottom + SmallGap;

            // Position Width Label And TextBox
            lblBuildingWidth.Top = lblBuildingName.Bottom + SmallGap + LabelTextSizeDiff;

            txtWidthInput.Width = TextBoxWidth;
            txtWidthInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtWidthInput.Top = txtNameInput.Bottom + SmallGap;

            // Position Height Label And TextBox
            lblBuildingHeight.Top = lblBuildingWidth.Bottom + SmallGap + LabelTextSizeDiff;

            txtHeightInput.Width = TextBoxWidth;
            txtHeightInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtHeightInput.Top = txtWidthInput.Bottom + SmallGap;

            // Position Confirm Button
            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = txtHeightInput.Bottom + Gap;

            // Position Cancel Button
            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = txtHeightInput.Bottom + Gap;

            // Adjust User Control Height Based On Sized And Positioned Controls
            this.ClientSize = new Size(this.ClientSize.Width, btnConfirm.Bottom + Gap);
        }

        /// <summary>
        /// Handles Confirm Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmClicked?.Invoke(CurrentFormType, CurrentBuilding, this,
                    (
                    txtNameInput.Text,
                    Convert.ToSingle(txtWidthInput.Text),
                    Convert.ToSingle(txtHeightInput.Text)
                    )
                );
            }
            catch (FormatException Exc)
            {
                MessageBox.Show("Format Error: Width And Height Must Be Numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles Cancel Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }
    }
}