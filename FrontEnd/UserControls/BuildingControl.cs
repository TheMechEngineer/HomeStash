using BackEnd.ModelClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            ScalingFactor *= ScaleModifier;

            this.Width = Convert.ToInt32(this.InitialDisplayWidth * ScalingFactor);
            this.Height = Convert.ToInt32(this.InitialDisplayHeight * ScalingFactor);

            PopulateRooms();
        }

        private void PopulateRooms()
        { 
            this.Controls.Clear();

            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                RoomControl DisplayedRoom = new RoomControl(CurrentRoom, DefaultPixelsPerUnit, ScalingFactor);

                DisplayedRoom.Name = "DisplayedRoom" + CurrentRoom.Name;

                int DisplayedRoomLeft = Convert.ToInt32((((CurrentRoom.CenterX - CurrentRoom.Width/2) * DefaultPixelsPerUnit) * ScalingFactor));
                int DisplayedRoomTop = Convert.ToInt32((((CurrentRoom.CenterY - CurrentRoom.Height / 2) * DefaultPixelsPerUnit) * ScalingFactor));
                
                DisplayedRoom.Location = new Point(DisplayedRoomLeft, DisplayedRoomTop);

                this.Controls.Add(DisplayedRoom);
            }
        }

        //
        //
        // Since this control is responsible for showing the rooms this class should subsribe to the building event that fires when roomlist changes
        // The event handler to attach to that event would be the method to draw the rooms in the building
        //
        //
    }
}
