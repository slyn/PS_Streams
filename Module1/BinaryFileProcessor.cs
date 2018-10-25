using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class BinaryFileProcessor
    {
        private readonly IFileSystem _fileSystem;
        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public BinaryFileProcessor(string inputFilePath, string outputFilePath):this(inputFilePath,outputFilePath,new FileSystem())
        {
        }

        public BinaryFileProcessor(string inputFilePath, string outputFilePath,IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public void Process()
        {
            //byte[] data = File.ReadAllBytes(InputFilePath);

            //byte largest = data.Max();

            //byte[] newData = new byte[data.Length + 1];
            //Array.Copy(data,newData,data.Length);
            //newData[newData.Length - 1] = largest;

            //File.WriteAllBytes(OutputFilePath,newData);
            //// V1
            //using(FileStream input = File.Open(InputFilePath,FileMode.Open,FileAccess.Read))
            //using (FileStream output = File.Create(InputFilePath))
            //{
            //    const int endOfStream = -1;
            //    int largestByte = 0;

            //    int currentByte = input.ReadByte();

            //    while (currentByte != endOfStream)
            //    {
            //        output.WriteByte((byte)currentByte);
            //        if (currentByte > largestByte)
            //        {
            //            largestByte = currentByte;
            //        }

            //        currentByte = input.ReadByte();
            //    }

            //    output.WriteByte((byte)largestByte);
            //}

            //// V2
            //using (FileStream inputFileStream = File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            //using (BinaryReader binaryStreamReader = new BinaryReader(inputFileStream))
            //using (FileStream outputFileStream = File.Create(InputFilePath))
            //using (BinaryWriter binaryStreamWriter = new BinaryWriter(outputFileStream))
            //{
            //    byte largest = 0;

            //    while (binaryStreamReader.BaseStream.Position < binaryStreamReader.BaseStream.Length)
            //    {
            //        byte currentByte = binaryStreamReader.ReadByte();

            //        binaryStreamWriter.Write(currentByte);

            //        if (currentByte > largest)
            //        {
            //            largest = currentByte;
            //        }
            //    }
            //    binaryStreamWriter.Write(largest);
            //}
            // V2
            using (Stream inputFileStream = _fileSystem.File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryStreamReader = new BinaryReader(inputFileStream))
            using (Stream outputFileStream = _fileSystem.File.Create(InputFilePath))
            using (BinaryWriter binaryStreamWriter = new BinaryWriter(outputFileStream))
            {
                byte largest = 0;

                while (binaryStreamReader.BaseStream.Position < binaryStreamReader.BaseStream.Length)
                {
                    byte currentByte = binaryStreamReader.ReadByte();

                    binaryStreamWriter.Write(currentByte);

                    if (currentByte > largest)
                    {
                        largest = currentByte;
                    }
                }
                binaryStreamWriter.Write(largest);
            }
        }
    }
}
