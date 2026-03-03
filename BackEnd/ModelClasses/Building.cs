using BackEnd.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Building
    {
        public event Action? RoomListChanged;

        public string Name { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        private Storage UnsortedItems = new Storage();

        private List<Room> __RoomList = new List<Room>();

        public IReadOnlyList<Room> RoomList
        {
            get
            { return __RoomList.AsReadOnly(); }
        }

        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return UnsortedItems.StoredItems; }
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

            if (string.IsNullOrEmpty(_BuildingName))
            {
                _ErrorMessage += "Building Name Must Contain Characters\n";
                CreationSuccess = false;
            }

            if (_Width <= 0 || _Height <= 0 )
            {
                _ErrorMessage += "Width And Height Dimensions Must Be Positive Numbers\n";
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

        internal bool TryModifyName(string _NewBuildingName, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (string.IsNullOrEmpty(_NewBuildingName))
            {
                _ErrorMessage = "Building Name Must Contain Characters";
                return false;
            }

            this.Name = _NewBuildingName;
            return true;
        }

        internal bool TryModifySize(float _NewWidth, float _NewHeight, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (_NewWidth <= 0 || _NewHeight <= 0)
            {
                _ErrorMessage = "Width And Height Dimensions Must Be Positive Numbers";
                return false;
            }

            this.Width = _NewWidth;
            this.Height = _NewHeight;
            return true;
        }

        public int TotalItemCount()
        {
            return UnsortedItems.TotalItemCount() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemCount());
        }

        public double TotalItemValue()
        {
            return UnsortedItems.TotalItemValue() + RoomList.Sum(CurrentRoom => CurrentRoom.TotalItemValue());
        }

        public void AddItem(IStored _ItemToAdd)
        {
            UnsortedItems.AddItem(_ItemToAdd);
        }

        public void RemoveItem(IStored _ItemToRemove)
        {
            UnsortedItems.RemoveItem(_ItemToRemove);
        }

        public void MoveItem(IStored _ItemToMove, IStorage _Destination)
        {
            UnsortedItems.MoveItem(_ItemToMove, _Destination);
        }

        public bool TryAddRoom(string _RoomName, float _Width, float _Height, float _CenterX, float _CenterY, int _RoomColor, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool CreationSuccess = true;

            //I know I could condense this to directly set CreationSuccess to this function, but I want to match the structure in my other classes.
            if (!NewRoomSystemValidation(_RoomName, _Width, _Height, _CenterX, _CenterY, out _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (CreationSuccess)
            {
                Room? NewRoom;

                if (Room.TryCreate(_RoomName, _Width, _Height, _CenterX, _CenterY, _RoomColor, out NewRoom, out _ErrorMessage))
                {
                    __RoomList.Add(NewRoom);
                    RoomListChanged?.Invoke();
                }
                else
                {
                    CreationSuccess = false;
                }
            }

            return CreationSuccess;
        }

        private bool NewRoomSystemValidation(string _RoomName, float _Width, float _Height, float _CenterX, float _CenterY, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (__RoomList.Any(CurrentRoom => CurrentRoom.Name == _RoomName))
            {
                _ErrorMessage += $"Two Rooms Cannot Have The Same Name. {_RoomName} already exists.\n";
                CreationSuccess = false;
            }

            //Check That Room Center Is In Building
            if (_CenterX < 0 || _CenterY < 0 || _CenterX > this.Width || _CenterY > this.Height)
            {
                _ErrorMessage += $"Room Center ({_CenterX},{_CenterY}) Is Outside Building Limits. Room Center Point Must Be Between (0,0) and ({this.Width},{this.Height})\n";
                CreationSuccess = false;
            }

            //Check That Room Left Is In Building
            if (_CenterX - _Width/2 < 0)
            {
                _ErrorMessage += "Room Left Boundary Is Outside Building Limits\n";
                CreationSuccess = false;
            }

            //system check needs for

            //check right, top, bottom are in bounds

            //check for collision with existing buildings

            //current idea for checking collision, the below checks for left wall, but will need variants of this for every wall

            //for each room in room list
            //if _centerX - Width/2 < room.centerx + room.width/2 && _centerX - Width/2 > room.centerx - room.width/2 (This means the left wall is between the left and right walls of the comparison)
            // { then check if it is vertically in the bounds, if so then error
            //this doesnt correctly identify if the new room surrounds the existing room on all four sides

            //maybe I need to treat the four corners of the rooms as points, instead of looking at walls. Then check if the four points are in between the four points
            // for example 
            // if topleftpoint is between room.right - room.left && room.bottom- room.top
            // this logic would repeat for the other three points
            // this seems pretty clean
            // now just need to figure out logic to catch if new room encompasses existing room

            if (!CreationSuccess)
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        public void RemoveRoom(Room _RoomToRemove)
        {
            __RoomList.Remove(_RoomToRemove);
        }
    }
}
