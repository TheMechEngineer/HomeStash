using BackEnd.ModelClasses;

namespace FrontEnd.UserControls
{
    internal partial class BuildingControl : UserControl
    {
        internal event Action? BuildingViewUpdated;
        internal event Action<Room?>? RoomSelectionChanged;

        private Building CurrentBuilding;

        private const int DefaultPixelsPerUnit = 10;
        private float ScalingFactor = 1.0f;

        private int _HGridCount = 10;
        internal int HGridCount
        {
            get
            { return _HGridCount; }

            set
            {
                _HGridCount = value > 0 ? value : _HGridCount;
                this.Invalidate();
                BuildingViewUpdated?.Invoke();
            }
        }

        private int _VGridCount = 10;
        internal int VGridCount
        {
            get
            { return _VGridCount; }

            set
            {
                _VGridCount = value > 0 ? value : _VGridCount;
                this.Invalidate();
                BuildingViewUpdated?.Invoke();
            }
        }

        private int BaseDisplayWidth;
        private int BaseDisplayHeight;

        private Color SelectedRoomColor = Color.Beige;
        private RoomControl? SelectedRoom;

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

        private void InitializeVisuals()
        {
            ScaleBuilding(1);
            RegenerateRooms();
        }

        private void Wire()
        {
            CurrentBuilding.RoomListChanged += RegenerateRooms;
            CurrentBuilding.BuildingDimensionsChanged += CurrentBuilding_BuildingDimensionsChanged; ;
            this.HandleDestroyed += UnWire;
        }

        private void UnWire(object? sender, EventArgs e)
        {
            CurrentBuilding.RoomListChanged -= RegenerateRooms;
            this.HandleDestroyed -= UnWire;
        }

        internal void ScaleBuilding(float _ScaleModifier)
        {
            ScalingFactor *= _ScaleModifier;

            this.Width = Convert.ToInt32(Math.Round(this.BaseDisplayWidth * ScalingFactor, MidpointRounding.AwayFromZero));
            this.Height = Convert.ToInt32(Math.Round(this.BaseDisplayHeight * ScalingFactor, MidpointRounding.AwayFromZero));

            this.Invalidate(); //This Causes Draw Grid To Trigger, Because Invalidate Causes The OnPaint Event To Fire, Which Is Tied To The Paint Event Hander Below
            BuildingViewUpdated?.Invoke();

            RefreshRooms();
        }

        private void DrawGrid(Graphics _GraphicsTool)
        {
            _GraphicsTool.Clear(this.BackColor);

            Pen DrawingTool = new Pen(Color.DarkGray);
            DrawingTool.Width = 2.0f;
            DrawingTool.DashPattern = new float[] { 3.0F, 6.0F };

            float VerticalGap = Convert.ToSingle(this.Width) / _HGridCount;
            float HorizontalGap = Convert.ToSingle(this.Height) / _VGridCount;

            //Vertical Grid Lines
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

            //Horizontal Grid Lines
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

        private void RegenerateRooms()
        {
            ClearExistingRooms();
            GenerateNewRooms();
            BuildingViewUpdated?.Invoke();
        }

        private void RefreshRooms()
        {
            foreach (RoomControl CurrentRoomControl in this.Controls.OfType<RoomControl>())
            {
                CurrentRoomControl.SetRoomScale(ScalingFactor);
            }
        }

        private void ClearExistingRooms()
        {
            List<Control> RemoveList = new List<Control>();

            foreach (Control CurrentRoomControl in this.Controls.OfType<RoomControl>())
            {
                RemoveList.Add(CurrentRoomControl);
            }

            foreach (Control RoomToRemove in RemoveList)
            {
                (RoomToRemove as RoomControl).RoomClicked -= Room_Click;
                this.Controls.Remove(RoomToRemove);
                RoomToRemove.Dispose();
            }
        }

        private void GenerateNewRooms()
        {
            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                RoomControl DisplayedRoom = new RoomControl(CurrentRoom, DefaultPixelsPerUnit, ScalingFactor);

                DisplayedRoom.RoomClicked += Room_Click;

                this.Controls.Add(DisplayedRoom);
            }
        }

        internal void ResetSelectedRoom()
        {
            if (SelectedRoom != null)
            {
                SelectedRoom.BackColor = Color.FromArgb((SelectedRoom.Tag as Room).RoomColor);
            }

            SelectedRoom = null;
            RoomSelectionChanged?.Invoke(SelectedRoom?.Tag as Room);
        }

        private void SetBaseDimensions()
        {
            //Need To Use Math.Round Because Convert.ToInt32 uses Bankers Rounding and we want Away From Zero Rounding
            BaseDisplayWidth = Convert.ToInt32(Math.Round(CurrentBuilding.Width * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
            BaseDisplayHeight = Convert.ToInt32(Math.Round(CurrentBuilding.Height * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
        }

        private void BuildingControl_Click(object sender, EventArgs e)
        {
            ResetSelectedRoom();
        }

        private void Room_Click(object sender, EventArgs e)
        {
            // This is pattern matching. Alternate Approach:
            // Label ClickedLabel = sender as Label
            if (sender is RoomControl ClickedRoom)
            {
                if (SelectedRoom != null)
                {
                    SelectedRoom.BackColor = Color.FromArgb((SelectedRoom.Tag as Room).RoomColor);
                }

                SelectedRoom = ClickedRoom;
                SelectedRoom.BackColor = SelectedRoomColor;
                RoomSelectionChanged?.Invoke(SelectedRoom.Tag as Room);

                // Get index in the FlowLayoutPanel
                //int index = flpUserList.Controls.IndexOf(ClickedLabel);
                //MessageBox.Show($"Clicked label at index {index}: {ClickedLabel.Text}");
            }
        }

        private void CurrentBuilding_BuildingDimensionsChanged()
        {
            SetBaseDimensions();
            ScaleBuilding(1);
        }

        private void BuildingControl_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
        }

    }
}
