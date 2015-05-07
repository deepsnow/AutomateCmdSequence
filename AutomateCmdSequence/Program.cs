using System;
using System.IO;
using ADG.DependencyInjection;
using AutomateCmdSequenceLib;

namespace AutomateCmdSequence
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("usage: AutomatedCmdSeqence.exe [path_to_xml_file]");
            }

            RegisterDependencies();

            string xmlFileContent = string.Empty;

            using (StreamReader sr = new StreamReader(args[0]))
            {
                xmlFileContent = sr.ReadToEnd();
            }

            var cmdSeq = CmdSequenceOps.Deserialize(xmlFileContent);

            cmdSeq.LogDirectory = CreateUniqueSubFolderAndReturnFullPath(cmdSeq.LogDirectory);

            CmdSequenceExecutor.ExecuteSequence(cmdSeq);
        }

        private static string CreateUniqueSubFolderAndReturnFullPath(string baseDirName)
        {
            string fullPathWithNewSubFolder = Path.Combine(baseDirName, FileSystemOps.GetFolderNameFromCurrentDateTime());
            Directory.CreateDirectory(fullPathWithNewSubFolder);
            return fullPathWithNewSubFolder;
        }

        private static void RegisterDependencies()
        {
            ServiceContainer.RegisterType<IProcessWrapper, ProcessWrapper>();
            ServiceContainer.RegisterType<IConsoleWrapper, ConsoleWrapper>();
            ServiceContainer.RegisterType<IEnvironmentWrapper, EnvironmentWrapper>();
        }
    }
}
