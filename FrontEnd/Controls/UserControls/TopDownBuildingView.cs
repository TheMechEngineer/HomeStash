using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;
using BrightIdeasSoftware;
using FrontEnd.Controls.Utilities;
using FrontEnd.Utilities;


namespace FrontEnd.UserControls
{
    internal partial class TopDownBuildingView : UserControl
    {
        private RootManager RootManagerInstance;
        private Building CurrentBuilding;
        private BuildingControlBuffer CurrentBufferedBuilding;

        private Room? SelectedRoom;
        private Item? SelectedItem;

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

            this.tstxtWidth.Text = CurrentBuilding.Width.ToString();
            this.tstxtHeight.Text = CurrentBuilding.Height.ToString();

            this.CameraPanel.Controls.Add(CurrentBufferedBuilding);

            tsbtnEditRoom.Enabled = SelectedRoom != null;
            tsbtnDeleteRoom.Enabled = SelectedRoom != null;

            tsbtnEditItem.Enabled = SelectedRoom != null;
            tsbtnMoveItem.Enabled = SelectedRoom != null;
            tsbtnDeleteItem.Enabled = SelectedRoom != null;
        }

        private void Wire()
        {
            this.Load += TopDownBuildingView_Load;
            CurrentBufferedBuilding.RoomSelectionChanged += CurrentBufferedBuilding_RoomSelectionChanged;
            CurrentBuilding.StoredItemsChanged += CurrentBuilding_StoredItemsChanged;
            CurrentBuilding.RoomListChanged += CurrentBuilding_RoomListChanged;
            this.HandleDestroyed += UnWire;
        }

        private void UnWire(object? sender, EventArgs e)
        {
            this.Load -= TopDownBuildingView_Load;
            CurrentBufferedBuilding.RoomSelectionChanged -= CurrentBufferedBuilding_RoomSelectionChanged;
            CurrentBuilding.StoredItemsChanged -= CurrentBuilding_StoredItemsChanged;
            CurrentBuilding.RoomListChanged -= CurrentBuilding_RoomListChanged;
            this.HandleDestroyed -= UnWire;
        }

