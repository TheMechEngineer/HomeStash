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

        private void UserInfo_Load(object sender, EventArgs e)
        {
            SizeForm();

            txtUserNameInput.Focus();
        }

        private void SizeForm()
        {
            
            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            int LabelTextSizeDiff = txtUserNameInput.Height - lblUserName.Height;
            int MaxLabelSize = new int[] { lblUserName.Width}.Max();
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

            lblUserName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtUserNameInput.Width = TextBoxWidth;
            txtUserNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtUserNameInput.Top = lblTitle.Bottom + SmallGap;

            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = txtUserNameInput.Bottom + Gap;

            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = txtUserNameInput.Bottom + Gap;

            this.ClientSize = new Size(this.ClientSize.Width, btnConfirm.Bottom + Gap);
            
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ConfirmClicked?.Invoke(CurrentFormType, CurrentUser, this, txtUserNameInput.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }
    }
}
