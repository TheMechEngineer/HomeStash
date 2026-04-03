using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;
using BrightIdeasSoftware;
using FrontEnd.Controls.Utilities;
using FrontEnd.Utilities;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl that displays a top-down view of a building, including its rooms and stored items.
    /// Handles visualization, camera control, and UI interaction for editing building contents.
    /// </summary>
    internal partial class TopDownBuildingView : UserControl
    {
        /// <summary>
        /// The RootManager Instance That Contains The Building Data
        /// </summary>
        private RootManager RootManagerInstance;

        /// <summary>
        /// Reference To The Building Currently Being Displayed
        /// </summary>
        private Building CurrentBuilding;

        /// <summary>
        /// Buffered Building Control
        /// </summary>
        private BuildingControlBuffer CurrentBufferedBuilding;

        /// <summary>
        /// Currently Selected Room In The UI
        /// </summary>
        private Room? SelectedRoom;

        /// <summary>
        /// Currently Selected Item In The UI
        /// </summary>
        private Item? SelectedItem;

        /// <summary>
        /// Panel That Houses The Building View, Allows For Scrolling And Zooming Capabilities
        /// </summary>
        private Panel CameraPanel;

        /// <summary>
        /// Displays A Hierarchical View Of The Building, Rooms, And Items
        /// </summary>
        TreeListView CurrentTreeListView = new TreeListView();

        /// <summary>
        /// Initializes The Top-Down View Using The RootManger Data.
        /// </summary>
        /// <param name="_ProgramRoot">RootManager Instance That Sources The Building Data</param>
        internal TopDownBuildingView(ref RootManager _ProgramRoot)
        {
            InitializeComponent();

            this.RootManagerInstance = _ProgramRoot;
            this.CurrentBuilding = RootManagerInstance.ActiveUser.ActiveBuilding;

            InitializeVisuals();
            Wire();
        }

        /// <summary>
        /// Initializes Visual State Of The Top-Down User Control
        /// </summary>
        private void InitializeVisuals()
        {
            // Create A Tree List View Of The Live Instance Data
            GenerateTreeListView();

            // Display The Tree List View In The Right Panel Of The Split Container
            splTopView.Panel2.Controls.Add(CurrentTreeListView);

            this.CameraPanel = splTopView.Panel1.Controls["pnlTopViewCamera"] as Panel;

            // Create An Instance Of The Buffered Building User Control, Using Live Data
            this.CurrentBufferedBuilding = new BuildingControlBuffer(CurrentBuilding);

            CurrentBufferedBuilding.Dock = DockStyle.None;
            CurrentBufferedBuilding.Name = "CurrentBufferedBuilding";
            CurrentBufferedBuilding.Location = new Point(0, 0);
            CurrentBufferedBuilding.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Set Initial Grid Counts From The Buffered Building
            this.tsnudHGridCount.Value = CurrentBufferedBuilding.HGridCount;
            this.tsnudVGridCount.Value = CurrentBufferedBuilding.VGridCount;

            // Display Current Building Dimensions
            this.tstxtWidth.Text = CurrentBuilding.Width.ToString();
            this.tstxtHeight.Text = CurrentBuilding.Height.ToString();

            // Add Buffered Building User Control To Camera Panel
            this.CameraPanel.Controls.Add(CurrentBufferedBuilding);

            // Enable/Disable Room And Item Buttons Based On Selection
            tsbtnEditRoom.Enabled = SelectedRoom != null;
            tsbtnDeleteRoom.Enabled = SelectedRoom != null;

            tsbtnEditItem.Enabled = SelectedRoom != null;
            tsbtnMoveItem.Enabled = SelectedRoom != null;
            tsbtnDeleteItem.Enabled = SelectedRoom != null;
        }

        /// <summary>
        /// Wires Backend Events To Control Handlers
        /// </summary>
        private void Wire()
        {
            this.Load += TopDownBuildingView_Load;
            CurrentBufferedBuilding.RoomSelectionChanged += CurrentBufferedBuilding_RoomSelectionChanged;
            CurrentBuilding.StoredItemsChanged += CurrentBuilding_StoredItemsChanged;
            CurrentBuilding.RoomListChanged += CurrentBuilding_RoomListChanged;
            this.HandleDestroyed += UnWire;
        }

        /// <summary>
        /// Unwires All Events When The Control Is Destroyed To Avoid Memory Leaks
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void UnWire(object? sender, EventArgs e)
        {
            this.Load -= TopDownBuildingView_Load;
            CurrentBufferedBuilding.RoomSelectionChanged -= CurrentBufferedBuilding_RoomSelectionChanged;
            CurrentBuilding.StoredItemsChanged -= CurrentBuilding_StoredItemsChanged;
            CurrentBuilding.RoomListChanged -= CurrentBuilding_RoomListChanged;
            this.HandleDestroyed -= UnWire;
        }

        /// <summary>
        /// Handles Top-Down Load Event For Visualization That Requires Load First To Be Accurate
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void TopDownBuildingView_Load(object sender, EventArgs e)
        {
            // Defers Execution Until UI Is Fully Loaded
            this.BeginInvoke(() =>
            {
                ResetSplitPanelSize();
                FitBuildingToScreen();
            });
        }

        /// <summary>
        /// When The Buffered Buidling Is Larger Than The Camera Panel, Centers The Camera View On The Buffered Building
        /// </summary>
        private void CenterCameraView()
        {
            int BufferedWidth = this.CurrentBufferedBuilding.Width;
            int BufferedHeight = this.CurrentBufferedBuilding.Height;

            int CameraWidth = this.CameraPanel.ClientSize.Width;
            int CameraHeight = this.CameraPanel.ClientSize.Height;

            // This Calculation Makes Sense, Because The AutoScroll Bar Full Height Or Width Are The Larger Object
            // The Active Handle Of The Scroll Bar Represents The Full Height Or Width Of The ViewPort
            // When You set The Position Of The Scroll Bar, You Are Setting Its Top Or Left Position
            // So By Setting It At Half The Difference, You Are Splitting The Difference Between The Total View
            int ViewLeftBound = (BufferedWidth - CameraWidth) / 2;
            int ViewTopBound = (BufferedHeight - CameraHeight) / 2;

            // Move The Scroll Bar
            this.CameraPanel.AutoScrollPosition = new Point(ViewLeftBound, ViewTopBound);
        }

        /// <summary>
        /// Scales The Building To Fit Within The Camera Panel
        /// </summary>
        private void FitBuildingToScreen()
        {
            // The Buffer Is Larger Than The Building By 2 * BuildingOffsetBuffer On Each Axis
            // We Need The Scale Multiplier For The Building Itself, Not The Buffer,
            // So We Strip The Buffer Padding Out Before Calculating The Ratio

            float PercentOfScreenToFill = 0.95f;

            // Calculates Building Dimensions Without Buffer Offset
            float BufferedControlWidth = Convert.ToSingle(this.CurrentBufferedBuilding.Width);
            float BuildingControlWidth = Convert.ToSingle(BufferedControlWidth - (2 * CurrentBufferedBuilding.BuildingOffsetBuffer));

            float BufferedControlHeight = Convert.ToSingle(this.CurrentBufferedBuilding.Height);
            float BuildingControlHeight = Convert.ToSingle(BufferedControlHeight - (2 * CurrentBufferedBuilding.BuildingOffsetBuffer));

            // Calculates Desired Buffer Size Relative To Screen
            float DesiredBufferWidth = PercentOfScreenToFill * Convert.ToSingle(this.CameraPanel.ClientSize.Width);
            float WidthLinearIncrease = DesiredBufferWidth - BufferedControlWidth;

            float DesiredBufferHeight = PercentOfScreenToFill * Convert.ToSingle(this.CameraPanel.ClientSize.Height);
            float HeightLinearIncrease = DesiredBufferHeight - BufferedControlHeight;

            // Determines Required Scaling Factors
            float RequiredWidthScale = (WidthLinearIncrease + BuildingControlWidth) / BuildingControlWidth;
            float RequiredHeightScale = (HeightLinearIncrease + BuildingControlHeight) / BuildingControlHeight;

            // We Want To For The Entire Building To Be On The Screen, So We Select The Scale That Is Smaller Between Vertical Or Horizontal
            // This Way It Will Always Scale To Have One Side Be 95% Of The Screen, And The Other To Be Less Than The Full Screen
            float SelectedScale = Math.Min(RequiredWidthScale, RequiredHeightScale);

            CurrentBufferedBuilding.ScaleBuilding(SelectedScale);
        }

        /// <summary>
        /// Displays Add Room Control And Blocks Background Interaction
        /// </summary>
        private void AddNewRoom()
        {
            RoomInfo AddNewRoom = new RoomInfo();

            // Wires Control Events
            AddNewRoom.ConfirmClicked += RoomInfo_ConfirmClicked;
            AddNewRoom.CancelClicked += RoomInfo_CancelClicked;

            AddNewRoom.Dock = DockStyle.Fill;
            AddNewRoom.Name = "AddNewRoom";

            // Positions Control In Right Panel Amd Sizes Panel To Match Control
            splTopView.SplitterDistance = splTopView.ClientSize.Width - AddNewRoom.Width;
            splTopView.Panel2.Controls.Add(AddNewRoom);
            AddNewRoom.BringToFront();

            // Disables Toolbar While In The Add New Room Control
            tsrTopDown.Enabled = false;

            // Adds Blocking Panel Over Camera View, To Prevent Iteraction With Camera Panel Elements While Adding Room
            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            // Brings Invisible Block Panel To The Front So All Attempted Clicks Are Intercepted
            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        /// <summary>
        /// Displays Modify Room Control And Blocks Background Interaction
        /// </summary>
        private void ModifyRoom()
        {
            RoomInfo ModifyRoom = new RoomInfo(SelectedRoom);

            // Wires Control Events
            ModifyRoom.ConfirmClicked += RoomInfo_ConfirmClicked;
            ModifyRoom.CancelClicked += RoomInfo_CancelClicked;

            ModifyRoom.Dock = DockStyle.Fill;
            ModifyRoom.Name = "ModifyRoom";

            // Positions Control In Right Panel Amd Sizes Panel To Match Control
            splTopView.SplitterDistance = splTopView.ClientSize.Width - ModifyRoom.Width;
            splTopView.Panel2.Controls.Add(ModifyRoom);
            ModifyRoom.BringToFront();

            // Disables Toolbar While In The Modify Room Control
            tsrTopDown.Enabled = false;

            // Adds Blocking Panel Over Camera View, To Prevent Iteraction With Camera Panel Elements While Modifying Room
            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            // Brings Invisible Block Panel To The Front So All Attempted Clicks Are Intercepted
            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        /// <summary>
        /// Attempts To Delete The Currently Selected Room From The Building
        /// </summary>
        private void DeleteRoom()
        {
            string? _ErrorMessage;

            // Attempt To Delete Room And Show Error If Delete Unsuccessful
            if (!CurrentBuilding.TryRemoveRoom(SelectedRoom, out _ErrorMessage))
            {
                MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Displays Add Item Control And Blocks Background Interaction
        /// </summary>
        private void AddNewItem()
        {
            ItemInfo AddNewItem = new ItemInfo(CurrentBuilding);

            // Wires Control Events
            AddNewItem.ConfirmClicked += ItemInfo_ConfirmClicked;
            AddNewItem.CancelClicked += ItemInfo_CancelClicked;

            AddNewItem.Dock = DockStyle.Fill;
            AddNewItem.Name = "AddNewItem";

            // Positions Control In Right Panel Amd Sizes Panel To Match Control
            splTopView.SplitterDistance = splTopView.ClientSize.Width - AddNewItem.Width;
            splTopView.Panel2.Controls.Add(AddNewItem);
            AddNewItem.BringToFront();

            // Disables Toolbar While In The Add New Item Control
            tsrTopDown.Enabled = false;

            // Adds Blocking Panel Over Camera View, To Prevent Iteraction With Camera Panel Elements While Adding Item
            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            // Brings Invisible Block Panel To The Front So All Attempted Clicks Are Intercepted
            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        /// <summary>
        /// Displays Modify Or Move Item Control And Blocks Background Interaction
        /// </summary>
        /// <param name="_ModifyOrMove">If Operation Modifys Or Moves An Item</param>
        private void ModifyItem(bool _ModifyOrMove)
        {
            ItemInfo ModifyItem = new ItemInfo(_ModifyOrMove, SelectedItem, CurrentBuilding);

            // Wires Control Events
            ModifyItem.ConfirmClicked += ItemInfo_ConfirmClicked;
            ModifyItem.CancelClicked += ItemInfo_CancelClicked;

            ModifyItem.Dock = DockStyle.Fill;
            ModifyItem.Name = "ModifyItem";

            // Positions Control In Right Panel Amd Sizes Panel To Match Control
            splTopView.SplitterDistance = splTopView.ClientSize.Width - ModifyItem.Width;
            splTopView.Panel2.Controls.Add(ModifyItem);
            ModifyItem.BringToFront();

            // Disables Toolbar While In The Modify/Move Item Control
            tsrTopDown.Enabled = false;

            // Adds Blocking Panel Over Camera View, To Prevent Iteraction With Camera Panel Elements While Modifying/Moving Item
            TransparentPanel BlockerPanel = new TransparentPanel();
            BlockerPanel.Name = "Blocker";
            BlockerPanel.Dock = DockStyle.Fill;
            BlockerPanel.BackColor = Color.Black;
            BlockerPanel.Opacity = 20;

            // Brings Invisible Block Panel To The Front So All Attempted Clicks Are Intercepted
            this.CameraPanel.Controls.Add(BlockerPanel);
            BlockerPanel.BringToFront();
        }

        /// <summary>
        /// Attempts To Delete The Currently Selected Item From Its Parent Container
        /// </summary>
        private void DeleteItem()
        {
            string? _ErrorMessage;

            // Attempt To Delete Item And Show Error If Delete Unsuccessful
            if (!SelectedItem.ImmediateParent.TryRemoveIStored(SelectedItem, out _ErrorMessage))
            {
                MessageBox.Show(_ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configures And Generates The TreeListView For Displaying Building Data
        /// </summary>
        private void GenerateTreeListView()
        {
            // Wires Control Events
            CurrentTreeListView.SelectionChanged += CurrentTreeListView_SelectionChanged;

            CurrentTreeListView.Dock = DockStyle.Fill;

            // Enable Alternating Back Colors For The Rows
            CurrentTreeListView.UseAlternatingBackColors = true;
            CurrentTreeListView.AlternateRowBackColor = Color.Beige;

            // I Dont Think This Is Needed, But If I Want To Add Images Later (Icons Next To Building Room Item Container, Maybe I Need This)
            ImageList IMG = new ImageList();
            IMG.ImageSize = new Size(16, 16);
            CurrentTreeListView.SmallImageList = IMG;

            // Could Also Be Done Like : OLVColumn nameColumn = new OLVColumn("Name", "Name");
            // But I Like The More Verbose Approach
            // Creates The Name Column, And Specifies That It Should Use The "Name" Property Of The Source Objects To Populate Its Values
            OLVColumn ColName = new OLVColumn();
            ColName.Text = "Name";
            // This Sets The Value Displayed In Each Row Of The Column
            ColName.AspectName = "Name"; //Dont Need To Use Aspect Getter Because The Property Is The Same Name Across All Objects
            ColName.Width = 250;
            ColName.Sortable = false;

            // Creates The Count Column, And Specifies How It Should Dyanmically Display The Count Based On The Source Object Type
            OLVColumn ColCount = new OLVColumn();
            ColCount.Text = "Count";
            ColCount.Width = 130;
            ColCount.Sortable = false;
            // This Sets The Value Displayed In Each Row Of The Column
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

            // Creates The Unit Value Column, And Specifies How It Should Dyanmically Display The Monetary Unit Value Based On The Source Object Type
            OLVColumn ColUnitValue = new OLVColumn();
            ColUnitValue.Text = "Unit Value";
            ColUnitValue.Width = 200;
            ColUnitValue.Sortable = false;
            // This Sets The Value Displayed In Each Row Of The Column
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

            // Creates The Total Value Column, And Specifies How It Should Dyanmically Display The Monetary Total Value Based On The Source Object Type
            OLVColumn ColTotalValue = new OLVColumn();
            ColTotalValue.Text = "Total Value";
            ColTotalValue.Width = 120;
            ColTotalValue.Sortable = false;
            // This Sets The Value Displayed In Each Row Of The Column
            ColTotalValue.AspectGetter = delegate (object x)
            {
                if (x is Building CurrentBuilding) { return "Total: " + string.Format("{0:C2}", CurrentBuilding.TotalItemValue()); }
                if (x is Room CurrentRoom) { return "Subtotal: " + string.Format("{0:C2}", CurrentRoom.TotalItemValue()); }
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return "Subtotal: " + string.Format("{0:C2}", CurrentContainer.TotalItemValue()); }
                if (x is Item CurrentItem) { return string.Format("{0:C2}", CurrentItem.Value * CurrentItem.Quantity); }
                return "";
            };

            // Create A List Of All The Object List View (OLV) Columns I Defined Above
            List<OLVColumn> ColumnList = new List<OLVColumn> { ColName, ColCount, ColUnitValue, ColTotalValue };

            // Add Each Column To The Tree List View
            foreach (OLVColumn CurrentColumn in ColumnList)
            {
                CurrentTreeListView.AllColumns.Add(CurrentColumn);
            }

            // This Is Needed Or The Columns Dont Appear
            CurrentTreeListView.RebuildColumns();

            // This Also Works Instead Of The Above Approach Of Adding Columns
            // TestView.Columns.AddRange(ColumnList.Cast<ColumnHeader>().ToArray());

            // This Determines Whether A Certain Row Can Be Expanded To Hide Or Display Its Children. The Rule Is Based On The Object Type
            CurrentTreeListView.CanExpandGetter = delegate (object x)
            {
                if (x is Building CurrentBuilding) { return CurrentBuilding.RoomList.Count > 0 || CurrentBuilding.StoredItems.Count > 0; }
                if (x is Room CurrentRoom) { return CurrentRoom.StoredItems.Count > 0; }
                if (x is BackEnd.ModelClasses.Container CurrentContainer) { return CurrentContainer.StoredItems.Count > 0; }
                if (x is Item CurrentItem) { return false; }
                return false;
            };

            // This Is An Important Portion Of The Code
            // This Defines What Objects Are Nested Under What Parents, Without This No Nesting Would Occur
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

            // Sets The Top Level Node That Populates The Rest Of The Tree List View
            CurrentTreeListView.Roots = new List<Building> { CurrentBuilding };

            // Defaults The Tree List View To Have All Expandable Rows Expanded
            CurrentTreeListView.ExpandAll();

            // When A Cell (Row/Column Intersection) Is Clicked, Select The Entire Row Instead Of The Cell
            CurrentTreeListView.FullRowSelect = true;
        }

        /// <summary>
        /// Refreshes The TreeListView To Reflect Updated Data
        /// </summary>
        private void RefreshTreeListView()
        {
            CurrentTreeListView.RebuildAll(true);
        }

        /// <summary>
        /// Resets The Split Panel Size Based On TreeListView Column Width
        /// </summary>
        private void ResetSplitPanelSize()
        {
            int CombinedColumnWidth = CurrentTreeListView.AllColumns.Sum(CurrentColumn => CurrentColumn.Width);
            // If I Have Time To Refactor, Have This Resize Account For If The Vertical Scrollbar Is Present
            // Reference What I Did In The Selection Control

            CombinedColumnWidth = Convert.ToInt32(CombinedColumnWidth * 1.05);
            splTopView.SplitterDistance = this.Width - CombinedColumnWidth;
        }


        // This Is Obsolete After Implementing The Tree List View, However I Still Want To Keep It In As A Reference
        /// <summary>
        /// Generates A Standard TreeView For Building Inventory (Legacy Implementation)
        /// </summary>
        private void GenerateTreeView()
        {
            System.Windows.Forms.TreeView CurrentTreeView = new System.Windows.Forms.TreeView();

            CurrentTreeView.Dock = DockStyle.Fill;

            splTopView.Panel2.Controls.Add(CurrentTreeView);

            CurrentTreeView.Nodes.Clear();

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

            CurrentTreeView.Nodes.Add(BuildingNode);
            CurrentTreeView.ExpandAll();
        }

        /// <summary>
        /// Sets Room Selection State And Updates Related Controls
        /// </summary>
        /// <param name="_SelectedRoom">The Selected Room</param>
        private void SetRoomControls(Room? _SelectedRoom)
        {
            SelectedRoom = _SelectedRoom;
            tsbtnEditRoom.Enabled = SelectedRoom != null;
            tsbtnDeleteRoom.Enabled = SelectedRoom != null;
        }

        /// <summary>
        /// Sets Item Selection State And Updates Related Controls
        /// </summary>
        /// <param name="_SelectedItem">The Selected Item</param>
        private void SetItemControls(Item? _SelectedItem)
        {
            SelectedItem = _SelectedItem;
            tsbtnEditItem.Enabled = SelectedItem != null;
            tsbtnMoveItem.Enabled = SelectedItem != null;
            tsbtnDeleteItem.Enabled = SelectedItem != null;
        }

        /// <summary>
        /// Handles Scale Button Click For Zooming In Or Out
        /// </summary>
        /// <param name="sender">The Button That Triggered The Event</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnScale_Click(object sender, EventArgs e)
        {
            //Shrinks Or Grows Building Size By 10% Based On Which Button Was Clicked
            if (sender == this.tsbtnScaleDown)
            {
                CurrentBufferedBuilding.ScaleBuilding(.9f);
            }
            else if (sender == this.tsbtnScaleUp)
            {
                CurrentBufferedBuilding.ScaleBuilding(1.1f);
            }
        }

        /// <summary>
        /// Handles Mouse Down Event To Start Continuous Scaling
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Mouse Event Arguments</param>
        private void tsbtnScale_MouseDown(object sender, MouseEventArgs e)
        {
            // Start The Timer When The Mouse Is Pressed Down On The Specified Button 
            ClickHoldTimer.Tag = sender as ToolStripButton;
            ClickHoldTimer.Start();
        }

        /// <summary>
        /// Handles Mouse Up Event To Stop Continuous Scaling
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Mouse Event Arguments</param>
        private void tsbtnScale_MouseUp(object sender, MouseEventArgs e)
        {
            // Stop The Timer When The Mouse Is Released On The Specified Button
            ClickHoldTimer.Tag = null;
            ClickHoldTimer.Stop();
        }

        /// <summary>
        /// Handles Mouse Leave Event To Stop Continuous Scaling
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnScale_MouseLeave(object sender, EventArgs e)
        {
            // Stop The Timer When The Mouse Leaves The Specified Button
            ClickHoldTimer.Tag = null;
            ClickHoldTimer.Stop();
        }

        /// <summary>
        /// Handles Timer Tick Event For Continuous Scaling
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void ClickHoldTimer_Tick(object sender, EventArgs e)
        {
            ToolStripButton CurrentButton = ClickHoldTimer.Tag as ToolStripButton;
            tsbtnScale_Click(CurrentButton, e);
        }

        /// <summary>
        /// Handles Fit To Screen Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnFitToScreen_Click(object sender, EventArgs e)
        {
            FitBuildingToScreen();
        }

        /// <summary>
        /// Handles Center Camera Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnCenter_Click(object sender, EventArgs e)
        {
            CenterCameraView();
        }

        /// <summary>
        /// Handles Add Room Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnAddRoom_Click(object sender, EventArgs e)
        {
            AddNewRoom();
        }

        /// <summary>
        /// Handles Edit Room Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnEditRoom_Click(object sender, EventArgs e)
        {
            ModifyRoom();
        }

        /// <summary>
        /// Handles Delete Room Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnDeleteRoom_Click(object sender, EventArgs e)
        {
            DeleteRoom();
        }

        /// <summary>
        /// Handles Add Item Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnAddItem_Click(object sender, EventArgs e)
        {
            AddNewItem();
        }

        /// <summary>
        /// Handles Edit Item Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnEditItem_Click(object sender, EventArgs e)
        {
            ModifyItem(true);
        }

        /// <summary>
        /// Handles Move Item Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnMoveItem_Click(object sender, EventArgs e)
        {
            ModifyItem(false);
        }

        /// <summary>
        /// Handles Delete Item Button Click
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsbtnDeleteItem_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        /// <summary>
        /// Handles RoomInfo Confirm Clicked Event
        /// </summary>
        /// <param name="_FormType">Form Operation Type (Add Or Modify)</param>
        /// <param name="_ModifiedRoom">Current Room (When Being Modified)</param>
        /// <param name="_CurrentControl">The Current Control That Sent The Event</param>
        /// <param name="_RoomValues">New Proposed Room Values</param>
        private void RoomInfo_ConfirmClicked(FormType _FormType, Room? _ModifiedRoom, RoomInfo _CurrentControl, (string Name, float Width, float Height, float CenterX, float CenterY, int ColorValue) _RoomValues)
        {
            string? _ErrorMessage;

            // BackEnd Call Based On The Form Operation Type
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

        /// <summary>
        /// Handles RoomInfo Cancel Clicked Event
        /// </summary>
        /// <param name="_CurrentControl">The Current Control That Sent The Event</param>
        private void RoomInfo_CancelClicked(RoomInfo _CurrentControl)
        {
            // Unwire RoomInfo Control Events 
            _CurrentControl.ConfirmClicked -= RoomInfo_ConfirmClicked;
            _CurrentControl.CancelClicked -= RoomInfo_CancelClicked;

            CurrentBufferedBuilding.ResetSelectedRoom();

            // Reenable ToolStrip 
            tsrTopDown.Enabled = true;

            // Reenable Top-Down View By Removing Invisible Blocker Panel 
            TransparentPanel BlockerPanel = this.CameraPanel.Controls["Blocker"] as TransparentPanel;
            this.CameraPanel.Controls.Remove(BlockerPanel);
            BlockerPanel.Dispose();

            // Set The Right Hand Panel To Fit The Tree List View
            ResetSplitPanelSize();

            // Clean Up RoomInfo Control
            splTopView.Panel2.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        /// <summary>
        /// Handles ItemInfo Confirm Clicked Event
        /// </summary>
        /// <param name="_FormType">Form Operation Type (Add Or Modify)</param>
        /// <param name="_ModifyOrMove">Determines If The Item Modification Is Modify Or Move</param>
        /// <param name="_ModifiedItem">Current Item (When Being Modified)</param>
        /// <param name="_CurrentControl">The Current Control That Sent The Event</param>
        /// <param name="_ItemValues">New Proposed Item Values</param>
        private void ItemInfo_ConfirmClicked(FormType _FormType, bool _ModifyOrMove, Item? _ModifiedItem, ItemInfo _CurrentControl, (string Name, string Description, double Value, int Quantity, IStorageHolder Location, BackEnd.Enumerations.StoredItemType CreationType) _ItemValues)
        {
            string? _ErrorMessage;

            // BackEnd Call Based On The Form Operation Type
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
            {
                // BackEnd Call Based On The Move Or Modify Type
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

        /// <summary>
        /// Handles ItemInfo Cancel Clicked Event
        /// </summary>
        /// <param name="_CurrentControl">The Current Control That Sent The Event</param>
        private void ItemInfo_CancelClicked(ItemInfo _CurrentControl)
        {
            // Unwire RoomInfo Control Events 
            _CurrentControl.ConfirmClicked -= ItemInfo_ConfirmClicked;
            _CurrentControl.CancelClicked -= ItemInfo_CancelClicked;

            CurrentBufferedBuilding.ResetSelectedRoom();

            // Reenable ToolStrip 
            tsrTopDown.Enabled = true;

            // Reenable Top-Down View By Removing Invisible Blocker Panel 
            TransparentPanel BlockerPanel = this.CameraPanel.Controls["Blocker"] as TransparentPanel;
            this.CameraPanel.Controls.Remove(BlockerPanel);
            BlockerPanel.Dispose();

            // Set The Right Hand Panel To Fit The Tree List View
            ResetSplitPanelSize();

            // Clean Up RoomInfo Control
            splTopView.Panel2.Controls.Remove(_CurrentControl);
            _CurrentControl.Dispose();
        }

        /// <summary>
        /// Handles Horizontal Grid Count Value Changed Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsnudHGridCount_ValueChanged(object sender, EventArgs e)
        {
            CurrentBufferedBuilding.HGridCount = Convert.ToInt32(tsnudHGridCount.Value);

            //Alternate Approach Using The Sender Instead
            //CurrentBufferedBuilding.HGridCount = Convert.ToInt32((sender as ToolStripNumericUpDown).Value);
        }

        /// <summary>
        /// Handles Vertical Grid Count Value Changed Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void tsnudVGridCount_ValueChanged(object sender, EventArgs e)
        {
            CurrentBufferedBuilding.VGridCount = Convert.ToInt32(tsnudVGridCount.Value);

            //Alternate Approach Using The Sender Instead
            //CurrentBufferedBuilding.VGridCount = Convert.ToInt32((sender as ToolStripNumericUpDown).Value);
        }

        /// <summary>
        /// Handles Room Selection Changed Event From Buffered Building
        /// </summary>
        /// <param name="_SelectedRoom">The Newly Selected Room</param>
        private void CurrentBufferedBuilding_RoomSelectionChanged(Room? _SelectedRoom)
        {
            SetRoomControls(_SelectedRoom);
            SetItemControls(null);
        }

        /// <summary>
        /// Handles Stored Items Changed Event From Building
        /// </summary>
        private void CurrentBuilding_StoredItemsChanged()
        {
            RefreshTreeListView();
        }

        /// <summary>
        /// Handles Room List Changed Event From Building
        /// </summary>
        private void CurrentBuilding_RoomListChanged()
        {
            RefreshTreeListView();
        }

        /// <summary>
        /// Handles Width TextBox Key Down Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Key Event Arguments</param>
        private void tstxtWidth_KeyDown(object sender, KeyEventArgs e)
        {
            // Only Update Values If The User Pressed The Enter Key
            if (e.KeyCode == Keys.Enter)
            {
                // Prevents The Enter Key From Being Sent On Past This Function
                e.SuppressKeyPress = true;

                string? ErrorMessage;
                ToolStripTextBox CurrentTextBox = sender as ToolStripTextBox;

                try
                {
                    if
                    (
                        // Attempt To Modify The BackEnd Building To Be The Value Set In The Text Box
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

        /// <summary>
        /// Handles Height TextBox Key Down Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tstxtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            // Only Update Values If The User Pressed The Enter Key
            if (e.KeyCode == Keys.Enter)
            {
                // Prevents The Enter Key From Being Sent On Past This Function
                e.SuppressKeyPress = true;

                string? ErrorMessage;
                ToolStripTextBox CurrentTextBox = sender as ToolStripTextBox;

                try
                {
                    if
                    (
                        // Attempt To Modify The BackEnd Building To Be The Value Set In The Text Box
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

        /// <summary>
        /// Handles TreeListView Selection Changed Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void CurrentTreeListView_SelectionChanged(object? sender, EventArgs e)
        {
            object? SelectedTreeListViewObject;

            //If The User Clicked On A Row In The Tree List View, Update The UI Based On What The User Clicked On
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
