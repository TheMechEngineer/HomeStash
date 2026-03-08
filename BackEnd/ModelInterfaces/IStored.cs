using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BackEnd.ModelClasses;

namespace BackEnd.ModelInterfaces
{
    public interface IStored
    {
        /// <summary>
        /// Unique Identifier For The Item
        /// </summary>
        //public int ID { get; }
        public string Name { get; }
        public string Description { get; }
        public double Value { get; }
        public int Quantity { get; }
        public IStorageHolder ImmediateParent { get; }
        //public Room? RoomParent { get; } //can be null if item is directly in the building
    }
}
