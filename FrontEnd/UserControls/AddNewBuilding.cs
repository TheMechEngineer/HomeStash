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
    internal partial class AddNewBuilding : UserControl
    {
        internal event Action<AddNewBuilding, (string Name, int Height, int Length)>? AddConfirmed;
        internal event Action<AddNewBuilding>? AddCanceled;

        private RootManager RootManagerInstance;

        internal AddNewBuilding()
        {
            InitializeComponent();
        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddConfirmed?.Invoke(this, 
                    (
                    txtNameInput.Text, 
                    Convert.ToInt32(txtHeightInput.Text), 
                    Convert.ToInt32(txtWidthInput.Text)
                    )
                );
            }
            catch (FormatException Exc)
            {
                MessageBox.Show("Height And Width Must Be Whole Numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            AddCanceled?.Invoke(this);
        }

        private void AddNewBuilding_Load(object sender, EventArgs e)
        {
            txtNameInput.Focus();
        }
    }
}
