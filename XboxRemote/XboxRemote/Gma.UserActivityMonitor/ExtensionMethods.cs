using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static bool Between(this int value, int left, int right)
        {
            return value > left && value < right;
        }
    }
}
