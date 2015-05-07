using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using AutomateCmdSequenceLib;

namespace AutomateCmdSequenceLibTests
{
    [TestFixture]
    public class CmdSequenceOpsTests
    {

        private string xmlStringRepresentation = @"
<?xml version=""1.0"" encoding=""utf-16""?>
<CmdSequence xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/AutomateCmdSequenceLib"">
    <EnvPathStrings>
        <EnvPathStr>
            <Path>E:\Insight50\AppDatabase\InstallScripts</Path>
        </EnvPathStr>
    </EnvPathStrings>
    <EnvVars>
        <EnvVar>
            <Name>M2_HOME</Name>
            <Value>E:\apache-maven-3.0.3</Value>
        </EnvVar>
    </EnvVars>
	<LogDirectory>C:\tmp\logs</LogDirectory>
	<RootSourceDirectory>E:\Insight50</RootSourceDirectory>
	<Sequence>
		<Command>
			<CmdArgs>red blue</CmdArgs>
			<CmdStr>doug.bat</CmdStr>
			<OutputFileName>doug_output.txt</OutputFileName>
			<WorkingDirectory>C:\tmp\exec_dir</WorkingDirectory>
		</Command>
	</Sequence>
</CmdSequence>
        ";

        [Test]
        public void Deserialize_SimpleInput_DeserializationSucceeds()
        {
            var cmdSeqDeser = CmdSequenceOps.Deserialize(xmlStringRepresentation.TrimStart());
            var cmdSeqConstr = CreateCmdSequenceFromScratch();

            Assert.AreEqual(cmdSeqDeser.LogDirectory, cmdSeqConstr.LogDirectory);
            Assert.AreEqual(cmdSeqDeser.RootSourceDirectory, cmdSeqConstr.RootSourceDirectory);

            Assert.True(cmdSeqDeser.Sequence.Count == 1);
            Assert.True(cmdSeqConstr.Sequence.Count == 1);
            Assert.True(DataContractObjectsAreEqual(cmdSeqDeser.Sequence[0], cmdSeqConstr.Sequence[0]));

            Assert.True(cmdSeqDeser.EnvVars.Count == 1);
            Assert.True(cmdSeqConstr.EnvVars.Count == 1);
            Assert.True(DataContractObjectsAreEqual(cmdSeqDeser.EnvVars[0], cmdSeqConstr.EnvVars[0]));

            Assert.True(cmdSeqDeser.EnvPathStrings.Count == 1);
            Assert.True(cmdSeqConstr.EnvPathStrings.Count == 1);
            Assert.True(DataContractObjectsAreEqual(cmdSeqDeser.EnvPathStrings[0], cmdSeqConstr.EnvPathStrings[0]));
        }

        private bool DataContractObjectsAreEqual<T>(T t1, T t2)
        {
            var propertyList =
                typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var propInfo in propertyList)
            {
                if (!propInfo.GetValue(t1).Equals(propInfo.GetValue(t2)))
                    return false;
            }
            return true;
        }

        [Test]
        public void Serialize_SimpleInput_SerializationSucceeds()
        {
            var cmdSeq = CreateCmdSequenceFromScratch();
            var xmlOutput = CmdSequenceOps.Serialize(cmdSeq);

            var xmlOutputClean = Regex.Replace(xmlOutput, @"\s+", "");
            var xmlStrRepClean = Regex.Replace(xmlStringRepresentation, @"\s+", "");

            Assert.AreEqual(xmlOutputClean, xmlStrRepClean);
        }

        private CmdSequence CreateCmdSequenceFromScratch()
        {
            var cmdSeq = new CmdSequence();
            cmdSeq.LogDirectory = @"C:\tmp\logs";
            cmdSeq.RootSourceDirectory = @"E:\Insight50";
            var cmd = new Command();
            cmd.CmdStr = "doug.bat";
            cmd.CmdArgs = "red blue";
            cmd.WorkingDirectory = @"C:\tmp\exec_dir";
            cmd.OutputFileName = "doug_output.txt";
            cmdSeq.Sequence = new List<Command>() { cmd };
            var envVar = new EnvVar() {Name = "M2_HOME", Value = @"E:\apache-maven-3.0.3"};
            cmdSeq.EnvVars = new List<EnvVar>() { envVar };
            var envPath = new EnvPathStr() {Path = @"E:\Insight50\AppDatabase\InstallScripts"};
            cmdSeq.EnvPathStrings = new List<EnvPathStr>() { envPath };

            return cmdSeq;
        }
    }
}
