using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace lecture_3
{
    static class GlobalFunctions
    {
        static StreamWriter swWriteLogs = new StreamWriter("logs.txt", true, Encoding.UTF8);

        static GlobalFunctions()//this is the constructor of the static class. it will be called when the first time this class is referenced in the application during runtime
        {
            swWriteLogs.AutoFlush = true;
        }

        public static void logThread(string srCallerName)
        {
            var vrThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            swWriteLogs.WriteLine(DateTime.Now + "\t\t" + vrThreadId + "\t\t calling method: " + srCallerName);
        }
    }
}
