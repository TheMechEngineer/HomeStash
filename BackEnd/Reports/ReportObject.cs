using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Reports
{
    internal class ReportObject
    {
        required internal string Name;
        required internal string Location;
        required internal string Description;
        required internal double Value;
        required internal int Quantity;
    }
}
