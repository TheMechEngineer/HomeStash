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
        BuildingControl DisplayedBuilding;

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

        private void temp()
        {
            List<Control> RemoveList = new List<Control>();

            foreach (Control item in this.Controls.OfType<Label>())
            {
                RemoveList.Add(item);
            }

            foreach (Control ritem in RemoveList)
            {
                this.Controls.Remove(ritem);
                ritem.Dispose();
            }
        }

        private void InitializeVisuals()
        {
            this.Padding = new Padding(BuildingOffsetBuffer);
            this.BackColor = BufferColor;

            this.DisplayedBuilding = new BuildingControl(CurrentBuilding);

            this.DisplayedBuilding.Dock = DockStyle.None;
            this.DisplayedBuilding.Name = "DisplayedBuilding";
            this.DisplayedBuilding.Location = new Point(BuildingOffsetBuffer, BuildingOffsetBuffer);
            this.DisplayedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.Controls.Add(this.DisplayedBuilding);
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
