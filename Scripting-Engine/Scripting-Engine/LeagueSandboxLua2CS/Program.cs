using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSandboxLua2CS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LeagueSandbox-Default Location || classType(static etc....)");
            string[] input = Console.ReadLine().Split(' ');
            ClassWriter classWriter = new ClassWriter(input[0], input[1]);
        }
    }
}
