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
        private RootManager RootManagerInstance;

        internal AddNewUser(ref RootManager _ProgramRoot)
        {
            InitializeComponent();

            RootManagerInstance = _ProgramRoot;
        }

        private void CloseAndReturnControl()
        {
            this.Parent.Controls["UserSelection"].Enabled = true;
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            //troublshooting because this.Parent was coming back null
            //MessageBox.Show($"Parent is null: {this.Parent == null}\nParent type: {this.Parent?.GetType().Name}");

            User NewUser = new User { UserName = txtUserNameInput.Text };

            RootManagerInstance.AddUser(NewUser);

            CloseAndReturnControl();
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            CloseAndReturnControl();
        }

        private void AddNewUser_Load(object sender, EventArgs e)
        {
            txtUserNameInput.Focus();
        }
    }
}
