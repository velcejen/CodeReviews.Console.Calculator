using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Enums
    {
        internal enum Perimeter
        {
            top = 1,
            left = 10,
            width = 31,
            height = 29 
        }
        internal enum Display
        {
            top = 10,
            left = 12,
            width = 27,
            height = 4
        }
        internal enum SavedCalculations
        {
            top = 3,
            left = 12,
            width = 27,
            height = 7
        }
        internal enum HelpTypes
        {
            CalculatorHelp=0,
            listHelp=1,
        }
    }
}

