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
    internal partial class BuildingInfo : UserControl
    {
        internal event Action<FormType, Building?, BuildingInfo, (string Name, float Width, float Height)>? ConfirmClicked;
        internal event Action<BuildingInfo>? CancelClicked;

        private Building? CurrentBuilding;
        private FormType CurrentFormType;

        internal BuildingInfo()
        {
            InitializeComponent();

            CurrentFormType = FormType.Add;

            InitializeVisuals();
        }

        internal BuildingInfo(Building _BuildingToModify)
        {
            InitializeComponent();

            CurrentFormType = FormType.Modify;
            CurrentBuilding = _BuildingToModify;

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            if (CurrentFormType == FormType.Add)
            {
                lblTitle.Text = "Add New Building";
            }
            else if (CurrentFormType == FormType.Modify)
            {
                lblTitle.Text = "Modify Building";
                txtNameInput.Text = CurrentBuilding.Name;
                txtWidthInput.Text = CurrentBuilding.Width.ToString();
                txtHeightInput.Text = CurrentBuilding.Height.ToString();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmClicked?.Invoke(CurrentFormType, CurrentBuilding, this,
                    (
                    txtNameInput.Text,
                    Convert.ToSingle(txtWidthInput.Text),
                    Convert.ToSingle(txtHeightInput.Text)
                    )
                );
            }
            catch (FormatException Exc)
            {
                MessageBox.Show("Format Error: Width And Height Must Be Numbers", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }

        private void AddNewBuilding_Load(object sender, EventArgs e)
        {
            txtNameInput.Focus();
        }
    }
}
