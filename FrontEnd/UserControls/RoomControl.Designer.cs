namespace FrontEnd.UserControls
{
    internal partial class RoomControl
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
            lblRoomInfo = new Label();
            SuspendLayout();
            // 
            // lblRoomInfo
            // 
            lblRoomInfo.AutoSize = true;
            lblRoomInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRoomInfo.Location = new Point(41, 73);
            lblRoomInfo.Name = "lblRoomInfo";
            lblRoomInfo.Size = new Size(66, 15);
            lblRoomInfo.TabIndex = 0;
            lblRoomInfo.Text = "Room Info";
            lblRoomInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RoomControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Green;
            Controls.Add(lblRoomInfo);
            Margin = new Padding(0);
            Name = "RoomControl";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRoomInfo;
    }
}
