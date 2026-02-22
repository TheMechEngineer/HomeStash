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
    public partial class BuildingControl : UserControl
    {
        private RootManager RootManagerInstance;
        int ScaleFactor;

        public int InitialWidth { get; }
        public int InitialHeight { get; }

        public BuildingControl(ref RootManager _ProgramRoot, int _ScaleFactor)
        {
            InitializeComponent();

            RootManagerInstance = _ProgramRoot;
            ScaleFactor = _ScaleFactor;

            InitialWidth = RootManagerInstance.ActiveUser.ActiveBuilding.Width * ScaleFactor;
            InitialHeight = RootManagerInstance.ActiveUser.ActiveBuilding.Height * ScaleFactor;

            this.Width = InitialWidth;
            this.Height = InitialHeight;
        }
    }
}
