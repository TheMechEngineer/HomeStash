using BackEnd.Enumerations;

namespace BackEnd.ModelInterfaces
{
    public interface IStorageHolder
    {
        internal string Name { get; }
        internal IStorage CurrentStorage { get; }
        public IReadOnlyList<IStored> StoredItems { get; }
        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage);
        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage);
        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage);
        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage);
        public int TotalItemCount();
        public double TotalItemValue();
    }
}
