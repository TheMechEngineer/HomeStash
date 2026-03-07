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
        internal event Action<FormType, Item?, ItemInfo, (string Name, string Description, double Value, int Quantity, IStorageHolder Location, StoredItemType CreationType)>? ConfirmClicked;
        internal event Action<ItemInfo>? CancelClicked;

        private Item? CurrentItem;
        private FormType CurrentFormType;

        private Building CurrentBuilding;

        internal ItemInfo(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            CurrentFormType = FormType.Add;

            rdoItem.Tag = StoredItemType.Item;
            rdoContainer.Tag = StoredItemType.Container;

            InitializeVisuals();
        }

        internal ItemInfo(Item _ItemToModify, Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            CurrentFormType = FormType.Modify;
            CurrentItem = _ItemToModify;

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            if (CurrentFormType == FormType.Add)
            {
                lblTitle.Text = "Add New Item";

                PopulateComboBox();

                if (cmbLocationInput.Items.Count > 0)
                {
                    cmbLocationInput.SelectedIndex = 0;
                }

            }
            else if (CurrentFormType == FormType.Modify)
            {
                //lblTitle.Text = "Modify Item";

                //txtNameInput.Text = CurrentItem.Name;
                //txtWidthInput.Text = CurrentItem.Width.ToString();
                //txtHeightInput.Text = CurrentItem.Height.ToString();
                //txtQuantityInput.Text = CurrentItem.CenterX.ToString();
                //txtYCoordInput.Text = CurrentItem.CenterY.ToString();

                //txtLocationInput.Text = CurrentItem.ItemColor.ToString();
                //txtLocationInput.BackColor = Color.FromArgb(CurrentItem.ItemColor);
                //txtLocationInput.ForeColor = Color.FromArgb(CurrentItem.ItemColor);
            }
        }

        private void PopulateComboBox()
        {
            ComboBoxLineItem CurrentComboBoxLineItem = new ComboBoxLineItem(CurrentBuilding);
            this.cmbLocationInput.Items.Add(CurrentComboBoxLineItem);

            List<ComboBoxLineItem> ValidStorageList = new List<ComboBoxLineItem>();
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
                ConfirmClicked?.Invoke(CurrentFormType, CurrentItem, this,
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

        private void AddNewItem_Load(object sender, EventArgs e)
        {
            txtNameInput.Focus();
        }
    }
}
