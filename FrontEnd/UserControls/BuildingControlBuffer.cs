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
    public partial class BuildingControlBuffer : UserControl
    {
        Building CurrentBuilding;

        private const int BuildingOffsetBuffer = 25;
        private Color BufferColor = Color.Black;
        private Color BufferTextColor = Color.DarkGray;

        public BuildingControlBuffer(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;

            InitializeVisuals();
            Wire();
        }

        private void InitializeVisuals()
        {
            this.Padding = new Padding(BuildingOffsetBuffer);
            this.BackColor = BufferColor;

            BuildingControl DisplayedBuilding = new BuildingControl(CurrentBuilding);

            DisplayedBuilding.Dock = DockStyle.None;
            DisplayedBuilding.Name = "DisplayedBuilding";
            DisplayedBuilding.Location = new Point(BuildingOffsetBuffer, BuildingOffsetBuffer);
            DisplayedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.Controls.Add(DisplayedBuilding);
        }

        private void Wire()
        {
            //CurrentBuilding.RoomListChanged += PopulateRooms;
            this.HandleDestroyed += UnWire;
        }
        private void UnWire(object? sender, EventArgs e)
        {
            //CurrentBuilding.RoomListChanged -= PopulateRooms;
            this.HandleDestroyed -= UnWire;
        }
    }
}
