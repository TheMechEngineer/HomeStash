using BackEnd.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelInterfaces
{
    public interface IStorageHolder
    {
        internal IStorage CurrentStorage { get; }
        public IReadOnlyList<IStored> StoredItems { get; }

        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage);

        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage);

        public bool TryMoveIStored(IStored _ItemToMove, IStorage _Destination, out string? _ErrorMessage);

        public int TotalItemCount();
        public double TotalItemValue();
    }
}
