namespace FrontEnd.UserControls
{
    internal partial class ItemInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConfirm = new Button();
            btnCancel = new Button();
            lblItemName = new Label();
            txtNameInput = new TextBox();
            txtDescriptionInput = new TextBox();
            lblItemDescription = new Label();
            txtValueInput = new TextBox();
            lblItemValue = new Label();
            lblItemLocation = new Label();
            txtQuantityInput = new TextBox();
            lblItemQuantity = new Label();
            lblTitle = new Label();
            grpItemType = new GroupBox();
            rdoContainer = new RadioButton();
            rdoItem = new RadioButton();
            cmbLocationInput = new ComboBox();
            grpItemType.SuspendLayout();
            SuspendLayout();
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(49, 563);
            btnConfirm.Margin = new Padding(6);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(139, 49);
            btnConfirm.TabIndex = 1;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(253, 563);
            btnCancel.Margin = new Padding(6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(174, 49);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblItemName
            // 
            lblItemName.AutoSize = true;
            lblItemName.Location = new Point(22, 72);
            lblItemName.Name = "lblItemName";
            lblItemName.Size = new Size(145, 32);
            lblItemName.TabIndex = 3;
            lblItemName.Text = "Item Name :";
            // 
            // txtNameInput
            // 
            txtNameInput.Location = new Point(231, 69);
            txtNameInput.Name = "txtNameInput";
            txtNameInput.Size = new Size(243, 39);
            txtNameInput.TabIndex = 4;
            // 
            // txtDescriptionInput
            // 
            txtDescriptionInput.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtDescriptionInput.Location = new Point(231, 139);
            txtDescriptionInput.Multiline = true;
            txtDescriptionInput.Name = "txtDescriptionInput";
            txtDescriptionInput.ScrollBars = ScrollBars.Vertical;
            txtDescriptionInput.Size = new Size(243, 168);
            txtDescriptionInput.TabIndex = 6;
            // 
            // lblItemDescription
            // 
            lblItemDescription.AutoSize = true;
            lblItemDescription.Location = new Point(22, 139);
            lblItemDescription.Name = "lblItemDescription";
            lblItemDescription.Size = new Size(202, 32);
            lblItemDescription.TabIndex = 5;
            lblItemDescription.Text = "Item Description :";
            // 
            // txtValueInput
            // 
            txtValueInput.Location = new Point(231, 334);
            txtValueInput.Name = "txtValueInput";
            txtValueInput.Size = new Size(243, 39);
            txtValueInput.TabIndex = 8;
            // 
            // lblItemValue
            // 
            lblItemValue.AutoSize = true;
            lblItemValue.Location = new Point(22, 337);
            lblItemValue.Name = "lblItemValue";
            lblItemValue.Size = new Size(139, 32);
            lblItemValue.TabIndex = 7;
            lblItemValue.Text = "Item Value :";
            // 
            // lblItemLocation
            // 
            lblItemLocation.AutoSize = true;
            lblItemLocation.Location = new Point(22, 487);
            lblItemLocation.Name = "lblItemLocation";
            lblItemLocation.Size = new Size(171, 32);
            lblItemLocation.TabIndex = 11;
            lblItemLocation.Text = "Item Location :";
            // 
            // txtQuantityInput
            // 
            txtQuantityInput.Location = new Point(231, 410);
            txtQuantityInput.Name = "txtQuantityInput";
            txtQuantityInput.Size = new Size(243, 39);
            txtQuantityInput.TabIndex = 10;
            // 
            // lblItemQuantity
            // 
            lblItemQuantity.AutoSize = true;
            lblItemQuantity.Location = new Point(22, 413);
            lblItemQuantity.Name = "lblItemQuantity";
            lblItemQuantity.Size = new Size(173, 32);
            lblItemQuantity.TabIndex = 9;
            lblItemQuantity.Text = "Item Quantity :";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(169, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(122, 32);
            lblTitle.TabIndex = 14;
            lblTitle.Text = "Form Title";
            // 
            // grpItemType
            // 
            grpItemType.Controls.Add(rdoContainer);
            grpItemType.Controls.Add(rdoItem);
            grpItemType.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            grpItemType.Location = new Point(22, 177);
            grpItemType.Name = "grpItemType";
            grpItemType.Size = new Size(166, 130);
            grpItemType.TabIndex = 15;
            grpItemType.TabStop = false;
            grpItemType.Text = "Item Type";
            // 
            // rdoContainer
            // 
            rdoContainer.AutoSize = true;
            rdoContainer.BackColor = Color.LightSteelBlue;
            rdoContainer.Location = new Point(14, 80);
            rdoContainer.Name = "rdoContainer";
            rdoContainer.Size = new Size(113, 29);
            rdoContainer.TabIndex = 1;
            rdoContainer.Text = "Container";
            rdoContainer.UseVisualStyleBackColor = false;
            // 
            // rdoItem
            // 
            rdoItem.AutoSize = true;
            rdoItem.BackColor = Color.LightSteelBlue;
            rdoItem.Checked = true;
            rdoItem.Location = new Point(14, 38);
            rdoItem.Name = "rdoItem";
            rdoItem.Size = new Size(67, 29);
            rdoItem.TabIndex = 0;
            rdoItem.TabStop = true;
            rdoItem.Text = "Item";
            rdoItem.UseVisualStyleBackColor = false;
            // 
            // cmbLocationInput
            // 
            cmbLocationInput.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLocationInput.FormattingEnabled = true;
            cmbLocationInput.IntegralHeight = false;
            cmbLocationInput.Location = new Point(231, 484);
            cmbLocationInput.MaxDropDownItems = 10;
            cmbLocationInput.Name = "cmbLocationInput";
            cmbLocationInput.Size = new Size(243, 40);
            cmbLocationInput.TabIndex = 16;
            // 
            // ItemInfo
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(cmbLocationInput);
            Controls.Add(grpItemType);
            Controls.Add(lblTitle);
            Controls.Add(lblItemLocation);
            Controls.Add(txtQuantityInput);
            Controls.Add(lblItemQuantity);
            Controls.Add(txtValueInput);
            Controls.Add(lblItemValue);
            Controls.Add(txtDescriptionInput);
            Controls.Add(lblItemDescription);
            Controls.Add(txtNameInput);
            Controls.Add(lblItemName);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6);
            MinimumSize = new Size(475, 0);
            Name = "ItemInfo";
            Size = new Size(500, 650);
            Load += ItemInfo_Load;
            grpItemType.ResumeLayout(false);
            grpItemType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblItemName;
        private TextBox txtNameInput;
        private Label lblItemDescription;
        private TextBox txtDescriptionInput;
        private Label lblItemValue;
        private TextBox txtValueInput;
        private Label lblItemQuantity;
        private TextBox txtQuantityInput;
        private Label lblItemLocation;
        private Label lblTitle;
        private GroupBox grpItemType;
        private RadioButton rdoContainer;
        private RadioButton rdoItem;
        private ComboBox cmbLocationInput;
    }
}
