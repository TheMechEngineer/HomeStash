using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelInterfaces
{
    public interface IStorageHolder
    {
        public IStorage Storage { get; }
        public IReadOnlyList<IStored> StoredItems { get; }

    }

}
