using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;
using System.Text.Json.Serialization;

namespace BackEnd.ModelClasses
{
    /// <summary>
    /// Represents A Container Item That Can Store Other Items,
    /// And Provides Validation And Event Notification For Changes
    /// </summary>
    public class Container : Item, IStorageHolder
    {
        /// <summary>
        /// Pass Through Event For When Stored Items Change
        /// </summary>
        public event Action? StoredItemsChanged
        {
            add { ContainerStorage.StoredItemsChanged += value; }
            remove { ContainerStorage.StoredItemsChanged -= value; }
        }

        /// <summary>
        /// Pass Through Event For When Stored Item Is Modified
        /// </summary>
        public event Action? StoredItemModified
        {
            add { ContainerStorage.StoredItemModified += value; }
            remove { ContainerStorage.StoredItemModified -= value; }
        }

        /// <summary>
        /// The Storage Backer For Items Contained Directly Within The Container
        /// </summary>
        private Storage ContainerStorage;

        /// <summary>
        /// The Storage For Items Contained Directly Within The Container
        /// </summary>
        public IStorage CurrentStorage
        {
            get
            { return ContainerStorage; }
        }

        /// <summary>
        /// The List Of Items Directly Stored In The Container
        /// </summary>
        [JsonIgnore] // Excludes Stored Items From JSON To Prevent Circular References During Serialization
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return ContainerStorage.StoredItems; }
        }

        /// <summary>
        /// Private Constructor Used For Controlled Creation Of Container Objects
        /// </summary>
        /// <param name="_ItemName">The Name Of The Container</param>
        /// <param name="_Description">The Description Of The Container</param>
        /// <param name="_Value">The Monetary Value Of The Container</param>
        /// <param name="_Quantity">The Quantity Of The Container</param>
        /// <param name="_ImmediateParent">The Immediate Parent Of The Container</param>
        private Container(string _ItemName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent)
            : base(_ItemName, _Description, _Value, _Quantity, _ImmediateParent)
        {
            ContainerStorage = new Storage(this);
        }

        /// <summary>
        /// Attempts To Create A New Container With Validation.
        /// Only Available Source To Create A Container Instance
        /// </summary>
        /// <param name="_ContainerName">The Proposed Name Of The Container</param>
        /// <param name="_Description">The Proposed Description Of The Container</param>
        /// <param name="_Value">The Proposed Monetary Value Of The Container</param>
        /// <param name="_Quantity">The Proposed Quantity Of The Container</param>
        /// <param name="_ImmediateParent">The Proposed Immediate Parent Of The Container</param>
        /// <param name="_CreatedContainer">The Container Instance Created If Successful</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal static bool TryCreate(string _ContainerName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent, out Container? _CreatedContainer, out string? _ErrorMessage)
        {
            _CreatedContainer = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            // Runs Container Name Self-Validation
            if (!NameSelfValidation(_ContainerName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Container Value Self-Validation
            if (!ValueSelfValidation(_Value, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Container Quantity Self-Validation
            if (!QuantitySelfValidation(_Quantity, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Immediate Parent Self-Validation
            if (!ImmediateParentSelfValidation(_ImmediateParent, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Creates New Container Instance If Container Passes Self-Validation Checks
            if (CreationSuccess)
            {
                _CreatedContainer = new Container(_ContainerName, _Description, _Value, _Quantity, _ImmediateParent);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        /// <summary>
        /// Attempts To Modify Container Properties With Validation
        /// </summary>
        /// <param name="_NewContainerName">The Proposed Name Of The Container</param>
        /// <param name="_NewDescription">The Proposed Description Of The Container</param>
        /// <param name="_NewValue">The Proposed Monetary Value Of The Container</param>
        /// <param name="_NewQuantity">The Proposed Quantity Of The Container</param>
        /// <param name="_NewImmediateParent">The Proposed Immediate Parent Of The Container</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal new bool TryModify(string _NewContainerName, string _NewDescription, double _NewValue, int _NewQuantity, IStorageHolder _NewImmediateParent, out string? _ErrorMessage) //new is needed to suppress the warning that we are overwriting the base method
        {
            return base.TryModify(_NewContainerName, _NewDescription, _NewValue, _NewQuantity, _NewImmediateParent, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Add A Stored Item To The Container Storage
        /// </summary>
        /// <param name="_StoredType">The Type Of The Proposed IStored</param>
        /// <param name="_StoredName">The Proposed Name Of The IStored</param>
        /// <param name="_Description">The Proposed Description Of The IStored</param>
        /// <param name="_Value">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_Quantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            return ContainerStorage.TryAddIStored(_StoredType, _StoredName, _Description, _Value, _Quantity, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Modify A Stored Item In The Container Storage
        /// </summary>
        /// <param name="_IStoredToModify">The IStored Instance To Attempt To Modify</param>
        /// <param name="_NewStoredName">The Proposed Name Of The IStored</param>
        /// <param name="_NewDescription">The Proposed Description Of The IStored</param>
        /// <param name="_NewValue">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_NewQuantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            return ContainerStorage.TryModifyIStored(_IStoredToModify, _NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Move A Stored Item From The Current Storage To Another Storage
        /// </summary>
        /// <param name="_ItemToMove">The IStored Instance To Attempt To Move</param>
        /// <param name="_Destination">The Proposed Destination Storage Holder</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage)
        {
            return ContainerStorage.TryMoveIStored(_ItemToMove, _Destination, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Remove A Stored Item From The Container Storage
        /// </summary>
        /// <param name="_StoredToRemove">The IStored Instance To Attempt To Remove</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage)
        {
            return ContainerStorage.TryRemoveIStored(_StoredToRemove, out _ErrorMessage);
        }

        /// <summary>
        /// Calculates The Total Number Of Items In The Container Including Nested Items
        /// </summary>
        public int TotalItemCount()
        {
            return (ContainerStorage.TotalItemCount() + 1) * this.Quantity;
        }

        /// <summary>
        /// Calculates The Total Value Of Items In The Container Including Nested Items
        /// </summary>
        public double TotalItemValue()
        {
            return (ContainerStorage.TotalItemValue() + this.Value) * this.Quantity;
        }
    }
}
