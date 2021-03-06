﻿using System;
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
            Console.WriteLine("Press A for class writer and B for class fixer: ");

            ConsoleKeyInfo num = Console.ReadKey();

            if (num.Key == ConsoleKey.A)
            {
                Console.WriteLine("LeagueSandbox-Default Location || classType(static etc....) || removeFilesBeforeWriting(true, false)");
                string[] input = Console.ReadLine().Split(' ');
                ClassWriter classWriter = new ClassWriter(input[0], input[1], input[2]);
                classWriter.Write();
            } else
            {
                Console.WriteLine("\nInput LeagueSandbox-Default directory: ");
                String directory = Console.ReadLine();
                ClassFixer classFixer = new ClassFixer(directory);
            }
            
        }
    }
}
