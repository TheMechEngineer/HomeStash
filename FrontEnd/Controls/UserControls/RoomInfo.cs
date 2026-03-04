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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmClicked?.Invoke( CurrentFormType, CurrentRoom, this,
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
                MessageBox.Show("Width, Height, And Coordinates Must Be Numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }

        private void AddNewRoom_Load(object sender, EventArgs e)
        {
            txtNameInput.Focus();
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
