using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;
using System.Text.Json.Serialization;

namespace BackEnd.ModelClasses
{
    /// <summary>
    /// Represents A Building That Contains Rooms And Storage,
    /// And Provides Validation And Event Notification For Changes
    /// </summary>
    public class Building : IStorageHolder
    {
        /// <summary>
        /// Event Triggered When The Room List Changes
        /// </summary>
        public event Action? RoomListChanged;

        /// <summary>
        /// Event Triggered When The Building Name Changes
        /// </summary>
        public event Action? BuildingNameChanged;

        /// <summary>
        /// Event Triggered When The Building Dimensions Change
        /// </summary>
        public event Action? BuildingDimensionsChanged;

        /// <summary>
        /// Internal Event Used To Track Stored Items Change
        /// </summary>
        private event Action? __StoredItemsChanged;

        /// <summary>
        /// Pass Through Event For When Stored Items Change
        /// </summary>
        public event Action? StoredItemsChanged
        {
            add
            {
                UnsortedItems.StoredItemsChanged += value;
                __StoredItemsChanged += value;
            }
            remove
            {
                UnsortedItems.StoredItemsChanged -= value;
                __StoredItemsChanged -= value;
            }
        }
        /// <summary>
        /// Event Triggered When A Stored Item Is Modified
        /// </summary>
        private event Action? __StoredItemModified;

        /// <summary>
        /// Pass Through Event For When Stored Item Is Modified
        /// </summary>
        public event Action? StoredItemModified
        {
            add
            {
                UnsortedItems.StoredItemModified += value;
                __StoredItemModified += value;
            }
            remove
            {
                UnsortedItems.StoredItemModified -= value;
                __StoredItemModified -= value;
            }
        }

        /// <summary>
        /// Internal Event Used To Track Room Name Changes
        /// </summary>
        private event Action? __RoomNameChanged;

        /// <summary>
        /// Pass Through Event For When A Room Name Changes
        /// </summary>
        public event Action? RoomNameChanged
        {
            add
            {
                __RoomNameChanged += value;
            }
            remove
            {
                __RoomNameChanged -= value;
            }
        }

        /// <summary>
        /// Internal Event Used To Track Room Dimension Changes
        /// </summary>
        private event Action? __RoomDimensionsChanged;

        /// <summary>
        /// Pass Through Event For When A Room Dimension Changes
        /// </summary>
        public event Action? RoomDimensionsChanged
        {
            add
            {
                __RoomDimensionsChanged += value;
            }
            remove
            {
                __RoomDimensionsChanged -= value;
            }
        }

        /// <summary>
        /// Internal Event Used To Track Room Color Changes
        /// </summary>
        private event Action? __RoomColorChanged;

        /// <summary>
        /// Pass Through Event For When A Room Color Changes
        /// </summary>
        public event Action? RoomColorChanged
        {
            add
            {
                __RoomColorChanged += value;
            }
            remove
            {
                __RoomColorChanged -= value;
            }
        }

        /// <summary>
        /// The Name Of The Building
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The Width Of The Building
        /// </summary>
        public float Width { get; private set; }

        /// <summary>
        /// The Height Of The Building
        /// </summary>
        public float Height { get; private set; }

        /// <summary>
        /// The Storage Backer For Items Contained Directly Within The Building
        /// </summary>
        private Storage UnsortedItems;

        /// <summary>
        /// The Storage For Items Contained Directly Within The Building
        /// </summary>
        public IStorage CurrentStorage
        {
            get
            { return UnsortedItems; }
        }

