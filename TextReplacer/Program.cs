using System;
using System.IO;
using TextReplacer.Components;

namespace TextReplacer.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Specify the directory as the first parameter!");
                return;
            }

            var dir = args[0];
            if (!Directory.Exists(dir))
            {
                Console.WriteLine($"Directory {dir} does not exists.");
                return;
            }
            
            var directoryHandler = new DirectoryHandler(dir);
            var numberOfFiles = directoryHandler.Process();
            Console.WriteLine($"Directory {dir} is processed (changed {numberOfFiles} file(s).");
        }
    }
}
