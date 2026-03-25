using BackEnd.DataContinuity;
using BackEnd.Enumerations;
using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Container = BackEnd.ModelClasses.Container;

namespace FrontEnd.UserControls
{
    internal partial class ItemInfo : UserControl
    {
        internal event Action<FormType, bool, Item?, ItemInfo, (string Name, string Description, double Value, int Quantity, IStorageHolder Location, StoredItemType CreationType)>? ConfirmClicked;
        internal event Action<ItemInfo>? CancelClicked;

        private Building CurrentBuilding;
        private Item? CurrentItem;
        private FormType CurrentFormType;
        private bool ModifyOrMove;

        internal ItemInfo(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            CurrentFormType = FormType.Add;

            InitializeVisuals();
        }

        internal ItemInfo(bool _ModifyOrMove, Item _ItemToModify, Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            CurrentItem = _ItemToModify;
            CurrentFormType = FormType.Modify;
            ModifyOrMove = _ModifyOrMove;

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            rdoItem.Tag = StoredItemType.Item;
            rdoContainer.Tag = StoredItemType.Container;

            PopulateComboBox();

            if (CurrentFormType == FormType.Add)
            {
                lblTitle.Text = "Add New Item";

                if (cmbLocationInput.Items.Count > 0)
                {
                    cmbLocationInput.SelectedIndex = 0;
                }
            }
            else if (CurrentFormType == FormType.Modify)
            {
                txtNameInput.Text = CurrentItem.Name;
                txtDescriptionInput.Text = CurrentItem.Description;
                txtValueInput.Text = CurrentItem.Value.ToString();
                txtQuantityInput.Text = CurrentItem.Quantity.ToString();
                cmbLocationInput.SelectedItem = SetComboBoxSelection();

                SetRadioButtonSelection();
                grpItemType.Enabled = false;

                if (ModifyOrMove)
                {
                    lblTitle.Text = "Modify Item";
                    cmbLocationInput.Enabled = false;
                }
                else
                {
                    lblTitle.Text = "Move Item";
                    txtNameInput.Enabled = false;
                    txtDescriptionInput.Enabled = false;
                    txtValueInput.Enabled = false;
                    txtQuantityInput.Enabled = false;
                }
            }
        }

        private void ItemInfo_Load(object sender, EventArgs e)
        {
            SizeForm();

            txtNameInput.Focus();
        }

        private void SizeForm()
        {
            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            int LabelTextSizeDiff = txtNameInput.Height - lblItemName.Height;
            int MaxLabelSize = new int[] { lblItemName.Width, lblItemDescription.Width, lblItemValue.Width, lblItemQuantity.Width, lblItemLocation.Width }.Max();
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

            lblItemName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtNameInput.Width = TextBoxWidth;
            txtNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtNameInput.Top = lblTitle.Bottom + SmallGap;

            lblItemDescription.Top = lblItemName.Bottom + SmallGap + LabelTextSizeDiff;

            txtDescriptionInput.Width = TextBoxWidth;
            txtDescriptionInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtDescriptionInput.Top = txtNameInput.Bottom + SmallGap;

            grpItemType.Top = lblItemDescription.Bottom + LabelTextSizeDiff;
            grpItemType.Left = lblItemDescription.Left;
            grpItemType.Height = txtDescriptionInput.Bottom - grpItemType.Top;
            grpItemType.Width = txtDescriptionInput.Left - SmallGap - grpItemType.Left;

            lblItemValue.Top = grpItemType.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtValueInput.Width = TextBoxWidth;
            txtValueInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtValueInput.Top = txtDescriptionInput.Bottom + SmallGap;

            lblItemQuantity.Top = lblItemValue.Bottom + SmallGap + LabelTextSizeDiff;

            txtQuantityInput.Width = TextBoxWidth;
            txtQuantityInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtQuantityInput.Top = txtValueInput.Bottom + SmallGap;

            lblItemLocation.Top = lblItemQuantity.Bottom + SmallGap + LabelTextSizeDiff;

            cmbLocationInput.Width = TextBoxWidth;
            cmbLocationInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            cmbLocationInput.Top = txtQuantityInput.Bottom + SmallGap;

            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = cmbLocationInput.Bottom + Gap;

            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = cmbLocationInput.Bottom + Gap;

            this.ClientSize = new Size(this.ClientSize.Width, btnConfirm.Bottom + Gap);

        }

