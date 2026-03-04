using BackEnd.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Building : IStorageHolder
    {
        public event Action? RoomListChanged;

        public string Name { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        private Storage UnsortedItems = new Storage();

        public IStorage Storage
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

            if (this.Name != _NewBuildingName || this.Width != _NewWidth || this.Height != _NewHeight)
            {
                if (this.Name != _NewBuildingName)
                {
                    if (!NameSelfValidation(_NewBuildingName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                if (this.Width != _NewWidth || this.Height != _NewHeight)
                {
                    if (!SizeSelfValidation(_NewWidth, _NewHeight, ref _ErrorMessage))
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
                _ErrorMessage += "Building Name Must Contain Characters\n";
                BuildingNameValid = false;
            }

            return BuildingNameValid;
        }

        private static bool SizeSelfValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool BuildingSizeValid = true;

            if (_Width <= 0 || _Height <= 0)
            {
                _ErrorMessage += "Width And Height Dimensions Must Be Positive Numbers\n";
                BuildingSizeValid = false;
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

            if (_RoomToModify.Name != _NewRoomName || _RoomToModify.Width != _NewWidth || _RoomToModify.Height != _NewHeight || _RoomToModify.CenterX != _NewCenterX || _RoomToModify.CenterY != _NewCenterY || _RoomToModify.RoomColor != _NewRoomColor)
            {
                if (_RoomToModify.Name != _NewRoomName)
                {
                    if (!RoomNameSystemValidation(_NewRoomName, ref _ErrorMessage))
                    {
                        ModifyRoomSuccess = false;
                    }
                }

                if (_RoomToModify.Width != _NewWidth || _RoomToModify.Height != _NewHeight || _RoomToModify.CenterX != _NewCenterX || _RoomToModify.CenterY != _NewCenterY)
                {
                    if (!RoomDimensionValidation(_NewWidth, _NewHeight, _NewCenterX, _NewCenterY, ref _ErrorMessage, _RoomToModify))
                    {
                        ModifyRoomSuccess = false;
                    }
                }

                if (ModifyRoomSuccess)
                {
                    if (_RoomToModify.TryModify(_NewRoomName, _NewWidth, _NewHeight, _NewCenterX, _NewCenterY, _NewRoomColor, out _ErrorMessage))
                    {
                        RoomListChanged?.Invoke();
                    }
                    else
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

            __RoomList.Remove(_RoomToRemove);
            RoomListChanged?.Invoke();
            return true;
        }

        private bool RoomNameSystemValidation(string _RoomName, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            if (__RoomList.Any(CurrentRoom => CurrentRoom.Name == _RoomName))
            {
                _ErrorMessage += $"Two Rooms Cannot Have The Same Name. {_RoomName} already exists.\n";
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
                _ErrorMessage += $"Room Center ({_CenterX},{_CenterY}) Is Outside Building Limits. Must Be Between (0,0) and ({this.Width},{this.Height})\n";
                SystemValid = false;
            }

            //Check That Room Left Is In Building
            if (NewRoomLeftLocation < 0)
            {
                _ErrorMessage += $"Room Left Boundary ({NewRoomLeftLocation}) Is Outside Building Limits. Must Be Greater Than 0.\n";
                SystemValid = false;
            }

            //Check That Room Right Is In Building
            if (NewRoomRightLocation > this.Width)
            {
                _ErrorMessage += $"Room Right Boundary ({NewRoomRightLocation}) Is Outside Building Limits. Must Be Less Than {this.Width}.\n";
                SystemValid = false;
            }

            //Check That Room Top Is In Building
            if (NewRoomTopLocation < 0)
            {
                _ErrorMessage += $"Room Top Boundary ({NewRoomTopLocation}) Is Outside Building Limits. Must Be Greater Than 0.\n";
                SystemValid = false;
            }

            //Check That Room Bottom Is In Building
            if (NewRoomBottomLocation > this.Height)
            {
                _ErrorMessage += $"Room Bottom Boundary ({NewRoomBottomLocation}) Is Outside Building Limits. Must Be Less Than {this.Height}.\n";
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
                        _ErrorMessage += $"Room Collides With {CurrentRoom.Name}\n";
                        SystemValid = false;
                    }
                }
            }

            return SystemValid;
        }

        public int TotalItemCount()
        {
            return UnsortedItems.TotalItemCount() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemCount());
        }

        public double TotalItemValue()
        {
            return UnsortedItems.TotalItemValue() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemValue());
        }

        //public void AddItem(IStored _ItemToAdd)
        //{
        //    UnsortedItems.AddItem(_ItemToAdd);
        //}

        //public void RemoveItem(IStored _ItemToRemove)
        //{
        //    UnsortedItems.RemoveItem(_ItemToRemove);
        //}

        //public void MoveItem(IStored _ItemToMove, IStorage _Destination)
        //{
        //    UnsortedItems.MoveItem(_ItemToMove, _Destination);
        //}
    }
}
