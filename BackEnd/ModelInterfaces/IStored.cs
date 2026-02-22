using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BackEnd.ModelClasses;

namespace BackEnd.ModelInterfaces
{
    public interface IStored
    {
        /// <summary>
        /// Unique Identifier For The Item
        /// </summary>
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public int Quantity { get; set; }
        public IStorage ImmediateParent { get; set; }
        public Room RoomParent { get; set; }

        //Need to add actual objects that are the parents
    }
}
