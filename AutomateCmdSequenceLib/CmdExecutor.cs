using System.IO;
using System.Diagnostics;
using ADG.DependencyInjection;

namespace AutomateCmdSequenceLib
{
    public interface IProcessWrapper
    {
        void StartCommand(string cmd, string args, string workingDirectory, string outputFilePath);
        int WaitForExit();
    }

    public class ProcessWrapper : IProcessWrapper
    {
        private Process process = new Process();
        private string _outputFilePath;

        public void StartCommand(string cmd, string args, string workingDirectory, string outputFilePath)
        {
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = args;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            _outputFilePath = outputFilePath;
            process.OutputDataReceived += HandleRedirectedOutputEvents;

            process.Start();
            process.BeginOutputReadLine();
        }

        public int WaitForExit()
        {
            process.WaitForExit();
            return process.ExitCode;
        }

        private void HandleRedirectedOutputEvents(object sender, DataReceivedEventArgs e)
        {
            string line = e.Data;
            if (line != null)
            {
                using (var sw = File.AppendText(_outputFilePath))
        {
                    sw.WriteLine(line);
                }
            }
        }
    }

    public class CmdExecutor
    {
        private string logDirectory;
        private string rootSourceDirectory;

        public CmdExecutor(string logDir, string rootSourceDir)
        {
            logDirectory = logDir;
            rootSourceDirectory = rootSourceDir;
        }

        public int Execute(string cmd, string args, string workingDirectory, string outputFileName)
        {
            var processWrapper = ServiceLocator.Get<IProcessWrapper>();
            processWrapper.StartCommand(cmd, args, Path.Combine(rootSourceDirectory, workingDirectory), Path.Combine(logDirectory, outputFileName));
            return processWrapper.WaitForExit();
        }

        // for threading support
        public int Execute(object command)
        {
            var cmd = (Command) command;
            return Execute(cmd.CmdStr, cmd.CmdArgs, cmd.WorkingDirectory, cmd.OutputFileName);
        }
    }
}