        private void PopulateComboBox()
        {
            ComboBoxLineItem CurrentComboBoxLineItem = new ComboBoxLineItem(CurrentBuilding);
            this.cmbLocationInput.Items.Add(CurrentComboBoxLineItem);

            List<ComboBoxLineItem> ValidStorageList = new List<ComboBoxLineItem>();

            foreach (Container CurrentContainer in CurrentBuilding.StoredItems.OfType<Container>())
            {
                ValidStorageList.AddRange(GetNestedContainerItems(CurrentContainer));
            }

            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                ValidStorageList.Add(new ComboBoxLineItem(CurrentRoom));

                foreach (Container CurrentContainer in CurrentRoom.StoredItems.OfType<Container>())
                {
                    ValidStorageList.AddRange(GetNestedContainerItems(CurrentContainer));
                }
            }

            //Option 1 This Explicitly Casts The Objects In The List To An Object, Before Converting To An Array
            this.cmbLocationInput.Items.AddRange(ValidStorageList.Cast<object>().ToArray());

            //Option 2 Relies On The Objects Implicitly Being An Object Already
            //this.cmbLocationInput.Items.AddRange(ValidStorageList.ToArray());

            //Option 3 A Traditional For Loop
            //foreach (ComboBoxLineItem CurrentValue in ValidStorageList) { this.cmbLocationInput.Items.Add(CurrentValue); }
        }

        private List<ComboBoxLineItem> GetNestedContainerItems(Container _CurrentContainer)
        {
            List<ComboBoxLineItem> ValidContainerList = new List<ComboBoxLineItem>();
            ValidContainerList.Add(new ComboBoxLineItem(_CurrentContainer));

            foreach (Container CurrentContainer in _CurrentContainer.StoredItems.OfType<Container>())
            {
                ValidContainerList.AddRange(GetNestedContainerItems(CurrentContainer));
            }

            return ValidContainerList;
        }

        private ComboBoxLineItem SetComboBoxSelection()
        {
            foreach (ComboBoxLineItem CurrentLineItem in cmbLocationInput.Items)
            {
                if (CurrentLineItem.Tag == CurrentItem.ImmediateParent)
                {
                    return CurrentLineItem;
                }
            }

            return null;
        }

        private void SetRadioButtonSelection()
        {
            if (CurrentItem.GetType() == typeof(Item))
            {
                rdoItem.Checked = true;
            }
            else
            {
                rdoContainer.Checked = true;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            StoredItemType StoredItemType = StoredItemType.Item;

            foreach (RadioButton CurrentRadioButton in grpItemType.Controls)
            {
                if (CurrentRadioButton.Checked)
                {
                    StoredItemType = (StoredItemType)CurrentRadioButton.Tag;
                    break;
                }
            }

            try
            {
                //Need To Add A Return For the Modify Or Move Type
                ConfirmClicked?.Invoke(CurrentFormType, ModifyOrMove, CurrentItem, this,
                    (
                        txtNameInput.Text,
                        txtDescriptionInput.Text,
                        Convert.ToDouble(txtValueInput.Text),
                        Convert.ToInt32(txtQuantityInput.Text),
                        (cmbLocationInput.SelectedItem as ComboBoxLineItem).Tag,
                        StoredItemType
                    )
                );
            }
            catch (FormatException Exc)
            {
                MessageBox.Show("Format Error: Value Must Be A Number, Quantity Must Be A Whole Number", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }
    }
}
