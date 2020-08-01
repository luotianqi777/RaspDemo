using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemo.Controllers
{
    public class Debuger
    {
        public static void WriteLine(object value)
        {
            Console.WriteLine(value.ToString());
        }
    }
}
