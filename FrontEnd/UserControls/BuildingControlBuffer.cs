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
        private void temp3()
        {
            temp();
            temp2();
        }

        private void temp2()
        {
            this.Width = this.DisplayedBuilding.Width + 2 * BuildingOffsetBuffer;
            this.Height = this.DisplayedBuilding.Height + 2 * BuildingOffsetBuffer;

            int GridCount = 10;
            float VerticalGap = Convert.ToSingle(DisplayedBuilding.Width) / GridCount;
            float HorizontalGap = Convert.ToSingle(DisplayedBuilding.Height) / GridCount;

            this.SuspendLayout();

            for (int j = 0; j <= 1; j++)
            {
                for (int i = 0; i <= GridCount; i++)
                {
                    Label TestLabel = new Label();

                    TestLabel.AutoSize = true;
                    TestLabel.Text = (CurrentBuilding.Width / GridCount * i).ToString();
                    TestLabel.TextAlign = ContentAlignment.MiddleCenter;
                    TestLabel.ForeColor = BufferTextColor;

                    this.Controls.Add(TestLabel); // Need To Do This First Or AutoSize Doesnt Make Correct Size

                    int ControlLeftPosition = (int)(BuildingOffsetBuffer - (TestLabel.Width / 2) + (VerticalGap * i));
                    int ControlTopPosition = ((DisplayedBuilding.Height + BuildingOffsetBuffer) * j) + (BuildingOffsetBuffer / 2) - (TestLabel.Height / 2);

                    TestLabel.Location = new Point(ControlLeftPosition, ControlTopPosition);
                }
            }

            for (int j = 0; j <= 1; j++)
            {
                for (int i = 0; i <= GridCount; i++)
                {
                    Label TestLabel = new Label();

                    TestLabel.AutoSize = true;
                    TestLabel.Text = (CurrentBuilding.Height / GridCount * i).ToString();
                    TestLabel.TextAlign = ContentAlignment.MiddleCenter;
                    TestLabel.ForeColor = BufferTextColor;

                    this.Controls.Add(TestLabel); // Need To Do This First Or AutoSize Doesnt Make Correct Size

                    int ControlLeftPosition = ((DisplayedBuilding.Width + BuildingOffsetBuffer) * j) + (BuildingOffsetBuffer / 2) - (TestLabel.Width / 2);
                    int ControlTopPosition = (int)(BuildingOffsetBuffer - (TestLabel.Height / 2) + (HorizontalGap * i));

                    TestLabel.Location = new Point(ControlLeftPosition, ControlTopPosition);
                }
            }

            this.ResumeLayout();
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
            //this.Padding = new Padding(BuildingOffsetBuffer); // I used this when I had autosize on. Commenting Out Because Im turning Autosize off.
            this.DisplayedBuilding = new BuildingControl(CurrentBuilding);

            this.Width = this.DisplayedBuilding.Width + 2 * BuildingOffsetBuffer;
            this.Height = this.DisplayedBuilding.Height + 2 * BuildingOffsetBuffer;

            this.BackColor = BufferColor;

            this.DisplayedBuilding.Dock = DockStyle.None;
            this.DisplayedBuilding.Name = "DisplayedBuilding";
            this.DisplayedBuilding.Location = new Point(BuildingOffsetBuffer, BuildingOffsetBuffer);
            this.DisplayedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.Controls.Add(this.DisplayedBuilding);

            temp3();
        }

        private void DrawGridNumerics(Graphics _GraphicsTool)
        {

        }

        private void Wire()
        {
            DisplayedBuilding.BuildingScaled += temp3;
            this.HandleDestroyed += UnWire;
        }
        private void UnWire(object? sender, EventArgs e)
        {
            DisplayedBuilding.BuildingScaled -= temp3;
            this.HandleDestroyed -= UnWire;
        }
    }
}
