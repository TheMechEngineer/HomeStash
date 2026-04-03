using BackEnd.Enumerations;
using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;
using FrontEnd.Utilities;
using System.Data;
using Container = BackEnd.ModelClasses.Container;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Handles Display And Interaction For Adding, Modifying, Or Moving An Item
    /// </summary>
    internal partial class ItemInfo : UserControl
    {
        /// <summary>
        /// Event Triggered When The Confirm Button Is Clicked
        /// </summary>
        internal event Action<FormType, bool, Item?, ItemInfo, (string Name, string Description, double Value, int Quantity, IStorageHolder Location, StoredItemType CreationType)>? ConfirmClicked;

        /// <summary>
        /// Event Triggered When The Cancel Button Is Clicked
        /// </summary>
        internal event Action<ItemInfo>? CancelClicked;

        /// <summary>
        /// The Building Associated With This Item
        /// </summary>
        private Building CurrentBuilding;

        /// <summary>
        /// The Item Being Modified, Null If Adding A New Item
        /// </summary>
        private Item? CurrentItem;

        /// <summary>
        /// Indicates Whether The Form Is In Add Or Modify Mode
        /// </summary>
        private FormType CurrentFormType;

        /// <summary>
        /// Indicates Whether The Form Is Modifying Or Moving The Item
        /// </summary>
        private bool ModifyOrMove;

        /// <summary>
        /// Initializes The ItemInfo Control In Add Mode
        /// </summary>
        /// <param name="_CurrentBuilding"></param>
        internal ItemInfo(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            CurrentFormType = FormType.Add;

            InitializeVisuals();
        }

        /// <summary>
        /// Initializes The ItemInfo Control In Modify/Move Mode With The Provided Item
        /// </summary>
        /// <param name="_ModifyOrMove">True If Modifying Item Properties, False If Moving The Item</param>
        /// <param name="_ItemToModify">The Item Object To Modify Or Move</param>
        /// <param name="_CurrentBuilding">The Building Associated With The Item</param>
        internal ItemInfo(bool _ModifyOrMove, Item _ItemToModify, Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            CurrentItem = _ItemToModify;
            CurrentFormType = FormType.Modify;
            ModifyOrMove = _ModifyOrMove;

            InitializeVisuals();
        }

        /// <summary>
        /// Initializes Visual Elements Of The ItemInfo Control Based On Form Mode And Modification Mode
        /// </summary>
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

        /// <summary>
        /// Handles ItemInfo Load Event For Visualization That Requires Load First To Be Accurate
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void ItemInfo_Load(object sender, EventArgs e)
        {
            SizeForm();

            txtNameInput.Focus();
        }

        /// <summary>
        /// Sizes And Positions Controls Within The Form
        /// </summary>
        private void SizeForm()
        {
            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            int LabelTextSizeDiff = txtNameInput.Height - lblItemName.Height;
            int MaxLabelSize = new int[] { lblItemName.Width, lblItemDescription.Width, lblItemValue.Width, lblItemQuantity.Width, lblItemLocation.Width }.Max();
            int TextBoxWidth = this.ClientSize.Width - 2 * Gap - MaxLabelSize;

            // Right Align All Labels That Are Not The Title Label
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

            // Center Title Label
            lblTitle.Left = this.ClientSize.Width / 2 - lblTitle.Width / 2;
            lblTitle.Top = SmallGap;

            // Position Item Name Label and TextBox
            lblItemName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtNameInput.Width = TextBoxWidth;
            txtNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtNameInput.Top = lblTitle.Bottom + SmallGap;

            // Position Item Description Label and TextBox
            lblItemDescription.Top = lblItemName.Bottom + SmallGap + LabelTextSizeDiff;

            txtDescriptionInput.Width = TextBoxWidth;
            txtDescriptionInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtDescriptionInput.Top = txtNameInput.Bottom + SmallGap;

            // Position Item Type Group Box
            grpItemType.Top = lblItemDescription.Bottom + LabelTextSizeDiff;
            grpItemType.Left = lblItemDescription.Left;
            grpItemType.Height = txtDescriptionInput.Bottom - grpItemType.Top;
            grpItemType.Width = txtDescriptionInput.Left - SmallGap - grpItemType.Left;

            // Position Item Value Label and TextBox
            lblItemValue.Top = grpItemType.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtValueInput.Width = TextBoxWidth;
            txtValueInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtValueInput.Top = txtDescriptionInput.Bottom + SmallGap;

            // Position Item Quantity Label and TextBox
            lblItemQuantity.Top = lblItemValue.Bottom + SmallGap + LabelTextSizeDiff;

            txtQuantityInput.Width = TextBoxWidth;
            txtQuantityInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtQuantityInput.Top = txtValueInput.Bottom + SmallGap;

            // Position Item Location Label and Combobox
            lblItemLocation.Top = lblItemQuantity.Bottom + SmallGap + LabelTextSizeDiff;

            cmbLocationInput.Width = TextBoxWidth;
            cmbLocationInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            cmbLocationInput.Top = txtQuantityInput.Bottom + SmallGap;

            // Position Confirm Button
            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = cmbLocationInput.Bottom + Gap;

            // Position Cancel Button
            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = cmbLocationInput.Bottom + Gap;

            // Adjust User Control Height Based On Positioned Controls
            this.ClientSize = new Size(this.ClientSize.Width, btnConfirm.Bottom + Gap);
        }

        /// <summary>
        /// Populates The Location ComboBox With All Valid Storage Locations In The Building
        /// </summary>
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

        /// <summary>
        /// Recursively Retrieves All Nested Containers Within A Container
        /// </summary>
        /// <param name="_CurrentContainer">The Top Level Container To Recursively Search</param>
        /// <returns>A List Of ComboBoxLineItem Representing Nested Containers</returns>
        private List<ComboBoxLineItem> GetNestedContainerItems(Container _CurrentContainer)
        {
            // Add The Current Container To The List
            List<ComboBoxLineItem> ValidContainerList = new List<ComboBoxLineItem>();
            ValidContainerList.Add(new ComboBoxLineItem(_CurrentContainer));

            // Add Any Direct And Indirect Child Containers Within This Container To The List
            foreach (Container CurrentContainer in _CurrentContainer.StoredItems.OfType<Container>())
            {
                ValidContainerList.AddRange(GetNestedContainerItems(CurrentContainer));
            }

            return ValidContainerList;
        }

        /// <summary>
        /// Returns The ComboBoxLineItem That Matches The Current Item's Immediate Parent
        /// </summary>
        /// <returns>The Matching ComboBoxLineItem</returns>
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

        /// <summary>
        /// Sets The RadioButton Selection Based On The Current Item Type
        /// </summary>
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

        /// <summary>
        /// Handles Confirm Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            StoredItemType StoredItemType = StoredItemType.Item;

            // Return The Type Of The Stored Item Based On The Selected Radio Button
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

        /// <summary>
        /// Handles Cancel Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }
    }
}