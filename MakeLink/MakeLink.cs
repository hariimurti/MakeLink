using System;
using System.Diagnostics;
using System.IO;

namespace MakeLink
{
    class MakeLink
    {
        public static bool Create(string option, string output, string source)
        {
            try
            {
                ProcessStartInfo prog = new ProcessStartInfo();
                prog.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
                prog.Arguments = "/c mklink " + ((option != null) ? $"{option} " : "") + $"\"{output}\" \"{source}\"";
                prog.WindowStyle = ProcessWindowStyle.Hidden;
                Process exec = Process.Start(prog);
                exec.WaitForExit();
                exec.Dispose();

                return (File.Exists(output) || Directory.Exists(output));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}
