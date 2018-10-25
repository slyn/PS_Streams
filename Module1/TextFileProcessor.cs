using System.IO;
using System.IO.Abstractions;
//using static System.Console;

namespace DataProcessor
{
    public class TextFileProcessor
    {
        private readonly IFileSystem _fileSystem;
        public string InputFilePath { get; }
        public string OutputFilePath { get; }
        // prod kullanır
        public TextFileProcessor(string inputFilePath, string outputFilePath):this(inputFilePath,outputFilePath,new FileSystem())
        {
        }

        // test kullanır
        public TextFileProcessor(string inputFilePath, string outputFilePath,IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public void Process()
        {
            //// Using read all text

            ////string originalText = File.ReadAllText(InputFilePath);
            ////string processedText = originalText.ToUpperInvariant();

            ////File.WriteAllText(OutputFilePath,processedText);

            //// Using read all lines
            //string[] lines = File.ReadAllLines(InputFilePath);
            //lines[1] = lines[1].ToUpperInvariant(); // Assumes there is a line 2

            //File.WriteAllLines(OutputFilePath,lines);

            // // V1
            //using (var inputFileStream = new FileStream(InputFilePath, FileMode.Open))
            //using (var inputStreamReader = new StreamReader(inputFileStream))
            //using (var outputFileStream = new FileStream(OutputFilePath, FileMode.Create))
            //using (var outputStreamWriter = new StreamWriter(outputFileStream))
            //{
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        string line = inputStreamReader.ReadLine();
            //        string processedLine = line.ToUpperInvariant();

            //        outputStreamWriter.WriteLine(processedLine);

            //    }
            //}

            // // V2
            //using (var inputStreamReader = File.OpenText(InputFilePath))
            //using (var outputFileStream = new FileStream(OutputFilePath, FileMode.Create))
            //using (var outputStreamWriter = new StreamWriter(outputFileStream))
            //{
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        string line = inputStreamReader.ReadLine();
            //        string processedLine = line.ToUpperInvariant();

            //        outputStreamWriter.WriteLine(processedLine);

            //    }
            //}

            // // V3
            //using (var inputStreamReader = new StreamReader(InputFilePath))
            //using (var outputStreamWriter = new StreamWriter(OutputFilePath))
            //{
            //    //while (!inputStreamReader.EndOfStream)
            //    //{
            //    //    string line = inputStreamReader.ReadLine();
            //    //    string processedLine = line.ToUpperInvariant();

            //    //    //outputStreamWriter.WriteLine(processedLine);
            //    //    bool isLastLine = inputStreamReader.EndOfStream;

            //    //    if (isLastLine)
            //    //    {
            //    //        outputStreamWriter.Write(processedLine);
            //    //    }
            //    //    else
            //    //    {
            //    //        outputStreamWriter.WriteLine(processedLine);
            //    //    }
            //    //}

            //    var currentLineNumber = 1;
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        string line = inputStreamReader.ReadLine();
            //        if (currentLineNumber == 2)
            //        {
            //            Write(line.ToUpperInvariant());
            //        }
            //        else
            //        {
            //            Write(line);
            //        }

            //        currentLineNumber++;

            //        void Write(string content)
            //        {
            //            bool isLastLine = inputStreamReader.EndOfStream;

            //            if (isLastLine)
            //            {
            //                outputStreamWriter.Write(content);
            //            }
            //            else
            //            {
            //                outputStreamWriter.WriteLine(content);
            //            }
            //        }
            //    }
            //}

            // v4 with test
            using (var inputStreamReader = _fileSystem.File.OpenText(InputFilePath))
            using (var outputStreamWriter = _fileSystem.File.CreateText(OutputFilePath))
            {
              
                var currentLineNumber = 1;
                while (!inputStreamReader.EndOfStream)
                {
                    string line = inputStreamReader.ReadLine();
                    if (currentLineNumber == 2)
                    {
                        Write(line.ToUpperInvariant());
                    }
                    else
                    {
                        Write(line);
                    }

                    currentLineNumber++;

                    void Write(string content)
                    {
                        bool isLastLine = inputStreamReader.EndOfStream;

                        if (isLastLine)
                        {
                            outputStreamWriter.Write(content);
                        }
                        else
                        {
                            outputStreamWriter.WriteLine(content);
                        }
                    }
                }
            }
        }
    }
}