        /// <summary>
        /// The List Of Items Directly Stored In The Building
        /// </summary>
        [JsonIgnore] // Excludes Stored Items From JSON To Prevent Repeat References During Serialization
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return UnsortedItems.StoredItems; }
        }

        /// <summary>
        /// Internal List Of Rooms In The Building
        /// </summary>
        private List<Room> __RoomList = new List<Room>();

        /// <summary>
        /// The Read-Only List Of Rooms In The Building
        /// </summary>
        public IReadOnlyList<Room> RoomList
        {
            get
            { return __RoomList.AsReadOnly(); }
        }


        /// <summary>
        /// Private Constructor Used For Controlled Creation Of Building Objects
        /// </summary>
        /// <param name="_Name">The Name Of The Building</param>
        /// <param name="_Width">The Width Of The Building</param>
        /// <param name="_Height">The Height Of The Building</param>
        private Building(string _Name, float _Width, float _Height)
        {
            this.Name = _Name;
            this.Width = _Width;
            this.Height = _Height;

            UnsortedItems = new Storage(this);
        }

        /// <summary>
        /// Attempts To Create A New Building With Validation.
        /// Only Available Source To Create A Building Instance
        /// </summary>
        /// <param name="_BuildingName">The Proposed Name Of The Building</param>
        /// <param name="_Width">The Proposed Width Of The Building</param>
        /// <param name="_Height">The Proposed Height Of The Building</param>
        /// <param name="_CreatedBuilding">The Building Instance Created If Successful</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal static bool TryCreate(string _BuildingName, float _Width, float _Height, out Building? _CreatedBuilding, out string? _ErrorMessage)
        {
            _CreatedBuilding = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            // Runs Building Name Self-Validation
            if (!NameSelfValidation(_BuildingName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Building Dimension Self-Validation
            if (!SizeSelfValidation(_Width, _Height, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Creates New Building Instance If Building Passes Self-Validation Checks
            if (CreationSuccess)
            {
                _CreatedBuilding = new Building(_BuildingName, _Width, _Height);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        /// <summary>
        /// Attempts To Modify Building Properties With Validation
        /// </summary>
        /// <param name="_NewBuildingName">The Proposed Name Of The Building</param>
        /// <param name="_NewWidth">The Proposed Width Of The Building</param>
        /// <param name="_NewHeight">The Proposed Height Of The Building</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal bool TryModify(string _NewBuildingName, float _NewWidth, float _NewHeight, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            bool NameChanged = this.Name != _NewBuildingName;
            bool DimensionsChanged = this.Width != _NewWidth || this.Height != _NewHeight;

            // Checks If Any Fields Have Been Modified
            if (NameChanged || DimensionsChanged)
            {
                // Runs Building Name Self-Validation If Name Was Changed
                if (NameChanged)
                {
                    if (!NameSelfValidation(_NewBuildingName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                // Runs Building Dimension Self-Validation If Dimension Was Changed
                if (DimensionsChanged)
                {
                    if (!SizeSelfValidation(_NewWidth, _NewHeight, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }

                    if (!SizeSystemValidation(_NewWidth, _NewHeight, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }
            }
            else
            {
                ModifySuccess = false;
                _ErrorMessage += $"No Building Fields Have Been Modified\n";
            }

            // Modifies Building Fields If Building Passes Self-Validation Checks
            if (ModifySuccess)
            {
                this.Name = _NewBuildingName;
                this.Width = _NewWidth;
                this.Height = _NewHeight;

                if (NameChanged)
                {
                    this.BuildingNameChanged?.Invoke();
                }

                if (DimensionsChanged)
                {
                    this.BuildingDimensionsChanged?.Invoke();
                }
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifySuccess;
        }

        /// <summary>
        /// Validates Building Name Self Requirements
        /// </summary>
        /// <param name="_BuildingName">The Proposed Name To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        private static bool NameSelfValidation(string _BuildingName, ref string? _ErrorMessage)
        {
            bool BuildingNameValid = true;

            // Validates The Name Entered Was Not Empty
            if (string.IsNullOrEmpty(_BuildingName))
            {
                _ErrorMessage += "Self Validation Error: Building Name Must Contain Characters\n";
                BuildingNameValid = false;
            }

            return BuildingNameValid;
        }

        /// <summary>
        /// Validates Building Dimension Self Requirements
        /// </summary>
        /// <param name="_Width">The Proposed Width To Validate</param>
        /// <param name="_Height">The Proposed Height To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        private static bool SizeSelfValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool BuildingSizeValid = true;

            // Validates The Dimensions Are Positive Numbers
            if (_Width <= 0 || _Height <= 0)
            {
                _ErrorMessage += "Self Validation Error: Width And Height Dimensions Must Be Positive Numbers\n";
                BuildingSizeValid = false;
            }

            return BuildingSizeValid;
        }

        /// <summary>
        /// Validates Building Dimension System Requirements
        /// </summary>
        /// <param name="_Width">The Proposed Width To Validate</param>
        /// <param name="_Height">The Proposed Height To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        private bool SizeSystemValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool BuildingSizeValid = true;

            // Validates That The Boundaries Of All The Rooms Are Within the Boundaries Of The Building
            foreach (Room CurrentRoom in this.__RoomList)
            {
                float CurrentRoomRightLocation = CurrentRoom.CenterX + CurrentRoom.Width / 2;
                float CurrentRoomBottomLocation = CurrentRoom.CenterY + CurrentRoom.Height / 2;

                if (_Width < CurrentRoomRightLocation || _Height < CurrentRoomBottomLocation)
                {
                    _ErrorMessage += "System Validation Error: Width And Height Dimensions Must Exceed All Room Boundaries\n";
                    BuildingSizeValid = false;
                    break;
                }
            }

            return BuildingSizeValid;
        }

        /// <summary>
        /// Attempts To Add A Room To The Building With Validation
        /// </summary>
        /// <param name="_RoomName">The Proposed Name Of The Room</param>
        /// <param name="_Width">The Proposed Width Of The Room</param>
        /// <param name="_Height">The Proposed Height Of The Room</param>
        /// <param name="_CenterX">The Proposed Center X Coordinate Of The Room</param>
        /// <param name="_CenterY">The Proposed Center Y Coordinate Of The Room</param>
        /// <param name="_RoomColor">The Proposed Color Of The Room</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryAddRoom(string _RoomName, float _Width, float _Height, float _CenterX, float _CenterY, int _RoomColor, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddRoomSuccess = true;

            // Runs Room Name System-Validation
            if (!RoomNameSystemValidation(_RoomName, ref _ErrorMessage))
            {
                AddRoomSuccess = false;
            }

            // Runs Room Dimension System-Validation
            if (!RoomDimensionValidation(_Width, _Height, _CenterX, _CenterY, ref _ErrorMessage))
            {
                AddRoomSuccess = false;
            }

            // If Room Passes System-Validation Attempt To Create The Room
            if (AddRoomSuccess)
            {
                Room? NewRoom;

                if (Room.TryCreate(_RoomName, _Width, _Height, _CenterX, _CenterY, _RoomColor, out NewRoom, out _ErrorMessage))
                {
                    __RoomList.Add(NewRoom);
                    NewRoom.StoredItemsChanged += Room_StoredItemsChanged;
                    NewRoom.StoredItemModified += Room_StoredItemModified;
                    NewRoom.RoomNameChanged += Room_RoomNameChanged;
                    NewRoom.RoomDimensionsChanged += Room_RoomDimensionsChanged;
                    NewRoom.RoomColorChanged += Room_RoomColorChanged;
                    RoomListChanged?.Invoke();
                }
                else
                {
                    AddRoomSuccess = false;
                }
            }

            if (!AddRoomSuccess)
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return AddRoomSuccess;
        }

        /// <summary>
        /// Attempts To Modify A Room With Validation
        /// </summary>
        /// <param name="_RoomToModify">The Room Instance To Attempt To Modify</param>
        /// <param name="_NewRoomName">The Proposed Name Of The Room</param>
        /// <param name="_NewWidth">The Proposed Width Of The Room</param>
        /// <param name="_NewHeight">The Proposed Height Of The Room</param>
        /// <param name="_NewCenterX">The Proposed Center X Coordinate Of The Room</param>
        /// <param name="_NewCenterY">The Proposed Center Y Coordinate Of The Room</param>
        /// <param name="_NewRoomColor">The Proposed Color Of The Room</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryModifyRoom(Room _RoomToModify, string _NewRoomName, float _NewWidth, float _NewHeight, float _NewCenterX, float _NewCenterY, int _NewRoomColor, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyRoomSuccess = true;

            bool NameChanged = _RoomToModify.Name != _NewRoomName;
            bool DimensionsChanged = _RoomToModify.Width != _NewWidth || _RoomToModify.Height != _NewHeight || _RoomToModify.CenterX != _NewCenterX || _RoomToModify.CenterY != _NewCenterY;
            bool ColorChanged = _RoomToModify.RoomColor != _NewRoomColor;

            // Checks If Any Fields Have Been Modified
            if (NameChanged || DimensionsChanged || ColorChanged)
            {
                // Runs Room Name System-Validation If Name Was Changed
                if (NameChanged)
                {
                    if (!RoomNameSystemValidation(_NewRoomName, ref _ErrorMessage))
                    {
                        ModifyRoomSuccess = false;
                    }
                }

                // Runs Room Dimension System-Validation If Dimension Was Changed
                if (DimensionsChanged)
                {
                    if (!RoomDimensionValidation(_NewWidth, _NewHeight, _NewCenterX, _NewCenterY, ref _ErrorMessage, _RoomToModify))
                    {
                        ModifyRoomSuccess = false;
                    }
                }

                // If Room Passes System-Validation Attempt To Modify The Room
                if (ModifyRoomSuccess)
                {
                    if (!_RoomToModify.TryModify(_NewRoomName, _NewWidth, _NewHeight, _NewCenterX, _NewCenterY, _NewRoomColor, out _ErrorMessage))
                    {
                        ModifyRoomSuccess = false;
                    }
                }
            }
            else
            {
                ModifyRoomSuccess = false;
                _ErrorMessage += $"No Room Fields Have Been Modified\n";
            }

            if (!ModifyRoomSuccess)
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifyRoomSuccess;
        }

        /// <summary>
        /// Attempts To Remove A Room From The Building
        /// </summary>
        /// <param name="_RoomToRemove">The Room Instance To Attempt To Remove</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryRemoveRoom(Room _RoomToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            // Validates The Room To Remove Exists Within The Building
            if (!__RoomList.Contains(_RoomToRemove))
            {
                _ErrorMessage = "Room To Be Removed Must Exist In The Room List";
                return false;
            }

            // If Room Passes Validation Remove Room And Perform Clean Up
            _RoomToRemove.StoredItemsChanged -= Room_StoredItemsChanged;
            _RoomToRemove.StoredItemModified -= Room_StoredItemModified;
            _RoomToRemove.RoomNameChanged -= Room_RoomNameChanged;
            _RoomToRemove.RoomDimensionsChanged -= Room_RoomDimensionsChanged;
            _RoomToRemove.RoomColorChanged -= Room_RoomColorChanged;

            __RoomList.Remove(_RoomToRemove);
            RoomListChanged?.Invoke();

            if (_RoomToRemove.StoredItems.Count > 0)
            {
                __StoredItemsChanged?.Invoke();
            }
            return true;
        }

        /// <summary>
        /// Validates Room Name System Requirements
        /// </summary>
        /// <param name="_RoomName">The Proposed Name To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        private bool RoomNameSystemValidation(string _RoomName, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            // Validates The Name Entered Is Not A Duplicate
            if (__RoomList.Any(CurrentRoom => CurrentRoom.Name == _RoomName))
            {
                _ErrorMessage += $"System Validation Error: Two Rooms Cannot Have The Same Name. {_RoomName} already exists.\n";
                SystemValid = false;
            }

            return SystemValid;
        }

        /// <summary>
        /// Validates Room Dimension System Requirements
        /// </summary>
        /// <param name="_Width">The Proposed Width To Validate</param>
        /// <param name="_Height">The Proposed Height To Validate</param>
        /// <param name="_CenterX">The Proposed Center X Coordinate To Validate</param>
        /// <param name="_CenterY">The Proposed Center Y Coordinate To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <param name="_RoomToExclude">The Room To Exclude From The System Validation, Use The Current Room When Modifying A Room</param>
        /// <returns></returns>
        private bool RoomDimensionValidation(float _Width, float _Height, float _CenterX, float _CenterY, ref string? _ErrorMessage, Room? _RoomToExclude = null)
        {
            bool SystemValid = true;

            float NewRoomLeftLocation = _CenterX - _Width / 2;
            float NewRoomRightLocation = _CenterX + _Width / 2;
            float NewRoomTopLocation = _CenterY - _Height / 2;
            float NewRoomBottomLocation = _CenterY + _Height / 2;

            // Validates That Room Center Is In Building
            if (_CenterX < 0 || _CenterY < 0 || _CenterX > this.Width || _CenterY > this.Height)
            {
                _ErrorMessage += $"System Validation Error: Room Center ({_CenterX},{_CenterY}) Is Outside Building Limits. Must Be Between (0,0) and ({this.Width},{this.Height})\n";
                SystemValid = false;
            }

            // Validates That Room Left Is In Building
            if (NewRoomLeftLocation < 0)
            {
                _ErrorMessage += $"System Validation Error: Room Left Boundary ({NewRoomLeftLocation}) Is Outside Building Limits. Must Be Greater Than 0.\n";
                SystemValid = false;
            }

            // Validates That Room Right Is In Building
            if (NewRoomRightLocation > this.Width)
            {
                _ErrorMessage += $"System Validation Error: Room Right Boundary ({NewRoomRightLocation}) Is Outside Building Limits. Must Be Less Than {this.Width}.\n";
                SystemValid = false;
            }

            // Validates That Room Top Is In Building
            if (NewRoomTopLocation < 0)
            {
                _ErrorMessage += $"System Validation Error: Room Top Boundary ({NewRoomTopLocation}) Is Outside Building Limits. Must Be Greater Than 0.\n";
                SystemValid = false;
            }

            // Validates That Room Bottom Is In Building
            if (NewRoomBottomLocation > this.Height)
            {
                _ErrorMessage += $"System Validation Error: Room Bottom Boundary ({NewRoomBottomLocation}) Is Outside Building Limits. Must Be Less Than {this.Height}.\n";
                SystemValid = false;
            }

            if (SystemValid)
            {
                // Validates That Room Does Not Collide With Any Other Rooms
                // The .Where LINQ Method Is Used To Exlude The Current Room, When This Method Is Called To Verify A Modify Operation On An Existing Room.
                foreach (Room CurrentRoom in __RoomList.Where(CurrentRoom => CurrentRoom != _RoomToExclude))
                {
                    float CurrentRoomLeftLocation = CurrentRoom.CenterX - CurrentRoom.Width / 2;
                    float CurrentRoomRightLocation = CurrentRoom.CenterX + CurrentRoom.Width / 2;
                    float CurrentRoomTopLocation = CurrentRoom.CenterY - CurrentRoom.Height / 2;
                    float CurrentRoomBottomLocation = CurrentRoom.CenterY + CurrentRoom.Height / 2;

                    if (!
                            (
                            NewRoomLeftLocation >= CurrentRoomRightLocation ||
                            NewRoomRightLocation <= CurrentRoomLeftLocation ||
                            NewRoomTopLocation >= CurrentRoomBottomLocation ||
                            NewRoomBottomLocation <= CurrentRoomTopLocation
                            )
                        )
                    {
                        _ErrorMessage += $"System Validation Error: Room Collides With {CurrentRoom.Name}\n";
                        SystemValid = false;
                    }
                }
            }

            return SystemValid;
        }

        /// <summary>
        /// Attempts To Add A Stored Item To The Building Storage
        /// </summary>
        /// <param name="_StoredType">The Type Of The Proposed IStored</param>
        /// <param name="_StoredName">The Proposed Name Of The IStored</param>
        /// <param name="_Description">The Proposed Description Of The IStored</param>
        /// <param name="_Value">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_Quantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            return UnsortedItems.TryAddIStored(_StoredType, _StoredName, _Description, _Value, _Quantity, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Modify A Stored Item In The Building Storage
        /// </summary>
        /// <param name="_IStoredToModify">The IStored Instance To Attempt To Modify</param>
        /// <param name="_NewStoredName">The Proposed Name Of The IStored</param>
        /// <param name="_NewDescription">The Proposed Description Of The IStored</param>
        /// <param name="_NewValue">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_NewQuantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            return UnsortedItems.TryModifyIStored(_IStoredToModify, _NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Move A Stored Item From The Current Storage To Another Storage
        /// </summary>
        /// <param name="_ItemToMove">The IStored Instance To Attempt To Move</param>
        /// <param name="_Destination">The Proposed Destination Storage Holder</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage)
        {
            return UnsortedItems.TryMoveIStored(_ItemToMove, _Destination, out _ErrorMessage);
        }

        /// <summary>
        ///Attempts To Remove A Stored Item From The Building Storage
        /// </summary>
        /// <param name="_StoredToRemove">The IStored Instance To Attempt To Remove</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage)
        {
            return UnsortedItems.TryRemoveIStored(_StoredToRemove, out _ErrorMessage);
        }

        /// <summary>
        /// Calculates The Total Number Of Items In The Building And All Rooms
        /// </summary>
        public int TotalItemCount()
        {
            return UnsortedItems.TotalItemCount() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemCount());
        }

        /// <summary>
        /// Calculates The Total Value Of Items In The Building And All Rooms
        /// </summary>
        public double TotalItemValue()
        {
            return UnsortedItems.TotalItemValue() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemValue());
        }

        /// <summary>
        /// Propagates Stored Item Change Events From Rooms
        /// </summary>
        private void Room_StoredItemsChanged()
        {
            __StoredItemsChanged?.Invoke();
        }

        /// <summary>
        /// Propagates Stored Item Modification Events From Rooms
        /// </summary>
        private void Room_StoredItemModified()
        {
            __StoredItemModified?.Invoke();
        }

        /// <summary>
        /// Propagates Room Name Change Events
        /// </summary>
        private void Room_RoomNameChanged()
        {
            __RoomNameChanged?.Invoke();
        }

        /// <summary>
        /// Propagates Room Dimension Change Events
        /// </summary>
        private void Room_RoomDimensionsChanged()
        {
            __RoomDimensionsChanged?.Invoke();
        }

        /// <summary>
        /// Propagates Room Color Change Events
        /// </summary>
        private void Room_RoomColorChanged()
        {
            __RoomColorChanged?.Invoke();
        }
    }
}