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
        internal event Action<AddNewRoom, Room>? AddConfirmed;
        internal event Action<AddNewRoom>? AddCanceled;

        //private RootManager RootManagerInstance;
        //private TopDownBuildingView RootView;

        //internal AddNewRoom(ref RootManager _ProgramRoot, TopDownBuildingView _RootView)
        internal AddNewRoom()
        {
            InitializeComponent();

            //RootManagerInstance = _ProgramRoot;
            //RootView = _RootView;

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
                Room NewRoom = new Room
                {
                    Name = txtNameInput.Text,
                    Height = Convert.ToInt32(txtHeightInput.Text),
                    Width = Convert.ToInt32(txtWidthInput.Text),
                    CenterX = Convert.ToInt32(txtXCoordInput.Text),
                    CenterY = Convert.ToInt32(txtXCoordInput.Text),
                    RoomColor = Convert.ToInt32(txtColorInput.Text)
                };

                AddConfirmed?.Invoke(this, NewRoom);
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
