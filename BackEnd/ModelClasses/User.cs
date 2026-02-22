using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class User
    {
        public event Action? ActiveBuildingChanged;
        public event Action? BuildingListChanged;

        public string UserName { get; set; }

        private List<Building> __BuildingList = new List<Building>();

        private Building? __ActiveBuilding;

        public Building? ActiveBuilding
        {
            get
            {
                return __ActiveBuilding;
            }

            set
            {
                __ActiveBuilding = value;
                ActiveBuildingChanged?.Invoke();
            }
        }

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
            BuildingListChanged?.Invoke();
        }

        public void RemoveBuilding(Building _BuildingToRemove)
        {
            __BuildingList.Remove(_BuildingToRemove);
            BuildingListChanged?.Invoke();
        }
        public void CopyBuilding(Building _BuildingToCopy)
        {
            //stub
        }
        
    }
}
