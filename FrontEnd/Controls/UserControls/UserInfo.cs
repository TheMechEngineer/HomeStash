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
    internal partial class UserInfo : UserControl
    {
        internal event Action<FormType, User?, UserInfo, string>? ConfirmClicked;
        internal event Action<UserInfo>? CancelClicked;

        private User? CurrentUser;
        private FormType CurrentFormType;

        internal UserInfo()
        {
            InitializeComponent();

            CurrentFormType = FormType.Add;
            InitializeVisuals();
        }

        internal UserInfo(User _UserToModify)
        {
            InitializeComponent();

            CurrentFormType = FormType.Modify;
            CurrentUser = _UserToModify;

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            if (CurrentFormType == FormType.Add)
            {
                lblTitle.Text = "Add New User";
            }
            else if (CurrentFormType == FormType.Modify)
            {
                lblTitle.Text = "Modify User";
                txtUserNameInput.Text = CurrentUser.Username;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ConfirmClicked?.Invoke(CurrentFormType, CurrentUser, this, txtUserNameInput.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }

        private void AddNewUser_Load(object sender, EventArgs e)
        {
            txtUserNameInput.Focus();
        }


    }
}
