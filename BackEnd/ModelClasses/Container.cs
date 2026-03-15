using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Container : Item, IStorageHolder
    {
        public event Action? StoredItemsChanged
        {
            add { ContainerStorage.StoredItemsChanged += value; }
            remove { ContainerStorage.StoredItemsChanged -= value; }
        }

        public event Action? StoredItemModified
        {
            add { ContainerStorage.StoredItemModified += value; }
            remove { ContainerStorage.StoredItemModified -= value; }
        }

        private Storage ContainerStorage;
        public IStorage CurrentStorage
        {
            get
            { return ContainerStorage; }
        }

        [JsonIgnore]
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return ContainerStorage.StoredItems; }
        }

        private Container(string _ItemName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent)
            : base(_ItemName, _Description, _Value, _Quantity, _ImmediateParent)
        {
            ContainerStorage = new Storage(this);
        }

        internal static bool TryCreate(string _ContainerName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent, out Container? _CreatedContainer, out string? _ErrorMessage)
        {
            _CreatedContainer = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (!NameSelfValidation(_ContainerName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!ValueSelfValidation(_Value, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!QuantitySelfValidation(_Quantity, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!ImmediateParentSelfValidation(_ImmediateParent, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (CreationSuccess)
            {
                _CreatedContainer = new Container(_ContainerName, _Description, _Value, _Quantity, _ImmediateParent);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        internal new bool TryModify(string _NewContainerName, string _NewDescription, double _NewValue, int _NewQuantity, IStorageHolder _NewImmediateParent, out string? _ErrorMessage) //new is needed to suppress the warning that we are overwriting the base method
        {
            return base.TryModify(_NewContainerName, _NewDescription, _NewValue, _NewQuantity, _NewImmediateParent, out _ErrorMessage);
        }

        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            return ContainerStorage.TryAddIStored(_StoredType, _StoredName, _Description, _Value, _Quantity, out _ErrorMessage);
        }

        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            return ContainerStorage.TryModifyIStored(_IStoredToModify, _NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
        }

        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage)
        {
            return ContainerStorage.TryMoveIStored(_ItemToMove, _Destination, out _ErrorMessage);
        }

        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage)
        {
            return ContainerStorage.TryRemoveIStored(_StoredToRemove, out _ErrorMessage);
        }

        public int TotalItemCount()
        {
            return (ContainerStorage.TotalItemCount() + 1) * this.Quantity;
        }
        public double TotalItemValue()
        {
            return (ContainerStorage.TotalItemValue() + this.Value) * this.Quantity;
        }
    }
}
