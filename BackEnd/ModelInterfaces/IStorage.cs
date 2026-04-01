using BackEnd.Enumerations;
using System.Text.Json.Serialization;

namespace BackEnd.ModelInterfaces
{
    /// <summary>
    /// Interface Defining Storage Behavior Storage Objects
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// The List Of Items Stored In The Storage
        /// </summary>
        [JsonInclude]
        internal IReadOnlyList<IStored> StoredItems { get; }

        /// <summary>
        /// Attempts To Add A New Item Or Container To The Storage
        /// </summary>
        internal bool TryAddIStored(StoredItemType _IStoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Modify An Existing Item Or Container In The Storage
        /// </summary>
        internal bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Move An Item Or Container To Another Storage Holder
        /// </summary>
        internal bool TryMoveIStored(IStored _IStoredToMove, IStorageHolder _Destination, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Remove An Item Or Container From The Storage
        /// </summary>
        internal bool TryRemoveIStored(IStored _IStoredToRemove, out string? _ErrorMessage);

        /// <summary>
        /// Returns The Total Count Of Items In The Storage, Including Nested Containers
        /// </summary>
        internal int TotalItemCount();

        /// <summary>
        /// Returns The Total Value Of Items In The Storage, Including Nested Containers
        /// </summary>
        internal double TotalItemValue();
    }
}
