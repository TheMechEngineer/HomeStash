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
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        private Storage UnsortedItems = new Storage();

        private List<Room> __Rooms = new List<Room>();

        public IReadOnlyList<Room> Rooms
        {
            get
            {
                return __Rooms.AsReadOnly();
            }
        }

        public IReadOnlyList<IStored> StoredItems
        {
            get
            {
                return UnsortedItems.StoredItems;
            }
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
