namespace FrontEnd.UserControls
{
    internal partial class RoomInfo
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
            lblTitle = new Label();
            lblX = new Label();
            lblY = new Label();
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
            btnConfirm.Click += btnConfirm_Click;
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
            btnCancel.Click += btnCancel_Click;
            // 
            // lblRoomName
            // 
            lblRoomName.AutoSize = true;
            lblRoomName.Location = new Point(22, 72);
            lblRoomName.Name = "lblRoomName";
            lblRoomName.Size = new Size(159, 32);
            lblRoomName.TabIndex = 3;
            lblRoomName.Text = "Room Name :";
            // 
            // txtNameInput
            // 
            txtNameInput.Location = new Point(206, 69);
            txtNameInput.Name = "txtNameInput";
            txtNameInput.Size = new Size(243, 39);
            txtNameInput.TabIndex = 4;
            // 
            // txtWidthInput
            // 
            txtWidthInput.Location = new Point(206, 139);
            txtWidthInput.Name = "txtWidthInput";
            txtWidthInput.Size = new Size(243, 39);
            txtWidthInput.TabIndex = 6;
            // 
            // lblRoomWidth
            // 
            lblRoomWidth.AutoSize = true;
            lblRoomWidth.Location = new Point(22, 142);
            lblRoomWidth.Name = "lblRoomWidth";
            lblRoomWidth.Size = new Size(159, 32);
            lblRoomWidth.TabIndex = 5;
            lblRoomWidth.Text = "Room Width :";
            // 
            // txtHeightInput
            // 
            txtHeightInput.Location = new Point(206, 213);
            txtHeightInput.Name = "txtHeightInput";
            txtHeightInput.Size = new Size(243, 39);
            txtHeightInput.TabIndex = 8;
            // 
            // lblRoomHeight
            // 
            lblRoomHeight.AutoSize = true;
            lblRoomHeight.Location = new Point(22, 216);
            lblRoomHeight.Name = "lblRoomHeight";
            lblRoomHeight.Size = new Size(167, 32);
            lblRoomHeight.TabIndex = 7;
            lblRoomHeight.Text = "Room Height :";
            // 
            // txtYCoordInput
            // 
            txtYCoordInput.Location = new Point(371, 289);
            txtYCoordInput.Name = "txtYCoordInput";
            txtYCoordInput.Size = new Size(78, 39);
            txtYCoordInput.TabIndex = 12;
            // 
            // lblRoomColor
            // 
            lblRoomColor.AutoSize = true;
            lblRoomColor.Location = new Point(22, 366);
            lblRoomColor.Name = "lblRoomColor";
            lblRoomColor.Size = new Size(152, 32);
            lblRoomColor.TabIndex = 11;
            lblRoomColor.Text = "Room Color :";
            // 
            // txtXCoordInput
            // 
            txtXCoordInput.Location = new Point(250, 289);
            txtXCoordInput.Name = "txtXCoordInput";
            txtXCoordInput.Size = new Size(70, 39);
            txtXCoordInput.TabIndex = 10;
            // 
            // lblRoomLocation
            // 
            lblRoomLocation.AutoSize = true;
            lblRoomLocation.Location = new Point(22, 292);
            lblRoomLocation.Name = "lblRoomLocation";
            lblRoomLocation.Size = new Size(166, 32);
            lblRoomLocation.TabIndex = 9;
            lblRoomLocation.Text = "Room Center :";
            // 
            // txtColorInput
            // 
            txtColorInput.Cursor = Cursors.Hand;
            txtColorInput.Location = new Point(206, 363);
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
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(169, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(122, 32);
            lblTitle.TabIndex = 14;
            lblTitle.Text = "Form Title";
            // 
            // lblX
            // 
            lblX.AutoSize = true;
            lblX.Location = new Point(204, 292);
            lblX.Name = "lblX";
            lblX.Size = new Size(40, 32);
            lblX.TabIndex = 15;
            lblX.Text = "X :";
            // 
            // lblY
            // 
            lblY.AutoSize = true;
            lblY.Location = new Point(326, 292);
            lblY.Name = "lblY";
            lblY.Size = new Size(39, 32);
            lblY.TabIndex = 16;
            lblY.Text = "Y :";
            // 
            // RoomInfo
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(lblY);
            Controls.Add(lblX);
            Controls.Add(lblTitle);
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
            Name = "RoomInfo";
            Size = new Size(500, 650);
            Load += RoomInfo_Load;
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
        private Label lblTitle;
        private Label lblX;
        private Label lblY;
    }
}
