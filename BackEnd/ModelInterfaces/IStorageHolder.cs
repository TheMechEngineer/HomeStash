using BackEnd.Enumerations;

namespace BackEnd.ModelInterfaces
{
    /// <summary>
    /// Interface Representing An Entity That Has Storage
    /// </summary>
    public interface IStorageHolder
    {
        /// <summary>
        /// The Name Of The Storage Holder
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// The Storage Associated With The Holder
        /// </summary>
        internal IStorage CurrentStorage { get; }

        /// <summary>
        /// The List Of Items Directly Stored By The Holder
        /// </summary>
        public IReadOnlyList<IStored> StoredItems { get; }

        /// <summary>
        /// Attempts To Add A New Item Or Container To The Holder
        /// </summary>
        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Modify An Existing Item Or Container Of The Holder
        /// </summary>
        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Move An Item Or Container To Another Storage Holder
        /// </summary>
        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Remove An Item Or Container From The Holder
        /// </summary>
        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage);

        /// <summary>
        /// Returns The Total Count Of Items In The Holder, Including Nested Containers
        /// </summary>
        public int TotalItemCount();

        /// <summary>
        /// Returns The Total Value Of Items In The Holder, Including Nested Containers
        /// </summary>
        public double TotalItemValue();
    }
}
