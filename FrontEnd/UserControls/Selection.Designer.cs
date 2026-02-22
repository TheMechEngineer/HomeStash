namespace FrontEnd.UserControls
{
    internal partial class Selection
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
            flpSelectionList = new FlowLayoutPanel();
            btnAdd = new Button();
            btnDelete = new Button();
            btnSelect = new Button();
            lblSelectionTitle = new Label();
            SuspendLayout();
            // 
            // flpSelectionList
            // 
            flpSelectionList.AccessibleRole = AccessibleRole.OutlineButton;
            flpSelectionList.AutoScroll = true;
            flpSelectionList.BackColor = Color.LightSlateGray;
            flpSelectionList.BackgroundImageLayout = ImageLayout.None;
            flpSelectionList.BorderStyle = BorderStyle.FixedSingle;
            flpSelectionList.FlowDirection = FlowDirection.TopDown;
            flpSelectionList.Location = new Point(62, 47);
            flpSelectionList.Margin = new Padding(6);
            flpSelectionList.Name = "flpSelectionList";
            flpSelectionList.Size = new Size(350, 455);
            flpSelectionList.TabIndex = 0;
            flpSelectionList.WrapContents = false;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(25, 575);
            btnAdd.Margin = new Padding(6);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(220, 49);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Add Button";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += buttonAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(257, 575);
            btnDelete.Margin = new Padding(6);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(224, 49);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete Button";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += buttonDelete_Click;
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(144, 514);
            btnSelect.Margin = new Padding(6);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(214, 49);
            btnSelect.TabIndex = 1;
            btnSelect.Text = "Select Button";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += buttonSelect_Click;
            // 
            // lblSelectionTitle
            // 
            lblSelectionTitle.AutoSize = true;
            lblSelectionTitle.Location = new Point(176, 9);
            lblSelectionTitle.Name = "lblSelectionTitle";
            lblSelectionTitle.Size = new Size(165, 32);
            lblSelectionTitle.TabIndex = 3;
            lblSelectionTitle.Text = "Selection Title";
            // 
            // Selection
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            Controls.Add(lblSelectionTitle);
            Controls.Add(btnDelete);
            Controls.Add(btnSelect);
            Controls.Add(btnAdd);
            Controls.Add(flpSelectionList);
            Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6);
            Name = "Selection";
            Size = new Size(500, 635);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flpSelectionList;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnSelect;
        private Label lblSelectionTitle;
    }
}