        private void TopDownBuildingView_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(() =>
            {
                ResetSplitPanelSize();
                FitBuildingToScreen();
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

            //We want to for the entire building to be on the screen, so we select the scale that is smaller between vertical or horizontal.
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

        private void AddNewItem()
        {
            ItemInfo AddNewItem = new ItemInfo(CurrentBuilding);

            AddNewItem.ConfirmClicked += ItemInfo_ConfirmClicked;
            AddNewItem.CancelClicked += ItemInfo_CancelClicked;

            AddNewItem.Dock = DockStyle.Fill;
            AddNewItem.Name = "AddNewItem";

            splTopView.SplitterDistance = splTopView.ClientSize.Width - AddNewItem.Width;
            splTopView.Panel2.Controls.Add(AddNewItem);
            AddNewItem.BringToFront();

            tsrTopDown.Enabled = false;

            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        private void ModifyItem(bool _ModifyOrMove)
        {
            ItemInfo ModifyItem = new ItemInfo(_ModifyOrMove, SelectedItem, CurrentBuilding);

            ModifyItem.ConfirmClicked += ItemInfo_ConfirmClicked;
            ModifyItem.CancelClicked += ItemInfo_CancelClicked;

            ModifyItem.Dock = DockStyle.Fill;
            ModifyItem.Name = "ModifyItem";

            splTopView.SplitterDistance = splTopView.ClientSize.Width - ModifyItem.Width;
            splTopView.Panel2.Controls.Add(ModifyItem);
            ModifyItem.BringToFront();

            tsrTopDown.Enabled = false;

            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        private void DeleteItem()
        {
            string? _ErrorMessage;

            if (!SelectedItem.ImmediateParent.TryRemoveIStored(SelectedItem, out _ErrorMessage))
            {
                MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateTreeListView()
        {
            CurrentTreeListView.SelectionChanged += CurrentTreeListView_SelectionChanged;

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
            ColName.Width = 250;
            ColName.Sortable = false;

            OLVColumn ColCount = new OLVColumn();
            ColCount.Text = "Count";
            ColCount.Width = 130;
            ColCount.Sortable = false;
            ColCount.AspectGetter = delegate (object x) //Aspect Getter Is Used Becuase Properties Are Different Across Objects
            {
                if (x is Building CurrentBuilding) { return "Total: " + CurrentBuilding.TotalItemCount().ToString(); }
                if (x is Room CurrentRoom) { return "Subtotal: " + CurrentRoom.TotalItemCount(); }
                if (x is BackEnd.ModelClasses.Container CurrentContainer)
                {
                    int ContainerQTY = CurrentContainer.Quantity;
                    int ChildrenQTY = (CurrentContainer.TotalItemCount() / CurrentContainer.Quantity) - 1;
                    return $"{ContainerQTY} ({ChildrenQTY} Children Each)";
                }
                if (x is Item CurrentItem) { return CurrentItem.Quantity; }
                return "";
            };

            OLVColumn ColUnitValue = new OLVColumn();
            ColUnitValue.Text = "Unit Value";
            ColUnitValue.Width = 200;
            ColUnitValue.Sortable = false;
            ColUnitValue.AspectGetter = delegate (object x)
            {
                if (x is Building CurrentBuilding) { return ""; }
                if (x is Room CurrentRoom) { return ""; }
                if (x is BackEnd.ModelClasses.Container CurrentContainer)
                {
                    double ContainerValue = CurrentContainer.Value;
                    double ChildrenValue = (CurrentContainer.TotalItemValue() / CurrentContainer.Quantity) - CurrentContainer.Value;
                    return $"{string.Format("{0:C2}", ContainerValue)} ({string.Format("{0:C2}", ChildrenValue)} Children Value Each)";
                }
                if (x is Item CurrentItem) { return string.Format("{0:C2}", CurrentItem.Value); }
                return "";
            };

            OLVColumn ColTotalValue = new OLVColumn();
            ColTotalValue.Text = "Total Value";
            ColTotalValue.Width = 120;
            ColTotalValue.Sortable = false;
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
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return CurrentContainer.StoredItems.Count > 0; }
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
            // If I have time to refactor, have this resize based on if the vertical scrollbar appear
            //Reference what i did in the selection control
            CombinedColumnWidth = Convert.ToInt32(CombinedColumnWidth * 1.05);
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

        private void SetRoomControls(Room? _SelectedRoom)
        {
            SelectedRoom = _SelectedRoom;
            tsbtnEditRoom.Enabled = SelectedRoom != null;
            tsbtnDeleteRoom.Enabled = SelectedRoom != null;
        }

        private void SetItemControls(Item? _SelectedItem)
        {
            SelectedItem = _SelectedItem;
            tsbtnEditItem.Enabled = SelectedItem != null;
            tsbtnMoveItem.Enabled = SelectedItem != null;
            tsbtnDeleteItem.Enabled = SelectedItem != null;
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

        private void tsbtnAddItem_Click(object sender, EventArgs e)
        {
            AddNewItem();
        }

        private void tsbtnEditItem_Click(object sender, EventArgs e)
        {
            ModifyItem(true);
        }
        private void tsbtnMoveItem_Click(object sender, EventArgs e)
        {
            ModifyItem(false);
        }

        private void tsbtnDeleteItem_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        private void RoomInfo_ConfirmClicked(FormType _FormType, Room? _ModifiedRoom, RoomInfo _CurrentControl, (string Name, float Width, float Height, float CenterX, float CenterY, int ColorValue) _RoomValues)
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
                if (CurrentBuilding.TryModifyRoom(_ModifiedRoom, _RoomValues.Name, _RoomValues.Width, _RoomValues.Height, _RoomValues.CenterX, _RoomValues.CenterY, _RoomValues.ColorValue, out _ErrorMessage))
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

        private void ItemInfo_ConfirmClicked(FormType _FormType, bool _ModifyOrMove, Item? _ModifiedItem, ItemInfo _CurrentControl, (string Name, string Description, double Value, int Quantity, IStorageHolder Location, BackEnd.Enumerations.StoredItemType CreationType) _ItemValues)
        {
            string? _ErrorMessage;

            if (_FormType == FormType.Add)
            {
                if (_ItemValues.Location.TryAddIStored(_ItemValues.CreationType, _ItemValues.Name, _ItemValues.Description, _ItemValues.Value, _ItemValues.Quantity, out _ErrorMessage))
                {
                    ItemInfo_CancelClicked(_CurrentControl);
                }
                else
                {
                    MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (_FormType == FormType.Modify)

            //new to add a nested if here to account for move or modify
            {
                if (_ModifyOrMove)
                {
                    if (_ItemValues.Location.TryModifyIStored(_ModifiedItem, _ItemValues.Name, _ItemValues.Description, _ItemValues.Value, _ItemValues.Quantity, out _ErrorMessage))
                    {
                        ItemInfo_CancelClicked(_CurrentControl);
                    }
                    else
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (_ModifiedItem.ImmediateParent.TryMoveIStored(_ModifiedItem, _ItemValues.Location, out _ErrorMessage))
                    {
                        ItemInfo_CancelClicked(_CurrentControl);
                    }
                    else
                    {
                        MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ItemInfo_CancelClicked(ItemInfo _CurrentControl)
        {
            _CurrentControl.ConfirmClicked -= ItemInfo_ConfirmClicked;
            _CurrentControl.CancelClicked -= ItemInfo_CancelClicked;

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
            SetRoomControls(_SelectedRoom);
            SetItemControls(null);
        }

        private void CurrentBuilding_StoredItemsChanged()
        {
            RefreshTreeListView();
        }

        private void CurrentBuilding_RoomListChanged()
        {
            RefreshTreeListView();
        }

        private void tstxtWidth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                string? ErrorMessage;
                ToolStripTextBox CurrentTextBox = sender as ToolStripTextBox;

                try
                {
                    if
                    (
                        !this.RootManagerInstance.ActiveUser.TryModifyBuilding
                        (
                            this.CurrentBuilding,
                            this.CurrentBuilding.Name,
                            Convert.ToSingle(CurrentTextBox.Text),
                            this.CurrentBuilding.Height,
                            out ErrorMessage
                        )
                    )
                    {
                        MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.tstxtWidth.Text = CurrentBuilding.Width.ToString();
                    }
                }
                catch
                {
                    MessageBox.Show("Format Error: Width Must Be A Number", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.tstxtWidth.Text = CurrentBuilding.Width.ToString();
                }
            }
        }
        private void tstxtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                string? ErrorMessage;
                ToolStripTextBox CurrentTextBox = sender as ToolStripTextBox;

                try
                {
                    if
                    (
                        !this.RootManagerInstance.ActiveUser.TryModifyBuilding
                        (
                            this.CurrentBuilding,
                            this.CurrentBuilding.Name,
                            this.CurrentBuilding.Width,
                            Convert.ToSingle(CurrentTextBox.Text),
                            out ErrorMessage
                        )
                    )
                    {
                        MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.tstxtWidth.Text = CurrentBuilding.Width.ToString();
                    }
                }
                catch
                {
                    MessageBox.Show("Format Error: Height Must Be A Number", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.tstxtWidth.Text = CurrentBuilding.Width.ToString();
                }
            }
        }

        private void CurrentTreeListView_SelectionChanged(object? sender, EventArgs e)
        {
            object? SelectedTreeListViewObject;

            if (CurrentTreeListView.SelectedObject != null)
            {
                SelectedTreeListViewObject = CurrentTreeListView.SelectedObject;
                Type SelectedType = SelectedTreeListViewObject.GetType();

                switch (SelectedType)
                {
                    case Type CurrentType when SelectedType == typeof(Item):
                        SetRoomControls(null);
                        SetItemControls(SelectedTreeListViewObject as Item);
                        break;
                    case Type CurrentType when SelectedType == typeof(BackEnd.ModelClasses.Container):
                        SetRoomControls(null);
                        SetItemControls(SelectedTreeListViewObject as BackEnd.ModelClasses.Container);
                        break;
                    case Type CurrentType when SelectedType == typeof(Room):
                        SetRoomControls(SelectedTreeListViewObject as Room);
                        SetItemControls(null);
                        break;
                    case Type CurrentType when SelectedType == typeof(Building):
                        SetRoomControls(null);
                        SetItemControls(null);
                        break;
                }

                //CurrentBufferedBuilding.ResetSelectedRoom(); //This Prevents Room Controls From Enabling
                //MessageBox.Show(SelectedType.ToString());
            }
        }
    }
}
