using BackEnd.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Container : Item, IStorageHolder
    {
        private Storage ContainerStorage = new Storage();
        public IStorage Storage
        {
            get
            { return ContainerStorage; }
        }

        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return ContainerStorage.StoredItems; }
        }

        private Container(string _ItemName, string _Description, double _Value, int _Quantity)
            : base(_ItemName, _Description, _Value, _Quantity)
        {}

        internal static bool TryCreate(string _ContainerName, string _Description, double _Value, int _Quantity, out Container? _CreatedContainer, out string? _ErrorMessage)
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

            if (CreationSuccess)
            {
                _CreatedContainer = new Container(_ContainerName, _Description, _Value, _Quantity);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        internal new bool TryModify(string _NewContainerName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage) //new is needed to suppress the warning that we are overwriting the base method
        {
            return base.TryModify(_NewContainerName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
        }

        public int TotalItemCount()
        {
            return ContainerStorage.TotalItemCount();
        }
        public double TotalItemValue()
        {
            return ContainerStorage.TotalItemValue();
        }
        //public void AddItem(IStored _ItemToAdd)
        //{
        //    ContainerStorage.AddItem(_ItemToAdd);
        //}

        //public void RemoveItem(IStored _ItemToRemove)
        //{
        //    ContainerStorage.RemoveItem(_ItemToRemove);
        //}
        //public void MoveItem(IStored _ItemToMove, IStorage _Destination)
        //{
        //    ContainerStorage.MoveItem(_ItemToMove, _Destination);
        //}

    }
}
