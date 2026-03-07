
using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    internal class Storage : IStorage
    {
        internal event Action? StoredItemsChanged;

        private List<IStored> __StoredItems = new List<IStored>();
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return __StoredItems.AsReadOnly(); }
        }

        //I dont want this to be public, but to satisfy the compiler interface rules it must be. However since the class itself and inteface are internal, the front end wont be able to see the method anyways.
        //From what I understand, because the interface method signature is private, the front end wont be able to access this even though its public
        public bool TryAddIStored(StoredItemType _IStoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddIStoredSuccess = true;

            //No System Validation For Adding Stored Items At This Point

            IStored? StoredObject = null;

            switch (_IStoredType)
            {
                case StoredItemType.Item:
                    Item? _NewStoredItem;
                    AddIStoredSuccess = Item.TryCreate(_StoredName, _Description, _Value, _Quantity, out _NewStoredItem, out _ErrorMessage);
                    StoredObject = _NewStoredItem;
                    break;
                case StoredItemType.Container:
                    Container? _NewStoredContainer;
                    AddIStoredSuccess = Container.TryCreate(_StoredName, _Description, _Value, _Quantity, out _NewStoredContainer, out _ErrorMessage);
                    StoredObject = _NewStoredContainer;
                    break;
            }

            if (AddIStoredSuccess)
            {
                __StoredItems.Add(StoredObject);

                if (StoredObject is Container NewContainer)
                {
                    NewContainer.StoredItemsChanged += StoredItemsChangedFowarding;
                }

                StoredItemsChanged?.Invoke();
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }
            return AddIStoredSuccess;
        }

        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyIStoredSuccess = true;

            if (_IStoredToModify.Name != _NewStoredName || _IStoredToModify.Description != _NewDescription || _IStoredToModify.Value != _NewValue || _IStoredToModify.Quantity != _NewQuantity)
            {
                //No System Validation For Adding Stored Items At This Point

                Type SelectionType = _IStoredToModify.GetType();

                switch (SelectionType)
                {
                    case Type CurrentType when SelectionType == typeof(Item):
                        ModifyIStoredSuccess = (_IStoredToModify as Item).TryModify(_NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
                        break;
                    case Type CurrentType when SelectionType == typeof(Container):
                        ModifyIStoredSuccess = (_IStoredToModify as Container).TryModify(_NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
                        break;
                }

            }
            else
            {
                ModifyIStoredSuccess = false;
                _ErrorMessage += $"No Item Fields Have Been Modified\n";
            }

            if (ModifyIStoredSuccess)
            {
                StoredItemsChanged?.Invoke();
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifyIStoredSuccess;

        }

        public bool TryRemoveIStored(IStored _IStoredToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (!__StoredItems.Contains(_IStoredToRemove))
            {
                _ErrorMessage = "Item To Be Removed Must Exist In The Storage List";
                return false;
            }

            if (_IStoredToRemove is Container RemovedContainer)
            {
                RemovedContainer.StoredItemsChanged -= StoredItemsChangedFowarding;
            }

            __StoredItems.Remove(_IStoredToRemove);
            StoredItemsChanged?.Invoke();
            return true;
        }

        public bool TryMoveIStored(IStored _IStoredToMove, IStorage _Destination, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (!this.__StoredItems.Contains(_IStoredToMove))
            {
                _ErrorMessage = "Item To Be Moved Must Exist In The Storage List";
                return false;
            }

            Storage StorageDestination = _Destination as Storage;

            if (_IStoredToMove is Container MovedContainer)
            {
                MovedContainer.StoredItemsChanged -= StoredItemsChangedFowarding;
                MovedContainer.StoredItemsChanged += StorageDestination.StoredItemsChangedFowarding;
            }

            StorageDestination.__StoredItems.Add(_IStoredToMove);
            StorageDestination.StoredItemsChanged?.Invoke();

            this.__StoredItems.Remove(_IStoredToMove);
            this.StoredItemsChanged?.Invoke();

            return true;
        }

        public int TotalItemCount()
        {
            int TotalItemCount = 0;

            foreach (IStored CurrentStored in __StoredItems)
            {
                TotalItemCount += (CurrentStored.GetType() == typeof(Container) ? (CurrentStored as Container).TotalItemCount() : CurrentStored.Quantity);
            }

            return TotalItemCount;
        }
        public double TotalItemValue()
        {
            double TotalItemValue = 0;

            foreach (IStored CurrentStored in __StoredItems)
            {
                TotalItemValue += (CurrentStored.GetType() == typeof(Container) ? (CurrentStored as Container).TotalItemValue() : CurrentStored.Quantity * CurrentStored.Value);
            }

            return TotalItemValue;
            return __StoredItems.Sum(Item => Item.Value * Item.Quantity);
        }

        private void StoredItemsChangedFowarding()
        {
            this.StoredItemsChanged?.Invoke();
        }
    }
}
