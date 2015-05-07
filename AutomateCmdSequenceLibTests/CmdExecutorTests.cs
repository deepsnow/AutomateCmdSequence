using System;
using System.IO;
using NUnit.Framework;
using AutomateCmdSequenceLib;
using Moq;
using ADG.DependencyInjection;

namespace AutomateCmdSequenceLibTests
{
    [TestFixture]
    public class CmdExecutorTests
    {
        private Mock<IProcessWrapper> processWrapper;
        private string logDirectory = @"C:\tmp\logs";
        private string rootSourceDirectory = @"C:\svn\code";

        [SetUp]
        public void Init()
        {
            processWrapper = new Mock<IProcessWrapper>();
            ServiceContainer.RegisterInstance(processWrapper.Object);
        }

        [Test]
        public void ExecuteCommand_ValidInput_ExecutionInvoked()
        {
            var executor = new CmdExecutor(logDirectory, rootSourceDirectory);
            string outputFileName = "doug_output.txt";
            
            processWrapper.Setup(
                e => e.StartCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            string outputFileFullPath = Path.Combine(logDirectory, outputFileName);

            processWrapper.Setup(e => e.WaitForExit());

            executor.Execute("doug.bat", "red blue", @"C:\tmp\doug", "doug_output.txt");

            processWrapper.VerifyAll();
        }
    }
}
