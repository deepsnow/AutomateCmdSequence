using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AutomateCmdSequenceLib
{
    [DataContract]
    public class CmdSequence
    {
        [DataMember]
        public string LogDirectory { get; set; }

        [DataMember]
        public string RootSourceDirectory { get; set; }

        [DataMember]
        public List<EnvVar> EnvVars { get; set; }

        [DataMember]
        public List<EnvPathStr> EnvPathStrings { get; set; }

        [DataMember]
        public List<Command> Sequence { get; set; }
    }

    [DataContract]
    public class Command
    {
        [DataMember]
        public string WorkingDirectory { get; set; }

        [DataMember]
        public string CmdStr { get; set; }

        [DataMember]
        public string CmdArgs { get; set; }

        [DataMember]
        public string OutputFileName { get; set; }
    }

    [DataContract]
    public class EnvVar
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class EnvPathStr
    {
        [DataMember]
        public string Path { get; set; }
    }
}
