using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LeagueSandbox.GameServer.Core.Logic;
using System.Numerics;

namespace Scripting_Engine
{
    public class CSharpScriptEngine
    {
        Assembly scriptAssembly = null;
        public CSharpScriptEngine()
        {
        }
        //Compile example assembly to load all the compiler resources, takes 1700 milliseconds first time
        public void prepareCompiler()
        {
            Benchmark.StartTiming("Prepare Compiler");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
            using System;

            namespace LoadCompilerNamespace
            {
                public class LoadCompilerClass
                {
                    public void LoadCompilerFunction(string message)
                    {
                        Benchmark.Log(message);
                    }
                }
            }");
            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release).WithConcurrentBuild(true));

            using (var ms = new MemoryStream())
            {
                ///using (var pdb = new MemoryStream())
                //{
                EmitResult result = compilation.Emit(ms);//, pdb);
                //}
            }
            Benchmark.EndTiming("Prepare Compiler");
        }
        public void loadSubdirectoryScripts(String folder)
        {
            String[] allfiles = System.IO.Directory.GetFiles(folder, "*.cs", System.IO.SearchOption.AllDirectories);
            load(new List<string>(allfiles));
        }
        //Takes about 300 milliseconds for a single script
        public void load(List<string> scriptLocations)
        {

            Benchmark.StartTiming("Script Loading");
            List<SyntaxTree> treeList = new List<SyntaxTree>();
            Parallel.For(0, scriptLocations.Count, (i)=> {
                using (StreamReader sr = new StreamReader(scriptLocations[i]))
                {
                    // Read the stream to a string, and write the string to the console.
                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sr.ReadToEnd());
                    lock (treeList)
                    {
                        treeList.Add(syntaxTree);
                    }
                }
            });
            Benchmark.EndTiming("Script Loading");
            
            string assemblyName = Path.GetRandomFileName();

            Benchmark.StartTiming("Reference Loading");
            List<MetadataReference> references = new List<MetadataReference>();
            foreach (Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                references.Add(MetadataReference.CreateFromFile(a.Location));
            }
            //Now add game reference
            references.Add(MetadataReference.CreateFromFile(typeof(Game).Assembly.Location));
            Benchmark.EndTiming("Reference Loading");

            var op = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release).WithConcurrentBuild(true);
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: treeList,
                references: references,
                options: op);

            using (var ms = new MemoryStream())
            {
                //using (var pdb = new MemoryStream())
                {
                    Benchmark.StartTiming("Compiled class");
                    EmitResult result = compilation.Emit(ms);//, pdb);
                    Benchmark.EndTiming("Compiled class");
                    
                    if (!result.Success)
                    {
                        IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                            diagnostic.IsWarningAsError ||
                            diagnostic.Severity == DiagnosticSeverity.Error);

                        foreach (Diagnostic diagnostic in failures)
                        {
                            Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                        }
                    }
                    else
                    {
                        ms.Seek(0, SeekOrigin.Begin);

                        Benchmark.StartTiming("Loaded assembly");
                        scriptAssembly = Assembly.Load(ms.ToArray());
                        Benchmark.EndTiming("Loaded assembly");

                        /*
                        Type type = assembly.GetType("RoslynCompileSample.Writer");
                        object obj = Activator.CreateInstance(type);
                        type.InvokeMember("Write",
                            BindingFlags.Default | BindingFlags.InvokeMethod,
                            null,
                            obj,
                            new object[] { "Hello World" });
                            */
                    }
                }
            }

            
        }
        public T getStaticMethod<T>(String scriptNamespace, String scriptClass, String scriptFunction)
        {
            if (scriptAssembly == null) return default(T);

            Type classType = scriptAssembly.GetType(scriptNamespace + "." + scriptClass);
            MethodInfo desiredFunction = classType.GetMethod(scriptFunction, BindingFlags.Public | BindingFlags.Static);

            Type typeParameterType = typeof(T);
            return (T)((object)Delegate.CreateDelegate(typeParameterType, desiredFunction));
        }
        public T createObject<T>(String scriptNamespace, String scriptClass)
        {
            if (scriptAssembly == null) return default(T);


            Benchmark.StartTiming("Script Assembly Type Lookup");
            Type classType = scriptAssembly.GetType(scriptNamespace + "." + scriptClass);
            Benchmark.EndTiming("Script Assembly Type Lookup");

            return (T)Activator.CreateInstance(classType);
        }
        public static object runFunctionOnObject(object obj, String method, params object[] args)
        {
            return obj.GetType().InvokeMember(method,
                            BindingFlags.Default | BindingFlags.InvokeMethod,
                            null,
                            obj,
                            args);
        }
        public static T getObjectMethod<T>(object obj, String scriptFunction)
        {
            Type classType = obj.GetType();
            MethodInfo desiredFunction = classType.GetMethod(scriptFunction, BindingFlags.Public | BindingFlags.Instance);

            Type typeParameterType = typeof(T);
            return (T)((object)Delegate.CreateDelegate(typeParameterType, obj, desiredFunction));
        }
        /*
        public void runFunction(String scriptNamespace, String scriptClass, String scriptFunction, params object[] args)
        {
            if (scriptAssembly == null) return;
            
            Benchmark.Log("Script type: " + scriptAssembly.GetType(scriptNamespace + "." + scriptClass).FullName);
            foreach (MethodInfo m in scriptAssembly.GetType(scriptNamespace + "." + scriptClass).GetMethods())
            {
                Benchmark.Log("Method: " + m.Name);
            }
            Benchmark.Log("Script method: " + scriptAssembly.GetType(scriptNamespace + "." + scriptClass).GetMethod(scriptFunction, BindingFlags.Public | BindingFlags.Static).ToString());

            Type classType = scriptAssembly.GetType(scriptNamespace + "." + scriptClass);
            MethodInfo desiredFunction = classType.GetMethod(scriptFunction, BindingFlags.Public | BindingFlags.Static);
            //desiredFunction.Invoke(null, args);

            Action<double> functionDelegate = (Action<double>)Delegate.CreateDelegate
                (typeof(Action<double>), desiredFunction);

            Benchmark.StartTiming("Running Function");
            functionDelegate();
            Benchmark.EndTiming("Running Function");
        }
        */
        //void RunFunction(string function, params object[] args);
    }
}
