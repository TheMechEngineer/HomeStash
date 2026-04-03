using BackEnd.ModelClasses;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Acts As A Buffered Wrapper Around BuildingControl For UI And Information
    /// </summary>
    internal partial class BuildingControlBuffer : UserControl
    {
        /// <summary>
        /// Event Fowarding For When Room Selection Change
        /// </summary>
        internal event Action<Room?>? RoomSelectionChanged
        {
            add { DisplayedBuilding.RoomSelectionChanged += value; }
            remove { DisplayedBuilding.RoomSelectionChanged -= value; }
        }

        /// <summary>
        /// The BackEnd Building Associated With This Control
        /// </summary>
        private Building CurrentBuilding;

        /// <summary>
        /// The Building Control Being Displayed Within The Buffer
        /// </summary>
        private BuildingControl DisplayedBuilding;

        /// <summary>
        /// Background Color Used For The Buffer Area
        /// </summary>
        private Color BufferColor = Color.Black;

        /// <summary>
        ///  Color Used For Text Within The Buffer
        /// </summary>
        private Color BufferTextColor = Color.DarkGray;

        /// <summary>
        /// Size Of The Offset Buffer Applied Around The Building Control
        /// </summary>
        private const int _BuildingOffsetBuffer = 50;

        /// <summary>
        /// Gets The Offset Buffer Applied Around The Building Control
        /// </summary>
        internal int BuildingOffsetBuffer
        {
            get { return _BuildingOffsetBuffer; }
        }

        /// <summary>
        /// Gets Or Sets The Number Of Horizontal Grid Lines Displayed For The Buffer
        /// </summary>
        internal int HGridCount
        {
            get
            { return DisplayedBuilding.HGridCount; }

            set
            { DisplayedBuilding.HGridCount = value > 0 ? value : DisplayedBuilding.HGridCount; }
        }

        /// <summary>
        /// Gets Or Sets The Number Of Vertical Grid Lines Displayed For The Buffer
        /// </summary>
        internal int VGridCount
        {
            get
            { return DisplayedBuilding.VGridCount; }

            set
            { DisplayedBuilding.VGridCount = value > 0 ? value : DisplayedBuilding.VGridCount; }
        }

        /// <summary>
        /// Initializes The BuildingControlBuffer With The Provided Building
        /// </summary>
        /// <param name="_CurrentBuilding">The Building To Display</param>
        internal BuildingControlBuffer(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;

            InitializeVisuals();
            Wire();
        }

        /// <summary>
        /// Initializes Visual State Of The Buidling Buffer User Control
        /// </summary>
        private void InitializeVisuals()
        {
            this.BackColor = BufferColor;

            // Creates Displayed Building Control
            this.DisplayedBuilding = new BuildingControl(CurrentBuilding);
            this.DisplayedBuilding.Name = "DisplayedBuilding";

            // Sets Buffer Size Based On Building Dimensions And Offset
            this.Width = this.DisplayedBuilding.Width + 2 * BuildingOffsetBuffer;
            this.Height = this.DisplayedBuilding.Height + 2 * BuildingOffsetBuffer;

            // Positions Building Inside Buffer With Buffer Offset
            this.DisplayedBuilding.Dock = DockStyle.None;
            this.DisplayedBuilding.Location = new Point(BuildingOffsetBuffer, BuildingOffsetBuffer);
            this.DisplayedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.Controls.Add(this.DisplayedBuilding);

            RefreshSize();
        }

        /// <summary>
        /// Wires Building Events To Control Handlers
        /// </summary>
        private void Wire()
        {
            DisplayedBuilding.BuildingViewUpdated += RefreshSize;
            this.HandleDestroyed += UnWire;
        }

        /// <summary>
        /// Unwires All Events When The Control Is Destroyed To Avoid Memory Leaks
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void UnWire(object? sender, EventArgs e)
        {
            DisplayedBuilding.BuildingViewUpdated -= RefreshSize;
            this.HandleDestroyed -= UnWire;
        }

        /// <summary>
        /// Refreshes Buffer Size Based On Displayed Building And Updates Grid Labels
        /// </summary>
        private void RefreshSize()
        {
            this.Width = this.DisplayedBuilding.Width + 2 * BuildingOffsetBuffer;
            this.Height = this.DisplayedBuilding.Height + 2 * BuildingOffsetBuffer;

            RefreshGridLabels();
        }

        /// <summary>
        /// Refreshes All Grid Labels By Clearing And Regenerating Them
        /// </summary>
        private void RefreshGridLabels()
        {
            ClearExistingGridLabels();
            GenerateNewGridLabels();
        }

        /// <summary>
        /// Removes All Existing Grid Labels From The Buffer
        /// </summary>
        private void ClearExistingGridLabels()
        {
            List<Control> RemoveList = new List<Control>();

            // The Only Labels In The Buffer Controls Are The Grid Labels
            // Add All Labels To A List Of Labels
            foreach (Control CurrentLabel in this.Controls.OfType<Label>())
            {
                RemoveList.Add(CurrentLabel);
            }

            // They Must First Be Added To A Remove List And Then Removed
            // If Removed In The Above Loops, It Causes Skipping Of Labels
            foreach (Control LabelToRemove in RemoveList)
            {
                this.Controls.Remove(LabelToRemove);
                LabelToRemove.Dispose();
            }
        }

        /// <summary>
        /// Generates New Grid Labels For Both Horizontal And Vertical Axes
        /// </summary>
        private void GenerateNewGridLabels()
        {
            float HorizontalGap = Convert.ToSingle(DisplayedBuilding.Width) / HGridCount;

            // Generates Labels For Vertical Grid Lines
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= HGridCount; j++)
                {
                    Label VerticalGridLineLabel = new Label();

                    VerticalGridLineLabel.AutoSize = true;
                    VerticalGridLineLabel.Text = string.Format("{0:F2}", (CurrentBuilding.Width / HGridCount * j));
                    VerticalGridLineLabel.TextAlign = ContentAlignment.MiddleCenter;
                    VerticalGridLineLabel.ForeColor = BufferTextColor;

                    this.Controls.Add(VerticalGridLineLabel); // Need To Do This First Or Position Doesnt Work, Because AutoSize Doesnt Make Correct Size

                    // Set Left And Top Position To Center Align With The Grid Lines
                    int ControlLeftPosition = (int)(BuildingOffsetBuffer - (VerticalGridLineLabel.Width / 2) + (HorizontalGap * j));
                    int ControlTopPosition = ((DisplayedBuilding.Height + BuildingOffsetBuffer) * i) + (BuildingOffsetBuffer / 2) - (VerticalGridLineLabel.Height / 2);

                    VerticalGridLineLabel.Location = new Point(ControlLeftPosition, ControlTopPosition);
                }
            }

            float VerticalGap = Convert.ToSingle(DisplayedBuilding.Height) / VGridCount;

            // Generates Labels For Horizontal Grid Lines
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= VGridCount; j++)
                {
                    Label HorizontalGridLineLabel = new Label();

                    HorizontalGridLineLabel.AutoSize = true;
                    HorizontalGridLineLabel.Text = string.Format("{0:F2}", (CurrentBuilding.Height / VGridCount * j));
                    HorizontalGridLineLabel.TextAlign = ContentAlignment.MiddleCenter;
                    HorizontalGridLineLabel.ForeColor = BufferTextColor;

                    this.Controls.Add(HorizontalGridLineLabel); // Need To Do This First Or AutoSize Doesnt Make Correct Size

                    // Set Left And Top Position To Center Align With The Grid Lines
                    int ControlLeftPosition = ((DisplayedBuilding.Width + BuildingOffsetBuffer) * i) + (BuildingOffsetBuffer / 2) - (HorizontalGridLineLabel.Width / 2);
                    int ControlTopPosition = (int)(BuildingOffsetBuffer - (HorizontalGridLineLabel.Height / 2) + (VerticalGap * j));

                    HorizontalGridLineLabel.Location = new Point(ControlLeftPosition, ControlTopPosition);
                }
            }

        }

        /// <summary>
        /// Scales The Displayed Building By The Provided Scale Modifier
        /// </summary>
        /// <param name="_ScaleModifier"></param>
        internal void ScaleBuilding(float _ScaleModifier)
        {
            DisplayedBuilding.ScaleBuilding(_ScaleModifier);
        }

        /// <summary>
        /// Resets The Currently Selected Room In The Displayed Building
        /// </summary>
        internal void ResetSelectedRoom()
        {
            DisplayedBuilding.ResetSelectedRoom();
        }
    }
}