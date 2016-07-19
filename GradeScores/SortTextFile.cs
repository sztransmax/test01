namespace GradeScores
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class SortTextFile
    {
        public string Error { get; set; }

        private IEnumerable<string> SplitFile(string filepath, long chunkSize)
        {
            var buffer = new List<string>();
            var size = 0L;

            using (var reader = new StreamReader(filepath))
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (size + line.Length + 2 >= chunkSize)
                    {
                        size = 0L;
                        yield return CreateChunkFile(buffer);
                    }

                    size += line.Length + 2;
                    buffer.Add(line);
                }

            if (buffer.Any())
            {
                yield return CreateChunkFile(buffer);
            }
        }

        private bool IsLineValid(string line)
        {
            int value = 0;
            return (!string.IsNullOrEmpty(line)) && (line.Split(',').Length == 3) && (int.TryParse(line.Split(',')[2], out value));
        }

        private List<string> SortListByColumns(List<string> source)
        {
            return source
                .Where(line => IsLineValid(line))
                .Select(line => new StudentData(line))
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => x.RearrangedLine)
                .ToList();
        }

        private string RestoreString(string input)
        {
            var elements = input.Split(',').Select(p => p.Trim()).ToList();
            return elements[1] + ", " + elements[2] + ", " + elements[0];
        }

        private string CreateChunkFile(List<string> buffer)
        {
            buffer = SortListByColumns(buffer);
            var chunkFilePath = Path.GetTempFileName();

            try
            {
                File.WriteAllLines(chunkFilePath, buffer);
            }
            catch (Exception e)
            {
                Error = e.Message;
                return string.Empty;
            }

            buffer.Clear();
            return chunkFilePath;
        }

        private bool IsFileLocked(string filePath)
        {
            var file = new FileInfo(filePath);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        private bool KMerge(IEnumerable<string> chunkFilePaths, string resultFilePath)
        {
            if (File.Exists(resultFilePath) && IsFileLocked(resultFilePath))
            {
                Error = string.Format("Cannot overwrite output file {0}.", resultFilePath);
                return false;
            }

            var chunkReaders = chunkFilePaths
                .Select(path => new StreamReader(path))
                .Where(chunkReader => !chunkReader.EndOfStream)
                .ToList();

            var queue = new PriorityQueue<string, TextReader>((x, y) => -string.CompareOrdinal(x, y));
            chunkReaders.ForEach(chunkReader => queue.Enqueue(chunkReader.ReadLine(), chunkReader));

            try
            {
                using (var resultWriter = new StreamWriter(resultFilePath, false))
                    while (queue.Count > 0)
                    {
                        var smallest = queue.Dequeue();
                        var line = smallest.Key;
                        var chunkReader = smallest.Value;

                        resultWriter.WriteLine(RestoreString(line));

                        var nextLine = chunkReader.ReadLine();
                        if (nextLine != null)
                        {
                            queue.Enqueue(nextLine, chunkReader);
                        }
                        else
                        {
                            chunkReader.Dispose();
                        }
                    }
            }
            catch(Exception e)
            {
                Error = string.Format("Cannot create output file, {0}.", e.Message);
                return false;
            }
            return true;
        }

        private bool ValidateFiles(string inputFile, string outputFile)
        {
            if (string.IsNullOrEmpty(inputFile))
            {
                Error = "Input file path not specified.";
                return false;
            }


            if (!File.Exists(inputFile))
            {
                Error = "Inavlid input file.";
                return false;
            }

            if (new FileInfo(inputFile).Length == 0)
            {
                Error = "Input file is empty.";
                return false;
            }

            if (string.IsNullOrEmpty(outputFile))
            {
                Error = "Output file path not specified.";
                return false;
            }

            return true;
        }

        public bool SortFile(string inputFile, string outputFile, long chunkSize)
        {
            Error = string.Empty;
            if (!ValidateFiles(inputFile, outputFile))
                return false;
            var chunkFilePaths = SplitFile(inputFile, chunkSize);
            if (!string.IsNullOrEmpty(Error))
            {
                return false;
            }
            return KMerge(chunkFilePaths, outputFile);
        }

    }
}
