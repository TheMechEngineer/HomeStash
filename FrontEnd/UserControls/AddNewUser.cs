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
        public event Action<AddNewUser, User> AddConfirmed;
        public event Action<AddNewUser> AddCanceled;

        internal AddNewUser()
        {
            InitializeComponent();
        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            User NewUser = new User { UserName = txtUserNameInput.Text };

            AddConfirmed.Invoke(this, NewUser);
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            AddCanceled.Invoke(this);
        }

        private void AddNewUser_Load(object sender, EventArgs e)
        {
            txtUserNameInput.Focus();
        }
    }
}
