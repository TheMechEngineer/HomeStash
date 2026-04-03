using BackEnd.ModelClasses;
using FrontEnd.Utilities;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Handles Display And Interaction For Adding Or Modifying A Room
    /// </summary>
    internal partial class RoomInfo : UserControl
    {
        /// <summary>
        /// Event Triggered When The Confirm Button Is Clicked
        /// </summary>
        internal event Action<FormType, Room?, RoomInfo, (string Name, float Width, float Height, float CenterX, float CenterY, int ColorValue)>? ConfirmClicked;

        /// <summary>
        /// Event Triggered When The Cancel Button Is Clicked
        /// </summary>
        internal event Action<RoomInfo>? CancelClicked;

        /// <summary>
        /// The Room Object Being Modified, Null If Adding A New Room
        /// </summary>
        private Room? CurrentRoom;

        /// <summary>
        /// Indicates Whether The Form Is In Add Or Modify Mode
        /// </summary>
        private FormType CurrentFormType;

        /// <summary>
        /// Initializes The RoomInfo Control In Add Mode
        /// </summary>
        internal RoomInfo()
        {
            InitializeComponent();

            CurrentFormType = FormType.Add;

            InitializeVisuals();
        }

        /// <summary>
        /// Initializes The RoomInfo Control In Modify Mode With The Provided Room
        /// </summary>
        /// <param name="_RoomToModify">The Room Object To Modify</param>
        internal RoomInfo(Room _RoomToModify)
        {
            InitializeComponent();

            CurrentFormType = FormType.Modify;
            CurrentRoom = _RoomToModify;

            InitializeVisuals();
        }

        /// <summary>
        /// Initializes Visual Elements Of The RoomInfo Control Based On Form Mode
        /// </summary>
        private void InitializeVisuals()
        {
            if (CurrentFormType == FormType.Add)
            {
                lblTitle.Text = "Add New Room";

                txtColorInput.Text = Color.Green.ToArgb().ToString();
                txtColorInput.BackColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
                txtColorInput.ForeColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
            }
            else if (CurrentFormType == FormType.Modify)
            {
                lblTitle.Text = "Modify Room";

                txtNameInput.Text = CurrentRoom.Name;
                txtWidthInput.Text = CurrentRoom.Width.ToString();
                txtHeightInput.Text = CurrentRoom.Height.ToString();
                txtXCoordInput.Text = CurrentRoom.CenterX.ToString();
                txtYCoordInput.Text = CurrentRoom.CenterY.ToString();

                txtColorInput.Text = CurrentRoom.RoomColor.ToString();
                txtColorInput.BackColor = Color.FromArgb(CurrentRoom.RoomColor);
                txtColorInput.ForeColor = Color.FromArgb(CurrentRoom.RoomColor);
            }
        }

        /// <summary>
        /// Handles RoomInfo Load Event For Visualization That Requires Load First To Be Accurate
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void RoomInfo_Load(object sender, EventArgs e)
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

            int LabelTextSizeDiff = txtNameInput.Height - lblRoomName.Height;
            int MaxLabelSize = new int[] { lblRoomName.Width, lblRoomWidth.Width, lblRoomHeight.Width, lblRoomLocation.Width, lblRoomColor.Width }.Max();
            int TextBoxWidth = this.ClientSize.Width - 2 * Gap - MaxLabelSize;

            // Right Align All Labels That Are Not The Title Label
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

            // Position Room Name Label and TextBox
            lblRoomName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtNameInput.Width = TextBoxWidth;
            txtNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtNameInput.Top = lblTitle.Bottom + SmallGap;

            // Position Width Label and TextBox
            lblRoomWidth.Top = lblRoomName.Bottom + SmallGap + LabelTextSizeDiff;

            txtWidthInput.Width = TextBoxWidth;
            txtWidthInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtWidthInput.Top = txtNameInput.Bottom + SmallGap;

            // Position Height Label and TextBox
            lblRoomHeight.Top = lblRoomWidth.Bottom + SmallGap + LabelTextSizeDiff;

            txtHeightInput.Width = TextBoxWidth;
            txtHeightInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtHeightInput.Top = txtWidthInput.Bottom + SmallGap;

            // Position Coordinates Labels and TextBoxes
            lblRoomLocation.Top = lblRoomHeight.Bottom + SmallGap + LabelTextSizeDiff;

            lblX.Top = lblRoomLocation.Top;
            lblX.Left = lblRoomLocation.Right;

            txtXCoordInput.Width = (TextBoxWidth - lblX.Width - lblY.Width) / 2;
            txtXCoordInput.Left = lblX.Right;
            txtXCoordInput.Top = txtHeightInput.Bottom + SmallGap;

            lblY.Top = lblRoomLocation.Top;
            lblY.Left = txtXCoordInput.Right;

            txtYCoordInput.Width = (TextBoxWidth - lblX.Width - lblY.Width) / 2;
            txtYCoordInput.Left = lblY.Right;
            txtYCoordInput.Top = txtHeightInput.Bottom + SmallGap;

            // Position Color Label and TextBox
            lblRoomColor.Top = lblRoomLocation.Bottom + SmallGap + LabelTextSizeDiff;

            txtColorInput.Width = TextBoxWidth;
            txtColorInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtColorInput.Top = txtYCoordInput.Bottom + SmallGap;

            // Position Confirm Button
            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = txtColorInput.Bottom + Gap;

            // Position Cancel Button
            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = txtColorInput.Bottom + Gap;

            // Adjust User Control Height Based On Positioned Controls
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
                ConfirmClicked?.Invoke(CurrentFormType, CurrentRoom, this,
                    (
                        txtNameInput.Text,
                        Convert.ToSingle(txtWidthInput.Text),
                        Convert.ToSingle(txtHeightInput.Text),
                        Convert.ToSingle(txtXCoordInput.Text),
                        Convert.ToSingle(txtYCoordInput.Text),
                        Convert.ToInt32(txtColorInput.Text)
                    )
                );
            }
            catch (FormatException Exc)
            {
                MessageBox.Show("Format Error: Width, Height, And Coordinates Must Be Numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// Handles MouseDown Event For The Color TextBox
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Mouse Event Arguments</param>
        private void txtColorInput_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null; //Prevents A Cursor From Appearing In The Color Text Box
            if (cldRoomColor.ShowDialog() == DialogResult.OK)
            {
                txtColorInput.Text = cldRoomColor.Color.ToArgb().ToString();
            }

            // Modify The Text Box Colors So It Shows As The Color The User Selected
            txtColorInput.BackColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
            txtColorInput.ForeColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
        }
    }
}