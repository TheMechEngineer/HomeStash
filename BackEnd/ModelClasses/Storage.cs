
using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    internal class Storage : IStorage
    {
        public event Action? StoredItemsChanged;

        private List<IStored> __StoredItems = new List<IStored>();
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return __StoredItems.AsReadOnly(); }
        }

        internal void TryAddIStored(StoredItemType _ItemType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool TryAddSuccess = true;

            //No System Validation For Adding Stored Items At This Point

            IStored? NewStoredItem;

            switch (_ItemType)
            {
                case StoredItemType.Item:
                    TryAddSuccess = Item.TryCreate(_StoredName, _Description, _Value, _Quantity,);
                    break;
                case StoredItemType.Container:
                    TryAddSuccess = Container.TryCreate(_StoredName, _Description, _Value, _Quantity,);
                    break;
            }







            return TryAddSuccess;



            __StoredItems.Add(_ItemToAdd);
            //_ItemToAdd.ImmediateParent = this;

        }

        //NEED TO UPDATE THE ROOM OF THE MOVED ITEM TOO

        //MAYBE I NEED TO MAKE A REMOVE ITEM WHICH PUTS IT IN THE UNSORTED AND DELETE ITEM WHICH ACTUALLY DELTES IT
        public void TryRemoveIStored(IStored _ItemToRemove)
        {
            __StoredItems.Remove(_ItemToRemove);
            //NEED TO UPDATE THE ROOM OF THE MOVED ITEM TOO
            //If the item is deleted then this isnt needed
            //and if its moving to the unsorted items it isnt null
            //_ItemToRemove.ImmediateParent = null;
        }
        public void TryMoveIStored(IStored _ItemToMove, IStorage _Destination)
        {
            _Destination.AddItem(_ItemToMove);
            this.RemoveItem(_ItemToMove);
        }
        public int TotalItemCount()
        {
            return __StoredItems.Sum(CurrentItem => CurrentItem.Quantity);
        }
        public double TotalItemValue()
        {
            return __StoredItems.Sum(Item => Item.Value * Item.Quantity);
        }
    }
}
