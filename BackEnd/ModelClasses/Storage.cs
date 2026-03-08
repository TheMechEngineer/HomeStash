
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
        internal event Action? StoredItemModified;

        private List<IStored> __StoredItems = new List<IStored>();
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return __StoredItems.AsReadOnly(); }
        }

        internal IStorageHolder ImmediateParent { get; set; }

        internal Storage(IStorageHolder _ImmediateParent)
        {
            ImmediateParent = _ImmediateParent;
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
                    AddIStoredSuccess = Item.TryCreate(_StoredName, _Description, _Value, _Quantity, this.ImmediateParent, out _NewStoredItem, out _ErrorMessage);
                    StoredObject = _NewStoredItem;
                    break;
                case StoredItemType.Container:
                    Container? _NewStoredContainer;
                    AddIStoredSuccess = Container.TryCreate(_StoredName, _Description, _Value, _Quantity, this.ImmediateParent, out _NewStoredContainer, out _ErrorMessage);
                    StoredObject = _NewStoredContainer;
                    break;
            }

            if (AddIStoredSuccess)
            {
                __StoredItems.Add(StoredObject);

                if (StoredObject is Container NewContainer)
                {
                    NewContainer.StoredItemsChanged += StoredItemsChangedFowarding;
                    NewContainer.StoredItemModified += StoredItemModifiedFowarding;
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

            bool TextChanged = _IStoredToModify.Name != _NewStoredName || _IStoredToModify.Description != _NewDescription;
            bool NumericsChanged = _IStoredToModify.Value != _NewValue || _IStoredToModify.Quantity != _NewQuantity;

            if ( TextChanged  || NumericsChanged)
            {
                //No System Validation For Adding Stored Items At This Point

                Type SelectionType = _IStoredToModify.GetType();

                switch (SelectionType)
                {
                    case Type CurrentType when SelectionType == typeof(Item):
                        ModifyIStoredSuccess = (_IStoredToModify as Item).TryModify(_NewStoredName, _NewDescription, _NewValue, _NewQuantity, _IStoredToModify.ImmediateParent, out _ErrorMessage);
                        break;
                    case Type CurrentType when SelectionType == typeof(Container):
                        ModifyIStoredSuccess = (_IStoredToModify as Container).TryModify(_NewStoredName, _NewDescription, _NewValue, _NewQuantity, _IStoredToModify.ImmediateParent, out _ErrorMessage);
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
                //If This Isn't Sufficient, I Could Change To Do Event Fowarding From Item Class ( I might have to figure out if that would work with containers too, since they inherit from item)
                StoredItemModified?.Invoke();
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifyIStoredSuccess;

        }

        public bool TryMoveIStored(IStored _IStoredToMove, IStorageHolder _Destination, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool MoveIStoredSuccess = true;

            bool ImmediateParentChanged = _IStoredToMove.ImmediateParent != _Destination;
 
            if (ImmediateParentChanged)
            {
                if (ImmediateParentChanged)
                {
                    if (!IStoredMoveSystemValidation(_IStoredToMove, _Destination, ref _ErrorMessage))
                    {
                        MoveIStoredSuccess = false;
                    }
                }

                if (MoveIStoredSuccess)
                {
                    Type SelectionType = _IStoredToMove.GetType();

                    switch (SelectionType)
                    {
                        case Type CurrentType when SelectionType == typeof(Item):
                            MoveIStoredSuccess = (_IStoredToMove as Item).TryModify(_IStoredToMove.Name, _IStoredToMove.Description, _IStoredToMove.Value, _IStoredToMove.Quantity, _Destination, out _ErrorMessage);
                            break;
                        case Type CurrentType when SelectionType == typeof(Container):
                            MoveIStoredSuccess = (_IStoredToMove as Container).TryModify(_IStoredToMove.Name, _IStoredToMove.Description, _IStoredToMove.Value, _IStoredToMove.Quantity, _Destination, out _ErrorMessage);
                            break;
                    }
                }

            }
            else
            {
                MoveIStoredSuccess = false;
                _ErrorMessage += $"No Item Fields Have Been Modified\n";
            }

            if (MoveIStoredSuccess)
            {
                Storage StorageDestination = _Destination.CurrentStorage as Storage;

                if (_IStoredToMove is Container MovedContainer)
                {
                    MovedContainer.StoredItemsChanged -= StoredItemsChangedFowarding;
                    MovedContainer.StoredItemsChanged += StorageDestination.StoredItemsChangedFowarding;

                    MovedContainer.StoredItemModified -= StoredItemModifiedFowarding;
                    MovedContainer.StoredItemModified += StorageDestination.StoredItemModifiedFowarding;
                }

                StorageDestination.__StoredItems.Add(_IStoredToMove);
                StorageDestination.StoredItemsChanged?.Invoke();

                this.__StoredItems.Remove(_IStoredToMove);
                this.StoredItemsChanged?.Invoke();
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return MoveIStoredSuccess;

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
                RemovedContainer.StoredItemModified -= StoredItemModifiedFowarding;
            }

            __StoredItems.Remove(_IStoredToRemove);
            StoredItemsChanged?.Invoke();
            return true;
        }

        private bool IStoredMoveSystemValidation(IStored _IStoredToMove, IStorageHolder _Destination, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            if (_IStoredToMove.GetType() == typeof(Container))
            {
                if (_IStoredToMove == _Destination)
                {
                    _ErrorMessage += $"System Validation Error: {(_IStoredToMove as Container).Name} Cannot Be Moved Into Itself.\n";
                    SystemValid = false;
                }

                List<Container> InvalidContainers = GetNestedContainerItems(_IStoredToMove as Container);
                InvalidContainers.Remove(_IStoredToMove as Container);

                if (InvalidContainers.Contains(_Destination))
                {
                    _ErrorMessage += $"System Validation Error: {(_IStoredToMove as Container).Name} Cannot Be Moved Into One Of Its Own Children.\n";
                    SystemValid = false;
                }
            }
            
            return SystemValid;
        }

        private List<Container> GetNestedContainerItems(Container _CurrentContainer)
        {
            List<Container> ValidContainerList = new List<Container>();
            ValidContainerList.Add(_CurrentContainer);

            foreach (Container CurrentContainer in _CurrentContainer.StoredItems.OfType<Container>())
            {
                ValidContainerList.AddRange(GetNestedContainerItems(CurrentContainer));
            }

            return ValidContainerList;
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
        }

        private void StoredItemsChangedFowarding()
        {
            this.StoredItemsChanged?.Invoke();
        }

        private void StoredItemModifiedFowarding()
        {
            this.StoredItemModified?.Invoke();
        }
    }
}
