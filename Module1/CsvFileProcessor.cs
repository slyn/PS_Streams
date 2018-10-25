using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataProcessor
{
    public class CsvFileProcessor
    {
        private readonly IFileSystem _fileSystem;
        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public CsvFileProcessor(string inputFilePath, string outputFilePath):this(inputFilePath,outputFilePath,new FileSystem())
        {
        }
        public CsvFileProcessor(string inputFilePath, string outputFilePath,IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public void Process()
        {
            //using (StreamReader input = File.OpenText(InputFilePath))
            //using (CsvReader csvReader = new CsvReader(input))
            //{
            //    IEnumerable<dynamic> records = csvReader.GetRecords<dynamic>();

            //    csvReader.Configuration.TrimOptions = TrimOptions.Trim;
            //    csvReader.Configuration.Comment = '@'; // Default is '#'
            //    csvReader.Configuration.AllowComments = true;

            //    //csvReader.Configuration.IgnoreBlankLines = true; // default value : true,  ignore blank lines
            //    //csvReader.Configuration.Delimiter = ";"; // default ',' csv dosyasında ayraç karakter
            //    //csvReader.Configuration.HasHeaderRecord = false;  // default : true,  csv dosyasının başlık bilgisi yoksa okuma işleminde hata

            //    foreach (var record in records)
            //    {
            //        Console.WriteLine(record.OrderNumber);
            //        Console.WriteLine(record.CustomerNumber);
            //        Console.WriteLine(record.Description);
            //        Console.WriteLine(record.Quantity);

            //        //// başlık bilgisi yoksa kolon ismi 
            //        //Console.WriteLine(record.Field1);
            //        //Console.WriteLine(record.Field2);
            //        //Console.WriteLine(record.Field3);
            //        //Console.WriteLine(record.Field4);
            //    }
            //}

            //// csv den okunan veri modele yüklenir!
            //using (StreamReader input = File.OpenText(InputFilePath))
            //using (CsvReader csvReader = new CsvReader(input))
            //{
            //    IEnumerable<Order> records = csvReader.GetRecords<Order>();

            //    csvReader.Configuration.TrimOptions = TrimOptions.Trim;
            //    csvReader.Configuration.Comment = '@'; // Default is '#'
            //    csvReader.Configuration.AllowComments = true;


            //    foreach (Order record in records)
            //    {
            //        Console.WriteLine(record.OrderNumber);
            //        Console.WriteLine(record.CustomerNumber);
            //        Console.WriteLine(record.Description);
            //        Console.WriteLine(record.Quantity);
            //    }
            //}

            //// csv den okunan veri farklı modele yükle
            //using (StreamReader input = File.OpenText(InputFilePath))
            //using (CsvReader csvReader = new CsvReader(input))
            //{
            //    IEnumerable<ProcessedOrder> records = csvReader.GetRecords<ProcessedOrder>();

            //    csvReader.Configuration.TrimOptions = TrimOptions.Trim;
            //    csvReader.Configuration.Comment = '@'; // Default is '#'
            //    csvReader.Configuration.AllowComments = true;

            //    csvReader.Configuration.RegisterClassMap<ProcessedOrderMap>();

            //    foreach (ProcessedOrder record in records)
            //    {
            //        Console.WriteLine(record.OrderNumber);
            //        Console.WriteLine(record.Customer);
            //        Console.WriteLine(record.Amount);
            //    }
            //}

            //// csv dosyasına yaz
            //using (StreamReader input = File.OpenText(InputFilePath))
            //using (CsvReader csvReader = new CsvReader(input))
            //using (StreamWriter output = File.CreateText(OutputFilePath))
            //using (CsvWriter csvWriter = new CsvWriter(output))
            //{

            //    csvReader.Configuration.TrimOptions = TrimOptions.Trim;
            //    csvReader.Configuration.Comment = '@'; // Default is '#'
            //    csvReader.Configuration.AllowComments = true;

            //    csvReader.Configuration.RegisterClassMap<ProcessedOrderMap>();

            //    IEnumerable<ProcessedOrder> records = csvReader.GetRecords<ProcessedOrder>();

            //    //csvWriter.WriteRecords(records); // yeni satır ekleyip bırakır

            //    csvWriter.WriteHeader<ProcessedOrder>();
            //    csvWriter.NextRecord();

            //    var recordsArray = records.ToArray();
            //    for (int i = 0; i < recordsArray.Length; i++)
            //    {
            //        csvWriter.WriteField(recordsArray[i].OrderNumber);
            //        csvWriter.WriteField(recordsArray[i].Customer);
            //        csvWriter.WriteField(recordsArray[i].Amount);

            //        bool isLastRecord = i == recordsArray.Length - 1;

            //        if (!isLastRecord)
            //        {
            //            csvWriter.NextRecord();

            //        }
            //    }

            //}
            // csv dosyasına yaz
            using (StreamReader input = _fileSystem.File.OpenText(InputFilePath))
            using (CsvReader csvReader = new CsvReader(input))
            using (StreamWriter output = _fileSystem.File.CreateText(OutputFilePath))
            using (CsvWriter csvWriter = new CsvWriter(output))
            {

                csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                csvReader.Configuration.Comment = '@'; // Default is '#'
                csvReader.Configuration.AllowComments = true;

                csvReader.Configuration.RegisterClassMap<ProcessedOrderMap>();

                IEnumerable<ProcessedOrder> records = csvReader.GetRecords<ProcessedOrder>();

                //csvWriter.WriteRecords(records); // yeni satır ekleyip bırakır

                csvWriter.WriteHeader<ProcessedOrder>();
                csvWriter.NextRecord();

                var recordsArray = records.ToArray();
                for (int i = 0; i < recordsArray.Length; i++)
                {
                    csvWriter.WriteField(recordsArray[i].OrderNumber);
                    csvWriter.WriteField(recordsArray[i].Customer);
                    csvWriter.WriteField(recordsArray[i].Amount);

                    bool isLastRecord = i == recordsArray.Length - 1;

                    if (!isLastRecord)
                    {
                        csvWriter.NextRecord();

                    }
                }

            }
        }
    }
}
