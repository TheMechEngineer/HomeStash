using BackEnd.ModelInterfaces;
using System.Text.Json.Serialization;

namespace BackEnd.ModelClasses
{
    /// <summary>
    /// Represents An Item That Can Be Stored,
    /// And Provides Validation And Event Notification For Changes
    /// </summary>
    public class Item : IStored
    {
        /// <summary>
        /// Event Triggered When The Item Text Changes
        /// </summary>
        public event Action? TextChanged;

        /// <summary>
        /// Event Triggered When The Item Value Changes
        /// </summary>
        public event Action? ValueChanged;

        /// <summary>
        /// Event Triggered When The Item Quantity Changes
        /// </summary>
        public event Action? QuantityChanged;

        /// <summary>
        /// Event Triggered When The Immediate Parent Changes
        /// </summary>
        public event Action? ImmediateParentChanged;

        /// <summary>
        /// Unique Identifier For The Item
        /// </summary>
        //public int ID { get; set; }

        /// <summary>
        /// The Name Of The Item
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The Description Of The Item
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The Monetary Value Of The Item
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// The Quantity Of The Item
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// The Immediate Parent Storage Holder Containing This Item
        /// </summary>
        [JsonIgnore] // Excludes Immediate Parent From JSON To Prevent Circular References During Serialization
        public IStorageHolder ImmediateParent { get; internal set; }

        /// <summary>
        /// Protected Constructor Used For Controlled Creation Of Item Objects
        /// </summary>
        protected Item(string _ItemName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent)
        {
            Name = _ItemName;
            Description = _Description;
            Value = _Value;
            Quantity = _Quantity;
            ImmediateParent = _ImmediateParent;
        }

        /// <summary>
        /// Attempts To Create A New Item With Validation.
        /// Only Available Source To Create An Item Instance
        /// </summary>
        internal static bool TryCreate(string _ItemName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent, out Item? _CreatedItem, out string? _ErrorMessage)
        {
            _CreatedItem = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            // Runs Item Name Self-Validation
            if (!NameSelfValidation(_ItemName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Item Value Self-Validation
            if (!ValueSelfValidation(_Value, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Item Quantity Self-Validation
            if (!QuantitySelfValidation(_Quantity, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Immediate Parent Self-Validation
            if (!ImmediateParentSelfValidation(_ImmediateParent, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Creates New Item Instance If Item Passes Self-Validation Checks
            if (CreationSuccess)
            {
                _CreatedItem = new Item(_ItemName, _Description, _Value, _Quantity, _ImmediateParent);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        /// <summary>
        /// Attempts To Modify Item Properties With Validation
        /// </summary>
        internal bool TryModify(string _NewItemName, string _NewDescription, double _NewValue, int _NewQuantity, IStorageHolder _NewImmediateParent, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            bool TextChanged = this.Name != _NewItemName || this.Description != _NewDescription;
            bool ValueChanged = this.Value != _NewValue;
            bool QuantityChanged = this.Quantity != _NewQuantity;
            bool ImmediateParentChanged = this.ImmediateParent != _NewImmediateParent;

            // Checks If Any Fields Have Been Modified
            if (TextChanged || ValueChanged || QuantityChanged || ImmediateParentChanged)
            {
                if (TextChanged)
                {
                    // Runs Item Name Self-Validation If Text Was Changed
                    if (!NameSelfValidation(_NewItemName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                // Runs Item Value Self-Validation If Value Was Changed
                if (ValueChanged)
                {
                    if (!ValueSelfValidation(_NewValue, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                // Runs Item Quantity Self-Validation If Quantity Was Changed
                if (QuantityChanged)
                {
                    if (!QuantitySelfValidation(_NewQuantity, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                // Runs Immediate Parent Self-Validation If Parent Was Changed
                if (ImmediateParentChanged)
                {
                    if (!ImmediateParentSelfValidation(_NewImmediateParent, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }
            }
            else
            {
                ModifySuccess = false;
                _ErrorMessage += $"No Item Fields Have Been Modified\n";
            }

            // Modifies Item Fields If Item Passes Self-Validation Checks
            if (ModifySuccess)
            {
                this.Name = _NewItemName;
                this.Description = _NewDescription;
                this.Value = _NewValue;
                this.Quantity = _NewQuantity;
                this.ImmediateParent = _NewImmediateParent;
                if (TextChanged)
                {
                    this.TextChanged?.Invoke();
                }

                if (ValueChanged)
                {
                    this.ValueChanged?.Invoke();
                }

                if (QuantityChanged)
                {
                    this.QuantityChanged?.Invoke();
                }

                if (ImmediateParentChanged)
                {
                    this.ImmediateParentChanged?.Invoke();
                }
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifySuccess;
        }

        /// <summary>
        /// Validates Item Name Self Requirements
        /// </summary>
        protected static bool NameSelfValidation(string _ItemName, ref string? _ErrorMessage)
        {
            bool ItemNameValid = true;

            // Validates The Name Entered Was Not Empty
            if (string.IsNullOrEmpty(_ItemName))
            {
                _ErrorMessage += "Self Validation Error: Item Name Must Contain Characters\n";
                ItemNameValid = false;
            }

            return ItemNameValid;
        }

        /// <summary>
        /// Validates Item Value Self Requirements
        /// </summary>
        protected static bool ValueSelfValidation(double _Value, ref string? _ErrorMessage)
        {
            bool ValueValid = true;

            // Validates The Value Is Not Negative
            if (_Value < 0)
            {
                _ErrorMessage += "Self Validation Error: Value Cannot Be Negative\n";
                ValueValid = false;
            }

            return ValueValid;
        }

        /// <summary>
        /// Validates Item Quantity Self Requirements
        /// </summary>
        protected static bool QuantitySelfValidation(int _Quantity, ref string? _ErrorMessage)
        {
            bool QuantityValid = true;

            // Validates The Quantity Is At Least 1
            if (_Quantity <= 0)
            {
                _ErrorMessage += "Self Validation Error: Quantity Must Be At Least 1\n";
                QuantityValid = false;
            }

            return QuantityValid;
        }

        /// <summary>
        /// Validates Immediate Parent Self Requirements
        /// </summary>
        protected static bool ImmediateParentSelfValidation(IStorageHolder _ImmediateParent, ref string? _ErrorMessage)
        {
            bool ImmediateParentValid = true;

            // Validates The Immediate Parent Is Not Null
            if (_ImmediateParent == null)
            {
                _ErrorMessage += "Self Validation Error: Must Select A Valid Parent\n";
                ImmediateParentValid = false;
            }

            return ImmediateParentValid;
        }
    }
}
