using BackEnd.ModelClasses;

namespace FrontEnd.Adapters
{
    /// <summary>
    /// Adapter That Converts A Source List Of Objects Into A Standardized List For The Selection Control
    /// </summary>
    internal class AdapterSelection
    {
        /// <summary>
        /// Event Triggered When The Source Data Is Updated
        /// </summary>
        internal event Action? SourceUpdated;

        /// <summary>
        /// Gets The Type Of Objects Contained In The Source List
        /// </summary>
        internal Type SelectionType { get; }

        /// <summary>
        /// Backing Field For ButtonText Property
        /// </summary>
        private string __ButtonText;

        /// <summary>
        /// Gets The Display Text Used For Selection Buttons
        /// </summary>
        internal string ButtonText
        {
            get
            { return __ButtonText; }
        }

        /// <summary>
        /// The Original Source List Of Objects To Be Adapted
        /// </summary>
        private IReadOnlyList<object> SourceList;

        /// <summary>
        /// The Converted List Used For Display In The Selection Control
        /// </summary>
        private List<AdapterSelectionItem> ConvertedList = new List<AdapterSelectionItem>();

        /// <summary>
        /// The Root Manager Instance For Live Data
        /// </summary>
        private readonly RootManager RootManagerInstance;

        /// <summary>
        /// Initializes The AdapterSelection With Source Data And Display Configuration
        /// </summary>
        /// <param name="_RootManagerInstance">The Root Manager Used For Event Wiring</param>
        /// <param name="_SourceList">The Source List Of Objects To Adapt</param>
        /// <param name="_ButtonText">The Text Used For Display In Selection Controls</param>
        internal AdapterSelection(ref RootManager _RootManagerInstance, IReadOnlyList<object> _SourceList, string _ButtonText)
        {
            __ButtonText = _ButtonText;
            SourceList = _SourceList;
            RootManagerInstance = _RootManagerInstance;

            SelectionType = SourceList.GetType().GetGenericArguments()[0];

            RefreshConvertedList();
            Wire();
        }

        /// <summary>
        /// Wires Root Manager Events To Control Handlers
        /// </summary>
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

        /// <summary>
        /// Handles Source Data Changes And Notifies Dependent Controls
        /// </summary>
        private void UpdateDependents()
        {
            SourceUpdated?.Invoke();
        }

        /// <summary>
        /// Refreshes The Converted List Based On The Current Source Data
        /// </summary>
        private void RefreshConvertedList()
        {
            ConvertedList.Clear();

            foreach (object CurrentObject in SourceList)
            {
                // Use Different Properties Of Different Source Objects, To Create Each AdapterSelectionItem In The List
                switch (SelectionType)
                {
                    case Type CurrentType when SelectionType == typeof(User):
                        User CurrentUser = CurrentObject as User;
                        ConvertedList.Add(new AdapterSelectionItem(CurrentUser.Username, CurrentUser));
                        break;
                    case Type CurrentType when SelectionType == typeof(Building):
                        Building CurrentBuilding = CurrentObject as Building;
                        ConvertedList.Add(new AdapterSelectionItem(CurrentBuilding.Name, CurrentBuilding));
                        break;
                }
            }
        }

        /// <summary>
        /// Returns The Converted List As A ReadOnly Collection For Display
        /// </summary>
        /// <returns>A ReadOnly List Of AdapterSelectionItem Objects</returns>
        internal IReadOnlyList<AdapterSelectionItem> GetAList()
        {
            RefreshConvertedList();
            return ConvertedList.AsReadOnly();
        }
    }
}
