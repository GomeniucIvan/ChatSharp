using System.Diagnostics;

namespace ChatSharp.Extensions
{
    public class ConsoleExtensions
    {
        [Conditional("DEBUG")]
        public static void ErrorWriteLine(Exception e)
        {
            Console.WriteLine("ERROR:" + e);
        }

        [Conditional("DEBUG")]
        public static void DebugWriteLine(Exception e)
        {
            Console.WriteLine("ERROR:" + e);
        }

        [Conditional("DEBUG")]
        public static void InfoWriteLine(string infoMessage)
        {
            Console.WriteLine("INFO:" + infoMessage);
        }

        [Conditional("DEBUG")]
        public static void DebugWarningWriteLine(string warningMessage)
        {
            Console.WriteLine("WARNING:" + warningMessage);
        }

        [Conditional("DEBUG")]
        public static void DebugWriteLine(string e)
        {
            Console.WriteLine(e);
        }
    }
}
