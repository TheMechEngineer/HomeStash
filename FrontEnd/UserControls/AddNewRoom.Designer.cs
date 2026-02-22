namespace FrontEnd.UserControls
{
    internal partial class AddNewRoom
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
            textBox1 = new TextBox();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            textBox3 = new TextBox();
            SuspendLayout();
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(38, 445);
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
            btnCancel.Location = new Point(251, 445);
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
            lblBuildingName.Size = new Size(152, 32);
            lblBuildingName.TabIndex = 3;
            lblBuildingName.Text = "Room Name:";
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
            lblBuildingWidth.Size = new Size(152, 32);
            lblBuildingWidth.TabIndex = 5;
            lblBuildingWidth.Text = "Room Width:";
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
            lblBuildingHeight.Size = new Size(160, 32);
            lblBuildingHeight.TabIndex = 7;
            lblBuildingHeight.Text = "Room Height:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(335, 263);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(114, 39);
            textBox1.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 340);
            label1.Name = "label1";
            label1.Size = new Size(145, 32);
            label1.TabIndex = 11;
            label1.Text = "Room Color:";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(206, 263);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 39);
            textBox2.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 266);
            label2.Name = "label2";
            label2.Size = new Size(159, 32);
            label2.TabIndex = 9;
            label2.Text = "Room Center:";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(206, 337);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(243, 39);
            textBox3.TabIndex = 13;
            // 
            // AddNewRoom
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(textBox3);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(label2);
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
            MinimumSize = new Size(475, 0);
            Name = "AddNewRoom";
            Size = new Size(475, 530);
            Load += AddNewRoom_Load;
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
        private TextBox textBox1;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
        private TextBox textBox3;
    }
}
