using BackEnd.ModelClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Adapters
{
    internal class ASelection
    {
        internal event Action? SourceUpdated;

        internal Type SelectionType { get; }

        internal string ButtonText
        {
            get
            { return __ButtonText; }
        }

        private IReadOnlyList<object> SourceList;
        private List<ASelectionItem> ConvertedList = new List<ASelectionItem>();

        private readonly RootManager RootManagerInstance;
        private string __ButtonText;

        internal ASelection(ref RootManager _RootManagerInstance, IReadOnlyList<object> _SourceList, string _ButtonText)
        {
            __ButtonText = _ButtonText;
            SourceList = _SourceList;
            RootManagerInstance = _RootManagerInstance;

            SelectionType = SourceList.GetType().GetGenericArguments()[0];

            RefreshConvertedList();
            Wire();
        }

        private void Wire()
        {
            switch (SelectionType)
            {
                case Type CurrentType when SelectionType == typeof(User):
                    RootManagerInstance.UserListChanged += UpdateDependents;
                    break;
                case Type CurrentType when SelectionType == typeof(Building):
                    RootManagerInstance.ActiveUser.BuildingListChanged += UpdateDependents;
                    break;
            }
        }
        
        private void UpdateDependents()
        {
            SourceUpdated?.Invoke();
        }

        private void RefreshConvertedList()
        {
            ConvertedList.Clear();

            foreach (object CurrentObject in SourceList) {
                switch (SelectionType)
                {
                    case Type CurrentType when SelectionType == typeof(User):
                        User CurrentUser = CurrentObject as User;
                        ConvertedList.Add(new ASelectionItem(CurrentUser.Username, CurrentUser));
                        break;
                    case Type CurrentType when SelectionType == typeof(Building):
                        Building CurrentBuilding = CurrentObject as Building;
                        ConvertedList.Add(new ASelectionItem(CurrentBuilding.Name, CurrentBuilding));
                        break;
                }

            }

        }

        internal IReadOnlyList<ASelectionItem> GetAList()
        {
            RefreshConvertedList();
            return ConvertedList.AsReadOnly();
        }
    }
}
