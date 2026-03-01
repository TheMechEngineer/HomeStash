using BackEnd.ModelClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd.UserControls
{
    internal partial class BuildingControl : UserControl
    {
        private Building CurrentBuilding;

        private const int DefaultPixelsPerUnit = 10;
        private float ScalingFactor = 1.0f;

        internal int InitialDisplayWidth;
        internal int InitialDisplayHeight;

        internal BuildingControl(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;

            InitialDisplayWidth = Convert.ToInt32(Math.Round(CurrentBuilding.Width * DefaultPixelsPerUnit));
            InitialDisplayHeight = Convert.ToInt32(Math.Round(CurrentBuilding.Height * DefaultPixelsPerUnit));

            InitializeVisuals();
            Wire();
        }

        private void InitializeVisuals()
        {
            ScaleBuilding(1);
        }

        private void Wire()
        {
            CurrentBuilding.RoomListChanged += PopulateRooms;
            this.HandleDestroyed += UnWire;
        }
        private void UnWire(object? sender, EventArgs e)
        {
            CurrentBuilding.RoomListChanged -= PopulateRooms;
            this.HandleDestroyed -= UnWire;
        }

        internal void ScaleBuilding(float ScaleModifier)
        {
            this.SuspendLayout();

            ScalingFactor *= ScaleModifier;

            this.Width = Convert.ToInt32(this.InitialDisplayWidth * ScalingFactor);
            this.Height = Convert.ToInt32(this.InitialDisplayHeight * ScalingFactor);

            PopulateRooms();

            this.ResumeLayout();
            this.Invalidate();
        }

        private void DrawGrid(Graphics _GraphicsTool)
        {
            _GraphicsTool.Clear(this.BackColor);

            Pen DrawingTool = new Pen(Color.DarkGray);
            DrawingTool.Width = 2.0f;
            DrawingTool.DashPattern = new float[] { 3.0F, 6.0F};

            int GridCount = 10;
            float VerticalGap = Convert.ToSingle(this.Width) / GridCount;
            float HorizontalGap = Convert.ToSingle(this.Height) / GridCount;

            for (int i = 0; i <= GridCount; i++)
            {

                PointF HStartPoint = new PointF(0, HorizontalGap * i);
                PointF HEndPoint = new PointF(this.Width, HorizontalGap * i);

                PointF VStartPoint = new PointF(VerticalGap * i, 0);
                PointF VEndPoint = new PointF(VerticalGap * i, this.Height);

                if (i == GridCount && DrawingTool.Width == 1.0f)
                {
                    HStartPoint.Y -= 1.0f;
                    HEndPoint.Y -= 1.0f;

                    VStartPoint.X -= 1.0f;
                    VEndPoint.X -= 1.0f;
                }
   

                _GraphicsTool.DrawLine(DrawingTool, HStartPoint, HEndPoint);
                _GraphicsTool.DrawLine(DrawingTool, VStartPoint, VEndPoint);
            }
        }

        private void PopulateRooms()
        {
            this.Controls.Clear();

            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                RoomControl DisplayedRoom = new RoomControl(CurrentRoom, DefaultPixelsPerUnit, ScalingFactor);

                DisplayedRoom.Name = "DisplayedRoom" + CurrentRoom.Name;

                int DisplayedRoomLeft = Convert.ToInt32((((CurrentRoom.CenterX - CurrentRoom.Width / 2) * DefaultPixelsPerUnit) * ScalingFactor));
                int DisplayedRoomTop = Convert.ToInt32((((CurrentRoom.CenterY - CurrentRoom.Height / 2) * DefaultPixelsPerUnit) * ScalingFactor));

                DisplayedRoom.Location = new Point(DisplayedRoomLeft, DisplayedRoomTop);

                this.Controls.Add(DisplayedRoom);
            }
        }

        private void BuildingControl_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
        }
    }
}
