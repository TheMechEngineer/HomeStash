using BackEnd.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Item : IStored
    {
        public event Action? TextChanged;
        public event Action? ValueChanged;
        public event Action? QuantityChanged;
        public event Action? ImmediateParentChanged;

        //public int ID { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Value { get; private set; }
        public int Quantity { get; private set; }
        [JsonIgnore]
        public IStorageHolder ImmediateParent { get; internal set; }

        protected Item(string _ItemName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent)
        {
            Name = _ItemName;
            Description = _Description;
            Value = _Value;
            Quantity = _Quantity;
            ImmediateParent = _ImmediateParent;
        }

        internal static bool TryCreate(string _ItemName, string _Description, double _Value, int _Quantity, IStorageHolder _ImmediateParent, out Item? _CreatedItem, out string? _ErrorMessage)
        {
            _CreatedItem = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (!NameSelfValidation(_ItemName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!ValueSelfValidation(_Value, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!QuantitySelfValidation(_Quantity, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!ImmediateParentSelfValidation(_ImmediateParent, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

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

        internal bool TryModify(string _NewItemName, string _NewDescription, double _NewValue, int _NewQuantity, IStorageHolder _NewImmediateParent, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            bool TextChanged = this.Name != _NewItemName || this.Description != _NewDescription;
            bool ValueChanged = this.Value != _NewValue;
            bool QuantityChanged = this.Quantity != _NewQuantity;
            bool ImmediateParentChanged = this.ImmediateParent != _NewImmediateParent;

            if (TextChanged || ValueChanged || QuantityChanged || ImmediateParentChanged)
            {
                if (TextChanged)
                {
                    if (!NameSelfValidation(_NewItemName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                if (ValueChanged)
                {
                    if (!ValueSelfValidation(_NewValue, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                if (QuantityChanged)
                {
                    if (!QuantitySelfValidation(_NewQuantity, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

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

        protected static bool NameSelfValidation(string _ItemName, ref string? _ErrorMessage)
        {
            bool ItemNameValid = true;

            if (string.IsNullOrEmpty(_ItemName))
            {
                _ErrorMessage += "Self Validation Error: Item Name Must Contain Characters\n";
                ItemNameValid = false;
            }

            return ItemNameValid;
        }

        protected static bool ValueSelfValidation(double _Value, ref string? _ErrorMessage)
        {
            bool ValueValid = true;

            if (_Value < 0)
            {
                _ErrorMessage += "Self Validation Error: Value Cannot Be Negative\n";
                ValueValid = false;
            }

            return ValueValid;
        }

        protected static bool QuantitySelfValidation(int _Quantity, ref string? _ErrorMessage)
        {
            bool QuantityValid = true;

            if (_Quantity <= 0)
            {
                _ErrorMessage += "Self Validation Error: Quantity Must Be At Least 1\n";
                QuantityValid = false;
            }

            return QuantityValid;
        }

        protected static bool ImmediateParentSelfValidation(IStorageHolder _ImmediateParent, ref string? _ErrorMessage)
        {
            bool ImmediateParentValid = true;

            if (_ImmediateParent == null)
            {
                _ErrorMessage += "Self Validation Error: Must Select A Valid Parent\n";
                ImmediateParentValid = false;
            }

            return ImmediateParentValid;
        }
    }
}
