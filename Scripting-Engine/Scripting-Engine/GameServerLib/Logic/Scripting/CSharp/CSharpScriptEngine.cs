using LeagueSandbox.GameServer.Core.Logic;
using CSScriptLibrary;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LeagueSandbox.GameServer.Logic.Scripting.CSharp
{
    class CSharpScriptEngine : IScriptEngine
    {
        private Logger _logger = Program.ResolveDependency<Logger>();

        private bool _isLoaded;

        public CSharpScriptEngine()
        {
            //CSScript.
            /*
                var doMath = CSScript.CreateFunc<double>(@"double doMath(int a)
                                             {
                                                 return Math.Sin(10*10+7);
                                             }");
                val[i] = doMath(0);
             */
            /*
             dynamic script = CSScript.LoadCode(
                          @"using System.Windows.Forms;
                            public class Script
                            {
                                public void SayHello(string greeting)
                                {
                                    MessageBox.Show($""Greeting: {greeting}"");
                                }
                            }")
                            .CreateObject("*");
script.SayHello("Hello World!");
             */
            /*
             var SayHello = CSScript.LoadMethod(
                       @"using System.Windows.Forms;
                         public static void SayHello(string greeting)
                         {
                             MessageBoxSayHello(greeting);
                             ConsoleSayHello(greeting);
                         }
                         static void MessageBoxSayHello(string greeting)
                         {
                             MessageBox.Show(greeting);
                         }
                         static void ConsoleSayHello(string greeting)
                         {
                             Console.WriteLine(greeting);
                         }")
                        .GetStaticMethod("SayHello" , typeof(string)); 
SayHello("Hello again!");
             */
        }

        public bool IsLoaded()
        {
            return _isLoaded;
        }

        public void Load(string location)
        {
            _isLoaded = false;
            try
            {
                //var s = _lua.DoFile(location);
                _isLoaded = true;
            }
            catch (Exception e)
            {
                _logger.LogCoreError(e.Message);
            }
        }

        public void RegisterFunction(string path, object target, MethodBase function)
        {
            //_lua.RegisterFunction(path, target, function);
        }

        public void Execute(string script)
        {
            //_lua.DoString(script);
        }

        public void RunFunction(string function, params object[] args)
        {
            //_lua.GetFunction(function).Call(args);
        }

        public void SetGlobalVariable(string name, object value)
        {
            //_lua[name] = value;
        }
    }
}
