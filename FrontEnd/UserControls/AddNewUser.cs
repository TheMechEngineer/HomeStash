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
    internal partial class AddNewUser : UserControl
    {
        internal event Action<AddNewUser, string>? AddConfirmed;
        internal event Action<AddNewUser>? AddCanceled;

        internal AddNewUser()
        {
            InitializeComponent();
        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            AddConfirmed?.Invoke(this, txtUserNameInput.Text);
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            AddCanceled?.Invoke(this);
        }

        private void AddNewUser_Load(object sender, EventArgs e)
        {
            txtUserNameInput.Focus();
        }
    }
}
