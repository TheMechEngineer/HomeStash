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
        private int DefaultScaleFactor;

        internal int InitialDisplayWidth;
        internal int InitialDisplayHeight;

        internal BuildingControl(ref Building _CurrentBuilding, int _DefaultScaleFactor)
        {
            InitializeComponent();

            CurrentBuilding = _CurrentBuilding;
            DefaultScaleFactor = _DefaultScaleFactor;

            InitialDisplayWidth = CurrentBuilding.Width * DefaultScaleFactor;
            InitialDisplayHeight = CurrentBuilding.Height * DefaultScaleFactor;

            InitializeVisuals();
        }

        private void InitializeVisuals()
        {
            this.Width = InitialDisplayWidth;
            this.Height = InitialDisplayHeight;
        }
    }
}
