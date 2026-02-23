using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Adapters
{
    internal class ASelectionItem
    {
        public string DisplayText { get; }
        public object Value { get; }

        public ASelectionItem(string _DisplayText, object _Value)
        {
            DisplayText = _DisplayText;
            Value = _Value;
        }

    }
}
