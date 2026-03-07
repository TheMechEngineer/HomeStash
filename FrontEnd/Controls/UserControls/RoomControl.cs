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
        internal event EventHandler? RoomClicked;

        private Room CurrentRoom;

        private int DefaultPixelsPerUnit;
        private float ScalingFactor;

        private int BaseDisplayWidth;
        private int BaseDisplayHeight;

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

        private void InitializeVisuals()
        {
            SetText();
            SetColor();
            SetDisplayedDimensions();
        }

        private void Wire()
        {
            CurrentRoom.StoredItemsChanged += CurrentRoom_StoredItemsChanged;
            CurrentRoom.StoredItemModified += CurrentRoom_StoredItemModified;

            CurrentRoom.RoomNameChanged += CurrentRoom_RoomNameChanged;
            CurrentRoom.RoomDimensionsChanged += CurrentRoom_RoomDimensionsChanged;
            CurrentRoom.RoomColorChanged += CurrentRoom_RoomColorChanged;

            this.Click += RoomControl_Click;

            foreach (Control CurrentControl in this.Controls)
            {
                CurrentControl.Click += RoomControl_Click;
            }

            this.HandleDestroyed += UnWire;
        }

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

        private void SetText()
        {
            this.lblRoomInfo.Text = $"{CurrentRoom.Name}\nItem Count: {CurrentRoom.TotalItemCount()}\nItem Value: {CurrentRoom.TotalItemValue():C2}";
        }

        private void SetColor()
        {
            this.BackColor = Color.FromArgb(CurrentRoom.RoomColor);
        }

        private void SetBaseDimensions()
        {
            BaseDisplayWidth = Convert.ToInt32(Math.Round(CurrentRoom.Width * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
            BaseDisplayHeight = Convert.ToInt32(Math.Round(CurrentRoom.Height * DefaultPixelsPerUnit, MidpointRounding.AwayFromZero));
        }

        private void SetDisplayedDimensions()
        {
            this.Width = Convert.ToInt32(Math.Round(this.BaseDisplayWidth * ScalingFactor, MidpointRounding.AwayFromZero));
            this.Height = Convert.ToInt32(Math.Round(this.BaseDisplayHeight * ScalingFactor, MidpointRounding.AwayFromZero));

            int DisplayedRoomLeft = Convert.ToInt32(Math.Round((((CurrentRoom.CenterX - CurrentRoom.Width / 2) * DefaultPixelsPerUnit) * ScalingFactor), MidpointRounding.AwayFromZero));
            int DisplayedRoomTop = Convert.ToInt32(Math.Round((((CurrentRoom.CenterY - CurrentRoom.Height / 2) * DefaultPixelsPerUnit) * ScalingFactor), MidpointRounding.AwayFromZero));

            this.Location = new Point(DisplayedRoomLeft, DisplayedRoomTop);
        }

        internal void SetRoomScale(float _NewScalingFactor)
        {
            ScalingFactor = _NewScalingFactor;
            SetDisplayedDimensions();
        }

        private void RoomControl_Click(object? sender, EventArgs e)
        {
            RoomClicked?.Invoke(this, e);
        }

        private void CurrentRoom_StoredItemsChanged()
        {
            SetText();
        }

        private void CurrentRoom_StoredItemModified()
        {
            SetText();
        }

        private void CurrentRoom_RoomNameChanged()
        {
            SetText();
        }

        private void CurrentRoom_RoomColorChanged()
        {
            SetColor();
        }

        private void CurrentRoom_RoomDimensionsChanged()
        {
            SetBaseDimensions();
            SetDisplayedDimensions();
        }
    }
}
