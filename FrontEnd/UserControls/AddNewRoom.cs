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
    public partial class AddNewRoom : UserControl
    {
        private RootManager RootManagerInstance;
        private TopDownBuildingView RootView;

        public AddNewRoom(ref RootManager _ProgramRoot, TopDownBuildingView _RootView)
        {
            InitializeComponent();

            RootManagerInstance = _ProgramRoot;
            RootView = _RootView;
        }

        private void CloseAndReturnControl()
        {
            RootView.CloseAddNewRoom();
        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            //troublshooting because this.Parent was coming back null
            //MessageBox.Show($"Parent is null: {this.Parent == null}\nParent type: {this.Parent?.GetType().Name}");

            try
            {
                Building NewBuilding = new Building
                {
                    Name = txtNameInput.Text,
                    Height = Convert.ToInt32(txtHeightInput.Text),
                    Width = Convert.ToInt32(txtWidthInput.Text)
                };

                RootManagerInstance.ActiveUser.AddBuilding(NewBuilding);

                CloseAndReturnControl();
            }
            catch (FormatException Exc)
            {
                MessageBox.Show("Height, Width, And Location Must Be Whole Numbers", "Invalid Input");
            }

        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            CloseAndReturnControl();
        }

        private void AddNewRoom_Load(object sender, EventArgs e)
        {
            txtNameInput.Focus();
        }
    }
}
