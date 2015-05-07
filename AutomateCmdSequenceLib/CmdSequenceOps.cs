using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace AutomateCmdSequenceLib
{
    public class CmdSequenceOps
    {
        public static CmdSequence Deserialize(string xmlInput)
        {
            var serializer = new DataContractSerializer(typeof(CmdSequence));
            var reader = XmlReader.Create(new StringReader(xmlInput));

            return serializer.ReadObject(reader) as CmdSequence;
        }

        // This method exists primarily for testing and maintenance.
        public static string Serialize(CmdSequence cmdSeq)
        {
            var serializer = new DataContractSerializer(typeof(CmdSequence));
            var xmlstring = new StringBuilder();
            var writer = XmlWriter.Create(xmlstring);
            serializer.WriteObject(writer, cmdSeq);
            writer.Flush();
            return xmlstring.ToString();
        }
    }
}
