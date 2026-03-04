using BackEnd.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Item : IStored
    {
        //public int ID { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Value { get; private set; }
        public int Quantity { get; private set; }

        protected Item(string _ItemName, string _Description, double _Value, int _Quantity)
        {
            Name = _ItemName;
            Description = _Description;
            Value = _Value;
            Quantity = _Quantity;
        }

        internal static bool TryCreate(string _ItemName, string _Description, double _Value, int _Quantity, out Item? _CreatedItem, out string? _ErrorMessage)
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

            if (CreationSuccess)
            {
                _CreatedItem = new Item(_ItemName, _Description, _Value, _Quantity);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        internal bool TryModify(string _NewItemName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            if (this.Name != _NewItemName || this.Description != _NewDescription || this.Value != _NewValue || this.Quantity != _NewQuantity)
            {
                if (this.Name != _NewItemName)
                {
                    if (!NameSelfValidation(_NewItemName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                if (this.Value != _NewValue)
                {
                    if (!ValueSelfValidation(_NewValue, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                if (this.Quantity != _NewQuantity)
                {
                    if (!QuantitySelfValidation(_NewQuantity, ref _ErrorMessage))
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
                _ErrorMessage += "Item Name Must Contain Characters\n";
                ItemNameValid = false;
            }

            return ItemNameValid;
        }

        protected static bool ValueSelfValidation(double _Value, ref string? _ErrorMessage)
        {
            bool ValueValid = true;

            if (_Value < 0)
            {
                _ErrorMessage += "Value Cannot Be Negative\n";
                ValueValid = false;
            }

            return ValueValid;
        }

        protected static bool QuantitySelfValidation(int _Quantity, ref string? _ErrorMessage)
        {
            bool QuantityValid = true;

            if (_Quantity <= 0)
            {
                _ErrorMessage += "Quantity Must Be At Least 1\n";
                QuantityValid = false;
            }

            return QuantityValid;
        }

    }
}
