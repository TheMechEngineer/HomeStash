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
        private int DefaultPixelsPerUnit;

        internal int InitialDisplayWidth;
        internal int InitialDisplayHeight;

        internal BuildingControl(ref Building _CurrentBuilding, int _DefaultPixelsPerUnit)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            DefaultPixelsPerUnit = _DefaultPixelsPerUnit;

            InitialDisplayWidth = Convert.ToInt32(Math.Round(CurrentBuilding.Width * DefaultPixelsPerUnit));
            InitialDisplayHeight = Convert.ToInt32(Math.Round(CurrentBuilding.Height * DefaultPixelsPerUnit));

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            this.Width = InitialDisplayWidth;
            this.Height = InitialDisplayHeight;
        }

        //
        //
        // Since this control is responsible for showing the rooms this class should subsribe to the building event that fires when roomlist changes
        // The event handler to attach to that event would be the method to draw the rooms in the building
        //
        //
    }
}
