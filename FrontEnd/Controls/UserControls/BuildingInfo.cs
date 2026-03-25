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

        private void BuildingInfo_Load(object sender, EventArgs e)
        {
            SizeForm();

            txtNameInput.Focus();
        }

        private void SizeForm()
        {
            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            int LabelTextSizeDiff = txtNameInput.Height - lblBuildingName.Height;
            int MaxLabelSize = new int [] { lblBuildingName.Width, lblBuildingHeight.Width, lblBuildingWidth.Width }.Max();
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
            
            lblBuildingName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff/2;

            txtNameInput.Width = TextBoxWidth;
            txtNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtNameInput.Top = lblTitle.Bottom + SmallGap;

            lblBuildingWidth.Top = lblBuildingName.Bottom + SmallGap + LabelTextSizeDiff;

            txtWidthInput.Width = TextBoxWidth;
            txtWidthInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtWidthInput.Top = txtNameInput.Bottom + SmallGap;

            lblBuildingHeight.Top = lblBuildingWidth.Bottom + SmallGap + LabelTextSizeDiff;

            txtHeightInput.Width = TextBoxWidth;
            txtHeightInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtHeightInput.Top = txtWidthInput.Bottom + SmallGap;

            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = txtHeightInput.Bottom + Gap;

            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = txtHeightInput.Bottom + Gap;

            this.ClientSize = new Size(this.ClientSize.Width, btnConfirm.Bottom + Gap);

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
    }
}
