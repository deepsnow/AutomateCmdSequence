using System.Collections.Generic;
using ADG.DependencyInjection;
using AutomateCmdSequenceLib;
using Moq;
using NUnit.Framework;

namespace AutomateCmdSequenceLibTests
{
    [TestFixture]
    public class CmdSequenceExecutorTests
    {
        private CmdSequence cmdSeq;
        private Mock<IProcessWrapper> mockProcessWrapper;
        private Mock<IConsoleWrapper> mockConsoleWrapper;
        private Mock<IEnvironmentWrapper> mockEnvWrapper;

        [SetUp]
        public void Init()
        {
            CreateCommandSequence();

            mockProcessWrapper = new Mock<IProcessWrapper>();
            ServiceContainer.RegisterInstance(mockProcessWrapper.Object);

            mockConsoleWrapper = new Mock<IConsoleWrapper>();
            ServiceContainer.RegisterInstance(mockConsoleWrapper.Object);

            mockEnvWrapper = new Mock<IEnvironmentWrapper>();
            ServiceContainer.RegisterInstance(mockEnvWrapper.Object);
        }

        private void CreateCommandSequence()
        {
            cmdSeq = new CmdSequence();
            cmdSeq.LogDirectory = @"C:\tmp\logs";
            cmdSeq.RootSourceDirectory = @"C:\svn";
            cmdSeq.Sequence = new List<Command>();

            var cmd1 = new Command();
            cmd1.WorkingDirectory = "business-services-common";
            cmd1.OutputFileName = "BSC_mvn_clean.log";
            cmd1.CmdStr = "mvn";
            cmd1.CmdArgs = "clean";

            var cmd2 = new Command();
            cmd2.WorkingDirectory = "ediscovery-services";
            cmd2.OutputFileName = "ES_mvn_clean.log";
            cmd2.CmdStr = "mvn";
            cmd2.CmdArgs = "clean";

            cmdSeq.Sequence.Add(cmd1);
            cmdSeq.Sequence.Add(cmd2);

            cmdSeq.EnvPathStrings = new List<EnvPathStr>();
            var epc1 = new EnvPathStr() {Path = @"E:\Insight50\AppDatabase\InstallScripts"};
            cmdSeq.EnvPathStrings.Add(epc1);
        }

        [Test]
        public void ExecuteSequence_ValidInput_AllCmdsInvokedCapturedAndWaited()
        {         
            CmdSequenceExecutor.ExecuteSequence(cmdSeq);

            mockProcessWrapper.Verify(p => p.StartCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockProcessWrapper.Verify(p => p.WaitForExit(), Times.Exactly(2));
        }

        [Test]
        public void ExecuteSequence_ValidInput_AllCmdsInvokedAndReportedToConsole()
        {
            CmdSequenceExecutor.ExecuteSequence(cmdSeq);

            mockConsoleWrapper.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void ExecuteSequence_FirstCmdFails_SecondCommandNotExecuted()
        {
            mockProcessWrapper.Setup(p => p.WaitForExit()).Returns(1);

            CmdSequenceExecutor.ExecuteSequence(cmdSeq);

            mockProcessWrapper.Verify(p => p.StartCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        }

        [Test]
        public void ExecuteSequence_FirstCmdFails_FirstCmdAndItsErrorCodeReportedToConsole()
        {
            mockProcessWrapper.Setup(p => p.WaitForExit()).Returns(1);

            CmdSequenceExecutor.ExecuteSequence(cmdSeq);

            mockConsoleWrapper.Verify(c => c.WriteLine("mvn exited with error code 1"));
        }

        [Test]
        public void ExecuteSequence_ValidInput_EnvPathConcatIsInvoked()
        {
            CmdSequenceExecutor.ExecuteSequence(cmdSeq);

            mockEnvWrapper.Verify(e => e.GetEnvironmentVar("PATH"));
            mockEnvWrapper.Verify(e => e.SetEnvironmentVar("PATH", It.IsAny<string>()));
        }

        //var mockThreadWrapper = new Mock<IThreadWrapper>();
    }
}
