using LeagueSandbox.GameServer.Logic.GameObjects;
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

            //Removed function
            //Func<double> ezrealQFakeFunction = scriptingEngine.getStaticMethod<Func<double>>("Ezreal", "Q", "fakeFunc");
            //double number = ezrealQFakeFunction();

            ISpellScript ezrealEObject = scriptingEngine.createObject<ISpellScript>("Ezreal", "EObject");
            ezrealEObject.onStartCasting(null);
            
            object ezrealEObjectGeneric = scriptingEngine.createObject<object>("Ezreal", "EObject");
            CSharpScriptEngine.runFunctionOnObject(ezrealEObjectGeneric, "onStartCasting", new object[] { null });

            Action<Champion> onObjectStartCasting = CSharpScriptEngine.getObjectMethod<Action<Champion>>(ezrealEObjectGeneric, "onStartCasting");
            onObjectStartCasting(null);

            Benchmark.EndTiming("Program Execution Time");

            Console.ReadKey();
        }
    }
}