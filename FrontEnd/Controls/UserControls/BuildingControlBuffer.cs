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
    internal partial class BuildingControlBuffer : UserControl
    {
        //This Is Called Event Fowarding
        internal event Action<Room?>? RoomSelectionChanged
        {
            add { DisplayedBuilding.RoomSelectionChanged += value; }
            remove { DisplayedBuilding.RoomSelectionChanged -= value; }
        }

        internal event Action? StoredItemsChanged
        {
            add { DisplayedBuilding.StoredItemsChanged += value; }
            remove { DisplayedBuilding.StoredItemsChanged -= value; }
        }

        internal event Action? RoomListChanged
        {
            add { DisplayedBuilding.RoomListChanged += value; }
            remove { DisplayedBuilding.RoomListChanged -= value; }
        }

        private Building CurrentBuilding;
        private BuildingControl DisplayedBuilding;

        private Color BufferColor = Color.Black;
        private Color BufferTextColor = Color.DarkGray;

        private const int _BuildingOffsetBuffer = 50;
        internal int BuildingOffsetBuffer 
        {
            get {  return _BuildingOffsetBuffer; }
        }

        internal int HGridCount
        {
            get
            { return DisplayedBuilding.HGridCount; }

            set
            { DisplayedBuilding.HGridCount = value > 0 ? value : DisplayedBuilding.HGridCount; }
        }

        internal int VGridCount
        {
            get
            { return DisplayedBuilding.VGridCount; }

            set
            { DisplayedBuilding.VGridCount = value > 0 ? value : DisplayedBuilding.VGridCount; }
        }

        internal BuildingControlBuffer(Building _CurrentBuilding)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;

            InitializeVisuals();
            Wire();
        }

        private void InitializeVisuals()
        {
            this.BackColor = BufferColor;

            this.DisplayedBuilding = new BuildingControl(CurrentBuilding);
            this.DisplayedBuilding.Name = "DisplayedBuilding";

            this.Width = this.DisplayedBuilding.Width + 2 * BuildingOffsetBuffer;
            this.Height = this.DisplayedBuilding.Height + 2 * BuildingOffsetBuffer;

            this.DisplayedBuilding.Dock = DockStyle.None;
            this.DisplayedBuilding.Location = new Point(BuildingOffsetBuffer, BuildingOffsetBuffer);
            this.DisplayedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.Controls.Add(this.DisplayedBuilding);

            RefreshGridLabels();
        }

        private void Wire()
        {
            DisplayedBuilding.BuildingViewUpdated += RefreshGridLabels;
            this.HandleDestroyed += UnWire;
        }
        private void UnWire(object? sender, EventArgs e)
        {
            DisplayedBuilding.BuildingViewUpdated -= RefreshGridLabels;
            this.HandleDestroyed -= UnWire;
        }

        private void RefreshGridLabels()
        {
            this.Width = this.DisplayedBuilding.Width + 2 * BuildingOffsetBuffer;
            this.Height = this.DisplayedBuilding.Height + 2 * BuildingOffsetBuffer;

            ClearExistingGridLabels();
            GenerateNewGridLabels();
        }

        private void ClearExistingGridLabels()
        {
            List<Control> RemoveList = new List<Control>();

            foreach (Control CurrentLabel in this.Controls.OfType<Label>())
            {
                RemoveList.Add(CurrentLabel);
            }

            foreach (Control LabelToRemove in RemoveList)
            {
                this.Controls.Remove(LabelToRemove);
                LabelToRemove.Dispose();
            }
        }

        private void GenerateNewGridLabels()
        {
            this.SuspendLayout();

            float HorizontalGap = Convert.ToSingle(DisplayedBuilding.Width) / HGridCount;

            //Labels For Vertical Grid Lines
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

                    int ControlLeftPosition = (int)(BuildingOffsetBuffer - (VerticalGridLineLabel.Width / 2) + (HorizontalGap * j));
                    int ControlTopPosition = ((DisplayedBuilding.Height + BuildingOffsetBuffer) * i) + (BuildingOffsetBuffer / 2) - (VerticalGridLineLabel.Height / 2);

                    VerticalGridLineLabel.Location = new Point(ControlLeftPosition, ControlTopPosition);
                }
            }

            float VerticalGap = Convert.ToSingle(DisplayedBuilding.Height) / VGridCount;

            //Labels For Horizontal Grid Lines
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

                    int ControlLeftPosition = ((DisplayedBuilding.Width + BuildingOffsetBuffer) * i) + (BuildingOffsetBuffer / 2) - (HorizontalGridLineLabel.Width / 2);
                    int ControlTopPosition = (int)(BuildingOffsetBuffer - (HorizontalGridLineLabel.Height / 2) + (VerticalGap * j));

                    HorizontalGridLineLabel.Location = new Point(ControlLeftPosition, ControlTopPosition);
                }
            }

            this.ResumeLayout();
        }

        internal void ScaleBuilding(float _ScaleModifier)
        {
            DisplayedBuilding.ScaleBuilding(_ScaleModifier);
        }

        internal void ResetSelectedRoom()
        {
            DisplayedBuilding.ResetSelectedRoom();
        }
    }
}
