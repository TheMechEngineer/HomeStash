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
        public IReadOnlyList<IStored> StoredItems { get; }
        public int TotalItemCount();
        public double TotalItemValue();
        public void AddItem(IStored _ItemToAdd);
        public void RemoveItem(IStored _ItemToRemove);
        public void MoveItem(IStored _ItemToMove, IStorage _Destination);

    }
}
