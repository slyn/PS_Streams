using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Caching;
using System.Threading;
using static System.Console;

namespace DataProcessor
{
    class Program
    {

        //private static ConcurrentDictionary<string,string> FilesToProcess = new ConcurrentDictionary<string, string>();
        private static MemoryCache FilesToProcess = MemoryCache.Default;
        static void Main(string[] args)
        {
            WriteLine("Parsing command line options");

            var directoryToWatch = args[0];

            if (!Directory.Exists(directoryToWatch))
            {
                WriteLine($"ERROR: {directoryToWatch} does not exist");
            }
            else
            {
                WriteLine($"Watching directory {directoryToWatch} for changes");

                ProcessExistingFiles(directoryToWatch);

                using (var inputFileWatcher = new FileSystemWatcher(directoryToWatch))
                //using(var timer = new Timer(ProcessFiles,null,0,1000)) // concurrent dict.
                {
                    inputFileWatcher.IncludeSubdirectories = false;

                    inputFileWatcher.InternalBufferSize = 32768; // 32 KB
                    inputFileWatcher.Filter = "*.*";
                    inputFileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

                    inputFileWatcher.Created += FileCreated;
                    inputFileWatcher.Changed += FileChanged;
                    inputFileWatcher.Deleted += FileDeleted;
                    inputFileWatcher.Renamed += FileRenamed;
                    inputFileWatcher.Error += WatcherError;

                    inputFileWatcher.EnableRaisingEvents = true;


                    ReadLine();
                }

            }
            //var command = args[0];

            //if (command == "--file")
            //{
            //    var filePath = args[1];
            //    WriteLine($"Single file {filePath} selected");
            //    ProcessSingleFile(filePath);
            //}
            //else if (command == "--dir")
            //{
            //    var directoryPath = args[1];
            //    var fileType = args[2];
            //    WriteLine($"Directory {directoryPath} selected for {fileType} files");
            //    ProcessDirectory(directoryPath,fileType);
            //}
            //else
            //{
            //    WriteLine("Invalid command line options");
            //}

            ReadLine();
        }

        private static void ProcessSingleFile(string filePath)
        {
            var fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }

        private static void ProcessDirectory(string directoryPath, string fileType)
        {
            //var allFiles = Directory.GetFiles(directoryPath); // to get all files
            switch (fileType)
            {
                case "TEXT":
                    string[] textFiles = Directory.GetFiles(directoryPath, "*.txt");
                    foreach (var textFilePath in textFiles)
                    {
                        var fileProcessor = new FileProcessor(textFilePath);
                        fileProcessor.Process();
                    }

                    break;
                default:
                    WriteLine($"ERROR: {fileType} is not supported");
                    return;
            }
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");

            //var fileProcessor = new FileProcessor(e.FullPath);
            //fileProcessor.Process();

            //FilesToProcess.TryAdd(e.FullPath, e.FullPath); // concurrent dict.
            AddToCache(e.FullPath);
        }
        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File changed: {e.Name} - type: {e.ChangeType}");

            //var fileProcessor = new FileProcessor(e.FullPath);
            //fileProcessor.Process();

            //FilesToProcess.TryAdd(e.FullPath, e.FullPath); // concurrent dict.
            AddToCache(e.FullPath);
        }
        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");
        }
        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            WriteLine($"* File renamed: {e.OldName} to {e.Name} - type: {e.ChangeType}");
        }

        private static void WatcherError(object sender, ErrorEventArgs e)
        {
            WriteLine($"ERROR: file system watching may no longer be active :{e.GetException()}");
        }

        //private static void ProcessFiles(Object stateInfo)
        //{
        //    foreach (var fileName in FilesToProcess.Keys)
        //    {
        //        if (FilesToProcess.TryRemove(fileName, out _))
        //        {
        //            var fileProcessor = new FileProcessor(fileName);
        //            fileProcessor.Process();
        //        }
        //    }
        //}

        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath,fullPath);
            var policy = new CacheItemPolicy()
            {
                RemovedCallback = ProcessFile,
                SlidingExpiration = TimeSpan.FromSeconds(2)
            };

            FilesToProcess.Add(item, policy);
        }

        private static void ProcessFile(CacheEntryRemovedArguments args)
        { 
            WriteLine($"* Cache item removed: {args.CacheItem.Key} because {args.RemovedReason}");

            if (args.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var fileProcessor = new FileProcessor(args.CacheItem.Key);
                fileProcessor.Process();
            }
            else
            {
                WriteLine($"WARNING: {args.CacheItem.Key} was removed unexpectedly and may not ");
            }
        }

        private static void ProcessExistingFiles(string inputDirectory)
        {
            WriteLine($"Cheking {inputDirectory} for exisiting files");

            foreach (var filePath in Directory.EnumerateFiles(inputDirectory))
            {
                WriteLine($" - Found {filePath}");
                AddToCache(filePath);
            }
        }
    }
}
