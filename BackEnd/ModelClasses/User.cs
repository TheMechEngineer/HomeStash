using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class User
    {
        public string UserName { get; set; }

        private List<Building> __BuildingList = new List<Building>();

        public Building ActiveBuilding { get; set; }

        public IReadOnlyList<Building> BuildingList
        {
            get
            {
                return __BuildingList.AsReadOnly();
            }

        }

        public void AddBuilding(Building _BuildingToAdd)
        {
            __BuildingList.Add(_BuildingToAdd);
        }

        public void RemoveBuilding(Building _BuildingToRemove)
        {
            __BuildingList.Remove(_BuildingToRemove);
        }
        public void CopyBuilding(Building _BuildingToCopy)
        {
            //stub
        }
        
    }
}
