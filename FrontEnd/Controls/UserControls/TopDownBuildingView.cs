using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;
using BrightIdeasSoftware;
using FrontEnd.Controls.Utilities;
using FrontEnd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BrightIdeasSoftware.TreeListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace FrontEnd.UserControls
{
    internal partial class TopDownBuildingView : UserControl
    {
        private RootManager RootManagerInstance;
        private Building CurrentBuilding;
        private BuildingControlBuffer CurrentBufferedBuilding;

        private Room? SelectedRoom;

        private Panel CameraPanel;

        TreeListView CurrentTreeListView = new TreeListView();

        internal TopDownBuildingView(ref RootManager _ProgramRoot)
        {
            InitializeComponent();

            this.RootManagerInstance = _ProgramRoot;
            this.CurrentBuilding = RootManagerInstance.ActiveUser.ActiveBuilding;

            InitializeVisuals();
            Wire();
            
        }

        private void InitializeVisuals()
        {
            GenerateTreeListView();

            splTopView.Panel2.Controls.Add(CurrentTreeListView);

            this.CameraPanel = splTopView.Panel1.Controls["pnlTopViewCamera"] as Panel;

            this.CurrentBufferedBuilding = new BuildingControlBuffer(CurrentBuilding);

            CurrentBufferedBuilding.Dock = DockStyle.None;
            CurrentBufferedBuilding.Name = "CurrentBufferedBuilding";
            CurrentBufferedBuilding.Location = new Point(0, 0);
            CurrentBufferedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            this.tsnudHGridCount.Value = CurrentBufferedBuilding.HGridCount;
            this.tsnudVGridCount.Value = CurrentBufferedBuilding.VGridCount;

            this.CameraPanel.Controls.Add(CurrentBufferedBuilding);

            tsbtnEditRoom.Enabled = SelectedRoom != null;
            tsbtnDeleteRoom.Enabled = SelectedRoom != null;
            tsbtnAddItemToRoom.Enabled = SelectedRoom != null;
        }

        private void Wire()
        {
            this.Load += TopDownBuildingView_Load;
            CurrentBufferedBuilding.RoomSelectionChanged += CurrentBufferedBuilding_RoomSelectionChanged;
            CurrentBufferedBuilding.StoredItemsChanged += CurrentBufferedBuilding_StoredItemsChanged;
            CurrentBufferedBuilding.RoomListChanged += CurrentBufferedBuilding_RoomListChanged;
            this.HandleDestroyed += UnWire;
        }

        private void UnWire(object? sender, EventArgs e)
        {
            this.Load -= TopDownBuildingView_Load;
            CurrentBufferedBuilding.RoomSelectionChanged -= CurrentBufferedBuilding_RoomSelectionChanged;
            CurrentBufferedBuilding.StoredItemsChanged -= CurrentBufferedBuilding_StoredItemsChanged;
            CurrentBufferedBuilding.RoomListChanged -= CurrentBufferedBuilding_RoomListChanged;
            this.HandleDestroyed -= UnWire;
        }

        private void TopDownBuildingView_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(() =>
            {
                ResetSplitPanelSize();
                FitBuildingToScreen();
                CenterCameraView();
            });
        }

        private void CenterCameraView()
        {
            int BufferedWidth = this.CurrentBufferedBuilding.Width;
            int BufferedHeight = this.CurrentBufferedBuilding.Height;

            int CameraWidth = this.CameraPanel.ClientSize.Width;
            int CameraHeight = this.CameraPanel.ClientSize.Height;

            int ViewLeftBound = (BufferedWidth - CameraWidth) / 2;
            int ViewTopBound = (BufferedHeight - CameraHeight) / 2;

            this.CameraPanel.AutoScrollPosition = new Point(ViewLeftBound, ViewTopBound);
        }

        private void FitBuildingToScreen()
        {
            // The buffer is larger than the building by 2 * BuildingOffsetBuffer on each axis.
            // We need the scale multiplier for the building itself, not the buffer,
            // so we strip the buffer padding out before calculating the ratio.

            float PercentOfScreenToFill = 0.95f;

            float BufferedControlWidth = Convert.ToSingle(this.CurrentBufferedBuilding.Width);
            float BuildingControlWidth = Convert.ToSingle(BufferedControlWidth - (2 * CurrentBufferedBuilding.BuildingOffsetBuffer));

            float BufferedControlHeight = Convert.ToSingle(this.CurrentBufferedBuilding.Height);
            float BuildingControlHeight = Convert.ToSingle(BufferedControlHeight - (2 * CurrentBufferedBuilding.BuildingOffsetBuffer));

            float DesiredBufferWidth = PercentOfScreenToFill * Convert.ToSingle(this.CameraPanel.ClientSize.Width);
            float WidthLinearIncrease = DesiredBufferWidth - BufferedControlWidth;

            float DesiredBufferHeight = PercentOfScreenToFill * Convert.ToSingle(this.CameraPanel.ClientSize.Height);
            float HeightLinearIncrease = DesiredBufferHeight - BufferedControlHeight;

            float RequiredWidthScale = (WidthLinearIncrease + BuildingControlWidth) / BuildingControlWidth;
            float RequiredHeightScale = (HeightLinearIncrease + BuildingControlHeight) / BuildingControlHeight;

            //We want to fir the entire building on to the screen, so we select the scale that is smaller between vertical or horizontal.
            //This way it will always scale to have one side be 95% of the screen, and the other to be less than.
            float SelectedScale = Math.Min(RequiredWidthScale, RequiredHeightScale);

            CurrentBufferedBuilding.ScaleBuilding(SelectedScale);
        }

        private void AddNewRoom()
        {
            RoomInfo AddNewRoom = new RoomInfo();

            AddNewRoom.ConfirmClicked += RoomInfo_ConfirmClicked;
            AddNewRoom.CancelClicked += RoomInfo_CancelClicked;

            AddNewRoom.Dock = DockStyle.Fill;
            AddNewRoom.Name = "AddNewRoom";

            splTopView.SplitterDistance = splTopView.ClientSize.Width - AddNewRoom.Width;
            splTopView.Panel2.Controls.Add(AddNewRoom);
            AddNewRoom.BringToFront();

            tsrTopDown.Enabled = false;

            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        private void ModifyRoom()
        {
            RoomInfo ModifyRoom = new RoomInfo(SelectedRoom);

            ModifyRoom.ConfirmClicked += RoomInfo_ConfirmClicked;
            ModifyRoom.CancelClicked += RoomInfo_CancelClicked;

            ModifyRoom.Dock = DockStyle.Fill;
            ModifyRoom.Name = "ModifyRoom";

            splTopView.SplitterDistance = splTopView.ClientSize.Width - ModifyRoom.Width;
            splTopView.Panel2.Controls.Add(ModifyRoom);
            ModifyRoom.BringToFront();

            tsrTopDown.Enabled = false;

            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        private void DeleteRoom()
        {
            string? _ErrorMessage;

            if (!CurrentBuilding.TryRemoveRoom(SelectedRoom, out _ErrorMessage))
            {
                MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateTreeListView()
        {
            //TreeListView TestView = new TreeListView();
            CurrentTreeListView.Dock = DockStyle.Fill;

            CurrentTreeListView.UseAlternatingBackColors = true;
            CurrentTreeListView.AlternateRowBackColor = Color.Beige;

            //I dont think this is needed, but if I want to add images later (icons next to building room item container, maybe i need this)
            ImageList IMG = new ImageList();
            IMG.ImageSize = new Size(16, 16);
            CurrentTreeListView.SmallImageList = IMG;

            // Could also be done like: OLVColumn nameColumn = new OLVColumn("Name", "Name");
            //But I like the more verbose approach
            OLVColumn ColName = new OLVColumn();
            ColName.Text = "Name";
            ColName.AspectName = "Name"; //Dont Need To Use Aspect Getter Because The Property Is The Same Name Across All Objects
            ColName.Width = 200;

            OLVColumn ColCount = new OLVColumn();
            ColCount.Text = "Count";
            ColCount.Width = 120;
            ColCount.AspectGetter = delegate (object x) //Aspect Getter Is Used Becuase Properties Are Different Across Objects
            {
                if (x is Building CurrentBuilding) { return "Total: " + CurrentBuilding.TotalItemCount().ToString(); }
                if (x is Room CurrentRoom) { return "Subtotal: " + CurrentRoom.TotalItemCount(); }
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return CurrentContainer.Quantity; }
                if (x is Item CurrentItem) { return CurrentItem.Quantity; }
                return "";
            };

            OLVColumn ColUnitValue = new OLVColumn();
            ColUnitValue.Text = "Unit Value";
            ColUnitValue.Width = 120;
            ColUnitValue.AspectGetter = delegate (object x)
            {
                if (x is Building CurrentBuilding) { return ""; }
                if (x is Room CurrentRoom) { return ""; }
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return string.Format("{0:C2}", CurrentContainer.Value); }
                if (x is Item CurrentItem) { return string.Format("{0:C2}", CurrentItem.Value); }
                return "";
            };

            OLVColumn ColTotalValue = new OLVColumn();
            ColTotalValue.Text = "Total Value";
            ColTotalValue.Width = 120;
            ColTotalValue.AspectGetter = delegate (object x)
            {
                if (x is Building CurrentBuilding) { return "Total: " + string.Format("{0:C2}", CurrentBuilding.TotalItemValue()); }
                if (x is Room CurrentRoom) { return "Subtotal: " + string.Format("{0:C2}", CurrentRoom.TotalItemValue()); }
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return "Subtotal: " + string.Format("{0:C2}", CurrentContainer.TotalItemValue()); }
                if (x is Item CurrentItem) { return string.Format("{0:C2}", CurrentItem.Value * CurrentItem.Quantity); }
                return "";
            };

            List<OLVColumn> ColumnList = new List<OLVColumn> { ColName, ColCount, ColUnitValue, ColTotalValue };

            foreach (OLVColumn CurrentColumn in ColumnList)
            {
                CurrentTreeListView.AllColumns.Add(CurrentColumn);
            }

            //This Is Needed Or The Columns Dont Appear
            CurrentTreeListView.RebuildColumns();

            //This Also Works Instead Of The Above Approach Of Adding Columns
            //TestView.Columns.AddRange(ColumnList.Cast<ColumnHeader>().ToArray());

            CurrentTreeListView.CanExpandGetter = delegate (object x)
            {
                if (x is Building CurrentBuilding) { return CurrentBuilding.RoomList.Count > 0 || CurrentBuilding.StoredItems.Count > 0; }
                if (x is Room CurrentRoom) { return CurrentRoom.StoredItems.Count > 0; }
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return true; } //CurrentContainer.StoredItems.Count > 0
                if (x is Item CurrentItem) { return false; }
                return false;
            };

            CurrentTreeListView.ChildrenGetter = delegate (object x)
            {
                if (x is Building CurrentBuilding)
                {
                    List<object> Children = new List<object>();
                    Children.AddRange(CurrentBuilding.RoomList);
                    Children.AddRange(CurrentBuilding.StoredItems);
                    return Children;
                }
                if (x is Room CurrentRoom) { return CurrentRoom.StoredItems; }
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return CurrentContainer.StoredItems; }
                return null;
            };

            CurrentTreeListView.Roots = new List<Building> { CurrentBuilding };

            CurrentTreeListView.ExpandAll();
            CurrentTreeListView.FullRowSelect = true;
        }

        private void RefreshTreeListView()
        {
            CurrentTreeListView.RebuildAll(true);
        }

        private void ResetSplitPanelSize()
        {
            int CombinedColumnWidth = CurrentTreeListView.AllColumns.Sum(CurrentColumn => CurrentColumn.Width);
            splTopView.SplitterDistance = this.Width - CombinedColumnWidth;
        }


        //This Is Obsolete After Implementing The Tree List View, However I still Want To Keep It In Because I Had To Learn It
        private void GenerateTreeView()
        {
            System.Windows.Forms.TreeView tvBuildingInventory = new System.Windows.Forms.TreeView();

            tvBuildingInventory.Dock = DockStyle.Fill;

            splTopView.Panel2.Controls.Add(tvBuildingInventory);

            tvBuildingInventory.Nodes.Clear();

            TreeNode BuildingNode = new TreeNode(CurrentBuilding.Name);

            foreach (IStored StoredObject in CurrentBuilding.StoredItems)
            {
                TreeNode StoredNode = new TreeNode(StoredObject.Name);

                BuildingNode.Nodes.Add(StoredNode);
            }

            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                TreeNode RoomNode = new TreeNode(CurrentRoom.Name);

                foreach (IStored StoredObject in CurrentRoom.StoredItems)
                {
                    TreeNode StoredNode = new TreeNode(StoredObject.Name);

                    RoomNode.Nodes.Add(StoredNode);
                }

                BuildingNode.Nodes.Add(RoomNode);
            }

            tvBuildingInventory.Nodes.Add(BuildingNode);
            tvBuildingInventory.ExpandAll();
        }

        private void tsbtnScale_Click(object sender, EventArgs e)
        {
            if (sender == this.tsbtnScaleDown)
            {
                CurrentBufferedBuilding.ScaleBuilding(.9f);
            }
            else if (sender == this.tsbtnScaleUp)
            {
                CurrentBufferedBuilding.ScaleBuilding(1.1f);
            }
        }

        private void tsbtnScale_MouseDown(object sender, MouseEventArgs e)
        {
            ClickHoldTimer.Tag = sender as ToolStripButton;
            ClickHoldTimer.Start();
        }

        private void tsbtnScale_MouseUp(object sender, MouseEventArgs e)
        {
            ClickHoldTimer.Tag = null;
            ClickHoldTimer.Stop();
        }

        private void tsbtnScale_MouseLeave(object sender, EventArgs e)
        {
            ClickHoldTimer.Tag = null;
            ClickHoldTimer.Stop();
        }

        private void ClickHoldTimer_Tick(object sender, EventArgs e)
        {
            ToolStripButton CurrentButton = ClickHoldTimer.Tag as ToolStripButton;
            tsbtnScale_Click(CurrentButton, e);
        }

        private void tsbtnFitToScreen_Click(object sender, EventArgs e)
        {
            FitBuildingToScreen();
        }

        private void tsbtnCenter_Click(object sender, EventArgs e)
        {
            CenterCameraView();
        }

        private void tsbtnAddRoom_Click(object sender, EventArgs e)
        {
            AddNewRoom();
        }

        private void tsbtnEditRoom_Click(object sender, EventArgs e)
        {
            ModifyRoom();
        }

        private void tsbtnDeleteRoom_Click(object sender, EventArgs e)
        {
            DeleteRoom();
        }

        private void RoomInfo_ConfirmClicked(FormType _FormType, Room? _CurrentRoom, RoomInfo _CurrentControl, (string Name, float Width, float Height, float CenterX, float CenterY, int ColorValue) _RoomValues)
        {
            string? _ErrorMessage;

            if (_FormType == FormType.Add)
            {
                if (CurrentBuilding.TryAddRoom(_RoomValues.Name, _RoomValues.Width, _RoomValues.Height, _RoomValues.CenterX, _RoomValues.CenterY, _RoomValues.ColorValue, out _ErrorMessage))
                {
                    RoomInfo_CancelClicked(_CurrentControl);
                }
                else
                {
                    MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (_FormType == FormType.Modify)
            {
                if (CurrentBuilding.TryModifyRoom(_CurrentRoom, _RoomValues.Name, _RoomValues.Width, _RoomValues.Height, _RoomValues.CenterX, _RoomValues.CenterY, _RoomValues.ColorValue, out _ErrorMessage))
                {
                    RoomInfo_CancelClicked(_CurrentControl);
                }
                else
                {
                    MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RoomInfo_CancelClicked(RoomInfo _CurrentControl)
        {
            _CurrentControl.ConfirmClicked -= RoomInfo_ConfirmClicked;
            _CurrentControl.CancelClicked -= RoomInfo_CancelClicked;

            CurrentBufferedBuilding.ResetSelectedRoom();

            tsrTopDown.Enabled = true;

            TransparentPanel BlockerPanel = this.CameraPanel.Controls["Blocker"] as TransparentPanel;
            this.CameraPanel.Controls.Remove(BlockerPanel);
            BlockerPanel.Dispose();

            ResetSplitPanelSize();

            splTopView.Panel2.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        private void tsnudHGridCount_ValueChanged(object sender, EventArgs e)
        {
            CurrentBufferedBuilding.HGridCount = Convert.ToInt32(tsnudHGridCount.Value);

            //Alternate Approach Using The Sender Instead
            //CurrentBufferedBuilding.HGridCount = Convert.ToInt32((sender as ToolStripNumericUpDown).Value);
        }

        private void tsnudVGridCount_ValueChanged(object sender, EventArgs e)
        {
            CurrentBufferedBuilding.VGridCount = Convert.ToInt32(tsnudVGridCount.Value);

            //Alternate Approach Using The Sender Instead
            //CurrentBufferedBuilding.VGridCount = Convert.ToInt32((sender as ToolStripNumericUpDown).Value);
        }

        private void CurrentBufferedBuilding_RoomSelectionChanged(Room? _SelectedRoom)
        {
            SelectedRoom = _SelectedRoom;
            tsbtnEditRoom.Enabled = SelectedRoom != null;
            tsbtnAddItemToRoom.Enabled = SelectedRoom != null;
            tsbtnDeleteRoom.Enabled = SelectedRoom != null;
        }



        private void tsbtnAddItemToRoom_Click(object sender, EventArgs e)
        {

        }
        private void CurrentBufferedBuilding_StoredItemsChanged()
        {
            RefreshTreeListView();
        }

        private void CurrentBufferedBuilding_RoomListChanged()
        {
            RefreshTreeListView();
        }


    }
}
