using BackEnd.Enumerations;
using BackEnd.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelInterfaces
{
    public interface IStorage
    {
        internal IReadOnlyList<IStored> StoredItems { get; }
        internal bool TryAddIStored(StoredItemType _IStoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage);
        internal bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage);
        internal bool TryRemoveIStored(IStored _IStoredToRemove, out string? _ErrorMessage);
        internal bool TryMoveIStored(IStored _IStoredToMove, IStorage _Destination, out string? _ErrorMessage);
        internal int TotalItemCount();
        internal double TotalItemValue();
    }
}
