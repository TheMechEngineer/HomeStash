using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using FrontEnd.Forms;
using FrontEnd.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd.UserControls
{
    internal partial class RoomInfo : UserControl
    {
        internal event Action<FormType, Room?, RoomInfo, (string Name, float Width, float Height, float CenterX, float CenterY, int ColorValue)>? ConfirmClicked;
        internal event Action<RoomInfo>? CancelClicked;

        private Room? CurrentRoom;
        private FormType CurrentFormType;

        internal RoomInfo()
        {
            InitializeComponent();

            CurrentFormType = FormType.Add;

            InitializeVisuals();
        }

        internal RoomInfo(Room _RoomToModify)
        {
            InitializeComponent();

            CurrentFormType = FormType.Modify;
            CurrentRoom = _RoomToModify;

            InitializeVisuals();
        }

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

        private void RoomInfo_Load(object sender, EventArgs e)
        {
            SizeForm();

            txtNameInput.Focus();
        }
        private void SizeForm()
        {
            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            int LabelTextSizeDiff = txtNameInput.Height - lblRoomName.Height;
            int MaxLabelSize = new int[] { lblRoomName.Width, lblRoomWidth.Width, lblRoomHeight.Width, lblRoomLocation.Width, lblRoomColor.Width }.Max();
            int TextBoxWidth = this.ClientSize.Width - 2 * Gap - MaxLabelSize;

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

            lblTitle.Left = this.ClientSize.Width / 2 - lblTitle.Width / 2;
            lblTitle.Top = SmallGap;

            lblRoomName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtNameInput.Width = TextBoxWidth;
            txtNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtNameInput.Top = lblTitle.Bottom + SmallGap;

            lblRoomWidth.Top = lblRoomName.Bottom + SmallGap + LabelTextSizeDiff;

            txtWidthInput.Width = TextBoxWidth;
            txtWidthInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtWidthInput.Top = txtNameInput.Bottom + SmallGap;

            lblRoomHeight.Top = lblRoomWidth.Bottom + SmallGap + LabelTextSizeDiff;

            txtHeightInput.Width = TextBoxWidth;
            txtHeightInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtHeightInput.Top = txtWidthInput.Bottom + SmallGap;

            lblRoomLocation.Top = lblRoomHeight.Bottom + SmallGap + LabelTextSizeDiff;

            lblX.Top = lblRoomLocation.Top;
            lblX.Left = lblRoomLocation.Right;

            txtXCoordInput.Width = (TextBoxWidth - lblX.Width - lblY.Width)/2;
            txtXCoordInput.Left = lblX.Right;
            txtXCoordInput.Top = txtHeightInput.Bottom + SmallGap;

            lblY.Top = lblRoomLocation.Top;
            lblY.Left = txtXCoordInput.Right;

            txtYCoordInput.Width = (TextBoxWidth - lblX.Width - lblY.Width) / 2;
            txtYCoordInput.Left = lblY.Right;
            txtYCoordInput.Top = txtHeightInput.Bottom + SmallGap;

            lblRoomColor.Top = lblRoomLocation.Bottom + SmallGap + LabelTextSizeDiff;

            txtColorInput.Width = TextBoxWidth;
            txtColorInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtColorInput.Top = txtYCoordInput.Bottom + SmallGap;

            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = txtColorInput.Bottom + Gap;

            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = txtColorInput.Bottom + Gap;

            this.ClientSize = new Size(this.ClientSize.Width, btnConfirm.Bottom + Gap);

        }

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }

        private void txtColorInput_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null; //Prevents A Cursor From Appearing In The Color Text Box
            if (cldRoomColor.ShowDialog() == DialogResult.OK)
            {
                txtColorInput.Text = cldRoomColor.Color.ToArgb().ToString();
            }

            txtColorInput.BackColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
            txtColorInput.ForeColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
        }
    }
}
