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
        public string Name { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        private Storage UnsortedItems = new Storage();

        private List<Room> __Rooms = new List<Room>();

        public IReadOnlyList<Room> Rooms
        {
            get
            { return __Rooms.AsReadOnly(); }
        }

        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return UnsortedItems.StoredItems; }
        }

        private Building(string _Name, int _Height, int _Width)
        {
            this.Name = _Name;
            this.Height = _Height;
            this.Width = _Width;
        }

        internal static bool TryCreate(string _BuildingName, int _Height, int _Width, out Building? _CreatedBuilding, out string? _ErrorMessage)
        {
            _CreatedBuilding = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (string.IsNullOrEmpty(_BuildingName))
            {
                _ErrorMessage += "Building Name Must Contain Characters\n";
                CreationSuccess = false;
            }

            if (_Height <= 0 || _Width <= 0)
            {
                _ErrorMessage += "Height And Width Dimensions Must Be Positive Whole Numbers\n";
                CreationSuccess = false;
            }

            if (CreationSuccess)
            {
                _CreatedBuilding = new Building(_BuildingName, _Height, _Width);
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

        internal bool TryModifySize(int _NewHeight, int _NewWidth, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (_NewHeight <= 0 || _NewWidth <= 0)
            {
                _ErrorMessage = "Height And Width Dimensions Must Be Positive Whole Numbers";
                return false;
            }

            this.Height = _NewHeight;
            this.Width = _NewWidth;
            return true;
        }

        public int TotalItemCount()
        {
            return UnsortedItems.TotalItemCount() + Rooms.Sum(CurrentRoom => CurrentRoom.TotalItemCount());
        }

        public double TotalItemValue()
        {
            return UnsortedItems.TotalItemValue() + Rooms.Sum(CurrentRoom => CurrentRoom.TotalItemValue());
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

        public void AddRoom(Room _RoomToAdd)
        {
            __Rooms.Add(_RoomToAdd);
        }

        public void RemoveRoom(Room _RoomToRemove)
        {
            __Rooms.Remove(_RoomToRemove);
        }
    }
}
