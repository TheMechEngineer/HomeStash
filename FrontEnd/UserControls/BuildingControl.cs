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
        private RootManager RootManagerInstance;
        private int DefaultScaleFactor;

        internal int InitialWidth { get; }
        internal int InitialHeight { get; }

        internal BuildingControl(ref RootManager _ProgramRoot, int _DefaultScaleFactor)
        {
            InitializeComponent();

            RootManagerInstance = _ProgramRoot;
            DefaultScaleFactor = _DefaultScaleFactor;

            InitialWidth = RootManagerInstance.ActiveUser.ActiveBuilding.Width * DefaultScaleFactor;
            InitialHeight = RootManagerInstance.ActiveUser.ActiveBuilding.Height * DefaultScaleFactor;

            this.Width = InitialWidth;
            this.Height = InitialHeight;
        }
    }
}
