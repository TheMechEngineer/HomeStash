namespace FrontEnd.UserControls
{
    internal partial class AddNewBuilding
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
            lblBuildingName = new Label();
            txtNameInput = new TextBox();
            txtWidthInput = new TextBox();
            lblBuildingWidth = new Label();
            txtHeightInput = new TextBox();
            lblBuildingHeight = new Label();
            SuspendLayout();
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(40, 277);
            btnConfirm.Margin = new Padding(6);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(139, 49);
            btnConfirm.TabIndex = 1;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirmAdd_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(253, 277);
            btnCancel.Margin = new Padding(6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(174, 49);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancelAdd_Click;
            // 
            // lblBuildingName
            // 
            lblBuildingName.AutoSize = true;
            lblBuildingName.Location = new Point(22, 46);
            lblBuildingName.Name = "lblBuildingName";
            lblBuildingName.Size = new Size(178, 32);
            lblBuildingName.TabIndex = 3;
            lblBuildingName.Text = "Building Name:";
            // 
            // txtNameInput
            // 
            txtNameInput.Location = new Point(206, 43);
            txtNameInput.Name = "txtNameInput";
            txtNameInput.Size = new Size(243, 39);
            txtNameInput.TabIndex = 4;
            // 
            // txtWidthInput
            // 
            txtWidthInput.Location = new Point(206, 113);
            txtWidthInput.Name = "txtWidthInput";
            txtWidthInput.Size = new Size(243, 39);
            txtWidthInput.TabIndex = 6;
            // 
            // lblBuildingWidth
            // 
            lblBuildingWidth.AutoSize = true;
            lblBuildingWidth.Location = new Point(22, 116);
            lblBuildingWidth.Name = "lblBuildingWidth";
            lblBuildingWidth.Size = new Size(178, 32);
            lblBuildingWidth.TabIndex = 5;
            lblBuildingWidth.Text = "Building Width:";
            // 
            // txtHeightInput
            // 
            txtHeightInput.Location = new Point(206, 187);
            txtHeightInput.Name = "txtHeightInput";
            txtHeightInput.Size = new Size(243, 39);
            txtHeightInput.TabIndex = 8;
            // 
            // lblBuildingHeight
            // 
            lblBuildingHeight.AutoSize = true;
            lblBuildingHeight.Location = new Point(22, 190);
            lblBuildingHeight.Name = "lblBuildingHeight";
            lblBuildingHeight.Size = new Size(186, 32);
            lblBuildingHeight.TabIndex = 7;
            lblBuildingHeight.Text = "Building Height:";
            // 
            // AddNewBuilding
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(txtHeightInput);
            Controls.Add(lblBuildingHeight);
            Controls.Add(txtWidthInput);
            Controls.Add(lblBuildingWidth);
            Controls.Add(txtNameInput);
            Controls.Add(lblBuildingName);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6);
            Name = "AddNewBuilding";
            Size = new Size(475, 349);
            Load += AddNewBuilding_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblBuildingName;
        private TextBox txtNameInput;
        private TextBox txtWidthInput;
        private Label lblBuildingWidth;
        private TextBox txtHeightInput;
        private Label lblBuildingHeight;
    }
}
