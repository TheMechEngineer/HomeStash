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
            lblRoomName = new Label();
            txtNameInput = new TextBox();
            txtWidthInput = new TextBox();
            lblRoomWidth = new Label();
            txtHeightInput = new TextBox();
            lblRoomHeight = new Label();
            txtYCoordInput = new TextBox();
            lblRoomColor = new Label();
            txtXCoordInput = new TextBox();
            lblRoomLocation = new Label();
            txtColorInput = new TextBox();
            cldRoomColor = new ColorDialog();
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
            // lblRoomName
            // 
            lblRoomName.AutoSize = true;
            lblRoomName.Location = new Point(22, 46);
            lblRoomName.Name = "lblRoomName";
            lblRoomName.Size = new Size(152, 32);
            lblRoomName.TabIndex = 3;
            lblRoomName.Text = "Room Name:";
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
            // lblRoomWidth
            // 
            lblRoomWidth.AutoSize = true;
            lblRoomWidth.Location = new Point(22, 116);
            lblRoomWidth.Name = "lblRoomWidth";
            lblRoomWidth.Size = new Size(152, 32);
            lblRoomWidth.TabIndex = 5;
            lblRoomWidth.Text = "Room Width:";
            // 
            // txtHeightInput
            // 
            txtHeightInput.Location = new Point(206, 187);
            txtHeightInput.Name = "txtHeightInput";
            txtHeightInput.Size = new Size(243, 39);
            txtHeightInput.TabIndex = 8;
            // 
            // lblRoomHeight
            // 
            lblRoomHeight.AutoSize = true;
            lblRoomHeight.Location = new Point(22, 190);
            lblRoomHeight.Name = "lblRoomHeight";
            lblRoomHeight.Size = new Size(160, 32);
            lblRoomHeight.TabIndex = 7;
            lblRoomHeight.Text = "Room Height:";
            // 
            // txtYCoordInput
            // 
            txtYCoordInput.Location = new Point(335, 263);
            txtYCoordInput.Name = "txtYCoordInput";
            txtYCoordInput.Size = new Size(114, 39);
            txtYCoordInput.TabIndex = 12;
            // 
            // lblRoomColor
            // 
            lblRoomColor.AutoSize = true;
            lblRoomColor.Location = new Point(22, 340);
            lblRoomColor.Name = "lblRoomColor";
            lblRoomColor.Size = new Size(145, 32);
            lblRoomColor.TabIndex = 11;
            lblRoomColor.Text = "Room Color:";
            // 
            // txtXCoordInput
            // 
            txtXCoordInput.Location = new Point(206, 263);
            txtXCoordInput.Name = "txtXCoordInput";
            txtXCoordInput.Size = new Size(100, 39);
            txtXCoordInput.TabIndex = 10;
            // 
            // lblRoomLocation
            // 
            lblRoomLocation.AutoSize = true;
            lblRoomLocation.Location = new Point(22, 266);
            lblRoomLocation.Name = "lblRoomLocation";
            lblRoomLocation.Size = new Size(159, 32);
            lblRoomLocation.TabIndex = 9;
            lblRoomLocation.Text = "Room Center:";
            // 
            // txtColorInput
            // 
            txtColorInput.Location = new Point(206, 337);
            txtColorInput.Name = "txtColorInput";
            txtColorInput.Size = new Size(243, 39);
            txtColorInput.TabIndex = 13;
            txtColorInput.TabStop = false;
            txtColorInput.MouseDown += txtColorInput_MouseDown;
            // 
            // cldRoomColor
            // 
            cldRoomColor.AnyColor = true;
            cldRoomColor.FullOpen = true;
            // 
            // AddNewRoom
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(txtColorInput);
            Controls.Add(lblRoomColor);
            Controls.Add(txtYCoordInput);
            Controls.Add(txtXCoordInput);
            Controls.Add(lblRoomLocation);
            Controls.Add(txtHeightInput);
            Controls.Add(lblRoomHeight);
            Controls.Add(txtWidthInput);
            Controls.Add(lblRoomWidth);
            Controls.Add(txtNameInput);
            Controls.Add(lblRoomName);
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
        private Label lblRoomName;
        private TextBox txtNameInput;
        private TextBox txtWidthInput;
        private Label lblRoomWidth;
        private TextBox txtHeightInput;
        private Label lblRoomHeight;
        private TextBox txtYCoordInput;
        private Label lblRoomColor;
        private TextBox txtXCoordInput;
        private Label lblRoomLocation;
        private TextBox txtColorInput;
        private ColorDialog cldRoomColor;
    }
}
