using BackEnd.ModelClasses;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Represents A Single Room Within A Building And Handles Its Display And Interaction
    /// </summary>
    internal partial class RoomControl : UserControl
    {
        /// <summary>
        /// Event Triggered When The Room Control Is Clicked
        /// </summary>
        internal event EventHandler? RoomClicked;

        /// <summary>
        /// The BackEnd Room Associated With This Control
        /// </summary>
        private Room CurrentRoom;

        /// <summary>
        /// Default Pixel Density Per Unit Of Room Measurement
        /// </summary>
        private int DefaultPixelsPerUnit;

        /// <summary>
        /// Current Scaling Factor Applied To The Room Display
        /// </summary>
        private float ScalingFactor;

        /// <summary>
        /// Base Width Of The Room Display Before Scaling
        /// </summary>
        private int BaseDisplayWidth;

        /// <summary>
        /// Base Height Of The Room Display Before Scaling
        /// </summary>
        private int BaseDisplayHeight;

        /// <summary>
        /// Initializes The RoomControl With The Provided Room And Display Settings
        /// </summary>
        /// <param name="_CurrentRoom">The Room To Display</param>
        /// <param name="_DefaultPixelsPerUnit">Default Pixels Per Unit For Scaling</param>
        /// <param name="_ScalingFactor">Initial Scaling Factor</param>
        internal RoomControl(Room _CurrentRoom, int _DefaultPixelsPerUnit, float _ScalingFactor)
        {
            InitializeComponent();

            CurrentRoom = _CurrentRoom;

            DefaultPixelsPerUnit = _DefaultPixelsPerUnit;
            ScalingFactor = _ScalingFactor;

            SetBaseDimensions();

            this.Name = CurrentRoom.Name;
            this.Tag = CurrentRoom;

            InitializeVisuals();
            Wire();
        }

        /// <summary>
        /// Initializes Visual State Of The Room User Control
        /// </summary>
        private void InitializeVisuals()
        {
            SetText();
            SetColor();
            SetDisplayedDimensions();
        }

        /// <summary>
        /// Wires Room Events To Control Handlers
        /// </summary>
        private void Wire()
        {
            CurrentRoom.StoredItemsChanged += CurrentRoom_StoredItemsChanged;
            CurrentRoom.StoredItemModified += CurrentRoom_StoredItemModified;

            CurrentRoom.RoomNameChanged += CurrentRoom_RoomNameChanged;
            CurrentRoom.RoomDimensionsChanged += CurrentRoom_RoomDimensionsChanged;
            CurrentRoom.RoomColorChanged += CurrentRoom_RoomColorChanged;

            this.Click += RoomControl_Click;

            // This Makes Is So The RoomControl Click Event Occurs Even If The User Clicks On A Label, etc.
            foreach (Control CurrentControl in this.Controls)
            {
                CurrentControl.Click += RoomControl_Click;
            }

            this.HandleDestroyed += UnWire;
        }

        /// <summary>
        /// Unwires All Events When The Control Is Destroyed To Avoid Memory Leaks
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void UnWire(object? sender, EventArgs e)
        {
            CurrentRoom.StoredItemsChanged -= CurrentRoom_StoredItemsChanged;
            CurrentRoom.RoomNameChanged -= CurrentRoom_RoomNameChanged;
            CurrentRoom.RoomDimensionsChanged -= CurrentRoom_RoomDimensionsChanged;
            CurrentRoom.RoomColorChanged -= CurrentRoom_RoomColorChanged;

            this.Click -= RoomControl_Click;

            foreach (Control CurrentControl in this.Controls)
            {
                CurrentControl.Click -= RoomControl_Click;
            }

            this.HandleDestroyed -= UnWire;
        }

        /// <summary>
        /// Updates The Displayed Text Information For The Room Control
        /// </summary>
        private void SetText()
        {
            this.lblRoomInfo.Text = $"{CurrentRoom.Name}\nItem Count: {CurrentRoom.TotalItemCount()}\nItem Value: {CurrentRoom.TotalItemValue():C2}";
        }

        /// <summary>
        /// Updates The Background Color Of The Room Control
        /// </summary>
        private void SetColor()
        {
            this.BackColor = Color.FromArgb(CurrentRoom.RoomColor);
        }

        /// <summary>
        /// Calculates Base Dimensions Of The Room Control Based On Units
        /// </summary>
        private void SetBaseDimensions()
        {
            BaseDisplayWidth = Convert.ToInt32(Math.Round(CurrentRoom.Width * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
            BaseDisplayHeight = Convert.ToInt32(Math.Round(CurrentRoom.Height * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Sets The Dimensions And Position Of The Room Control Based On Scaling
        /// </summary>
        private void SetDisplayedDimensions()
        {
            // Always Modify Based On The Base Values Instead Of The Current Value To Prevent Distortion And Rounding Errors
            this.Width = Convert.ToInt32(Math.Round(this.BaseDisplayWidth * ScalingFactor, MidpointRounding.AwayFromZero));
            this.Height = Convert.ToInt32(Math.Round(this.BaseDisplayHeight * ScalingFactor, MidpointRounding.AwayFromZero));

            int DisplayedRoomLeft = Convert.ToInt32(Math.Round((((CurrentRoom.CenterX - CurrentRoom.Width / 2) * DefaultPixelsPerUnit) * ScalingFactor), MidpointRounding.AwayFromZero));
            int DisplayedRoomTop = Convert.ToInt32(Math.Round((((CurrentRoom.CenterY - CurrentRoom.Height / 2) * DefaultPixelsPerUnit) * ScalingFactor), MidpointRounding.AwayFromZero));

            this.Location = new Point(DisplayedRoomLeft, DisplayedRoomTop);
        }

        /// <summary>
        /// Updates The Scaling Factor For The Room And Recalculates Display Dimensions
        /// </summary>
        /// <param name="_NewScalingFactor">The New Scaling Factor</param>
        internal void SetRoomScale(float _NewScalingFactor)
        {
            ScalingFactor = _NewScalingFactor;
            SetDisplayedDimensions();
        }

        /// <summary>
        /// Handles Click Event For The Room Control
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void RoomControl_Click(object? sender, EventArgs e)
        {
            RoomClicked?.Invoke(this, e);
        }

        /// <summary>
        /// Handles Stored Items Changed Event
        /// </summary>
        private void CurrentRoom_StoredItemsChanged()
        {
            SetText();
        }

        /// <summary>
        /// Handles Stored Item Modified Event
        /// </summary>
        private void CurrentRoom_StoredItemModified()
        {
            SetText();
        }

        /// <summary>
        /// Handles Room Name Changed Event
        /// </summary>
        private void CurrentRoom_RoomNameChanged()
        {
            SetText();
        }

        /// <summary>
        /// Handles Room Color Changed Event
        /// </summary>
        private void CurrentRoom_RoomColorChanged()
        {
            SetColor();
        }

        /// <summary>
        /// Handles Room Dimensions Changed Event
        /// </summary>
        private void CurrentRoom_RoomDimensionsChanged()
        {
            SetBaseDimensions();
            SetDisplayedDimensions();
        }
    }
}