using BackEnd.DataContinuity;
using BackEnd.ModelClasses;
using FrontEnd.Forms;
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
    internal partial class AddNewRoom : UserControl
    {
        internal event Action<AddNewRoom, (string Name, int Height, int Width, int CenterX, int CenterY, int ColorValue)>? AddConfirmed;
        internal event Action<AddNewRoom>? AddCanceled;

        internal AddNewRoom()
        {
            InitializeComponent();

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            txtColorInput.Text = Color.Green.ToArgb().ToString();
            txtColorInput.BackColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
            txtColorInput.ForeColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            try
            {

                AddConfirmed?.Invoke(this, 
                    (
                        txtNameInput.Text, 
                        Convert.ToInt32(txtHeightInput.Text), 
                        Convert.ToInt32(txtWidthInput.Text), 
                        Convert.ToInt32(txtXCoordInput.Text), 
                        Convert.ToInt32(txtYCoordInput.Text), 
                        Convert.ToInt32(txtColorInput.Text)
                    )
                );
            }
            catch (FormatException Exc)
            {
                MessageBox.Show("Height, Width, And Coordinates Must Be Whole Numbers", "Invalid Input");
            }
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            AddCanceled?.Invoke(this);
        }

        private void AddNewRoom_Load(object sender, EventArgs e)
        {
            txtNameInput.Focus();
        }

        private void txtColorInput_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;
            if (cldRoomColor.ShowDialog() == DialogResult.OK)
            {
                txtColorInput.Text = cldRoomColor.Color.ToArgb().ToString();
            }
            else
            {
                txtColorInput.Text = Color.Green.ToArgb().ToString();
            }

            txtColorInput.BackColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
            txtColorInput.ForeColor = Color.FromArgb(Convert.ToInt32(txtColorInput.Text));
        }
    }
}
