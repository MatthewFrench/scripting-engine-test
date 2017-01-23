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

            scriptingEngine.load(new List<String>{ "LeagueSandbox-Default/Champions/Ezreal/Q.cs",
                                                   "LeagueSandbox-Default/Champions/Ezreal/EObject.cs"});

            Action<double> ezrealQOnUpdate = scriptingEngine.getStaticMethod<Action<double>>("Ezreal", "Q", "onUpdate");
            ezrealQOnUpdate( 50.0 );

            Func<double> ezrealQFakeFunction = scriptingEngine.getStaticMethod<Func<double>>("Ezreal", "Q", "fakeFunc");
            double number = ezrealQFakeFunction();

            ISpellScript ezrealEObject = scriptingEngine.createObject<ISpellScript>("Ezreal", "EObject");
            ezrealEObject.onStartCasting(null);

            Console.ReadKey();
        }
    }
}