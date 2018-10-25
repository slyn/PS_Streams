using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using DataProcessor;
using Xunit;

namespace DataProcessor.Test
{
    public class CsvFileProcessorShould
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void OutputProcessedOrderCsvData()
        {
            const string inputDir = @"c:\root\in";
            const string inputFileName = "myFile.csv";
            var inputFilePath = Path.Combine(inputDir, inputFileName);

            const string outputDir = @"c:\root\out";
            const string outputFileName = "myFile.csv";
            var outputFilePath = Path.Combine(outputDir, outputFileName);

            var csvLines = new StringBuilder();
            csvLines.AppendLine("OrderNumber,CustomerNumber,Description,Quantity");
            csvLines.AppendLine("42,100001,Shirt,II");
            csvLines.AppendLine("43,200002,Shorts,I");
            csvLines.AppendLine("@ This is a comment");
            csvLines.AppendLine("");
            csvLines.AppendLine("44, 300003,Cap,V");
            
            var mockInputFile = new MockFileData(csvLines.ToString());

            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(inputFilePath, mockInputFile);
            mockFileSystem.AddDirectory(outputDir);

            var sut = new CsvFileProcessor(inputFilePath,outputFilePath, mockFileSystem);
            sut.Process();

            Assert.True(mockFileSystem.FileExists(outputFilePath));

            var processedFile = mockFileSystem.GetFile(outputFilePath);

            //var lines = processedFile.TextContents.SplitLines();

            //Assert.Equal("OrderNumber,Customer,Amount", lines[0]);
            //Assert.Equal("42,100001,2", lines[1]);
            //Assert.Equal("43,200002,1", lines[2]);
            //Assert.Equal("44, 300003,5", lines[3]);

            Approvals.Verify(processedFile.TextContents);

        }
    }
}
