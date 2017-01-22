using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting_Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            CSharpScriptEngine scriptingEngine = new CSharpScriptEngine();
            scriptingEngine.prepareCompiler();

            scriptingEngine.load("LeagueSandbox-Default/Champions/Ezreal/Q.cs");

            Console.ReadKey();
        }
    }
}