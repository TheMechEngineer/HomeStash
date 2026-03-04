using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Adapters
{
    internal class AdapterSelectionItem
    {
        internal string DisplayText { get; }
        internal object Value { get; }

        internal AdapterSelectionItem(string _DisplayText, object _Value)
        {
            DisplayText = _DisplayText;
            Value = _Value;
        }

    }
}
