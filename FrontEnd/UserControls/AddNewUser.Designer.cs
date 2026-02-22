namespace FrontEnd.UserControls
{
    internal partial class AddNewUser
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
            lblUserName = new Label();
            txtUserNameInput = new TextBox();
            SuspendLayout();
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(22, 113);
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
            btnCancel.Location = new Point(193, 113);
            btnCancel.Margin = new Padding(6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(174, 49);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancelAdd_Click;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(22, 46);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(137, 32);
            lblUserName.TabIndex = 3;
            lblUserName.Text = "User Name:";
            // 
            // txtUserNameInput
            // 
            txtUserNameInput.Location = new Point(175, 43);
            txtUserNameInput.Name = "txtUserNameInput";
            txtUserNameInput.Size = new Size(243, 39);
            txtUserNameInput.TabIndex = 4;
            // 
            // AddNewUser
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(txtUserNameInput);
            Controls.Add(lblUserName);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6);
            Name = "AddNewUser";
            Size = new Size(435, 175);
            Load += AddNewUser_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblUserName;
        private TextBox txtUserNameInput;
    }
}
