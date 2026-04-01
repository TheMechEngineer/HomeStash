using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;

namespace BackEnd.ModelClasses
{
    /// <summary>
    /// Represents Storage That Contains Items And Containers,
    /// And Provides Validation And Event Notification For Changes
    /// </summary>
    internal class Storage : IStorage
    {
        /// <summary>
        /// Event Triggered When Stored Items Change
        /// </summary>
        internal event Action? StoredItemsChanged;

        /// <summary>
        /// Event Triggered When A Stored Item Is Modified
        /// </summary>
        internal event Action? StoredItemModified;

        /// <summary>
        /// The Internal List Of Stored Items
        /// </summary>
        private List<IStored> __StoredItems = new List<IStored>();

        /// <summary>
        /// The Read-Only List Of Stored Items
        /// </summary>
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return __StoredItems.AsReadOnly(); }
        }

        /// <summary>
        /// The Storage Holder That Owns This Storage
        /// </summary>
        internal IStorageHolder ImmediateParent { get; set; }

        /// <summary>
        /// Constructor Used To Initialize Storage With Its Parent
        /// </summary>
        internal Storage(IStorageHolder _ImmediateParent)
        {
            ImmediateParent = _ImmediateParent;
        }

        //I dont want this to be public, but to satisfy the compiler interface rules it must be. However since the class itself and inteface are internal, the front end wont be able to see the method anyways.
        //From what I understand, because the interface method signature is private, the front end wont be able to access this even though its public
        /// <summary>
        /// Attempts To Add A Stored Item To The Current Storage
        /// </summary>
        public bool TryAddIStored(StoredItemType _IStoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddIStoredSuccess = true;

            //No System Validation For Adding Stored Items At This Point

            IStored? StoredObject = null;

            // Attempt To Create Either An Item Or Container Based On The Type Specified In The Signature
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

            // Adds Stored Object If Creation Was Successful
            if (AddIStoredSuccess)
            {
                __StoredItems.Add(StoredObject);

                // Subscribes To Nested Container Events For Event Forwarding
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

        /// <summary>
        /// Attempts To Modify A Stored Item In The Current Storage
        /// </summary>
        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyIStoredSuccess = true;

            bool TextChanged = _IStoredToModify.Name != _NewStoredName || _IStoredToModify.Description != _NewDescription;
            bool NumericsChanged = _IStoredToModify.Value != _NewValue || _IStoredToModify.Quantity != _NewQuantity;

            // Checks If Any Fields Have Been Modified
            if (TextChanged || NumericsChanged)
            {
                //No System Validation For Adding Stored Items At This Point

                Type SelectionType = _IStoredToModify.GetType();

                // Attempt To Modify Either An Item Or Container Based On The Type Specified In The Signature
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

            // Triggers Event If Modification Was Successful
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

        /// <summary>
        /// Attempts To Move A Stored Item From The Current Storage To Another Storage
        /// </summary>
        public bool TryMoveIStored(IStored _IStoredToMove, IStorageHolder _Destination, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool MoveIStoredSuccess = true;

            bool ImmediateParentChanged = _IStoredToMove.ImmediateParent != _Destination;

            // Checks If The Destination Is Not The Same As The Origin
            if (ImmediateParentChanged)
            {
                if (ImmediateParentChanged)
                {
                    // Runs Move System-Validation If Parent Was Changed
                    if (!IStoredMoveSystemValidation(_IStoredToMove, _Destination, ref _ErrorMessage))
                    {
                        MoveIStoredSuccess = false;
                    }
                }

                // If Stored Item Passes System-Validation Attempt To Modify The Stored Item
                if (MoveIStoredSuccess)
                {
                    Type SelectionType = _IStoredToMove.GetType();

                    // Attempt To Set New Parent For Either An Item Or Container Based On The Type Specified In The Signature
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

            // Moves Stored Item Between Storage Objects If PArent Modfication Successful
            if (MoveIStoredSuccess)
            {
                Storage StorageDestination = _Destination.CurrentStorage as Storage;

                // Rewires Event Forwarding For Containers
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

        /// <summary>
        /// Attempts To Remove A Stored Item From The Current Storage
        /// </summary
        public bool TryRemoveIStored(IStored _IStoredToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            // Validates The Stored Item Exists Within The Storage
            if (!__StoredItems.Contains(_IStoredToRemove))
            {
                _ErrorMessage = "Item To Be Removed Must Exist In The Storage List";
                return false;
            }

            // Unsubscribes Event Forwarding For Containers
            if (_IStoredToRemove is Container RemovedContainer)
            {
                RemovedContainer.StoredItemsChanged -= StoredItemsChangedFowarding;
                RemovedContainer.StoredItemModified -= StoredItemModifiedFowarding;
            }

            __StoredItems.Remove(_IStoredToRemove);
            StoredItemsChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Validates Stored Item Move System Requirements
        /// </summary>
        private bool IStoredMoveSystemValidation(IStored _IStoredToMove, IStorageHolder _Destination, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            if (_IStoredToMove.GetType() == typeof(Container))
            {
                // Validates Item Is Not Moved Into Itself
                if (_IStoredToMove == _Destination)
                {
                    _ErrorMessage += $"System Validation Error: {(_IStoredToMove as Container).Name} Cannot Be Moved Into Itself.\n";
                    SystemValid = false;
                }

                // Builds A List Of All Containers Nested In The Current Container
                List<Container> InvalidContainers = GetNestedContainerItems(_IStoredToMove as Container);
                InvalidContainers.Remove(_IStoredToMove as Container);

                // Validates Item Is Not Moved Into One Of Its Children
                if (InvalidContainers.Contains(_Destination))
                {
                    _ErrorMessage += $"System Validation Error: {(_IStoredToMove as Container).Name} Cannot Be Moved Into One Of Its Own Children.\n";
                    SystemValid = false;
                }
            }

            return SystemValid;
        }

        /// <summary>
        /// Retrieves All Nested Containers Within A Container
        /// </summary>
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

        /// <summary>
        /// Calculates The Total Number Of Items In The Storage Including Nested Containers
        /// </summary>
        public int TotalItemCount()
        {
            int TotalItemCount = 0;

            foreach (IStored CurrentStored in __StoredItems)
            {
                TotalItemCount += (CurrentStored.GetType() == typeof(Container) ? (CurrentStored as Container).TotalItemCount() : CurrentStored.Quantity);
            }

            return TotalItemCount;
        }

        /// <summary>
        /// Calculates The Total Value Of Items In The Storage Including Nested Containers
        /// </summary
        public double TotalItemValue()
        {
            double TotalItemValue = 0;

            foreach (IStored CurrentStored in __StoredItems)
            {
                TotalItemValue += (CurrentStored.GetType() == typeof(Container) ? (CurrentStored as Container).TotalItemValue() : CurrentStored.Quantity * CurrentStored.Value);
            }

            return TotalItemValue;
        }

        /// <summary>
        /// Forwards Stored Items Changed Events From Nested Containers
        /// </summary>
        private void StoredItemsChangedFowarding()
        {
            this.StoredItemsChanged?.Invoke();
        }

        /// <summary>
        /// Forwards Stored Item Modified Events From Nested Containers
        /// </summary>
        private void StoredItemModifiedFowarding()
        {
            this.StoredItemModified?.Invoke();
        }
    }
}