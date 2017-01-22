using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting_Engine
{
    static public class Benchmark
    {
        static IDictionary<string, Stopwatch> map = new Dictionary<string, Stopwatch>();
        static public void StartTiming(String label)
        {
            Stopwatch stopwatch = new Stopwatch();
            map[label] = stopwatch;
            stopwatch.Reset();
            stopwatch.Start();
        }
        static public void EndTiming(String label)
        {
            Stopwatch stopwatch = map[label];
            stopwatch.Stop();
            Task t = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("{0} Elapsed(MS) = {1} - FPS: {2}", label, stopwatch.ElapsedMilliseconds, 1000.0 / stopwatch.ElapsedMilliseconds);
            });
            map.Remove(label);
        }
    }
}
