namespace GradeScores
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Error: Input file name is missing.");
                Console.WriteLine("grade-scores <InputFileFullPath>");
                return;
            }

            var input = args[0];
            if (!File.Exists(input))
            {
                Console.WriteLine(@"Input file ""{0}"" not found!", input);
                return;
            }

            var output = GetOutputFilePath(input);

            var sortTextFile = new SortTextFile();
            var res = sortTextFile.SortFile(input, output, 1024*1024*1024);
            if (!res)
            {
                Console.WriteLine("Error {0}", sortTextFile.Error);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Finished: created {0}", output);
                Console.ReadLine();
            }
        }

        private static string GetOutputFilePath(string inputFile)
        {
            var filePath = Path.GetDirectoryName(inputFile);
            var fileName = Path.GetFileNameWithoutExtension(inputFile);
            var fileExt = Path.GetExtension(inputFile);
            return Path.Combine(filePath, string.Format("{0}-graded{1}", fileName, fileExt));
        }
    }
}
