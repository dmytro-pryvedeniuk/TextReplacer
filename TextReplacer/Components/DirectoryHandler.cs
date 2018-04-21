using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace TextReplacer.Components
{
    /// <summary>
    /// Processes all files having extension 'xml', 'xsl' or 'xslt' in the working directory.
    /// </summary>
    public class DirectoryHandler
    {
        private string _workDir;
        private string[] _extensions = new[] { ".xml", ".xsl", ".xslt" };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workDir">Working directory to be processed (subfolders are not processed).</param>
        public DirectoryHandler(string workDir)
        {
            _workDir = workDir;
        }

        /// <summary>
        /// Processes all files in the working directory. Only the files with 'xml', 'xsl' or 'xslt' extensions
        /// are processed.
        /// </summary>
        /// <returns>The number of the processed files.</returns>
        public int Process()
        {
            if (!Directory.Exists(_workDir))
                return 0;

            var files = Directory.EnumerateFiles(_workDir, "*.*")
                .Where(s => _extensions.Contains(Path.GetExtension(s).ToLower()));

            var count = 0;

            foreach (var file in files.AsParallel())
            {
                var text = File.ReadAllText(file);
                var replacer = new Replacer();
                var replaceResult = replacer.Process(new ReplaceArgs {
                    Input = text
                });
                
                if (replaceResult.IsProcessed)
                {
                    File.Copy(file, Path.ChangeExtension(file, ".bak"));
                    File.WriteAllText(file, replaceResult.Output);
                    Interlocked.Add(ref count, 1);
                    continue;
                }

                if (replaceResult.Error != null)
                {
                    Console.WriteLine($"{file} can not be processed - {replaceResult.Error}");
                    continue;
                }
            }

            return count;
        }
    }
}