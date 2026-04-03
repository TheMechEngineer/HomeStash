using BackEnd.ModelClasses;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Handles Rendering And Interaction For A Building Including Rooms And Grid
    /// </summary>
    internal partial class BuildingControl : UserControl
    {
        /// <summary>
        /// Event Triggered When The Building View Is Updated
        /// </summary>
        internal event Action? BuildingViewUpdated;

        /// <summary>
        /// Event Triggered When The Selected Room Changes
        /// </summary>
        internal event Action<Room?>? RoomSelectionChanged;

        /// <summary>
        /// The BackEnd Building Associated With This Control
        /// </summary>
        private Building CurrentBuilding;

        /// <summary>
        /// Default Pixel Density Per Unit Of Building Measurement
        /// </summary>
        private const int DefaultPixelsPerUnit = 10;

        /// <summary>
        /// Current Scaling Factor Applied To The Building Display
        /// </summary>
        private float ScalingFactor = 1.0f;

        /// <summary>
        /// Backing Field For Horizontal Grid Count
        /// </summary>
        private int _HGridCount = 10;

        /// <summary>
        /// Gets Or Sets The Horizontal Grid Count
        /// </summary>
        internal int HGridCount
        {
            get
            { return _HGridCount; }

            set
            {
                _HGridCount = value > 0 ? value : _HGridCount;
                // Forces Redraw Of Grid
                this.Invalidate();
                BuildingViewUpdated?.Invoke();
            }
        }

        /// <summary>
        /// Backing Field For Vertical Grid Count
        /// </summary>
        private int _VGridCount = 10;

        /// <summary>
        /// Gets Or Sets The Vertical Grid Count
        /// </summary>
        internal int VGridCount
        {
            get
            { return _VGridCount; }

            set
            {
                _VGridCount = value > 0 ? value : _VGridCount;
                // Forces Redraw Of Grid
                this.Invalidate();
                BuildingViewUpdated?.Invoke();
            }
        }

        /// <summary>
        /// Base Width Of The Building Display Before Scaling
        /// </summary>
        private int BaseDisplayWidth;

        /// <summary>
        /// Base Height Of The Building Display Before Scaling
        /// </summary>
        private int BaseDisplayHeight;

        /// <summary>
        /// Color Used To Highlight The Selected Room
        /// </summary>
        private Color SelectedRoomColor = Color.Beige;

        /// <summary>
        /// Currently Selected Room Control
        /// </summary>
        private RoomControl? SelectedRoom;

        /// <summary>
        /// Initializes The BuildingControl With The Provided Building
        /// </summary>
        /// <param name="_CurrentBuilding">The Building To Display</param>
        internal BuildingControl(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;

            SetBaseDimensions();

            this.Name = CurrentBuilding.Name;
            this.Tag = CurrentBuilding;

            InitializeVisuals();
            Wire();
        }

        /// <summary>
        /// Initializes Visual State Of The Building User Control
        /// </summary>
        private void InitializeVisuals()
        {
            ScaleBuilding(1);
            RegenerateRooms();
        }

        /// <summary>
        /// Wires Building Events To Control Handlers
        /// </summary>
        private void Wire()
        {
            CurrentBuilding.RoomListChanged += RegenerateRooms;
            CurrentBuilding.BuildingDimensionsChanged += CurrentBuilding_BuildingDimensionsChanged; ;
            this.HandleDestroyed += UnWire;
        }

        /// <summary>
        /// Unwires All Events When The Control Is Destroyed To Avoid Memory Leaks
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void UnWire(object? sender, EventArgs e)
        {
            CurrentBuilding.RoomListChanged -= RegenerateRooms;
            this.HandleDestroyed -= UnWire;
        }

        /// <summary>
        /// Scales The Building Display By The Provided Scale Modifier
        /// </summary>
        /// <param name="_ScaleModifier">The Scale Multiplier To Apply</param>
        internal void ScaleBuilding(float _ScaleModifier)
        {
            // Adjust The Running Scaling Factor Based On The Modifier
            this.ScalingFactor *= _ScaleModifier;

            // Always Modify Based On The Base Values Instead Of The Current Value To Prevent Distortion And Rounding Errors
            this.Width = Convert.ToInt32(Math.Round(this.BaseDisplayWidth * ScalingFactor, MidpointRounding.AwayFromZero));
            this.Height = Convert.ToInt32(Math.Round(this.BaseDisplayHeight * ScalingFactor, MidpointRounding.AwayFromZero));

            this.Invalidate(); //This Causes Draw Grid To Trigger, Because Invalidate Causes The OnPaint Event To Fire, Which Is Tied To The Paint Event Hander Below
            BuildingViewUpdated?.Invoke();

            RefreshRooms();
        }

        /// <summary>
        /// Draws The Grid Overlay On The Building Control
        /// </summary>
        /// <param name="_GraphicsTool">The Graphics Context Used For Drawing</param>
        private void DrawGrid(Graphics _GraphicsTool)
        {
            _GraphicsTool.Clear(this.BackColor);

            // Visual Settings For The Grid Lines
            Pen DrawingTool = new Pen(Color.DarkGray);
            DrawingTool.Width = 2.0f;
            DrawingTool.DashPattern = new float[] { 3.0F, 6.0F };

            // Distance Between Grid Lines
            float VerticalGap = Convert.ToSingle(this.Width) / _HGridCount;
            float HorizontalGap = Convert.ToSingle(this.Height) / _VGridCount;

            // Generate Vertical Grid Lines
            for (int i = 0; i <= _HGridCount; i++)
            {
                PointF VStartPoint = new PointF(VerticalGap * i, 0);
                PointF VEndPoint = new PointF(VerticalGap * i, this.Height);

                if (i == _HGridCount && DrawingTool.Width == 1.0f)
                {
                    VStartPoint.X -= 1.0f;
                    VEndPoint.X -= 1.0f;
                }

                _GraphicsTool.DrawLine(DrawingTool, VStartPoint, VEndPoint); //Vertical Grid Line
            }

            // Generate Horizontal Grid Lines
            for (int i = 0; i <= _VGridCount; i++)
            {

                PointF HStartPoint = new PointF(0, HorizontalGap * i);
                PointF HEndPoint = new PointF(this.Width, HorizontalGap * i);

                if (i == _VGridCount && DrawingTool.Width == 1.0f)
                {
                    HStartPoint.Y -= 1.0f;
                    HEndPoint.Y -= 1.0f;
                }

                _GraphicsTool.DrawLine(DrawingTool, HStartPoint, HEndPoint); //Horizontal Grid Line
            }
        }

        /// <summary>
        /// Regenerates All Room Controls Based On Current Building Data
        /// </summary>
        private void RegenerateRooms()
        {
            ClearExistingRooms();
            GenerateNewRooms();
            BuildingViewUpdated?.Invoke();
        }

        /// <summary>
        /// Refreshes All Room Controls Based On Current Scaling Factor
        /// </summary>
        private void RefreshRooms()
        {
            foreach (RoomControl CurrentRoomControl in this.Controls.OfType<RoomControl>())
            {
                CurrentRoomControl.SetRoomScale(ScalingFactor);
            }
        }

        /// <summary>
        /// Removes All Existing Room Controls From The Control Display
        /// </summary>
        private void ClearExistingRooms()
        {
            List<Control> RemoveList = new List<Control>();

            // Add Every Room To A List Of Rooms
            foreach (Control CurrentRoomControl in this.Controls.OfType<RoomControl>())
            {
                RemoveList.Add(CurrentRoomControl);
            }

            // They Must First Be Added To A Remove List And Then Removed
            // If Removed In The Above Loops, It Causes Skipping Of Rooms
            foreach (Control RoomToRemove in RemoveList)
            {
                (RoomToRemove as RoomControl).RoomClicked -= Room_Click;
                this.Controls.Remove(RoomToRemove);
                RoomToRemove.Dispose();
            }
        }

        /// <summary>
        /// Generates New Room Controls For Each BackEnd Room In The BackEnd Building
        /// </summary>
        private void GenerateNewRooms()
        {
            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                RoomControl DisplayedRoom = new RoomControl(CurrentRoom, DefaultPixelsPerUnit, ScalingFactor);

                DisplayedRoom.RoomClicked += Room_Click;

                this.Controls.Add(DisplayedRoom);
            }
        }

        /// <summary>
        /// Resets The Currently Selected Room And Clears Selection State
        /// </summary>
        internal void ResetSelectedRoom()
        {
            // If There Is A Current Selected Room Change Its Color Back To Its Room Color
            if (this.SelectedRoom != null)
            {
                this.SelectedRoom.BackColor = Color.FromArgb((this.SelectedRoom.Tag as Room).RoomColor);
            }

            this.SelectedRoom = null;
            RoomSelectionChanged?.Invoke(SelectedRoom?.Tag as Room);
        }

        /// <summary>
        /// Calculates Base Dimensions Of The Building Display Based On Units
        /// </summary>
        private void SetBaseDimensions()
        {
            //Need To Use Math.Round Because Convert.ToInt32 Uses Bankers Rounding And We Want Away From Zero Rounding
            this.BaseDisplayWidth = Convert.ToInt32(Math.Round(CurrentBuilding.Width * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
            this.BaseDisplayHeight = Convert.ToInt32(Math.Round(CurrentBuilding.Height * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Handles Click Event Directly On Building Control
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void BuildingControl_Click(object sender, EventArgs e)
        {
            ResetSelectedRoom();
        }

        /// <summary>
        /// Handles Room Click Event
        /// </summary>
        /// <param name="sender">The Room Control That Was Clicked</param>
        /// <param name="e">Event Arguments</param>
        private void Room_Click(object sender, EventArgs e)
        {
            if (sender is RoomControl ClickedRoom)
            {
                // If There Is A Current Selected Room Change Its Color Back To Its Room Color
                if (SelectedRoom != null)
                {
                    SelectedRoom.BackColor = Color.FromArgb((SelectedRoom.Tag as Room).RoomColor);
                }

                // Highlight The Newly Selected Room
                SelectedRoom = ClickedRoom;
                SelectedRoom.BackColor = SelectedRoomColor;
                RoomSelectionChanged?.Invoke(SelectedRoom.Tag as Room);
            }
        }

        /// <summary>
        /// Handles Building Dimensions Changed Event
        /// </summary>
        private void CurrentBuilding_BuildingDimensionsChanged()
        {
            SetBaseDimensions();
            ScaleBuilding(1);
        }

        /// <summary>
        /// Handles Paint Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildingControl_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
        }
    }
}