using System.IO;
using AutomateCmdSequenceLib;
using NUnit.Framework;

namespace AutomateCmdSequenceLibTests
{
    [TestFixture]
    public class FileSystemOpsTests
    {
        [Test]
        public void ConvertDateTimeToValidFolderName_ValidInput_FolderNameIsValid()
        {
            string folderName = FileSystemOps.GetFolderNameFromCurrentDateTime();
            foreach (var invalidChar in Path.GetInvalidPathChars())
            {
                Assert.True(-1 == folderName.IndexOf(invalidChar));
            }
        }
    }
}
