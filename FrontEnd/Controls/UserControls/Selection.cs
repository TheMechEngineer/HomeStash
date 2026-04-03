using FrontEnd.Adapters;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Displays A Selectable List Of Objects And Provides CRUD Interaction Options
    /// </summary>
    internal partial class Selection : UserControl
    {
        /// <summary>
        /// Event Triggered When Select Action Is Invoked
        /// </summary>
        internal event Action<Selection, Type, object>? SelectClicked;

        /// <summary>
        /// Event Triggered When Add Action Is Invoked
        /// </summary>
        internal event Action<Selection, Type>? AddClicked;

        /// <summary>
        /// Event Triggered When Modify Action Is Invoked
        /// </summary>
        internal event Action<Selection, Type, object>? ModifyClicked;

        /// <summary>
        /// Event Triggered When Delete Action Is Invoked
        /// </summary>
        internal event Action<Type, object>? DeleteClicked;

        /// <summary>
        /// Adapter Used To Provide Consistent Data Regardless Of Source Object Type
        /// </summary>
        private AdapterSelection SelectionAdapter;

        /// <summary>
        /// Initial Client Width Of The FlowLayoutPanel Used For Layout Calculations
        /// </summary>
        private int InitialFLPClientWidth;

        /// <summary>
        /// Background Color For Unselected Labels
        /// </summary>
        private Color UnselectedLabelColor = Color.White;

        /// <summary>
        /// Background Color For Selected Labels
        /// </summary>
        private Color SelectedLabelColor = Color.Beige;

        /// <summary>
        /// Currently Selected Label In The List
        /// </summary>
        private Label? SelectedLabel;

        /// <summary>
        /// Initializes The Selection Control With The Provided Adapter Data
        /// </summary>
        /// <param name="_SelectionAdapter">The Adapter That Provides Selection Data</param>
        internal Selection(AdapterSelection _SelectionAdapter)
        {
            InitializeComponent();

            SelectionAdapter = _SelectionAdapter;
            InitialFLPClientWidth = flpSelectionList.ClientSize.Width;

            InitializeVisuals();
            Wire();
        }

        /// <summary>
        /// Initializes Visual State Of The Selection Control
        /// </summary>
        private void InitializeVisuals()
        {
            SetDisplayText();
            PopulateSelectionList();
        }

        /// <summary>
        /// Wires Adapter Events To Control Handlers
        /// </summary>
        private void Wire()
        {
            SelectionAdapter.SourceUpdated += PopulateSelectionList;
            this.HandleDestroyed += UnWire;
        }

        /// <summary>
        /// Unwires All Events When The Control Is Destroyed To Avoid Memory Leaks
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void UnWire(object? sender, EventArgs e)
        {
            SelectionAdapter.SourceUpdated -= PopulateSelectionList;
            this.HandleDestroyed -= UnWire;
        }

        /// <summary>
        /// Handles Selection Load Event For Visualization That Requires Load First To Be Accurate
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void Selection_Load(object sender, EventArgs e)
        {
            // Defers Execution Until UI Is Fully Loaded
            this.BeginInvoke(() =>
            {
                SizeForm();
            });
        }

        /// <summary>
        /// Sets Display Text For Labels And Buttons Based On Adapter Data
        /// </summary>
        private void SetDisplayText()
        {
            string ControlText = SelectionAdapter.ButtonText;

            lblSelectionTitle.Text = ControlText + " Selection";
            btnSelect.Text = "Select " + ControlText;
            btnModify.Text = "Modify " + ControlText;
            btnAdd.Text = "Add " + ControlText;
            btnDelete.Text = "Delete " + ControlText;

        }

        /// <summary>
        /// Sizes And Positions Controls Within The Form
        /// </summary>
        private void SizeForm()
        {
            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            // Center Title Label
            lblSelectionTitle.Left = this.ClientSize.Width / 2 - lblSelectionTitle.Width / 2;
            lblSelectionTitle.Top = SmallGap;

            // Center Selection List
            flpSelectionList.Left = Gap;
            flpSelectionList.Width = this.ClientSize.Width - 2 * Gap;
            flpSelectionList.Top = lblSelectionTitle.Bottom + SmallGap;
            flpSelectionList.Height = this.ClientSize.Height - flpSelectionList.Top - (2 * ButtonHeight) - (2 * Gap) - SmallGap;

            // Position Select Button
            btnSelect.Left = flpSelectionList.Left;
            btnSelect.Top = flpSelectionList.Bottom + Gap;

            // Position Add Button
            btnAdd.Left = flpSelectionList.Left;
            btnAdd.Top = btnSelect.Bottom + SmallGap;

            // Position Modify Button
            btnModify.Left = flpSelectionList.Right - btnModify.Width;
            btnModify.Top = flpSelectionList.Bottom + Gap;

            // Position Delete Button
            btnDelete.Left = flpSelectionList.Right - btnDelete.Width;
            btnDelete.Top = btnModify.Bottom + SmallGap;
        }

        /// <summary>
        /// Populates The Selection List With Items From The Adapter
        /// </summary>
        private void PopulateSelectionList()
        {
            string DisplayName;

            // Empty The Current List, So Repeat Adds Dont Create Duplicates
            flpSelectionList.Controls.Clear();

            // Convert The Adapter List Of Items To Select From Into Labels That Are Placed In The Selection List
            foreach (AdapterSelectionItem CurrentSelection in SelectionAdapter.GetAList())
            {
                DisplayName = CurrentSelection.DisplayText;

                Label NewSelectionItem = new Label();
                NewSelectionItem.Name = DisplayName;
                NewSelectionItem.Text = DisplayName;
                NewSelectionItem.Margin = new Padding(3);
                NewSelectionItem.Width = InitialFLPClientWidth - NewSelectionItem.Margin.Left - NewSelectionItem.Margin.Right;
                NewSelectionItem.TextAlign = ContentAlignment.MiddleCenter;
                NewSelectionItem.BackColor = Color.White;
                NewSelectionItem.Height = 40;
                NewSelectionItem.Click += Label_Click;
                NewSelectionItem.Tag = CurrentSelection.Value;
                flpSelectionList.Controls.Add(NewSelectionItem);

                // Adjust The Width Of The Labels To Still Show A Gap On The Left And Right Regardless Of Scroll Bar Visibility
                if (flpSelectionList.HorizontalScroll.Visible == true && flpSelectionList.VerticalScroll.Visible == true)
                {
                    flpSelectionList.ClientSize = new Size(InitialFLPClientWidth + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, flpSelectionList.ClientSize.Height + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight);
                }
                else if (flpSelectionList.VerticalScroll.Visible == false && flpSelectionList.Controls[0].Width < flpSelectionList.Width - flpSelectionList.Controls[0].Margin.Left - flpSelectionList.Controls[0].Margin.Right)
                {
                    flpSelectionList.ClientSize = new Size(InitialFLPClientWidth, flpSelectionList.ClientSize.Height);
                }
            }

        }

        /// <summary>
        /// Handles Select Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (SelectedLabel != null)
            {
                SelectClicked?.Invoke(this, SelectionAdapter.SelectionType, SelectedLabel.Tag);
            }
        }


        /// <summary>
        /// Handles Modify Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (SelectedLabel != null)
            {
                ModifyClicked?.Invoke(this, SelectionAdapter.SelectionType, SelectedLabel.Tag);
            }
        }

        /// <summary>
        /// Handles Add Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddClicked?.Invoke(this, SelectionAdapter.SelectionType);
        }

        /// <summary>
        /// Handles Delete Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedLabel != null)
            {
                // Prompt The User To Confirm They Want To Delete The Entry
                string MessagePrompt = $"Do You Want To Delete {SelectedLabel.Text}\nThis Is Permanent And Cannot Be Undone";

                // If the User Confirms They Want To Delete The Entry, Invoke The Delete Event
                if (MessageBox.Show(MessagePrompt, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteClicked?.Invoke(SelectionAdapter.SelectionType, SelectedLabel.Tag);
                }
                else
                {
                    SelectedLabel.BackColor = UnselectedLabelColor;
                }

                SelectedLabel = null;
            }
        }

        /// <summary>
        /// Handles Label Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_Click(object sender, EventArgs e)
        {

            // This Is Pattern Matching. Alternate Approach:
            // Label ClickedLabel = sender as Label
            if (sender is Label ClickedLabel)
            {
                // If There Is A Current Selected Item Change Its Color Back To The Unselected Color
                if (SelectedLabel != null)
                {
                    SelectedLabel.BackColor = UnselectedLabelColor;
                }

                // Highlight The Newly Selected Item
                SelectedLabel = ClickedLabel;
                SelectedLabel.BackColor = SelectedLabelColor;

                // Get index in the FlowLayoutPanel
                //int index = flpUserList.Controls.IndexOf(ClickedLabel);
                //MessageBox.Show($"Clicked label at index {index}: {ClickedLabel.Text}");
            }
        }
    }
}