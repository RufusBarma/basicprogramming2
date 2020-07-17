using System;
using System.Collections.Generic;

namespace Delegate
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = new Func<int, int[]>[3];
            f[0] = (x) => new int[x];
            int result = (f[0](0))[0];
        }
    }
}
