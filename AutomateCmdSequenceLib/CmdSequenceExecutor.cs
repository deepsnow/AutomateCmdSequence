using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ADG.DependencyInjection;

namespace AutomateCmdSequenceLib
{
    public class CmdSequenceExecutor
    {
        public static void ExecuteSequence(CmdSequence cmdSeq)
        {
            var cmdExec = new CmdExecutor(cmdSeq.LogDirectory, cmdSeq.RootSourceDirectory);
            var console = ServiceLocator.Get<IConsoleWrapper>();

            ConfigureEnvironmentPath(cmdSeq.EnvPathStrings);

            foreach (var cmd in cmdSeq.Sequence)
            {
                int resultCode = ExecSingleCommandAndReportToConsole(cmdExec, cmd, console);
                if (resultCode != 0)
                {
                    console.WriteLine(string.Format("{0} exited with error code {1}", cmd.CmdStr, resultCode));
                    break;
                }
            }
        }

        private static int ExecSingleCommandAndReportToConsole(CmdExecutor cmdExec, Command cmd, IConsoleWrapper console)
        {
            console.WriteLine(string.Format("now running: {0} {1} in {2}", cmd.CmdStr, cmd.CmdArgs, cmd.WorkingDirectory));
            return cmdExec.Execute(cmd.CmdStr, cmd.CmdArgs, cmd.WorkingDirectory, cmd.OutputFileName);            
        }

        private static void ConfigureEnvironmentPath(List<EnvPathStr> epclist)
        {
            var env = ServiceLocator.Get<IEnvironmentWrapper>();
            string origPath = env.GetEnvironmentVar("PATH");
            string newPaths = string.Empty;

            foreach (var envPath in epclist)
            {
                newPaths = newPaths + ";" + envPath.Path;
            }

            env.SetEnvironmentVar("PATH", origPath + newPaths);
        }
    }

    public interface IConsoleWrapper
    {
        void WriteLine(string line);
    }

    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }

    //public interface IThreadWrapper
    //{
    //    void Start(object command);
    //    ThreadStart MyThreadMethod { get; set; }
    //}

    //public class ThreadWrapper : IThreadWrapper
    //{
    //    public ThreadStart MyThreadMethod { get; set; }

    //    public void Start(object command)
    //    {
    //        new Thread(MyThreadMethod).Start(command);
    //    }
    //}

    public interface IEnvironmentWrapper
    {
        void SetEnvironmentVar(string var, string value);
        string GetEnvironmentVar(string var);
    }

    public class EnvironmentWrapper : IEnvironmentWrapper
    {
        public string GetEnvironmentVar(string var)
        {
            return Environment.GetEnvironmentVariable(var);
        }

        public void SetEnvironmentVar(string var, string value)
        {
            Environment.SetEnvironmentVariable(var, value);
        }
    }
}
