using BackEnd.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.ModelInterfaces
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(Item))]
    [JsonDerivedType(typeof(Container))]
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

        [JsonIgnore]
        public IStorageHolder ImmediateParent { get; }
        //public Room? RoomParent { get; } //can be null if item is directly in the building
    }
}
