using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal static class SavedOperations 
    {

        private static readonly List<string> _list = new();
        internal static IReadOnlyList<string> List => _list;

        internal static void Add(string item) => _list.Insert(0,item);
        
        // Remove item by value
        internal static bool Remove(string item) => _list.Remove(item);
        

        // Remove item by index
        internal static void RemoveAt(int index) => _list.RemoveAt(index);
        
        // Optional: Clear all items
        internal static void Clear() => _list.Clear();
 

}
}

