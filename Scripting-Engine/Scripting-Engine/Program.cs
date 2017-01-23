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
            Benchmark.StartTiming("Program Execution Time");

            CSharpScriptEngine scriptingEngine = new CSharpScriptEngine();
            scriptingEngine.prepareCompiler();

            scriptingEngine.loadSubdirectoryScripts("LeagueSandbox-Default/");

            Action<double> ezrealQOnUpdate = scriptingEngine.getStaticMethod<Action<double>>("Ezreal", "Q", "onUpdate");
            ezrealQOnUpdate( 50.0 );

            Func<double> ezrealQFakeFunction = scriptingEngine.getStaticMethod<Func<double>>("Ezreal", "Q", "fakeFunc");
            double number = ezrealQFakeFunction();

            ISpellScript ezrealEObject = scriptingEngine.createObject<ISpellScript>("Ezreal", "EObject");
            ezrealEObject.onStartCasting(null);

            Benchmark.EndTiming("Program Execution Time");

            Console.ReadKey();
        }
    }
}