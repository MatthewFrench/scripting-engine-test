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
        public CSharpScriptEngine()
        {
        }
        //Compile example assembly to load all the compiler resources, takes 1700 milliseconds first time
        public void prepareCompiler()
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
            using System;

            namespace LoadCompilerNamespace
            {
                public class LoadCompilerClass
                {
                    public void LoadCompilerFunction(string message)
                    {
                        Console.WriteLine(message);
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
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {

                Benchmark.StartTiming("Load Compiler");
                EmitResult result = compilation.Emit(ms);
                Benchmark.EndTiming("Load Compiler");
            }
        }
        public void load(String scriptLocation)
        {
            Benchmark.StartTiming("Load Function");
            String script = "";
            using (StreamReader sr = new StreamReader(scriptLocation))
            {
                // Read the stream to a string, and write the string to the console.
                script = sr.ReadToEnd();
            }
            Tuple<List<string>, string> scriptHoist = QuickUsings.Hoist(script);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(scriptHoist.Item2);
            string assemblyName = Path.GetRandomFileName();
            
            List<MetadataReference> references = new List<MetadataReference>();
            foreach (Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                references.Add(MetadataReference.CreateFromFile(a.Location));
            }
            //Now add game reference
            references.Add(MetadataReference.CreateFromFile(typeof(Game).Assembly.Location));
            
            var op = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: op);

            using (var ms = new MemoryStream())
            {
                Benchmark.StartTiming("Compiled class");
                EmitResult result = compilation.Emit(ms);
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
                    Assembly assembly = Assembly.Load(ms.ToArray());
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


            Benchmark.EndTiming("Load Function");
        }
    }
}
