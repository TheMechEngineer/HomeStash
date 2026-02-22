using BackEnd.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Container : Item, IStorage
    {
        private Storage ContainerStorage = new Storage();
        public IReadOnlyList<IStored> StoredItems
        {
            get
            {
                return ContainerStorage.StoredItems;
            }

        }

        public int TotalItemCount()
        {
            return ContainerStorage.TotalItemCount();
        }
        public double TotalItemValue()
        {
            return ContainerStorage.TotalItemValue();
        }
        public void AddItem(IStored _ItemToAdd)
        {
            ContainerStorage.AddItem(_ItemToAdd);
        }

        public void RemoveItem(IStored _ItemToRemove)
        {
            ContainerStorage.RemoveItem(_ItemToRemove);
        }
        public void MoveItem(IStored _ItemToMove, IStorage _Destination)
        {
            ContainerStorage.MoveItem(_ItemToMove, _Destination);
        }

    }
}
