using BackEnd.ModelClasses;
using System.Text.Json.Serialization;

namespace BackEnd.ModelInterfaces
{
    /// <summary>
    /// Interface Representing An Item Or Container That Can Be Stored
    /// </summary>
    [JsonPolymorphic] // Indicates That This Interface Supports Polymorphic JSON Serialization
    [JsonDerivedType(typeof(Item))] // Registers 'Item' As A Derived Type For Polymorphic JSON Serialization
    [JsonDerivedType(typeof(Container))] // Registers 'Container' As A Derived Type For Polymorphic JSON Serialization
    public interface IStored
    {
        /// <summary>
        /// Unique Identifier For The Item
        /// </summary>
        //public int ID { get; }

        /// <summary>
        /// The Name Of The Stored Item
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The Description Of The Stored Item
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The Monetary Value Of The Stored Item
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// The Quantity Of The Stored Item
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// The Immediate Parent Storage Holder Containing This Item
        /// </summary>
        [JsonIgnore] // Excludes ImmediateParent From JSON To Prevent Circular References During Serialization
        public IStorageHolder ImmediateParent { get; }
    }
}
