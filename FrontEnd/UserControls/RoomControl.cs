using BackEnd.ModelClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd.UserControls
{
    internal partial class RoomControl : UserControl
    {
        private Room CurrentRoom;

        private int DefaultPixelsPerUnit;
        private float ScalingFactor;

        internal int InitialDisplayWidth;
        internal int InitialDisplayHeight;

        internal RoomControl(Room _CurrentRoom, int _DefaultPixelsPerUnit, float _ScalingFactor)
        {
            InitializeComponent();

            CurrentRoom = _CurrentRoom;

            DefaultPixelsPerUnit = _DefaultPixelsPerUnit;
            ScalingFactor = _ScalingFactor;

            InitialDisplayWidth = Convert.ToInt32(Math.Round(CurrentRoom.Width * DefaultPixelsPerUnit));
            InitialDisplayHeight = Convert.ToInt32(Math.Round(CurrentRoom.Height * DefaultPixelsPerUnit));

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            this.BackColor = Color.FromArgb(CurrentRoom.RoomColor);
            this.lblRoomInfo.Text = $"{CurrentRoom.Name}\n{CurrentRoom.TotalItemValue():C2}";

            ScaleRoom();
        }

        private void ScaleRoom()
        {
            this.Width = Convert.ToInt32(this.InitialDisplayWidth * ScalingFactor);
            this.Height = Convert.ToInt32(this.InitialDisplayHeight * ScalingFactor);

            this.lblRoomInfo.Location = new Point(this.Width / 2, this.Height / 2); //this isnt perfect but works for now
        }

    }
}
