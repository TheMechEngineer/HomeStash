using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Building : IStorageHolder
    {
        public event Action? RoomListChanged;
        public event Action? BuildingNameChanged;
        public event Action? BuildingDimensionsChanged;

        private event Action? __StoredItemsChanged;
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

        private event Action? __StoredItemModified;
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
                __StoredItemModified += value;
            }
        }

        private event Action? __RoomNameChanged;
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

        private event Action? __RoomDimensionsChanged;
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

        private event Action? __RoomColorChanged;
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

        public string Name { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        private Storage UnsortedItems;

        public IStorage CurrentStorage
        {
            get
            { return UnsortedItems; }
        }

        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return UnsortedItems.StoredItems; }
        }

        private List<Room> __RoomList = new List<Room>();
        public IReadOnlyList<Room> RoomList
        {
            get
            { return __RoomList.AsReadOnly(); }
        }

        private Building(string _Name, float _Width, float _Height)
        {
            this.Name = _Name;
            this.Width = _Width;
            this.Height = _Height;

            UnsortedItems = new Storage(this);
        }

        internal static bool TryCreate(string _BuildingName, float _Width, float _Height, out Building? _CreatedBuilding, out string? _ErrorMessage)
        {
            _CreatedBuilding = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (!NameSelfValidation(_BuildingName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!SizeSelfValidation(_Width, _Height, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

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

        internal bool TryModify(string _NewBuildingName, float _NewWidth, float _NewHeight, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            bool NameChanged = this.Name != _NewBuildingName;
            bool DimensionsChanged = this.Width != _NewWidth || this.Height != _NewHeight;

            if (NameChanged || DimensionsChanged)
            {
                if (NameChanged)
                {
                    if (!NameSelfValidation(_NewBuildingName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

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

        private static bool NameSelfValidation(string _BuildingName, ref string? _ErrorMessage)
        {
            bool BuildingNameValid = true;

            if (string.IsNullOrEmpty(_BuildingName))
            {
                _ErrorMessage += "Self Validation Error: Building Name Must Contain Characters\n";
                BuildingNameValid = false;
            }

            return BuildingNameValid;
        }

        private static bool SizeSelfValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool BuildingSizeValid = true;

            if (_Width <= 0 || _Height <= 0)
            {
                _ErrorMessage += "Self Validation Error: Width And Height Dimensions Must Be Positive Numbers\n";
                BuildingSizeValid = false;
            }

            return BuildingSizeValid;
        }

        private bool SizeSystemValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool BuildingSizeValid = true;

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

        public bool TryAddRoom(string _RoomName, float _Width, float _Height, float _CenterX, float _CenterY, int _RoomColor, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddRoomSuccess = true;

            if (!RoomNameSystemValidation(_RoomName, ref _ErrorMessage))
            {
                AddRoomSuccess = false;
            }

            if (!RoomDimensionValidation(_Width, _Height, _CenterX, _CenterY, ref _ErrorMessage))
            {
                AddRoomSuccess = false;
            }

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

        public bool TryModifyRoom(Room _RoomToModify, string _NewRoomName, float _NewWidth, float _NewHeight, float _NewCenterX, float _NewCenterY, int _NewRoomColor, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyRoomSuccess = true;

            bool NameChanged = _RoomToModify.Name != _NewRoomName;
            bool DimensionsChanged = _RoomToModify.Width != _NewWidth || _RoomToModify.Height != _NewHeight || _RoomToModify.CenterX != _NewCenterX || _RoomToModify.CenterY != _NewCenterY;
            bool ColorChanged = _RoomToModify.RoomColor != _NewRoomColor;

            if (NameChanged || DimensionsChanged || ColorChanged)
            {
                if (NameChanged)
                {
                    if (!RoomNameSystemValidation(_NewRoomName, ref _ErrorMessage))
                    {
                        ModifyRoomSuccess = false;
                    }
                }

                if (DimensionsChanged)
                {
                    if (!RoomDimensionValidation(_NewWidth, _NewHeight, _NewCenterX, _NewCenterY, ref _ErrorMessage, _RoomToModify))
                    {
                        ModifyRoomSuccess = false;
                    }
                }

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

        public bool TryRemoveRoom(Room _RoomToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (!__RoomList.Contains(_RoomToRemove))
            {
                _ErrorMessage = "Room To Be Removed Must Exist In The Room List";
                return false;
            }

            _RoomToRemove.StoredItemsChanged -= Room_StoredItemsChanged;
            _RoomToRemove.StoredItemModified -= Room_StoredItemModified;
            _RoomToRemove.RoomNameChanged -= Room_RoomNameChanged;
            _RoomToRemove.RoomDimensionsChanged -= Room_RoomDimensionsChanged;
            _RoomToRemove.RoomColorChanged -= Room_RoomColorChanged;

            __RoomList.Remove(_RoomToRemove);
            RoomListChanged?.Invoke();

            if(_RoomToRemove.StoredItems.Count > 0)
            {
                __StoredItemsChanged?.Invoke();
            }
            return true;
        }

        private bool RoomNameSystemValidation(string _RoomName, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            if (__RoomList.Any(CurrentRoom => CurrentRoom.Name == _RoomName))
            {
                _ErrorMessage += $"System Validation Error: Two Rooms Cannot Have The Same Name. {_RoomName} already exists.\n";
                SystemValid = false;
            }

            return SystemValid;
        }

        private bool RoomDimensionValidation(float _Width, float _Height, float _CenterX, float _CenterY, ref string? _ErrorMessage, Room? _RoomToExclude = null)
        {
            bool SystemValid = true;

            float NewRoomLeftLocation = _CenterX - _Width / 2;
            float NewRoomRightLocation = _CenterX + _Width / 2;
            float NewRoomTopLocation = _CenterY - _Height / 2;
            float NewRoomBottomLocation = _CenterY + _Height / 2;

            //Check That Room Center Is In Building
            if (_CenterX < 0 || _CenterY < 0 || _CenterX > this.Width || _CenterY > this.Height)
            {
                _ErrorMessage += $"System Validation Error: Room Center ({_CenterX},{_CenterY}) Is Outside Building Limits. Must Be Between (0,0) and ({this.Width},{this.Height})\n";
                SystemValid = false;
            }

            //Check That Room Left Is In Building
            if (NewRoomLeftLocation < 0)
            {
                _ErrorMessage += $"System Validation Error: Room Left Boundary ({NewRoomLeftLocation}) Is Outside Building Limits. Must Be Greater Than 0.\n";
                SystemValid = false;
            }

            //Check That Room Right Is In Building
            if (NewRoomRightLocation > this.Width)
            {
                _ErrorMessage += $"System Validation Error: Room Right Boundary ({NewRoomRightLocation}) Is Outside Building Limits. Must Be Less Than {this.Width}.\n";
                SystemValid = false;
            }

            //Check That Room Top Is In Building
            if (NewRoomTopLocation < 0)
            {
                _ErrorMessage += $"System Validation Error: Room Top Boundary ({NewRoomTopLocation}) Is Outside Building Limits. Must Be Greater Than 0.\n";
                SystemValid = false;
            }

            //Check That Room Bottom Is In Building
            if (NewRoomBottomLocation > this.Height)
            {
                _ErrorMessage += $"System Validation Error: Room Bottom Boundary ({NewRoomBottomLocation}) Is Outside Building Limits. Must Be Less Than {this.Height}.\n";
                SystemValid = false;
            }

            if (SystemValid)
            {
                //The .Where LINQ method is used to exlude the current room, when this method is called to verify on modify instead of add.
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

        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
           return UnsortedItems.TryAddIStored(_StoredType, _StoredName, _Description, _Value, _Quantity, out _ErrorMessage);
        }

        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            return UnsortedItems.TryModifyIStored(_IStoredToModify, _NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
        }

        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage)
        {
            return UnsortedItems.TryRemoveIStored(_StoredToRemove, out _ErrorMessage);
        }

        public bool TryMoveIStored(IStored _ItemToMove, IStorage _Destination, out string? _ErrorMessage)
        {
            return UnsortedItems.TryMoveIStored(_ItemToMove, _Destination, out _ErrorMessage);
        }

        public int TotalItemCount()
        {
            return UnsortedItems.TotalItemCount() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemCount());
        }

        public double TotalItemValue()
        {
            return UnsortedItems.TotalItemValue() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemValue());
        }

        private void Room_StoredItemsChanged()
        {
            __StoredItemsChanged?.Invoke();
        }

        private void Room_StoredItemModified()
        {
            __StoredItemModified?.Invoke();
        }

        private void Room_RoomNameChanged()
        {
            __RoomNameChanged?.Invoke();
        }

        private void Room_RoomDimensionsChanged()
        {
            __RoomDimensionsChanged?.Invoke();
        }

        private void Room_RoomColorChanged()
        {
            __RoomColorChanged?.Invoke();
        }
    }
}
