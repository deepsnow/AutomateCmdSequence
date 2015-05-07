using System;
using System.Globalization;

namespace AutomateCmdSequenceLib
{
    public class FileSystemOps
    {
        public static string GetFolderNameFromCurrentDateTime()
        {
            return DateTime.Now.ToString("ddMMMyyyy-HH-mm-ss", CultureInfo.InvariantCulture);
        }
    }
}
