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

            scriptingEngine.load(new List<String>{ "LeagueSandbox-Default/Champions/Ezreal/Q.cs" });

            scriptingEngine.runFunction("Ezreal", "Q", "onUpdate", new object[] { 50.0 });

            Console.ReadKey();
        }
    }
}